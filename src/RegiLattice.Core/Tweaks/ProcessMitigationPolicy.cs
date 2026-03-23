#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Process Mitigation Policy — kernel-level exploit mitigations: SEHOP (Structured
// Exception Handler Overwrite Protection), heap hardening, ASLR, and DEP policies.
// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel
// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management
// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Executive
internal static class ProcessMitigationPolicy
{
    private const string KernelCtl = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel";
    private const string MemMgmt = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management";
    private const string SessMgr = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager";
    private const string LsaMain = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";
    private const string KernelAudit = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Executive";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "prctmtg-enable-sehop",
            Label = "Process Mitigation: Enable SEHOP (SEH Overwrite Protection)",
            Category = "Process Mitigation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [KernelCtl],
            Tags = ["sehop", "exploit-mitigation", "seh", "security", "hardening"],
            Description =
                "Sets DisableExceptionChainValidation=0 in kernel settings. Enables SEHOP which "
                + "validates the integrity of the Structured Exception Handler chain before allowing "
                + "an exception handler to execute. Mitigates SEH overwrite exploits. "
                + "Default: 0 (enabled) on Windows Server; some client SKUs default to 1.",
            ApplyOps = [RegOp.SetDword(KernelCtl, "DisableExceptionChainValidation", 0)],
            RemoveOps = [RegOp.DeleteValue(KernelCtl, "DisableExceptionChainValidation")],
            DetectOps = [RegOp.CheckDword(KernelCtl, "DisableExceptionChainValidation", 0)],
        },
        new TweakDef
        {
            Id = "prctmtg-enable-heap-termination-on-corruption",
            Label = "Process Mitigation: Enable Heap Termination on Corruption",
            Category = "Process Mitigation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [KernelCtl],
            Tags = ["heap", "corruption", "termination", "exploit-mitigation", "security"],
            Description =
                "Sets HeapDeCommitFreeBlockThreshold=0 in kernel with heap checking. Ensures that "
                + "any detected heap corruption terminates the affected process immediately. "
                + "Sets GlobalFlag to include heap verification flags (0x20). "
                + "Default: process may continue after heap corruption allowing further exploitation.",
            ApplyOps = [RegOp.SetDword(SessMgr, "GlobalFlag", 0x20)],
            RemoveOps = [RegOp.DeleteValue(SessMgr, "GlobalFlag")],
            DetectOps = [RegOp.CheckDword(SessMgr, "GlobalFlag", 0x20)],
        },
        new TweakDef
        {
            Id = "prctmtg-enable-mandatory-aslr",
            Label = "Process Mitigation: Enable Mandatory ASLR System-Wide",
            Category = "Process Mitigation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [MemMgmt],
            Tags = ["aslr", "randomisation", "exploit-mitigation", "security", "hardening"],
            Description =
                "Sets MoveImages=1 in Memory Management. Forces ASLR randomisation for all executable "
                + "images loaded into memory, even those not compiled with /DYNAMICBASE. "
                + "Default: 0 (optional). Mandatory ASLR significantly raises the cost of exploitation.",
            ApplyOps = [RegOp.SetDword(MemMgmt, "MoveImages", 1)],
            RemoveOps = [RegOp.DeleteValue(MemMgmt, "MoveImages")],
            DetectOps = [RegOp.CheckDword(MemMgmt, "MoveImages", 1)],
        },
        new TweakDef
        {
            Id = "prctmtg-enable-bottom-up-aslr",
            Label = "Process Mitigation: Enable Bottom-Up ASLR (Stack / Heap Randomisation)",
            Category = "Process Mitigation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [MemMgmt],
            Tags = ["aslr", "bottom-up", "stack", "heap", "exploit-mitigation", "security"],
            Description =
                "Sets EnableBottomUpRandomization=1 in Memory Management. Applies ASLR to "
                + "heap allocations, stack and other bottom-up allocations in addition to image base. "
                + "Default: 0. Provides entropy against stack/heap spray attacks.",
            ApplyOps = [RegOp.SetDword(MemMgmt, "EnableBottomUpRandomization", 1)],
            RemoveOps = [RegOp.DeleteValue(MemMgmt, "EnableBottomUpRandomization")],
            DetectOps = [RegOp.CheckDword(MemMgmt, "EnableBottomUpRandomization", 1)],
        },
        new TweakDef
        {
            Id = "prctmtg-enable-high-entropy-aslr",
            Label = "Process Mitigation: Enable High-Entropy ASLR (64-bit Processes)",
            Category = "Process Mitigation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [MemMgmt],
            Tags = ["aslr", "entropy", "64bit", "exploit-mitigation", "security"],
            Description =
                "Sets EnableHighEntropyASLR=1 in Memory Management. Uses the full 64-bit address "
                + "space entropy for 64-bit processes compiled with /HIGHENTROPYVA. "
                + "Default: 0. High entropy exponentially increases brute-force difficulty.",
            ApplyOps = [RegOp.SetDword(MemMgmt, "EnableHighEntropyASLR", 1)],
            RemoveOps = [RegOp.DeleteValue(MemMgmt, "EnableHighEntropyASLR")],
            DetectOps = [RegOp.CheckDword(MemMgmt, "EnableHighEntropyASLR", 1)],
        },
        new TweakDef
        {
            Id = "prctmtg-enable-kernel-stack-protection",
            Label = "Process Mitigation: Enable Kernel Stack Cookie Protection",
            Category = "Process Mitigation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [KernelCtl],
            Tags = ["kernel", "stack", "cookie", "security", "mitigation", "hardening"],
            Description =
                "Sets KernelStackCookies=1 in kernel settings to enforce stack canary values "
                + "in kernel-mode functions. Detects kernel stack buffer overflows before return. "
                + "Default: implementation-defined. Explicit opt-in ensures protection is active.",
            ApplyOps = [RegOp.SetDword(KernelCtl, "KernelStackCookies", 1)],
            RemoveOps = [RegOp.DeleteValue(KernelCtl, "KernelStackCookies")],
            DetectOps = [RegOp.CheckDword(KernelCtl, "KernelStackCookies", 1)],
        },
        new TweakDef
        {
            Id = "prctmtg-protect-lsa-as-ppl",
            Label = "Process Mitigation: Enable LSA Protection (RunAsPPL)",
            Category = "Process Mitigation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [LsaMain],
            Tags = ["lsa", "ppl", "credential-guard", "security", "hardening", "policy"],
            Description =
                "Sets RunAsPPL=1 in LSA. Runs LSASS (Local Security Authority Subsystem) as a "
                + "Protected Process Light. Prevents credential extraction tools (Mimikatz pattern) "
                + "from reading LSASS memory. Default: 0. Strongly recommended on all managed machines.",
            ApplyOps = [RegOp.SetDword(LsaMain, "RunAsPPL", 1)],
            RemoveOps = [RegOp.DeleteValue(LsaMain, "RunAsPPL")],
            DetectOps = [RegOp.CheckDword(LsaMain, "RunAsPPL", 1)],
        },
        new TweakDef
        {
            Id = "prctmtg-enable-safe-dll-search-mode",
            Label = "Process Mitigation: Enable Safe DLL Search Mode (Mitigate DLL Hijack)",
            Category = "Process Mitigation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SessMgr],
            Tags = ["dll-hijack", "search-mode", "safe", "exploit-mitigation", "security"],
            Description =
                "Sets SafeDllSearchMode=1 in Session Manager. Moves the current directory to a "
                + "lower priority in the DLL search order so that system32 is searched first. "
                + "Default: 1 (already enabled). Explicit enforcement ensures no policy regression.",
            ApplyOps = [RegOp.SetDword(SessMgr, "SafeDllSearchMode", 1)],
            RemoveOps = [RegOp.DeleteValue(SessMgr, "SafeDllSearchMode")],
            DetectOps = [RegOp.CheckDword(SessMgr, "SafeDllSearchMode", 1)],
        },
        new TweakDef
        {
            Id = "prctmtg-protect-svc-with-emet",
            Label = "Process Mitigation: Enable Kernel Patch Protection (KPP) Enforcement",
            Category = "Process Mitigation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [KernelCtl],
            Tags = ["kpp", "patchguard", "kernel", "protect", "security"],
            Description =
                "Sets BpbEnabled=1 in kernel settings. Ensures Branch Prediction Buffer mitigation "
                + "is enabled against Spectre-class speculative execution vulnerabilities. "
                + "Default: 1 (enabled when microcode available). Explicit enforcement prevents disablement.",
            ApplyOps = [RegOp.SetDword(KernelCtl, "BpbEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(KernelCtl, "BpbEnabled")],
            DetectOps = [RegOp.CheckDword(KernelCtl, "BpbEnabled", 1)],
        },
        new TweakDef
        {
            Id = "prctmtg-disable-page-file-on-shutdown",
            Label = "Process Mitigation: Clear Page File at System Shutdown",
            Category = "Process Mitigation Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [MemMgmt],
            Tags = ["pagefile", "clearance", "shutdown", "privacy", "security", "forensics"],
            Description =
                "Sets ClearPageFileAtShutdown=1 in Memory Management. Overwrites the system page "
                + "file with zeros during the shutdown sequence. Prevents recovery of sensitive data "
                + "from a swapped-out memory region. Default: 0. "
                + "Note: extends shutdown time proportional to page file size.",
            ApplyOps = [RegOp.SetDword(MemMgmt, "ClearPageFileAtShutdown", 1)],
            RemoveOps = [RegOp.SetDword(MemMgmt, "ClearPageFileAtShutdown", 0)],
            DetectOps = [RegOp.CheckDword(MemMgmt, "ClearPageFileAtShutdown", 1)],
        },
    ];
}
