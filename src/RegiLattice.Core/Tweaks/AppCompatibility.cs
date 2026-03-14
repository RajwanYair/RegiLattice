namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Application compatibility and experience tweaks — disables compatibility telemetry,
/// removes compatibility assistants, and configures app execution policies.
/// </summary>
internal static class AppCompatibility
{
    private const string CuKey = @"HKEY_CURRENT_USER";
    private const string LmKey = @"HKEY_LOCAL_MACHINE";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "compat-disable-compatibility-telemetry",
            Label = "Disable Application Compatibility Telemetry",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Application Experience telemetry component (CompatTelRunner.exe) that collects app usage data.",
            Tags = ["compatibility", "telemetry", "privacy", "performance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable", 0)],
        },
        new TweakDef
        {
            Id = "compat-disable-program-compatibility-assistant",
            Label = "Disable Program Compatibility Assistant",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables PCA which monitors programs and offers to apply compatibility fixes. Can slow down program launches.",
            Tags = ["compatibility", "performance", "pca"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-steps-recorder",
            Label = "Disable Steps Recorder (PSR)",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Problem Steps Recorder (PSR.exe) used for recording user actions for troubleshooting. Reduces background resource usage.",
            Tags = ["compatibility", "privacy", "recorder", "troubleshooting"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-inventory-collector",
            Label = "Disable Application Inventory Collector",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the inventory collector that scans installed applications and sends data to Microsoft for compatibility assessment.",
            Tags = ["compatibility", "telemetry", "privacy", "inventory"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-engine",
            Label = "Disable Compatibility Engine",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Completely disables the Application Compatibility Engine. Programs will run without any compatibility shims applied.",
            Tags = ["compatibility", "performance", "engine"],
            SideEffects = "Some older applications may not function correctly.",
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-switchback",
            Label = "Disable SwitchBack Compatibility",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables SwitchBack compatibility, which reverts some system behaviour for older apps. Improves consistency on modern systems.",
            Tags = ["compatibility", "performance", "switchback"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "SbEnable", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "SbEnable")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "SbEnable", 0)],
        },
        new TweakDef
        {
            Id = "compat-disable-web-search-in-run",
            Label = "Disable Web Search in Run Dialog",
            Category = "App Compatibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the Run dialog from searching the web when a command is not found locally.",
            Tags = ["compatibility", "privacy", "search", "run"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRunasInstallPrompt", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRunasInstallPrompt")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRunasInstallPrompt", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-fault-tolerant-heap",
            Label = "Disable Fault Tolerant Heap",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Fault Tolerant Heap (FTH) which Windows enables for apps that crash frequently. FTH adds overhead.",
            Tags = ["compatibility", "performance", "heap", "memory"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\FTH"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\FTH", "Enabled", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\FTH", "Enabled", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\FTH", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "compat-disable-customer-experience",
            Label = "Disable Customer Experience Improvement Program",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows CEIP which collects hardware and software usage data for quality improvement.",
            Tags = ["compatibility", "telemetry", "privacy", "ceip"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\SQMClient\Windows"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\SQMClient\Windows", "CEIPEnable", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
        },
        new TweakDef
        {
            Id = "compat-disable-smart-screen-apps",
            Label = "Disable SmartScreen for Downloaded Apps",
            Category = "App Compatibility",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Disables SmartScreen checking for apps downloaded from the Internet. Speeds up app launches.",
            Tags = ["compatibility", "smartscreen", "security", "performance"],
            SideEffects = "Reduced protection against unrecognised executables.",
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps = [RegOp.SetString($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer", "SmartScreenEnabled", "Off")],
            RemoveOps = [RegOp.SetString($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer", "SmartScreenEnabled", "Warn")],
            DetectOps = [RegOp.CheckString($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer", "SmartScreenEnabled", "Off")],
        },
    ];
}
