namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── Sprint 662-666 ────────────────────────────────────────────────────────────
// RegiLattice.Core — Tweaks/PolicyMisc3.cs
// AutoPlay enforcement, Windows Store deployment, Lock Screen appearance,
// Remote Assistance security, and Smart Card authentication policies.
// Category varies per module — see individual class headers.
// All tweaks: NeedsAdmin = true, CorpSafe = true, declarative RegOps.

// ── Sprint 662 — PolicyAutoRun ────────────────────────────────────────────────
/// <summary>
/// AutoRun and AutoPlay Group Policy controls (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\Explorer
///           HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer
/// These keys restrict or disable AutoRun/AutoPlay behaviour via Group Policy,
/// preventing automatic execution of content from removable media.
/// </summary>
internal static class PolicyAutoRun
{
    private const string ExplorerPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";
    private const string ExplorerCur = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "autoplay-policy-disable-autoplay",
            Label = "Disable AutoPlay via Policy",
            Category = "AutoPlay Policy",
            Description =
                "Sets NoAutoplayfornonVolume=1 under the Windows\\Explorer Group Policy key. "
                + "Prevents AutoPlay from running when non-volume devices such as cameras, phones, "
                + "or audio players are connected. "
                + "Removes the risk of malicious autorun payloads executing on connection.",
            Tags = ["autoplay", "usb", "policy", "security", "removable"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "AutoPlay disabled for non-volume devices; prevents accidental execution from USB accessories.",
            ApplyOps = [RegOp.SetDword(ExplorerPol, "NoAutoplayfornonVolume", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerPol, "NoAutoplayfornonVolume")],
            DetectOps = [RegOp.CheckDword(ExplorerPol, "NoAutoplayfornonVolume", 1)],
        },
        new TweakDef
        {
            Id = "autoplay-policy-disable-autorun",
            Label = "Disable AutoRun via Policy (All Drives)",
            Category = "AutoPlay Policy",
            Description =
                "Sets NoDriveTypeAutoRun=0xFF under the Windows\\CurrentVersion\\Policies\\Explorer key. "
                + "Disables AutoRun for all drive types (0xFF = all bits set), including optical drives, removable drives, "
                + "network drives, and fixed drives. "
                + "Prevents malware from exploiting the autorun.inf mechanism on any media.",
            Tags = ["autorun", "autoplay", "drives", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "AutoRun disabled on all drive types; eliminates autorun.inf exploitation vector.",
            ApplyOps = [RegOp.SetDword(ExplorerCur, "NoDriveTypeAutoRun", 0xFF)],
            RemoveOps = [RegOp.DeleteValue(ExplorerCur, "NoDriveTypeAutoRun")],
            DetectOps = [RegOp.CheckDword(ExplorerCur, "NoDriveTypeAutoRun", 0xFF)],
        },
        new TweakDef
        {
            Id = "autoplay-policy-disable-removable",
            Label = "Disable AutoRun for Removable Drives",
            Category = "AutoPlay Policy",
            Description =
                "Sets NoDriveTypeAutoRun=0x04 under the Policies\\Explorer key. "
                + "Specifically targets drive type 4 (removable/USB flash drives) for AutoRun suppression. "
                + "The most common vector for USB-delivered malware; removing autorun.inf execution eliminates drive-by infections.",
            Tags = ["autorun", "usb", "removable", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Removable-drive AutoRun blocked; USB flash drives will not auto-execute any payload.",
            ApplyOps = [RegOp.SetDword(ExplorerCur, "NoDriveTypeAutoRun", 4)],
            RemoveOps = [RegOp.DeleteValue(ExplorerCur, "NoDriveTypeAutoRun")],
            DetectOps = [RegOp.CheckDword(ExplorerCur, "NoDriveTypeAutoRun", 4)],
        },
        new TweakDef
        {
            Id = "autoplay-policy-default-no-action",
            Label = "Set AutoPlay Default Action to No Action",
            Category = "AutoPlay Policy",
            Description =
                "Sets NoAutorun=1 under the Policies\\Explorer key. "
                + "Forces the AutoPlay handler to take no action by default when media or devices are inserted, "
                + "requiring explicit user choice instead of automatic program launch. "
                + "Prevents silent background execution of AutoPlay handlers.",
            Tags = ["autoplay", "default", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "AutoPlay shows a prompt instead of executing; users must explicitly choose an action.",
            ApplyOps = [RegOp.SetDword(ExplorerCur, "NoAutorun", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerCur, "NoAutorun")],
            DetectOps = [RegOp.CheckDword(ExplorerCur, "NoAutorun", 1)],
        },
        new TweakDef
        {
            Id = "autoplay-policy-prevent-mixed-content",
            Label = "Prevent AutoPlay for Mixed-Content Drives",
            Category = "AutoPlay Policy",
            Description =
                "Sets HonorAutorunSetting=1 under the Windows\\Explorer Group Policy key. "
                + "Instructs Windows to honour the AutoRun setting from the device itself for mixed-content discs "
                + "(CD/DVD with both data and audio tracks). "
                + "Ensures that mixed-content media does not bypass AutoRun suppression policies.",
            Tags = ["autoplay", "cd", "dvd", "mixed-content", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Mixed-content media respects per-device AutoRun flags; no automatic execution on insert.",
            ApplyOps = [RegOp.SetDword(ExplorerPol, "HonorAutorunSetting", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerPol, "HonorAutorunSetting")],
            DetectOps = [RegOp.CheckDword(ExplorerPol, "HonorAutorunSetting", 1)],
        },
        new TweakDef
        {
            Id = "autoplay-policy-disable-cd-autoplay",
            Label = "Disable AutoPlay for CD/DVD Drives",
            Category = "AutoPlay Policy",
            Description =
                "Sets NoDriveTypeAutoRun with bit 0x20 (0x20 = optical drives) under Policies\\Explorer. "
                + "Disables AutoRun specifically for CD-ROM and DVD drives without affecting other drive types. "
                + "Prevents the classic optical-media autorun.inf attack vector while keeping USB handling configurable.",
            Tags = ["autoplay", "cd", "dvd", "optical", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "CD and DVD autorun suppressed; physical media will not auto-execute on insert.",
            ApplyOps = [RegOp.SetDword(ExplorerCur, "NoDriveTypeAutoRun", 0x20)],
            RemoveOps = [RegOp.DeleteValue(ExplorerCur, "NoDriveTypeAutoRun")],
            DetectOps = [RegOp.CheckDword(ExplorerCur, "NoDriveTypeAutoRun", 0x20)],
        },
        new TweakDef
        {
            Id = "autoplay-policy-block-set-default",
            Label = "Block Users from Changing AutoPlay Default",
            Category = "AutoPlay Policy",
            Description =
                "Sets NoAutoplayBackpropagation=1 under the Windows\\Explorer Group Policy key. "
                + "Prevents the AutoPlay dialog from remembering and persisting new user-selected defaults. "
                + "Ensures that the centrally-configured AutoPlay policy is not overridden by individual user actions.",
            Tags = ["autoplay", "policy", "enterprise", "default"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "User-selected AutoPlay defaults are not saved; policy setting always takes precedence.",
            ApplyOps = [RegOp.SetDword(ExplorerPol, "NoAutoplayBackpropagation", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerPol, "NoAutoplayBackpropagation")],
            DetectOps = [RegOp.CheckDword(ExplorerPol, "NoAutoplayBackpropagation", 1)],
        },
        new TweakDef
        {
            Id = "autoplay-policy-disable-network-autoplay",
            Label = "Disable AutoPlay for Network Drives",
            Category = "AutoPlay Policy",
            Description =
                "Sets NoDriveTypeAutoRun with bit 0x40 (0x40 = network drives) under Policies\\Explorer. "
                + "Disables AutoRun for mapped network drives. "
                + "Prevents rogue network shares from triggering AutoPlay handlers when browsed by Explorer.",
            Tags = ["autoplay", "network", "share", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Network drive AutoPlay suppressed; mapped shares cannot trigger auto-execution.",
            ApplyOps = [RegOp.SetDword(ExplorerCur, "NoDriveTypeAutoRun", 0x40)],
            RemoveOps = [RegOp.DeleteValue(ExplorerCur, "NoDriveTypeAutoRun")],
            DetectOps = [RegOp.CheckDword(ExplorerCur, "NoDriveTypeAutoRun", 0x40)],
        },
        new TweakDef
        {
            Id = "autoplay-policy-disable-shell-autoplay-handlers",
            Label = "Disable Shell AutoPlay Handlers for Removable Media",
            Category = "AutoPlay Policy",
            Description =
                "Sets DisableAutoplayForRemovableMedia=1 under the Windows\\Explorer Group Policy key. "
                + "Suppresses all shell AutoPlay handler registrations for removable media when enforced via Group Policy. "
                + "Ensures that third-party software cannot add its own AutoPlay handler entries for USB drives.",
            Tags = ["autoplay", "shell", "handlers", "removable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Third-party AutoPlay handler registrations for removable media are blocked.",
            ApplyOps = [RegOp.SetDword(ExplorerPol, "DisableAutoplayForRemovableMedia", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerPol, "DisableAutoplayForRemovableMedia")],
            DetectOps = [RegOp.CheckDword(ExplorerPol, "DisableAutoplayForRemovableMedia", 1)],
        },
        new TweakDef
        {
            Id = "autoplay-policy-turn-off-autoplay",
            Label = "Turn Off AutoPlay for All Media Types",
            Category = "AutoPlay Policy",
            Description =
                "Sets DisableAutoplay=1 under the Windows\\Explorer Group Policy key — the master switch. "
                + "Completely disables the AutoPlay feature for ALL media and devices system-wide. "
                + "When this policy is applied, Windows will not process autorun.inf files or launch AutoPlay dialog handlers "
                + "regardless of device type.",
            Tags = ["autoplay", "disable", "master", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "AutoPlay completely disabled system-wide; no media or device will trigger automatic actions.",
            ApplyOps = [RegOp.SetDword(ExplorerPol, "DisableAutoplay", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplorerPol, "DisableAutoplay")],
            DetectOps = [RegOp.CheckDword(ExplorerPol, "DisableAutoplay", 1)],
        },
    ];
}

// ── Sprint 663 — PolicyWindowsStore ──────────────────────────────────────────
/// <summary>
/// Windows Store application deployment and installation policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\WindowsStore
/// These keys control whether users can access the Microsoft Store, install apps,
/// and how updates are distributed in managed environments.
/// </summary>
internal static class PolicyWindowsStore
{
    private const string StoreKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "storepl-policy-disable-store",
            Label = "Disable Windows Store App via Policy",
            Category = "Store Policy",
            Description =
                "Sets DisableStoreApps=1 under the WindowsStore Group Policy key. "
                + "Prevents users from launching the Microsoft Store application. "
                + "Useful in enterprise environments where app distribution is controlled via SCCM, Intune, or WSUS.",
            Tags = ["store", "msstore", "policy", "enterprise", "disable"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Microsoft Store UI blocked; app installation via Store prevented for all users.",
            ApplyOps = [RegOp.SetDword(StoreKey, "DisableStoreApps", 1)],
            RemoveOps = [RegOp.DeleteValue(StoreKey, "DisableStoreApps")],
            DetectOps = [RegOp.CheckDword(StoreKey, "DisableStoreApps", 1)],
        },
        new TweakDef
        {
            Id = "storepl-policy-remove-store",
            Label = "Remove Windows Store from Settings",
            Category = "Store Policy",
            Description =
                "Sets RemoveWindowsStore=1 under the WindowsStore Group Policy key. "
                + "Removes the Microsoft Store entry from Settings and any related UI surfaces. "
                + "More aggressive than DisableStoreApps — hides the Store completely rather than just blocking launch.",
            Tags = ["store", "remove", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Store entry removed from Settings UI; users cannot navigate to or launch the Store.",
            ApplyOps = [RegOp.SetDword(StoreKey, "RemoveWindowsStore", 1)],
            RemoveOps = [RegOp.DeleteValue(StoreKey, "RemoveWindowsStore")],
            DetectOps = [RegOp.CheckDword(StoreKey, "RemoveWindowsStore", 1)],
        },
        new TweakDef
        {
            Id = "storepl-policy-require-private-store",
            Label = "Require Private Store Only",
            Category = "Store Policy",
            Description =
                "Sets RequirePrivateStoreOnly=1 under the WindowsStore Group Policy key. "
                + "Restricts the Store application to show only apps from the organisation's private Store catalogue, "
                + "blocking access to the public Microsoft Store inventory. "
                + "Standard enterprise governance control for app deployment.",
            Tags = ["store", "private", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Store displays private catalogue only; public app listings not visible to users.",
            ApplyOps = [RegOp.SetDword(StoreKey, "RequirePrivateStoreOnly", 1)],
            RemoveOps = [RegOp.DeleteValue(StoreKey, "RequirePrivateStoreOnly")],
            DetectOps = [RegOp.CheckDword(StoreKey, "RequirePrivateStoreOnly", 1)],
        },
        new TweakDef
        {
            Id = "storepl-policy-disable-auto-download",
            Label = "Disable Automatic App Download in Store",
            Category = "Store Policy",
            Description =
                "Sets AutoDownload=2 (disabled) under the WindowsStore Group Policy key. "
                + "Prevents the Store from automatically downloading app updates in the background. "
                + "Useful when bandwidth or storage consumption must be controlled centrally.",
            Tags = ["store", "download", "auto-update", "policy", "bandwidth"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Store app updates no longer download automatically; manual update required.",
            ApplyOps = [RegOp.SetDword(StoreKey, "AutoDownload", 2)],
            RemoveOps = [RegOp.DeleteValue(StoreKey, "AutoDownload")],
            DetectOps = [RegOp.CheckDword(StoreKey, "AutoDownload", 2)],
        },
        new TweakDef
        {
            Id = "storepl-policy-disable-offer-to-update",
            Label = "Disable Store Update Offers to Users",
            Category = "Store Policy",
            Description =
                "Sets DisableOSUpgrade=1 under the WindowsStore Group Policy key. "
                + "Prevents the Microsoft Store from showing upgrade or update offers to users. "
                + "Stops OS-level upgrade promotions delivered through the Store channel.",
            Tags = ["store", "upgrade", "update", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Store-delivered OS upgrade and update offers will not be shown to users.",
            ApplyOps = [RegOp.SetDword(StoreKey, "DisableOSUpgrade", 1)],
            RemoveOps = [RegOp.DeleteValue(StoreKey, "DisableOSUpgrade")],
            DetectOps = [RegOp.CheckDword(StoreKey, "DisableOSUpgrade", 1)],
        },
        new TweakDef
        {
            Id = "storepl-policy-turn-off-store-notifications",
            Label = "Turn Off Store Notifications",
            Category = "Store Policy",
            Description =
                "Sets TurnOffStoreNotifications=1 under the WindowsStore Group Policy key. "
                + "Suppresses all notification toasts generated by the Windows Store, including app update availability, "
                + "promotional offers, and feature announcements. "
                + "Reduces notification noise in managed environments.",
            Tags = ["store", "notifications", "toasts", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Store-generated notification toasts are suppressed for all users.",
            ApplyOps = [RegOp.SetDword(StoreKey, "TurnOffStoreNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(StoreKey, "TurnOffStoreNotifications")],
            DetectOps = [RegOp.CheckDword(StoreKey, "TurnOffStoreNotifications", 1)],
        },
        new TweakDef
        {
            Id = "storepl-policy-disable-purchase",
            Label = "Disable Store App Purchases",
            Category = "Store Policy",
            Description =
                "Sets DisablePurchasePage=1 under the WindowsStore Group Policy key. "
                + "Prevents users from viewing or completing in-app purchases or paid app transactions in the Store. "
                + "Blocks the payment flow entirely, preventing accidental or unauthorised charges.",
            Tags = ["store", "purchase", "payment", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "In-app purchases and paid app transactions are blocked in the Store.",
            ApplyOps = [RegOp.SetDword(StoreKey, "DisablePurchasePage", 1)],
            RemoveOps = [RegOp.DeleteValue(StoreKey, "DisablePurchasePage")],
            DetectOps = [RegOp.CheckDword(StoreKey, "DisablePurchasePage", 1)],
        },
        new TweakDef
        {
            Id = "storepl-policy-disable-app-install",
            Label = "Block Users from Installing Apps",
            Category = "Store Policy",
            Description =
                "Sets BlockNonAdminUserInstall=1 under the WindowsStore Group Policy key. "
                + "Prevents non-administrator users from initiating app installations from any source via the Store. "
                + "Centralises app permission to administrators only.",
            Tags = ["store", "install", "non-admin", "policy", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Standard users cannot install apps; only administrators can approve and install.",
            ApplyOps = [RegOp.SetDword(StoreKey, "BlockNonAdminUserInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(StoreKey, "BlockNonAdminUserInstall")],
            DetectOps = [RegOp.CheckDword(StoreKey, "BlockNonAdminUserInstall", 1)],
        },
        new TweakDef
        {
            Id = "storepl-policy-disable-video-streaming",
            Label = "Disable Store Video Streaming",
            Category = "Store Policy",
            Description =
                "Sets DisableVideoPage=1 under the WindowsStore Group Policy key. "
                + "Hides the Video section of the Microsoft Store, preventing access to streaming video content purchasing. "
                + "Reduces bandwidth usage and removes non-productivity content from the Store in enterprise settings.",
            Tags = ["store", "video", "streaming", "policy", "bandwidth"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Store Video section hidden; streaming purchases not accessible.",
            ApplyOps = [RegOp.SetDword(StoreKey, "DisableVideoPage", 1)],
            RemoveOps = [RegOp.DeleteValue(StoreKey, "DisableVideoPage")],
            DetectOps = [RegOp.CheckDword(StoreKey, "DisableVideoPage", 1)],
        },
        new TweakDef
        {
            Id = "storepl-policy-disable-music-streaming",
            Label = "Disable Store Music Streaming",
            Category = "Store Policy",
            Description =
                "Sets DisableMusicPage=1 under the WindowsStore Group Policy key. "
                + "Hides the Music section of the Microsoft Store, removing access to Groove/Xbox Music purchasing. "
                + "Complements DisableVideoPage in locking down the Store to business-relevant apps only.",
            Tags = ["store", "music", "streaming", "policy", "bandwidth"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Store Music section hidden; streaming music purchases not accessible.",
            ApplyOps = [RegOp.SetDword(StoreKey, "DisableMusicPage", 1)],
            RemoveOps = [RegOp.DeleteValue(StoreKey, "DisableMusicPage")],
            DetectOps = [RegOp.CheckDword(StoreKey, "DisableMusicPage", 1)],
        },
    ];
}

// ── Sprint 664 — PolicyLockScreen ────────────────────────────────────────────
/// <summary>
/// Lock Screen appearance and personalisation restriction policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\Personalization
/// These keys control whether the lock screen can be customised by users,
/// and which content is shown before authentication.
/// </summary>
internal static class PolicyLockScreen
{
    private const string PersonalKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "lockpol-policy-no-lock-screen",
            Label = "Disable Lock Screen via Policy",
            Category = "Lock Screen Policy",
            Description =
                "Sets NoLockScreen=1 under the Personalization Group Policy key. "
                + "Removes the lock screen entirely, proceeding directly to the sign-in screen when the computer wakes. "
                + "Reduces the number of clicks required to authenticate on shared or kiosk workstations.",
            Tags = ["lockscreen", "policy", "kiosk", "signin"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Lock screen bypassed; system jumps directly to sign-in prompt on wake.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoLockScreen")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoLockScreen", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-changing-lock-screen",
            Label = "Prevent Users from Changing Lock Screen Image",
            Category = "Lock Screen Policy",
            Description =
                "Sets NoChangingLockScreen=1 under the Personalization Group Policy key. "
                + "Prevents users from changing the lock screen background image. "
                + "Useful for enforcing a corporate-branded lock screen image across all workstations.",
            Tags = ["lockscreen", "image", "policy", "branding", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Lock screen image cannot be modified by users; centralised branding enforced.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoChangingLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoChangingLockScreen")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoChangingLockScreen", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight",
            Label = "Disable Windows Spotlight on Lock Screen",
            Category = "Lock Screen Policy",
            Description =
                "Sets NoWindowsSpotlightOnLockScreen=1 under the Personalization Group Policy key. "
                + "Prevents Windows Spotlight from downloading and displaying rotating background images, "
                + "ads, and tips on the lock screen. "
                + "Eliminates background network traffic and removes Microsoft advertising from the pre-auth surface.",
            Tags = ["lockscreen", "spotlight", "policy", "privacy", "ads"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Windows Spotlight disabled on lock screen; no rotating images or advertising content.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoWindowsSpotlightOnLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoWindowsSpotlightOnLockScreen")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoWindowsSpotlightOnLockScreen", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight-action-center",
            Label = "Disable Windows Spotlight in Action Centre",
            Category = "Lock Screen Policy",
            Description =
                "Sets NoWindowsSpotlightOnActionCenter=1 under the Personalization Group Policy key. "
                + "Prevents Windows Spotlight suggestions and featured apps from appearing in the Action Centre panel. "
                + "Removes promotional content from the notification/action area.",
            Tags = ["lockscreen", "spotlight", "action-center", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Spotlight suggestions removed from Action Centre; notification area shows only real notifications.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoWindowsSpotlightOnActionCenter", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoWindowsSpotlightOnActionCenter")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoWindowsSpotlightOnActionCenter", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-third-party-suggestions",
            Label = "Disable Third-Party App Suggestions on Lock Screen",
            Category = "Lock Screen Policy",
            Description =
                "Sets NoThirdPartySuggestions=1 under the Personalization Group Policy key. "
                + "Prevents Windows from displaying app suggestions from third-party publishers on the lock screen. "
                + "Eliminates advertising and unsolicited install prompts from the sign-in surface.",
            Tags = ["lockscreen", "suggestions", "ads", "third-party", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Third-party app suggestions removed from lock screen; clean pre-auth experience.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoThirdPartySuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoThirdPartySuggestions")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoThirdPartySuggestions", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight-windows-welcome",
            Label = "Disable Windows Welcome Experience Spotlight",
            Category = "Lock Screen Policy",
            Description =
                "Sets NoWindowsSpotlightWindowsWelcomeExperience=1 under the Personalization Group Policy key. "
                + "Prevents the 'What's new in Windows' welcome experience from appearing after feature updates. "
                + "Stops an animated fullscreen takeover that introduces new features at the expense of user focus.",
            Tags = ["lockscreen", "spotlight", "welcome", "policy", "onboarding"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Post-update Windows Welcome Experience spotlight is suppressed.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoWindowsSpotlightWindowsWelcomeExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoWindowsSpotlightWindowsWelcomeExperience")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoWindowsSpotlightWindowsWelcomeExperience", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-lock-screen-camera",
            Label = "Disable Camera Access from Lock Screen",
            Category = "Lock Screen Policy",
            Description =
                "Sets NoCameraOnLockScreen=1 under the Personalization Group Policy key. "
                + "Prevents the Camera app from launching directly from the lock screen. "
                + "Eliminates a potential avenue for taking photos or accessing media without authenticating first.",
            Tags = ["lockscreen", "camera", "policy", "security", "access"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Camera cannot be launched from lock screen; full authentication required to access camera.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoCameraOnLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoCameraOnLockScreen")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoCameraOnLockScreen", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight-features",
            Label = "Turn Off All Spotlight Features",
            Category = "Lock Screen Policy",
            Description =
                "Sets ConfigureWindowsSpotlight=2 under the Personalization Group Policy key. "
                + "Value 2 applies the most restrictive Spotlight policy: disabled entirely. "
                + "This replaces all per-feature Spotlight toggles with a single master-off switch for Group Policy compliance.",
            Tags = ["lockscreen", "spotlight", "master", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "All Windows Spotlight feature categories disabled via a single master policy value.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "ConfigureWindowsSpotlight", 2)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "ConfigureWindowsSpotlight")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "ConfigureWindowsSpotlight", 2)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight-taskbar",
            Label = "Disable Spotlight Suggestions in Taskbar Search",
            Category = "Lock Screen Policy",
            Description =
                "Sets NoWindowsSpotlightInSearch=1 under the Personalization Group Policy key. "
                + "Removes Microsoft-curated Spotlight content suggestions from appearing in the Windows Search bar on the taskbar. "
                + "Search results show only local and Bing content, not Spotlight-injected promotions.",
            Tags = ["lockscreen", "spotlight", "search", "taskbar", "policy", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Spotlight suggestions removed from taskbar Search; search shows only query results.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoWindowsSpotlightInSearch", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoWindowsSpotlightInSearch")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoWindowsSpotlightInSearch", 1)],
        },
        new TweakDef
        {
            Id = "lockpol-policy-no-spotlight-settings",
            Label = "Disable Spotlight Tips in Settings",
            Category = "Lock Screen Policy",
            Description =
                "Sets NoWindowsSpotlightInSettings=1 under the Personalization Group Policy key. "
                + "Removes the Windows Spotlight-powered tips and feature highlights that appear throughout the Settings app. "
                + "Reduces noise from Microsoft suggestions inside Settings pages.",
            Tags = ["lockscreen", "spotlight", "settings", "tips", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Spotlight tips and suggestions removed from all Settings pages.",
            ApplyOps = [RegOp.SetDword(PersonalKey, "NoWindowsSpotlightInSettings", 1)],
            RemoveOps = [RegOp.DeleteValue(PersonalKey, "NoWindowsSpotlightInSettings")],
            DetectOps = [RegOp.CheckDword(PersonalKey, "NoWindowsSpotlightInSettings", 1)],
        },
    ];
}

// ── Sprint 665 — PolicyRemoteAssistance ──────────────────────────────────────
/// <summary>
/// Remote Assistance security and behavioural Group Policy controls (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\RemoteAssistance
/// These keys govern Windows Remote Assistance (as distinct from RDP), including
/// solicitation controls, session duration, and ticket lifetime.
/// </summary>
internal static class PolicyRemoteAssistance
{
    private const string RemAssist = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";
    private const string RemAssistRA = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemoteAssistance";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "remassist-policy-disable-remote-assistance",
            Label = "Disable Remote Assistance Completely",
            Category = "Remote Assistance Policy",
            Description =
                "Sets fAllowToGetHelp=0 under the Terminal Services Group Policy key. "
                + "Prevents users from requesting or receiving Remote Assistance connections. "
                + "Closes the Remote Assistance channel entirely, which is distinct from Remote Desktop (RDP).",
            Tags = ["remote-assistance", "ra", "policy", "security", "disable"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Remote Assistance (msra.exe) blocked; no helper can connect via the RA channel.",
            ApplyOps = [RegOp.SetDword(RemAssist, "fAllowToGetHelp", 0)],
            RemoveOps = [RegOp.DeleteValue(RemAssist, "fAllowToGetHelp")],
            DetectOps = [RegOp.CheckDword(RemAssist, "fAllowToGetHelp", 0)],
        },
        new TweakDef
        {
            Id = "remassist-policy-disable-offer-ra",
            Label = "Disable Offer Remote Assistance",
            Category = "Remote Assistance Policy",
            Description =
                "Sets fAllowUnsolicited=0 under the Terminal Services Group Policy key. "
                + "Prevents helpers from offering unsolicited Remote Assistance sessions. "
                + "Disables the 'Offer RA' variant where a helper can push a connection without the user initiating a request.",
            Tags = ["remote-assistance", "offer", "unsolicited", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Unsolicited Remote Assistance offers blocked; only user-initiated requests allowed.",
            ApplyOps = [RegOp.SetDword(RemAssist, "fAllowUnsolicited", 0)],
            RemoveOps = [RegOp.DeleteValue(RemAssist, "fAllowUnsolicited")],
            DetectOps = [RegOp.CheckDword(RemAssist, "fAllowUnsolicited", 0)],
        },
        new TweakDef
        {
            Id = "remassist-policy-require-explicit-prompt",
            Label = "Require Explicit User Consent for RA Control",
            Category = "Remote Assistance Policy",
            Description =
                "Sets fEnableFullControl=0 under the Terminal Services Group Policy key. "
                + "Restricts incoming Remote Assistance sessions to view-only mode; the user must grant explicit "
                + "permission before the helper can take mouse and keyboard control. "
                + "Prevents silent takeover of user sessions.",
            Tags = ["remote-assistance", "consent", "control", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "RA helper can view but not control the session until the user approves.",
            ApplyOps = [RegOp.SetDword(RemAssist, "fEnableFullControl", 0)],
            RemoveOps = [RegOp.DeleteValue(RemAssist, "fEnableFullControl")],
            DetectOps = [RegOp.CheckDword(RemAssist, "fEnableFullControl", 0)],
        },
        new TweakDef
        {
            Id = "remassist-policy-max-ticket-expiry-hours",
            Label = "Limit Remote Assistance Ticket Validity to 1 Hour",
            Category = "Remote Assistance Policy",
            Description =
                "Sets MaxTicketExpiryUnits=1 and MaxTicketExpiry=1 under the RemoteAssistance Policy key. "
                + "Limits invitation ticket validity to 1 hour. "
                + "Shortens the window during which an RA invitation can be acted upon, reducing exposure.",
            Tags = ["remote-assistance", "ticket", "expiry", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "RA invitation tickets expire within 1 hour; stale invitations cannot be reused.",
            ApplyOps =
            [
                RegOp.SetDword(RemAssistRA, "MaxTicketExpiryUnits", 1),
                RegOp.SetDword(RemAssistRA, "MaxTicketExpiry", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(RemAssistRA, "MaxTicketExpiryUnits"),
                RegOp.DeleteValue(RemAssistRA, "MaxTicketExpiry"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(RemAssistRA, "MaxTicketExpiryUnits", 1),
                RegOp.CheckDword(RemAssistRA, "MaxTicketExpiry", 1),
            ],
        },
        new TweakDef
        {
            Id = "remassist-policy-require-bandwidth-limit",
            Label = "Set Remote Assistance Bandwidth Limit",
            Category = "Remote Assistance Policy",
            Description =
                "Sets MaxAllowedBandwidth=2 under the RemoteAssistance Policy key. "
                + "Caps bandwidth consumed by a Remote Assistance session (value 2 = 2 Mbps maximum). "
                + "Prevents Remote Assistance from saturating network links during active sessions.",
            Tags = ["remote-assistance", "bandwidth", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Remote Assistance bandwidth capped at 2 Mbps; network impact during sessions is bounded.",
            ApplyOps = [RegOp.SetDword(RemAssistRA, "MaxAllowedBandwidth", 2)],
            RemoveOps = [RegOp.DeleteValue(RemAssistRA, "MaxAllowedBandwidth")],
            DetectOps = [RegOp.CheckDword(RemAssistRA, "MaxAllowedBandwidth", 2)],
        },
        new TweakDef
        {
            Id = "remassist-policy-disable-email-tickets",
            Label = "Disable Email Invitation Tickets for RA",
            Category = "Remote Assistance Policy",
            Description =
                "Sets fAllowEmailInvitations=0 under the Terminal Services Group Policy key. "
                + "Prevents users from creating Remote Assistance invitation tickets delivered by email. "
                + "Email-based RA tickets can be forwarded, intercepted, or expire without notice; disabling this channel is a security best practice.",
            Tags = ["remote-assistance", "email", "invitation", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Email-based Remote Assistance invitations disabled; only Easy Connect or file-based invites allowed.",
            ApplyOps = [RegOp.SetDword(RemAssist, "fAllowEmailInvitations", 0)],
            RemoveOps = [RegOp.DeleteValue(RemAssist, "fAllowEmailInvitations")],
            DetectOps = [RegOp.CheckDword(RemAssist, "fAllowEmailInvitations", 0)],
        },
        new TweakDef
        {
            Id = "remassist-policy-disable-easy-connect",
            Label = "Disable Easy Connect Remote Assistance",
            Category = "Remote Assistance Policy",
            Description =
                "Sets fAllowEasyConnect=0 under the RemoteAssistance Policy key. "
                + "Disables the 'Easy Connect' Remote Assistance method which uses the Peer Name Resolution Protocol "
                + "(PNRP) cloud service instead of a ticket file. "
                + "Easy Connect depends on external cloud infrastructure; disabling it constrains RA to local network or ticket methods.",
            Tags = ["remote-assistance", "easy-connect", "pnrp", "cloud", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Easy Connect cloud-based RA method disabled; ticket-file method remains available.",
            ApplyOps = [RegOp.SetDword(RemAssistRA, "fAllowEasyConnect", 0)],
            RemoveOps = [RegOp.DeleteValue(RemAssistRA, "fAllowEasyConnect")],
            DetectOps = [RegOp.CheckDword(RemAssistRA, "fAllowEasyConnect", 0)],
        },
        new TweakDef
        {
            Id = "remassist-policy-log-sessions",
            Label = "Enable Remote Assistance Session Logging",
            Category = "Remote Assistance Policy",
            Description =
                "Sets EnableRASSessionAudit=1 under the RemoteAssistance Policy key. "
                + "Enables audit logging of Remote Assistance connection events. "
                + "Records who connected, when, and for how long — supports compliance and incident investigation.",
            Tags = ["remote-assistance", "logging", "audit", "policy", "compliance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "RA session events logged to the Windows event log for audit purposes.",
            ApplyOps = [RegOp.SetDword(RemAssistRA, "EnableRASSessionAudit", 1)],
            RemoveOps = [RegOp.DeleteValue(RemAssistRA, "EnableRASSessionAudit")],
            DetectOps = [RegOp.CheckDword(RemAssistRA, "EnableRASSessionAudit", 1)],
        },
        new TweakDef
        {
            Id = "remassist-policy-disable-clipboard-transfer",
            Label = "Disable Clipboard Transfer During RA Sessions",
            Category = "Remote Assistance Policy",
            Description =
                "Sets fDisableClip=1 under the Terminal Services Group Policy key. "
                + "Prevents clipboard content from being shared between the local and remote machines during a Remote Assistance session. "
                + "Blocks data exfiltration via clipboard paste during support sessions.",
            Tags = ["remote-assistance", "clipboard", "exfiltration", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Clipboard disabled during RA sessions; copy-paste between machines not allowed.",
            ApplyOps = [RegOp.SetDword(RemAssist, "fDisableClip", 1)],
            RemoveOps = [RegOp.DeleteValue(RemAssist, "fDisableClip")],
            DetectOps = [RegOp.CheckDword(RemAssist, "fDisableClip", 1)],
        },
        new TweakDef
        {
            Id = "remassist-policy-disable-file-transfer",
            Label = "Disable File Transfer During RA Sessions",
            Category = "Remote Assistance Policy",
            Description =
                "Sets fDisableExclamation=1 under the Terminal Services Group Policy key. "
                + "Disables the file-transfer feature in Remote Assistance sessions. "
                + "Prevents the helper from sending or receiving files during a session, "
                + "blocking a common data exfiltration or dropper-delivery path.",
            Tags = ["remote-assistance", "file-transfer", "exfiltration", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "File transfer blocked during RA sessions; helpers cannot upload or download files.",
            ApplyOps = [RegOp.SetDword(RemAssist, "fDisableExclamation", 1)],
            RemoveOps = [RegOp.DeleteValue(RemAssist, "fDisableExclamation")],
            DetectOps = [RegOp.CheckDword(RemAssist, "fDisableExclamation", 1)],
        },
    ];
}

// ── Sprint 666 — PolicySmartCard ──────────────────────────────────────────────
/// <summary>
/// Smart Card authentication and credential provider Group Policy controls (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\System
/// These keys govern Smart Card logon behaviour, PIN policies, and forced card removal.
/// </summary>
internal static class PolicySmartCard
{
    private const string ScCredProv = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider";
    private const string WinSystem = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "smartcard-policy-require-smart-card",
            Label = "Require Smart Card for Interactive Logon",
            Category = "Smart Card Policy",
            Description =
                "Sets scforceoption=1 under the SmartCardCredentialProvider Group Policy key. "
                + "Forces all interactive logons to use a Smart Card credential. "
                + "Password-only logon is blocked; users must present a physical token or virtual smart card.",
            Tags = ["smartcard", "logon", "policy", "security", "mfa"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "Smart card mandatory for logon; password-only authentication blocked system-wide.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "scforceoption", 1)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "scforceoption")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "scforceoption", 1)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-remove-on-removal",
            Label = "Lock Workstation on Smart Card Removal",
            Category = "Smart Card Policy",
            Description =
                "Sets scremoveoption=1 under the SmartCardCredentialProvider Group Policy key. "
                + "Automatically locks the workstation when the smart card is removed from the reader. "
                + "Enforces physical token presence as a continuous authentication requirement; "
                + "the session is inaccessible the moment the card is removed.",
            Tags = ["smartcard", "lock", "removal", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Workstation locks instantly on card removal; no credential required to trigger the lock.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "scremoveoption", 1)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "scremoveoption")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "scremoveoption", 1)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-force-logoff-on-removal",
            Label = "Force Logoff on Smart Card Removal",
            Category = "Smart Card Policy",
            Description =
                "Sets scremoveoption=2 under the SmartCardCredentialProvider Group Policy key. "
                + "Value 2 causes a full sign-out (rather than a lock) when the smart card is removed. "
                + "More aggressive than the lock option — suitable for high-security shared terminal environments.",
            Tags = ["smartcard", "logoff", "removal", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "User is immediately logged off when card is removed; unsaved work may be lost.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "scremoveoption", 2)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "scremoveoption")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "scremoveoption", 2)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-allow-integrated-unblock",
            Label = "Allow Integrated Unblock Screen for Smart Card PIN",
            Category = "Smart Card Policy",
            Description =
                "Sets AllowIntegratedUnblock=1 under the SmartCardCredentialProvider Group Policy key. "
                + "Enables a built-in PIN unlock screen that appears on the credential provider for blocked smart cards, "
                + "removing the need for a separate help-desk intervention to reset a locked card.",
            Tags = ["smartcard", "pin", "unblock", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Users can self-service unblock a PIN-locked smart card via the Windows credential screen.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "AllowIntegratedUnblock", 1)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "AllowIntegratedUnblock")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "AllowIntegratedUnblock", 1)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-turn-on-virtual-card",
            Label = "Enable Virtual Smart Card Creation",
            Category = "Smart Card Policy",
            Description =
                "Sets AllowDomainPINLogon=1 under the SmartCardCredentialProvider Group Policy key. "
                + "Allows BitLocker Network Unlock and domain accounts to authenticate with a PIN against a virtual TPM smart card. "
                + "Enables software-based smart card equivalent for devices without physical card readers.",
            Tags = ["smartcard", "virtual", "tpm", "pin", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Virtual smart card PIN logon enabled; TPM-backed credential usable without a physical card reader.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "AllowDomainPINLogon", 1)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "AllowDomainPINLogon")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "AllowDomainPINLogon", 1)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-disable-credential-caching",
            Label = "Disable Smart Card Credential Caching",
            Category = "Smart Card Policy",
            Description =
                "Sets DisallowPlaintextPin=1 under the SmartCardCredentialProvider Group Policy key. "
                + "Prevents the credential provider from caching smart card credentials in memory as plaintext PIN. "
                + "Reduces the risk of credential harvesting by memory-resident malware.",
            Tags = ["smartcard", "cache", "pin", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Smart card plaintext PIN caching disabled; protected credential store used instead.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "DisallowPlaintextPin", 1)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "DisallowPlaintextPin")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "DisallowPlaintextPin", 1)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-enable-certificate-propagation",
            Label = "Enable Smart Card Certificate Propagation",
            Category = "Smart Card Policy",
            Description =
                "Sets CertPropEnabled=1 under the Windows\\System Group Policy key. "
                + "Enables automatic propagation of smart card certificates from the card to the user's certificate store "
                + "when the card is inserted. "
                + "Required for applications that directly query the user certificate store rather than the card reader.",
            Tags = ["smartcard", "certificate", "propagation", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Smart card certificates automatically appear in the user certificate store on card insert.",
            ApplyOps = [RegOp.SetDword(WinSystem, "CertPropEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(WinSystem, "CertPropEnabled")],
            DetectOps = [RegOp.CheckDword(WinSystem, "CertPropEnabled", 1)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-enable-cleanup-certificates",
            Label = "Clean Up Smart Card Certificates on Card Removal",
            Category = "Smart Card Policy",
            Description =
                "Sets CleanupCerts=1 under the Windows\\System Group Policy key. "
                + "Removes smart card certificates from the user store when the card is removed. "
                + "Works in conjunction with CertPropEnabled to maintain a consistent certificate state "
                + "reflecting only currently-inserted cards.",
            Tags = ["smartcard", "certificate", "cleanup", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Propagated certificates removed from user store when smart card is removed.",
            ApplyOps = [RegOp.SetDword(WinSystem, "CleanupCerts", 1)],
            RemoveOps = [RegOp.DeleteValue(WinSystem, "CleanupCerts")],
            DetectOps = [RegOp.CheckDword(WinSystem, "CleanupCerts", 1)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-turn-off-root-auto-update",
            Label = "Prevent Smart Card Root Certificate Auto-Update",
            Category = "Smart Card Policy",
            Description =
                "Sets RootCertificateAutoUpdate=0 under the SmartCardCredentialProvider key. "
                + "Prevents Windows from automatically downloading and updating root certificates from Windows Update "
                + "for smart card trust anchors. "
                + "Appropriate in air-gapped or strictly controlled environments where certificate trust is managed manually.",
            Tags = ["smartcard", "certificate", "root", "update", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Root certificate auto-update for smart cards disabled; manual CA trust management required.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "RootCertificateAutoUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "RootCertificateAutoUpdate")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "RootCertificateAutoUpdate", 0)],
        },
        new TweakDef
        {
            Id = "smartcard-policy-disable-pinpad-logon",
            Label = "Disable PIN Pad Bypass for Smart Card Logon",
            Category = "Smart Card Policy",
            Description =
                "Sets DisallowPINLessLogon=1 under the SmartCardCredentialProvider Group Policy key. "
                + "Requires a PIN to be entered for every smart card logon, even if the card supports and is configured for PINless logon. "
                + "Ensures that PIN knowledge (something-you-know) combined with card possession (something-you-have) is always required.",
            Tags = ["smartcard", "pin", "logon", "policy", "mfa", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "PINless logon blocked; card + PIN is always required for true two-factor authentication.",
            ApplyOps = [RegOp.SetDword(ScCredProv, "DisallowPINLessLogon", 1)],
            RemoveOps = [RegOp.DeleteValue(ScCredProv, "DisallowPINLessLogon")],
            DetectOps = [RegOp.CheckDword(ScCredProv, "DisallowPINLessLogon", 1)],
        },
    ];
}
