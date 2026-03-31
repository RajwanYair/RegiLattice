// RegiLattice.Core — Tweaks/PolicyPowerShell.cs
// PowerShell execution, ISE deprecation, PS7 modes, script block logging, and scripted diagnostics policies
// Category: "PowerShell & Scripting Policy"
// Consolidated from 6 modules.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyPowerShell
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _IseDeprecationPolicy.Data,
            .. _PowerShellPolicy.Data,
            .. _Ps7ExecutionModePolicy.Data,
            .. _ScriptBlockLoggingAdvancedPolicy.Data,
            .. _ScriptedDiagnosticsPolicy.Data,
            .. _WindowsTerminalAdvancedPolicy.Data,
        ];

    // ── IseDeprecationPolicy ──
    private static class _IseDeprecationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ProtectedEventLogging";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "isedep-block-ise-launch",
                    Label = "Block PowerShell ISE Launch",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Blocks launch of the Windows PowerShell ISE (Integrated Scripting Environment), which is end-of-life and lacks modern security controls like AMSI integration.",
                    Tags = ["powershell", "ise", "deprecation", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "PowerShell ISE cannot be opened; users must use VS Code or PowerShell 7 instead.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableISE", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableISE")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableISE", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-force-remoting-allsigned",
                    Label = "Block Unsigned Scripts via PS Remoting",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Sets the remoting script execution policy to AllSigned, so scripts delivered via WinRM PowerShell remoting sessions must be digitally signed.",
                    Tags = ["powershell", "remoting", "signing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Unsigned scripts over WinRM remoting blocked; signed scripts required.",
                    ApplyOps = [RegOp.SetDword(Key, "RemotingExecutionPolicy", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RemotingExecutionPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "RemotingExecutionPolicy", 2)],
                },
                new TweakDef
                {
                    Id = "isedep-disable-v2-engine",
                    Label = "Disable PowerShell v2 Engine",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Disables the Windows PowerShell version 2 engine (powershell.exe -version 2) which bypasses modern security controls such as AMSI, ETW, and Constrained Language Mode.",
                    Tags = ["powershell", "v2", "downgrade", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "PS v2 engine blocked; attackers cannot downgrade to bypass AMSI. Legacy apps requiring PS2 will break.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePowerShellV2", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePowerShellV2")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePowerShellV2", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-enable-protected-event-logging",
                    Label = "Enable Protected Event Logging",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Enables Protected Event Logging (PEL) for PowerShell, which encrypts sensitive PowerShell script block log entries at rest using a certificate, protecting them from unauthorized access.",
                    Tags = ["powershell", "event-logging", "encryption", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Script block logs encrypted; only authorised certificate holders can decrypt and read them.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableProtectedEventLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableProtectedEventLogging")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableProtectedEventLogging", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-disable-credential-prompt",
                    Label = "Disable Credential Prompt in PowerShell Sessions",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Disables interactive credential prompts within PowerShell sessions, forcing scripts to use pre-provisioned credentials or fail instead of prompting the user.",
                    Tags = ["powershell", "credentials", "prompt", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Credential prompts inside PS sessions blocked; scripts requiring user input must be refactored.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCredentialRequestPrompt", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCredentialRequestPrompt")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCredentialRequestPrompt", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-enable-module-logging",
                    Label = "Enable Module Logging for PowerShell",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Enables module-level logging for all PowerShell modules by default, ensuring that all custom module invocations are captured in the Windows PowerShell/Operational event log.",
                    Tags = ["powershell", "module-logging", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "All loaded PS module commands logged; log volume scales with module usage.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableModuleLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableModuleLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableModuleLogging", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-disable-script-download",
                    Label = "Disable Script Download from Internet in PowerShell",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Blocks PowerShell from downloading and executing scripts from internet URIs using Invoke-Expression (IEX) with web requests, a common living-off-the-land attack technique.",
                    Tags = ["powershell", "download-cradle", "iex", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "PS internet download-and-exec cradles blocked; IEX/webclient patterns are prevented.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableScriptDownloadFromInternet", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableScriptDownloadFromInternet")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableScriptDownloadFromInternet", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-block-ps-dev-mode",
                    Label = "Block PowerShell Developer Mode",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Disables the PowerShell developer mode flag that bypasses certain security policies, ensuring that production machines do not inadvertently run in a relaxed-security development mode.",
                    Tags = ["powershell", "developer-mode", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PS developer/debug mode disabled; policy overrides cannot be bypassed via dev flags.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockPSDeveloperMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockPSDeveloperMode")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockPSDeveloperMode", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-disable-ps-telemetry",
                    Label = "Disable Windows PowerShell 5 Telemetry",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Disables usage telemetry collection in Windows PowerShell 5.1, preventing execution metadata and error statistics from being sent to Microsoft.",
                    Tags = ["powershell", "ps5", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "PS 5.1 telemetry stopped; no functional impact.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-force-network-restricted-sessions",
                    Label = "Force Network-Restricted PowerShell Remoting Sessions",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Forces all incoming PowerShell remoting sessions to run as NetworkRestricted, preventing remotely established sessions from making outbound network connections.",
                    Tags = ["powershell", "remoting", "network-restricted", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Remote PS sessions cannot make new network connections; lateral movement via PS remoting reduced.",
                    ApplyOps = [RegOp.SetDword(Key, "ForceNetworkRestrictedSessions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ForceNetworkRestrictedSessions")],
                    DetectOps = [RegOp.CheckDword(Key, "ForceNetworkRestrictedSessions", 1)],
                },
            ];

    }

    // ── PowerShellPolicy ──
    private static class _PowerShellPolicy
    {
        private const string PsRoot = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell";
        private const string ScriptBlockLogging = PsRoot + @"\ScriptBlockLogging";
        private const string ModuleLogging = PsRoot + @"\ModuleLogging";
        private const string Transcription = PsRoot + @"\Transcription";
        private const string ProtectedEventLogging = PsRoot + @"\ProtectedEventLogging";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "pspolicy-script-block-logging",
                Label = "Enable PowerShell Script Block Logging",
                Category = "PowerShell & Scripting Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enables PowerShell Script Block Logging (GPO) so that all script blocks "
                    + "executed by PowerShell are written to the Operational event log "
                    + "(Microsoft-Windows-PowerShell/Operational). Essential for threat hunting.",
                Tags = ["powershell", "logging", "script block", "security", "audit"],
                RegistryKeys = [ScriptBlockLogging],
                ApplyOps = [RegOp.SetDword(ScriptBlockLogging, "EnableScriptBlockLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(ScriptBlockLogging, "EnableScriptBlockLogging")],
                DetectOps = [RegOp.CheckDword(ScriptBlockLogging, "EnableScriptBlockLogging", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-script-invocation-logging",
                Label = "Enable PowerShell Script Invocation Logging",
                Category = "PowerShell & Scripting Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enables logging of script-block invocation start/stop events in addition "
                    + "to the raw script text. Adds invocation sequence to event IDs 4104/4105/4106. "
                    + "EnableScriptBlockInvocationLogging=1.",
                Tags = ["powershell", "logging", "invocation logging", "security"],
                RegistryKeys = [ScriptBlockLogging],
                ApplyOps = [RegOp.SetDword(ScriptBlockLogging, "EnableScriptBlockInvocationLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(ScriptBlockLogging, "EnableScriptBlockInvocationLogging")],
                DetectOps = [RegOp.CheckDword(ScriptBlockLogging, "EnableScriptBlockInvocationLogging", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-module-logging",
                Label = "Enable PowerShell Module Logging",
                Category = "PowerShell & Scripting Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enables Module Logging, which records pipeline execution details for "
                    + "specified (or all) PowerShell modules to the Windows event log. "
                    + "Helps detect malicious module imports. EnableModuleLogging=1.",
                Tags = ["powershell", "module logging", "security", "audit"],
                RegistryKeys = [ModuleLogging],
                ApplyOps = [RegOp.SetDword(ModuleLogging, "EnableModuleLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(ModuleLogging, "EnableModuleLogging")],
                DetectOps = [RegOp.CheckDword(ModuleLogging, "EnableModuleLogging", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-transcription-on",
                Label = "Enable PowerShell Transcription",
                Category = "PowerShell & Scripting Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Turns on PowerShell Transcription, writing a plain-text record of every "
                    + "PowerShell session (all commands and output) to a configured output "
                    + "directory. EnableTranscripting=1.",
                Tags = ["powershell", "transcription", "logging", "audit"],
                RegistryKeys = [Transcription],
                ApplyOps = [RegOp.SetDword(Transcription, "EnableTranscripting", 1)],
                RemoveOps = [RegOp.DeleteValue(Transcription, "EnableTranscripting")],
                DetectOps = [RegOp.CheckDword(Transcription, "EnableTranscripting", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-transcription-header",
                Label = "Include Invocation Header in PowerShell Transcripts",
                Category = "PowerShell & Scripting Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Adds timestamps, username, and invocation details to each PowerShell "
                    + "transcript header, making audit review significantly easier. "
                    + "EnableInvocationHeader=1.",
                Tags = ["powershell", "transcription", "header", "audit"],
                RegistryKeys = [Transcription],
                ApplyOps = [RegOp.SetDword(Transcription, "EnableInvocationHeader", 1)],
                RemoveOps = [RegOp.DeleteValue(Transcription, "EnableInvocationHeader")],
                DetectOps = [RegOp.CheckDword(Transcription, "EnableInvocationHeader", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-transcription-output-path",
                Label = "Set PowerShell Transcript Output Directory",
                Category = "PowerShell & Scripting Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets the PowerShell transcript output directory to "
                    + @"%SYSTEMROOT%\Logs\PowerShell so transcripts are centralised and "
                    + "survive user profile deletion. OutputDirectory (REG_SZ).",
                Tags = ["powershell", "transcription", "output path", "audit"],
                RegistryKeys = [Transcription],
                ApplyOps = [RegOp.SetString(Transcription, "OutputDirectory", @"%SYSTEMROOT%\Logs\PowerShell")],
                RemoveOps = [RegOp.DeleteValue(Transcription, "OutputDirectory")],
                DetectOps = [RegOp.CheckString(Transcription, "OutputDirectory", @"%SYSTEMROOT%\Logs\PowerShell")],
            },
            new TweakDef
            {
                Id = "pspolicy-disable-ps2-engine",
                Label = "Disable PowerShell 2.0 Engine",
                Category = "PowerShell & Scripting Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets EnablePS2=0 via GPO to prevent PowerShell from falling back to the "
                    + "legacy v2 engine, which lacks script block logging and constrained language "
                    + "mode — a known AMSI/logging bypass vector.",
                Tags = ["powershell", "ps2", "downgrade attack", "security", "amsi"],
                RegistryKeys = [PsRoot],
                ApplyOps = [RegOp.SetDword(PsRoot, "EnablePS2", 0)],
                RemoveOps = [RegOp.DeleteValue(PsRoot, "EnablePS2")],
                DetectOps = [RegOp.CheckDword(PsRoot, "EnablePS2", 0)],
            },
            new TweakDef
            {
                Id = "pspolicy-protected-event-logging",
                Label = "Enable Protected Event Logging",
                Category = "PowerShell & Scripting Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Enables Protected Event Logging, which encrypts sensitive event log data "
                    + "(such as PowerShell credentials in transcripts) using a CMS certificate, "
                    + "so only authorized readers can decrypt. EnableProtectedEventLogging=1.",
                Tags = ["powershell", "event log", "encryption", "protected logging", "security"],
                RegistryKeys = [ProtectedEventLogging],
                ApplyOps = [RegOp.SetDword(ProtectedEventLogging, "EnableProtectedEventLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(ProtectedEventLogging, "EnableProtectedEventLogging")],
                DetectOps = [RegOp.CheckDword(ProtectedEventLogging, "EnableProtectedEventLogging", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-enable-scripts",
                Label = "Enable PowerShell Script Execution (GPO)",
                Category = "PowerShell & Scripting Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets EnableScripts=1 via GPO, activating the policy-controlled execution "
                    + "policy. Must be enabled before ExecutionPolicy can be enforced via "
                    + "Group Policy. Set together with pspolicy-require-signed-scripts.",
                Tags = ["powershell", "execution policy", "scripts", "gpo"],
                RegistryKeys = [PsRoot],
                ApplyOps = [RegOp.SetDword(PsRoot, "EnableScripts", 1)],
                RemoveOps = [RegOp.DeleteValue(PsRoot, "EnableScripts")],
                DetectOps = [RegOp.CheckDword(PsRoot, "EnableScripts", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-require-signed-scripts",
                Label = "Require Signed PowerShell Scripts (AllSigned)",
                Category = "PowerShell & Scripting Policy",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Sets ExecutionPolicy=AllSigned via GPO, requiring that all scripts and "
                    + "configuration files — including local scripts — be signed by a trusted "
                    + "publisher. Strongest policy-level execution restriction.",
                Tags = ["powershell", "execution policy", "signed", "allsigned", "security"],
                RegistryKeys = [PsRoot],
                ApplyOps = [RegOp.SetString(PsRoot, "ExecutionPolicy", "AllSigned")],
                RemoveOps = [RegOp.DeleteValue(PsRoot, "ExecutionPolicy")],
                DetectOps = [RegOp.CheckString(PsRoot, "ExecutionPolicy", "AllSigned")],
            },
        ];

    }

    // ── Ps7ExecutionModePolicy ──
    private static class _Ps7ExecutionModePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PowerShellCore";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PowerShellCore\ScriptBlockLogging";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ps7exec-enable-constrained-language",
                    Label = "Enable Constrained Language Mode in PowerShell 7",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Enables Constrained Language Mode (CLM) for PowerShell 7 (pwsh), restricting the .NET types and COM objects that scripts can use and mitigating fileless malware execution.",
                    Tags = ["powershell", "ps7", "constrained-language", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote = "PS7 runs in Constrained Language Mode; complex scripts and .NET interop may break.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableConstrainedLanguageMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableConstrainedLanguageMode")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableConstrainedLanguageMode", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-set-allsigned-policy",
                    Label = "Enforce AllSigned Execution Policy in PowerShell 7",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Sets the PowerShell 7 execution policy to AllSigned, requiring all scripts (including local scripts) to be digitally signed by a trusted publisher before execution.",
                    Tags = ["powershell", "ps7", "execution-policy", "signing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Only code-signed PS7 scripts run; unsigned dev scripts require explicit bypass.",
                    ApplyOps = [RegOp.SetDword(Key, "ExecutionPolicy", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ExecutionPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "ExecutionPolicy", 2)],
                },
                new TweakDef
                {
                    Id = "ps7exec-disable-remoting",
                    Label = "Disable PowerShell 7 Remoting",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Disables PowerShell 7 remoting (WinRM/SSH transport) via policy, preventing pwsh from being used as a remote administration target.",
                    Tags = ["powershell", "ps7", "remoting", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "PS7 remoting disabled; WinRM/SSH-based remote pwsh sessions blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRemoting", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRemoting")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRemoting", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-disable-implicit-remoting",
                    Label = "Disable PS7 Implicit Remoting Module Import",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Disables implicit remoting module imports in PowerShell 7, preventing a script from automatically importing and executing remote commands from untrusted sources.",
                    Tags = ["powershell", "ps7", "implicit-remoting", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Implicit remote module imports blocked; remote command invocation requires explicit setup.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableImplicitRemoting", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableImplicitRemoting")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableImplicitRemoting", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-require-signed-modules",
                    Label = "Require Signed Module Manifests in PowerShell 7",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Requires all PowerShell 7 module manifests (.psd1) to be signed by a trusted publisher before the module can be loaded, blocking unsigned third-party modules.",
                    Tags = ["powershell", "ps7", "modules", "signing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Unsigned PS7 modules cannot load; all modules must have trusted code signatures.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireSignedModuleManifests", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireSignedModuleManifests")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireSignedModuleManifests", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-block-ps-gallery",
                    Label = "Block PowerShell Gallery Repository in PS7",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Disables access to the default PowerShell Gallery online repository in PowerShell 7, forcing module and script installation through an approved internal repository.",
                    Tags = ["powershell", "ps7", "gallery", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PSGallery blocked; Install-Module will not reach the public gallery.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockPSGallery", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockPSGallery")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockPSGallery", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-enable-script-block-logging",
                    Label = "Enable Script Block Logging in PowerShell 7",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Enables script block logging in PowerShell 7 to record all script blocks executed to the event log (Microsoft-Windows-PowerShell/Operational), supporting forensic analysis.",
                    Tags = ["powershell", "ps7", "script-block-logging", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "All executed PS7 script blocks logged; event log volume may increase significantly.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableScriptBlockLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableScriptBlockLogging")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableScriptBlockLogging", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-enable-invocation-logging",
                    Label = "Enable Script Block Invocation Logging in PS7",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Enables verbose script block invocation logging in PowerShell 7, capturing start and stop events for each script block execution for detailed forensic trails.",
                    Tags = ["powershell", "ps7", "invocation-logging", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Verbose invocation events logged; high-traffic PS7 hosts will generate significant log volume.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableScriptBlockInvocationLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableScriptBlockInvocationLogging")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableScriptBlockInvocationLogging", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-disable-telemetry",
                    Label = "Disable PowerShell 7 Telemetry",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Disables the PowerShell 7 telemetry feature that sends usage statistics (command names, error categories, OS info) to Microsoft via opt-out environment variable enforcement at policy level.",
                    Tags = ["powershell", "ps7", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PS7 telemetry fully disabled at policy level; no usage data sent.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-disable-update-notif",
                    Label = "Disable PowerShell 7 Update Notifications",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Suppresses in-session PowerShell 7 update available notifications that prompt users to download newer versions, deferring updates to a managed patching process.",
                    Tags = ["powershell", "ps7", "update", "notifications", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "PS7 update banners not shown; version management via package manager.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableUpdateNotifications", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUpdateNotifications")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUpdateNotifications", 1)],
                },
            ];

    }

    // ── ScriptBlockLoggingAdvancedPolicy ──
    private static class _ScriptBlockLoggingAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\Transcription";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sbloga-enable-script-block-logging",
                    Label = "Enable Script Block Logging (Windows PS)",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Enables PowerShell script block logging for Windows PowerShell 5.1 via the dedicated ScriptBlockLogging policy key, recording all executed script blocks to the PowerShell/Operational event log.",
                    Tags = ["powershell", "script-block-logging", "audit", "siem", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Full script block content logged; essential for threat hunting but increases log volume.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableScriptBlockLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableScriptBlockLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableScriptBlockLogging", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-enable-invocation-header",
                    Label = "Enable Script Block Invocation Header Logging",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Enables logging of the start and stop events for each function/script-block invocation, providing timestamped execution boundaries in the event log.",
                    Tags = ["powershell", "script-block-logging", "invocation", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Invocation start/stop events logged; detailed execution trail generated.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableScriptBlockInvocationLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableScriptBlockInvocationLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableScriptBlockInvocationLogging", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-enable-transcription",
                    Label = "Enable PowerShell Transcript Logging",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Enables PowerShell session transcription that saves a full text copy of every PowerShell session to a transcript file on disk, providing a human-readable audit trail.",
                    Tags = ["powershell", "transcription", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Every PS session recorded to transcript file; disk space usage increases with PS activity.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableTranscripting", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableTranscripting")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableTranscripting", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-enable-invocation-header-transcript",
                    Label = "Include Invocation Header in PS Transcripts",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Adds invocation header information (command name, arguments, timestamps, username, process info) to PowerShell transcript files.",
                    Tags = ["powershell", "transcription", "header", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Transcript files include rich header context for each command.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableInvocationHeader", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableInvocationHeader")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableInvocationHeader", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-set-output-directory",
                    Label = "Set Centralised PowerShell Transcript Directory",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Sets the PowerShell transcript output directory to a centralised network share or admin-controlled path so all endpoint transcripts are collected in one location.",
                    Tags = ["powershell", "transcription", "directory", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Transcripts written to admin-specified path; ensure path is writable and monitored.",
                    ApplyOps = [RegOp.SetString(Key2, "OutputDirectory", @"C:\Windows\Logs\PowerShell")],
                    RemoveOps = [RegOp.DeleteValue(Key2, "OutputDirectory")],
                    DetectOps = [RegOp.CheckString(Key2, "OutputDirectory", @"C:\Windows\Logs\PowerShell")],
                },
                new TweakDef
                {
                    Id = "sbloga-log-encoded-commands",
                    Label = "Log Encoded PowerShell Command Executions",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Enables script block logging specifically targeting Base64-encoded commands (-EncodedCommand), which are commonly used by malware to obfuscate payloads.",
                    Tags = ["powershell", "encoded-commands", "obfuscation", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Encoded command executions captured in logs; key detection for fileless attacks.",
                    ApplyOps = [RegOp.SetDword(Key, "LogEncodedCommands", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogEncodedCommands")],
                    DetectOps = [RegOp.CheckDword(Key, "LogEncodedCommands", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-log-dynamic-code",
                    Label = "Log Dynamically Generated PowerShell Code",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Enables logging of dynamically generated PowerShell code (e.g., from Invoke-Expression or Add-Type), capturing obfuscated payloads that are assembled at runtime.",
                    Tags = ["powershell", "dynamic-code", "invoke-expression", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Dynamically generated PS code blocks captured; critical for detecting memory-only malware.",
                    ApplyOps = [RegOp.SetDword(Key, "LogDynamicCode", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogDynamicCode")],
                    DetectOps = [RegOp.CheckDword(Key, "LogDynamicCode", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-set-max-log-size",
                    Label = "Set PowerShell Operational Log Max Size to 512 MB",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Increases the Microsoft-Windows-PowerShell/Operational event log maximum size to 512 MB to prevent log overwriting (circular buffer) during high-volume script block logging.",
                    Tags = ["powershell", "event-log", "size", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PS operational log grows to 512 MB max; older entries retained longer before cycling.",
                    ApplyOps =
                    [
                        RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "MaxSize", 524288),
                    ],
                    RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "MaxSize")],
                    DetectOps =
                    [
                        RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "MaxSize", 524288),
                    ],
                },
                new TweakDef
                {
                    Id = "sbloga-retain-on-clear",
                    Label = "Retain PowerShell Log Archive on Clear",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Configures the PowerShell operational event log to archive before clearing when the log becomes full, preventing permanent log loss during log maintenance.",
                    Tags = ["powershell", "event-log", "archive", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PS log archived to .evtx file before clearing; no historical data lost.",
                    ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "Retention", 0)],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "Retention"),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "Retention", 0),
                    ],
                },
                new TweakDef
                {
                    Id = "sbloga-block-clear-eventlog",
                    Label = "Block Standard Users from Clearing Event Logs",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Restricts the ability to clear the PowerShell and Windows event logs to administrators only, preventing attackers with standard user access from clearing their tracks.",
                    Tags = ["powershell", "event-log", "clear", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only admins can clear event logs; standard users get access denied.",
                    ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security", "RestrictGuestAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security", "RestrictGuestAccess")],
                    DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security", "RestrictGuestAccess", 1)],
                },
            ];

    }

    // ── ScriptedDiagnosticsPolicy ──
    private static class _ScriptedDiagnosticsPolicy
    {
        private const string SdKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnostics";
        private const string SdProv = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy";
        private const string TshootKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Troubleshooting\AllowRecommendations";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "sdiag-disable-scripted-diagnostics",
                Label = "Disable Scripted Diagnostics Execution",
                Category = "PowerShell & Scripting Policy",
                Description =
                    "Sets ExecutionPolicy=0 in the ScriptedDiagnostics policy key. "
                    + "Prevents Windows from executing scripted diagnostic packages (.diagpkg, .diag files), "
                    + "including the automated troubleshooters triggered from 'Troubleshoot settings'. "
                    + "Reduces data collection and prevents unintended automated changes. "
                    + "Default: absent (diagnostics run). Recommended: 1 on managed or high-security systems.",
                Tags = ["diagnostics", "scripted", "troubleshooter", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Scripted diagnostic packages (.diagpkg) cannot execute; automated troubleshooters are blocked.",
                ApplyOps = [RegOp.SetDword(SdKey, "ExecutionPolicy", 0)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "ExecutionPolicy")],
                DetectOps = [RegOp.CheckDword(SdKey, "ExecutionPolicy", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-online-troubleshooters",
                Label = "Disable Online Troubleshooting Recommendations",
                Category = "PowerShell & Scripting Policy",
                Description =
                    "Sets EnabledPolicy=0 in the ScriptedDiagnosticsProvider Policy key. "
                    + "Prevents Windows from downloading and applying troubleshooting recommendations from Microsoft's "
                    + "online diagnostic database. Stops automatic remediation steps that could modify system settings. "
                    + "Default: absent (online recommendations enabled). Recommended: 0 on air-gapped or privacy-sensitive systems.",
                Tags = ["diagnostics", "online", "recommendations", "troubleshooter", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Online troubleshooting recommendations from Microsoft not fetched or applied.",
                ApplyOps = [RegOp.SetDword(SdProv, "EnabledPolicy", 0)],
                RemoveOps = [RegOp.DeleteValue(SdProv, "EnabledPolicy")],
                DetectOps = [RegOp.CheckDword(SdProv, "EnabledPolicy", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-recommended-troubleshooting",
                Label = "Disable Windows Recommended Troubleshooting",
                Category = "PowerShell & Scripting Policy",
                Description =
                    "Sets TurnOffWindowsErrorReportingServer=1 in the AllowRecommendations "
                    + "Troubleshooting policy key. Disables the 'Recommended troubleshooting' feature "
                    + "that automatically diagnoses and resolves common problems. Prevents Windows from "
                    + "silently applying fixes based on crash data from Windows Error Reporting. "
                    + "Default: absent. Recommended: 1 when automated fixes are undesired in production environments.",
                Tags = ["diagnostics", "recommended", "auto-fix", "troubleshooter", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Windows Recommended Troubleshooting feature disabled; no automatic problem fixes applied.",
                ApplyOps = [RegOp.SetDword(TshootKey, "TurnOffWindowsErrorReportingServer", 1)],
                RemoveOps = [RegOp.DeleteValue(TshootKey, "TurnOffWindowsErrorReportingServer")],
                DetectOps = [RegOp.CheckDword(TshootKey, "TurnOffWindowsErrorReportingServer", 1)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-automatic-maintenance-diagnostics",
                Label = "Disable Automatic Maintenance Diagnostics",
                Category = "PowerShell & Scripting Policy",
                Description =
                    "Sets EnableAutomatedTroubleshooting=0 in the ScriptedDiagnostics policy key. "
                    + "Prevents Windows Automatic Maintenance from running scripted diagnostic jobs "
                    + "in the background during maintenance windows. Avoids unexpected system changes from "
                    + "background maintenance troubleshooters. "
                    + "Default: absent (enabled). Recommended: 0 in change-controlled environments.",
                Tags = ["diagnostics", "maintenance", "automated", "background", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Scripted diagnostic jobs from Windows Automatic Maintenance are disabled.",
                ApplyOps = [RegOp.SetDword(SdKey, "EnableAutomatedTroubleshooting", 0)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "EnableAutomatedTroubleshooting")],
                DetectOps = [RegOp.CheckDword(SdKey, "EnableAutomatedTroubleshooting", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-elevated-troubleshooter",
                Label = "Disable Elevated Scripted Troubleshooter Execution",
                Category = "PowerShell & Scripting Policy",
                Description =
                    "Sets RunAsHighestAvailablePrivilege=0 in the ScriptedDiagnostics policy key. "
                    + "Prevents scripted diagnostic packages from automatically requesting elevation to "
                    + "run with highest available privileges. Forces diagnostics to run as standard user "
                    + "unless explicitly elevated by an administrator. "
                    + "Default: absent (auto-elevation allowed). Recommended: 0 on principle-of-least-privilege systems.",
                Tags = ["diagnostics", "elevation", "uac", "privilege", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Diagnostic packages cannot auto-elevate; admin must explicitly run elevated troubleshooters.",
                ApplyOps = [RegOp.SetDword(SdKey, "RunAsHighestAvailablePrivilege", 0)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "RunAsHighestAvailablePrivilege")],
                DetectOps = [RegOp.CheckDword(SdKey, "RunAsHighestAvailablePrivilege", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-results-upload",
                Label = "Disable Diagnostic Results Upload",
                Category = "PowerShell & Scripting Policy",
                Description =
                    "Sets AllowDiagnosticDataUpload=0 in the ScriptedDiagnostics policy key. "
                    + "Prevents scripted diagnostic packages from uploading their results logs, "
                    + "diagnostic data, or anonymised telemetry to Microsoft or third-party servers. "
                    + "Default: absent (upload allowed). Recommended: 0 on air-gapped or privacy-sensitive systems.",
                Tags = ["diagnostics", "upload", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Diagnostic results not uploaded; data stays on-device.",
                ApplyOps = [RegOp.SetDword(SdKey, "AllowDiagnosticDataUpload", 0)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "AllowDiagnosticDataUpload")],
                DetectOps = [RegOp.CheckDword(SdKey, "AllowDiagnosticDataUpload", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-user-initiated-troubleshooter",
                Label = "Block User-Initiated Troubleshooters",
                Category = "PowerShell & Scripting Policy",
                Description =
                    "Sets DisableUserDiagnostics=1 in the ScriptedDiagnostics policy key. "
                    + "Prevents non-administrator users from launching troubleshooters from Settings "
                    + "('Get help', 'Troubleshoot', 'Fix problems'). Only administrators can initiate "
                    + "diagnostic packages. Useful on shared or terminal-server machines. "
                    + "Default: absent (users can launch troubleshooters). Recommended: 1 on kiosk/terminal machines.",
                Tags = ["diagnostics", "user", "kiosk", "restrict", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Non-admin users cannot launch Windows troubleshooters from Settings.",
                ApplyOps = [RegOp.SetDword(SdKey, "DisableUserDiagnostics", 1)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "DisableUserDiagnostics")],
                DetectOps = [RegOp.CheckDword(SdKey, "DisableUserDiagnostics", 1)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-third-party-diagnostics",
                Label = "Block Third-Party Diagnostic Packages",
                Category = "PowerShell & Scripting Policy",
                Description =
                    "Sets AllowThirdPartyDiagnostics=0 in the ScriptedDiagnostics policy key. "
                    + "Prevents Windows from running scripted diagnostic packages (.diagpkg) from publishers "
                    + "other than Microsoft. Only Microsoft-signed diagnostic packages are permitted to run. "
                    + "Default: absent (third-party packages allowed). Recommended: 0 to limit diagnostic execution surface.",
                Tags = ["diagnostics", "third-party", "packages", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Third-party diagnostic packages (.diagpkg) blocked; only Microsoft-signed packages run.",
                ApplyOps = [RegOp.SetDword(SdKey, "AllowThirdPartyDiagnostics", 0)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "AllowThirdPartyDiagnostics")],
                DetectOps = [RegOp.CheckDword(SdKey, "AllowThirdPartyDiagnostics", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-scheduled-diagnostics",
                Label = "Disable Scheduled Diagnostic Tasks",
                Category = "PowerShell & Scripting Policy",
                Description =
                    "Sets DisableScheduledDiagnostics=1 in the ScriptedDiagnostics policy key. "
                    + "Prevents the Scheduled Maintenance Diagnostics task scheduler jobs from creating "
                    + "or running scripted diagnostic tasks in the background on a schedule. "
                    + "Reduces background system load and unexpected modifications. "
                    + "Default: absent (scheduled diagnostics run). Recommended: 1 on optimised/stable systems.",
                Tags = ["diagnostics", "scheduled", "maintenance", "task-scheduler", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Scheduled background diagnostic maintenance tasks are blocked.",
                ApplyOps = [RegOp.SetDword(SdKey, "DisableScheduledDiagnostics", 1)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "DisableScheduledDiagnostics")],
                DetectOps = [RegOp.CheckDword(SdKey, "DisableScheduledDiagnostics", 1)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-troubleshooting-history",
                Label = "Disable Troubleshooting History Storage",
                Category = "PowerShell & Scripting Policy",
                Description =
                    "Sets DisableTroubleshootingHistory=1 in the ScriptedDiagnostics policy key. "
                    + "Prevents Windows from writing troubleshooter run results and histories to the "
                    + "machine's troubleshooting log database. Reduces local data accumulation from "
                    + "diagnostic activities. "
                    + "Default: absent (history stored). Recommended: 1 on privacy-focused or ephemeral systems.",
                Tags = ["diagnostics", "history", "privacy", "logging", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Troubleshooter run history and results are not stored in the local database.",
                ApplyOps = [RegOp.SetDword(SdKey, "DisableTroubleshootingHistory", 1)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "DisableTroubleshootingHistory")],
                DetectOps = [RegOp.CheckDword(SdKey, "DisableTroubleshootingHistory", 1)],
            },
        ];

    }

    // ── WindowsTerminalAdvancedPolicy ──
    private static class _WindowsTerminalAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal\Updates";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "termadv-disable-auto-update",
                    Label = "Disable Windows Terminal Auto-Update",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Disables automatic update checks and downloads for Windows Terminal, ensuring the terminal version is managed by WSUS or package management rather than in-app updates.",
                    Tags = ["terminal", "update", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Windows Terminal will not auto-update; version management via package manager or WSUS.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoUpdate")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoUpdate", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-telemetry",
                    Label = "Disable Windows Terminal Telemetry",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Disables usage telemetry collection in Windows Terminal including keyboard shortcut usage, profile creation frequency, and renderer performance data.",
                    Tags = ["terminal", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Terminal telemetry disabled; no usage data sent to Microsoft.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-store-launch",
                    Label = "Disable Store Launch from Windows Terminal",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Prevents Windows Terminal from launching the Microsoft Store for extensions, themes, or profile suggestions, reducing MS Store telemetry exposure.",
                    Tags = ["terminal", "store", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "MS Store launch button in terminal disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableStoreLaunch", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableStoreLaunch")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableStoreLaunch", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-startup-tasks",
                    Label = "Disable Windows Terminal Startup Tasks",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Disables Windows Terminal startup task registration that auto-starts terminal on user login, reducing unnecessary background process startup.",
                    Tags = ["terminal", "startup", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Terminal does not auto-launch at logon.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableStartupTasks", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableStartupTasks")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableStartupTasks", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-enforce-restricted-profile",
                    Label = "Enforce Restricted Profile in Windows Terminal",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Enables restricted profile enforcement in Windows Terminal, blocking users from modifying terminal profiles, settings JSON, or key bindings.",
                    Tags = ["terminal", "profile", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Users cannot modify terminal settings; only admin-defined profiles are available.",
                    ApplyOps = [RegOp.SetDword(Key, "EnforceRestrictedProfile", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnforceRestrictedProfile")],
                    DetectOps = [RegOp.CheckDword(Key, "EnforceRestrictedProfile", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-extensions",
                    Label = "Disable Windows Terminal Extensions",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Disables the ability to install or run third-party extensions in Windows Terminal, reducing the attack surface from unvetted extension code execution.",
                    Tags = ["terminal", "extensions", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Terminal extensions disabled; only built-in functionality available.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableExtensions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableExtensions")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableExtensions", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-block-ssh-agent",
                    Label = "Block SSH Agent Integration in Windows Terminal",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Disables the SSH agent forwarding integration in Windows Terminal, preventing terminal sessions from forwarding SSH keys to remote hosts.",
                    Tags = ["terminal", "ssh", "agent", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "SSH agent forwarding blocked from terminal; prevents key forwarding to hostile servers.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockSshAgentIntegration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockSshAgentIntegration")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockSshAgentIntegration", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-preview-builds",
                    Label = "Disable Windows Terminal Preview Build Channel",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Forces Windows Terminal to the stable release channel, disabling the Preview and Canary build channels to ensure only stable, vetted versions are used.",
                    Tags = ["terminal", "preview", "channel", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Terminal locked to stable channel; Preview/Canary builds not offered.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisablePreviewBuilds", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisablePreviewBuilds")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisablePreviewBuilds", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-update-notifications",
                    Label = "Disable Update Notifications in Windows Terminal",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Suppresses in-app update available notifications in Windows Terminal, which can distract users and prompt unauthorized manual updates.",
                    Tags = ["terminal", "update", "notifications", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Update reminder banners not shown in terminal.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableUpdateNotifications", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableUpdateNotifications")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableUpdateNotifications", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-block-manual-updates",
                    Label = "Block Manual Windows Terminal Updates by Users",
                    Category = "PowerShell & Scripting Policy",
                    Description =
                        "Prevents standard users from triggering manual Windows Terminal update checks or downloads, ensuring that all terminal update operations require administrator rights.",
                    Tags = ["terminal", "update", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Standard users cannot manually update terminal; admin action required.",
                    ApplyOps = [RegOp.SetDword(Key2, "BlockManualUpdates", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "BlockManualUpdates")],
                    DetectOps = [RegOp.CheckDword(Key2, "BlockManualUpdates", 1)],
                },
            ];

    }

}
