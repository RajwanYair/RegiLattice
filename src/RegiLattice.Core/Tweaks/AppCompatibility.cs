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
        new TweakDef
        {
            Id = "compat-disable-app-launch-tracking",
            Label = "Disable App Launch Tracking",
            Category = "App Compatibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables tracking of application launches used for Start menu personalisation and telemetry.",
            Tags = ["compatibility", "privacy", "tracking", "start-menu"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
            RemoveOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 1)],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
        },
        new TweakDef
        {
            Id = "compat-disable-autoplay-devices",
            Label = "Disable AutoPlay for Non-Volume Devices",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables AutoPlay for non-volume devices like cameras and phones. Prevents automatic import prompts.",
            Tags = ["compatibility", "security", "autoplay", "devices"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutoplayfornonVolume", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutoplayfornonVolume")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutoplayfornonVolume", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-maintenance-wakeup",
            Label = "Disable Automatic Maintenance Wake-Up",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows automatic maintenance from waking the computer from sleep.",
            Tags = ["compatibility", "performance", "maintenance", "sleep", "power"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 0)],
        },
        new TweakDef
        {
            Id = "compat-set-diagnostic-data-basic",
            Label = "Set Diagnostic Data to Basic (Required Only)",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets Windows diagnostic data collection to Basic level, sending only required telemetry data.",
            Tags = ["compatibility", "telemetry", "privacy", "diagnostics"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "compat-force-classic-shutdown",
            Label = "Force Classic Shutdown Dialog",
            Category = "App Compatibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces the classic shutdown dialog (Alt+F4 on desktop) instead of the modern one.",
            Tags = ["compatibility", "ui", "shutdown", "classic"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoWinKeys", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoWinKeys")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoWinKeys", 0)],
        },
        new TweakDef
        {
            Id = "compat-disable-background-apps",
            Label = "Disable Background Apps (Global)",
            Category = "App Compatibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents UWP/Store apps from running in the background. Reduces CPU and memory usage from idle apps.",
            Tags = ["compatibility", "performance", "background", "uwp", "memory"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications", "GlobalUserDisabled", 1)],
            RemoveOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications", "GlobalUserDisabled", 0)],
            DetectOps =
            [
                RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications", "GlobalUserDisabled", 1),
            ],
        },
        new TweakDef
        {
            Id = "compat-disable-tips-suggestions",
            Label = "Disable Tips and Suggestions Notifications",
            Category = "App Compatibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows tips, tricks, and suggestion notifications that appear in the Action Center.",
            Tags = ["compatibility", "notifications", "tips", "suggestions", "ui"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338389Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled"),
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338389Enabled"),
            ],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "compat-disable-shim-database",
            Label = "Disable Application Shim Database (SDB)",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the application compatibility shim database that applies runtime fixes for older software.",
            Tags = ["compatibility", "performance", "shim", "legacy"],
            SideEffects = "Some older applications may not function correctly without shims.",
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePropPage", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePropPage")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePropPage", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-wer-server-connection",
            Label = "Disable WER Server Connection",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows Error Reporting from connecting to Microsoft servers to upload crash dumps and diagnostic data.",
            Tags = ["compatibility", "crash", "wer", "privacy", "telemetry"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-compat-telemetry-runner",
            Label = "Disable CompatTelRunner Scheduled Tasks",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Microsoft Compatibility Appraiser and CompatTelRunner tasks that upload telemetry during idle time.",
            Tags = ["compatibility", "telemetry", "scheduled-task", "privacy"],
            ApplyAction = dryRun =>
            {
                if (!dryRun)
                {
                    ShellRunner.Run(
                        "schtasks",
                        ["/Change", "/TN", @"Microsoft\Windows\Application Experience\Microsoft Compatibility Appraiser", "/DISABLE"]
                    );
                    ShellRunner.Run("schtasks", ["/Change", "/TN", @"Microsoft\Windows\Application Experience\ProgramDataUpdater", "/DISABLE"]);
                }
            },
            RemoveAction = dryRun =>
            {
                if (!dryRun)
                {
                    ShellRunner.Run(
                        "schtasks",
                        ["/Change", "/TN", @"Microsoft\Windows\Application Experience\Microsoft Compatibility Appraiser", "/ENABLE"]
                    );
                    ShellRunner.Run("schtasks", ["/Change", "/TN", @"Microsoft\Windows\Application Experience\ProgramDataUpdater", "/ENABLE"]);
                }
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run(
                    "schtasks",
                    ["/Query", "/TN", @"Microsoft\Windows\Application Experience\Microsoft Compatibility Appraiser", "/FO", "LIST"]
                );
                return stdout.Contains("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "compat-disable-user-choice-protection",
            Label = "Disable User Choice Protection Driver (UCPD)",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the User Choice Protection Driver that Microsoft installs to prevent changing default browser/app associations via registry.",
            Tags = ["compatibility", "defaults", "ucpd", "browser"],
            SideEffects = "Microsoft may re-enable this periodically via Windows Update.",
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\UCPD"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\UCPD", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\UCPD", "Start", 2)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\UCPD", "Start", 4)],
        },
        new TweakDef
        {
            Id = "compat-disable-vdm-allowed",
            Label = "Disable 16-bit DOS Application Support (VDM)",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Virtual DOS Machine (NTVDM/VDM) that allows 16-bit legacy applications to run on 64-bit Windows.",
            Tags = ["compatibility", "legacy", "vdm", "dos", "security"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "VDMDisallowed", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "VDMDisallowed")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "VDMDisallowed", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-app-repkg-service",
            Label = "Disable App Repackaging Service",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Application Repackage Service used for automatic compatibility assessment, reducing background CPU usage.",
            Tags = ["compatibility", "performance", "background", "service"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\AppReadiness"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\AppReadiness", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\AppReadiness", "Start", 3)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\AppReadiness", "Start", 4)],
        },
        new TweakDef
        {
            Id = "compat-disable-install-service",
            Label = "Disable Application Identity Service",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Application Identity service (AppIDSvc) used by AppLocker for code integrity checks.",
            Tags = ["compatibility", "applocker", "service", "appid"],
            SideEffects = "AppLocker policies will not function if this service is disabled.",
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\AppIDSvc"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\AppIDSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\AppIDSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\AppIDSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "compat-disable-just-in-time-debugging",
            Label = "Disable Just-In-Time (JIT) Debugger",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the JIT debugger entry so that application crashes don't prompt 'Would you like to debug?' dialog boxes.",
            Tags = ["compatibility", "debugging", "crash", "ux"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug"],
            ApplyOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug", "Debugger")],
            RemoveOps =
            [
                RegOp.SetString($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug", "Debugger", @"vsjitdebugger.exe -p %ld -e %ld"),
            ],
            DetectOps = [RegOp.CheckMissing($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug", "Debugger")],
        },
        new TweakDef
        {
            Id = "compat-enable-dep-always-on",
            Label = "Enable DEP Always-On (All Programs)",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces Data Execution Prevention (DEP/NX) for all processes, not just Windows system components, increasing exploit mitigation.",
            Tags = ["compatibility", "dep", "security", "exploit", "hardening"],
            SideEffects = "Some very old or poorly-written applications may crash with DEP enabled.",
            ApplyAction = dryRun =>
            {
                if (!dryRun)
                    ShellRunner.Run("bcdedit", ["/set", "nx", "AlwaysOn"]);
            },
            RemoveAction = dryRun =>
            {
                if (!dryRun)
                    ShellRunner.Run("bcdedit", ["/set", "nx", "OptIn"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("nx                      AlwaysOn", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "compat-disable-error-reporting-ui",
            Label = "Disable Error Reporting UI Dialog",
            Category = "App Compatibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Suppresses the 'Report problem to Microsoft?' dialog box shown after application crashes.",
            Tags = ["compatibility", "wer", "crash", "ux", "privacy"],
            RegistryKeys = [$@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontShowUI", 1)],
            RemoveOps = [RegOp.DeleteValue($@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontShowUI")],
            DetectOps = [RegOp.CheckDword($@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontShowUI", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-ie-compat-view",
            Label = "Disable IE Compatibility View List Updates",
            Category = "App Compatibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic updates to the IE Compatibility View List from Microsoft, preventing background internet checks.",
            Tags = ["compatibility", "ie", "internet-explorer", "privacy", "network"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Internet Explorer\BrowserEmulation"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Internet Explorer\BrowserEmulation", "DisableIECompatView", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Internet Explorer\BrowserEmulation", "DisableIECompatView")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Internet Explorer\BrowserEmulation", "DisableIECompatView", 1)],
        },
    ];
}
