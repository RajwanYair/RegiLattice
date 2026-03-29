// RegiLattice.Core — Tweaks/WdagFileCachePolicy.cs
// Windows Defender Application Guard File Cache Policy — Sprint 575.
// Configures Group Policy for Windows Defender Application Guard
// (WDAG): container settings, network isolation, file cache,
// persistence, print allowance, and host interaction controls.
// Category: "WDAG File Cache Policy" | Slug: wdagfc
// Registry: HKLM\SOFTWARE\Policies\Microsoft\AppHVSI

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WdagFileCachePolicy
{
    private const string WdagKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wdagfc-enable-wdag-for-edge",
                Label = "WDAG: Enable Application Guard for Microsoft Edge",
                Category = "WDAG File Cache Policy",
                Description =
                    "Sets AllowAppHVSI_ProviderSet=1 in AppHVSI policy. Enables Windows Defender Application Guard (WDAG) for Microsoft Edge at the enterprise policy level. When WDAG is enabled, Edge opens untrusted websites (those not in the enterprise trusted zone) inside a hardware-isolated Hyper-V container. If a malicious website exploits a browser vulnerability within the WDAG container, the exploit is contained within the isolated environment and cannot access the host OS, its files, credentials, or the clipboard. The container is discarded after the session. This is the highest-grade browser isolation available on Windows.",
                Tags = ["wdag", "application-guard", "browser-isolation", "hyperv", "container"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "Edge opens untrusted websites in a Hyper-V container. Requires enterprise-grade hardware (Hyper-V support, SLAT, 8GB+ RAM). Container startup adds 5–15 second latency for the first WDAG session. Copy-paste between container and host is blocked by default unless explicitly enabled by policy. Printing and camera access are also restricted by default.",
                ApplyOps = [RegOp.SetDword(WdagKey, "AllowAppHVSI_ProviderSet", 1)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "AllowAppHVSI_ProviderSet")],
                DetectOps = [RegOp.CheckDword(WdagKey, "AllowAppHVSI_ProviderSet", 1)],
            },
            new TweakDef
            {
                Id = "wdagfc-block-clipboard-from-container",
                Label = "WDAG: Block Clipboard from WDAG Container to Host",
                Category = "WDAG File Cache Policy",
                Description =
                    "Sets AppHVSIClipboardSettings=1 in AppHVSI policy (host-to-container only). Configures clipboard sharing so that content can be pasted from the host into the WDAG container (needed to enter URLs or credentials) but content from the container cannot be pasted to the host. This prevents an attack where a compromised WDAG session tries to exfiltrate data by copying it to the clipboard and the user then pastes it outside the container. The asymmetric clipboard policy allows productive use of WDAG without creating an exfiltration channel.",
                Tags = ["wdag", "clipboard", "exfiltration-prevention", "asymmetric", "container"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Clipboard is one-way: host → container only. Users cannot copy content from WDAG container sessions to the host. Legitimate workflows that require copying content from a WDAG session (e.g., copying a URL from an isolated browser to share) are blocked. Users must retype or use an approved sharing method.",
                ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIClipboardSettings", 1)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIClipboardSettings")],
                DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIClipboardSettings", 1)],
            },
            new TweakDef
            {
                Id = "wdagfc-block-print-from-container",
                Label = "WDAG: Block Printing from the WDAG Container",
                Category = "WDAG File Cache Policy",
                Description =
                    "Sets AppHVSIPrintBlockSettings=0 in AppHVSI policy (all printing blocked). Disables printing from within the WDAG container. WDAG printing is a potential exfiltration vector: a compromised website within the container could automatically trigger printing sensitive data from the host-provided print queue. While this is a low-probability attack (requiring significant user interaction), blocking printing from the container eliminates the risk while having minimal impact — users who need to print document in an isolated session can download the document to an approved location first.",
                Tags = ["wdag", "print-block", "exfiltration", "container", "network-printer"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Printing from within the WDAG Edge container is disabled. Content accessed in the WDAG container cannot be directly printed. Users who need to print a WDAG page must save and transfer through an approved workflow. Negligible productivity impact for typical web browsing.",
                ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIPrintBlockSettings", 0)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIPrintBlockSettings")],
                DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIPrintBlockSettings", 0)],
            },
            new TweakDef
            {
                Id = "wdagfc-disable-container-persistence",
                Label = "WDAG: Disable Container Persistence (Discard Container on Close)",
                Category = "WDAG File Cache Policy",
                Description =
                    "Sets AllowPersistence=0 in AppHVSI policy. Configures WDAG to discard the container state when the WDAG session is closed — the container does not persist browser history, cookies, downloads, or any state between sessions. Each WDAG session starts from a clean container image. Persistence, if enabled, allows attack artefacts (malware files, poisoned cookies, modified registry state) to survive across WDAG sessions and potentially be leveraged in future sessions. Discarding the container eliminates the possibility of session-to-session attack propagation within WDAG.",
                Tags = ["wdag", "persistence", "container", "discard", "clean-slate"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "WDAG container is discarded on session close. No browsing history, cookies, or downloads persist in WDAG. Users who need to return to a WDAG session must log in again to websites. Clean container on each session eliminates attack artefact accumulation.",
                ApplyOps = [RegOp.SetDword(WdagKey, "AllowPersistence", 0)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "AllowPersistence")],
                DetectOps = [RegOp.CheckDword(WdagKey, "AllowPersistence", 0)],
            },
            new TweakDef
            {
                Id = "wdagfc-disable-camera-mic-in-container",
                Label = "WDAG: Disable Camera and Microphone Access in WDAG Container",
                Category = "WDAG File Cache Policy",
                Description =
                    "Sets AppHVSICameraAndMicrophoneSettings=0 in AppHVSI policy (both disabled). Prevents websites running inside the WDAG container from accessing the host's camera and microphone. Camera and microphone access from an isolated container is a potential privacy and exfiltration risk — a malicious site in the container could silently activate the camera or microphone to capture the user's environment. Since WDAG is intended for untrusted websites, granting media device access undermines the isolation model. The default is blocked; this policy explicitly enforces it.",
                Tags = ["wdag", "camera", "microphone", "privacy", "media-access"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Websites in the WDAG container cannot access camera or microphone. Video conferencing and voice features in WDAG browser sessions are blocked. This is appropriate for untrusted sites. Trusted sites should not be opened in WDAG — they should be in the enterprise trusted zone.",
                ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSICameraAndMicrophoneSettings", 0)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSICameraAndMicrophoneSettings")],
                DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSICameraAndMicrophoneSettings", 0)],
            },
            new TweakDef
            {
                Id = "wdagfc-block-file-download-from-container",
                Label = "WDAG: Block File Downloads from the WDAG Container to Host",
                Category = "WDAG File Cache Policy",
                Description =
                    "Sets SaveFilesToHost=0 in AppHVSI policy. Prevents files downloaded within the WDAG container from being saved directly to the host file system. Without this restriction, a malicious site could prompt the user to download and save a file — the file lands on the host's file system outside the container isolation, potentially infecting the host. Blocking file save to host means downloads within WDAG are contained within the isolated environment. Users who need a downloaded file from a WDAG session must go through an approved file transfer workflow.",
                Tags = ["wdag", "file-download", "exfiltration", "container-escape", "host-protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "Files downloaded in the WDAG container cannot be saved to the host. Attempts to save downloads land in the container's temporary storage (discarded on close if persistence is disabled). Users cannot retrieve downloads from WDAG sessions without an explicit transfer mechanism defined by IT.",
                ApplyOps = [RegOp.SetDword(WdagKey, "SaveFilesToHost", 0)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "SaveFilesToHost")],
                DetectOps = [RegOp.CheckDword(WdagKey, "SaveFilesToHost", 0)],
            },
            new TweakDef
            {
                Id = "wdagfc-define-enterprise-network-domain-list",
                Label = "WDAG: Enable Enterprise Network Isolation (Route Untrusted Sites to Container)",
                Category = "WDAG File Cache Policy",
                Description =
                    "Sets AppHVSIAllowedDomains policy enabled flag=1. Activates the WDAG enterprise network domain isolation feature. Enterprise domains (configured via the Network Isolation policy) are considered trusted and open in the standard Edge host browser. All other domains are considered untrusted and are automatically redirected to the WDAG container. This network isolation approach ensures that users are protected by default without needing to consciously open WDAG — the browser automatically routes untrusted traffic to the container.",
                Tags = ["wdag", "network-isolation", "domain-routing", "automatic", "enterprise-trusted"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "All non-enterprise-domain sites automatically open in the WDAG container. Requires NetworkIsolation policy to define the trusted enterprise domains. First-time WDAG container startup adds latency. Enterprise trusted zone domains (intranet, SharePoint, corporate portals) must be correctly enumerated for a smooth user experience.",
                ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIAllowedDomainsEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIAllowedDomainsEnabled")],
                DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIAllowedDomainsEnabled", 1)],
            },
            new TweakDef
            {
                Id = "wdagfc-enable-wdag-audit-logging",
                Label = "WDAG: Enable WDAG Container Event Logging",
                Category = "WDAG File Cache Policy",
                Description =
                    "Sets AppHVSIAuditMode=1 in AppHVSI policy. Enables event logging for WDAG container lifecycle and security events: container start, container stop, isolation boundary violations, clipboard policy enforcement, network domain routing decisions, and container crashes. WDAG events are logged to the Microsoft-Windows-Windows-Defender-ApplicationGuard/Operational channel. These events enable IT to track WDAG usage patterns, detect frequent container crashes (indicating exploit attempts), and audit clipboard and print policy enforcement.",
                Tags = ["wdag", "audit-logging", "container-events", "event-log", "security-monitoring"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "WDAG container lifecycle and security events are logged. Logs include container start/stop, boundary violations, and policy enforcement decisions. Low log volume — events are per-container-session, not per-page-load. Enables SIEM alerting on anomalous WDAG behaviour.",
                ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIAuditMode", 1)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIAuditMode")],
                DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIAuditMode", 1)],
            },
            new TweakDef
            {
                Id = "wdagfc-restrict-wdag-to-edge-only",
                Label = "WDAG: Restrict WDAG Isolation to Microsoft Edge Only",
                Category = "WDAG File Cache Policy",
                Description =
                    "Sets AppHVSIBrowserOptions=1 in AppHVSI policy. Restricts WDAG container usage to Microsoft Edge. Prevents other application types from launching their own WDAG containers. If enabled for standalone WDAG (value 2), arbitrary applications can request WDAG isolation. Restricting to Edge-only (value 1) ensures the WDAG container surface is limited to the browser scenario, reducing the attack surface of the WDAG subsystem itself.",
                Tags = ["wdag", "edge-only", "app-isolation", "restriction", "container-surface"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "WDAG containers can only be started by Microsoft Edge. Applications that attempt to use the standalone WDAG API are denied. Reduces WDAG attack surface to the browser scenario only. Standalone WDAG for Office documents (Word, Excel) will not function.",
                ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIBrowserOptions", 1)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIBrowserOptions")],
                DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIBrowserOptions", 1)],
            },
            new TweakDef
            {
                Id = "wdagfc-enable-wdag-telemetry",
                Label = "WDAG: Enable WDAG Diagnostic Telemetry for Threat Intelligence",
                Category = "WDAG File Cache Policy",
                Description =
                    "Sets AppHVSITelemetry=1 in AppHVSI policy. Enables WDAG to send diagnostic telemetry data to Microsoft when a threat is detected or a container security boundary violation is attempted. WDAG telemetry covers container anomaly detection: unexpected kernel calls from the container, attempts to access host memory, and container process crashes consistent with exploit activity. This telemetry feeds Microsoft's Windows Defender threat intelligence, improving detection of novel browser exploits. In regulated environments, telemetry policy should be reviewed against data handling requirements.",
                Tags = ["wdag", "telemetry", "threat-intelligence", "diagnostics", "microsoft"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "WDAG sends diagnostic data to Microsoft on detected anomalies. Telemetry includes container explosion attempts and security boundary violations — not browsing content. Review data handling obligations before enabling in regulated industries (HIPAA, PCI-DSS). Disabled by default in some enterprise configurations.",
                ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSITelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSITelemetry")],
                DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSITelemetry", 1)],
            },
        ];
}
