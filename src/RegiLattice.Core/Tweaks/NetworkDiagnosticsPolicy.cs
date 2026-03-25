#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 244 — Network Diagnostics & WDI Scenario Policy (10 tweaks)
// Keys: HKLM\SOFTWARE\Policies\Microsoft\Windows\NetworkDiagnostics
//       HKLM\SOFTWARE\Policies\Microsoft\Windows\WDI\{...} (WDI scenario GUIDs)
//       HKLM\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnostics
internal static class NetworkDiagnosticsPolicy
{
    private const string NetDiagKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkDiagnostics";
    private const string WdiWireless = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{EBC068D3-BD0A-4B60-9078-6B952B7B04D1}";
    private const string WdiNetConn = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{A7A5847A-7511-4E4E-90B1-45AD2A002F51}";
    private const string WdiNetPerf = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{4DC08CD6-E593-4B38-9ABC-9C25B15571C1}";
    private const string WdiNetCfg = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{C76A4930-2379-4C5F-B2B3-F671FDDF73E2}";
    private const string ScriptDiag = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnostics";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ndiag-disable-helper-engine",
            Label = "Disable Network Diagnostics Helper Engine",
            Category = "Network Diagnostics Policy",
            Description =
                "Sets DisableHelperEngine=1 in the NetworkDiagnostics policy key. "
                + "Turns off the Windows Network Diagnostics helper engine entirely, preventing automated diagnosis "
                + "of network connectivity issues and the 'Diagnose this problem' link in error dialogs. "
                + "Default: absent (engine active). Recommended: 1 when users must escalate to IT for network issues.",
            Tags = ["network", "diagnostics", "policy", "restriction"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Removes the 'Windows Network Diagnostics' auto-repair flow; network connectivity itself is unaffected.",
            ApplyOps = [RegOp.SetDword(NetDiagKey, "DisableHelperEngine", 1)],
            RemoveOps = [RegOp.DeleteValue(NetDiagKey, "DisableHelperEngine")],
            DetectOps = [RegOp.CheckDword(NetDiagKey, "DisableHelperEngine", 1)],
        },
        new TweakDef
        {
            Id = "ndiag-disable-wireless-scenario",
            Label = "Disable WDI Wireless Diagnostics Scenario",
            Category = "Network Diagnostics Policy",
            Description =
                "Sets ScenarioExecutionEnabled=0 for the WDI Wireless Diagnostics scenario (GUID EBC068D3). "
                + "Prevents the Windows Diagnostics Infrastructure from automatically running wireless troubleshooting "
                + "steps when WLAN connectivity issues are detected. "
                + "Default: absent (scenario active). Recommended: 0 in managed environments where WLAN is centrally controlled.",
            Tags = ["wireless", "wdi", "diagnostics", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Disables automated wireless diagnostics; WLAN connectivity itself is unaffected.",
            ApplyOps = [RegOp.SetDword(WdiWireless, "ScenarioExecutionEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(WdiWireless, "ScenarioExecutionEnabled")],
            DetectOps = [RegOp.CheckDword(WdiWireless, "ScenarioExecutionEnabled", 0)],
        },
        new TweakDef
        {
            Id = "ndiag-disable-netconn-scenario",
            Label = "Disable WDI Network Connectivity Diagnostics Scenario",
            Category = "Network Diagnostics Policy",
            Description =
                "Sets ScenarioExecutionEnabled=0 for the WDI Network Connectivity scenario (GUID A7A5847A). "
                + "Prevents Windows from automatically running network connectivity troubleshooting steps. "
                + "Default: absent (scenario active). Recommended: 0 on tightly managed networks.",
            Tags = ["network", "wdi", "diagnostics", "connectivity", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Suppresses automatic network connectivity diagnosis; manual 'Troubleshoot' actions still work.",
            ApplyOps = [RegOp.SetDword(WdiNetConn, "ScenarioExecutionEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(WdiNetConn, "ScenarioExecutionEnabled")],
            DetectOps = [RegOp.CheckDword(WdiNetConn, "ScenarioExecutionEnabled", 0)],
        },
        new TweakDef
        {
            Id = "ndiag-disable-netperf-scenario",
            Label = "Disable WDI Network Performance Monitoring Scenario",
            Category = "Network Diagnostics Policy",
            Description =
                "Sets ScenarioExecutionEnabled=0 for the WDI Network Performance Monitoring scenario (GUID 4DC08CD6). "
                + "Turns off the background network performance data collection component of the Windows Diagnostics Infrastructure. "
                + "Default: absent (monitoring active). Recommended: 0 to reduce background network telemetry on managed systems.",
            Tags = ["network", "wdi", "performance", "monitoring", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Disables background WDI network performance monitoring; reduces diagnostic data collection.",
            ApplyOps = [RegOp.SetDword(WdiNetPerf, "ScenarioExecutionEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(WdiNetPerf, "ScenarioExecutionEnabled")],
            DetectOps = [RegOp.CheckDword(WdiNetPerf, "ScenarioExecutionEnabled", 0)],
        },
        new TweakDef
        {
            Id = "ndiag-disable-netcfg-scenario",
            Label = "Disable WDI Network Configuration Diagnostics Scenario",
            Category = "Network Diagnostics Policy",
            Description =
                "Sets ScenarioExecutionEnabled=0 for the WDI Network Configuration scenario (GUID C76A4930). "
                + "Prevents Windows from automatically running diagnostics when network adapter configuration issues are detected. "
                + "Default: absent (scenario active). Recommended: 0 when NIC configuration is locked by policy.",
            Tags = ["network", "wdi", "configuration", "diagnostics", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Disables the WDI network configuration diagnostic scenario; adapter configuration is unaffected.",
            ApplyOps = [RegOp.SetDword(WdiNetCfg, "ScenarioExecutionEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(WdiNetCfg, "ScenarioExecutionEnabled")],
            DetectOps = [RegOp.CheckDword(WdiNetCfg, "ScenarioExecutionEnabled", 0)],
        },
        new TweakDef
        {
            Id = "ndiag-disable-scripted-diagnostics",
            Label = "Disable Scripted Diagnostics",
            Category = "Network Diagnostics Policy",
            Description =
                "Sets EnableDiagnostics=0 in the ScriptedDiagnostics policy key. "
                + "Disables the Windows Scripted Diagnostics service that powers interactive troubleshooting packs (*.diagpkg). "
                + "Troubleshooting wizards in Control Panel and Settings will be unavailable. "
                + "Default: absent (enabled). Recommended: 0 when automated diagnosis must be controlled by IT.",
            Tags = ["diagnostics", "scripted", "troubleshooter", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Disables all Windows Troubleshooter wizards; built-in scripted diagnostic packs are suppressed.",
            ApplyOps = [RegOp.SetDword(ScriptDiag, "EnableDiagnostics", 0)],
            RemoveOps = [RegOp.DeleteValue(ScriptDiag, "EnableDiagnostics")],
            DetectOps = [RegOp.CheckDword(ScriptDiag, "EnableDiagnostics", 0)],
        },
        new TweakDef
        {
            Id = "ndiag-validate-diag-helpers",
            Label = "Require Validation of Diagnostic Helper Modules",
            Category = "Network Diagnostics Policy",
            Description =
                "Sets ValidateHelpers=1 in the ScriptedDiagnostics policy key. "
                + "Requires that all diagnostic helper modules (*.dll) loaded by the scripted diagnostics engine be "
                + "digitally signed and validated before execution, preventing unsigned diagnostic extensions. "
                + "Default: absent (helpers not validated). Recommended: 1 for security on managed endpoints.",
            Tags = ["diagnostics", "scripted", "validation", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Enforces digital signature validation for all diagnostic helper DLLs; unsigned helpers are blocked.",
            ApplyOps = [RegOp.SetDword(ScriptDiag, "ValidateHelpers", 1)],
            RemoveOps = [RegOp.DeleteValue(ScriptDiag, "ValidateHelpers")],
            DetectOps = [RegOp.CheckDword(ScriptDiag, "ValidateHelpers", 1)],
        },
        new TweakDef
        {
            Id = "ndiag-no-remote-server-query",
            Label = "Block Scripted Diagnostics Remote Server Queries",
            Category = "Network Diagnostics Policy",
            Description =
                "Sets DisableQueryRemoteServer=1 in the ScriptedDiagnostics policy key. "
                + "Prevents the Windows Scripted Diagnostics service from querying Microsoft online servers "
                + "for additional troubleshooting content or updated diagnostic packs. "
                + "Default: absent (remote queries allowed). Recommended: 1 on air-gapped or privacy-sensitive environments.",
            Tags = ["diagnostics", "scripted", "privacy", "remote", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks diagnostic helper queries to Microsoft servers; local troubleshooters continue to work.",
            ApplyOps = [RegOp.SetDword(ScriptDiag, "DisableQueryRemoteServer", 1)],
            RemoveOps = [RegOp.DeleteValue(ScriptDiag, "DisableQueryRemoteServer")],
            DetectOps = [RegOp.CheckDword(ScriptDiag, "DisableQueryRemoteServer", 1)],
        },
        new TweakDef
        {
            Id = "ndiag-restrict-wireless-execution-level",
            Label = "Set WDI Wireless Diagnostics to View-Only",
            Category = "Network Diagnostics Policy",
            Description =
                "Sets EnabledScenarioExecutionLevel=1 for the WDI Wireless Diagnostics scenario. "
                + "Allows Windows to gather wireless diagnostic information and present results to the user, "
                + "but prevents automatic repair actions from being taken without user confirmation. "
                + "Default: absent (automatic repair). Recommended: 1 where users may view diagnostics but not auto-fix.",
            Tags = ["wireless", "wdi", "diagnostics", "restricted", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "WDI wireless diagnostics shows results but does not auto-apply network fixes.",
            ApplyOps = [RegOp.SetDword(WdiWireless, "EnabledScenarioExecutionLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(WdiWireless, "EnabledScenarioExecutionLevel")],
            DetectOps = [RegOp.CheckDword(WdiWireless, "EnabledScenarioExecutionLevel", 1)],
        },
        new TweakDef
        {
            Id = "ndiag-restrict-netconn-execution-level",
            Label = "Set WDI Network Connectivity Diagnostics to View-Only",
            Category = "Network Diagnostics Policy",
            Description =
                "Sets EnabledScenarioExecutionLevel=1 for the WDI Network Connectivity scenario. "
                + "Allows diagnosis of network problems but restricts the engine from automatically applying fixes. "
                + "Default: absent (automatic repair). Recommended: 1 on networks where configuration changes require IT approval.",
            Tags = ["network", "wdi", "diagnostics", "restricted", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "WDI network connectivity diagnostics shows results but does not auto-apply connection fixes.",
            ApplyOps = [RegOp.SetDword(WdiNetConn, "EnabledScenarioExecutionLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(WdiNetConn, "EnabledScenarioExecutionLevel")],
            DetectOps = [RegOp.CheckDword(WdiNetConn, "EnabledScenarioExecutionLevel", 1)],
        },
    ];
}
