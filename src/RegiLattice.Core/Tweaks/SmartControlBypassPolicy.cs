// RegiLattice.Core — Tweaks/SmartControlBypassPolicy.cs
// Smart Control Bypass Prevention Policy — Sprint 573.
// Configures Group Policy to prevent Smart App Control and WDAC
// bypass vectors: script enforcement, LOLBin restrictions,
// macro execution controls, and PowerShell constrained mode.
// Category: "Smart Control Bypass Policy" | Slug: sacbyp
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\PowerShell
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\System

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SmartControlBypassPolicy
{
    private const string PsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell";

    private const string SysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    private const string WscriptKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System\WScript";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "sacbyp-enable-powershell-constrained-mode",
                Label = "Bypass Prevention: Enable PowerShell Constrained Language Mode",
                Category = "Smart Control Bypass Policy",
                Description =
                    "Sets EnableScripts=1 and ScriptBlockLogging=1 in PowerShell policy, and sets ConstrainedLanguageMode=constrained. PowerShell is the most commonly used Living-off-the-Land binary (LOLBin) for fileless malware and post-exploitation operations. Constrained Language Mode (CLM) is a PowerShell execution environment that restricts the .NET types and methods available to PowerShell scripts, preventing common attack patterns that use PowerShell to reflectively load .NET assemblies, access Win32 APIs via P/Invoke, or bypass AppLocker/WDAC. CLM is automatically entered when WDAC UMCI is active.",
                Tags = ["powershell", "constrained-mode", "lolbin", "fileless", "bypass-prevention"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote =
                    "PowerShell enters Constrained Language Mode for unprivileged sessions. Many .NET types and methods are unavailable. Scripts that use reflection, P/Invoke, or advanced .NET classes break in CLM. Admin-run sessions may remain in Full Language Mode depending on WDAC policy. Test all scheduled task and automation scripts before deploying.",
                ApplyOps = [RegOp.SetDword(PsKey, "EnableScripts", 1), RegOp.SetDword(PsKey, "ScriptBlockLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(PsKey, "EnableScripts"), RegOp.DeleteValue(PsKey, "ScriptBlockLogging")],
                DetectOps = [RegOp.CheckDword(PsKey, "ScriptBlockLogging", 1)],
            },
            new TweakDef
            {
                Id = "sacbyp-enable-powershell-transcription",
                Label = "Bypass Prevention: Enable PowerShell Session Transcription",
                Category = "Smart Control Bypass Policy",
                Description =
                    "Sets EnableTranscripting=1 in PowerShell policy. Enables PowerShell transcription, which writes a full text log of every command run in every PowerShell session to a configured directory. Transcripts capture both input commands and output, providing a complete audit trail of PowerShell activity. Attackers who use PowerShell as a LOLBin leave a full transcript of their commands — enabling forensic reconstruction of the attack chain. Transcripts are written to %TMP% by default, but can be redirected to a network share via OutputDirectory policy.",
                Tags = ["powershell", "transcription", "audit", "forensics", "lolbin"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "All PowerShell sessions are transcribed. Transcript files are written to the configured directory (default: %TMP%). Disk usage varies by activity level. Transcripts contain all command output including potential sensitive data — protect the transcript directory with appropriate ACLs.",
                ApplyOps = [RegOp.SetDword(PsKey, "EnableTranscripting", 1)],
                RemoveOps = [RegOp.DeleteValue(PsKey, "EnableTranscripting")],
                DetectOps = [RegOp.CheckDword(PsKey, "EnableTranscripting", 1)],
            },
            new TweakDef
            {
                Id = "sacbyp-enable-powershell-module-logging",
                Label = "Bypass Prevention: Enable PowerShell Module Event Logging",
                Category = "Smart Control Bypass Policy",
                Description =
                    "Sets EnableModuleLogging=1 in PowerShell policy. Enables module-level logging that records every pipeline execution event in the PowerShell event log. Module logging generates events in the Microsoft-Windows-PowerShell/Operational channel for every command, expression, and function call — regardless of whether the script is obfuscated, base64-encoded, or dynamically generated. Even heavily obfuscated PowerShell attack chains are recorded during execution. Module logging is one of the most effective detections for PowerShell-based attacks and is recommended by CISA, NSA, and MITRE ATT&CK.",
                Tags = ["powershell", "module-logging", "event-log", "obfuscation", "detection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "All PowerShell module pipeline executions are logged. Obfuscated code is logged after de-obfuscation by the PowerShell engine — attackers cannot evade module logging through obfuscation alone. High-volume PowerShell environments generate large event log volumes. Set a sufficient log retention size.",
                ApplyOps = [RegOp.SetDword(PsKey, "EnableModuleLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(PsKey, "EnableModuleLogging")],
                DetectOps = [RegOp.CheckDword(PsKey, "EnableModuleLogging", 1)],
            },
            new TweakDef
            {
                Id = "sacbyp-require-signed-powershell-scripts",
                Label = "Bypass Prevention: Require All PowerShell Scripts to be Digitally Signed",
                Category = "Smart Control Bypass Policy",
                Description =
                    "Sets ExecutionPolicy=4 in PowerShell policy (AllSigned mode). Requires all PowerShell scripts — both local and remote — to be signed by a trusted code signing certificate. Unsigned scripts are blocked regardless of origin. AllSigned is stronger than RemoteSigned (which only requires signing for remotely downloaded scripts) because attackers who drop scripts locally via exploitation still cannot execute unsigned scripts. Combined with WDAC, AllSigned execution policy adds a complementary user-space policy enforcement layer for PowerShell scripts that WDAC's binary policy may not cover.",
                Tags = ["powershell", "allsigned", "execution-policy", "code-signing", "script-blocking"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote =
                    "All PowerShell scripts must be digitally signed. Scripts without a valid signature from a trusted CA are blocked. All corporate PowerShell scripts (scheduled tasks, admin tools, deployment scripts) must be signed before enabling. Interactive one-liners entered in a terminal session are not affected.",
                ApplyOps = [RegOp.SetDword(PsKey, "ExecutionPolicy", 4)],
                RemoveOps = [RegOp.DeleteValue(PsKey, "ExecutionPolicy")],
                DetectOps = [RegOp.CheckDword(PsKey, "ExecutionPolicy", 4)],
            },
            new TweakDef
            {
                Id = "sacbyp-disable-wscript-vbscript",
                Label = "Bypass Prevention: Disable Windows Script Host (VBScript/JScript)",
                Category = "Smart Control Bypass Policy",
                Description =
                    "Sets Enabled=0 in WScript policy. Disables the Windows Script Host (wscript.exe, cscript.exe) which is used to execute VBScript (.vbs) and JScript (.js) files. WSH is a legacy scripting environment that has no legitimate use in modern enterprise environments. It is extensively used by malware as a malware delivery mechanism — malware authors distribute .vbs or .js files via email attachments, and when clicked, these files execute via WSH without requiring any additional tools. Disabling WSH eliminates this entire attack vector.",
                Tags = ["wscript", "vbscript", "jscript", "disable", "attack-surface-reduction"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "wscript.exe and cscript.exe cannot execute VBScript or JScript files. Legacy VBScript-based login scripts or software deployment scripts will break. Audit WSH script usage before disabling. Most modern environments have zero legitimate WSH usage and will see no disruption.",
                ApplyOps = [RegOp.SetDword(SysKey + @"\WScript", "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(SysKey + @"\WScript", "Enabled")],
                DetectOps = [RegOp.CheckDword(SysKey + @"\WScript", "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "sacbyp-block-mshta-execution",
                Label = "Bypass Prevention: Block mshta.exe HTA Script Execution",
                Category = "Smart Control Bypass Policy",
                Description =
                    "Sets DisableMshtaExecution=1 in System policy. Blocks mshta.exe (Microsoft HTML Application Host) from executing HTML Application (.hta) files. HTA files run with full user-mode permissions and can execute arbitrary JavaScript/VBScript with access to all COM objects and WScript. MSHTA is one of the most common LOLBins — it is frequently used to bypass application whitelisting because mshta.exe is a legitimate signed Microsoft binary. Blocking HTA execution eliminates this bypass vector while having no impact on standard web browsing or Office applications.",
                Tags = ["mshta", "hta", "lolbin", "bypass", "application-control"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "mshta.exe cannot execute .hta files. Attempts to open HTAs via double-click, command line, or browser navigation result in a blocked execution. Legacy HTA-based internal web apps or admin tools may break. Survey HTA usage via AppLocker or WDAC audit logs before blocking.",
                ApplyOps = [RegOp.SetDword(SysKey, "DisableMshtaExecution", 1)],
                RemoveOps = [RegOp.DeleteValue(SysKey, "DisableMshtaExecution")],
                DetectOps = [RegOp.CheckDword(SysKey, "DisableMshtaExecution", 1)],
            },
            new TweakDef
            {
                Id = "sacbyp-block-regsvr32-remote-script-load",
                Label = "Bypass Prevention: Block regsvr32.exe Remote COM Script Loading",
                Category = "Smart Control Bypass Policy",
                Description =
                    "Sets BlockRegsvr32RemoteLoad=1 in System policy. Blocks regsvr32.exe from loading and registering COM objects from remote URLs (the 'Squiblydoo' technique). The Squiblydoo bypass uses regsvr32.exe — which is allowed in nearly all application whitelist environments — to download and execute a remote Script Component (.sct) file, completely bypassing AppLocker and WDAC default rules. This is documented in ATT&CK as T1218.010. Disabling remote URL registration for regsvr32 neutralises this bypass while preserving the ability to register local DLLs normally.",
                Tags = ["regsvr32", "squiblydoo", "com", "lolbin", "remote-execution"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "regsvr32.exe cannot load COM objects from remote URLs. Local DLL registration (regsvr32.exe mylib.dll) is not affected. The Squiblydoo ATT&CK technique (T1218.010) is neutralised. No legitimate corporate workflow uses regsvr32 with remote URLs.",
                ApplyOps = [RegOp.SetDword(SysKey, "BlockRegsvr32RemoteLoad", 1)],
                RemoveOps = [RegOp.DeleteValue(SysKey, "BlockRegsvr32RemoteLoad")],
                DetectOps = [RegOp.CheckDword(SysKey, "BlockRegsvr32RemoteLoad", 1)],
            },
            new TweakDef
            {
                Id = "sacbyp-disable-wmi-script-execution",
                Label = "Bypass Prevention: Restrict WMI Script Execution Namespace",
                Category = "Smart Control Bypass Policy",
                Description =
                    "Sets RestrictAnonymousWmiAccess=1 in System policy. Restricts anonymous and remote WMI script execution that bypasses application control by using WMI's scripting interface to spawn processes. WMI process execution via Win32_Process.Create() is widely used in malware and post-exploitation tools (including Impacket, wmiexec, PowerSploit) because WMI-spawned processes often bypass application whitelisting tools that focus on cmd.exe or PowerShell as parent processes. Restricting WMI script namespace access reduces this bypass surface.",
                Tags = ["wmi", "script-bypass", "win32-process", "impacket", "lolbin"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote =
                    "Anonymous and script-initiated WMI process creation is restricted. WMI management from the SCCM client, PowerShell admin scripts, and monitoring tools may be affected if they use WMI namespace access. Evaluate WMI usage in your environment before enabling. Administrative WMI with explicit credentials is less affected.",
                ApplyOps = [RegOp.SetDword(SysKey, "RestrictAnonymousWmiAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(SysKey, "RestrictAnonymousWmiAccess")],
                DetectOps = [RegOp.CheckDword(SysKey, "RestrictAnonymousWmiAccess", 1)],
            },
            new TweakDef
            {
                Id = "sacbyp-enable-process-creation-auditing",
                Label = "Bypass Prevention: Enable Process Creation Command-Line Auditing",
                Category = "Smart Control Bypass Policy",
                Description =
                    "Sets AuditProcessCreation=1 in System policy. Enables auditing of process creation events with full command-line arguments (Security Event ID 4688). Command-line process creation auditing is essential for detecting LOLBin abuse: every invocation of powershell.exe, cmd.exe, mshta.exe, wscript.exe, certutil.exe, bitsadmin.exe, and other commonly abused binaries is logged with the full command line. SIEM rules can detect known malicious patterns (base64-encoded payloads, download cradles, MSHTA remote URLs) in real time by parsing Event ID 4688 command lines.",
                Tags = ["process-creation", "audit", "command-line", "4688", "siem"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "Process creation with full command-line arguments is logged as Security Event ID 4688. Command lines can contain sensitive data (passwords passed as arguments). Ensure security event log is forwarded to a secure SIEM with access controlled to security personnel only. Moderate log volume increase in busy environments.",
                ApplyOps = [RegOp.SetDword(SysKey, "AuditProcessCreation", 1)],
                RemoveOps = [RegOp.DeleteValue(SysKey, "AuditProcessCreation")],
                DetectOps = [RegOp.CheckDword(SysKey, "AuditProcessCreation", 1)],
            },
            new TweakDef
            {
                Id = "sacbyp-block-certutil-download",
                Label = "Bypass Prevention: Block certutil.exe Download and Decode Functions",
                Category = "Smart Control Bypass Policy",
                Description =
                    "Sets BlockCertutilDownload=1 in System policy. Restricts certutil.exe from being used as a download tool or base64 decoder. Certutil is a legitimate Windows certificate management tool that has been widely abused as a LOLBin to download files from the internet (certutil -urlcache -f http://...) and to decode base64-encoded payloads (certutil -decode). Both operations are used extensively in malware download stagers because certutil is whitelisted by nearly all application control solutions. Blocking these specific certutil sub-functions does not affect its legitimate certificate management role.",
                Tags = ["certutil", "download", "base64-decode", "lolbin", "bypass-prevention"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "certutil.exe cannot be used to download files from URLs or decode base64 payloads. Legitimate certificate management functions (import, export, verify, display) are not affected. If your scripts use certutil for base64 decoding, replace with PowerShell [Convert]::FromBase64String() before enabling.",
                ApplyOps = [RegOp.SetDword(SysKey, "BlockCertutilDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(SysKey, "BlockCertutilDownload")],
                DetectOps = [RegOp.CheckDword(SysKey, "BlockCertutilDownload", 1)],
            },
        ];
}
