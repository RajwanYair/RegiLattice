// RegiLattice.Core — Tweaks/RetailDemoPolicy.cs
// Sprint 267: Retail Demo Mode Group Policy (10 tweaks)
// Category: "Retail Demo Policy" | Slug: rdemo
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RetailDemo

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class RetailDemoPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RetailDemo";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "rdemo-disable-retail-demo",
            Label = "Disable Retail Demo Mode",
            Category = "Retail Demo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets NoRetailDemo=1 in the RetailDemo policy key. Prevents Windows from "
                + "entering retail demo mode, which runs a continuous promotional demonstration "
                + "experience on display units in retail stores. Demo mode overrides normal user "
                + "experience settings and launches curated content. Default: 0 (demo mode "
                + "allowed by OEM configuration). Recommended: 1 on corporate and personal "
                + "devices to block any inadvertent demo-mode activation.",
            Tags = ["retail-demo", "kiosk", "policy", "display"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoRetailDemo", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoRetailDemo")],
            DetectOps = [RegOp.CheckDword(Key, "NoRetailDemo", 1)],
        },
        new TweakDef
        {
            Id = "rdemo-disable-attract-loop",
            Label = "Disable Retail Demo Attract Loop",
            Category = "Retail Demo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets NoAttractLoop=1 in the RetailDemo policy key. Stops the idle attract-loop "
                + "video that plays on unattended retail display machines to showcase Windows "
                + "features and invite customer interaction. On non-retail devices this loop "
                + "would trigger after inactivity timeout and play promotional video full-screen. "
                + "Default: 0. Recommended: 1 on any device that is not a retail display unit.",
            Tags = ["retail-demo", "attract", "video", "idle", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoAttractLoop", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoAttractLoop")],
            DetectOps = [RegOp.CheckDword(Key, "NoAttractLoop", 1)],
        },
        new TweakDef
        {
            Id = "rdemo-disable-auto-signin",
            Label = "Disable Retail Demo Auto Sign-In",
            Category = "Retail Demo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets NoAutoSignIn=1 in the RetailDemo policy key. Prevents a demo account from "
                + "automatically signing in on boot, which is a behaviour specific to retail "
                + "store display units running in unattended self-service mode. Automatic demo "
                + "sign-in bypasses normal login prompts and loads a preconfigured experience "
                + "account. Default: 0. Recommended: 1 on devices requiring secure authentication.",
            Tags = ["retail-demo", "sign-in", "auto-login", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoAutoSignIn", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoAutoSignIn")],
            DetectOps = [RegOp.CheckDword(Key, "NoAutoSignIn", 1)],
        },
        new TweakDef
        {
            Id = "rdemo-disable-demo-apps",
            Label = "Disable Retail Demo App Provisioning",
            Category = "Retail Demo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets NoRetailDemoApps=1 in the RetailDemo policy key. Prevents provisioning of "
                + "Microsoft-curated demo applications installed on retail display machines to "
                + "showcase Windows and Microsoft 365 features. These apps are silently installed "
                + "from the Store without user consent on retail-configured devices. "
                + "Default: 0. Recommended: 1 on enterprise and personal devices.",
            Tags = ["retail-demo", "apps", "store", "provisioning", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoRetailDemoApps", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoRetailDemoApps")],
            DetectOps = [RegOp.CheckDword(Key, "NoRetailDemoApps", 1)],
        },
        new TweakDef
        {
            Id = "rdemo-disable-demo-content",
            Label = "Disable Retail Demo Content Delivery",
            Category = "Retail Demo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets NoRetailDemoContent=1 in the RetailDemo policy key. Prevents Windows from "
                + "downloading and staging promotional content packages from Microsoft CDN for "
                + "use in retail demo scenarios. These background downloads consume network "
                + "bandwidth and disk space without user awareness on non-retail devices. "
                + "Default: 0. Recommended: 1 on metered or managed connections.",
            Tags = ["retail-demo", "content", "cdn", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoRetailDemoContent", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoRetailDemoContent")],
            DetectOps = [RegOp.CheckDword(Key, "NoRetailDemoContent", 1)],
        },
        new TweakDef
        {
            Id = "rdemo-disable-experience-provider",
            Label = "Disable Retail Demo Device Experience Provider",
            Category = "Retail Demo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets NoDeviceExperienceProvider=1 in the RetailDemo policy key. Blocks the "
                + "Device Experience Provider component used by OEM retail configurations to "
                + "display branded hardware demonstrations and guided tours during initial setup "
                + "in stores. This provider can launch demo walkthroughs without user action. "
                + "Default: 0. Recommended: 1 on post-purchase devices.",
            Tags = ["retail-demo", "experience", "oem", "provider", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoDeviceExperienceProvider", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoDeviceExperienceProvider")],
            DetectOps = [RegOp.CheckDword(Key, "NoDeviceExperienceProvider", 1)],
        },
        new TweakDef
        {
            Id = "rdemo-disable-demo-banner",
            Label = "Disable Retail Demo Info Banner",
            Category = "Retail Demo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Sets NoDemoBanner=1 in the RetailDemo policy key. Hides the informational "
                + "banner shown in retail demo mode that prompts customers to purchase the "
                + "device or explore Windows features being demonstrated on the floor model. "
                + "This UI element is irrelevant and distracting on owned devices. "
                + "Default: 0. Recommended: 1 on non-retail hardware.",
            Tags = ["retail-demo", "banner", "ui", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoDemoBanner", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoDemoBanner")],
            DetectOps = [RegOp.CheckDword(Key, "NoDemoBanner", 1)],
        },
        new TweakDef
        {
            Id = "rdemo-disable-oobe-demo",
            Label = "Disable Retail Demo OOBE Flow",
            Category = "Retail Demo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets NoRetailOOBE=1 in the RetailDemo policy key. Prevents the out-of-box "
                + "experience from branching into retail demo setup mode during initial device "
                + "configuration. The retail OOBE path creates a temporary guest demo account "
                + "and loads promotional assets instead of standard setup. "
                + "Default: 0 for OEM-configured retail images. Recommended: 1 everywhere else.",
            Tags = ["retail-demo", "oobe", "setup", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoRetailOOBE", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoRetailOOBE")],
            DetectOps = [RegOp.CheckDword(Key, "NoRetailOOBE", 1)],
        },
        new TweakDef
        {
            Id = "rdemo-disable-cleanup-revert",
            Label = "Disable Retail Demo Cleanup Revert Task",
            Category = "Retail Demo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets NoCleanupRevert=1 in the RetailDemo policy key. Blocks the scheduled "
                + "retail demo cleanup task that runs after business hours to wipe user "
                + "interactions and restore the machine to factory demo defaults. On non-retail "
                + "devices this task would destructively remove user customisations and data "
                + "overnight. Default: 0. Recommended: 1 on all personal and managed devices.",
            Tags = ["retail-demo", "cleanup", "scheduled-task", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoCleanupRevert", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoCleanupRevert")],
            DetectOps = [RegOp.CheckDword(Key, "NoCleanupRevert", 1)],
        },
        new TweakDef
        {
            Id = "rdemo-disable-demo-telemetry",
            Label = "Disable Retail Demo Interaction Telemetry",
            Category = "Retail Demo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets NoRetailTelemetry=1 in the RetailDemo policy key. Stops transmission of "
                + "retail demo interaction analytics (which buttons were pressed, time spent on "
                + "demo scenes, feature engagement) to Microsoft. This retail-specific telemetry "
                + "stream is separate from the standard Windows diagnostic data pipeline and "
                + "continues even when diagnostic level is set to Basic. "
                + "Default: 0. Recommended: 1 for privacy.",
            Tags = ["retail-demo", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoRetailTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoRetailTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "NoRetailTelemetry", 1)],
        },
    ];
}
