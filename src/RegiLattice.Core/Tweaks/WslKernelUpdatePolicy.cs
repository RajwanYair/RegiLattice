// RegiLattice.Core — Tweaks/WslKernelUpdatePolicy.cs
// WSL Linux kernel update channel, signing enforcement, and patching cadence Group Policy controls (Sprint 611).
// Category: "WSL Kernel Update Policy" | Slug: wslkupd
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\Lxss\Updates

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WslKernelUpdatePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss\Updates";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "wslkupd-pin-kernel-update-channel-stable",
            Label = "WSL Kernel Update: Pin WSL Kernel Updates to Stable Channel",
            Category = "WSL Kernel Update Policy",
            Description = "Sets KernelUpdateChannel=0 in Lxss Updates policy. Locks the WSL kernel update distribution channel to the 'Stable' channel (0 = stable release), preventing the system from automatically switching to preview or developer channel kernel builds via Windows Update. " +
                "Preview and developer channel WSL kernel builds may contain experimental features or recently introduced security changes that have not undergone the full Windows Update quality validation cycle. In enterprise environments, pinning to the stable channel ensures that WSL kernel updates receive the same update quality bar as production Windows kernel updates, reducing the risk of a kernel regression breaking production developer workflows.",
            Tags = ["wsl", "kernel", "update-channel", "stable", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "WSL kernel updates restricted to Stable channel; no preview kernel builds deployed via Windows Update.",
            ApplyOps = [RegOp.SetDword(Key, "KernelUpdateChannel", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "KernelUpdateChannel")],
            DetectOps = [RegOp.CheckDword(Key, "KernelUpdateChannel", 0)],
        },
        new TweakDef
        {
            Id = "wslkupd-enforce-kernel-signature-verification",
            Label = "WSL Kernel Update: Require Digital Signature Verification on Kernel Updates",
            Category = "WSL Kernel Update Policy",
            Description = "Sets EnforceKernelSignatureVerification=1 in Lxss Updates policy. Requires that every WSL kernel update package delivered via Microsoft Update is verified against Microsoft's Authenticode certificate chain before being applied, blocking tampered or unsigned kernel update packages. " +
                "Microsoft Update delivery of WSL kernel updates uses HTTPS transport, but a compromised Windows Update cache, WSUS proxy, or a threat actor with WSUS-level man-in-the-proxy access could substitute a malicious kernel package. Requiring explicit Authenticode signature verification ensures that even a package delivered through a compromised update pipeline is rejected if it is not signed by Microsoft's production signing keys.",
            Tags = ["wsl", "kernel", "signature", "update", "supply-chain"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "WSL kernel packages require valid Microsoft Authenticode signature; tampered packages rejected before kernel update.",
            ApplyOps = [RegOp.SetDword(Key, "EnforceKernelSignatureVerification", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceKernelSignatureVerification")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceKernelSignatureVerification", 1)],
        },
        new TweakDef
        {
            Id = "wslkupd-block-manual-kernel-downgrade",
            Label = "WSL Kernel Update: Block Manual WSL Kernel Version Downgrade",
            Category = "WSL Kernel Update Policy",
            Description = "Sets DisableKernelVersionDowngrade=1 in Lxss Updates policy. Prevents users from manually reverting the WSL Linux kernel to an older version via 'wsl --update --rollback' or by directly replacing the kernel package files. " +
                "A threat actor who knows of an unpatched kernel vulnerability in an older WSL kernel version may attempt to roll the kernel back to the vulnerable version after the enterprise has applied a security patch. Blocking downgrade ensures that once a security-relevant WSL kernel update has been applied, it cannot be reversed without administrative action, enforcing a one-way patch ratchet.",
            Tags = ["wsl", "kernel", "downgrade", "patch", "rollback"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "WSL kernel rollback via --rollback blocked; applied kernel security patches cannot be reversed by standard users.",
            ApplyOps = [RegOp.SetDword(Key, "DisableKernelVersionDowngrade", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableKernelVersionDowngrade")],
            DetectOps = [RegOp.CheckDword(Key, "DisableKernelVersionDowngrade", 1)],
        },
        new TweakDef
        {
            Id = "wslkupd-enable-urgent-kernel-security-updates",
            Label = "WSL Kernel Update: Enable Urgent Security Updates via Windows Update",
            Category = "WSL Kernel Update Policy",
            Description = "Sets AllowUrgentKernelSecurityUpdates=1 in Lxss Updates policy. Allows the Windows Update service to automatically apply WSL kernel updates that are classified as 'Critical' or 'Security Update' severity without waiting for the standard Patch Tuesday deployment cycle. " +
                "WSL kernel security vulnerabilities that are being actively exploited in the wild may be patched with emergency out-of-band updates. Without this policy, enterprises using slow deployment rings (e.g., broad ring with 14–30 day deferral) may leave systems vulnerable for weeks after Microsoft releases an emergency patch. Enabling urgent update delivery ensures critical WSL kernel fixes bypass deployment ring deferrals.",
            Tags = ["wsl", "kernel", "security-update", "critical", "patch"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Critical WSL kernel security updates applied immediately; bypasses deployment ring deferrals for emergency patches.",
            ApplyOps = [RegOp.SetDword(Key, "AllowUrgentKernelSecurityUpdates", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowUrgentKernelSecurityUpdates")],
            DetectOps = [RegOp.CheckDword(Key, "AllowUrgentKernelSecurityUpdates", 1)],
        },
        new TweakDef
        {
            Id = "wslkupd-set-kernel-update-defer-days-0",
            Label = "WSL Kernel Update: Disable WSL Kernel Update Deferral (Apply Immediately)",
            Category = "WSL Kernel Update Policy",
            Description = "Sets KernelUpdateDeferralDays=0 in Lxss Updates policy. Sets the WSL kernel update deferral period to zero days, ensuring that WSL kernel updates are applied as soon as they are delivered and approved in the Windows Update service, with no additional deferral delay. " +
                "Unlike the Windows NT kernel which is updated on the monthly Patch Tuesday cadence, WSL kernel updates are typically small, targeted patches. Deferring them unnecessarily extends the window during which a known-patched WSL kernel vulnerability remains present. Setting deferral to zero ensures the enterprise's WSL kernel vulnerability exposure window matches the Windows Update delivery latency, not an additional IT-imposed delay.",
            Tags = ["wsl", "kernel", "deferral", "patch", "timely"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "WSL kernel update deferral removed; updates applied as soon as Windows Update delivers them.",
            ApplyOps = [RegOp.SetDword(Key, "KernelUpdateDeferralDays", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "KernelUpdateDeferralDays")],
            DetectOps = [RegOp.CheckDword(Key, "KernelUpdateDeferralDays", 0)],
        },
        new TweakDef
        {
            Id = "wslkupd-enable-kernel-update-audit-log",
            Label = "WSL Kernel Update: Enable Security Event Log Entry on Kernel Update or Rollback",
            Category = "WSL Kernel Update Policy",
            Description = "Sets EnableKernelUpdateAuditLog=1 in Lxss Updates policy. Causes a Security event log entry to be written whenever the WSL Linux kernel is updated to a new version or rolled back to a previous version, recording the previous and new kernel version strings. " +
                "Without audit logging of WSL kernel version changes, there is no Security event record of when WSL kernel updates were applied (or not applied) on a device. This makes it impossible to determine whether a managed device was running a vulnerable WSL kernel during a specific incident timeframe. Kernel version change events enable compliance managers to demonstrate patch deployment timelines and detect unexplained rollbacks.",
            Tags = ["wsl", "kernel", "audit", "logging", "compliance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Security log entry written on WSL kernel version change; kernel update history auditable in SIEM.",
            ApplyOps = [RegOp.SetDword(Key, "EnableKernelUpdateAuditLog", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableKernelUpdateAuditLog")],
            DetectOps = [RegOp.CheckDword(Key, "EnableKernelUpdateAuditLog", 1)],
        },
        new TweakDef
        {
            Id = "wslkupd-block-user-manual-kernel-update",
            Label = "WSL Kernel Update: Block Manual 'wsl --update' by Standard Users",
            Category = "WSL Kernel Update Policy",
            Description = "Sets DisableUserManualKernelUpdate=1 in Lxss Updates policy. Removes the ability of standard (non-administrator) users to manually trigger WSL kernel updates via the 'wsl --update' command, restricting kernel update initiation to Windows Update and IT administrator action only. " +
                "While 'wsl --update' legitimately downloads the latest stable kernel, standard users initiating manual updates bypass the enterprise's staged Windows Update ring deployment schedule. If a specific kernel version is deferred in slow deployment rings due to a regression, users running 'wsl --update' would receive the update immediately, bypassing change management controls. Restricting update initiation to Windows Update enforces the enterprise deployment schedule.",
            Tags = ["wsl", "kernel", "manual-update", "deployment-ring", "change-management"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "wsl --update blocked for standard users; WSL kernel updates controlled by Windows Update and admin action only.",
            ApplyOps = [RegOp.SetDword(Key, "DisableUserManualKernelUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableUserManualKernelUpdate")],
            DetectOps = [RegOp.CheckDword(Key, "DisableUserManualKernelUpdate", 1)],
        },
        new TweakDef
        {
            Id = "wslkupd-enable-kernel-integrity-verification-on-boot",
            Label = "WSL Kernel Update: Verify WSL Kernel Image Integrity at VM Start",
            Category = "WSL Kernel Update Policy",
            Description = "Sets VerifyKernelIntegrityOnBoot=1 in Lxss Updates policy. Enables hash-based integrity verification of the WSL Linux kernel image file (vmlinux) against its stored signature each time a WSL session starts a new Hyper-V VM instance. " +
                "Without startup integrity verification, a threat actor with write access to the WSL kernel image path on the Windows file system can replace the kernel with a modified version that installs kernel-level hooks for all subsequent WSL sessions. Verifying the kernel image hash at each VM start ensures that any tampering with the stored kernel image is detected before the compromised kernel is loaded into the Hyper-V VM.",
            Tags = ["wsl", "kernel", "integrity", "hash", "vm-start"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "WSL kernel image hash verified at each VM start; tampered kernel image detected before being loaded.",
            ApplyOps = [RegOp.SetDword(Key, "VerifyKernelIntegrityOnBoot", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "VerifyKernelIntegrityOnBoot")],
            DetectOps = [RegOp.CheckDword(Key, "VerifyKernelIntegrityOnBoot", 1)],
        },
        new TweakDef
        {
            Id = "wslkupd-block-preview-kernel-builds",
            Label = "WSL Kernel Update: Block Preview and Insider WSL Kernel Preview Builds",
            Category = "WSL Kernel Update Policy",
            Description = "Sets BlockPreviewKernelBuilds=1 in Lxss Updates policy. Prevents Windows Insider Preview and Windows Update Preview channels from delivering pre-release WSL Linux kernel builds to managed devices that are enrolled in preview rings for Windows OS preview builds. " +
                "Managed enterprise devices may be enrolled in Windows Insider rings for OS preview testing. However, preview WSL kernel builds may have known vulnerabilities, experimental security mitigations disabled, and debugging interfaces enabled that are inappropriate for production use. Blocking preview kernel builds ensures enterprise devices only receive production-quality WSL kernels regardless of their Windows Insider ring membership.",
            Tags = ["wsl", "kernel", "preview", "insider", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Preview WSL kernel builds blocked even for Insider-enrolled devices; production kernels only.",
            ApplyOps = [RegOp.SetDword(Key, "BlockPreviewKernelBuilds", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockPreviewKernelBuilds")],
            DetectOps = [RegOp.CheckDword(Key, "BlockPreviewKernelBuilds", 1)],
        },
        new TweakDef
        {
            Id = "wslkupd-require-restart-to-apply-kernel-update",
            Label = "WSL Kernel Update: Require Full WSL Session Termination to Activate Kernel Updates",
            Category = "WSL Kernel Update Policy",
            Description = "Sets RequireSessionRestartForKernelUpdate=1 in Lxss Updates policy. Requires that all running WSL distro sessions are fully terminated (wsl --shutdown) and a new Hyper-V VM is started before a delivered WSL kernel update becomes active. Prevents in-place kernel hot-patching that may skip the integrity verification startup checks. " +
                "Hot-patching-style kernel activation (patching a running VM's kernel in-memory) bypasses the startup integrity verification performed when a new VM is instantiated. By requiring a full session shutdown and VM restart after each kernel update, this policy ensures that the newly applied kernel image goes through the full chain-of-trust verification (signature check → AppArmor load → seccomp policy activation) before any distro sessions run on it.",
            Tags = ["wsl", "kernel", "restart", "hot-patch", "chain-of-trust"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "WSL kernel updates require session restart to activate; hot-patch bypasses chain-of-trust checks prevented.",
            ApplyOps = [RegOp.SetDword(Key, "RequireSessionRestartForKernelUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireSessionRestartForKernelUpdate")],
            DetectOps = [RegOp.CheckDword(Key, "RequireSessionRestartForKernelUpdate", 1)],
        },
    ];
}
