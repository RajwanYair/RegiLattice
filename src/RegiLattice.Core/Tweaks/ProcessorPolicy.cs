// RegiLattice.Core — Tweaks/ProcessorPolicy.cs
// Sprint 318: Processor Policy tweaks (10 tweaks)
// Category: "Processor Policy" | Slug: proccpol
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Processor

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ProcessorPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Processor";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "proccpol-disable-speculative-execution",
            Label = "Enable Spectre/Meltdown Mitigations",
            Category = "Processor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Spectre and Meltdown are hardware vulnerabilities in modern processors that allow malicious code to read arbitrary memory through side-channel attacks. Enabling processor mitigations activates kernel and firmware-level protections that prevent exploitation of these speculative execution vulnerabilities. Without mitigations enabled malicious processes can read kernel memory, other process memory, and hypervisor memory they should not have access to. Intel, AMD, and ARM all released firmware and microcode updates to address these vulnerabilities when combined with OS-level mitigations. Performance impact from mitigations varies by workload but security benefits far outweigh the performance cost for enterprise endpoints. Mitigations must be enabled both through OS policy and microcode/firmware updates for complete protection.",
            Tags = ["processor", "spectre", "meltdown", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableMitigations", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableMitigations")],
            DetectOps = [RegOp.CheckDword(Key, "EnableMitigations", 1)],
        },
        new TweakDef
        {
            Id = "proccpol-enable-retpoline",
            Label = "Enable Retpoline Spectre Variant 2 Mitigation",
            Category = "Processor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Retpoline is a software mitigation technique that replaces indirect branch instructions with a safer equivalent that prevents branch target injection. Enabling Retpoline activates the compiler-based mitigation for Spectre variant 2 branch target injection vulnerabilities. Spectre variant 2 allows malicious code to manipulate CPU branch prediction to speculatively execute code at arbitrary locations. Retpoline provides Spectre variant 2 protection with significantly lower performance overhead than alternative mitigations. Windows builds include Retpoline when supported by the system configuration including processor microcode and OS version. Retpoline is the preferred mitigation approach and should be enabled wherever the required processor and system support is present.",
            Tags = ["processor", "spectre", "retpoline", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableRetpoline", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableRetpoline")],
            DetectOps = [RegOp.CheckDword(Key, "EnableRetpoline", 1)],
        },
        new TweakDef
        {
            Id = "proccpol-enable-kva-shadowing",
            Label = "Enable Kernel VA Shadowing (Meltdown Mitigation)",
            Category = "Processor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Kernel Virtual Address Shadowing separates kernel and user address spaces to prevent user-mode code from accessing kernel memory through Meltdown. KVA Shadowing ensures that kernel pages are not mapped into user process address space preventing Meltdown-style reads of kernel data. Meltdown allows user processes to read arbitrary kernel memory including passwords, encryption keys, and other sensitive data. KVA Shadowing was introduced in Windows 10 1803 as the primary Meltdown mitigation for Intel CPUs. AMD CPUs are generally not vulnerable to Meltdown but enabling KVA Shadowing provides defense-in-depth. KVA Shadowing does have a performance impact on workloads with frequent kernel transitions but the security benefit is essential.",
            Tags = ["processor", "meltdown", "kva", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableKvaShadowing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableKvaShadowing")],
            DetectOps = [RegOp.CheckDword(Key, "EnableKvaShadowing", 1)],
        },
        new TweakDef
        {
            Id = "proccpol-enable-ssbd",
            Label = "Enable Speculative Store Bypass Disable (SSBD)",
            Category = "Processor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Speculative Store Bypass is a CPU vulnerability where speculative execution can bypass store-to-load forwarding and read stale data from memory. Enabling SSBD activates hardware mitigation via the SSBD MSR bit that prevents speculative access to data from prior stores. Speculative Store Bypass can be exploited to read data that should have been overwritten or isolated by store operations. SSBD is required for JIT-compiled execution environments including browser JavaScript engines where multiple execution contexts share a process. Enterprise endpoints running JavaScript-based enterprise applications may be vulnerable through browser JIT compilation without SSBD. SSBD has low performance impact and should be enabled on all processors that support the SSBD hardware bit.",
            Tags = ["processor", "ssbd", "speculative", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSSBD", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSSBD")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSSBD", 1)],
        },
        new TweakDef
        {
            Id = "proccpol-enable-mds-mitigations",
            Label = "Enable Microarchitectural Data Sampling Mitigations",
            Category = "Processor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Microarchitectural Data Sampling vulnerabilities including RIDL and Fallout allow processes to sample data from CPU internal buffers during speculative execution. Enabling MDS mitigations activates CPU buffer clearing operations that flush microarchitectural buffers to prevent cross-domain data leakage. MDS attacks can leak data across process boundaries, hypervisor boundaries, and between SMT sibling threads in Intel processors. On systems with Hyper-Threading or SMT enabled MDS mitigations may include disabling SMT for complete protection. Intel CPUs from Cascade Lake and later include hardware mitigations that reduce the performance impact of software MDS mitigations. MDS mitigations should be enabled on all Intel processors that do not have hardware MDS mitigations built in.",
            Tags = ["processor", "mds", "speculative", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableMDSMitigations", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableMDSMitigations")],
            DetectOps = [RegOp.CheckDword(Key, "EnableMDSMitigations", 1)],
        },
        new TweakDef
        {
            Id = "proccpol-disable-hyper-threading-spectre",
            Label = "Configure SMT for Speculative Execution Safety",
            Category = "Processor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Simultaneous Multi-Threading shares processor resources between logical cores which creates side-channel leakage paths for speculative execution attacks. Configuring SMT safely for speculative execution ensures that sibling thread data is isolated through appropriate microarchitectural mitigations. MDS and cache-based side-channel attacks are more effective when the attacker and victim share an SMT core. For extremely high-security workloads where perfect isolation is required SMT disabling may be considered despite the performance impact. Modern processor microcode combined with OS MDS mitigations provides substantial SMT isolation that covers most enterprise threat models. Security teams should evaluate whether remaining SMT-based side-channel exposure is within tolerance for their specific threat environment.",
            Tags = ["processor", "smt", "speculative", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ConfigureSMTForSecurity", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ConfigureSMTForSecurity")],
            DetectOps = [RegOp.CheckDword(Key, "ConfigureSMTForSecurity", 1)],
        },
        new TweakDef
        {
            Id = "proccpol-enable-tsx-mitigations",
            Label = "Enable TSX Asynchronous Abort Mitigations",
            Category = "Processor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "TSX Asynchronous Abort is an Intel CPU vulnerability where transactional synchronization extensions can leak data during transactional abort handling. Enabling TAA mitigations prevents exploitation of TSX Asynchronous Abort vulnerabilities through VERW instruction flushing of CPU buffers. TAA is closely related to MDS vulnerabilities and requires similar buffer-clearing mitigations. Systems with Intel TSX disabled through microcode updates are protected against TAA but TAA mitigations should also be enabled as defense-in-depth. The TAA mitigation VERW instruction overhead is minimal on processors that support the enhanced TAA mitigation capability. TAA mitigations do not affect functionality and should be enabled on all Intel processors with TSX capabilities.",
            Tags = ["processor", "taa", "tsx", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableTAAMitigations", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableTAAMitigations")],
            DetectOps = [RegOp.CheckDword(Key, "EnableTAAMitigations", 1)],
        },
        new TweakDef
        {
            Id = "proccpol-enable-ibrs",
            Label = "Enable Indirect Branch Restricted Speculation (IBRS)",
            Category = "Processor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Indirect Branch Restricted Speculation prevents software running at lower privilege levels from influencing indirect branches in more privileged code. Enabling IBRS prevents user-mode code from poisoning indirect branch predictors used by kernel-mode code for Spectre variant 2 attacks. IBRS provides hardware-level mitigation when combined with appropriate processor microcode that supports the IBRS capability. Enhanced IBRS available in newer processors keeps IBRS active continuously with lower performance overhead than the original IBRS implementation. Retpoline provides an alternative Spectre variant 2 mitigation but IBRS provides hardware-based protection where Retpoline is not available. Both IBRS and Retpoline should be evaluated based on the processor generation present in the enterprise hardware fleet.",
            Tags = ["processor", "ibrs", "spectre", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableIBRS", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableIBRS")],
            DetectOps = [RegOp.CheckDword(Key, "EnableIBRS", 1)],
        },
        new TweakDef
        {
            Id = "proccpol-enable-ibpb",
            Label = "Enable Indirect Branch Predictor Barrier (IBPB)",
            Category = "Processor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Indirect Branch Predictor Barrier flushes indirect branch predictor state when transitioning between different privilege levels or security contexts. Enabling IBPB ensures that predictions accumulated in one security context cannot influence code execution in a different security context. IBPB is particularly important at context switches between processes to prevent cross-process branch prediction poisoning. Without IBPB a malicious process can train the branch predictor before a context switch and influence speculative execution in the victim process. IBPB has some performance overhead at context switches but provides important cross-process isolation for Spectre variant 2. IBPB should be enabled on all processors that support the IBPB mechanism through microcode or architecture.",
            Tags = ["processor", "ibpb", "spectre", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableIBPB", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableIBPB")],
            DetectOps = [RegOp.CheckDword(Key, "EnableIBPB", 1)],
        },
        new TweakDef
        {
            Id = "proccpol-enable-stibp",
            Label = "Enable Single Thread Indirect Branch Predictors (STIBP)",
            Category = "Processor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Single Thread Indirect Branch Predictors isolation prevents branch predictor sharing between sibling hyperthreads on SMT-enabled processors. Enabling STIBP ensures that branch state in one logical processor is isolated from its SMT sibling's branch prediction. Spectre cross-hyperthread attacks allow one logical processor to train the shared branch predictor and affect execution in the sibling thread. STIBP is essential for preventing cross-hyperthread Spectre variant 2 attacks on systems with SMT or Hyper-Threading enabled. The performance overhead of STIBP is process-context-dependent but modern Always-On STIBP implementations have reduced overhead. STIBP should be enabled on SMT-capable processors to prevent the hyperthread-based Spectre attack pathway.",
            Tags = ["processor", "stibp", "spectre", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSTIBP", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSTIBP")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSTIBP", 1)],
        },
    ];
}
