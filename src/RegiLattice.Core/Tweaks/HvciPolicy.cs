// RegiLattice.Core — Tweaks/HvciPolicy.cs
// Hypervisor-Protected Code Integrity (HVCI) policy for drivers, mode enforcement, and exclusions — Sprint 463.
// Uses ScenarioHVCI path exclusively — distinct from VbsEnforcementPolicy which uses DeviceGuard root.
// Category: "HVCI Policy" | Slug: hvci
// Registry: HKLM\SYSTEM\CurrentControlSet\Control\CI\Policy
//           HKLM\SYSTEM\CurrentControlSet\Control\CI\Config

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class HvciPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Policy";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Config";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "hvci-set-policy-level-strict",
                Label = "Set HVCI Code Integrity Policy to Strict",
                Category = "HVCI Policy",
                Description =
                    "Sets the Code Integrity policy level to Strict via the CI\\Policy key, blocking DLL injections and kernel-mode payloads that exploit unsigned code paths not caught by the default policy.",
                Tags = ["hvci", "code-integrity", "strict", "driver", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "Strict CI policy; unsigned kernel/user-mode injections blocked. Test pre-rollout; may break old software.",
                ApplyOps = [RegOp.SetDword(Key, "SkipInvalidUnattendSpecPass", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SkipInvalidUnattendSpecPass")],
                DetectOps = [RegOp.CheckDword(Key, "SkipInvalidUnattendSpecPass", 0)],
            },
            new TweakDef
            {
                Id = "hvci-block-driver-vulnerability-list",
                Label = "Enable Vulnerable Driver Blocklist",
                Category = "HVCI Policy",
                Description =
                    "Enables the Microsoft Vulnerable Driver Blocklist (also built into Windows Security Center) via the CI policy, preventing known exploitable drivers from loading regardless of signature status.",
                Tags = ["hvci", "driver-blocklist", "vulnerable-drivers", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Vulnerable drivers on the Microsoft blocklist cannot load; prevents BYOVD kernel exploits.",
                ApplyOps = [RegOp.SetDword(Key, "BlockPolicyUpdateTaskEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockPolicyUpdateTaskEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "BlockPolicyUpdateTaskEnabled", 1)],
            },
            new TweakDef
            {
                Id = "hvci-enable-ci-flight-check",
                Label = "Enable Code Integrity Flight Signing Check",
                Category = "HVCI Policy",
                Description =
                    "Enables flight signing checks in CI policy, ensuring Windows Insider / pre-release kernel updates still pass code integrity verification while on production builds.",
                Tags = ["hvci", "flight-signing", "ci", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Flight signing verified; CI policy does not break on pre-release or insider kernel updates.",
                ApplyOps = [RegOp.SetDword(Key, "FlightSigningEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "FlightSigningEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "FlightSigningEnabled", 1)],
            },
            new TweakDef
            {
                Id = "hvci-block-dev-mode-km-bypass",
                Label = "Block Developer Mode Kernel Bypass of HVCI",
                Category = "HVCI Policy",
                Description =
                    "Prevents Developer Mode (Sideload Apps / Test Signing) from bypassing HVCI code integrity enforcement, ensuring HVCI cannot be defeated by enabling Developer Mode.",
                Tags = ["hvci", "developer-mode", "test-signing", "bypass", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Developer Mode cannot disable HVCI; test-signed drivers still blocked on locked-down machines.",
                ApplyOps = [RegOp.SetDword(Key, "BlockDevModeHVCIBypass", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockDevModeHVCIBypass")],
                DetectOps = [RegOp.CheckDword(Key, "BlockDevModeHVCIBypass", 1)],
            },
            new TweakDef
            {
                Id = "hvci-disable-kernel-debug-bypass",
                Label = "Disable Kernel Debugging Bypass of Code Integrity",
                Category = "HVCI Policy",
                Description =
                    "Prevents kernel debugger attachment from disabling HVCI code integrity checks, ensuring that even a live kernel debug session cannot load unsigned drivers.",
                Tags = ["hvci", "kernel-debug", "code-integrity", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Kernel debug mode cannot bypass CI; active kernel debugging will not load unsigned code.",
                ApplyOps = [RegOp.SetDword(Key, "DisableKernelDebugCIBypass", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableKernelDebugCIBypass")],
                DetectOps = [RegOp.CheckDword(Key, "DisableKernelDebugCIBypass", 1)],
            },
            new TweakDef
            {
                Id = "hvci-enable-user-mode-ci",
                Label = "Enable User-Mode Code Integrity (UMCI)",
                Category = "HVCI Policy",
                Description =
                    "Extends code integrity enforcement to user mode via UMCI, requiring all user-mode executables and DLLs to be signed, providing application whitelisting at the OS policy level.",
                Tags = ["hvci", "umci", "user-mode", "code-integrity", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "UMCI enforced; all user-mode binaries require signatures. Breaks most unsigned applications.",
                ApplyOps = [RegOp.SetDword(Key2, "UMCIEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "UMCIEnabled")],
                DetectOps = [RegOp.CheckDword(Key2, "UMCIEnabled", 1)],
            },
            new TweakDef
            {
                Id = "hvci-block-hmac-degradation",
                Label = "Block HMAC Algorithm Downgrade in CI Validation",
                Category = "HVCI Policy",
                Description =
                    "Prevents CI signature validation from falling back to weak legacy HMAC algorithms, ensuring code integrity checks always use strong cryptographic hashing.",
                Tags = ["hvci", "hmac", "cryptography", "downgrade", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Weak HMAC fallback in CI disabled; old binaries signed with MD5/SHA1 only may fail verification.",
                ApplyOps = [RegOp.SetDword(Key2, "BlockHMACDowngrade", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "BlockHMACDowngrade")],
                DetectOps = [RegOp.CheckDword(Key2, "BlockHMACDowngrade", 1)],
            },
            new TweakDef
            {
                Id = "hvci-enforce-efi-boot-driver-check",
                Label = "Enforce EFI Boot Driver Code Integrity Check",
                Category = "HVCI Policy",
                Description =
                    "Extends HVCI enforcement to EFI boot-time drivers loaded by the firmware, ensuring CI policy covers the entire boot chain and not just post-HORM drivers.",
                Tags = ["hvci", "efi", "boot-driver", "secure-boot", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "EFI boot driver integrity verified; unsigned EFI modules blocked before OS handoff.",
                ApplyOps = [RegOp.SetDword(Key2, "EnforceEFIBootDriverCI", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "EnforceEFIBootDriverCI")],
                DetectOps = [RegOp.CheckDword(Key2, "EnforceEFIBootDriverCI", 1)],
            },
            new TweakDef
            {
                Id = "hvci-block-ci-opt-out-for-drivers",
                Label = "Block Per-Driver CI Opt-Out Flag",
                Category = "HVCI Policy",
                Description =
                    "Prevents individual drivers from setting a CI opt-out flag in their INF to bypass HVCI verification, ensuring the CI policy cannot be weakened driver-by-driver.",
                Tags = ["hvci", "driver", "opt-out", "bypass-prevention", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Per-driver CI opt-out disallowed; all drivers must comply with CI policy uniformly.",
                ApplyOps = [RegOp.SetDword(Key2, "BlockCIOptOutForDrivers", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "BlockCIOptOutForDrivers")],
                DetectOps = [RegOp.CheckDword(Key2, "BlockCIOptOutForDrivers", 1)],
            },
            new TweakDef
            {
                Id = "hvci-enable-ci-policy-telemetry",
                Label = "Enable CI Policy Violation Telemetry Reporting",
                Category = "HVCI Policy",
                Description =
                    "Enables reporting of Code Integrity policy violations to Windows Defender ATP / Microsoft Defender for Endpoint, supporting cloud-based detection of BYOVD and LOLBAS attack patterns.",
                Tags = ["hvci", "telemetry", "defender", "atp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "CI violation telemetry sent to MDE; security team can detect driver-based attacks from the cloud.",
                ApplyOps = [RegOp.SetDword(Key, "EnableCIPolicyTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableCIPolicyTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "EnableCIPolicyTelemetry", 1)],
            },
        ];
}
