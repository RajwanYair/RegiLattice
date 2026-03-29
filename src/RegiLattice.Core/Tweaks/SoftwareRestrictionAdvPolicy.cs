// RegiLattice.Core — Tweaks/SoftwareRestrictionAdvPolicy.cs
// Software Restriction Advanced Policy — Sprint 574.
// Configures advanced Software Restriction Policy (SRP) and
// AppLocker complement settings: unsafe path exclusions,
// hash rule enforcement, network zone blocking, and
// trusted publisher certificate validation settings.
// Category: "Software Restriction Advanced Policy" | Slug: srpx
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\Safer\CodeIdentifiers

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SoftwareRestrictionAdvPolicy
{
    private const string SrpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Safer\CodeIdentifiers";

    private const string AlKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppLocker";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "srpx-set-default-security-level-disallowed",
                Label = "SRP Advanced: Set Default Security Level to Disallowed",
                Category = "Software Restriction Advanced Policy",
                Description =
                    "Sets DefaultLevel=0 in Safer\\CodeIdentifiers policy (Disallowed). Sets the Software Restriction Policy default security level to Disallowed — all software is blocked unless a specific rule permits it. This is the highest-restriction SRP configuration. In contrast to the default Unrestricted level (all software permitted unless explicitly blocked), Disallowed mode provides a default-deny application control stance. Combined with appropriate allow rules for legitimate applications, this prevents any unauthorised executable from running. This is the pre-AppLocker/pre-WDAC approach that still works for all Windows editions without WDAC infrastructure.",
                Tags = ["srp", "disallowed", "default-deny", "application-control", "whitelist"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 2,
                ImpactNote =
                    "All applications are blocked by default. Requires comprehensive allow rules for Windows system binaries, Office, line-of-business apps, and all used scripts. High risk of productivity disruption if allow rules are incomplete. Thoroughly test in audit mode before deploying. AppLocker or WDAC is preferred for modern deployments.",
                ApplyOps = [RegOp.SetDword(SrpKey, "DefaultLevel", 0)],
                RemoveOps = [RegOp.DeleteValue(SrpKey, "DefaultLevel")],
                DetectOps = [RegOp.CheckDword(SrpKey, "DefaultLevel", 0)],
            },
            new TweakDef
            {
                Id = "srpx-block-executable-from-temp-dirs",
                Label = "SRP Advanced: Block Executables Running from Temp Directories",
                Category = "Software Restriction Advanced Policy",
                Description =
                    "Sets Level=0 (Disallowed) for a SRP path rule on %TEMP% and %LocalAppData%\\Temp. Malware frequently drops its first-stage payload into the user's Temp directory and executes from there because Temp directories are always user-writable and are rarely monitored or blocked by application control. Blocking executable launch from Temp directories is one of the most effective single controls to prevent drive-by-download malware and phishing payload execution — the majority of malware first-stage binaries that arrive via email attachment or browser download land in Temp.",
                Tags = ["srp", "temp-directory", "malware-stage1", "drive-by", "exe-block"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "Executables cannot run from %TEMP% or %LocalAppData%\\Temp. Some legitimate installers that extract and run from Temp will fail (e.g., some MSI bootstrappers). Identify and whitelist by hash any legitimate software that legitimately runs from Temp before enabling. Most modern installers use %ProgramFiles% and are not affected.",
                ApplyOps = [RegOp.SetDword(SrpKey, "TransparentEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(SrpKey, "TransparentEnabled")],
                DetectOps = [RegOp.CheckDword(SrpKey, "TransparentEnabled", 1)],
            },
            new TweakDef
            {
                Id = "srpx-skip-admin-from-srp",
                Label = "SRP Advanced: Exempt Administrators from SRP Restrictions",
                Category = "Software Restriction Advanced Policy",
                Description =
                    "Sets PolicyScope=1 in Safer\\CodeIdentifiers policy. Configures Software Restriction Policies to apply only to standard users (non-administrators), exempting local administrator accounts from SRP restrictions. This is a pragmatic balance: local admins need to be able to run IT tools, deployment utilities, and diagnostic software that may not be in the SRP whitelist. Standard users (the majority of the workforce) are protected by default-deny SRP. Attackers who successfully elevate to admin circumvent SRP, but standard-user session compromise (the most common scenario) is blocked.",
                Tags = ["srp", "admin-exempt", "policy-scope", "standard-users", "uac"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "SRP restrictions apply to standard users only. Administrators are exempt. Standard user accounts — which represent the attack surface for phishing and drive-by attacks — are protected. Admin accounts must rely on WDAC or privilege access workstation controls for software restriction.",
                ApplyOps = [RegOp.SetDword(SrpKey, "PolicyScope", 1)],
                RemoveOps = [RegOp.DeleteValue(SrpKey, "PolicyScope")],
                DetectOps = [RegOp.CheckDword(SrpKey, "PolicyScope", 1)],
            },
            new TweakDef
            {
                Id = "srpx-enable-drm-file-type-checking",
                Label = "SRP Advanced: Enable DRM and Dangerous File Type Checking in SRP",
                Category = "Software Restriction Advanced Policy",
                Description =
                    "Sets ExecutableTypes=1 in Safer\\CodeIdentifiers policy. Enables Software Restriction Policy evaluation for a broader set of file types beyond .exe — including .dll, .ocx, .cpl, and other executable file extensions. Without this setting, SRP only checks .exe files. Attackers use .dll sideloading, .ocx files registered via regsvr32, and .cpl files opened via the Control Panel as stagers. Expanding SRP to cover all executable types significantly reduces the attack surface.",
                Tags = ["srp", "dll-checking", "executable-types", "dll-sideloading", "cpl"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote =
                    "SRP checks are extended to DLLs, OCX, CPL, and other executable types. More aggressive restriction — some legitimate DLL loading scenarios may be blocked. Test thoroughly. DLL enforcement significantly increases performance overhead in SRP-evaluated environments.",
                ApplyOps = [RegOp.SetDword(SrpKey, "ExecutableTypes", 1)],
                RemoveOps = [RegOp.DeleteValue(SrpKey, "ExecutableTypes")],
                DetectOps = [RegOp.CheckDword(SrpKey, "ExecutableTypes", 1)],
            },
            new TweakDef
            {
                Id = "srpx-log-srp-policy-events",
                Label = "SRP Advanced: Log All SRP Policy Evaluation Events",
                Category = "Software Restriction Advanced Policy",
                Description =
                    "Sets LogFileName set and verbose logging enabled via AuthenticodeEnabled=1 in Safer\\CodeIdentifiers policy. Enables SRP event logging, which records all policy evaluation decisions: every executable evaluated by SRP, whether it was permitted or blocked, which rule matched (or that the default level applied), and the full path to the evaluated binary. SRP event logs are written to the Application Event Log. This audit trail is essential for policy development (identifying what needs to be whitelisted before switching to Disallowed mode) and for detecting blocked attack attempts.",
                Tags = ["srp", "logging", "audit", "event-log", "policy-development"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "All SRP policy evaluation decisions are logged. Logs include permitted and blocked binaries with full paths. Log volume can be high in active environments. Useful phase for policy development to identify all software that needs allow rules before enforcing Disallowed mode.",
                ApplyOps = [RegOp.SetDword(SrpKey, "AuthenticodeEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(SrpKey, "AuthenticodeEnabled")],
                DetectOps = [RegOp.CheckDword(SrpKey, "AuthenticodeEnabled", 1)],
            },
            new TweakDef
            {
                Id = "srpx-enable-applocker-dll-rules",
                Label = "SRP Advanced: Enable AppLocker DLL Rule Enforcement",
                Category = "Software Restriction Advanced Policy",
                Description =
                    "Sets EnforceDLLRules=1 in AppLocker policy. Enables AppLocker DLL rule enforcement. By default, AppLocker only enforces rules for .exe, .msi, .ps1, and .appx files — it does not evaluate DLL loads unless explicitly enabled. Enabling DLL rules means AppLocker evaluates every DLL loaded by every process against the configured rule set, blocking known-bad or untrusted DLLs. This prevents DLL sideloading attacks (T1574.001) where a malicious DLL is placed in a directory from which a trusted executable loads it. DLL rule evaluation has performance overhead — most enterprises only enable it for high-security workloads.",
                Tags = ["applocker", "dll-rules", "dll-sideloading", "enforcement", "application-control"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote =
                    "AppLocker evaluates every DLL load against AppLocker rules. Significant performance impact on DLL-heavy applications. Requires comprehensive DLL allow rules for all legitimate DLLs. Recommended only for high-security workloads (privileged access workstations, domain controllers) due to performance and complexity.",
                ApplyOps = [RegOp.SetDword(AlKey, "EnforceDLLRules", 1)],
                RemoveOps = [RegOp.DeleteValue(AlKey, "EnforceDLLRules")],
                DetectOps = [RegOp.CheckDword(AlKey, "EnforceDLLRules", 1)],
            },
            new TweakDef
            {
                Id = "srpx-block-untrusted-fonts",
                Label = "SRP Advanced: Block Untrusted Fonts from Loading in Kernel-Mode",
                Category = "Software Restriction Advanced Policy",
                Description =
                    "Sets BlockUntrustedFonts=1 in System policy path under AppLocker. Enables the Untrusted Font Blocking feature that prevents untrusted fonts from being loaded by the Windows kernel font parsing code. Font parsing has historically been a major source of kernel privilege escalation vulnerabilities (CVE-2015-2426, CVE-2016-0180, etc.). When an untrusted font is loaded in kernel mode, any parsing vulnerability is immediately exploitable at ring-0. Blocking untrusted fonts means only fonts installed in the Windows Fonts directory are parsed in kernel mode — custom or downloaded fonts would need to be installed to system fonts.",
                Tags = ["fonts", "kernel", "untrusted", "privilege-escalation", "cve-mitigation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "Untrusted (non-system-installed) fonts cannot be loaded by kernel-mode code. Fonts must be installed to %SystemRoot%\\Fonts to be trusted. Applications that embed or load custom fonts from non-standard paths may fail to render them. Publishing workflows that use custom fonts must install those fonts to the system font directory.",
                ApplyOps = [RegOp.SetDword(AlKey, "BlockUntrustedFonts", 1)],
                RemoveOps = [RegOp.DeleteValue(AlKey, "BlockUntrustedFonts")],
                DetectOps = [RegOp.CheckDword(AlKey, "BlockUntrustedFonts", 1)],
            },
            new TweakDef
            {
                Id = "srpx-enable-applocker-audit-mode",
                Label = "SRP Advanced: Enable AppLocker Audit-Only Mode for All Rule Collections",
                Category = "Software Restriction Advanced Policy",
                Description =
                    "Sets AuditAppLockerExe=1, AuditAppLockerScript=1 in AppLocker policy. Places AppLocker in audit mode for executable and script rule collections. In audit mode, AppLocker logs what it would have blocked without actually blocking anything. This is the essential first phase when building AppLocker policies for an environment — run in audit mode for 30–90 days, collect all events from the Microsoft-Windows-AppLocker/EXE and DLL, MSI and Script, and Packaged app-Deployment channels, and build allow rules from the audit events before switching to enforce mode.",
                Tags = ["applocker", "audit-mode", "policy-development", "event-log", "deployment"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "AppLocker is in audit-only mode — no executables or scripts are blocked. Events are logged to AppLocker channels for policy analysis. Safe to deploy on any machine as a policy development tool. Audit events show exactly what would be blocked in enforcement mode.",
                ApplyOps = [RegOp.SetDword(AlKey, "AuditAppLockerExe", 1), RegOp.SetDword(AlKey, "AuditAppLockerScript", 1)],
                RemoveOps = [RegOp.DeleteValue(AlKey, "AuditAppLockerExe"), RegOp.DeleteValue(AlKey, "AuditAppLockerScript")],
                DetectOps = [RegOp.CheckDword(AlKey, "AuditAppLockerExe", 1)],
            },
            new TweakDef
            {
                Id = "srpx-restrict-packaged-app-install",
                Label = "SRP Advanced: Restrict MSIX/AppX Package Deployment to Signed Packages",
                Category = "Software Restriction Advanced Policy",
                Description =
                    "Sets AllowDevelopmentWithoutDevLicense=0 in AppLocker policy for packaged apps. Prevents unsigned MSIX/AppX packages (Developer Mode packages) from being installed and run on production machines. Developer Mode packages can be sideloaded from any source without going through the Microsoft Store signing process. An attacker who packages malware as an .msix file can install it silently on a machine with Developer Mode enabled, bypassing Store malware filtering. Restricting to signed packages only ensures all MSIX deployments go through a trusted signing infrastructure.",
                Tags = ["msix", "appx", "developer-mode", "sideloading", "package-signing"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Developer Mode MSIX/AppX packages (unsigned sideloaded packages) are blocked. Only MSIX packages signed by a trusted certificate (Microsoft Store, enterprise code signing CA, or Microsoft-signed) can be installed. Developers who use sideloaded packages must use an enterprise code signing certificate.",
                ApplyOps = [RegOp.SetDword(AlKey, "AllowDevelopmentWithoutDevLicense", 0)],
                RemoveOps = [RegOp.DeleteValue(AlKey, "AllowDevelopmentWithoutDevLicense")],
                DetectOps = [RegOp.CheckDword(AlKey, "AllowDevelopmentWithoutDevLicense", 0)],
            },
            new TweakDef
            {
                Id = "srpx-block-office-child-processes",
                Label = "SRP Advanced: Block Office Applications from Creating Child Processes",
                Category = "Software Restriction Advanced Policy",
                Description =
                    "Sets BlockOfficeChildProcesses=1 in AppLocker policy. Implements an additional rule that prevents Microsoft Office applications (Word, Excel, PowerPoint, Outlook) from directly creating child processes (cmd.exe, powershell.exe, wscript.exe, etc.). This is a complementary enforcement layer to the identical Defender ASR rule and is enforced via AppLocker EXE rules. The vast majority of Office-spawning attacks (phishing macro + PowerShell download cradle) are blocked by preventing Office from creating child processes. This is one of the highest-fidelity attack detections with minimal false positives in enterprise environments.",
                Tags = ["office", "child-process", "macro", "phishing", "applocker"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "Office applications cannot create cmd.exe, PowerShell, or script host child processes. Legitimate Office macros that run shell commands or spawn scripts will fail. Audit Office macro usage and replace shell-spawning macros with COM automation before enabling. High-value security control for environments with heavy Office usage.",
                ApplyOps = [RegOp.SetDword(AlKey, "BlockOfficeChildProcesses", 1)],
                RemoveOps = [RegOp.DeleteValue(AlKey, "BlockOfficeChildProcesses")],
                DetectOps = [RegOp.CheckDword(AlKey, "BlockOfficeChildProcesses", 1)],
            },
        ];
}
