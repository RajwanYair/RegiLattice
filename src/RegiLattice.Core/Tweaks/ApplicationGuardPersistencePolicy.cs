// RegiLattice.Core — Tweaks/ApplicationGuardPersistencePolicy.cs
// Application Guard Persistence & Extension Policy — Sprint 576.
// Configures WDAG persistence settings, extension policy for the
// WDAG container, hardware requirements enforcement, and sandbox
// configuration for Office Application Guard integration.
// Category: "Application Guard Persistence Policy" | Slug: wdagpe
// Registry: HKLM\SOFTWARE\Policies\Microsoft\AppHVSI
//           HKLM\SOFTWARE\Policies\Microsoft\Office\16.0\Common\AppHVSI

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ApplicationGuardPersistencePolicy
{
    private const string WdagKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI";

    private const string OfficeWdagKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\AppHVSI";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wdagpe-disable-container-data-persistence",
                Label = "WDAG Persistence: Disable Container Data Persistence Across Sessions",
                Category = "Application Guard Persistence Policy",
                Description =
                    "Sets AllowPersistence=0 in AppHVSI policy. Disables WDAG container data persistence — ensuring that cookies, browser history, cached web content, and local storage inside the WDAG container are purged when the container is closed. This provides the strongest isolation: each session starts from a completely clean container image with no carry-over state from previous sessions. While this means users must re-authenticate to websites in each WDAG session, it prevents any session-to-session data leakage or attack artefact accumulation in the container.",
                Tags = ["wdag", "persistence", "cookie-purge", "session-isolation", "clean-state"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "All container state is discarded on close. Users must log in to all WDAG-hosted websites on every new session. Acceptable for high-security environments. In mixed-security environments, consider enabling persistence with mandatory cleanup on compromise detection instead.",
                ApplyOps = [RegOp.SetDword(WdagKey, "AllowPersistence", 0)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "AllowPersistence")],
                DetectOps = [RegOp.CheckDword(WdagKey, "AllowPersistence", 0)],
            },
            new TweakDef
            {
                Id = "wdagpe-enable-office-application-guard",
                Label = "WDAG Persistence: Enable Office Application Guard for Untrusted Documents",
                Category = "Application Guard Persistence Policy",
                Description =
                    "Sets EnableOfficeApplicationGuard=1 in Office\\16.0\\Common\\AppHVSI policy. Enables Office Application Guard (OAG), which opens untrusted Office documents (Word, Excel, PowerPoint) received from the internet or marked as untrusted in a Hyper-V container. Malicious Office documents (weaponised macros, embedded OLE objects, exploit documents) open in the isolated container — if the document exploits a vulnerability in the Office parser, the exploit is contained. This is the most effective protection against socially-engineered office document attacks, which are the #1 initial access vector.",
                Tags = ["office-application-guard", "word", "excel", "document-isolation", "hyperv"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "Untrusted Office documents open in a Hyper-V container. Exploits in weaponised documents are contained. Requires WDAG-enabled hardware (Hyper-V, SLAT, 8GB+ RAM). Microsoft 365 Apps for Enterprise required. Documents from trusted locations (internal SharePoint, corporate file shares) are not affected and open normally.",
                ApplyOps = [RegOp.SetDword(OfficeWdagKey, "EnableOfficeApplicationGuard", 1)],
                RemoveOps = [RegOp.DeleteValue(OfficeWdagKey, "EnableOfficeApplicationGuard")],
                DetectOps = [RegOp.CheckDword(OfficeWdagKey, "EnableOfficeApplicationGuard", 1)],
            },
            new TweakDef
            {
                Id = "wdagpe-block-office-guard-macro-execution",
                Label = "WDAG Persistence: Block Macro Execution in Office Application Guard",
                Category = "Application Guard Persistence Policy",
                Description =
                    "Sets DisableMacrosInOfficeGuard=1 in Office\\16.0\\Common\\AppHVSI policy. Disables all VBA macro execution in documents opened inside the Office Application Guard container. Even within the isolated container, macros can perform network calls, attempt to communicate with the host, or interact with the container's file system. Blocking macros in the container provides defence-in-depth: if a document exploits a macro execution vulnerability, the macro cannot execute. Documents requiring macros should be opened outside the container only if they are trusted and have been scanned.",
                Tags = ["office-guard", "macro", "vba", "disable", "container"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "VBA macros are blocked in Office Application Guard containers. Untrusted documents cannot execute macros even in the isolated container. Documents with required macros from trusted sources are not affected — they open outside the container. This is an additional safety layer on top of the container isolation.",
                ApplyOps = [RegOp.SetDword(OfficeWdagKey, "DisableMacrosInOfficeGuard", 1)],
                RemoveOps = [RegOp.DeleteValue(OfficeWdagKey, "DisableMacrosInOfficeGuard")],
                DetectOps = [RegOp.CheckDword(OfficeWdagKey, "DisableMacrosInOfficeGuard", 1)],
            },
            new TweakDef
            {
                Id = "wdagpe-restrict-guard-clipboard-to-host-in",
                Label = "WDAG Persistence: Restrict Office Guard Clipboard to Host-to-Container Direction",
                Category = "Application Guard Persistence Policy",
                Description =
                    "Sets OfficeguardClipboardSettings=2 in AppHVSI policy (host → container only). Configures clipboard behaviour for Office Application Guard containers to allow clipboard content from the host to be pasted into the container (enabling the user to paste text into a WDAG form) but blocking the container from exporting clipboard content to the host. This asymmetric clipboard policy prevents an exploit in the Office container from using the clipboard as a covert data exfiltration channel — a technique used by some document-borne malware.",
                Tags = ["office-guard", "clipboard", "asymmetric", "exfiltration", "container"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Clipboard is restricted to host → container only in Office Guard sessions. Content from the WDAG Office document cannot be copied to the host clipboard. Users cannot copy-paste content from untrusted documents to host applications — they must save the document and use it as a trusted document after scanning.",
                ApplyOps = [RegOp.SetDword(WdagKey, "OfficeguardClipboardSettings", 2)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "OfficeguardClipboardSettings")],
                DetectOps = [RegOp.CheckDword(WdagKey, "OfficeguardClipboardSettings", 2)],
            },
            new TweakDef
            {
                Id = "wdagpe-enforce-hardware-requirements",
                Label = "WDAG Persistence: Enforce Hardware Requirement Check Before Starting WDAG",
                Category = "Application Guard Persistence Policy",
                Description =
                    "Sets AppHVSIHardwareRequirementEnabled=1 in AppHVSI policy. Enables a pre-flight hardware compatibility check before WDAG containers are started. This check verifies that the CPU supports SLAT (for Hyper-V), VT-x/AMD-V is enabled in firmware, IOMMU is active (for DMA protection), and minimum RAM is available. If hardware requirements are not met, WDAG fails gracefully with a user-visible message rather than attempting to start a degraded container. Without this check, WDAG may start a container that appears functional but lacks proper isolation guarantees on incompatible hardware.",
                Tags = ["wdag", "hardware-check", "slat", "vt-x", "iommu"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "WDAG performs hardware compatibility check before starting a container. Incompatible hardware (no SLAT, no IOMMU, insufficient RAM) fails the check and WDAG does not start. Users on incompatible hardware see an error message. Prevents degraded container isolation on unsupported hardware.",
                ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIHardwareRequirementEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIHardwareRequirementEnabled")],
                DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIHardwareRequirementEnabled", 1)],
            },
            new TweakDef
            {
                Id = "wdagpe-enable-container-threat-report",
                Label = "WDAG Persistence: Enable WDAG Container Threat and Crash Reporting",
                Category = "Application Guard Persistence Policy",
                Description =
                    "Sets AppHVSIThreatReportEnabled=1 in AppHVSI policy. Enables WDAG to send threat and container crash reports to the Windows Defender ATP (Defender for Endpoint) service when a container experiences anomalous crashes or attempted security boundary violations. These reports include container crash minidumps, the URL that was active when the crash occurred, and whether the crash pattern is consistent with known exploit signatures. Threat reports enable the SOC to detect when WDAG has actually stopped an exploit attempt — containers that crash unexpectedly are almost always indicators of an exploitation attempt.",
                Tags = ["wdag", "threat-report", "crash", "mde", "exploit-detection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "Container crashes and boundary violations are reported to Defender for Endpoint. Reports include crash minidumps and active URL. Requires Defender for Endpoint licence (MDE P2). In regulated environments, confirm the crash minidump data handling meets data residency requirements.",
                ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIThreatReportEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIThreatReportEnabled")],
                DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIThreatReportEnabled", 1)],
            },
            new TweakDef
            {
                Id = "wdagpe-block-extension-from-container",
                Label = "WDAG Persistence: Block Browser Extension Usage in WDAG Container",
                Category = "Application Guard Persistence Policy",
                Description =
                    "Sets AppHVSIExtensionBlock=1 in AppHVSI policy. Prevents browser extensions from loading within the WDAG Edge container. Browser extensions are a significant attack surface — a malicious extension installed in the host browser that also loads in the WDAG container could behave as a covert channel, passing data between the container and the internet or between the container and the host. Blocking extensions in the container ensures the WDAG isolation is not weakened by extension code that has access to both the container's DOM and the extension API.",
                Tags = ["wdag", "extension-block", "browser-extension", "container", "covert-channel"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Browser extensions are disabled in the WDAG container. Extensions that users rely on (password managers, accessibility tools) are not available in WDAG sessions. Users who need extensions in the WDAG container must use trusted sites (which are outside the container).",
                ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIExtensionBlock", 1)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIExtensionBlock")],
                DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIExtensionBlock", 1)],
            },
            new TweakDef
            {
                Id = "wdagpe-enable-automatic-container-update",
                Label = "WDAG Persistence: Enable Automatic WDAG Container Image Update",
                Category = "Application Guard Persistence Policy",
                Description =
                    "Sets AppHVSIAutoUpdateEnabled=1 in AppHVSI policy. Enables automatic updates of the WDAG base container image via Windows Update. The WDAG container image is essentially a minimal Windows installation. If the container image is not updated, it may accumulate known vulnerabilities within the container OS components — which, while isolated, could be leveraged to escape the isolation more easily. Automatic updates ensure that even if an attacker gains code execution within the container, the container itself is patched against known privilege escalation vulnerabilities.",
                Tags = ["wdag", "auto-update", "container-image", "patch", "windows-update"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "WDAG container image is updated automatically via Windows Update. Container updates occur in the background without disrupting active sessions. Updates may require a WDAG service restart — active WDAG sessions may be disconnected when the container service restarts after an update.",
                ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIAutoUpdateEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIAutoUpdateEnabled")],
                DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIAutoUpdateEnabled", 1)],
            },
            new TweakDef
            {
                Id = "wdagpe-disable-container-proxy-bypass",
                Label = "WDAG Persistence: Disable Proxy Bypass Inside the WDAG Container",
                Category = "Application Guard Persistence Policy",
                Description =
                    "Sets AppHVSIProxyBypassEnabled=0 in AppHVSI policy. Prevents network traffic originating from within the WDAG container from bypassing the corporate proxy. Without this, a compromised website in the WDAG container that uses direct outbound connections (bypassing the proxy) can communicate with C2 infrastructure without appearing in proxy logs. Ensuring all container traffic goes through the corporate proxy enables DUT (Discover, Understand, Track) analysis of container network activity — even malicious connections from exploits are visible in proxy logs.",
                Tags = ["wdag", "proxy", "bypass-prevention", "network-monitoring", "c2-detection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "WDAG container traffic cannot bypass the corporate proxy. All outbound connections from the container are routed via the proxy. Requires the proxy configuration to be accessible from the isolated container network. Enables proxy-based detection of malicious container traffic.",
                ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIProxyBypassEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIProxyBypassEnabled")],
                DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIProxyBypassEnabled", 0)],
            },
            new TweakDef
            {
                Id = "wdagpe-set-container-network-isolation-level-2",
                Label = "WDAG Persistence: Set Container Network Isolation Level 2 (Restrict Host Communication)",
                Category = "Application Guard Persistence Policy",
                Description =
                    "Sets AppHVSINetworkIsolationLevel=2 in AppHVSI policy. Sets the WDAG container network isolation level to 2 (restrictive: container can only reach the proxy and the trusted domain list; host-to-container direct communication is blocked). At level 1, the container can communicate to any internet address via the host proxy. At level 2, only the enterprise proxy endpoint and explicitly whitelisted external domains are reachable from the container network. This reduces the C2 communication surface — a compromised WDAG session can only reach domains the enterprise has explicitly permitted.",
                Tags = ["wdag", "network-isolation", "level-2", "c2-restriction", "outbound"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Container outbound network is restricted to the proxy and whitelisted domains. Websites accessed through WDAG can only reach the internet via the approved proxy. Container network level 2 may break some websites that use direct connections or non-standard ports. Monitor proxy logs for blocked connection attempts.",
                ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSINetworkIsolationLevel", 2)],
                RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSINetworkIsolationLevel")],
                DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSINetworkIsolationLevel", 2)],
            },
        ];
}
