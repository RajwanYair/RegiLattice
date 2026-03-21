// RegiLattice.Core — Tweaks/AppLockerWdac.cs
// AppLocker and Windows Defender Application Control (WDAC) policy controls.
// Slug: "apl" — application allowlisting and code integrity policy.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AppLockerWdac
{
    private const string CIPol = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Config";
    private const string AplPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2";
    private const string WdacPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Policy Manager";
    private const string DevGuardPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard";
    private const string AplAppx = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SrpV2\Appx";
    private const string SiPol = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Policy";
    private const string HvciPol = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "apl-enable-wdac-event-logging",
            Label = "Enable WDAC / Code Integrity Operational Event Log",
            Category = "AppLocker & WDAC",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["wdac", "code integrity", "security", "audit", "event log"],
            Description =
                "Enables the Microsoft-Windows-CodeIntegrity/Operational event channel. "
                + "Records every signature validation decision (pass/fail) made by the kernel Code Integrity module. "
                + "Essential for monitoring WDAC policy effectiveness and investigating block events.",
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WINEVT\Channels\Microsoft-Windows-CodeIntegrity/Operational",
                    "Enabled",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WINEVT\Channels\Microsoft-Windows-CodeIntegrity/Operational",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WINEVT\Channels\Microsoft-Windows-CodeIntegrity/Operational",
                    "Enabled",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "apl-block-vulnerable-driver-list",
            Label = "Enable Microsoft Vulnerable Driver Blocklist",
            Category = "AppLocker & WDAC",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["wdac", "security", "driver", "blocklist", "kernel"],
            Description =
                "Activates the Microsoft-maintained list of drivers with known kernel vulnerabilities. "
                + "Prevents BYOVD (Bring Your Own Vulnerable Driver) attacks where attackers load known-buggy drivers to escalate privileges.",
            ApplyOps = [RegOp.SetDword(CIPol, "VulnerableDriverBlocklistEnable", 1)],
            RemoveOps = [RegOp.DeleteValue(CIPol, "VulnerableDriverBlocklistEnable")],
            DetectOps = [RegOp.CheckDword(CIPol, "VulnerableDriverBlocklistEnable", 1)],
        },
        new TweakDef
        {
            Id = "apl-enable-smart-app-control-policy",
            Label = "Enable Smart App Control in Evaluate Mode",
            Category = "AppLocker & WDAC",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["wdac", "smart app control", "security", "application control"],
            Description =
                "Sets Smart App Control to evaluation mode (2) which silently evaluates unsigned apps without blocking. "
                + "Allows learning phase before switching to enforcement. Only applicable on fresh Windows 11 installs.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Policy", "VerifiedAndReputablePolicyState", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Policy", "VerifiedAndReputablePolicyState")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Policy", "VerifiedAndReputablePolicyState", 2)],
        },
        new TweakDef
        {
            Id = "apl-enable-hvci-strict",
            Label = "Enable Hypervisor-Protected Code Integrity (HVCI) Strict Mode",
            Category = "AppLocker & WDAC",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["wdac", "hvci", "security", "hypervisor", "kernel"],
            Description =
                "Enables HVCI (Memory Integrity) in strict mode. Kernel-mode code must be signed and validated "
                + "by the hypervisor before execution, preventing unsigned kernel-mode rootkits and driver exploits.",
            ApplyOps =
            [
                RegOp.SetDword(HvciPol, "EnableVirtualizationBasedSecurity", 1),
                RegOp.SetDword(HvciPol, "HypervisorEnforcedCodeIntegrity", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(HvciPol, "EnableVirtualizationBasedSecurity"),
                RegOp.DeleteValue(HvciPol, "HypervisorEnforcedCodeIntegrity"),
            ],
            DetectOps = [RegOp.CheckDword(HvciPol, "HypervisorEnforcedCodeIntegrity", 2)],
        },
        new TweakDef
        {
            Id = "apl-enable-secured-core-vbs",
            Label = "Enable Virtualization-Based Security (VBS)",
            Category = "AppLocker & WDAC",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["vbs", "security", "virtualization", "hypervisor", "credential guard"],
            Description =
                "Enables Virtualization-Based Security to create an isolated hypervisor environment. "
                + "Required for Credential Guard and HVCI. Set to be required if available (VBS locked).",
            ApplyOps =
            [
                RegOp.SetDword(DevGuardPol, "EnableVirtualizationBasedSecurity", 1),
                RegOp.SetDword(DevGuardPol, "RequirePlatformSecurityFeatures", 3),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(DevGuardPol, "EnableVirtualizationBasedSecurity"),
                RegOp.DeleteValue(DevGuardPol, "RequirePlatformSecurityFeatures"),
            ],
            DetectOps = [RegOp.CheckDword(DevGuardPol, "EnableVirtualizationBasedSecurity", 1)],
        },
        new TweakDef
        {
            Id = "apl-enable-credential-guard",
            Label = "Enable Windows Credential Guard",
            Category = "AppLocker & WDAC",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["credential guard", "security", "lsa", "pass-the-hash", "vbs"],
            Description =
                "Enables Credential Guard which stores NTLM hashes and Kerberos tickets in a VBS-protected enclave, "
                + "preventing pass-the-hash and pass-the-ticket attacks even if the OS kernel is compromised.",
            ApplyOps = [RegOp.SetDword(DevGuardPol, "EnableVirtualizationBasedSecurity", 1), RegOp.SetDword(DevGuardPol, "LsaCfgFlags", 1)],
            RemoveOps = [RegOp.DeleteValue(DevGuardPol, "EnableVirtualizationBasedSecurity"), RegOp.DeleteValue(DevGuardPol, "LsaCfgFlags")],
            DetectOps = [RegOp.CheckDword(DevGuardPol, "LsaCfgFlags", 1)],
        },
        new TweakDef
        {
            Id = "apl-block-ms-store-unsigned-apps",
            Label = "Block Unsigned Apps from Non-Store Sources (GPO)",
            Category = "AppLocker & WDAC",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["applocker", "security", "unsigned", "policy", "gpo"],
            Description =
                "Uses a Group Policy path to enable SmartScreen enforcement mode, requiring apps from non-Store sources "
                + "to pass reputation check before execution. Complementary to AppLocker rules.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableSmartScreen", 2),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "ShellSmartScreenLevel", "Block"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableSmartScreen"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "ShellSmartScreenLevel"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableSmartScreen", 2)],
        },
        new TweakDef
        {
            Id = "apl-disable-auto-play-allowlisting",
            Label = "Disable AutoPlay for Non-Listed Devices",
            Category = "AppLocker & WDAC",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["applocker", "security", "autoplay", "usb", "policy"],
            Description =
                "Prevents AutoPlay from running executables on removable media that aren't in the trusted device list. "
                + "Stops USB-based malware from auto-executing when inserted.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoAutoplayfornonVolume", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoAutoplayfornonVolume")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoAutoplayfornonVolume", 1)],
        },
        new TweakDef
        {
            Id = "apl-enable-lsa-protected-process",
            Label = "Enable LSA Protected Process Light (PPL)",
            Category = "AppLocker & WDAC",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["lsa", "security", "credential", "protected process", "anti-tamper"],
            Description =
                "Runs the Local Security Authority (lsass.exe) as a Protected Process Light. "
                + "Prevents unauthorized processes (including admin-level tools like Mimikatz) from reading memory of the credential store.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPL", 1)],
        },
        new TweakDef
        {
            Id = "apl-disable-office-macro-execution",
            Label = "Block Office Macro Execution from Internet-Origin Files",
            Category = "AppLocker & WDAC",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["office", "macro", "security", "applocker", "wdac", "policy"],
            Description =
                "Applies Group Policy to block VBA macros in Office documents downloaded from the internet. "
                + "Closes one of the most common malware delivery vectors (phishing attachments with malicious macros).",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\excel\security", "blockcontentexecutionfrominternet", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\excel\security", "blockcontentexecutionfrominternet"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\excel\security",
                    "blockcontentexecutionfrominternet",
                    1
                ),
            ],
        },
    ];
}
