// RegiLattice.Core — Tweaks/DataIntegrityPolicy.cs
// Sprint 304: Data Integrity Policy tweaks (10 tweaks)
// Category: "Data Integrity Policy" | Slug: dataintg
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataIntegrity

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DataIntegrityPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataIntegrity";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "dataintg-enable-integrity-checks",
            Label = "Enable System Data Integrity Checks",
            Category = "Data Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "System data integrity checks verify that critical Windows system files and data structures have not been modified from their expected state. Enabling integrity checks activates continuous monitoring of protected system components for unauthorized modification. Tampered system files are a hallmark of advanced persistent threat attacks seeking to establish persistent footholds. Integrity verification provides early detection of rootkits, bootkit infections, and file system tampering. Enterprise security operations should monitor integrity check violations as high-priority security events. Enabling integrity checks has a minor performance impact but provides substantial assurance against stealthy compromise scenarios.",
            Tags = ["integrity", "security", "system", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableIntegrityChecks", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableIntegrityChecks")],
            DetectOps = [RegOp.CheckDword(Key, "EnableIntegrityChecks", 1)],
        },
        new TweakDef
        {
            Id = "dataintg-enable-runtime-verification",
            Label = "Enable Runtime Data Integrity Verification",
            Category = "Data Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Runtime data integrity verification continuously monitors in-memory data structures critical to system security against modification. Enabling runtime verification detects attempts to tamper with security-critical data while it resides in memory. In-memory attacks that modify security tokens, ACLs, or kernel data structures are a sophisticated evasion technique. Runtime integrity monitoring catches these attacks before they can cause persistent damage or expand privilege. Security logging of runtime integrity violations enables the SOC to identify attacker actions against memory structures. The performance overhead of runtime verification is acceptable given the significant detection capability it provides.",
            Tags = ["integrity", "runtime", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableRuntimeVerification", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableRuntimeVerification")],
            DetectOps = [RegOp.CheckDword(Key, "EnableRuntimeVerification", 1)],
        },
        new TweakDef
        {
            Id = "dataintg-disable-bypass",
            Label = "Disable Data Integrity Check Bypass",
            Category = "Data Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Data integrity check bypass mechanisms allow exceptions to be granted for files or components that fail integrity verification. Disabling bypass mechanisms ensures that integrity failures are enforced without exception and cannot be overridden. Bypass mechanisms intended as temporary compatibility workarounds may be abused by attackers to suppress detection of tampering. A strict no-bypass enforcement model maximizes the effectiveness of integrity checking as a detective and preventive control. Legitimate software compatibility issues should be resolved through proper code signing and integrity manifest maintenance. Removing bypass paths eliminates a potential attacker technique for silencing integrity monitoring while maintaining a compromised system state.",
            Tags = ["integrity", "bypass", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableIntegrityBypass", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableIntegrityBypass")],
            DetectOps = [RegOp.CheckDword(Key, "DisableIntegrityBypass", 1)],
        },
        new TweakDef
        {
            Id = "dataintg-enable-audit-logging",
            Label = "Enable Data Integrity Audit Logging",
            Category = "Data Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Data integrity audit logging records integrity check results and any violations to the Windows Security event log. Enabling audit logging ensures that all integrity check failures are captured as auditable events for SOC investigation. Integrity violation events contain information about the modified component, modification type, and associated process. Security operations can use these events to identify compromise scope and timeline during incident response. Audit log entries for integrity violations should trigger high-priority alerts in SIEM systems. Enabling audit logging has negligible performance impact and is essential for maintaining a complete security audit record.",
            Tags = ["integrity", "logging", "audit", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableIntegrityAuditLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableIntegrityAuditLogging")],
            DetectOps = [RegOp.CheckDword(Key, "EnableIntegrityAuditLogging", 1)],
        },
        new TweakDef
        {
            Id = "dataintg-enable-boot-verification",
            Label = "Enable Boot-Time Data Integrity Verification",
            Category = "Data Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Boot-time data integrity verification checks the integrity of the Windows boot chain components before the operating system fully initializes. Enabling boot verification detects modifications to the bootloader, kernel, and early-launch drivers before they have a chance to execute. Bootkits that compromise the boot process are among the most difficult malware types to detect and remediate. Early detection during the boot phase enables recovery mechanisms to quarantine or remediate compromised components. Secure Boot and ELAM work together with boot-time integrity verification to create a chain of trust from firmware to OS. Boot-time verification results influence the security posture assessment used by attestation and device health services.",
            Tags = ["integrity", "boot", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableBootVerification", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableBootVerification")],
            DetectOps = [RegOp.CheckDword(Key, "EnableBootVerification", 1)],
        },
        new TweakDef
        {
            Id = "dataintg-enable-usermode-checks",
            Label = "Enable User-Mode Data Integrity Checks",
            Category = "Data Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "User-mode integrity checks verify the integrity of user-space process images and libraries against expected hash values. Enabling user-mode integrity verification detects attempts to tamper with application binaries and shared libraries on the filesystem. In-memory patching and DLL replacement attacks target user-mode processes to hijack application execution. User-mode integrity checking provides a complementary layer to kernel-mode protections. Enterprise endpoint protection requires integrity verification across both kernel and user spaces for comprehensive coverage. Suspicious user-mode integrity failures should be logged and acted upon as potential signs of malicious persistence mechanisms.",
            Tags = ["integrity", "user-mode", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableUserModeIntegrityChecks", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableUserModeIntegrityChecks")],
            DetectOps = [RegOp.CheckDword(Key, "EnableUserModeIntegrityChecks", 1)],
        },
        new TweakDef
        {
            Id = "dataintg-disable-rollback",
            Label = "Disable Data Integrity Version Rollback",
            Category = "Data Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Version rollback allows integrity-checked components to be replaced with older versions of the same component under certain conditions. Disabling version rollback prevents downgrade attacks that replace current patched versions of protected components with older vulnerable versions. Downgrade attacks are used to reintroduce known vulnerabilities that have been patched in current software versions. Attackers can use rollback capabilities to exploit historic CVEs in components where newer versions are not vulnerable. Anti-rollback enforcement ensures that the security improvement trajectory of patch deployment cannot be reversed. Legitimate downgrade requirements should be addressed through vendor support rather than disabling rollback controls.",
            Tags = ["integrity", "rollback", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableVersionRollback", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableVersionRollback")],
            DetectOps = [RegOp.CheckDword(Key, "DisableVersionRollback", 1)],
        },
        new TweakDef
        {
            Id = "dataintg-enable-hash-validation",
            Label = "Enable File Hash Validation Before Execution",
            Category = "Data Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "File hash validation computes and verifies the cryptographic hash of executable files against a known-good baseline before allowing execution. Enabling hash validation before execution creates a substantial barrier against execution of unauthorized or modified binaries. Changed file hashes indicate either unauthorized modification or dynamic loading of code not present in the original deployment. Hash validation adds execution latency proportional to file sizes and the number of files validated on each invocation. Enterprise environments maintaining a strict software allowlist can use hash validation to enforce the approved software inventory. Combining hash validation with code signing creates defense-in-depth for executable integrity assurance.",
            Tags = ["integrity", "hash", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableHashValidation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableHashValidation")],
            DetectOps = [RegOp.CheckDword(Key, "EnableHashValidation", 1)],
        },
        new TweakDef
        {
            Id = "dataintg-disable-telemetry",
            Label = "Disable Data Integrity Telemetry",
            Category = "Data Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Data integrity telemetry sends statistics about integrity check results, verification performance, and violation frequency to Microsoft. This data helps improve the data integrity infrastructure and identify systematic issues in the Windows ecosystem. Disabling integrity telemetry prevents check result data from being sent to Microsoft's analytics pipeline. Integrity check data revealing verification failures could expose sensitive information about the security state of enterprise systems. Security posture information should be shared through enterprise vulnerability management programs not consumer telemetry. Data integrity verification and enforcement continue to function normally with telemetry disabled.",
            Tags = ["integrity", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableIntegrityTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableIntegrityTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableIntegrityTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "dataintg-enforce-on-write",
            Label = "Enforce Data Integrity on File Write Operations",
            Category = "Data Integrity Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            Description =
                "Write-time integrity enforcement validates that files being written to protected locations meet integrity policy requirements before the write is committed. Enforcing integrity at write time prevents unauthorized modification of protected files from being persisted to disk. Post-write integrity scanning can miss write operations that occur between scan intervals allowing tampered files to remain. Write-time enforcement closes this window by blocking modification at write time rather than detecting it after the fact. Performance-sensitive workloads may experience write throughput reduction when write-time integrity checks are active. Critical system and security component directories are the highest priority targets for write-time integrity enforcement.",
            Tags = ["integrity", "write-protection", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceIntegrityOnWrite", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceIntegrityOnWrite")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceIntegrityOnWrite", 1)],
        },
    ];
}
