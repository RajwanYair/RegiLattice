// RegiLattice.Core — Tweaks/DynamicDataExchangePolicy.cs
// Sprint 314: DDE Policy tweaks (10 tweaks)
// Category: "DDE Policy" | Slug: ddepol
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DDE

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DynamicDataExchangePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DDE";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ddepol-disable-dde-protocol",
            Label = "Disable Dynamic Data Exchange Protocol",
            Category = "DDE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            Description =
                "Dynamic Data Exchange is a Windows inter-process communication mechanism that predates modern IPC alternatives and allows applications to exchange data. Disabling DDE prevents applications from using the legacy DDE protocol for inter-process communication and data sharing. DDE has been exploited to execute arbitrary code through malicious Office documents and file paths containing DDE expressions. Microsoft Office applications disabled DDE auto-execution in response to widespread exploitation of DDE-based command injection. Legacy DDE usage has been largely superseded by COM, OLE, and modern IPC mechanisms in contemporary applications. Disabling DDE reduces the attack surface associated with protocol-based code execution while minimally impacting modern applications.",
            Tags = ["dde", "ipc", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDDE", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDDE")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDDE", 1)],
        },
        new TweakDef
        {
            Id = "ddepol-disable-dde-server-launch",
            Label = "Disable DDE Server Launch Through Shell",
            Category = "DDE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "DDE server processes can be launched by the Windows Shell when a client application requests a DDE connection. Disabling DDE server launch through the shell prevents automatic execution of DDE topics registered in the shell file association database. Malicious file associations can register DDE command topics that execute arbitrary code when files are opened through the shell. DDE-based code execution through file associations was used in targeted attacks against organizations and government entities. Preventing shell-launched DDE server processes removes an often-overlooked code execution pathway associated with file handling. This setting does not affect documented and supported DDE applications using the full DDE API directly.",
            Tags = ["dde", "shell", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDdeServerLaunch", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDdeServerLaunch")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDdeServerLaunch", 1)],
        },
        new TweakDef
        {
            Id = "ddepol-disable-network-dde",
            Label = "Disable Network DDE Service",
            Category = "DDE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Network DDE extends the DDE protocol to allow data exchange between applications running on different network computers. Disabling Network DDE removes the ability to conduct cross-computer DDE communication through the network transport. Network DDE services (NetDDE) are legacy Windows services rarely required by modern applications and represent an unnecessary attack surface. Network DDE services on accessible endpoints have been targeted for exploitation in lateral movement techniques. The NetDDE service allows remote parties to initiate DDE connections subject to network security control bypass. Disabling Network DDE eliminates a lateral movement pathway and does not affect local applications or local DDE communication.",
            Tags = ["dde", "network", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNetworkDDE", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkDDE")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNetworkDDE", 1)],
        },
        new TweakDef
        {
            Id = "ddepol-disable-remote-dde",
            Label = "Disable Remote DDE Connections",
            Category = "DDE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Remote DDE connections allow applications on one system to connect to DDE applications running on remote network computers. Disabling remote DDE connections prevents inbound and outbound DDE connections across network boundaries on the endpoint. Remote DDE represents an attack vector for reaching applications that publish DDE topics over the network. Controlling lateral movement through enterprise networks requires restricting legacy protocols not needed for regular business operations. DDE-based lateral movement has been documented in APT actor techniques for moving between network-connected Windows endpoints. Modern applications and workflows do not require remote DDE connectivity and should use authenticated network protocols instead.",
            Tags = ["dde", "remote", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableRemoteDDE", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRemoteDDE")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRemoteDDE", 1)],
        },
        new TweakDef
        {
            Id = "ddepol-disable-dde-auto-link",
            Label = "Disable DDE Auto-Link Updates",
            Category = "DDE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "DDE auto-link updates automatically refresh linked data in documents when source data changes in other linked applications. Disabling DDE auto-link updates prevents automatic execution of DDE expressions embedded in documents when documents are opened or refreshed. Auto-link DDE expressions were abused to execute command shells through field codes in Office documents. Disabling auto-link removal addresses a specific attack vector while preserving the ability for users to manually refresh links. Documents with malicious auto-link DDE fields could execute arbitrary code without user awareness in unpatched configurations. Disabling automatic DDE link refreshing is part of the broader mitigation of DDE-based document attack vectors.",
            Tags = ["dde", "auto-link", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoLink", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoLink")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoLink", 1)],
        },
        new TweakDef
        {
            Id = "ddepol-disable-dde-in-explorer",
            Label = "Disable DDE in Windows Explorer File Associations",
            Category = "DDE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Windows Explorer uses file associations including DDE commands to launch applications when files are opened by double-clicking. Disabling DDE in Explorer file associations prevents DDE command sequences from executing when users open files through the shell. File association DDE commands are the most common exploitation pathway for DDE-based code execution attacks. Malicious documents with crafted file names or embedded associations can trigger DDE execution through Explorer shell operations. Removing DDE from Explorer file handling ensures that file opens go through direct application launch rather than DDE topic invocation. This setting is one of the most impactful DDE mitigations for preventing file-open-triggered code execution.",
            Tags = ["dde", "explorer", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableExplorerDDE", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableExplorerDDE")],
            DetectOps = [RegOp.CheckDword(Key, "DisableExplorerDDE", 1)],
        },
        new TweakDef
        {
            Id = "ddepol-disable-dde-in-hyperlinks",
            Label = "Disable DDE in Hyperlink Resolution",
            Category = "DDE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Hyperlinks in documents and emails can reference DDE topics as targets allowing code execution when the link is clicked. Disabling DDE in hyperlink resolution prevents linked references from triggering DDE-based application invocation. Malicious emails and documents with crafted hyperlinks can execute arbitrary commands through DDE-enabled link resolution. Hyperlink-triggered DDE execution is particularly dangerous as it is initiated by expected user action that appears legitimate. Removing DDE support from hyperlink handling closes a significant social engineering attack surface that has been used in phishing campaigns. Legitimate hyperlinks targeting web URLs and file paths continue to work normally after DDE hyperlink resolution is disabled.",
            Tags = ["dde", "hyperlinks", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableHyperlinkDDE", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableHyperlinkDDE")],
            DetectOps = [RegOp.CheckDword(Key, "DisableHyperlinkDDE", 1)],
        },
        new TweakDef
        {
            Id = "ddepol-disable-dde-warning-bypass",
            Label = "Prevent DDE Security Warning Bypass",
            Category = "DDE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Office applications display security warnings before executing DDE commands embedded in documents or when connecting to external DDE servers. Preventing DDE security warning bypass ensures that users cannot suppress warnings that protect against DDE-based exploitation. When users are allowed to bypass security prompts through persistence settings malicious DDE execution can proceed silently. Security prompts serve as the last line of defense against DDE attacks when technical prevention measures are not fully effective. Maintaining mandatory security warnings prevents users from unknowingly allowing malicious DDE connections through habitual prompt dismissal. DDE warnings should be considered alongside application-level DDE disabling for comprehensive coverage.",
            Tags = ["dde", "warnings", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDDEWarningBypass", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDDEWarningBypass")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDDEWarningBypass", 1)],
        },
        new TweakDef
        {
            Id = "ddepol-audit-dde-usage",
            Label = "Enable DDE Usage Audit Logging",
            Category = "DDE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "DDE audit logging records attempts to invoke DDE connections or execute DDE topics on the endpoint. Enabling DDE audit logging provides visibility into remaining DDE usage that can be used to identify applications relying on legacy DDE communication. Understanding remaining DDE usage is essential before fully disabling DDE to avoid breaking business-critical applications. Audit logs also capture potential DDE exploitation attempts allowing security teams to detect and investigate suspicious activity. DDE usage data in the event log can be forwarded to SIEM systems for correlation with other security events. Audit logging has minimal performance impact and provides valuable operational and security intelligence for DDE management.",
            Tags = ["dde", "audit", "logging", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditDDEUsage", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditDDEUsage")],
            DetectOps = [RegOp.CheckDword(Key, "AuditDDEUsage", 1)],
        },
        new TweakDef
        {
            Id = "ddepol-disable-clipboard-dde",
            Label = "Disable DDE via Clipboard Operations",
            Category = "DDE Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Description =
                "The clipboard can carry DDE format data that establishes DDE connections when pasted into DDE-aware applications. Disabling clipboard DDE prevents DDE link establishment through clipboard paste operations. Clipboard-based DDE attack delivery allows attackers to initiate DDE connections without file system involvement. Social engineering attacks can instruct users to paste clipboard contents into applications which then trigger DDE-based code execution. Disabling DDE clipboard format reduces the attack surface for clipboard-delivery of DDE attack payloads. Applications that legitimately rely on clipboard-based DDE data exchange will require alternative data sharing methods.",
            Tags = ["dde", "clipboard", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableClipboardDDE", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardDDE")],
            DetectOps = [RegOp.CheckDword(Key, "DisableClipboardDDE", 1)],
        },
    ];
}
