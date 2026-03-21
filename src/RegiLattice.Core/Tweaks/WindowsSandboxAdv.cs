// RegiLattice.Core — Tweaks/WindowsSandboxAdv.cs
// Advanced Windows Sandbox configuration via registry and MSCONFIG/policy paths.
// Slug: "sandbox" — Windows Sandbox networking, vGPU, clipboard, audio integration.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsSandboxAdv
{
    private const string SbPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox";
    private const string SbSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\cmstp";
    private const string ContainerPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Container";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sandbox-disable-networking",
            Label = "Disable Windows Sandbox Networking",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "security", "network", "isolation"],
            Description =
                "Disables network access inside Windows Sandbox via Group Policy. "
                + "Useful when analysing potentially malicious files and you don't want the sample to phone home or download payloads.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowNetworking", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowNetworking")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowNetworking", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-vgpu",
            Label = "Disable Windows Sandbox vGPU (Protect Host GPU Driver)",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "security", "gpu", "vgpu", "isolation"],
            Description =
                "Disables GPU virtualization for Windows Sandbox. "
                + "Prevents sandbox workloads from accessing the host GPU driver attack surface. "
                + "Sandbox falls back to software rendering — slower but more isolated.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowVGPU", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowVGPU")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowVGPU", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-clipboard",
            Label = "Disable Windows Sandbox Clipboard Sharing",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "security", "clipboard", "isolation", "data exfiltration"],
            Description =
                "Prevents clipboard data from being shared between the host and the sandbox. "
                + "Stops malicious code inside the sandbox from reading sensitive clipboard contents (passwords, tokens) from the host.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowClipboardRedirection", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowClipboardRedirection")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowClipboardRedirection", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-audio",
            Label = "Disable Windows Sandbox Audio Input",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "security", "audio", "microphone", "isolation"],
            Description =
                "Disables microphone and audio input redirection into Windows Sandbox. "
                + "Prevents malware inside the sandbox from accessing the host's microphone to eavesdrop.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowAudioInput", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowAudioInput")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowAudioInput", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-video-input",
            Label = "Disable Windows Sandbox Camera/Video Input",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "security", "camera", "video", "privacy", "isolation"],
            Description =
                "Disables camera and video input device redirection into Windows Sandbox. "
                + "Prevents code inside the sandbox from activating the host's camera.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowVideoInput", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowVideoInput")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowVideoInput", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-printer-sharing",
            Label = "Disable Windows Sandbox Printer Redirection",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "security", "printer", "isolation"],
            Description =
                "Prevents host printers from being visible inside Windows Sandbox. "
                + "Stops documents produced inside the sandbox from being printed on the host network.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowPrintAccess", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowPrintAccess")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowPrintAccess", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-protect-client-folders",
            Label = "Restrict Windows Sandbox Mapped Folder Write Access",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "security", "mapped folders", "write protection", "isolation"],
            Description =
                "Enforces read-only mode for all host folders mapped into Windows Sandbox by policy. "
                + "Prevents executed code from writing back to host filesystem through shared directories.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowMappedFolders", 1), RegOp.SetDword(SbPol, "AllowWriteToMappedFolders", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowWriteToMappedFolders")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowWriteToMappedFolders", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-set-memory-limit",
            Label = "Set Windows Sandbox Memory Limit (4 GB Cap)",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["sandbox", "performance", "memory", "resource limit"],
            Description =
                "Caps the maximum memory Windows Sandbox can consume at 4096 MB. "
                + "Prevents sandbox workloads from starving the host of RAM during analysis sessions.",
            ApplyOps = [RegOp.SetDword(SbPol, "MemoryInMB", 4096)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "MemoryInMB")],
            DetectOps = [RegOp.CheckDword(SbPol, "MemoryInMB", 4096)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-all-folder-mapping",
            Label = "Disable All Host Folder Mapping in Sandbox",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["sandbox", "isolation", "folders", "security"],
            Description =
                "Completely blocks host folder sharing into Windows Sandbox via Group Policy. "
                + "Combined with the write-protect tweak this provides the strongest "
                + "isolation: no host files are accessible from within the sandbox at all.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowMappedFolders", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowMappedFolders")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowMappedFolders", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-microphone",
            Label = "Disable Microphone Input in Windows Sandbox",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "microphone", "audio", "isolation", "privacy"],
            Description =
                "Prevents sandboxed applications from accessing the host microphone. "
                + "Complements the existing audio-disable tweak with dedicated "
                + "mic-only policy for environments that allow speaker pass-through.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowMicrophoneInput", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowMicrophoneInput")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowMicrophoneInput", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-enable-protected-client",
            Label = "Enable Protected Client Mode for Windows Sandbox",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "protected client", "isolation", "security"],
            Description =
                "Runs the Sandbox's RDP client in Windows Protected Process Light (PPL) "
                + "mode. This blocks injection into the sandbox session host process "
                + "from even admin-level processes on the host.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowProtectedClient", 1)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowProtectedClient")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowProtectedClient", 1)],
        },
        new TweakDef
        {
            Id = "sandbox-cap-cpu-count-2",
            Label = "Cap Windows Sandbox CPU Count to 2 Cores",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "cpu", "performance", "resource limit"],
            Description =
                "Limits Windows Sandbox to 2 logical processor cores via Group Policy. "
                + "Prevents sandbox workloads from monopolising the host CPU during "
                + "malware analysis or web isolation sessions.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowedCPUCount", 2)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowedCPUCount")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowedCPUCount", 2)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-windows-installer",
            Label = "Disable Windows Installer (MSI) Inside Sandbox",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "msi", "installer", "security", "policy"],
            Description =
                "Prevents the Windows Installer service from running inside the sandbox. "
                + "Restricts sandboxed sessions to portable/xcopy deployments and "
                + "blocks installer-level exploit techniques.",
            ApplyOps = [RegOp.SetDword(ContainerPol, "DisableWindowsInstaller", 1)],
            RemoveOps = [RegOp.DeleteValue(ContainerPol, "DisableWindowsInstaller")],
            DetectOps = [RegOp.CheckDword(ContainerPol, "DisableWindowsInstaller", 1)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-telemetry",
            Label = "Disable Telemetry and Diagnostics Inside Sandbox",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "telemetry", "privacy", "diagnostics"],
            Description =
                "Turns off Windows telemetry and diagnostic data collection "
                + "within the sandbox environment. Useful for clean-slate testing "
                + "and prevents telemetry from leaking sandbox activity to Microsoft.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-cortana-search",
            Label = "Disable Cortana / Windows Search Inside Sandbox",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "cortana", "search", "privacy"],
            Description =
                "Prevents Cortana and Windows Search from running inside the sandbox, "
                + "cutting network calls to Bing and reducing idle CPU usage in "
                + "sandboxed sessions.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowCortana", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowCortana")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowCortana", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-location-service",
            Label = "Disable Location Service Inside Sandbox",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "location", "gps", "privacy"],
            Description =
                "Blocks location API calls from applications running inside "
                + "Windows Sandbox. Prevents location-aware malware from "
                + "detecting the sandbox environment via geolocation discrepancies.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowLocationService", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowLocationService")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowLocationService", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-set-idle-timeout-1h",
            Label = "Set Windows Sandbox Idle Timeout to 1 Hour",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "idle timeout", "session", "resource"],
            Description =
                "Automatically terminates an idle Windows Sandbox session after "
                + "3600 seconds (1 hour), reclaiming RAM and CPU resources. "
                + "Prevents forgotten sandbox sessions from running indefinitely.",
            ApplyOps = [RegOp.SetDword(SbPol, "IdleTimeoutInSeconds", 3600)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "IdleTimeoutInSeconds")],
            DetectOps = [RegOp.CheckDword(SbPol, "IdleTimeoutInSeconds", 3600)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-logon-screensaver",
            Label = "Disable Screensaver Inside Windows Sandbox",
            Category = "Windows Sandbox",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "screensaver", "display", "session"],
            Description =
                "Disables the screensaver within the Windows Sandbox session to "
                + "prevent the sandbox display from blanking during automated "
                + "analysis tasks or long-running test scripts.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowScreenSaver", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowScreenSaver")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowScreenSaver", 0)],
        },
    ];
}
