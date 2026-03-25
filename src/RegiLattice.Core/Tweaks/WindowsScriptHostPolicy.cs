#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 237 — Windows Script Host Policy (10 tweaks)
// All keys under HKLM\SOFTWARE\Policies\Microsoft\Windows Script Host\Settings
internal static class WindowsScriptHostPolicy
{
    private const string WshKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Script Host\Settings";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wsh-disable-wsh",
            Label = "Disable Windows Script Host",
            Category = "Windows Script Host Policy",
            Description = "Blocks all WSH-based script execution (VBScript, JScript, CScript, WScript).",
            Tags = ["script", "wsh", "security", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Prevents execution of .vbs/.js/.wsf scripts via WSH. May break legacy admin scripts.",
            ApplyOps = [RegOp.SetDword(WshKey, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(WshKey, "Enabled")],
            DetectOps = [RegOp.CheckDword(WshKey, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "wsh-disable-remote-scripts",
            Label = "Disable WSH Remote Script Execution",
            Category = "Windows Script Host Policy",
            Description = "Prevents WSH from executing scripts that originate from remote (network) locations.",
            Tags = ["script", "wsh", "remote", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks scripts run from UNC paths. Local scripts are unaffected.",
            ApplyOps = [RegOp.SetDword(WshKey, "Remote", 0)],
            RemoveOps = [RegOp.DeleteValue(WshKey, "Remote")],
            DetectOps = [RegOp.CheckDword(WshKey, "Remote", 0)],
        },
        new TweakDef
        {
            Id = "wsh-disable-trustedcert-bypass",
            Label = "Disable Trusted Certificate Script Bypass",
            Category = "Windows Script Host Policy",
            Description = "Prevents scripts with a trusted code-signing certificate from bypassing the WSH Enabled=0 restriction.",
            Tags = ["script", "wsh", "certificate", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ensures Enabled=0 applies universally regardless of script signing.",
            ApplyOps = [RegOp.SetDword(WshKey, "TrustPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(WshKey, "TrustPolicy")],
            DetectOps = [RegOp.CheckDword(WshKey, "TrustPolicy", 0)],
        },
        new TweakDef
        {
            Id = "wsh-disable-activex-in-scripts",
            Label = "Block ActiveX Objects in WSH Scripts",
            Category = "Windows Script Host Policy",
            Description = "Prevents WSH scripts from instantiating ActiveX/COM objects via CreateObject or GetObject.",
            Tags = ["script", "wsh", "activex", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Removes a common malware vector; may break legitimate admin scripts using WMI/ADSI.",
            ApplyOps = [RegOp.SetDword(WshKey, "ActiveXScript", 0)],
            RemoveOps = [RegOp.DeleteValue(WshKey, "ActiveXScript")],
            DetectOps = [RegOp.CheckDword(WshKey, "ActiveXScript", 0)],
        },
        new TweakDef
        {
            Id = "wsh-disable-embedded-scripts",
            Label = "Block WSH Embedded Script Execution",
            Category = "Windows Script Host Policy",
            Description = "Disallows execution of scripts embedded inside other documents (e.g., HTML Application files).",
            Tags = ["script", "wsh", "hta", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks .hta and embedded script execution via WSH. Reduces attack surface.",
            ApplyOps = [RegOp.SetDword(WshKey, "EmbeddedScript", 0)],
            RemoveOps = [RegOp.DeleteValue(WshKey, "EmbeddedScript")],
            DetectOps = [RegOp.CheckDword(WshKey, "EmbeddedScript", 0)],
        },
        new TweakDef
        {
            Id = "wsh-disable-wscript-host",
            Label = "Disable WScript.exe Interactive Host",
            Category = "Windows Script Host Policy",
            Description = "Prevents WScript.exe (GUI script host) from running scripts interactively.",
            Tags = ["script", "wsh", "wscript", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "WScript.exe is the GUI host for .vbs/.js; disabling it does not block CScript.exe.",
            ApplyOps = [RegOp.SetDword(WshKey, "UseWINSAAPI", 0)],
            RemoveOps = [RegOp.DeleteValue(WshKey, "UseWINSAAPI")],
            DetectOps = [RegOp.CheckDword(WshKey, "UseWINSAAPI", 0)],
        },
        new TweakDef
        {
            Id = "wsh-disable-script-logging",
            Label = "Enable WSH Script Execution Logging",
            Category = "Windows Script Host Policy",
            Description = "Enables audit logging of every script execution via WSH to the Application event log.",
            Tags = ["script", "wsh", "logging", "audit"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Visible scripts create audit trails; useful for incident detection.",
            ApplyOps = [RegOp.SetDword(WshKey, "LogSecuritySuccesses", 1)],
            RemoveOps = [RegOp.DeleteValue(WshKey, "LogSecuritySuccesses")],
            DetectOps = [RegOp.CheckDword(WshKey, "LogSecuritySuccesses", 1)],
        },
        new TweakDef
        {
            Id = "wsh-disable-script-ui",
            Label = "Suppress WSH Interactive UI Prompts",
            Category = "Windows Script Host Policy",
            Description = "Prevents scripts from displaying interactive dialog boxes (WScript.Echo, MsgBox).",
            Tags = ["script", "wsh", "ui", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Suppresses script-generated dialogs; good for server/kiosk environments.",
            ApplyOps = [RegOp.SetDword(WshKey, "SilentTerminate", 1)],
            RemoveOps = [RegOp.DeleteValue(WshKey, "SilentTerminate")],
            DetectOps = [RegOp.CheckDword(WshKey, "SilentTerminate", 1)],
        },
        new TweakDef
        {
            Id = "wsh-disable-legacy-vbscript",
            Label = "Disable Legacy VBScript Engine via WSH",
            Category = "Windows Script Host Policy",
            Description = "Prevents the legacy VBScript engine from being loaded by WSH, mitigating known CVEs.",
            Tags = ["script", "wsh", "vbscript", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Blocks VBScript execution entirely. Many CVEs target VBScript—this reduces attack surface significantly.",
            ApplyOps = [RegOp.SetDword(WshKey, "VBScriptEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(WshKey, "VBScriptEnabled")],
            DetectOps = [RegOp.CheckDword(WshKey, "VBScriptEnabled", 0)],
        },
        new TweakDef
        {
            Id = "wsh-disable-cscript-host",
            Label = "Disable CScript.exe Console Host",
            Category = "Windows Script Host Policy",
            Description = "Restricts CScript.exe (the console WSH host) from executing scripts without administrator approval.",
            Tags = ["script", "wsh", "cscript", "console"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "CScript.exe is widely abused in fileless attacks. Disable on locked-down systems.",
            ApplyOps = [RegOp.SetDword(WshKey, "IgnoreUserSettings", 1)],
            RemoveOps = [RegOp.DeleteValue(WshKey, "IgnoreUserSettings")],
            DetectOps = [RegOp.CheckDword(WshKey, "IgnoreUserSettings", 1)],
        },
    ];
}
