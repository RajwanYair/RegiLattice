// RegiLattice.Core — Tweaks/AppVirtualization.cs
// Microsoft App-V (Application Virtualization) client policy settings (Sprint 129, T8.2).
// Slug "appv" — Microsoft App-V 5.x / App-V for Windows client policies.
// Manages package delivery, streaming, shell integration, and usage reporting.
// Distinct from Virtualization.cs (Hyper-V/WSL), AppxPolicy.cs (MSIX store apps).
// NOTE: App-V is only active when the App-V client feature is installed.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AppVirtualization
{
    private const string Client = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppV\Client";
    private const string Streaming = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppV\Client\Streaming";
    private const string Integration =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppV\Client\Integration";
    private const string Reporting = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppV\Client\Reporting";
    private const string Virtualization =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppV\Client\Virtualization";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "appv-enable-package-scripts",
            Label = "Allow Scripts Inside App-V Packages",
            Category = "App-V Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["appv", "scripts", "virtualization", "packages", "policy"],
            Description =
                "Permits PowerShell and batch scripts embedded within App-V packages to execute. "
                + "Required for complex applications that use scripts for first-run configuration, "
                + "licence activation, or environment setup. EnablePackageScripts=1.",
            ApplyOps = [RegOp.SetDword(Client, "EnablePackageScripts", 1)],
            RemoveOps = [RegOp.DeleteValue(Client, "EnablePackageScripts")],
            DetectOps = [RegOp.CheckDword(Client, "EnablePackageScripts", 1)],
        },
        new TweakDef
        {
            Id = "appv-block-high-cost-launch",
            Label = "Block App-V Package Launch on Metered Connections",
            Category = "App-V Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["appv", "metered", "launch", "streaming", "cost"],
            Description =
                "Prevents App-V packages from streaming content over metered network connections "
                + "(AllowHighCostLaunch=0). Avoids unexpected data charges when users roam on mobile "
                + "broadband. Packages that are already fully cached on disk still launch normally.",
            ApplyOps = [RegOp.SetDword(Client, "AllowHighCostLaunch", 0)],
            RemoveOps = [RegOp.DeleteValue(Client, "AllowHighCostLaunch")],
            DetectOps = [RegOp.CheckDword(Client, "AllowHighCostLaunch", 0)],
        },
        new TweakDef
        {
            Id = "appv-require-admin-to-publish",
            Label = "Require Admin Rights to Publish App-V Packages Globally",
            Category = "App-V Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["appv", "publish", "admin", "security", "policy"],
            Description =
                "Restricts global (all-user) App-V package publication to administrators only "
                + "(RequirePublishAsAdmin=1). Standard users can still publish packages to their own "
                + "profile. Prevents employees from self-publishing unvetted virtualised applications.",
            ApplyOps = [RegOp.SetDword(Client, "RequirePublishAsAdmin", 1)],
            RemoveOps = [RegOp.DeleteValue(Client, "RequirePublishAsAdmin")],
            DetectOps = [RegOp.CheckDword(Client, "RequirePublishAsAdmin", 1)],
        },
        new TweakDef
        {
            Id = "appv-autoload-previously-used",
            Label = "Auto-Load Previously Used App-V Packages in Background",
            Category = "App-V Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["appv", "autoload", "background", "streaming", "performance"],
            Description =
                "Configures the App-V streaming engine to proactively background-load packages that the "
                + "user has previously launched (AutoLoad=1, previously-used packages only). Improves "
                + "subsequent launch times by ensuring packages are fully cached before the user needs them.",
            ApplyOps = [RegOp.SetDword(Streaming, "AutoLoad", 1)],
            RemoveOps = [RegOp.DeleteValue(Streaming, "AutoLoad")],
            DetectOps = [RegOp.CheckDword(Streaming, "AutoLoad", 1)],
        },
        new TweakDef
        {
            Id = "appv-disable-shared-content-store",
            Label = "Disable App-V Shared Content Store Mode",
            Category = "App-V Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["appv", "content-store", "disk", "streaming", "cache"],
            Description =
                "Disables Shared Content Store (SCS) mode which streams content directly from the "
                + "App-V server without local caching (SharedContentStoreMode=0). Enables full local "
                + "caching for improved offline capability and resiliency when the App-V server is "
                + "unavailable.",
            ApplyOps = [RegOp.SetDword(Streaming, "SharedContentStoreMode", 0)],
            RemoveOps = [RegOp.DeleteValue(Streaming, "SharedContentStoreMode")],
            DetectOps = [RegOp.CheckDword(Streaming, "SharedContentStoreMode", 0)],
        },
        new TweakDef
        {
            Id = "appv-enable-process-interop",
            Label = "Enable App-V Process Interoperability",
            Category = "App-V Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["appv", "interop", "process", "integration", "virtual"],
            Description =
                "Allows virtualised App-V processes to interoperate with natively installed processes "
                + "outside the virtual environment (EnableProcessInterop=1). Required for scenarios "
                + "where virtualised applications need to interact with local tools like printers, "
                + "scanners, or on-device helper utilities.",
            ApplyOps = [RegOp.SetDword(Integration, "EnableProcessInterop", 1)],
            RemoveOps = [RegOp.DeleteValue(Integration, "EnableProcessInterop")],
            DetectOps = [RegOp.CheckDword(Integration, "EnableProcessInterop", 1)],
        },
        new TweakDef
        {
            Id = "appv-block-virtual-com-objects",
            Label = "Block Virtual COM Object Creation from App-V",
            Category = "App-V Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["appv", "com", "virtual", "objects", "security"],
            Description =
                "Prevents App-V virtualised applications from creating out-of-process COM objects that "
                + "would be visible to native (non-virtualised) processes (AllowVirtualCOMObjectCreation=0). "
                + "Reduces COM-based code injection and isolation boundary escape attack surface.",
            ApplyOps = [RegOp.SetDword(Virtualization, "AllowVirtualCOMObjectCreation", 0)],
            RemoveOps = [RegOp.DeleteValue(Virtualization, "AllowVirtualCOMObjectCreation")],
            DetectOps = [RegOp.CheckDword(Virtualization, "AllowVirtualCOMObjectCreation", 0)],
        },
        new TweakDef
        {
            Id = "appv-enable-reporting",
            Label = "Enable App-V Usage Reporting",
            Category = "App-V Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["appv", "reporting", "telemetry", "usage", "analytics"],
            Description =
                "Enables App-V client usage reporting which sends package launch, error, and access "
                + "telemetry to the App-V management server (EnableReporting=1). Provides IT with "
                + "application usage visibility for licence compliance and deployment health monitoring.",
            ApplyOps = [RegOp.SetDword(Reporting, "EnableReporting", 1)],
            RemoveOps = [RegOp.DeleteValue(Reporting, "EnableReporting")],
            DetectOps = [RegOp.CheckDword(Reporting, "EnableReporting", 1)],
        },
        new TweakDef
        {
            Id = "appv-reporting-interval-24h",
            Label = "Set App-V Reporting Upload Interval to 24 Hours",
            Category = "App-V Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["appv", "reporting", "interval", "upload", "schedule"],
            Description =
                "Sets the App-V client reporting upload interval to 24 hours (ReportingInterval=1440 "
                + "minutes). Reduces reporting traffic overhead while ensuring daily freshness of usage "
                + "data on the management server. Requires EnableReporting=1 to take effect.",
            ApplyOps = [RegOp.SetDword(Reporting, "ReportingInterval", 1440)],
            RemoveOps = [RegOp.DeleteValue(Reporting, "ReportingInterval")],
            DetectOps = [RegOp.CheckDword(Reporting, "ReportingInterval", 1440)],
        },
        new TweakDef
        {
            Id = "appv-streaming-timeout-120s",
            Label = "Set App-V Streaming Connection Timeout to 120 Seconds",
            Category = "App-V Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["appv", "streaming", "timeout", "network", "performance"],
            Description =
                "Sets the App-V streaming connection timeout to 120 seconds (StreamingConnectionTimeout=120). "
                + "On slow WAN links to the server, the default 30-second timeout causes premature failures. "
                + "A longer timeout prevents 'Application failed to initialize' errors over high-latency links.",
            ApplyOps = [RegOp.SetDword(Streaming, "StreamingConnectionTimeout", 120)],
            RemoveOps = [RegOp.DeleteValue(Streaming, "StreamingConnectionTimeout")],
            DetectOps = [RegOp.CheckDword(Streaming, "StreamingConnectionTimeout", 120)],
        },
    ];
}
