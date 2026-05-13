namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── Merged from WindowsSandboxAdv.cs ──────────────────────────────────────────────────

[TweakModule]
internal static class WindowsSandboxAdv
{
    private const string SbPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox";
    private const string SbSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\cmstp";
    private const string ContainerPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Container";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sandbox-disable-printer-sharing",
            Label = "Disable Windows Sandbox Printer Redirection",
            Category = "Virtualization",
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
            Id = "sandbox-set-memory-limit",
            Label = "Set Windows Sandbox Memory Limit (4 GB Cap)",
            Category = "Virtualization",
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
            Id = "sandbox-disable-microphone",
            Label = "Disable Microphone Input in Windows Sandbox",
            Category = "Virtualization",
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
            Id = "sandbox-cap-cpu-count-2",
            Label = "Cap Windows Sandbox CPU Count to 2 Cores",
            Category = "Virtualization",
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
            Category = "Virtualization",
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
            Id = "sandbox-disable-cortana-search",
            Label = "Disable Cortana / Windows Search Inside Sandbox",
            Category = "Virtualization",
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
            Category = "Virtualization",
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
            Category = "Virtualization",
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
            Category = "Virtualization",
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
