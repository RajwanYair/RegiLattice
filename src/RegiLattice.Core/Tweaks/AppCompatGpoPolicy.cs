#nullable enable

using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class AppCompatGpoPolicy
{
    private const string AppComp = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "accompat-disable-inventory-collector",
            Label = "Disable Application Compatibility Inventory Collector",
            Category = "App Compatibility Policy",
            Description = "Disables the Application Compatibility Inventory Collector, which collects program data for app compatibility telemetry.",
            Tags = ["appcompat", "privacy", "telemetry", "group-policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(AppComp, "DisableInventory", 1)],
            RemoveOps = [RegOp.DeleteValue(AppComp, "DisableInventory")],
            DetectOps = [RegOp.CheckDword(AppComp, "DisableInventory", 1)],
        },
        new TweakDef
        {
            Id = "accompat-disable-program-compatibility-assistant",
            Label = "Disable Program Compatibility Assistant",
            Category = "App Compatibility Policy",
            Description =
                "Turns off the Program Compatibility Assistant (PCA), which monitors programs for known compatibility issues and prompts users.",
            Tags = ["appcompat", "telemetry", "group-policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(AppComp, "DisablePCA", 1)],
            RemoveOps = [RegOp.DeleteValue(AppComp, "DisablePCA")],
            DetectOps = [RegOp.CheckDword(AppComp, "DisablePCA", 1)],
        },
        new TweakDef
        {
            Id = "accompat-disable-application-impact-telemetry",
            Label = "Turn Off Application Impact Telemetry",
            Category = "App Compatibility Policy",
            Description =
                "Stops the Application Impact Telemetry (AIT) agent from collecting and uploading application usage statistics to Microsoft.",
            Tags = ["appcompat", "privacy", "telemetry", "group-policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(AppComp, "AITEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(AppComp, "AITEnable")],
            DetectOps = [RegOp.CheckDword(AppComp, "AITEnable", 0)],
        },
        new TweakDef
        {
            Id = "accompat-disable-user-assistance-telemetry",
            Label = "Disable User Assistance Telemetry",
            Category = "App Compatibility Policy",
            Description = "Disables the User Assistance Telemetry component from sending application crash and help request data to Microsoft.",
            Tags = ["appcompat", "privacy", "telemetry", "group-policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(AppComp, "DisableUAT", 1)],
            RemoveOps = [RegOp.DeleteValue(AppComp, "DisableUAT")],
            DetectOps = [RegOp.CheckDword(AppComp, "DisableUAT", 1)],
        },
        new TweakDef
        {
            Id = "accompat-disable-wizard",
            Label = "Disable Program Compatibility Wizard",
            Category = "App Compatibility Policy",
            Description =
                "Removes the Program Compatibility Wizard from the context menu and prevents users from running it to set compatibility modes.",
            Tags = ["appcompat", "group-policy", "debloat"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(AppComp, "DisableWizard", 1)],
            RemoveOps = [RegOp.DeleteValue(AppComp, "DisableWizard")],
            DetectOps = [RegOp.CheckDword(AppComp, "DisableWizard", 1)],
        },
        new TweakDef
        {
            Id = "accompat-disable-engine",
            Label = "Disable Application Compatibility Engine",
            Category = "App Compatibility Policy",
            Description =
                "Turns off the Application Compatibility Engine, which performs shim lookups on every program launch. May cause some legacy applications to fail.",
            Tags = ["appcompat", "performance", "group-policy"],
            NeedsAdmin = true,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(AppComp, "DisableEngine", 1)],
            RemoveOps = [RegOp.DeleteValue(AppComp, "DisableEngine")],
            DetectOps = [RegOp.CheckDword(AppComp, "DisableEngine", 1)],
        },
        new TweakDef
        {
            Id = "accompat-disable-switchback-compatibility",
            Label = "Disable SwitchBack Compatibility Engine",
            Category = "App Compatibility Policy",
            Description = "Disables the SwitchBack compatibility engine which detects applications incompatible with Windows version upgrades.",
            Tags = ["appcompat", "performance", "group-policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(AppComp, "SbEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(AppComp, "SbEnable")],
            DetectOps = [RegOp.CheckDword(AppComp, "SbEnable", 0)],
        },
        new TweakDef
        {
            Id = "accompat-disable-steps-recorder",
            Label = "Disable Steps Recorder",
            Category = "App Compatibility Policy",
            Description =
                "Disables the Steps Recorder (Problem Steps Recorder) tool, which can capture screenshots and user actions for troubleshooting.",
            Tags = ["appcompat", "privacy", "group-policy", "debloat"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(AppComp, "DisableUAR", 1)],
            RemoveOps = [RegOp.DeleteValue(AppComp, "DisableUAR")],
            DetectOps = [RegOp.CheckDword(AppComp, "DisableUAR", 1)],
        },
        new TweakDef
        {
            Id = "accompat-prevent-access-16bit",
            Label = "Prevent Access to 16-bit Applications",
            Category = "App Compatibility Policy",
            Description = "Blocks execution of 16-bit applications by disabling the Windows on Windows (WOW) subsystem via Group Policy.",
            Tags = ["appcompat", "security", "group-policy", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(AppComp, "Prevent16BitMSDos", 1)],
            RemoveOps = [RegOp.DeleteValue(AppComp, "Prevent16BitMSDos")],
            DetectOps = [RegOp.CheckDword(AppComp, "Prevent16BitMSDos", 1)],
        },
        new TweakDef
        {
            Id = "accompat-turn-off-windows-error-reporting",
            Label = "Turn Off App Compatibility Windows Error Reporting",
            Category = "App Compatibility Policy",
            Description = "Suppresses Windows Error Reporting prompts generated by the Application Compatibility framework.",
            Tags = ["appcompat", "privacy", "telemetry", "group-policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(AppComp, "DisableWER", 1)],
            RemoveOps = [RegOp.DeleteValue(AppComp, "DisableWER")],
            DetectOps = [RegOp.CheckDword(AppComp, "DisableWER", 1)],
        },
    ];
}
