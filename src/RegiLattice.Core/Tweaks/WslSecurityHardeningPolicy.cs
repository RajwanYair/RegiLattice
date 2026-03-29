// RegiLattice.Core — Tweaks/WslSecurityHardeningPolicy.cs
// WSL 2 security isolation enforcement and hardening Group Policy controls (Sprint 610).
// Category: "WSL Security Hardening Policy" | Slug: wslsechrd
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\Lxss\Security

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WslSecurityHardeningPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss\Security";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "wslsechrd-disable-linux-privileged-container",
            Label = "WSL Security: Block Privileged Linux Containers (--privileged)",
            Category = "WSL Security Hardening Policy",
            Description = "Sets DisablePrivilegedContainerMode=1 in Lxss Security policy. Prevents users from launching Linux containers in privileged mode within WSL, which would grant the container full access to the WSL kernel's device tree and capabilities. " +
                "Privileged Linux containers bypass cgroup and namespace isolation — a privileged container escape is effectively a WSL hypervisor boundary bypass. Container image registries routinely contain malicious images that exploit the privileged mode to escape to the host. Blocking privileged container mode ensures all Docker and podman containers within WSL remain namespace-isolated.",
            Tags = ["wsl", "container", "privileged", "isolation", "escape"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Privileged Linux containers blocked in WSL; Docker --privileged flag will be denied.",
            ApplyOps = [RegOp.SetDword(Key, "DisablePrivilegedContainerMode", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePrivilegedContainerMode")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePrivilegedContainerMode", 1)],
        },
        new TweakDef
        {
            Id = "wslsechrd-require-secure-boot-for-wsl",
            Label = "WSL Security: Require Secure Boot (UEFI) Validation Before WSL Launch",
            Category = "WSL Security Hardening Policy",
            Description = "Sets RequireSecureBootForWsl=1 in Lxss Security policy. Configures WSL to verify that the host system has Secure Boot enabled and that the WSL system distro image is digitally signed before allowing any WSL distribution to launch. " +
                "A threat actor with physical access or bootkit privileges can replace the WSL system distro image (kernel/initramfs) with a malicious version that intercepts WSL sessions. Requiring Secure Boot validation means that only Microsoft-signed WSL kernel images will be accepted — any tampered or unsigned replacement will be rejected at launch, preventing persistent WSL-level rootkit installation.",
            Tags = ["wsl", "secure-boot", "kernel", "integrity", "bootkit"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "WSL requires Secure Boot; unsigned or tampered WSL kernel images rejected at launch.",
            ApplyOps = [RegOp.SetDword(Key, "RequireSecureBootForWsl", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireSecureBootForWsl")],
            DetectOps = [RegOp.CheckDword(Key, "RequireSecureBootForWsl", 1)],
        },
        new TweakDef
        {
            Id = "wslsechrd-enable-apparmor-enforcement",
            Label = "WSL Security: Enable AppArmor Mandatory Access Control Enforcement",
            Category = "WSL Security Hardening Policy",
            Description = "Sets RequireAppArmorEnforcement=1 in Lxss Security policy. Requires that AppArmor (the Linux Mandatory Access Control framework) is active and in enforcing mode within WSL distributions before those distros are permitted to run Linux processes. " +
                "AppArmor-enforcing mode means that every Linux process is subject to a per-executable MAC policy that limits the files, capabilities, and network resources it can access. Without AppArmor enforcement, a compromised Linux process within WSL can access any file the Linux user has permission to reach (including all DrvFs-mounted Windows files). AppArmor confines individual processes to their expected access patterns.",
            Tags = ["wsl", "apparmor", "mac", "mandatory-access-control", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "AppArmor MAC enforcement required; distros without AppArmor active will be blocked from launching processes.",
            ApplyOps = [RegOp.SetDword(Key, "RequireAppArmorEnforcement", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireAppArmorEnforcement")],
            DetectOps = [RegOp.CheckDword(Key, "RequireAppArmorEnforcement", 1)],
        },
        new TweakDef
        {
            Id = "wslsechrd-disable-wsl-sudo-escalation",
            Label = "WSL Security: Block sudo Privilege Escalation in WSL Distributions",
            Category = "WSL Security Hardening Policy",
            Description = "Sets DisableSudoEscalation=1 in Lxss Security policy. Prevents the 'sudo' and 'su' commands from granting root privileges within WSL distributions launched in standard user sessions, enforcing that all WSL processes run under the Linux user identity only. " +
                "sudo root access within WSL gives a Linux process full root capabilities within the WSL VM, including the ability to install kernel modules, bind to privileged ports, and reconfigure network namespaces. While the WSL VM boundary limits the blast radius, root access within WSL provides a much larger attack surface for container escape and VM privilege escalation research exploitation.",
            Tags = ["wsl", "sudo", "root", "privilege-escalation", "least-privilege"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "sudo/su blocked in WSL; all Linux processes run as the mapped Windows user identity only.",
            ApplyOps = [RegOp.SetDword(Key, "DisableSudoEscalation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSudoEscalation")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSudoEscalation", 1)],
        },
        new TweakDef
        {
            Id = "wslsechrd-enable-seccomp-enforcement",
            Label = "WSL Security: Enable Seccomp System Call Filtering Enforcement",
            Category = "WSL Security Hardening Policy",
            Description = "Sets RequireSeccompEnforcement=1 in Lxss Security policy. Requires the WSL Linux kernel to apply Seccomp-BPF (system call filtration) policies to all user-space processes, blocking access to dangerous system calls that are not needed for standard application workloads. " +
                "Many Linux kernel privilege escalation vulnerabilities are triggered via obscure or rarely-used system calls (ptrace, perf_event_open, io_uring). Seccomp filtering blocks these system calls unless explicitly allowed by the process's policy, preventing exploitation of kernel vulnerabilities that require those call paths. This is particularly important because the WSL Linux kernel shares the root namespace with all distros.",
            Tags = ["wsl", "seccomp", "syscall", "kernel", "exploit-prevention"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Seccomp syscall filtering enforced; dangerous kernel interfaces blocked from user-space, reducing kernel exploit surface.",
            ApplyOps = [RegOp.SetDword(Key, "RequireSeccompEnforcement", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireSeccompEnforcement")],
            DetectOps = [RegOp.CheckDword(Key, "RequireSeccompEnforcement", 1)],
        },
        new TweakDef
        {
            Id = "wslsechrd-disable-raw-socket-creation",
            Label = "WSL Security: Block Raw Socket Creation by Non-Root Linux Processes",
            Category = "WSL Security Hardening Policy",
            Description = "Sets DisableRawSocketCreation=1 in Lxss Security policy. Prevents non-root Linux processes within WSL from creating AF_PACKET (raw) network sockets, which would allow them to perform low-level network packet capture and injection without Windows-side network monitoring visibility. " +
                "Raw sockets enable Linux processes to capture all network traffic visible to the WSL VM's network namespace. In mirrored networking mode, this includes all traffic from the Windows host. A malicious tool running in WSL with raw socket access can perform ARP spoofing, DNS poisoning, and credential harvest from unencrypted protocol traffic, bypassing Windows network monitoring solutions.",
            Tags = ["wsl", "raw-socket", "packet-capture", "network", "monitoring"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Raw sockets blocked for non-root Linux processes; WSL cannot be used for network packet capture without root.",
            ApplyOps = [RegOp.SetDword(Key, "DisableRawSocketCreation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRawSocketCreation")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRawSocketCreation", 1)],
        },
        new TweakDef
        {
            Id = "wslsechrd-disable-ptrace-between-distros",
            Label = "WSL Security: Block ptrace Cross-Distro Process Attachment",
            Category = "WSL Security Hardening Policy",
            Description = "Sets DisableCrossDistrictPtrace=1 in Lxss Security policy. Prevents Linux processes in one WSL distribution from using ptrace() to attach to and debug processes running in another WSL distribution sharing the same Hyper-V VM. " +
                "When multiple WSL distros run in the same Hyper-V partition (the typical configuration), they share a Linux kernel and a process namespace at the VM level. Without a ptrace policy, a process in distro A can attach to a process in distro B and read its memory, modify its execution, or extract credentials it holds. Restricting cross-distro ptrace enforces process isolation between co-located Linux environments.",
            Tags = ["wsl", "ptrace", "debugging", "multi-distro", "isolation"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "ptrace blocked across WSL distros; one distro cannot debug or read memory of processes in another distro.",
            ApplyOps = [RegOp.SetDword(Key, "DisableCrossDistrictPtrace", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCrossDistrictPtrace")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCrossDistrictPtrace", 1)],
        },
        new TweakDef
        {
            Id = "wslsechrd-enable-user-namespace-restrictions",
            Label = "WSL Security: Restrict Unprivileged Linux User Namespace Creation",
            Category = "WSL Security Hardening Policy",
            Description = "Sets RestrictUnprivilegedUserNamespaces=1 in Lxss Security policy. Limits the ability of unprivileged Linux user-space processes to create new user namespaces within the WSL VM, which are frequently exploited as container escape stepping stones. " +
                "User namespaces are the Linux kernel primitive that enables unprivileged container tools (rootless Docker, rootless podman). However, user namespaces have also been the root cause or enabling boundary for a significant fraction of Linux kernel privilege escalation CVEs (CVE-2023-4911, CVE-2022-0847 'Dirty Pipe', CVE-2022-25375). Restricting their creation prevents their use as an escalation primitive while still allowing root-managed container workflows.",
            Tags = ["wsl", "user-namespace", "container", "cve", "kernel-exploit"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Unprivileged user namespace creation restricted; rootless container tools may not function; reduces kernel namespace CVE exposure.",
            ApplyOps = [RegOp.SetDword(Key, "RestrictUnprivilegedUserNamespaces", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictUnprivilegedUserNamespaces")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictUnprivilegedUserNamespaces", 1)],
        },
        new TweakDef
        {
            Id = "wslsechrd-enable-defender-scan-on-wsl-exec",
            Label = "WSL Security: Enable Microsoft Defender Scanning on WSL Executable Launch",
            Category = "WSL Security Hardening Policy",
            Description = "Sets EnableDefenderScanOnWslExecution=1 in Lxss Security policy. Enables Microsoft Defender for Endpoint to scan Linux ELF executables and scripts when they are launched within the WSL environment, before process execution begins. " +
                "By default, Defender's real-time file system protection monitors the Windows NTFS volume but may not scan Linux ELF binaries within the ext4 VHD. With WSL execution scanning enabled, Defender analyses Linux binaries using Linux threat intelligence signatures, detecting known Linux malware, coin miners, and reverse shells that reside within the WSL file system. This closes the gap between Windows AV coverage and Linux-resident threats.",
            Tags = ["wsl", "defender", "malware", "scanning", "elf"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Defender scans Linux ELF binaries at WSL launch time; Linux malware in the WSL VHD detected before execution.",
            ApplyOps = [RegOp.SetDword(Key, "EnableDefenderScanOnWslExecution", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableDefenderScanOnWslExecution")],
            DetectOps = [RegOp.CheckDword(Key, "EnableDefenderScanOnWslExecution", 1)],
        },
        new TweakDef
        {
            Id = "wslsechrd-disable-wsl-kernel-module-load",
            Label = "WSL Security: Block Loading of Unsigned Linux Kernel Modules in WSL",
            Category = "WSL Security Hardening Policy",
            Description = "Sets DisableUnsignedKernelModuleLoad=1 in Lxss Security policy. Prevents the WSL Linux kernel from loading unsigned or third-party kernel modules (LKMs) that are not part of the Microsoft-signed WSL kernel image. " +
                "Linux kernel modules run with ring-0 (kernel mode) privileges and have unrestricted access to all memory, devices, and kernel data structures within the VM. A malicious loadable kernel module loaded in WSL can install kernel-level hooks, intercept all WSL system calls, and exfiltrate data from other processes at the kernel level. Blocking unsigned module loading enforces that only the Microsoft-vetted WSL kernel components can extend the kernel's attack surface.",
            Tags = ["wsl", "kernel-module", "lkm", "ring0", "unsigned"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Unsigned Linux kernel modules blocked in WSL; custom LKMs and third-party drivers cannot be loaded into the WSL kernel.",
            ApplyOps = [RegOp.SetDword(Key, "DisableUnsignedKernelModuleLoad", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableUnsignedKernelModuleLoad")],
            DetectOps = [RegOp.CheckDword(Key, "DisableUnsignedKernelModuleLoad", 1)],
        },
    ];
}
