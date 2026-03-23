// Device Provisioning & Enrollment Policy — Sprint 144
// Slug "devprov" — controls OOBE setup experience, HomeGroup, Workplace Join, and
// initial device provisioning behavior via Group Policy registry settings.
//
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\OOBE
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HomeGroup
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent (distinct values)
#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class DeviceProvisioningPolicy
{
    private const string Oobe = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\OOBE";
    private const string HomeGrp = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HomeGroup";
    private const string WpjPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin";
    private const string CloudContent = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "devprov-disable-oobe-privacy",
            Label = "OOBE: Skip the privacy experience setup page",
            Category = "Device Provisioning Policy",
            Description =
                "Sets DisablePrivacyExperience=1 in the OOBE policy key. Suppresses the Windows "
                + "privacy settings page that appears during first sign-in after setup or a feature update.",
            Tags = ["oobe", "privacy", "setup", "policy", "provisioning"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Oobe, "DisablePrivacyExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(Oobe, "DisablePrivacyExperience")],
            DetectOps = [RegOp.CheckDword(Oobe, "DisablePrivacyExperience", 1)],
        },
        new TweakDef
        {
            Id = "devprov-skip-machine-oobe",
            Label = "OOBE: Skip the machine out-of-box experience setup",
            Category = "Device Provisioning Policy",
            Description =
                "Sets SkipMachineOOBE=1 in the OOBE policy key. Prevents the machine-level OOBE "
                + "wizard from running, useful for pre-provisioned enterprise devices.",
            Tags = ["oobe", "setup", "provisioning", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Oobe, "SkipMachineOOBE", 1)],
            RemoveOps = [RegOp.DeleteValue(Oobe, "SkipMachineOOBE")],
            DetectOps = [RegOp.CheckDword(Oobe, "SkipMachineOOBE", 1)],
        },
        new TweakDef
        {
            Id = "devprov-skip-user-oobe",
            Label = "OOBE: Skip the user out-of-box experience setup",
            Category = "Device Provisioning Policy",
            Description =
                "Sets SkipUserOOBE=1 in the OOBE policy key. Skips the per-user OOBE wizard that "
                + "prompts for Cortana, account sign-in, and other optional setup steps.",
            Tags = ["oobe", "setup", "provisioning", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Oobe, "SkipUserOOBE", 1)],
            RemoveOps = [RegOp.DeleteValue(Oobe, "SkipUserOOBE")],
            DetectOps = [RegOp.CheckDword(Oobe, "SkipUserOOBE", 1)],
        },
        new TweakDef
        {
            Id = "devprov-no-connected-oobe",
            Label = "OOBE: Disable cloud-connected experience during OOBE",
            Category = "Device Provisioning Policy",
            Description =
                "Sets DisableOOBEWithNetworkConnectivity=1 in the OOBE policy key. Prevents the "
                + "OOBE wizard from triggering cloud-connected steps when network connectivity is detected.",
            Tags = ["oobe", "cloud", "setup", "provisioning", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Oobe, "DisableOOBEWithNetworkConnectivity", 1)],
            RemoveOps = [RegOp.DeleteValue(Oobe, "DisableOOBEWithNetworkConnectivity")],
            DetectOps = [RegOp.CheckDword(Oobe, "DisableOOBEWithNetworkConnectivity", 1)],
        },
        new TweakDef
        {
            Id = "devprov-disable-homegroup",
            Label = "HomeGroup: Prevent computers from joining a HomeGroup",
            Category = "Device Provisioning Policy",
            Description =
                "Sets DisableHomeGroup=1 in the HomeGroup policy key. Prevents users from joining "
                + "or creating HomeGroups. Recommended on domain-joined and managed devices.",
            Tags = ["homegroup", "sharing", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(HomeGrp, "DisableHomeGroup", 1)],
            RemoveOps = [RegOp.DeleteValue(HomeGrp, "DisableHomeGroup")],
            DetectOps = [RegOp.CheckDword(HomeGrp, "DisableHomeGroup", 1)],
        },
        new TweakDef
        {
            Id = "devprov-block-aad-workplace-join",
            Label = "Workplace Join: Block Azure AD Workplace Join",
            Category = "Device Provisioning Policy",
            Description =
                "Sets BlockAADWorkplaceJoin=1 in the WorkplaceJoin policy key. Prevents users from "
                + "adding a work or school account via the 'Access work or school' Settings page.",
            Tags = ["workplace-join", "aad", "mdm", "enrollment", "policy"],
            NeedsAdmin = true,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(WpjPol, "BlockAADWorkplaceJoin", 1)],
            RemoveOps = [RegOp.DeleteValue(WpjPol, "BlockAADWorkplaceJoin")],
            DetectOps = [RegOp.CheckDword(WpjPol, "BlockAADWorkplaceJoin", 1)],
        },
        new TweakDef
        {
            Id = "devprov-disable-wpj-flyout",
            Label = "Workplace Join: Disable the 'Connect to work or school' flyout",
            Category = "Device Provisioning Policy",
            Description =
                "Sets FlyoutDisabled=1 in the WorkplaceJoin policy key. Hides the Workplace Join "
                + "notification flyout from the Action Center and Settings entry point.",
            Tags = ["workplace-join", "flyout", "notification", "policy"],
            NeedsAdmin = true,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(WpjPol, "FlyoutDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(WpjPol, "FlyoutDisabled")],
            DetectOps = [RegOp.CheckDword(WpjPol, "FlyoutDisabled", 1)],
        },
        new TweakDef
        {
            Id = "devprov-disable-find-my-device",
            Label = "Cloud Content: Disable the Find My Device feature",
            Category = "Device Provisioning Policy",
            Description =
                "Sets DisableFindMyDevice=1 in the CloudContent policy key. Prevents Windows from "
                + "registering the device with Microsoft's Find My Device location tracking service.",
            Tags = ["find-my-device", "cloud", "location", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableFindMyDevice", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableFindMyDevice")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableFindMyDevice", 1)],
        },
        new TweakDef
        {
            Id = "devprov-disable-windows-tips",
            Label = "Cloud Content: Disable Windows Tips and suggestions",
            Category = "Device Provisioning Policy",
            Description =
                "Sets DisableSoftLanding=1 in the CloudContent policy key. Prevents Windows from "
                + "showing 'tips', 'fun facts', and soft-landing welcome content to the user.",
            Tags = ["tips", "suggestions", "cloud", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableSoftLanding", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableSoftLanding")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableSoftLanding", 1)],
        },
        new TweakDef
        {
            Id = "devprov-disable-tailored-experiences",
            Label = "Cloud Content: Disable tailored experiences with diagnostic data",
            Category = "Device Provisioning Policy",
            Description =
                "Sets DisableTailoredExperiencesWithDiagnosticData=1. Prevents Microsoft from using "
                + "diagnostic and usage telemetry to personalise tips, ads, and recommendations.",
            Tags = ["tailored-experiences", "telemetry", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(CloudContent, "DisableTailoredExperiencesWithDiagnosticData", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableTailoredExperiencesWithDiagnosticData")],
            DetectOps = [RegOp.CheckDword(CloudContent, "DisableTailoredExperiencesWithDiagnosticData", 1)],
        },
    ];
}
