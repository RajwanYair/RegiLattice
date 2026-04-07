#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

internal static class DisplayPower
{
    // Display subgroup GUID: 7516b95f-f776-4464-8c53-06167f40cc99
    private const string DispSubgroupKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99";
    private const string DispTimeoutKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\3c0bc021-c8a8-4e07-a973-6b14cbcb2b7e";
    private const string AdaptiveDimKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\aded5e82-b909-4619-9949-f5d71dac0bcb";
    private const string DimmingTimeoutKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\17aaa29b-8b43-4b94-aafe-35f64daaf1ee";
    private const string AllowDisplayRequiredKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\a9ceb8da-cd46-44fb-a98b-02af69de4623";
    private const string BrightAutoKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\FBD9AA66-9101-4a11-AB08-05F5DBB4C7A3";
    private const string AmbientLightKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\A330F0B2-BE8C-4D7A-9E8F-8A3DA956E560";
    private const string DisplayScalingKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\a1662ab2-9d34-4e53-ba8b-2639b9e20857";
    private const string ContentAdaptiveBrightKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\a9ceb8da-cd46-44fb-a98b-02af69de4623";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "dispwr-expose-display-timeout",
            Label = "Expose Display Timeout Setting in Power Options UI",
            Category = "Display Power",
            Description =
                "Unhides the Display Timeout (Turn off display after) setting in the Power Options advanced settings dialog via Attributes=2. Allows per-plan control of how long the display stays on before turning off due to inactivity.",
            Tags = ["display", "power", "timeout", "screen", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Exposes display timeout control per-plan in Power Options UI.",
            ApplyOps = [RegOp.SetDword(DispTimeoutKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(DispTimeoutKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(DispTimeoutKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "dispwr-expose-adaptive-dimming",
            Label = "Expose Adaptive Display Dimming Setting in Power Options UI",
            Category = "Display Power",
            Description =
                "Unhides the Adaptive Display Dimming (dim display before turning off) setting in Power Options. When exposed, users can disable the pre-dim notice that reduces brightness before the display timeout fires.",
            Tags = ["display", "power", "dimming", "adaptive", "brightness", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Exposes adaptive dimming control for display-off transitions.",
            ApplyOps = [RegOp.SetDword(AdaptiveDimKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(AdaptiveDimKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(AdaptiveDimKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "dispwr-disable-adaptive-dimming-dc",
            Label = "Disable Adaptive Display Dimming on Battery",
            Category = "Display Power",
            Description =
                "Disables the adaptive display dimming feature on DC (battery) power by setting DCSettingIndex=0. Prevents Windows from dimming the screen before switching it off, which can be jarring during active use on laptops.",
            Tags = ["display", "power", "dimming", "battery"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Display will not pre-dim before shutting off on battery; cleaner cutoff at timeout.",
            ApplyOps = [RegOp.SetDword(AdaptiveDimKey, "DCSettingIndex", 0)],
            RemoveOps = [RegOp.DeleteValue(AdaptiveDimKey, "DCSettingIndex")],
            DetectOps = [RegOp.CheckDword(AdaptiveDimKey, "DCSettingIndex", 0)],
        },
        new TweakDef
        {
            Id = "dispwr-expose-display-required-allow",
            Label = "Expose Allow Display Required Policy in Power Options UI",
            Category = "Display Power",
            Description =
                "Unhides the 'Allow Display Required Policy' setting in Power Options. When enabled, this allows applications to call SetThreadExecutionState to prevent the display from turning off; exposing it lets power users override this per-plan.",
            Tags = ["display", "power", "policy", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Exposes app display-on override control in Power Options.",
            ApplyOps = [RegOp.SetDword(AllowDisplayRequiredKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(AllowDisplayRequiredKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(AllowDisplayRequiredKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "dispwr-expose-auto-brightness",
            Label = "Expose Automatic Brightness Adjustment Setting in Power Options",
            Category = "Display Power",
            Description =
                "Unhides the Automatic Brightness (ALS-based adaptive brightness) setting in the Power Options advanced dialog. Allows per-plan control over whether ambient light sensing is used to adjust display brightness automatically.",
            Tags = ["display", "power", "brightness", "ambient-light", "als", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Exposes ALS auto-brightness per-plan control in Power Options.",
            ApplyOps = [RegOp.SetDword(BrightAutoKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(BrightAutoKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(BrightAutoKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "dispwr-disable-adaptive-brightness-dc",
            Label = "Disable Adaptive Brightness on Battery",
            Category = "Display Power",
            Description =
                "Disables ALS-based automatic brightness adjustment on DC (battery) power. Prevents the display brightness from flickering or adjusting unexpectedly when ambient light conditions change, which can be distracting during mobile use.",
            Tags = ["display", "power", "brightness", "ambient-light", "battery"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Display brightness remains stable and user-set on battery; reduces distraction from automatic adjustments.",
            ApplyOps = [RegOp.SetDword(BrightAutoKey, "DCSettingIndex", 0)],
            RemoveOps = [RegOp.DeleteValue(BrightAutoKey, "DCSettingIndex")],
            DetectOps = [RegOp.CheckDword(BrightAutoKey, "DCSettingIndex", 0)],
        },
        new TweakDef
        {
            Id = "dispwr-disp-subgroup-visible",
            Label = "Ensure Display Subgroup is Visible in Power Options",
            Category = "Display Power",
            Description =
                "Sets Attributes=0 on the Display power settings subgroup, ensuring the Display section is always visible in the Power Options advanced settings dialog. Some OEM power plan templates hide this group.",
            Tags = ["display", "power", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Restores Display group visibility in advanced Power Options.",
            ApplyOps = [RegOp.SetDword(DispSubgroupKey, "Attributes", 0)],
            RemoveOps = [RegOp.SetDword(DispSubgroupKey, "Attributes", 0)],
            DetectOps = [RegOp.CheckDword(DispSubgroupKey, "Attributes", 0)],
        },
        new TweakDef
        {
            Id = "dispwr-expose-dimming-timeout",
            Label = "Expose Display Dimming Timeout in Power Options UI",
            Category = "Display Power",
            Description =
                "Unhides the Display Dimming Timeout setting in Power Options, which controls how long after no user input the display dims before going fully off. Setting this shorter than the display-off timeout creates a two-stage power-down behaviour.",
            Tags = ["display", "power", "dimming", "timeout", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Exposes two-stage display dim → off timeout control in Power Options UI.",
            ApplyOps = [RegOp.SetDword(DimmingTimeoutKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(DimmingTimeoutKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(DimmingTimeoutKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "dispwr-no-dim-ac",
            Label = "Disable Display Dimming on AC Power",
            Category = "Display Power",
            Description =
                "Disables the pre-display-off dimming step on AC (mains) power by setting ACSettingIndex=0. When plugged in, the display will transition directly from full brightness to off at the timeout instead of dimming first.",
            Tags = ["display", "power", "dimming", "ac"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "No pre-dim animation before display off on AC; power state visible immediately.",
            ApplyOps = [RegOp.SetDword(AdaptiveDimKey, "ACSettingIndex", 0)],
            RemoveOps = [RegOp.DeleteValue(AdaptiveDimKey, "ACSettingIndex")],
            DetectOps = [RegOp.CheckDword(AdaptiveDimKey, "ACSettingIndex", 0)],
        },
        new TweakDef
        {
            Id = "dispwr-expose-display-scaling",
            Label = "Expose Display Color Calibration on Battery Setting in Power Options",
            Category = "Display Power",
            Description =
                "Unhides the Display Color Calibration on Battery setting, which controls whether ICC color profiles are applied when running on battery power. Exposing this allows disabling the profile to save power on color-managed workflows on laptops.",
            Tags = ["display", "power", "color", "calibration", "battery", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Exposes battery ICC color calibration toggle in Power Options; disabling saves GPU compositing overhead.",
            ApplyOps = [RegOp.SetDword(DisplayScalingKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(DisplayScalingKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(DisplayScalingKey, "Attributes", 2)],
        },
    ];
}
