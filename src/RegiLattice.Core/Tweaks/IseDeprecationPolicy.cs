// RegiLattice.Core — Tweaks/IseDeprecationPolicy.cs
// Windows PowerShell ISE deprecation enforcement and legacy PS5 usage controls — Sprint 449.
// Category: "ISE Deprecation Policy" | Slug: isedep
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\PowerShell
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ProtectedEventLogging

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class IseDeprecationPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ProtectedEventLogging";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "isedep-block-ise-launch",
                Label = "Block PowerShell ISE Launch",
                Category = "ISE Deprecation Policy",
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
                Category = "ISE Deprecation Policy",
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
                Category = "ISE Deprecation Policy",
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
                Category = "ISE Deprecation Policy",
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
                Category = "ISE Deprecation Policy",
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
                Category = "ISE Deprecation Policy",
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
                Category = "ISE Deprecation Policy",
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
                Category = "ISE Deprecation Policy",
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
                Category = "ISE Deprecation Policy",
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
                Category = "ISE Deprecation Policy",
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
