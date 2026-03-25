#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 261 — Windows Sandbox Policy (10 tweaks)
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\Sandbox
// Governs the Windows Sandbox optional component (isolated throwaway desktop session).
// Slug: "sandbox-"
internal static class WindowsSandboxPolicy
{
    private const string SandboxKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sbpol-disable-sandbox",
            Label = "Disable Windows Sandbox",
            Category = "Windows Sandbox Policy",
            Description = "Sets AllowSandbox=0 in the Sandbox policy key. "
                + "Disables Windows Sandbox entirely via Group Policy. Windows Sandbox is a "
                + "lightweight isolated virtual environment for running untrusted applications. "
                + "While beneficial for security testing, it requires Hyper-V and consumes significant "
                + "CPU/RAM. Organisations that do not need it can disable the feature via GPO rather "
                + "than relying solely on the optional-features toggle. "
                + "Default: absent (Sandbox allowed if feature is installed). "
                + "Recommended: 0 on production servers and systems where Sandbox is not needed.",
            Tags = ["sandbox", "virtualization", "isolation", "policy", "hyperv"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Windows Sandbox blocked by policy; cannot be used even when the optional feature is installed.",
            ApplyOps  = [RegOp.SetDword(SandboxKey, "AllowSandbox", 0)],
            RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowSandbox")],
            DetectOps = [RegOp.CheckDword(SandboxKey, "AllowSandbox", 0)],
        },
        new TweakDef
        {
            Id = "sbpol-disable-networking",
            Label = "Disable Networking Inside Windows Sandbox",
            Category = "Windows Sandbox Policy",
            Description = "Sets AllowNetworking=0 in the Sandbox policy key. "
                + "Prevents the Windows Sandbox environment from accessing the host's network adapters "
                + "or the internet. Running untrusted applications with live network access allows "
                + "them to exfiltrate data, phone home for additional payloads, or probe internal "
                + "network services. Disabling Sandbox networking creates a true air-gap for testing. "
                + "Default: absent (networking enabled in Sandbox). "
                + "Recommended: 0 to create an isolated analysis environment.",
            Tags = ["sandbox", "networking", "isolation", "air-gap", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Sandbox has no network access; untrusted apps cannot reach the internet or internal network.",
            ApplyOps  = [RegOp.SetDword(SandboxKey, "AllowNetworking", 0)],
            RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowNetworking")],
            DetectOps = [RegOp.CheckDword(SandboxKey, "AllowNetworking", 0)],
        },
        new TweakDef
        {
            Id = "sbpol-disable-clipboard",
            Label = "Disable Clipboard Sharing With Windows Sandbox",
            Category = "Windows Sandbox Policy",
            Description = "Sets AllowClipboardRedirection=0 in the Sandbox policy key. "
                + "Blocks bi-directional clipboard sharing between the Windows Sandbox and the host OS. "
                + "Without this restriction, content copied inside the sandbox (e.g., stolen credentials "
                + "or extracted secrets harvested by a malicious app) can be trivially transferred to "
                + "the host by pasting. Similarly, sensitive content from the host can be accidentally "
                + "pasted into untrustworthy sandbox applications. "
                + "Default: absent (clipboard shared). Recommended: 0 for true isolation.",
            Tags = ["sandbox", "clipboard", "data-exfiltration", "isolation", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Copy/paste between Sandbox and host blocked in both directions.",
            ApplyOps  = [RegOp.SetDword(SandboxKey, "AllowClipboardRedirection", 0)],
            RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowClipboardRedirection")],
            DetectOps = [RegOp.CheckDword(SandboxKey, "AllowClipboardRedirection", 0)],
        },
        new TweakDef
        {
            Id = "sbpol-disable-vgpu",
            Label = "Disable Virtualized GPU (vGPU) in Windows Sandbox",
            Category = "Windows Sandbox Policy",
            Description = "Sets AllowVGPU=0 in the Sandbox policy key. "
                + "Disables GPU hardware acceleration (virtualised GPU) inside Windows Sandbox. "
                + "vGPU in Sandbox allows the containerised environment to access the system's "
                + "graphics adapter for accelerated rendering. However, graphics driver vulnerabilities "
                + "exposed through hypervisor shared vGPU paths have historically been VM-escape attack "
                + "vectors. Disabling vGPU forces software rendering inside Sandbox, eliminating this "
                + "attack surface at the cost of rendering performance. "
                + "Default: absent (vGPU on). Recommended: 0 for maximum isolation.",
            Tags = ["sandbox", "vgpu", "gpu", "vm-escape", "isolation", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Sandbox uses software rendering; GPU escape paths eliminated, performance reduced.",
            ApplyOps  = [RegOp.SetDword(SandboxKey, "AllowVGPU", 0)],
            RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowVGPU")],
            DetectOps = [RegOp.CheckDword(SandboxKey, "AllowVGPU", 0)],
        },
        new TweakDef
        {
            Id = "sbpol-disable-printer-redirection",
            Label = "Disable Printer Redirection Into Windows Sandbox",
            Category = "Windows Sandbox Policy",
            Description = "Sets AllowPrinterRedirection=0 in the Sandbox policy key. "
                + "Prevents printers that are in scope on the host from being redirected and made "
                + "available inside the Windows Sandbox session. Printer redirection exposes the "
                + "host's print subsystem to the sandbox, and a malicious application could use it "
                + "to print documents, discover IP print server addresses, or trigger driver invocations "
                + "against the host print spooler. Blocking redirected printing tightens isolation. "
                + "Default: absent (printers accessible in Sandbox). Recommended: 0 for isolation.",
            Tags = ["sandbox", "printing", "isolation", "spooler", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "No host printers visible inside Windows Sandbox; print spooler not exposed.",
            ApplyOps  = [RegOp.SetDword(SandboxKey, "AllowPrinterRedirection", 0)],
            RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowPrinterRedirection")],
            DetectOps = [RegOp.CheckDword(SandboxKey, "AllowPrinterRedirection", 0)],
        },
        new TweakDef
        {
            Id = "sbpol-disable-audio-input",
            Label = "Disable Microphone (Audio Input) in Windows Sandbox",
            Category = "Windows Sandbox Policy",
            Description = "Sets AllowAudioInput=0 in the Sandbox policy key. "
                + "Blocks the microphone and other audio capture devices from being accessible inside "
                + "the Windows Sandbox session. An untrusted application running in Sandbox could use "
                + "microphone access to capture ambient audio (conversation recording, credential "
                + "dictation) and exfiltrate it if networking is also enabled. Disabling microphone "
                + "inside Sandbox removes this passive surveillance capability. "
                + "Default: absent (microphone accessible). Recommended: 0 for privacy.",
            Tags = ["sandbox", "microphone", "audio", "privacy", "surveillance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Microphone not accessible inside Windows Sandbox; audio capture by sandboxed apps prevented.",
            ApplyOps  = [RegOp.SetDword(SandboxKey, "AllowAudioInput", 0)],
            RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowAudioInput")],
            DetectOps = [RegOp.CheckDword(SandboxKey, "AllowAudioInput", 0)],
        },
        new TweakDef
        {
            Id = "sbpol-disable-video-input",
            Label = "Disable Camera (Video Input) in Windows Sandbox",
            Category = "Windows Sandbox Policy",
            Description = "Sets AllowVideoInput=0 in the Sandbox policy key. "
                + "Blocks webcams and other video capture devices from being visible inside the "
                + "Windows Sandbox session. A sandboxed malicious application with camera access "
                + "could silently capture images or video of the user's environment and exfiltrate it. "
                + "Disabling video input in Sandbox eliminates this covert visual surveillance path "
                + "without affecting the host system's camera access outside Sandbox. "
                + "Default: absent (camera accessible). Recommended: 0 for privacy.",
            Tags = ["sandbox", "camera", "video", "privacy", "surveillance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Camera not accessible inside Windows Sandbox; video capture by sandboxed apps prevented.",
            ApplyOps  = [RegOp.SetDword(SandboxKey, "AllowVideoInput", 0)],
            RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowVideoInput")],
            DetectOps = [RegOp.CheckDword(SandboxKey, "AllowVideoInput", 0)],
        },
        new TweakDef
        {
            Id = "sbpol-disable-mapped-folders",
            Label = "Disable Host Folder Mapping Into Windows Sandbox",
            Category = "Windows Sandbox Policy",
            Description = "Sets AllowMappedFolders=0 in the Sandbox policy key. "
                + "Prevents host filesystem folders from being mapped and shared into the Windows Sandbox "
                + "environment via the Sandbox configuration file (WSB). Without this restriction, a "
                + "carefully crafted .wsb config file can mount sensitive host directories (Documents, "
                + "Desktop, code repositories) as writable shares inside Sandbox, enabling a malicious "
                + "payload to read or modify host files. Blocking mapped folders enforces full filesystem "
                + "isolation. Default: absent (folder mapping allowed). Recommended: 0 for strict isolation.",
            Tags = ["sandbox", "filesystem", "folder-mapping", "isolation", "data-access", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Host filesystem folders cannot be mapped into Sandbox; .wsb MappedFolders entries blocked.",
            ApplyOps  = [RegOp.SetDword(SandboxKey, "AllowMappedFolders", 0)],
            RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowMappedFolders")],
            DetectOps = [RegOp.CheckDword(SandboxKey, "AllowMappedFolders", 0)],
        },
        new TweakDef
        {
            Id = "sbpol-disable-mapped-folders-write",
            Label = "Restrict Mapped Sandbox Folders to Read-Only",
            Category = "Windows Sandbox Policy",
            Description = "Sets AllowWritableSharedFolders=0 in the Sandbox policy key. "
                + "Even when folder mapping is allowed (AllowMappedFolders=1), this policy ensures "
                + "that all mapped host folders are mounted as read-only inside the Sandbox. "
                + "A sandboxed application can read shared files but cannot write back to the host "
                + "filesystem. This provides a middle ground: files can be passed into Sandbox for "
                + "analysis without allowing the Sandbox to modify or delete them. "
                + "Default: absent (writable mapping permitted). Recommended: 0 if mapping is enabled.",
            Tags = ["sandbox", "filesystem", "read-only", "folder-mapping", "isolation", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Mapped host folders accessible read-only inside Sandbox; sandbox cannot write to host filesystem.",
            ApplyOps  = [RegOp.SetDword(SandboxKey, "AllowWritableSharedFolders", 0)],
            RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowWritableSharedFolders")],
            DetectOps = [RegOp.CheckDword(SandboxKey, "AllowWritableSharedFolders", 0)],
        },
        new TweakDef
        {
            Id = "sbpol-restrict-logon-credentials",
            Label = "Block Windows Logon Credential Exposure in Windows Sandbox",
            Category = "Windows Sandbox Policy",
            Description = "Sets AllowLogonCredentials=0 in the Sandbox policy key. "
                + "Prevents Windows from passing or forwarding the user's login credentials (tokens, "
                + "tickets, or cached NTLM hashes) into the Windows Sandbox environment. Without this "
                + "setting, a sandboxed process may be able to leverage inherited authentication tokens "
                + "to access domain resources or network shares as the current user. Blocking credential "
                + "propagation ensures that a compromised Sandbox cannot pivot to domain resources. "
                + "Default: absent. Recommended: 0 on domain-joined systems.",
            Tags = ["sandbox", "credentials", "ntlm", "token", "domain", "isolation", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Login credentials not forwarded into Sandbox; sandboxed apps cannot use domain identity.",
            ApplyOps  = [RegOp.SetDword(SandboxKey, "AllowLogonCredentials", 0)],
            RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowLogonCredentials")],
            DetectOps = [RegOp.CheckDword(SandboxKey, "AllowLogonCredentials", 0)],
        },
    ];
}
