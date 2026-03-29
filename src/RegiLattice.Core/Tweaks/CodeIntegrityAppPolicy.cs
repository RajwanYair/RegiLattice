// RegiLattice.Core — Tweaks/CodeIntegrityAppPolicy.cs
// Code Integrity / Application Policy — Sprint 572.
// Configures Group Policy for Windows Defender Application Control
// (WDAC) and Windows Code Integrity: enforcement mode, audit mode,
// UMCI, policy refresh, and unsigned code blocking configurations.
// Category: "Code Integrity Application Policy" | Slug: wdacapp
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\SrpV2

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CodeIntegrityAppPolicy
{
    private const string DgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard";

    private const string SrpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wdacapp-enable-hypervisor-code-integrity",
                Label = "WDAC: Enable HVCI (Hypervisor-Protected Code Integrity)",
                Category = "Code Integrity Application Policy",
                Description =
                    "Sets HypervisorEnforcedCodeIntegrity=1 in DeviceGuard policy. Enables Hypervisor-Protected Code Integrity (HVCI, also called Memory Integrity). HVCI moves kernel code integrity checking into the secure virtual machine backed by the CPU hypervisor, making it impossible for even a kernel-level exploit to modify the code signing enforcement rules. Without HVCI, a kernel exploit that gains ring-0 execution can disable code integrity by patching the CI routines in memory. HVCI requires hardware-enforced virtualisation (SLAT, IOMMU) and may require drivers to be WHQL-compliant. Incompatible drivers cause BSODs.",
                Tags = ["hvci", "memory-integrity", "hypervisor", "kernel", "code-signing"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote =
                    "HVCI is enabled. Kernel code integrity is enforced from the secure VM. Incompatible drivers (non-WHQL or using deprecated kernel APIs) cause BSODs. Pre-deployment driver compatibility scan is mandatory. 5–15% kernel performance overhead on older hardware due to memory isolation transitions.",
                ApplyOps = [RegOp.SetDword(DgKey, "HypervisorEnforcedCodeIntegrity", 1)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "HypervisorEnforcedCodeIntegrity")],
                DetectOps = [RegOp.CheckDword(DgKey, "HypervisorEnforcedCodeIntegrity", 1)],
            },
            new TweakDef
            {
                Id = "wdacapp-enable-user-mode-code-integrity",
                Label = "WDAC: Enable User-Mode Code Integrity (UMCI)",
                Category = "Code Integrity Application Policy",
                Description =
                    "Sets UsermodeCodeIntegrityPolicyEnforcementMode=1 in DeviceGuard policy. Enables enforcement of WDAC (Windows Defender Application Control) policies in user mode. UMCI extends application whitelisting from kernel-mode drivers to user-mode processes — requiring all executables (.exe, .dll, .ps1, script hosts) to be signed by trusted publishers before they are permitted to run. Without UMCI, application control only blocks untrusted kernel drivers. UMCI is the primary mechanism for application whitelisting that stops malware, ransomware, and living-off-the-land (LOtL) binaries from executing in user space.",
                Tags = ["umci", "application-control", "whitelisting", "user-mode", "wdac"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote =
                    "All user-mode executables, DLLs, and scripts must be signed by a trusted publisher. Unsigned or self-signed code is blocked. Legitimate internal applications that are unsigned must be signed or added to the WDAC policy exceptions before enabling enforcement mode. Recommended to run in audit mode for 90+ days before switching to enforcement.",
                ApplyOps = [RegOp.SetDword(DgKey, "UsermodeCodeIntegrityPolicyEnforcementMode", 1)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "UsermodeCodeIntegrityPolicyEnforcementMode")],
                DetectOps = [RegOp.CheckDword(DgKey, "UsermodeCodeIntegrityPolicyEnforcementMode", 1)],
            },
            new TweakDef
            {
                Id = "wdacapp-enable-credential-guard",
                Label = "WDAC: Enable Credential Guard via Virtualization-Based Security",
                Category = "Code Integrity Application Policy",
                Description =
                    "Sets LsaCfgFlags=1 in DeviceGuard policy. Enables Credential Guard via Virtualization-Based Security (VBS). Credential Guard stores NTLM hashes and Kerberos Ticket Granting Tickets (TGTs) inside an isolated secure virtual machine backed by the CPU hypervisor. From outside the secure VM, no code — not even kernel code — can read these credentials. Without Credential Guard, NTLM hashes and Kerberos tickets stored in LSASS can be extracted by Mimikatz or similar tools, enabling Pass-the-Hash and Pass-the-Ticket attacks across the entire domain. Credential Guard eliminates these credential theft vectors for credentials stored in LSASS.",
                Tags = ["credential-guard", "vbs", "lsass", "pass-the-hash", "kerberos"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "NTLM hashes and Kerberos TGTs are isolated in a secure VM. Mimikatz and LSASS dump attacks cannot extract credentials from the secure enclave. Requires hardware-enforced virtualisation (SLAT). Some legacy authentication protocols (NTLMv1) and smart card configurations may be incompatible — test thoroughly in a lab before deployment.",
                ApplyOps = [RegOp.SetDword(DgKey, "LsaCfgFlags", 1)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "LsaCfgFlags")],
                DetectOps = [RegOp.CheckDword(DgKey, "LsaCfgFlags", 1)],
            },
            new TweakDef
            {
                Id = "wdacapp-enable-vbs",
                Label = "WDAC: Enable Virtualization-Based Security (VBS)",
                Category = "Code Integrity Application Policy",
                Description =
                    "Sets EnableVirtualizationBasedSecurity=1 in DeviceGuard policy. Enables Virtualization-Based Security (VBS) which creates an isolated hypervisor partition (Virtual Secure Mode) for protecting security-sensitive OS components. VBS is the foundation that HVCI and Credential Guard require to operate. Without VBS enabled, neither HVCI nor Credential Guard can function — they will be silently disabled even if their individual GPO keys are set. VBS requires hardware that supports SLAT (Second Level Address Translation), DEP, IOMMU, and Secure Boot. Modern CPUs and UEFI firmware universally support this.",
                Tags = ["vbs", "virtual-secure-mode", "hypervisor", "foundation", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "VBS (Virtual Secure Mode) is enabled. This is the prerequisite for Credential Guard and HVCI. Requires SLAT, IOMMU, and Secure Boot. 3–10% memory overhead from hypervisor page table isolation. Incompatible with some nested virtualisation scenarios (running a Type-2 hypervisor inside a VBS-enabled environment).",
                ApplyOps = [RegOp.SetDword(DgKey, "EnableVirtualizationBasedSecurity", 1)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "EnableVirtualizationBasedSecurity")],
                DetectOps = [RegOp.CheckDword(DgKey, "EnableVirtualizationBasedSecurity", 1)],
            },
            new TweakDef
            {
                Id = "wdacapp-require-uefi-secure-firmware-for-vbs",
                Label = "WDAC: Require UEFI Firmware Lock for VBS (Platform Security Level 3)",
                Category = "Code Integrity Application Policy",
                Description =
                    "Sets RequirePlatformSecurityFeatures=3 in DeviceGuard policy. Sets the Required Platform Security Features flag to level 3 (Secure Boot + DMA protection required). Level 3 requires both UEFI Secure Boot (to ensure VBS is not bypassed by booting a modified OS) AND IOMMU DMA protection (to prevent DMA attacks using rogue PCIe devices that could read VBS-protected memory regions). Without DMA protection, a rogue Thunderbolt/PCIe device can bypass the hypervisor memory isolation and read Credential Guard secrets from the physical RAM.",
                Tags = ["vbs", "platform-security", "dma-protection", "iommu", "thunderbolt"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "VBS requires Secure Boot AND IOMMU DMA protection (platform security level 3). Devices without IOMMU or with disabled Thunderbolt DMA protection cannot enable VBS at this level. Required for full protection against DMA-based attacks on VBS-protected memory. Devices without these capabilities silently fail to enable VBS.",
                ApplyOps = [RegOp.SetDword(DgKey, "RequirePlatformSecurityFeatures", 3)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "RequirePlatformSecurityFeatures")],
                DetectOps = [RegOp.CheckDword(DgKey, "RequirePlatformSecurityFeatures", 3)],
            },
            new TweakDef
            {
                Id = "wdacapp-enable-srp-exe-control",
                Label = "WDAC: Enable Software Restriction Policies for Executable Control",
                Category = "Code Integrity Application Policy",
                Description =
                    "Sets DefaultLevel=0 in SrpV2 policy. Configures Software Restriction Policies (SRP) to Disallowed mode for executable types not explicitly whitelisted. SRP is the compatibility-layer predecessor to AppLocker and WDAC — it operates as a ring-3 policy enforcement mechanism. In Disallowed mode, all executables are blocked unless a rule explicitly permits them. While WDAC is preferred for modern deployments, SRP provides a fallback enforcement layer for scenarios where WDAC policy is not yet in place or for downlevel OS compatibility within a mixed fleet.",
                Tags = ["srp", "software-restriction", "disallowed-mode", "application-control", "whitelisting"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote =
                    "Software Restriction Policies set to Disallowed by default. All executables are blocked unless explicitly whitelisted by path, hash, or certificate rules. SRP is user-mode only — a kernel exploit bypasses it. This is a complementary, not a replacement, control to WDAC/AppLocker. Extensive whitelisting of legitimate software is required before deploying.",
                ApplyOps = [RegOp.SetDword(SrpKey, "DefaultLevel", 0)],
                RemoveOps = [RegOp.DeleteValue(SrpKey, "DefaultLevel")],
                DetectOps = [RegOp.CheckDword(SrpKey, "DefaultLevel", 0)],
            },
            new TweakDef
            {
                Id = "wdacapp-enable-wdac-policy-refresh",
                Label = "WDAC: Enable Policy Refresh for WDAC Code Integrity Rules",
                Category = "Code Integrity Application Policy",
                Description =
                    "Sets EnablePolicyRefresh=1 in DeviceGuard policy. Enables the ability to refresh WDAC code integrity policies at runtime without rebooting. Policy refresh allows administrators to push updated WDAC policy files to devices and have the new rules take effect immediately for newly spawned processes, without requiring the device to restart. Without policy refresh, every WDAC policy update requires a reboot — making policy iteration and incident response much more disruptive in production environments. Refresh is a key operational enabler for WDAC managed environments.",
                Tags = ["wdac", "policy-refresh", "runtime-update", "no-reboot", "operations"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "WDAC policies can be refreshed at runtime without reboot. Updated rules apply to newly launched processes immediately. Running processes are not affected by the refresh until they restart. Requires deploying the new policy file via MDM or Group Policy file copy before triggering refresh.",
                ApplyOps = [RegOp.SetDword(DgKey, "EnablePolicyRefresh", 1)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "EnablePolicyRefresh")],
                DetectOps = [RegOp.CheckDword(DgKey, "EnablePolicyRefresh", 1)],
            },
            new TweakDef
            {
                Id = "wdacapp-enable-ci-audit-event-logging",
                Label = "WDAC: Enable Code Integrity Audit Event Logging",
                Category = "Code Integrity Application Policy",
                Description =
                    "Sets AuditCodeIntegrityPolicyEnabled=1 in DeviceGuard policy. Enables audit event logging for Code Integrity policy violations in audit mode. When a WDAC policy is in audit mode (not enforcement mode), code that would have been blocked is logged as an audit event in the Microsoft-Windows-CodeIntegrity/Operational event log. These events include the binary path, the hash, the signing information, and why the binary would have been blocked. Audit events are essential for building the allow-list before switching to enforcement mode — production traffic can be captured and used to build an accurate whitelist.",
                Tags = ["wdac", "audit-mode", "event-logging", "code-integrity", "allow-list-building"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Code Integrity policy violations are logged (not blocked). Events written to Microsoft-Windows-CodeIntegrity/Operational channel. Use audit logs to identify all unsigned or untrusted binaries before switching to enforcement. Usually run in audit mode for 30–90 days to capture all legitimate software.",
                ApplyOps = [RegOp.SetDword(DgKey, "AuditCodeIntegrityPolicyEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "AuditCodeIntegrityPolicyEnabled")],
                DetectOps = [RegOp.CheckDword(DgKey, "AuditCodeIntegrityPolicyEnabled", 1)],
            },
            new TweakDef
            {
                Id = "wdacapp-block-vulnerable-driver-loading",
                Label = "WDAC: Enable Vulnerable Driver Blocklist via HVCI",
                Category = "Code Integrity Application Policy",
                Description =
                    "Sets MicrosoftVulnerableDriverBlocklistEnabled=1 in DeviceGuard policy. Enables the Microsoft-maintained Vulnerable Driver Blocklist, which is a WDAC policy that prevents known WHQL-signed but vulnerable kernel drivers from loading. Attackers use BYOVD (Bring Your Own Vulnerable Driver) attacks where they load a legitimately signed but exploitable kernel driver and then use its vulnerabilities to escalate to ring-0 and bypass HVCI. The blocklist is updated by Microsoft with newly discovered vulnerable drivers and is applied at the hypervisor layer when HVCI is active.",
                Tags = ["vulnerable-driver", "byovd", "hvci", "blocklist", "kernel"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "Microsoft's vulnerable driver blocklist is enforced. Known exploitable WHQL drivers are blocked. LKD (Legitimate but Vulnerable Driver) attacks are prevented. If your environment legitimately requires a driver that appears on the blocklist, you must add an explicit allow rule. Blocklist is updated via Windows Update.",
                ApplyOps = [RegOp.SetDword(DgKey, "MicrosoftVulnerableDriverBlocklistEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "MicrosoftVulnerableDriverBlocklistEnabled")],
                DetectOps = [RegOp.CheckDword(DgKey, "MicrosoftVulnerableDriverBlocklistEnabled", 1)],
            },
            new TweakDef
            {
                Id = "wdacapp-enable-smart-app-control-evaluation",
                Label = "WDAC: Enable Smart App Control Evaluation Mode",
                Category = "Code Integrity Application Policy",
                Description =
                    "Sets SmartAppControlState=2 in DeviceGuard policy. Sets Smart App Control (SAC) to evaluation mode. SAC uses an AI-based cloud intelligence service combined with WDAC to block malware and potentially unwanted applications without requiring a pre-configured policy. In evaluation mode, SAC silently evaluates whether enforcement mode is feasible without disrupting existing workflows — if no legitimate app blocking would occur, it transitions to enforcement mode automatically. Value 2 = evaluation, 1 = enforcement, 0 = off. Evaluation mode is safe to enable on existing devices without the risk of blocking legitimate software.",
                Tags = ["smart-app-control", "sac", "ai", "evaluation-mode", "malware-prevention"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Smart App Control enters evaluation mode. No apps are blocked during evaluation. The AI model evaluates whether full enforcement would cause issues. If no blocking would occur, SAC automatically transitions to enforcement. If issues are detected, SAC is turned off. Evaluation mode processes telemetry to the Microsoft cloud service.",
                ApplyOps = [RegOp.SetDword(DgKey, "SmartAppControlState", 2)],
                RemoveOps = [RegOp.DeleteValue(DgKey, "SmartAppControlState")],
                DetectOps = [RegOp.CheckDword(DgKey, "SmartAppControlState", 2)],
            },
        ];
}
