#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 248 — Windows Mobility Center Policy (10 tweaks)
// Keys: HKLM\SOFTWARE\Policies\Microsoft\Windows\MobilityCenter
//       HKCU\Software\Policies\Microsoft\Windows\MobilityCenter
//       HKLM\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Policies\Explorer (mobility items)
internal static class MobilityCenterPolicy
{
    private const string MobLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\MobilityCenter";
    private const string MobCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\MobilityCenter";
    private const string ExplLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Policies\Explorer";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "mob-disable-mobility-center",
            Label = "Disable Windows Mobility Center (Machine)",
            Category = "Mobility Center Policy",
            Description =
                "Sets NoMobilityCenter=1 in the machine-side Mobility Center policy key. "
                + "Disables Windows Mobility Center (quicklaunch panel for brightness, volume, battery, "
                + "display, sync, presentation mode) for all users on the machine. "
                + "Default: absent (Mobility Center enabled on laptops). Recommended: 1 on desktops or kiosks.",
            Tags = ["mobility-center", "laptop", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Mobility Center disabled; individual settings still accessible via Settings app.",
            ApplyOps = [RegOp.SetDword(MobLm, "NoMobilityCenter", 1)],
            RemoveOps = [RegOp.DeleteValue(MobLm, "NoMobilityCenter")],
            DetectOps = [RegOp.CheckDword(MobLm, "NoMobilityCenter", 1)],
        },
        new TweakDef
        {
            Id = "mob-disable-mobility-center-user",
            Label = "Disable Windows Mobility Center (Current User)",
            Category = "Mobility Center Policy",
            Description =
                "Sets NoMobilityCenter=1 in the per-user Mobility Center policy key. "
                + "Removes the Mobility Center shortcut and prevents the panel from being opened "
                + "for the current user via Win+X or the system tray. "
                + "Default: absent. Recommended: 1 for user profiles on non-mobile workstations.",
            Tags = ["mobility-center", "user", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Mobility Center disabled for this user only; other accounts on the machine are unaffected.",
            ApplyOps = [RegOp.SetDword(MobCu, "NoMobilityCenter", 1)],
            RemoveOps = [RegOp.DeleteValue(MobCu, "NoMobilityCenter")],
            DetectOps = [RegOp.CheckDword(MobCu, "NoMobilityCenter", 1)],
        },
        new TweakDef
        {
            Id = "mob-disable-presentation-settings",
            Label = "Disable Presentation Settings Tile in Mobility Center",
            Category = "Mobility Center Policy",
            Description =
                "Sets NoPresentationSettings=1 in the machine Mobility Center policy key. "
                + "Removes the Presentation Settings tile from Mobility Center, preventing users from "
                + "adjusting display/screensaver settings for presentations. "
                + "Default: absent (tile visible). Recommended: 1 on managed desktops.",
            Tags = ["mobility-center", "presentation", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Removes the Presentation Settings tile from Mobility Center UI only.",
            ApplyOps = [RegOp.SetDword(MobLm, "NoPresentationSettings", 1)],
            RemoveOps = [RegOp.DeleteValue(MobLm, "NoPresentationSettings")],
            DetectOps = [RegOp.CheckDword(MobLm, "NoPresentationSettings", 1)],
        },
        new TweakDef
        {
            Id = "mob-disable-battery-tile",
            Label = "Disable Battery Status Tile in Mobility Center",
            Category = "Mobility Center Policy",
            Description =
                "Sets NoBatteryTile=1 in the machine Mobility Center policy key. "
                + "Hides the battery status tile in Windows Mobility Center. "
                + "Useful on desktop machines or when battery management is handled by a third-party tool. "
                + "Default: absent. Recommended: 1 on AC-only desktops.",
            Tags = ["mobility-center", "battery", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Battery tile hidden in Mobility Center; system tray battery icon is unaffected.",
            ApplyOps = [RegOp.SetDword(MobLm, "NoBatteryTile", 1)],
            RemoveOps = [RegOp.DeleteValue(MobLm, "NoBatteryTile")],
            DetectOps = [RegOp.CheckDword(MobLm, "NoBatteryTile", 1)],
        },
        new TweakDef
        {
            Id = "mob-disable-sync-center-tile",
            Label = "Disable Sync Center Tile in Mobility Center",
            Category = "Mobility Center Policy",
            Description =
                "Sets NoSyncCenterTile=1 in the machine Mobility Center policy key. "
                + "Removes the Sync Center tile from Windows Mobility Center. "
                + "Appropriate when Work Folders or Sync Center is disabled by other policies. "
                + "Default: absent. Recommended: 1 when Sync Center is not used.",
            Tags = ["mobility-center", "sync", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Sync Center tile removed from Mobility Center; the Sync Center control panel is still accessible.",
            ApplyOps = [RegOp.SetDword(MobLm, "NoSyncCenterTile", 1)],
            RemoveOps = [RegOp.DeleteValue(MobLm, "NoSyncCenterTile")],
            DetectOps = [RegOp.CheckDword(MobLm, "NoSyncCenterTile", 1)],
        },
        new TweakDef
        {
            Id = "mob-disable-display-tile",
            Label = "Disable External Display Tile in Mobility Center",
            Category = "Mobility Center Policy",
            Description =
                "Sets NoExternalDisplayTile=1 in the machine Mobility Center policy key. "
                + "Hides the external display (projector/monitor) connection tile from Mobility Center. "
                + "Recommended on desktop machines with no docking or projector scenario.",
            Tags = ["mobility-center", "display", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "External display conn tile removed from Mobility Center; Win+P display mode still accessible.",
            ApplyOps = [RegOp.SetDword(MobLm, "NoExternalDisplayTile", 1)],
            RemoveOps = [RegOp.DeleteValue(MobLm, "NoExternalDisplayTile")],
            DetectOps = [RegOp.CheckDword(MobLm, "NoExternalDisplayTile", 1)],
        },
        new TweakDef
        {
            Id = "mob-disable-screen-rotation-tile",
            Label = "Disable Screen Rotation Tile in Mobility Center",
            Category = "Mobility Center Policy",
            Description =
                "Sets NoScreenRotationTile=1 in the machine Mobility Center policy key. "
                + "Removes the screen rotation tile from Mobility Center. "
                + "Appropriate on non-tablet devices or systems where screen rotation is not relevant. "
                + "Default: absent. Recommended: 1 on non-touchscreen desktops and laptops.",
            Tags = ["mobility-center", "rotation", "tablet", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Screen rotation tile removed from Mobility Center; OS rotation settings still accessible.",
            ApplyOps = [RegOp.SetDword(MobLm, "NoScreenRotationTile", 1)],
            RemoveOps = [RegOp.DeleteValue(MobLm, "NoScreenRotationTile")],
            DetectOps = [RegOp.CheckDword(MobLm, "NoScreenRotationTile", 1)],
        },
        new TweakDef
        {
            Id = "mob-disable-wireless-tile",
            Label = "Disable Wireless Network Tile in Mobility Center",
            Category = "Mobility Center Policy",
            Description =
                "Sets NoWirelessNetworkTile=1 in the machine Mobility Center policy key. "
                + "Hides the Wi-Fi / wireless network tile from Mobility Center. "
                + "Useful on wired-only desktops or when wireless access is managed via other policies. "
                + "Default: absent. Recommended: 1 on non-wireless desktops.",
            Tags = ["mobility-center", "wireless", "wifi", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Wireless tile hidden in Mobility Center; network settings accessible via taskbar tray.",
            ApplyOps = [RegOp.SetDword(MobLm, "NoWirelessNetworkTile", 1)],
            RemoveOps = [RegOp.DeleteValue(MobLm, "NoWirelessNetworkTile")],
            DetectOps = [RegOp.CheckDword(MobLm, "NoWirelessNetworkTile", 1)],
        },
        new TweakDef
        {
            Id = "mob-disable-volume-tile",
            Label = "Disable Volume Tile in Mobility Center",
            Category = "Mobility Center Policy",
            Description =
                "Sets NoVolumeTile=1 in the machine Mobility Center policy key. "
                + "Removes the speaker volume slider tile from Mobility Center. "
                + "Appropriate when volume management is handled via hardware keys or another interface. "
                + "Default: absent. Recommended: 1 on devices with dedicated audio hardware controls.",
            Tags = ["mobility-center", "volume", "audio", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Volume tile removed from Mobility Center; system tray volume control is unaffected.",
            ApplyOps = [RegOp.SetDword(MobLm, "NoVolumeTile", 1)],
            RemoveOps = [RegOp.DeleteValue(MobLm, "NoVolumeTile")],
            DetectOps = [RegOp.CheckDword(MobLm, "NoVolumeTile", 1)],
        },
        new TweakDef
        {
            Id = "mob-remove-mobility-center-from-context",
            Label = "Remove Windows Mobility Center from Context Menu",
            Category = "Mobility Center Policy",
            Description =
                "Sets NoMobilityCenterContextMenu=1 in the machine Explorer policy key. "
                + "Removes the Windows Mobility Center entry from the desktop right-click context menu. "
                + "Complements the NoMobilityCenter policy by also hiding the context menu launch path. "
                + "Default: absent. Recommended: 1 when Mobility Center is disabled.",
            Tags = ["mobility-center", "context-menu", "explorer", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Mobility Center removed from right-click desktop context menu; Win+X shortcut also disabled.",
            ApplyOps = [RegOp.SetDword(ExplLm, "NoMobilityCenterContextMenu", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplLm, "NoMobilityCenterContextMenu")],
            DetectOps = [RegOp.CheckDword(ExplLm, "NoMobilityCenterContextMenu", 1)],
        },
    ];
}
