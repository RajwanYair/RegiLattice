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
    ];
}
