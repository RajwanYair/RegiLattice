// RegiLattice.Core — Tweaks/WindowsDefenderApplicationControlPolicy.cs
// WDAC code integrity policy enforcement, UMCI, kernel driver signing, and policy refresh settings — Sprint 529.
// Category: "WDAC Code Integrity Policy" | Slug: wdacpol
// Registry: HKLM\SYSTEM\CurrentControlSet\Control\CI\Policy

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsDefenderApplicationControlPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Policy";
    private const string CfgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard";
    private const string SipKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SipEngine";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "wdacpol-enable-hvci-kernel-mode",
            Label        = "Enable Hypervisor-Protected Code Integrity (HVCI) in Strict Mode",
            Category     = "WDAC Code Integrity Policy",
            Description  = "Enables HVCI (Memory integrity) in strict enforcement mode, protecting kernel-mode code and data in a Hyper-V virtual trust level, preventing kernel driver exploits from modifying kernel memory or loading unsigned drivers.",
            Tags         = ["hvci", "memory-integrity", "kernel", "wdac", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "HVCI strict mode enabled; kernel memory protected by Hyper-V. Unsigned kernel driver exploits blocked.",
            ApplyOps     = [RegOp.SetDword(CfgKey, "EnableVirtualizationBasedSecurity", 1)],
            RemoveOps    = [RegOp.DeleteValue(CfgKey, "EnableVirtualizationBasedSecurity")],
            DetectOps    = [RegOp.CheckDword(CfgKey, "EnableVirtualizationBasedSecurity", 1)],
        },
        new TweakDef
        {
            Id           = "wdacpol-require-platform-security-vbs",
            Label        = "Require Platform Security Features for VBS (Secure Boot + DMA)",
            Category     = "WDAC Code Integrity Policy",
            Description  = "Configures Virtualization Based Security to require both Secure Boot and DMA protection (IOMMU) as mandatory platform security features, ensuring VBS protection cannot be enabled without proper hardware isolation.",
            Tags         = ["vbs", "secure-boot", "dma-protection", "hvci", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "VBS requires Secure Boot + DMA/IOMMU hardware; VBS cannot be bypassed by disabling hardware security.",
            ApplyOps     = [RegOp.SetDword(CfgKey, "RequirePlatformSecurityFeatures", 3)],
            RemoveOps    = [RegOp.DeleteValue(CfgKey, "RequirePlatformSecurityFeatures")],
            DetectOps    = [RegOp.CheckDword(CfgKey, "RequirePlatformSecurityFeatures", 3)],
        },
        new TweakDef
        {
            Id           = "wdacpol-enable-umci-enforcement",
            Label        = "Enable User Mode Code Integrity (UMCI) Enforcement via WDAC",
            Category     = "WDAC Code Integrity Policy",
            Description  = "Enables User Mode Code Integrity enforcement via Windows Defender Application Control, requiring all user-mode executables and DLLs to be signed by an allow-listed publisher or policy rule before loading.",
            Tags         = ["wdac", "umci", "user-mode", "code-signing", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "UMCI enforcement enabled; unsigned user-mode binaries blocked from executing. Requires WDAC policy deployment.",
            ApplyOps     = [RegOp.SetDword(CfgKey, "HypervisorEnforcedCodeIntegrity", 1)],
            RemoveOps    = [RegOp.DeleteValue(CfgKey, "HypervisorEnforcedCodeIntegrity")],
            DetectOps    = [RegOp.CheckDword(CfgKey, "HypervisorEnforcedCodeIntegrity", 1)],
        },
        new TweakDef
        {
            Id           = "wdacpol-disable-test-signing",
            Label        = "Disable Kernel Test Signing Mode (Block Development Bypass)",
            Category     = "WDAC Code Integrity Policy",
            Description  = "Prevents the kernel from loading drivers that are only test-signed (not production WHQL or EV-signed), closing the development bypass mode that allows unsigned driver loading without hardware attestation.",
            Tags         = ["wdac", "test-signing", "driver-signing", "kernel", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Kernel test signing disabled; only production-signed drivers load. Development signing bypass blocked.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableTestSigning", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableTestSigning")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableTestSigning", 1)],
        },
        new TweakDef
        {
            Id           = "wdacpol-block-vulnerable-driver-loading",
            Label        = "Enable WDAC Vulnerable Driver Blocklist (Microsoft HVCI Blocklist)",
            Category     = "WDAC Code Integrity Policy",
            Description  = "Enables the Microsoft-maintained vulnerable driver blocklist (applied via HVCI when memory integrity is on), preventing loading of known LOLBAS kernel drivers used for BYOVD (Bring Your Own Vulnerable Driver) kernel exploits.",
            Tags         = ["wdac", "vulnerable-driver", "byovd", "hvci", "blocklist", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "Microsoft vulnerable driver blocklist enforced; known BYOVD exploit driver loading blocked.",
            ApplyOps     = [RegOp.SetDword(CfgKey, "EnableWindowsDriverBlocklist", 1)],
            RemoveOps    = [RegOp.DeleteValue(CfgKey, "EnableWindowsDriverBlocklist")],
            DetectOps    = [RegOp.CheckDword(CfgKey, "EnableWindowsDriverBlocklist", 1)],
        },
        new TweakDef
        {
            Id           = "wdacpol-require-whql-for-new-drivers",
            Label        = "Require WHQL Signature for New Kernel-Mode Drivers",
            Category     = "WDAC Code Integrity Policy",
            Description  = "Configures code integrity to require WHQL (Windows Hardware Quality Lab) signatures on new kernel-mode drivers, blocking loading of drivers signed only with a self-signed or EV code signing certificate without WHQL attestation.",
            Tags         = ["wdac", "whql", "kernel-driver", "signing", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "New kernel drivers require WHQL signature; EV-only signed drivers without WHQL attestation blocked.",
            ApplyOps     = [RegOp.SetDword(Key, "RequireWHQLForNewDrivers", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "RequireWHQLForNewDrivers")],
            DetectOps    = [RegOp.CheckDword(Key, "RequireWHQLForNewDrivers", 1)],
        },
        new TweakDef
        {
            Id           = "wdacpol-disable-dynamic-code-policy",
            Label        = "Set WDAC Dynamic Code Security Policy to Enforce Mode",
            Category     = "WDAC Code Integrity Policy",
            Description  = "Sets the WDAC dynamic code policy to enforced mode, protecting dynamically generated code (JIT-compiled scripts, .NET, browsers) from injecting unsigned code pages that bypass static WDAC policy checks.",
            Tags         = ["wdac", "dynamic-code", "jit", "enforcement", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "WDAC dynamic code security enforced; JIT-injected code pages validated against code integrity policy.",
            ApplyOps     = [RegOp.SetDword(SipKey, "DynamicCodeSecurity", 2)],
            RemoveOps    = [RegOp.DeleteValue(SipKey, "DynamicCodeSecurity")],
            DetectOps    = [RegOp.CheckDword(SipKey, "DynamicCodeSecurity", 2)],
        },
        new TweakDef
        {
            Id           = "wdacpol-log-ci-failures",
            Label        = "Log Code Integrity Violation Events in CodeIntegrity Log",
            Category     = "WDAC Code Integrity Policy",
            Description  = "Enables logging of code integrity block decisions in the Microsoft-Windows-CodeIntegrity/Operational event log channel, providing audit records of all executables and drivers blocked by WDAC or HVCI policy.",
            Tags         = ["wdac", "event-log", "audit", "ci-failure", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Code integrity violation events logged; all WDAC/HVCI blocked files visible in CodeIntegrity event channel.",
            ApplyOps     = [RegOp.SetDword(Key, "LogCIFailures", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "LogCIFailures")],
            DetectOps    = [RegOp.CheckDword(Key, "LogCIFailures", 1)],
        },
        new TweakDef
        {
            Id           = "wdacpol-disable-debug-policy",
            Label        = "Disable WDAC Debug/Audit Mode (Enforce Kernel Debugging Disabled)",
            Category     = "WDAC Code Integrity Policy",
            Description  = "Prevents kernel debugging from being enabled on this system via bcdedit /debug, which would disable code integrity checks entirely, ensuring WDAC cannot be bypassed by attaching a kernel debugger.",
            Tags         = ["wdac", "kernel-debug", "debug-mode", "bypass", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "Kernel debug mode blocked; WDAC/CI cannot be bypassed via kernel debugger attachment.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableKernelDebugPolicy", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableKernelDebugPolicy")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableKernelDebugPolicy", 1)],
        },
        new TweakDef
        {
            Id           = "wdacpol-disable-wdac-telemetry",
            Label        = "Disable WDAC Code Integrity Telemetry to Microsoft",
            Category     = "WDAC Code Integrity Policy",
            Description  = "Prevents WDAC and Windows Code Integrity from reporting blocked binary hashes, publisher names, violation rates, and policy effectiveness telemetry to Microsoft.",
            Tags         = ["wdac", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "WDAC telemetry to Microsoft disabled; blocked binary hashes and policy stats not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(CfgKey, "DisableWDACTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(CfgKey, "DisableWDACTelemetry")],
            DetectOps    = [RegOp.CheckDword(CfgKey, "DisableWDACTelemetry", 1)],
        },
    ];
}
