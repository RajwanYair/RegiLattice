#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

internal static class ChargingOptimization
{
    private const string UsbAttrKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\2a737441-1930-4402-8d77-b2bebba308a3\48e6b7a6-50f5-4782-a5d4-53bb8f07e226";
    private const string BattSubgroupKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\e73a048d-bf27-4f12-9731-8b2076e8891f";
    private const string LowBattActKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\e73a048d-bf27-4f12-9731-8b2076e8891f\d8742dcb-3e6a-4b3c-b3fe-374623cdcf06";
    private const string LowBattLevelKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\e73a048d-bf27-4f12-9731-8b2076e8891f\8183ba9a-e910-48da-8769-14ae6dc1170a";
    private const string CritBattActKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\e73a048d-bf27-4f12-9731-8b2076e8891f\637ea02f-bbcb-4015-8e2c-a1c7b9c0b546";
    private const string CritBattLevelKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\e73a048d-bf27-4f12-9731-8b2076e8891f\9a66d8d7-4ff7-4ef9-b5a2-5a326ca2a469";
    private const string ReserveBattLevelKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\e73a048d-bf27-4f12-9731-8b2076e8891f\7648efa3-dd9c-4e3e-b566-50f929386280";
    private const string EnergySaverThreshKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\e73a048d-bf27-4f12-9731-8b2076e8891f\e69653ca-cf7f-4f05-aa73-cb833fa90ad4";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "chrg-show-low-battery-level",
            Label = "Expose Low Battery Level Setting in Power Options UI",
            Category = "Charging & Battery",
            Description =
                "Sets the Attributes value for the Low Battery Level power setting to unhide it in the Power Options advanced settings dialog, allowing users to configure the low battery percentage threshold directly from the UI.",
            Tags = ["battery", "power", "charging", "notifications"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Makes hidden power plan settings visible in the Power Options UI for easier configuration.",
            ApplyOps = [RegOp.SetDword(LowBattLevelKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(LowBattLevelKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(LowBattLevelKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "chrg-show-critical-battery-level",
            Label = "Expose Critical Battery Level Setting in Power Options UI",
            Category = "Charging & Battery",
            Description =
                "Unhides the Critical Battery Level setting (default hidden at Attributes=18) from the Power Options advanced settings dialog so users can configure it to a custom percentage.",
            Tags = ["battery", "power", "charging", "critical"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Users can adjust critical battery level from UI after applying.",
            ApplyOps = [RegOp.SetDword(CritBattLevelKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(CritBattLevelKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(CritBattLevelKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "chrg-show-reserve-battery-level",
            Label = "Expose Reserve Battery Level Setting in Power Options UI",
            Category = "Charging & Battery",
            Description =
                "Unhides the Reserve Battery Level power setting from the Power Options UI so users can configure the reserve battery percentage that triggers the low battery warning notification.",
            Tags = ["battery", "power", "charging", "reserve"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Allows users to adjust reserve battery threshold; threshold defaults range from 7-10%.",
            ApplyOps = [RegOp.SetDword(ReserveBattLevelKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(ReserveBattLevelKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(ReserveBattLevelKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "chrg-show-low-battery-action",
            Label = "Expose Low Battery Action Setting in Power Options UI",
            Category = "Charging & Battery",
            Description =
                "Unhides the Low Battery Action setting in Power Options advanced view, allowing users to configure what happens when the battery drops to the low level (nothing, sleep, hibernate, shutdown).",
            Tags = ["battery", "power", "charging", "action"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Exposes action selector for low battery in Power Options UI.",
            ApplyOps = [RegOp.SetDword(LowBattActKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(LowBattActKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(LowBattActKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "chrg-show-critical-battery-action",
            Label = "Expose Critical Battery Action Setting in Power Options UI",
            Category = "Charging & Battery",
            Description =
                "Unhides the Critical Battery Action power setting so users can configure behaviour at critical battery level (default is hibernate). Useful for systems where the default isn't appropriate.",
            Tags = ["battery", "power", "charging", "critical", "action"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Exposes critical battery action selector in Power Options UI.",
            ApplyOps = [RegOp.SetDword(CritBattActKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(CritBattActKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(CritBattActKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "chrg-show-energy-saver-threshold",
            Label = "Expose Energy Saver Battery Threshold in Power Options UI",
            Category = "Charging & Battery",
            Description =
                "Unhides the Energy Saver Battery Threshold power setting from the Power Options advanced settings, enabling per-plan configuration of when Battery Saver activates.",
            Tags = ["battery", "power", "battery-saver", "charging"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Exposes battery saver threshold setting per-power-plan in the advanced Power Options UI.",
            ApplyOps = [RegOp.SetDword(EnergySaverThreshKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(EnergySaverThreshKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(EnergySaverThreshKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "chrg-show-usb-selective-suspend-attr",
            Label = "Expose USB Selective Suspend Setting in Power Options UI",
            Category = "Charging & Battery",
            Description =
                "Unhides the USB Selective Suspend setting in the Power Options advanced settings dialog. When visible, users can disable USB selective suspend per-power-plan to prevent USB devices from losing power during charging.",
            Tags = ["battery", "power", "charging", "usb"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Exposes USB selective suspend toggle in per-plan Power Options; use to prevent USB charging interruptions.",
            ApplyOps = [RegOp.SetDword(UsbAttrKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(UsbAttrKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(UsbAttrKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "chrg-battery-subgroup-visible",
            Label = "Make Battery Subgroup Visible in Power Options",
            Category = "Charging & Battery",
            Description =
                "Sets Attributes=2 on the Battery subgroup node in the Power Settings registry, ensuring the Battery category is visible in the Power Options advanced settings dialog. Some power plan templates hide this group.",
            Tags = ["battery", "power", "charging", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Restores Battery group visibility in the Advanced Power Options dialog.",
            ApplyOps = [RegOp.SetDword(BattSubgroupKey, "Attributes", 0)],
            RemoveOps = [RegOp.SetDword(BattSubgroupKey, "Attributes", 0)],
            DetectOps = [RegOp.CheckDword(BattSubgroupKey, "Attributes", 0)],
        },
        new TweakDef
        {
            Id = "chrg-low-batt-dc-hibernate",
            Label = "Set Low Battery DC Action to Hibernate",
            Category = "Charging & Battery",
            Description =
                "Configures the Low Battery Action policy to hibernate (action index 3) when on battery (DC) power. Prevents data loss by saving system state to disk when the battery reaches the low level.",
            Tags = ["battery", "power", "charging", "hibernate", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "System hibernates at low battery threshold; prevents data loss from sudden shutdown.",
            ApplyOps = [RegOp.SetDword(LowBattActKey, "DCSettingIndex", 3), RegOp.SetDword(LowBattActKey, "ACSettingIndex", 0)],
            RemoveOps = [RegOp.DeleteValue(LowBattActKey, "DCSettingIndex"), RegOp.DeleteValue(LowBattActKey, "ACSettingIndex")],
            DetectOps = [RegOp.CheckDword(LowBattActKey, "DCSettingIndex", 3)],
        },
        new TweakDef
        {
            Id = "chrg-crit-batt-dc-hibernate",
            Label = "Set Critical Battery DC Action to Hibernate",
            Category = "Charging & Battery",
            Description =
                "Sets the Critical Battery Action to hibernate (index=3) when on battery (DC), ensuring the system saves state to disk before shutting down from critically low battery rather than a hard power-off.",
            Tags = ["battery", "power", "charging", "hibernate", "critical"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Prevents data loss from power-off at critical battery; full system state preserved to hibernation file.",
            ApplyOps = [RegOp.SetDword(CritBattActKey, "DCSettingIndex", 3), RegOp.SetDword(CritBattActKey, "ACSettingIndex", 0)],
            RemoveOps = [RegOp.DeleteValue(CritBattActKey, "DCSettingIndex"), RegOp.DeleteValue(CritBattActKey, "ACSettingIndex")],
            DetectOps = [RegOp.CheckDword(CritBattActKey, "DCSettingIndex", 3)],
        },
    ];
}
