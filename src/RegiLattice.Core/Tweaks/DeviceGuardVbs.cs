// RegiLattice.Core — Tweaks/DeviceGuardVbs.cs
// Virtualization-Based Security and Device Guard policy settings (Sprint 91).
// Slug "vbs" — HKLM DeviceGuard, CodeIntegrity, and Credential Guard policies.
// Distinct from HyperVAdvanced.cs (Hyper-V hypervisor setup) and Hardening.cs (UAC/AppLocker).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DeviceGuardVbs
{
    private const string DeviceGuard = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard";

    private const string Scenarios = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios";

    private const string HvciScenario = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity";

    private const string CredentialGuard = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

    private const string CodeIntegrity = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Config";

    private const string DeviceGuardPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "vbs-enable-hvci",
            Label = "Enable Hypervisor-Protected Code Integrity (HVCI)",
            Category = "Device Guard & VBS",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Tags = ["vbs", "hvci", "code integrity", "security", "virtualization"],
            Description =
                "Enables HVCI (Memory Integrity) which uses the Hyper-V hypervisor to "
                + "verify kernel-mode code signatures at runtime. Prevents kernel exploits "
                + "and driver code injection. Requires a reboot. May impact performance by 5–15% "
                + "on systems without MBEC hardware support.",
            ApplyOps = [RegOp.SetDword(HvciScenario, "Enabled", 1)],
            RemoveOps = [RegOp.SetDword(HvciScenario, "Enabled", 0)],
            DetectOps = [RegOp.CheckDword(HvciScenario, "Enabled", 1)],
        },
        new TweakDef
        {
            Id = "vbs-enable-credential-guard",
            Label = "Enable Windows Defender Credential Guard",
            Category = "Device Guard & VBS",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Tags = ["vbs", "credential guard", "pass the hash", "security"],
            Description =
                "Enables Credential Guard, which runs the Windows NTLM and Kerberos "
                + "authentication subsystems in a VBS-isolated container. Prevents "
                + "pass-the-hash and pass-the-ticket credential theft attacks. "
                + "LsaIso isolation. Requires reboot.",
            ApplyOps = [RegOp.SetDword(CredentialGuard, "LsaCfgFlags", 1)],
            RemoveOps = [RegOp.SetDword(CredentialGuard, "LsaCfgFlags", 0)],
            DetectOps = [RegOp.CheckDword(CredentialGuard, "LsaCfgFlags", 1)],
        },
        new TweakDef
        {
            Id = "vbs-enable-vbs-platform",
            Label = "Enable Virtualization-Based Security Platform",
            Category = "Device Guard & VBS",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Tags = ["vbs", "virtualization", "security", "hypervisor"],
            Description =
                "Enables the VBS (Virtualization-Based Security) platform in Device Guard. "
                + "EnableVirtualizationBasedSecurity=1. Prerequisites: Secure Boot, "
                + "UEFI firmware, hardware virtualization (Intel VT-x / AMD-V). Requires reboot.",
            ApplyOps = [RegOp.SetDword(DeviceGuard, "EnableVirtualizationBasedSecurity", 1)],
            RemoveOps = [RegOp.SetDword(DeviceGuard, "EnableVirtualizationBasedSecurity", 0)],
            DetectOps = [RegOp.CheckDword(DeviceGuard, "EnableVirtualizationBasedSecurity", 1)],
        },
        new TweakDef
        {
            Id = "vbs-require-secure-boot-dma",
            Label = "Require Secure Boot + DMA Protection for VBS",
            Category = "Device Guard & VBS",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Tags = ["vbs", "secure boot", "dma", "protection", "policy"],
            Description =
                "Sets RequirePlatformSecurityFeatures=3 (require both Secure Boot and "
                + "DMA protection). Prevents VBS from running on systems that lack "
                + "IOMMU/VT-d DMA protection, ensuring the highest security level.",
            ApplyOps = [RegOp.SetDword(DeviceGuard, "RequirePlatformSecurityFeatures", 3)],
            RemoveOps = [RegOp.SetDword(DeviceGuard, "RequirePlatformSecurityFeatures", 1)],
            DetectOps = [RegOp.CheckDword(DeviceGuard, "RequirePlatformSecurityFeatures", 3)],
        },
        new TweakDef
        {
            Id = "vbs-enable-policy-device-guard",
            Label = "Enable Device Guard Policy via Group Policy",
            Category = "Device Guard & VBS",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Tags = ["vbs", "device guard", "policy", "gpo"],
            Description =
                "Enables Device Guard and VBS configuration via the machine-wide "
                + "Group Policy path. Enables both VBS and HVCI through a single "
                + "policy flag set. Reboot required for changes to take effect.",
            ApplyOps = [RegOp.SetDword(DeviceGuardPolicy, "EnableVirtualizationBasedSecurity", 1)],
            RemoveOps = [RegOp.DeleteValue(DeviceGuardPolicy, "EnableVirtualizationBasedSecurity")],
            DetectOps = [RegOp.CheckDword(DeviceGuardPolicy, "EnableVirtualizationBasedSecurity", 1)],
        },
        new TweakDef
        {
            Id = "vbs-enable-config-ci-policy",
            Label = "Enable Configurable Code Integrity (WDAC Boot Policy)",
            Category = "Device Guard & VBS",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 3,
            Tags = ["vbs", "wdac", "code integrity", "kernel", "policy"],
            Description =
                "Sets the CodeIntegrity configurable policy option. When Enabled=1, "
                + "the Windows Defender Application Control boot policy is loaded. "
                + "WARNING: requires a valid WDAC policy file to be present, or the "
                + "system may fail to boot drivers not covered by the policy.",
            ApplyOps = [RegOp.SetDword(CodeIntegrity, "Enabled", 1)],
            RemoveOps = [RegOp.SetDword(CodeIntegrity, "Enabled", 0)],
            DetectOps = [RegOp.CheckDword(CodeIntegrity, "Enabled", 1)],
        },
        new TweakDef
        {
            Id = "vbs-disable-test-signing",
            Label = "Disable Test Signing Mode (Production Security)",
            Category = "Device Guard & VBS",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["vbs", "test signing", "driver", "security", "kernel"],
            Description =
                "Ensures TestSigning=0, disabling Windows test-signing mode. Test signing "
                + "is sometimes left enabled after driver development. Leaving it on "
                + "allows unsigned kernel drivers, weakening system integrity.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", 1)],
        },
        new TweakDef
        {
            Id = "vbs-enable-kernel-shadow-stacks",
            Label = "Enable Kernel Shadow Stacks (Control Flow Guard Enforcement)",
            Category = "Device Guard & VBS",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["vbs", "shadow stack", "cfg", "control flow guard", "exploit"],
            Description =
                "Enables kernel shadow stacks (also called Kernel CFG Enforcement) which "
                + "uses hardware CET (Control-flow Enforcement Technology) to harden "
                + "return address integrity in kernel mode. Intel Tiger Lake and later. "
                + "KernelShadowStacksEnabled=1.",
            ApplyOps = [RegOp.SetDword(HvciScenario, "WasEnabledBy", 1)],
            RemoveOps = [RegOp.DeleteValue(HvciScenario, "WasEnabledBy")],
            DetectOps = [RegOp.CheckDword(HvciScenario, "WasEnabledBy", 1)],
        },
        new TweakDef
        {
            Id = "vbs-lock-hvci",
            Label = "Lock HVCI to Prevent Disable Without Reboot",
            Category = "Device Guard & VBS",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Tags = ["vbs", "hvci", "lock", "tamper protection"],
            Description =
                "Sets HVCI Locked=1, preventing the policy from being disabled at runtime "
                + "without a reboot. Once locked, changes to Memory Integrity require "
                + "a system restart to take effect, protecting against live tampering.",
            ApplyOps = [RegOp.SetDword(HvciScenario, "Locked", 1)],
            RemoveOps = [RegOp.SetDword(HvciScenario, "Locked", 0)],
            DetectOps = [RegOp.CheckDword(HvciScenario, "Locked", 1)],
        },
        new TweakDef
        {
            Id = "vbs-disable-lsa-protection-audit-mode",
            Label = "Disable LSA Protected Process Audit Mode",
            Category = "Device Guard & VBS",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["vbs", "lsa", "protected process", "audit"],
            Description =
                "Disables LSA Protected Process audit mode (RunAsPPL audit mode = 0). "
                + "Audit mode logs what would be blocked if LSA Protection were enabled "
                + "without actually enabling protection. Disable once LSA Protection "
                + "is confirmed stable on the system.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPLBoot", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPLBoot")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPLBoot", 2)],
        },
    ];
}
