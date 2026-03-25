// RegiLattice.Core — Tweaks/CodeSigningPolicy.cs
// Sprint 319: Code Signing Policy tweaks (10 tweaks)
// Category: "Code Signing Policy" | Slug: codesign
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CodeSigning

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CodeSigningPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CodeSigning";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "codesign-require-signed-drivers",
            Label = "Require Signed Kernel Drivers",
            Category = "Code Signing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Kernel driver signing requirements prevent unsigned or improperly signed drivers from loading into the Windows kernel. Requiring signed drivers ensures that only code vetted and signed by Microsoft or authorized cross-signers can execute in kernel mode. Unsigned drivers are a primary attack vector for rootkits and persistent malware that need kernel-level access to hide their activity. Windows 10 and later systems with Secure Boot enabled enforce driver signing automatically but policy reinforcement provides additional protection. Driver signing has been mandatory for 64-bit Windows since Vista but third-party tools and older drivers may attempt to bypass this requirement. Enforcing driver signing through policy prevents test signing mode and signature validation bypasses.",
            Tags = ["codesigning", "drivers", "kernel", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireSignedDrivers", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireSignedDrivers")],
            DetectOps = [RegOp.CheckDword(Key, "RequireSignedDrivers", 1)],
        },
        new TweakDef
        {
            Id = "codesign-require-cross-cert-chain",
            Label = "Require Cross-Certificate Validation for Drivers",
            Category = "Code Signing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Cross-certificate chain validation requires that driver signatures trace back through a valid Microsoft-trusted cross-certificate hierarchy. Enabling cross-certificate requirements ensures that driver certificates issued by third-party CAs are chained through Microsoft's approved cross-certification program. Drivers signed with certificates not in the Microsoft cross-certificate program cannot be loaded even if the signature is technically valid. This policy prevents drivers signed by arbitrary commercial CAs or self-signed certificates from gaining kernel access. Cross-certificate validation is part of the Windows Hardware Quality Labs (WHQL) signing requirements for production drivers. Enforcing cross-certificate chains significantly reduces the attack surface for malicious kernel drivers attempting to use rogue or expired certificates.",
            Tags = ["codesigning", "cross-certificate", "drivers", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireCrossCertificatesChain", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireCrossCertificatesChain")],
            DetectOps = [RegOp.CheckDword(Key, "RequireCrossCertificatesChain", 1)],
        },
        new TweakDef
        {
            Id = "codesign-disable-test-signing",
            Label = "Block Test Signing Mode",
            Category = "Code Signing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Test signing mode allows unsigned or self-signed drivers to load for development purposes and is a significant security risk on production systems. Blocking test signing mode prevents users and malicious software from enabling bcdedit test signing which bypasses driver signature requirements. Attackers who gain administrator access can enable test signing to load malicious rootkits and drivers that would otherwise be blocked. Test signing mode is a BCD boot configuration option that can be set without UEFI Secure Boot being disabled. Policy enforcement of test signing restrictions prevents persistent configuration changes that would survive reboots. Production endpoints should never run in test signing mode and policy enforcement prevents accidental or malicious enablement.",
            Tags = ["codesigning", "test-signing", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockTestSigningMode", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockTestSigningMode")],
            DetectOps = [RegOp.CheckDword(Key, "BlockTestSigningMode", 1)],
        },
        new TweakDef
        {
            Id = "codesign-require-kernel-ehashes",
            Label = "Enable Enhanced Hash Algorithm for Driver Signing",
            Category = "Code Signing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Enhanced hash algorithms require driver signatures to use SHA-256 or stronger hash algorithms rather than the deprecated SHA-1. Enabling enhanced hash requirements ensures that driver signatures cannot be forged through SHA-1 collision attacks. SHA-1 has been cryptographically broken and certificates or signatures using SHA-1 should be considered untrusted in security-sensitive contexts. Windows has deprecated SHA-1 code signing certificates for drivers but policy enforcement ensures no SHA-1 signed drivers are accepted. Enhanced hash enforcement applies to both kernel-mode and user-mode driver components loaded during system operation. Transitioning entirely to SHA-256 or stronger hash algorithms for driver signing is best practice for all enterprise deployments.",
            Tags = ["codesigning", "sha256", "hash", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireEnhancedKeyHashes", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireEnhancedKeyHashes")],
            DetectOps = [RegOp.CheckDword(Key, "RequireEnhancedKeyHashes", 1)],
        },
        new TweakDef
        {
            Id = "codesign-enable-code-integrity-policy",
            Label = "Enable Code Integrity Policy Enforcement",
            Category = "Code Signing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Code integrity policy enforcement validates all kernel-mode code before execution against a policy that defines allowed code. Enabling code integrity enforcement prevents any code not matching the allowed code policy from executing in kernel mode. This forms the foundation of Windows HVCI and hypervisor-protected code integrity that protects the kernel from malicious drivers. Code integrity policies can be audit-only initially to identify violations before switching to enforcement mode. Policy-based code integrity is more flexible than simple driver signing as it can enforce specific file hashes and publisher identities. Code integrity policy combined with virtualization-based security provides the highest level of kernel protection available on Windows platforms.",
            Tags = ["codesigning", "integrity", "hvci", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableCodeIntegrityPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableCodeIntegrityPolicy")],
            DetectOps = [RegOp.CheckDword(Key, "EnableCodeIntegrityPolicy", 1)],
        },
        new TweakDef
        {
            Id = "codesign-block-vulnerable-drivers",
            Label = "Enable Microsoft Vulnerable Driver Blocklist",
            Category = "Code Signing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "The Microsoft Vulnerable Driver Blocklist prevents known vulnerable signed drivers from loading even though they have valid signatures. Enabling the blocklist protects against bring-your-own-vulnerable-driver attacks where attackers load signed but vulnerable drivers to escalate privileges. Signed drivers with known exploitable vulnerabilities have been used in numerous ransomware and APT attacks to bypass security software. The blocklist is maintained by Microsoft and updated through Windows updates to include newly discovered vulnerable drivers. Drivers on the blocklist include those with arbitrary kernel memory read/write capabilities, privilege escalation vulnerabilities, and security bypass functions. Enabling the vulnerable driver blocklist should be standard practice on all enterprise endpoints without compatibility exceptions.",
            Tags = ["codesigning", "blocklist", "drivers", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableVulnerableDriverBlocklist", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableVulnerableDriverBlocklist")],
            DetectOps = [RegOp.CheckDword(Key, "EnableVulnerableDriverBlocklist", 1)],
        },
        new TweakDef
        {
            Id = "codesign-require-signed-scripts",
            Label = "Require Signed Executable Scripts",
            Category = "Code Signing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Requiring signed executable scripts prevents malicious or unauthorized scripts from running in the Windows scripting environment. Script signing requirements apply to PowerShell scripts, batch files, and other executable script types depending on the script host configuration. Signed script requirements complement PowerShell execution policy but provide a broader policy mechanism applicable across script hosts. Unsigned scripts are a common delivery mechanism for malware, ransomware, and post-exploitation frameworks in enterprise attacks. Script signing tied to an enterprise PKI ensures that only IT-approved scripts can run on managed endpoints. Requiring signed scripts reduces the risk from phishing-delivered scripts and malicious script injections into legitimate directories.",
            Tags = ["codesigning", "scripts", "powershell", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireSignedScripts", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireSignedScripts")],
            DetectOps = [RegOp.CheckDword(Key, "RequireSignedScripts", 1)],
        },
        new TweakDef
        {
            Id = "codesign-enable-umci",
            Label = "Enable User Mode Code Integrity (UMCI)",
            Category = "Code Signing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            Description =
                "User Mode Code Integrity extends code integrity checking to user-mode processes requiring that all executable code be authorized by the code integrity policy. Enabling UMCI prevents execution of unauthorized binaries and DLLs in user space complementing kernel code integrity enforcement. UMCI is the user-mode component of Windows Defender Application Control and provides comprehensive protection against unauthorized code. User mode code integrity can prevent malicious DLL injection, unauthorized process creation, and execution of malware dropped by exploits. Device Guard in full lockdown mode combines HVCI with UMCI for both kernel and user mode protection. UMCI requires careful policy development to avoid blocking legitimate applications and may require an audit period before enforcement.",
            Tags = ["codesigning", "umci", "applocker", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableUMCI", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableUMCI")],
            DetectOps = [RegOp.CheckDword(Key, "EnableUMCI", 1)],
        },
        new TweakDef
        {
            Id = "codesign-block-dll-from-temp",
            Label = "Block Code Loading from Temporary Directories",
            Category = "Code Signing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Blocking code loading from temporary directories prevents malware dropped to TEMP locations from being executed through DLL side-loading or process injection. Code integrity policy rules can block executable and DLL loading from paths like %TEMP%, Downloads, and other writable user directories. Temporary directory-based execution is a hallmark of malware that avoids writing to monitored program directories. Attackers frequently exploit applications with DLL search order vulnerabilities to load malicious DLLs from the application directory or TEMP paths. Blocking code from temporary directories significantly reduces the attack surface for DLL hijacking and self-extracting malware delivery. This protection complements AppLocker and Windows Defender Application Control path-based rules targeting common malware staging locations.",
            Tags = ["codesigning", "temp", "dll", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockCodeFromTempPaths", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockCodeFromTempPaths")],
            DetectOps = [RegOp.CheckDword(Key, "BlockCodeFromTempPaths", 1)],
        },
        new TweakDef
        {
            Id = "codesign-audit-code-integrity",
            Label = "Enable Code Integrity Audit Logging",
            Category = "Code Signing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Code integrity audit logging records events whenever code is blocked or would have been blocked by code integrity policy in audit mode. Enabling code integrity audit events provides visibility into code integrity violations that can inform policy development and refinement. Audit logs identify applications, drivers, and scripts that would fail enforcement-mode code integrity to prepare for enforcement without disruption. Event ID 3076 and related code integrity events in the Microsoft-Windows-CodeIntegrity log provide detailed blocking information. Code integrity audit data should be collected to SIEM systems for correlation with other security events. Audit logging should always be enabled during the policy development phase before switching from audit to enforcement mode.",
            Tags = ["codesigning", "audit", "logging", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableAuditCodeIntegrity", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableAuditCodeIntegrity")],
            DetectOps = [RegOp.CheckDword(Key, "EnableAuditCodeIntegrity", 1)],
        },
    ];
}
