// RegiLattice.Core — Tweaks/PushToInstallPolicy.cs
// Sprint 268: Push-To-Install Group Policy (10 tweaks)
// Category: "Push To Install Policy" | Slug: pti
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PushToInstall

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PushToInstallPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PushToInstall";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "pti-disable-push-to-install",
            Label = "Disable Push-To-Install Service",
            Category = "Push To Install Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets DisablePushToInstall=1 in the PushToInstall policy key. Prevents the "
                + "Push-to-Install feature from delivering apps remotely. Push-to-Install allows "
                + "apps purchased or selected on one device to be silently installed on another "
                + "device signed in with the same Microsoft account. Blocking this prevents "
                + "unexpected app installations. Default: 0. Recommended: 1 for managed estates.",
            Tags = ["push-to-install", "store", "remote", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePushToInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePushToInstall")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePushToInstall", 1)],
        },
        new TweakDef
        {
            Id = "pti-disable-remote-push",
            Label = "Disable Remote Push App Delivery",
            Category = "Push To Install Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets DisableRemotePush=1 in the PushToInstall policy key. Blocks delivery of "
                + "applications to this device initiated from a remote session or another device. "
                + "Remote push allows an administrator or the account owner to trigger Store app "
                + "installations on a target machine without local interaction. "
                + "Default: 0. Recommended: 1 on enterprise endpoints.",
            Tags = ["push-to-install", "remote", "delivery", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableRemotePush", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRemotePush")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRemotePush", 1)],
        },
        new TweakDef
        {
            Id = "pti-disable-auto-provisioning",
            Label = "Disable Push-To-Install Auto Provisioning",
            Category = "Push To Install Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets DisableAutoProvisioning=1 in the PushToInstall policy key. Prevents "
                + "automatic app provisioning triggered by the Push-to-Install service when "
                + "a device is first joined to an account or MDM enrollment. Auto-provisioning "
                + "can install a large batch of apps without user review. "
                + "Default: 0. Recommended: 1 on carefully managed devices.",
            Tags = ["push-to-install", "provisioning", "auto", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoProvisioning", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoProvisioning")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoProvisioning", 1)],
        },
        new TweakDef
        {
            Id = "pti-disable-device-management-push",
            Label = "Disable Device Management Push Installs",
            Category = "Push To Install Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets DisableDeviceManagementPush=1 in the PushToInstall policy key. Blocks "
                + "the device management channel from using Push-to-Install to deploy Store "
                + "applications. MDM solutions such as Intune can use this channel to push "
                + "commercial app packages silently. This policy prevents that silent delivery "
                + "vector at the OS policy layer. Default: 0. Recommended: 1 when a separate "
                + "software distribution tool manages app deployment.",
            Tags = ["push-to-install", "mdm", "device-mgmt", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDeviceManagementPush", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDeviceManagementPush")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDeviceManagementPush", 1)],
        },
        new TweakDef
        {
            Id = "pti-disable-store-push-notifications",
            Label = "Disable Push-To-Install Store Notifications",
            Category = "Push To Install Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets DisableStorePushNotifications=1 in the PushToInstall policy key. Disables "
                + "notification toasts generated by the Push-to-Install service to inform users "
                + "that an app is being installed or has been successfully delivered from another "
                + "device. Reduces distraction and prevents disclosure of remote management "
                + "actions to end users. Default: 0. Recommended: 1.",
            Tags = ["push-to-install", "notifications", "store", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableStorePushNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableStorePushNotifications")],
            DetectOps = [RegOp.CheckDword(Key, "DisableStorePushNotifications", 1)],
        },
        new TweakDef
        {
            Id = "pti-disable-install-telemetry",
            Label = "Disable Push-To-Install Telemetry",
            Category = "Push To Install Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets DisableInstallTelemetry=1 in the PushToInstall policy key. Prevents the "
                + "Push-to-Install service from reporting installation events, success or failure "
                + "outcomes, and app engagement data back to Microsoft. This telemetry is "
                + "separate from standard diagnostic data and targets Store usage analytics. "
                + "Default: 0. Recommended: 1 when minimising data sharing with Microsoft.",
            Tags = ["push-to-install", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableInstallTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableInstallTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableInstallTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "pti-require-admin-approval",
            Label = "Require Admin Approval for Push-To-Install",
            Category = "Push To Install Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets RequireAdminApproval=1 in the PushToInstall policy key. Requires local "
                + "administrator confirmation before any remotely-pushed application may be "
                + "installed. This adds a UAC-equivalent gate to the push delivery pipeline, "
                + "preventing silent installs initiated from trusted Microsoft accounts or "
                + "management channels. Default: 0. Recommended: 1 for shared machines.",
            Tags = ["push-to-install", "admin", "approval", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireAdminApproval", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminApproval")],
            DetectOps = [RegOp.CheckDword(Key, "RequireAdminApproval", 1)],
        },
        new TweakDef
        {
            Id = "pti-disable-unattended-push",
            Label = "Disable Unattended Push-To-Install",
            Category = "Push To Install Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Sets DisableUnattendedPush=1 in the PushToInstall policy key. Prevents "
                + "unattended (background, no-user-present) push installations from executing "
                + "when the device screen is locked or the user is not logged in. Unattended "
                + "push can silently replace or downgrade apps on locked devices without user "
                + "knowledge. Default: 0. Recommended: 1.",
            Tags = ["push-to-install", "unattended", "background", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableUnattendedPush", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableUnattendedPush")],
            DetectOps = [RegOp.CheckDword(Key, "DisableUnattendedPush", 1)],
        },
        new TweakDef
        {
            Id = "pti-disable-cross-device-sync",
            Label = "Disable Push-To-Install Cross-Device Sync",
            Category = "Push To Install Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets DisableCrossDeviceSync=1 in the PushToInstall policy key. Prevents "
                + "synchronisation of the app installation queue across devices in the same "
                + "Microsoft account family. Without this policy, purchasing an app on a phone "
                + "or Xbox console can trigger a silent push install to all Windows devices "
                + "in the account. Default: 0. Recommended: 1 for isolation between devices.",
            Tags = ["push-to-install", "sync", "cross-device", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCrossDeviceSync", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCrossDeviceSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCrossDeviceSync", 1)],
        },
        new TweakDef
        {
            Id = "pti-disable-push-service-wake",
            Label = "Disable Push-To-Install Service Wake",
            Category = "Push To Install Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Sets DisablePushServiceWake=1 in the PushToInstall policy key. Prevents the "
                + "Push-to-Install background service from waking the device from sleep or "
                + "connected standby to complete a pending installation. This can cause "
                + "unexpected fan spin, battery drain, or network activity while the device "
                + "is supposedly idle or in a bag. Default: 0. Recommended: 1 for laptops.",
            Tags = ["push-to-install", "sleep", "wake", "power", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePushServiceWake", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePushServiceWake")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePushServiceWake", 1)],
        },
    ];
}
