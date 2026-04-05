namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static partial class PolicyDesktop
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _AutoPlayPolicy.Data,
            .. _AutoRunPolicy.Data,
            .. _ColorCalibrationPolicy.Data,
            .. _ColorManagement.Data,
            .. _CompartmentPolicy.Data,
            .. _ControlPanelPolicy.Data,
            .. _DirectXRenderingPolicy.Data,
            .. _DirectXShaderCachePolicy.Data,
            .. _DisplayAdapterPolicy.Data,
            .. _FocusAssistPolicy.Data,
            .. _FontInstallationPolicy.Data,
            .. _FontProviderPolicy.Data,
            .. _GdiRendererPolicy.Data,
            .. _GpuComputePolicy.Data,
            .. _GraphicsDriversPolicy.Data,
            .. _InputMethodPolicy.Data,
            .. _InputPersonalizationPolicy.Data,
            .. _KioskAssignedAccess.Data,
            .. _KioskBrowserPolicy.Data,
            .. _LanguageOptionsPolicy.Data,
            .. _MobilityCenterPolicy.Data,
            .. _ModernStandbyPolicy.Data,
            .. _PenWorkspaceGpoPolicy.Data,
            .. _PersonalizationLockPolicy.Data,
            .. _PersonalizationPolicy.Data,
            .. _PlayToDevicePolicy.Data,
            .. _ScreenSaverSecurityPolicy.Data,
            .. _SharedClipboardControlPolicy.Data,
            .. _ShellRestrictionsPolicy.Data,
            .. _ShutdownOptionsPolicy.Data,
            .. _SidebarGadgetsPolicy.Data,
            .. _StartMenuModernPolicy.Data,
            .. _SudoWindowsPolicy.Data,
            .. _SystemShutdown.Data,
            .. _TabletPcInputPolicy.Data,
            .. _TouchpadGestures.Data,
            .. _VideoCapturePolicy.Data,
            .. _VirtualKeyboardPolicy.Data,
            .. _WddmDriverPolicy.Data,
            .. _WiaImageAcquisitionPolicy.Data,
            .. _WindowsAccessibilityPolicy.Data,
            .. _WindowsInkWorkspaceAdvPolicy.Data,
            .. _WindowsSearchAdv.Data,
            .. _WindowsSearchIndexingAdvancedPolicy.Data,
            .. _VirtualizationPolicy.Data,
        ];

    // ── AutoPlayPolicy ──
    private static class _AutoPlayPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AutoPlay";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "autoplay-disable-autorun-all",
                    Label = "Disable AutoRun on All Drive Types",
                    Category = "Display — Auto Play",
                    Description =
                        "Disables AutoRun on all drive types (removable, fixed, optical, network, RAM disk). Prevents automatic execution of malware from inserted USB drives or optical media — one of the most common infection vectors. Default: AutoRun enabled for some types. Recommended: 255 (all types).",
                    Tags = ["autoplay", "autorun", "usb", "removable", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "AutoRun is disabled on all media; no code executes automatically when media is inserted.",
                    ApplyOps = [RegOp.SetDword(Key, "NoAutorun", 255)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoAutorun")],
                    DetectOps = [RegOp.CheckDword(Key, "NoAutorun", 255)],
                },
                new TweakDef
                {
                    Id = "autoplay-disable-for-removable",
                    Label = "Disable AutoPlay on Removable Drives",
                    Category = "Display — Auto Play",
                    Description =
                        "Turns off the AutoPlay dialog for removable storage (USB flash drives, memory cards). Users must manually open and browse inserted removable media. Default: AutoPlay dialog shown. Recommended: 1.",
                    Tags = ["autoplay", "usb", "removable", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "No AutoPlay dialog appears for USB stick or SD card insertions.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoplayForNonVolume", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoplayForNonVolume")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoplayForNonVolume", 1)],
                },
                new TweakDef
                {
                    Id = "autoplay-disable-for-optical",
                    Label = "Disable AutoPlay on Optical Drives (CD/DVD/Blu-ray)",
                    Category = "Display — Auto Play",
                    Description =
                        "Disables the AutoPlay dialog when a CD, DVD, or Blu-ray disc is inserted. Prevents automatic installation, media play, or execution of disc content. Default: AutoPlay shown for optical discs. Recommended: 1.",
                    Tags = ["autoplay", "cd", "dvd", "optical", "media", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "No AutoPlay dialog for optical disc insertions; disc contents must be browsed manually.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoplayForOptical", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoplayForOptical")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoplayForOptical", 1)],
                },
                new TweakDef
                {
                    Id = "autoplay-disable-for-network",
                    Label = "Disable AutoPlay on Network Drives",
                    Category = "Display — Auto Play",
                    Description =
                        "Prevents the AutoPlay dialog from opening when a network drive is mapped or connected. Eliminates risk from autorun.inf files on network shares. Default: AutoPlay disabled on network by default in Windows 10+. Recommended: 1 for explicit policy enforcement.",
                    Tags = ["autoplay", "network", "share", "smb", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "AutoPlay is explicitly blocked on network drives via policy.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoplayForNetworkDrive", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoplayForNetworkDrive")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoplayForNetworkDrive", 1)],
                },
                new TweakDef
                {
                    Id = "autoplay-set-default-action-none",
                    Label = "Set AutoPlay Default Action to 'Take No Action'",
                    Category = "Display — Auto Play",
                    Description =
                        "Configures the AutoPlay default handler for all media types to 'Take no action'. Even if AutoPlay is not fully disabled, no action is taken automatically on media insertion. Default: Windows auto-selects handler. Recommended: 1.",
                    Tags = ["autoplay", "default-action", "media", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "AutoPlay takes no action on any media type; users see no dialog and no auto-launch occurs.",
                    ApplyOps = [RegOp.SetDword(Key, "NoAutoplayDefaultAction", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoAutoplayDefaultAction")],
                    DetectOps = [RegOp.CheckDword(Key, "NoAutoplayDefaultAction", 1)],
                },
                new TweakDef
                {
                    Id = "autoplay-block-autorun-inf",
                    Label = "Block autorun.inf Execution from Any Drive",
                    Category = "Display — Auto Play",
                    Description =
                        "Explicitly blocks execution of autorun.inf files from all drive types. The autorun.inf mechanism is the primary vehicle for USB weaponisation. Default: blocked on fixed/network drives in modern Windows, but enforced here for all types. Recommended: 1.",
                    Tags = ["autoplay", "autorun.inf", "usb", "malware", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "autorun.inf files are ignored on all drive types; USB malware relying on this vector is blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAutorunInf", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAutorunInf")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAutorunInf", 1)],
                },
                new TweakDef
                {
                    Id = "autoplay-block-user-override",
                    Label = "Block Users from Changing AutoPlay Settings",
                    Category = "Display — Auto Play",
                    Description =
                        "Prevents users from changing AutoPlay settings in Settings → Bluetooth & Devices → AutoPlay. Ensures the IT-configured AutoPlay policy cannot be overridden by end users. Default: users can change. Recommended: 1.",
                    Tags = ["autoplay", "user-restriction", "settings", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "AutoPlay settings page is locked; users cannot re-enable AutoPlay or change media defaults.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAutoplaySettingsChange", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAutoplaySettingsChange")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAutoplaySettingsChange", 1)],
                },
                new TweakDef
                {
                    Id = "autoplay-disable-for-camera-import",
                    Label = "Disable AutoPlay for Camera / Photo Import",
                    Category = "Display — Auto Play",
                    Description =
                        "Prevents the AutoPlay dialog from offering to import photos/videos when a digital camera or phone is connected. Users must manually launch the import workflow. Default: AutoPlay dialog offered. Recommended: 1 to prevent unintended data access.",
                    Tags = ["autoplay", "camera", "photo", "import", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Camera/phone connections do not trigger the AutoPlay photo import dialog.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoPlayForCamera", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoPlayForCamera")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoPlayForCamera", 1)],
                },
                new TweakDef
                {
                    Id = "autoplay-log-media-insertions",
                    Label = "Audit Log Media Insertion Events",
                    Category = "Display — Auto Play",
                    Description =
                        "Enables logging of removable media insertion events to the Security audit log. Provides a device usage trail for DLP and forensic investigations. Default: not audited. Recommended: 1 on monitored endpoints.",
                    Tags = ["autoplay", "audit", "media", "usb", "dlp", "forensics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "USB/optical media insertions are recorded in the Security event log for forensic purposes.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditMediaInsertions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditMediaInsertions")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditMediaInsertions", 1)],
                },
                new TweakDef
                {
                    Id = "autoplay-disable-for-mtp-devices",
                    Label = "Disable AutoPlay for MTP / Portable Devices",
                    Category = "Display — Auto Play",
                    Description =
                        "Turns off AutoPlay for MTP (Media Transfer Protocol) devices such as smartphones, tablets, and MP3 players. Stops automatic launch of Windows Photo Import or Windows Media Player when a mobile device is connected. Default: AutoPlay dialog offered. Recommended: 1.",
                    Tags = ["autoplay", "mtp", "mobile", "portable-device", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Connecting a phone/tablet does not trigger AutoPlay; users must manually open File Explorer.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoPlayForMTP", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoPlayForMTP")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoPlayForMTP", 1)],
                },
            ];
    }

    // ── AutoRunPolicy ──
    private static class _AutoRunPolicy
    {
        private const string AutoRunUser = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";

        private const string AutoRunSys = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";

        private const string AutoPlayUser = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers";

        private const string AutoRunPolicy2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "autorun-disable-all-drives",
                Label = "Disable AutoRun on All Drive Types",
                Category = "Display — Auto Play",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["autorun", "autoplay", "security", "usb", "removable"],
                Description =
                    "Disables AutoRun on all drive types (0xFF = 255 = all drives). "
                    + "Prevents malware from auto-executing via USB drives, CDs, or network "
                    + "shares. One of the most effective defenses against physical-access malware.",
                ApplyOps = [RegOp.SetDword(AutoRunUser, "NoDriveTypeAutoRun", 0xFF)],
                RemoveOps = [RegOp.DeleteValue(AutoRunUser, "NoDriveTypeAutoRun")],
                DetectOps = [RegOp.CheckDword(AutoRunUser, "NoDriveTypeAutoRun", 0xFF)],
            },
            new TweakDef
            {
                Id = "autorun-disable-autoplay-default",
                Label = "Disable AutoPlay Default Handler",
                Category = "Display — Auto Play",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["autorun", "autoplay", "default", "usb"],
                Description =
                    "Sets AutoPlay to 'Take no action' for all media types by default. "
                    + "The AutoPlay dialog is still shown but no app launches automatically. "
                    + "Value 1 = disabled automatic action selection.",
                ApplyOps = [RegOp.SetDword(AutoPlayUser, "DisableAutoplay", 1)],
                RemoveOps = [RegOp.DeleteValue(AutoPlayUser, "DisableAutoplay")],
                DetectOps = [RegOp.CheckDword(AutoPlayUser, "DisableAutoplay", 1)],
            },
            new TweakDef
            {
                Id = "autorun-disable-autoplay-policy",
                Label = "Disable AutoPlay via System Policy",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["autorun", "autoplay", "policy", "system", "security"],
                Description =
                    "Disables the AutoPlay feature entirely via Group Policy for all drives. "
                    + "The AutoPlay dialog will not appear when media/devices are connected. "
                    + "Recommended for corporate/shared-use environments.",
                ApplyOps = [RegOp.SetDword(AutoRunPolicy2, "NoAutoPlay", 1)],
                RemoveOps = [RegOp.DeleteValue(AutoRunPolicy2, "NoAutoPlay")],
                DetectOps = [RegOp.CheckDword(AutoRunPolicy2, "NoAutoPlay", 1)],
            },
            new TweakDef
            {
                Id = "autorun-disable-network-drive-autoplay",
                Label = "Disable AutoPlay on Network Drives",
                Category = "Display — Auto Play",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["autorun", "network", "share", "autoplay", "security"],
                Description =
                    "Disables AutoPlay when accessing mapped network drives or UNC paths. "
                    + "Prevents attackers from placing malicious AutoRun content on "
                    + "network shares that automatically executes when browsed.",
                ApplyOps = [RegOp.SetDword(AutoRunUser, "NoDriveAutoRun", 0xFF)],
                RemoveOps = [RegOp.DeleteValue(AutoRunUser, "NoDriveAutoRun")],
                DetectOps = [RegOp.CheckDword(AutoRunUser, "NoDriveAutoRun", 0xFF)],
            },
            new TweakDef
            {
                Id = "autorun-disable-mixed-content-autoplay",
                Label = "Disable AutoPlay for Mixed-Content Drives",
                Category = "Display — Auto Play",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["autorun", "mixed content", "autoplay", "media"],
                Description =
                    "Disables AutoPlay for drives containing mixed content (both data files "
                    + "and media). Prevents Windows from auto-opening Explorer or a media "
                    + "player when such drives are inserted.",
                ApplyOps = [RegOp.SetDword(AutoPlayUser, "MixedContentAutoplayType", 0)],
                RemoveOps = [RegOp.DeleteValue(AutoPlayUser, "MixedContentAutoplayType")],
                DetectOps = [RegOp.CheckDword(AutoPlayUser, "MixedContentAutoplayType", 0)],
            },
            new TweakDef
            {
                Id = "autorun-disable-enhanced-autoplay",
                Label = "Disable Enhanced AutoPlay Search",
                Category = "Display — Auto Play",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["autorun", "autoplay", "search", "media"],
                Description =
                    "Disables Windows enhanced AutoPlay which scans removable media for "
                    + "additional content types (photos, music, video) beyond what AutoRun.inf "
                    + "specifies. Reduces disk scanning delay on USB insertion.",
                ApplyOps = [RegOp.SetDword(AutoPlayUser, "UseAutoPlay", 0)],
                RemoveOps = [RegOp.DeleteValue(AutoPlayUser, "UseAutoPlay")],
                DetectOps = [RegOp.CheckDword(AutoPlayUser, "UseAutoPlay", 0)],
            },
        ];
    }

    // ── ColorCalibrationPolicy ──
    private static class _ColorCalibrationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ColorControl";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "colcal-disable-calibration",
                Label = "Disable Display Color Calibration",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableColorCalibration=1 in the ColorControl policy key. Prevents "
                    + "users from running the Windows Display Color Calibration tool "
                    + "(dccw.exe) and applying custom color calibration profiles via the "
                    + "Color Management wizard. Ensures a consistent visual baseline across "
                    + "managed workstations. Default: calibration is available to users. "
                    + "Recommended: 1 on corporate workstations with standardised displays.",
                Tags = ["display", "color", "calibration", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableColorCalibration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableColorCalibration")],
                DetectOps = [RegOp.CheckDword(Key, "DisableColorCalibration", 1)],
            },
            new TweakDef
            {
                Id = "colcal-disable-icm-support",
                Label = "Disable Image Colour Management (ICM)",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets EnableICMSupport=0 in the ColorControl policy key. Disables Image "
                    + "Colour Management within GDI, preventing Windows from applying ICC "
                    + "colour profiles to rendered output. On machines where colour accuracy "
                    + "is irrelevant (kiosks, servers, projectors) this removes unnecessary "
                    + "profile-load overhead at startup. Default: ICM is active when a "
                    + "profile is associated with the display.",
                Tags = ["display", "color", "icm", "profile", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableICMSupport", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableICMSupport")],
                DetectOps = [RegOp.CheckDword(Key, "EnableICMSupport", 0)],
            },
            new TweakDef
            {
                Id = "colcal-hide-color-management-ui",
                Label = "Hide Color Management Control Panel",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets HideColorManagement=1 in the ColorControl policy key. Removes the "
                    + "Color Management applet from the Control Panel and Display Properties "
                    + "dialog so users cannot associate, install, or remove ICC profiles. "
                    + "Ensures that centrally deployed colour profiles cannot be overridden. "
                    + "Default: Color Management is accessible to all users.",
                Tags = ["display", "color", "control-panel", "group-policy", "lockdown"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "HideColorManagement", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "HideColorManagement")],
                DetectOps = [RegOp.CheckDword(Key, "HideColorManagement", 1)],
            },
            new TweakDef
            {
                Id = "colcal-disable-auto-display-calibration",
                Label = "Disable Automatic Display Calibration",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableAutoCalibration=1 in the ColorControl policy key. Prevents "
                    + "Windows from scheduling and running the automatic display calibration "
                    + "cycle that nudges display gamma toward sRGB using hardware VCGT data. "
                    + "On monitors with hardware calibration this is redundant; disabling "
                    + "avoids conflicting with dedicated calibration software. Default: "
                    + "automatic calibration runs on schedule when enabled.",
                Tags = ["display", "color", "calibration", "automation", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoCalibration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoCalibration")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoCalibration", 1)],
            },
            new TweakDef
            {
                Id = "colcal-block-icc-profile-install",
                Label = "Block User ICC Profile Installation",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets PreventUserICCInstall=1 in the ColorControl policy key. Blocks "
                    + "standard users from installing ICC/ICM colour profile files. Only "
                    + "administrators can add new profiles to the system profile store. "
                    + "Prevents inadvertent colour profile swaps that may affect colour-critical "
                    + "workflows or introduce unsigned third-party profiles. Default: any user "
                    + "can install profiles.",
                Tags = ["display", "color", "icc", "security", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventUserICCInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventUserICCInstall")],
                DetectOps = [RegOp.CheckDword(Key, "PreventUserICCInstall", 1)],
            },
            new TweakDef
            {
                Id = "colcal-disable-night-light-gpo",
                Label = "Disable Night Light via Group Policy",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableNightLight=1 in the ColorControl policy key. Prevents users "
                    + "from enabling or scheduling the Night Light (blue-light reduction) "
                    + "feature via Settings. Ensures consistent colour temperature on "
                    + "colour-critical workstations (photo/video editing, medical imaging) "
                    + "where Night Light would distort colour accuracy. Default: Night Light "
                    + "is user-configurable.",
                Tags = ["display", "night-light", "color", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNightLight", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNightLight")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNightLight", 1)],
            },
            new TweakDef
            {
                Id = "colcal-disable-hdr-gpo",
                Label = "Disable HDR Display Support via Group Policy",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableHDRSupport=1 in the ColorControl policy key. Forces Windows "
                    + "to treat all displays as SDR (Standard Dynamic Range), preventing "
                    + "users from enabling HDR in Display Settings. Useful on workstations "
                    + "where HDR causes UI element clipping in SDR applications or "
                    + "introduces colour management inconsistencies. Default: HDR enabled "
                    + "when supported hardware is detected.",
                Tags = ["display", "hdr", "color", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableHDRSupport", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableHDRSupport")],
                DetectOps = [RegOp.CheckDword(Key, "DisableHDRSupport", 1)],
            },
            new TweakDef
            {
                Id = "colcal-disable-wcs-service",
                Label = "Disable Windows Color System Background Service",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets DisableWCSService=1 in the ColorControl policy key. Prevents the "
                    + "Windows Colour System (WCS) background service from loading, which "
                    + "handles baseline display characterisation data and WCS profile "
                    + "mapping. On machines without colour-critical workflows this service "
                    + "consumes memory for no perceptible benefit. Default: WCS service "
                    + "runs when the system boots.",
                Tags = ["display", "color", "wcs", "services", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWCSService", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWCSService")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWCSService", 1)],
            },
            new TweakDef
            {
                Id = "colcal-disable-color-rendering-intent",
                Label = "Lock Colour Rendering Intent to Absolute Colorimetric",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets ForceAbsoluteColorimetric=1 in the ColorControl policy key. Overrides "
                    + "per-application colour rendering intent settings and forces all GDI "
                    + "colour-managed output to use Absolute Colorimetric intent. Eliminates "
                    + "gamma and white-point remapping that can introduce colour shifts on "
                    + "wide-gamut P3 or AdobeRGB displays. Default: intent is set per profile "
                    + "and application. Caution: may affect soft-proofing workflows.",
                Tags = ["display", "color", "rendering", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ForceAbsoluteColorimetric", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ForceAbsoluteColorimetric")],
                DetectOps = [RegOp.CheckDword(Key, "ForceAbsoluteColorimetric", 1)],
            },
            new TweakDef
            {
                Id = "colcal-disable-auto-color-correction",
                Label = "Disable Auto Color Correction",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableAutoColorCorrection=1 in the ColorControl policy key. Stops "
                    + "Windows from applying the automatic colour correction layer that "
                    + "adjusts output colour based on ambient light sensor data or display "
                    + "warm-up compensation. Prevents unexpected colour shifts during the "
                    + "work session. Default: auto correction is applied when supported by "
                    + "display hardware. Recommended: 1 on precision colour workstations.",
                Tags = ["display", "color", "correction", "ambient", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoColorCorrection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoColorCorrection")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoColorCorrection", 1)],
            },
        ];
    }

    // ── ColorManagement ──
    private static class _ColorManagement
    {
        private const string IcmUser = @"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\ICMRegData";

        private const string IcmSys = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\ICM";

        private const string DwmCompose = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM";

        private const string DisplayGamma = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\ICM";

        private const string HdrProfile = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Configuration";

        private const string ColorPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Display";

        private const string VideoSettings = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "color-disable-hdr-battery-impact",
                Label = "Disable HDR Streaming Battery Compensation",
                Category = "Display — Auto Play",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["color", "hdr", "battery", "display"],
                Description =
                    "Disables the automatic brightness compensation that Windows applies "
                    + "when HDR is active on battery power. Maintains consistent HDR brightness "
                    + "regardless of power source.",
                ApplyOps = [RegOp.SetDword(VideoSettings, "HDRBatteryOptimization", 0)],
                RemoveOps = [RegOp.DeleteValue(VideoSettings, "HDRBatteryOptimization")],
                DetectOps = [RegOp.CheckDword(VideoSettings, "HDRBatteryOptimization", 0)],
            },
            new TweakDef
            {
                Id = "color-set-sdr-brightness-hdr-mode",
                Label = "Increase SDR Content Brightness in HDR Mode (80 nits)",
                Category = "Display — Auto Play",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["color", "hdr", "sdr", "brightness", "display"],
                Description =
                    "Sets the SDR content brightness compensation to 80 nits when HDR mode "
                    + "is active. Prevents SDR apps and desktop from appearing washed out next "
                    + "to HDR content. Range: 0–100.",
                ApplyOps = [RegOp.SetDword(VideoSettings, "SDRBrightness", 80)],
                RemoveOps = [RegOp.DeleteValue(VideoSettings, "SDRBrightness")],
                DetectOps = [RegOp.CheckDword(VideoSettings, "SDRBrightness", 80)],
            },
            new TweakDef
            {
                Id = "color-disable-auto-color-management",
                Label = "Disable Automatic Display Color Management",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["color", "icc", "profile", "display", "management"],
                Description =
                    "Disables Windows automatic color management that adjusts output based "
                    + "on ICC profiles. Useful for content creators who manage color profiles "
                    + "manually through a dedicated color calibration tool.",
                ApplyOps = [RegOp.SetDword(ColorPolicy, "DisableAutoColorManagement", 1)],
                RemoveOps = [RegOp.DeleteValue(ColorPolicy, "DisableAutoColorManagement")],
                DetectOps = [RegOp.CheckDword(ColorPolicy, "DisableAutoColorManagement", 1)],
            },
            new TweakDef
            {
                Id = "color-enable-hdr-wcg-apps",
                Label = "Enable HDR and WCG for Apps",
                Category = "Display — Auto Play",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["color", "hdr", "wcg", "display", "wide color gamut"],
                Description =
                    "Enables Wide Color Gamut (WCG) and HDR support for Windows apps on "
                    + "compatible displays. Allows HDR-aware applications to display full "
                    + "HDR content through the DirectX swap chain.",
                ApplyOps = [RegOp.SetDword(VideoSettings, "EnableHDRForApps", 1)],
                RemoveOps = [RegOp.DeleteValue(VideoSettings, "EnableHDRForApps")],
                DetectOps = [RegOp.CheckDword(VideoSettings, "EnableHDRForApps", 1)],
            },
            new TweakDef
            {
                Id = "color-disable-dwm-color-depth",
                Label = "Disable DWM 10-bit Color Override",
                Category = "Display — Auto Play",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["color", "dwm", "depth", "display"],
                Description =
                    "Disables the Desktop Window Manager (DWM) 10-bit color processing "
                    + "override. Reverts to 8-bit color depth through DWM composition. "
                    + "Can fix color banding or rendering artifacts on some display drivers.",
                ApplyOps = [RegOp.SetDword(DwmCompose, "Use10BitColorDepth", 0)],
                RemoveOps = [RegOp.DeleteValue(DwmCompose, "Use10BitColorDepth")],
                DetectOps = [RegOp.CheckDword(DwmCompose, "Use10BitColorDepth", 0)],
            },
            new TweakDef
            {
                Id = "color-disable-night-light-gamma",
                Label = "Disable Night Light Gamma Ramp Interference",
                Category = "Display — Auto Play",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["color", "night light", "gamma", "display", "calibration"],
                Description =
                    "Disables the Night Light gamma ramp that can interfere with ICC color "
                    + "profile calibration. Set to 0 to prevent Night Light from overriding "
                    + "your display's calibrated color temperature.",
                ApplyOps = [RegOp.SetDword(DwmCompose, "ColorizationOpaqueBlend", 0)],
                RemoveOps = [RegOp.DeleteValue(DwmCompose, "ColorizationOpaqueBlend")],
                DetectOps = [RegOp.CheckDword(DwmCompose, "ColorizationOpaqueBlend", 0)],
            },
            new TweakDef
            {
                Id = "color-enable-calibration-system",
                Label = "Enable ICM Gamma Calibration System-Wide",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["color", "icc", "icm", "calibration", "gamma"],
                Description =
                    "Enables the Windows ICM gamma calibration system for all displays "
                    + "at the session level. Required for ICC profiles that include gamma "
                    + "correction to be applied correctly.",
                ApplyOps = [RegOp.SetDword(IcmSys, "GammaCal", 1)],
                RemoveOps = [RegOp.DeleteValue(IcmSys, "GammaCal")],
                DetectOps = [RegOp.CheckDword(IcmSys, "GammaCal", 1)],
            },
            new TweakDef
            {
                Id = "color-disable-hdr-auto-adjust",
                Label = "Disable HDR Automatic Tone Mapping",
                Category = "Display — Auto Play",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["color", "hdr", "tone mapping", "display", "auto"],
                Description =
                    "Disables Windows automatic HDR tone mapping that adjusts content "
                    + "brightness dynamically. Useful for video editors who need precise "
                    + "HDR level control without system interference.",
                ApplyOps = [RegOp.SetDword(VideoSettings, "HDRToneMapEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(VideoSettings, "HDRToneMapEnabled")],
                DetectOps = [RegOp.CheckDword(VideoSettings, "HDRToneMapEnabled", 0)],
            },
            new TweakDef
            {
                Id = "color-force-full-color-range",
                Label = "Force Full Color Range RGB (0-255)",
                Category = "Display — Auto Play",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["color", "rgb", "range", "full", "display"],
                Description =
                    "Forces the display output to use Full RGB color range (0–255) rather "
                    + "than Limited range (16–235). Eliminates washed-out colors and improves "
                    + "contrast on monitors that support full range input.",
                ApplyOps = [RegOp.SetDword(VideoSettings, "ForceFullColorRange", 1)],
                RemoveOps = [RegOp.DeleteValue(VideoSettings, "ForceFullColorRange")],
                DetectOps = [RegOp.CheckDword(VideoSettings, "ForceFullColorRange", 1)],
            },
        ];
    }

    // ── CompartmentPolicy ──
    private static class _CompartmentPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Compartment";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "compart-enable-network-compartmentalization",
                Label = "Enable Network Isolation for Application Groups",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Network isolation through compartmentalization controls which applications and process groups can communicate with which network destinations. Enabling network compartmentalization creates enforced boundaries between application groups preventing unrestricted network access. Applications confined to specific network compartments cannot initiate connections to hosts outside their designated network zone reducing lateral movement risk. Network compartmentalization is particularly effective for containing the damage from compromised applications by restricting their outbound connectivity. Containment policy should define network zones that align with the application's legitimate communication requirements and business function. Organizations implementing zero-trust network architecture can use compartmentalization to enforce application-level network segmentation beyond traditional VLAN-based controls.",
                Tags = ["compartment", "network-isolation", "lateral-movement", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableNetworkIsolation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableNetworkIsolation")],
                DetectOps = [RegOp.CheckDword(Key, "EnableNetworkIsolation", 1)],
            },
            new TweakDef
            {
                Id = "compart-restrict-localhost-loopback",
                Label = "Restrict Localhost Loopback Access Between Compartments",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Localhost loopback communication between compartments can be used to bypass network isolation boundaries by routing traffic through the local system. Restricting loopback access between compartments ensures that network isolation controls apply to both external and local communications. Applications in separate security compartments should not be able to communicate through local IPC or loopback unless explicitly permitted by policy. Unrestricted loopback access undermines network compartmentalization by providing a side channel for inter-compartment communication. Loopback restriction enforcement requires applications to use explicit inter-process communication mechanisms that can be monitored and controlled. Organizations should map legitimate loopback dependencies between application groups before enforcing loopback restrictions to prevent breaking application functionality.",
                Tags = ["compartment", "loopback", "network-isolation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictLoopbackAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictLoopbackAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictLoopbackAccess", 1)],
            },
            new TweakDef
            {
                Id = "compart-enable-process-isolation",
                Label = "Enable Process Memory Isolation Between Windows Compartments",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Process memory isolation between compartments prevents processes in one compartment from accessing or manipulating the memory of processes in other compartments. Enabling process isolation ensures that a compromised process in one security compartment cannot directly exploit processes in adjacent compartments. Memory isolation is a fundamental defense against process injection attacks that are commonly used for credential theft and privilege escalation. Strong isolation boundaries between compartments reduce the blast radius of a single process compromise to only the resources accessible from that compartment. Compartment process isolation complements HVCI for kernel code but addresses user-mode process interaction which HVCI does not directly control. Organizations deploying containerized applications can use compartment isolation to provide security boundaries similar to OS-level containers.",
                Tags = ["compartment", "process-isolation", "memory", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableProcessIsolation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableProcessIsolation")],
                DetectOps = [RegOp.CheckDword(Key, "EnableProcessIsolation", 1)],
            },
            new TweakDef
            {
                Id = "compart-restrict-file-system-access",
                Label = "Restrict File System Access Based on Compartment Membership",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "File system access restrictions based on compartment membership limit which files and directories each application group can read or write. Restricting file system access by compartment prevents a compromised application from reading sensitive files belonging to other application security zones. Data segregation through file system compartmentalization supports data classification requirements by ensuring applications only access data appropriate to their security classification. Compartment-based file restrictions supplement traditional NTFS ACLs by providing dynamic access control that follows application security group membership. The combination of NTFS permissions and compartment file restrictions creates multiple layers of protection against unauthorized data access. File system compartmentalization should be mapped carefully against application data access requirements to prevent operational disruption.",
                Tags = ["compartment", "file-access", "data-segregation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictFileSystemAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictFileSystemAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictFileSystemAccess", 1)],
            },
            new TweakDef
            {
                Id = "compart-enforce-object-permissions",
                Label = "Enforce Object Access Permissions for Compartment Boundaries",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Compartment object permission enforcement ensures that Windows objects including mutexes, semaphores, and named pipes respect compartment membership for access decisions. Enforcing object access permissions by compartment prevents cross-compartment object attacks where a malicious process creates objects that privileged processes will access. Named pipe impersonation attacks are prevented when pipes are restricted to be accessible only within the same compartment. Shared memory objects between application groups represent a common attack vector that compartment object restrictions can control. Object permission enforcement for compartments integrates with the Windows object manager to apply compartment membership checks transparently. Applications that use shared objects for inter-process communication across compartment boundaries require explicit policy exceptions.",
                Tags = ["compartment", "object-permissions", "ipc", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceObjectPermissions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceObjectPermissions")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceObjectPermissions", 1)],
            },
            new TweakDef
            {
                Id = "compart-audit-boundary-violations",
                Label = "Enable Audit Logging for Compartment Boundary Violations",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Compartment boundary violation auditing generates events when processes attempt to access resources outside their assigned compartment. Enabling boundary violation auditing provides visibility into attempted cross-compartment access that may indicate malicious activity or application misconfiguration. Violation events help identify applications that have dependencies outside their defined compartment requiring policy adjustments before enforcement. Security teams can use violation data to map legitimate cross-compartment communications and build accurate compartment policy. Repeated violation attempts from the same process targeting sensitive compartment resources indicate potential exploitation activity. Audit mode should be enabled before enforcement mode to allow analysis of violation patterns without disrupting legitimate operations.",
                Tags = ["compartment", "audit", "violation-detection", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditBoundaryViolations", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditBoundaryViolations")],
                DetectOps = [RegOp.CheckDword(Key, "AuditBoundaryViolations", 1)],
            },
            new TweakDef
            {
                Id = "compart-restrict-registry-access",
                Label = "Restrict Registry Access Based on Application Compartment",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Registry access restrictions by compartment limit which registry hives and keys can be read or modified by applications in each security group. Restricting registry access by compartment prevents compromised applications from reading sensitive configuration including credentials stored in the registry. Applications should only have access to the registry keys required for their legitimate function reducing access to configuration that could be modified for privilege escalation. Compartment-based registry restrictions supplement standard registry ACLs to provide dynamic access control that follows application group membership. Registry access restrictions prevent reconnaissance activities where malware scanned the registry to discover installed software and configuration details. Organizations should map application registry dependencies before enabling compartment-based restrictions to prevent disrupting application functionality.",
                Tags = ["compartment", "registry", "access-control", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictRegistryAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictRegistryAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictRegistryAccess", 1)],
            },
            new TweakDef
            {
                Id = "compart-disable-cross-compartment-clipboard",
                Label = "Disable Clipboard Sharing Across Security Compartments",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Clipboard sharing across compartments allows data to be transferred between security zones through the Windows clipboard bypassing network isolation controls. Disabling cross-compartment clipboard sharing prevents data from being copied from high-security compartments to lower-security application groups. Clipboard data contains user content including credentials, documents, and configuration that should respect compartment data classification when being transferred. Controlled clipboard sharing through approved transfer mechanisms is preferable to unrestricted clipboard access across compartment boundaries. Users who need to transfer data between compartments should use approved channels that can monitor and log the data exchange. Cross-compartment clipboard restrictions are particularly relevant in environments with classified data handling requirements.",
                Tags = ["compartment", "clipboard", "data-transfer", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCrossCompartmentClipboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCrossCompartmentClipboard")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCrossCompartmentClipboard", 1)],
            },
            new TweakDef
            {
                Id = "compart-enable-mandatory-integrity",
                Label = "Enforce Mandatory Integrity Control for Compartment Processes",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Mandatory Integrity Control (MIC) assigns integrity levels to processes and objects controlling which processes can modify resources based on their integrity level. Enabling MIC enforcement for compartment processes prevents lower-integrity processes from writing to higher-integrity compartment objects. Internet-facing applications running at low integrity cannot modify files and registry keys owned by higher-integrity compartment processes even with same-user permissions. MIC enforcement is the mechanism behind Protected Mode in Internet Explorer and similar sandboxing techniques for web-facing applications. Compartment MIC enforcement creates an additional layer of protection that supplements discretionary access controls with mandatory controls. Applications in high-trust compartments should run at higher integrity levels with internet-exposed components running at lower integrity.",
                Tags = ["compartment", "mandatory-integrity", "sandboxing", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceMandatoryIntegrity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceMandatoryIntegrity")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceMandatoryIntegrity", 1)],
            },
            new TweakDef
            {
                Id = "compart-restrict-interprocess-communication",
                Label = "Restrict IPC Mechanisms Across Compartment Boundaries",
                Category = "Display — Auto Play",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Inter-process communication across compartment boundaries allows application groups to exchange data through mechanisms like pipes, sockets, and mailslots bypassing file-level separation. Restricting IPC across compartments ensures that only approved communication channels with appropriate data inspection can bridge compartment boundaries. Uncontrolled IPC across compartment boundaries provides a covert channel for data exfiltration from restricted to unrestricted application zones. IPC restriction policies should define explicit allowed communication paths with appropriate authentication and data validation for cross-compartment channels. Named pipes and sockets that cross compartment boundaries should require authentication from both communicating parties to prevent impersonation. Organizations should use application firewalls or approved message brokers for cross-compartment communication rather than direct IPC mechanisms.",
                Tags = ["compartment", "ipc", "inter-process", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictCrossCompartmentIPC", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictCrossCompartmentIPC")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictCrossCompartmentIPC", 1)],
            },
        ];
    }

    // ── ControlPanelPolicy ──
    private static class _ControlPanelPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ControlPanel";
        private const string ExplorerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "ctrlpanel-disable-all-control-panel",
                Label = "Control Panel Policy: Disable Access to All Control Panel Items",
                Category = "Display — Auto Play",
                Description =
                    "Prevents all users from opening the Control Panel and PC Settings app. Control Panel contains sensitive system configuration dialogs including network settings, user accounts, and security options. On locked-down workstations this policy prevents unauthorised system reconfiguration.",
                Tags = ["control panel", "restriction", "access", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ExplorerKey],
                ApplyOps = [RegOp.SetDword(ExplorerKey, "NoControlPanel", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplorerKey, "NoControlPanel")],
                DetectOps = [RegOp.CheckDword(ExplorerKey, "NoControlPanel", 1)],
                ImpactScore = 4,
                SafetyRating = 2,
                ImpactNote = "Blocks all Control Panel access; users cannot change any system settings.",
            },
            new TweakDef
            {
                Id = "ctrlpanel-hide-personalization-settings",
                Label = "Control Panel Policy: Hide Personalization from Control Panel",
                Category = "Display — Auto Play",
                Description =
                    "Removes the Personalization item from the Control Panel and hides the right-click desktop personalization context menu entry. On shared/managed workstations, personalisation access can be used to change the desktop background (watermark bypass), install screen savers with custom payloads, or modify display scaling.",
                Tags = ["control panel", "personalization", "theme", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ExplorerKey],
                ApplyOps = [RegOp.SetDword(ExplorerKey, "NoThemesTab", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplorerKey, "NoThemesTab")],
                DetectOps = [RegOp.CheckDword(ExplorerKey, "NoThemesTab", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides Themes/Personalization from Control Panel; desktop right-click Personalize is also hidden.",
            },
            new TweakDef
            {
                Id = "ctrlpanel-hide-user-accounts",
                Label = "Control Panel Policy: Hide User Accounts from Control Panel",
                Category = "Display — Auto Play",
                Description =
                    "Hides the User Accounts section from the Control Panel. Standard users should not be able to navigate the user accounts management interface where they might attempt password changes, account type modifications, or credential manager access.",
                Tags = ["control panel", "user accounts", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ExplorerKey],
                ApplyOps = [RegOp.SetDword(ExplorerKey, "NoUserNameInStartMenu", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplorerKey, "NoUserNameInStartMenu")],
                DetectOps = [RegOp.CheckDword(ExplorerKey, "NoUserNameInStartMenu", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides username in Start menu tile; does not prevent access via Run > control userpasswords2.",
            },
            new TweakDef
            {
                Id = "ctrlpanel-hide-add-remove-programs",
                Label = "Control Panel Policy: Restrict Add/Remove Programs in Control Panel",
                Category = "Display — Auto Play",
                Description =
                    "Hides the Programs and Features (Add/Remove Programs) applet from the Control Panel. This prevents standard users from uninstalling installed software, adding Windows Features, or modifying installed programs. Useful on kiosk and managed endpoints where software management is IT-controlled.",
                Tags = ["control panel", "add remove programs", "uninstall", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ExplorerKey],
                ApplyOps = [RegOp.SetDword(ExplorerKey, "NoAddRemovePrograms", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplorerKey, "NoAddRemovePrograms")],
                DetectOps = [RegOp.CheckDword(ExplorerKey, "NoAddRemovePrograms", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks Programs and Features applet; users cannot uninstall or modify software.",
            },
            new TweakDef
            {
                Id = "ctrlpanel-disable-change-password",
                Label = "Control Panel Policy: Prevent Users from Changing Their Password",
                Category = "Display — Auto Play",
                Description =
                    "Prevents standard users from navigating to Control Panel > User Accounts > Change Password. In environments where passwords are centrally managed via Active Directory or MDM policy, allowing local password changes creates synchronisation conflicts and bypasses the centralised credential management flow.",
                Tags = ["control panel", "password", "change", "user accounts", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ExplorerKey],
                ApplyOps = [RegOp.SetDword(ExplorerKey, "DisallowCpl", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplorerKey, "DisallowCpl")],
                DetectOps = [RegOp.CheckDword(ExplorerKey, "DisallowCpl", 1)],
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Blocks all Control Panel CPL applets; use NoControlPanelShowOnly to allow specific applets.",
            },
            new TweakDef
            {
                Id = "ctrlpanel-restrict-network-connections",
                Label = "Control Panel Policy: Restrict Network Connections from Control Panel",
                Category = "Display — Auto Play",
                Description =
                    "Prevents standard users from accessing Network Connections in the Control Panel. Network Connections allows users to add/remove VPN entries, configure DNS, change adapter settings, and create ad-hoc networks — all actions that can bypass corporate network access controls.",
                Tags = ["control panel", "network", "connections", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Network Connections"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Network Connections", "NC_AddRemoveComponents", 0),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Network Connections", "NC_AddRemoveComponents"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Network Connections", "NC_AddRemoveComponents", 0),
                ],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents adding/removing network components; users cannot install new network protocols.",
            },
            new TweakDef
            {
                Id = "ctrlpanel-hide-system-properties",
                Label = "Control Panel Policy: Hide System Properties from Control Panel",
                Category = "Display — Auto Play",
                Description =
                    "Prevents access to the System Properties applet (sysdm.cpl) via Control Panel. System Properties exposes settings for computer name changes, domain joins, hardware device manager, remote desktop access, and environment variables — all sensitive configuration surfaces on managed workstations.",
                Tags = ["control panel", "system", "properties", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ExplorerKey],
                ApplyOps = [RegOp.SetDword(ExplorerKey, "NoPropertiesMyComputer", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplorerKey, "NoPropertiesMyComputer")],
                DetectOps = [RegOp.CheckDword(ExplorerKey, "NoPropertiesMyComputer", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Hides System Properties; right-click This PC > Properties and sysdm.cpl will not open.",
            },
            new TweakDef
            {
                Id = "ctrlpanel-disable-power-options",
                Label = "Control Panel Policy: Prevent Access to Power Options in Control Panel",
                Category = "Display — Auto Play",
                Description =
                    "Blocks user access to Power Options in the Control Panel. Users with access to Power Options can override sleep/hibernate settings, disable fast startup, create custom power plans, and modify sleep-on-lock behaviour — changes that may conflict with enterprise energy compliance or security lockout policies.",
                Tags = ["control panel", "power", "sleep", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "NoPowerOptionsPage", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "NoPowerOptionsPage")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "NoPowerOptionsPage", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides Power Options from Control Panel; users cannot change power plan or sleep settings.",
            },
            new TweakDef
            {
                Id = "ctrlpanel-hide-windows-update-settings",
                Label = "Control Panel Policy: Hide Windows Update from Control Panel",
                Category = "Display — Auto Play",
                Description =
                    "Hides the Windows Update item in the Control Panel. On managed devices where Windows Update is controlled by WSUS, Intune, or Configuration Manager, exposing the Windows Update control panel section allows users to manually trigger updates, pause updates, or change the update server — potentially disrupting managed patching schedules.",
                Tags = ["control panel", "windows update", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ExplorerKey],
                ApplyOps = [RegOp.SetDword(ExplorerKey, "HideWindowsUpdateDialog", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplorerKey, "HideWindowsUpdateDialog")],
                DetectOps = [RegOp.CheckDword(ExplorerKey, "HideWindowsUpdateDialog", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides Windows Update from Control panel on WSUS/Intune-managed devices.",
            },
        ];
    }

    // ── DirectXRenderingPolicy ──
    private static class _DirectXRenderingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Direct3D";
        private const string DxKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DirectX";
        private const string DgiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DXGI";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "d3dpol-disable-d3d-debug-layer",
                    Label = "Disable Direct3D Debug Layer in Production",
                    Category = "Display — Auto Play",
                    Description =
                        "Disables the Direct3D Debug Layer (D3D11_CREATE_DEVICE_DEBUG / DX12 debug flag) in production environments, preventing verbose GPU validation from activating when debug runtimes are installed on production machines.",
                    Tags = ["direct3d", "debug-layer", "gpu", "production", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "D3D debug layer disabled; no GPU validation overhead even if debug SDK is installed on the machine.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDebugLayer", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDebugLayer")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDebugLayer", 1)],
                },
                new TweakDef
                {
                    Id = "d3dpol-disable-d3d-warp-fallback",
                    Label = "Disable Direct3D WARP Software Renderer Fallback",
                    Category = "Display — Auto Play",
                    Description =
                        "Prevents applications from using the WARP (Windows Advanced Rasterisation Platform) CPU-based software renderer as a fallback when hardware Direct3D is unavailable, ensuring all D3D rendering uses physical GPU hardware.",
                    Tags = ["direct3d", "warp", "software-renderer", "gpu", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "WARP software renderer blocked; D3D apps fail without GPU rather than running at 1/100 speed on CPU.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWARPFallback", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWARPFallback")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWARPFallback", 1)],
                },
                new TweakDef
                {
                    Id = "d3dpol-set-feature-level-minimum-11",
                    Label = "Require Minimum Direct3D Feature Level 11.0",
                    Category = "Display — Auto Play",
                    Description =
                        "Sets a minimum required Direct3D feature level of 11.0, preventing applications from requesting feature levels below D3D11 and ensuring all GPU workloads use modern shader models and resource bindings.",
                    Tags = ["direct3d", "feature-level", "d3d11", "gpu", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "D3D minimum feature level set to 11.0; apps cannot fall back to D3D9/D3D10 mode.",
                    ApplyOps = [RegOp.SetDword(Key, "MinimumFeatureLevel", 0xB000)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MinimumFeatureLevel")],
                    DetectOps = [RegOp.CheckDword(Key, "MinimumFeatureLevel", 0xB000)],
                },
                new TweakDef
                {
                    Id = "d3dpol-disable-dxgi-fullscreen-opt",
                    Label = "Disable DXGI Fullscreen Optimisations App-Wide",
                    Category = "Display — Auto Play",
                    Description =
                        "Disables DXGI Fullscreen Optimisations at system policy level, preventing Windows from overriding fullscreen exclusive mode with a windowed swap chain, which can cause frame timing inconsistencies in precision rendering.",
                    Tags = ["dxgi", "fullscreen-optimisations", "exclusive-mode", "direct3d", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "DXGI fullscreen optimisations disabled; true exclusive fullscreen used for all DXGI apps system-wide.",
                    ApplyOps = [RegOp.SetDword(DgiKey, "DisableFullscreenOptimizations", 1)],
                    RemoveOps = [RegOp.DeleteValue(DgiKey, "DisableFullscreenOptimizations")],
                    DetectOps = [RegOp.CheckDword(DgiKey, "DisableFullscreenOptimizations", 1)],
                },
                new TweakDef
                {
                    Id = "d3dpol-enable-auto-hdr",
                    Label = "Enable Direct3D Auto HDR for SDR Application Upscaling",
                    Category = "Display — Auto Play",
                    Description =
                        "Enables Windows Auto HDR which algorithmically expands the luminance range of SDR Direct3D 11 and 12 applications for HDR display output, improving visual quality of DX applications without source code changes.",
                    Tags = ["direct3d", "auto-hdr", "hdr", "display", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Auto HDR enabled; D3D SDR apps upscaled to HDR range on compatible HDR monitors.",
                    ApplyOps = [RegOp.SetDword(Key, "AutoHDREnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AutoHDREnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "AutoHDREnabled", 1)],
                },
                new TweakDef
                {
                    Id = "d3dpol-disable-d3d-telemetry",
                    Label = "Disable Direct3D Telemetry Reporting to Microsoft",
                    Category = "Display — Auto Play",
                    Description =
                        "Prevents the Direct3D runtime from sending application GPU usage, feature level, and performance telemetry to Microsoft, protecting information about GPU workload characteristics from cloud disclosure.",
                    Tags = ["direct3d", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "D3D telemetry to Microsoft disabled; GPU app usage and feature level stats not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableD3DTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableD3DTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableD3DTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "d3dpol-enable-dx12-ultimate",
                    Label = "Enable DirectX 12 Ultimate Feature Set Policy",
                    Category = "Display — Auto Play",
                    Description =
                        "Configures the system to prefer the DirectX 12 Ultimate feature set (Shader Model 6.6, Mesh Shaders, Sampler Feedback, DirectX Raytracing 1.1) when available, enabling applications to use the highest GPU capability tier.",
                    Tags = ["direct3d", "dx12-ultimate", "raytracing", "mesh-shader", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "DX12 Ultimate feature set preferred; apps can advertise SM6.6 / RT 1.1 / Mesh Shaders on compatible GPUs.",
                    ApplyOps = [RegOp.SetDword(Key, "PreferDX12Ultimate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreferDX12Ultimate")],
                    DetectOps = [RegOp.CheckDword(Key, "PreferDX12Ultimate", 1)],
                },
                new TweakDef
                {
                    Id = "d3dpol-restrict-gpu-access-sandboxed",
                    Label = "Restrict Direct3D GPU Access in Sandboxed AppContainer Processes",
                    Category = "Display — Auto Play",
                    Description =
                        "Configures reduced-privilege Direct3D access for AppContainer (UWP sandbox) processes, preventing sandboxed applications from accessing full GPU command queue capabilities that could be used for side-channel attacks.",
                    Tags = ["direct3d", "appcontainer", "sandbox", "gpu-access", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Sandboxed GPU access restricted; AppContainer apps have limited GPU command queue capabilities.",
                    ApplyOps = [RegOp.SetDword(Key, "RestrictGPUAccessInSandbox", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RestrictGPUAccessInSandbox")],
                    DetectOps = [RegOp.CheckDword(Key, "RestrictGPUAccessInSandbox", 1)],
                },
                new TweakDef
                {
                    Id = "d3dpol-log-d3d-device-removed",
                    Label = "Log Direct3D Device Removed Events for Diagnostics",
                    Category = "Display — Auto Play",
                    Description =
                        "Enables Application event log entries for DXGI_ERROR_DEVICE_REMOVED and DXGI_ERROR_DEVICE_HUNG events generated by Direct3D, providing diagnostic information about GPU hardware failures, driver crashes, and TDR events.",
                    Tags = ["direct3d", "device-removed", "event-log", "diagnostics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "D3D device removed events logged; GPU failure reasons visible in Application event log for diagnostics.",
                    ApplyOps = [RegOp.SetDword(Key, "LogDeviceRemovedEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogDeviceRemovedEvents")],
                    DetectOps = [RegOp.CheckDword(Key, "LogDeviceRemovedEvents", 1)],
                },
                new TweakDef
                {
                    Id = "d3dpol-disable-overlay-planes",
                    Label = "Disable DirectX Hardware Overlay Planes",
                    Category = "Display — Auto Play",
                    Description =
                        "Disables DXGI hardware overlay planes that allow applications to render directly into GPU overlay surfaces, preventing overlay plane usage that bypasses DWM compositing and can lead to display corruption on multi-monitor setups.",
                    Tags = ["direct3d", "dxgi", "overlay-planes", "display", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Hardware overlay planes disabled; all rendering goes through DWM compositor. Reduces display corruption risk.",
                    ApplyOps = [RegOp.SetDword(DgiKey, "DisableHWOverlayPlanes", 1)],
                    RemoveOps = [RegOp.DeleteValue(DgiKey, "DisableHWOverlayPlanes")],
                    DetectOps = [RegOp.CheckDword(DgiKey, "DisableHWOverlayPlanes", 1)],
                },
            ];
    }

    // ── DirectXShaderCachePolicy ──
    private static class _DirectXShaderCachePolicy
    {
        private const string DisplayKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Display";
        private const string GfxKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "dxshdr-enable-d3d12-shader-cache",
                    Label = "DirectX: Enable D3D12 Shader Cache for Faster Game Load Times",
                    Category = "Display — Auto Play",
                    Description =
                        "Sets D3D12AllowSoftwareFallback=0 and relies on the D3D12 shader cache (DXGI shader disk cache) enabled by default. Sets DisableD3D12ShaderCache=0 in Display policy to explicitly keep shader caching enabled. "
                        + "D3D12 shader compilation caching stores pre-compiled GPU programs to disk so that subsequent runs of the same game or application do not need to recompile shaders from scratch. Without the cache, every game launch triggers fresh GPU shader compilation — causing stuttering and load times that can exceed 5 minutes in large open-world titles. Keeping this enabled is important on imaging/VDI scenarios where the cache may be inadvertently cleared.",
                    Tags = ["dx12", "shader-cache", "load-time", "compilation", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Shader cache ON; eliminates per-session recompilation stutters and long load times.",
                    ApplyOps = [RegOp.SetDword(DisplayKey, "DisableD3D12ShaderCache", 0)],
                    RemoveOps = [RegOp.DeleteValue(DisplayKey, "DisableD3D12ShaderCache")],
                    DetectOps = [RegOp.CheckDword(DisplayKey, "DisableD3D12ShaderCache", 0)],
                },
                new TweakDef
                {
                    Id = "dxshdr-disable-dxgi-information-queue",
                    Label = "DirectX: Disable DXGI Debug Information Queue Logging",
                    Category = "Display — Auto Play",
                    Description =
                        "Sets DisableDXGIInfoQueue=1 in GraphicsDrivers registry. Disables the DXGI debug information queue, which logs verbose DXGI API validation messages to the debug output stream in debug builds. "
                        + "The DXGI information queue is a developer debugging tool that has no benefit in production builds. Disabling it eliminates the per-frame memory allocation overhead of maintaining the queue ring buffer, which can cause visible micro-stutters when the queue fills and wraps around — particularly noticeable in frame-time sensitive applications on lower-end hardware.",
                    Tags = ["dx12", "dxgi", "debug", "performance", "micro-stutter"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Disables DXGI debug queue; eliminates ring-buffer overhead micro-stutters in release builds.",
                    ApplyOps = [RegOp.SetDword(GfxKey, "DisableDXGIInfoQueue", 1)],
                    RemoveOps = [RegOp.DeleteValue(GfxKey, "DisableDXGIInfoQueue")],
                    DetectOps = [RegOp.CheckDword(GfxKey, "DisableDXGIInfoQueue", 1)],
                },
                new TweakDef
                {
                    Id = "dxshdr-enable-flipex-presentation-model",
                    Label = "DirectX: Enable DXGI FlipEx Swap Chain for Reduced Presentation Latency",
                    Category = "Display — Auto Play",
                    Description =
                        "Sets ForceFlipEx=1 in GraphicsDrivers registry. Instructs the DXGI flip model to use the FlipEx presentation path (Flip Discard with direct scanout) when available, bypassing the desktop window manager (DWM) composition pass. "
                        + "FlipEx allows full-screen exclusive applications to directly control the scanout buffer without the frame going through DWM composition. This eliminates one full frame of latency compared to the DWM composition path (Blit model), reducing end-to-end input-to-photon latency by ~8–16 ms on a 60 Hz display. This is the primary latency optimisation for competitive games.",
                    Tags = ["gpu", "flipex", "presentation", "latency", "frame-time"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "FlipEx direct scanout: ~8–16ms lower presentation latency vs DWM composition; requires full-screen exclusive mode.",
                    ApplyOps = [RegOp.SetDword(GfxKey, "ForceFlipEx", 1)],
                    RemoveOps = [RegOp.DeleteValue(GfxKey, "ForceFlipEx")],
                    DetectOps = [RegOp.CheckDword(GfxKey, "ForceFlipEx", 1)],
                },
                new TweakDef
                {
                    Id = "dxshdr-disable-display-power-saving-technology",
                    Label = "DirectX: Disable Display Power-Saving Technology for Accurate Color",
                    Category = "Display — Auto Play",
                    Description =
                        "Sets DisableDisplayPowerSaving=1 in GraphicsDrivers registry. Disables vendor-specific display power-saving technologies (Intel DPST, AMD VDDG, NVIDIA SmartGPU) that dynamically adjust backlight and GPU power based on displayed content brightness. "
                        + "Display power-saving technologies alter the luminance and colour rendering of the GPU scanout in real time. This makes accurate colour reproduction impossible for photo editing, video colour grading, and design work. Disabling it restores consistent, unmodified colour output from the GPU — essential for any colour-managed workflow.",
                    Tags = ["gpu", "display", "color-accuracy", "backlight", "creative"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Disables adaptive backlight/colour adjustment; accurate colour for design/photo workflows at cost of minor battery life.",
                    ApplyOps = [RegOp.SetDword(GfxKey, "DisableDisplayPowerSaving", 1)],
                    RemoveOps = [RegOp.DeleteValue(GfxKey, "DisableDisplayPowerSaving")],
                    DetectOps = [RegOp.CheckDword(GfxKey, "DisableDisplayPowerSaving", 1)],
                },
                new TweakDef
                {
                    Id = "dxshdr-disable-directx-diagnostic-reporting",
                    Label = "DirectX: Disable DirectX Diagnostic Reporting to Microsoft",
                    Category = "Display — Auto Play",
                    Description =
                        "Sets DisableDiagnosticReporting=1 in Display policy. Prevents the DirectX diagnostics subsystem from sending GPU compatibility reports, DirectX error events, and driver crash dumps to Microsoft's telemetry pipeline. "
                        + "DirectX diagnostic reporting can include driver version information, GPU model details, and crash callstacks, which constitute system fingerprinting data. On secure or air-gapped environments, turning off all outbound diagnostic reporting channels is a standard hardening measure.",
                    Tags = ["dx", "diagnostics", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks DirectX diagnostic reports to Microsoft; GPU details not included in telemetry.",
                    ApplyOps = [RegOp.SetDword(DisplayKey, "DisableDiagnosticReporting", 1)],
                    RemoveOps = [RegOp.DeleteValue(DisplayKey, "DisableDiagnosticReporting")],
                    DetectOps = [RegOp.CheckDword(DisplayKey, "DisableDiagnosticReporting", 1)],
                },
                new TweakDef
                {
                    Id = "dxshdr-disable-display-driver-auto-updates",
                    Label = "DirectX: Disable Automatic Display Driver Updates via Windows Update",
                    Category = "Display — Auto Play",
                    Description =
                        "Sets ExcludeWUDriversForDisplay=1 in Display policy. Prevents Windows Update from automatically installing newer display driver versions. "
                        + "Display driver updates during production hours can cause unexpected desktop resolution changes, HDR/SDR rendering behaviour changes, WHQL validation differences, and application crashes in software that depends on specific driver API behaviour. Enterprise GPU workstations should pin driver versions through a controlled update process rather than allowing automatic WU-driven driver installs.",
                    Tags = ["gpu", "driver", "windows-update", "stability", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Display drivers excluded from WU auto-update; manually controlled driver versioning for production stability.",
                    ApplyOps = [RegOp.SetDword(DisplayKey, "ExcludeWUDriversForDisplay", 1)],
                    RemoveOps = [RegOp.DeleteValue(DisplayKey, "ExcludeWUDriversForDisplay")],
                    DetectOps = [RegOp.CheckDword(DisplayKey, "ExcludeWUDriversForDisplay", 1)],
                },
                new TweakDef
                {
                    Id = "dxshdr-enable-gpu-virtual-memory-deduplication",
                    Label = "DirectX: Enable GPU Virtual Memory Page Deduplication",
                    Category = "Display — Auto Play",
                    Description =
                        "Sets EnableGPUPageDeduplication=1 in GraphicsDrivers registry. Enables page deduplication for GPU-accessible virtual memory, allowing the OS to coalesce identical read-only GPU memory pages (common in texture streaming) into shared physical pages. "
                        + "GPU texture streaming in games and rendering applications often loads the same texture mips into multiple contexts (shadow maps, reflection captures, environment renders). Page deduplication can reduce VRAM pressure by 5–15% in texture-heavy workloads, helping devices with lower VRAM capacities handle more assets without evicting and reloading from system RAM.",
                    Tags = ["gpu", "memory", "vram", "deduplication", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "GPU page dedup: 5–15% VRAM savings in texture-heavy workloads; helpful on lower-VRAM GPUs.",
                    ApplyOps = [RegOp.SetDword(GfxKey, "EnableGPUPageDeduplication", 1)],
                    RemoveOps = [RegOp.DeleteValue(GfxKey, "EnableGPUPageDeduplication")],
                    DetectOps = [RegOp.CheckDword(GfxKey, "EnableGPUPageDeduplication", 1)],
                },
            ];
    }

    // ── DisplayAdapterPolicy ──
    private static class _DisplayAdapterPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DisplayAdapters";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "dispadp-block-driver-install",
                Label = "Block Display Driver Installation by Users",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets DisableDriverInstall=1 in the DisplayAdapters policy key. Prevents "
                    + "standard users from installing or updating display adapter drivers "
                    + "without administrator approval. Stops unauthorised GPU driver "
                    + "changes that could destabilise corporate workstations or bypass "
                    + "validated driver stacks. Default: users with elevated privileges can "
                    + "install drivers. Recommended: 1 on managed corporate images.",
                Tags = ["display", "driver", "lockdown", "security", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDriverInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDriverInstall")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDriverInstall", 1)],
            },
            new TweakDef
            {
                Id = "dispadp-force-standard-vga",
                Label = "Force Standard VGA Display Adapter",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "Sets ForceStandardVGA=1 in the DisplayAdapters policy key. Forces Windows "
                    + "to use the Standard VGA driver instead of vendor-supplied GPU drivers. "
                    + "Useful during OS deployment, driver troubleshooting, or when locking "
                    + "VDI sessions to a baseline display mode. Disables GPU acceleration and "
                    + "hardware rendering in applications. Default: vendor driver is used. "
                    + "Caution: significantly degrades graphical performance.",
                Tags = ["display", "vga", "driver", "vdi", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ForceStandardVGA", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ForceStandardVGA")],
                DetectOps = [RegOp.CheckDword(Key, "ForceStandardVGA", 1)],
            },
            new TweakDef
            {
                Id = "dispadp-disable-dxva",
                Label = "Disable DirectX Video Acceleration (DXVA)",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Sets DisableDXVA=1 in the DisplayAdapters policy key. Prevents "
                    + "applications from using DirectX Video Acceleration (DXVA2) for "
                    + "hardware-accelerated video decoding. Forces software-based video "
                    + "decode. Useful in virtual environments where DXVA causes rendering "
                    + "artefacts or when validating software rendering consistency. Default: "
                    + "DXVA is active when supported by the GPU.",
                Tags = ["display", "dxva", "gpu", "video", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDXVA", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDXVA")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDXVA", 1)],
            },
            new TweakDef
            {
                Id = "dispadp-disable-wddm-gpu-compute",
                Label = "Disable GPU Compute (DirectCompute) in WDDM",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Sets DisableGPUCompute=1 in the DisplayAdapters policy key. Prevents "
                    + "applications from issuing DirectCompute (D3D11 Compute Shader / OpenCL) "
                    + "workloads through the WDDM display driver. Eliminates the GPU compute "
                    + "attack surface in locked-down environments and prevents unauthorised "
                    + "use of GPU resources for cryptocurrency mining. Default: GPU compute "
                    + "workloads are permitted.",
                Tags = ["display", "gpu", "compute", "security", "wddm", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableGPUCompute", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableGPUCompute")],
                DetectOps = [RegOp.CheckDword(Key, "DisableGPUCompute", 1)],
            },
            new TweakDef
            {
                Id = "dispadp-disable-display-scaling",
                Label = "Disable Custom Display Scaling (DPI Override) via Policy",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableDisplayScaling=1 in the DisplayAdapters policy key. Prevents "
                    + "users from overriding the system DPI scaling factor set by IT. Ensures "
                    + "a uniform 100% or 125% display scale across all managed workstations "
                    + "for consistent application layout and screen recording output. Default: "
                    + "DPI scale is user-configurable via Display Settings.",
                Tags = ["display", "dpi", "scaling", "group-policy", "lockdown"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDisplayScaling", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDisplayScaling")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDisplayScaling", 1)],
            },
            new TweakDef
            {
                Id = "dispadp-disable-display-rotation",
                Label = "Disable Display Rotation by Users",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableDisplayRotation=1 in the DisplayAdapters policy key. Prevents "
                    + "accidental or unauthorised screen rotation by locking the display "
                    + "orientation to landscape (0°). Eliminates the risk of users or "
                    + "applications flipping the display to portrait/inverted modes on "
                    + "fixed-mount kiosk or desktop units. Default: rotation is freely "
                    + "adjustable via Settings or keyboard shortcuts.",
                Tags = ["display", "rotation", "kiosk", "lockdown", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDisplayRotation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDisplayRotation")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDisplayRotation", 1)],
            },
            new TweakDef
            {
                Id = "dispadp-disable-mirroring",
                Label = "Disable Multi-Monitor Display Mirroring",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableMirroring=1 in the DisplayAdapters policy key. Prevents users "
                    + "from setting up display mirroring (duplicate/clone) configurations "
                    + "through Display Settings. Enforces extended desktop topology on "
                    + "multi-monitor workstations where mirroring would reduce display "
                    + "bandwidth or allow information to appear on a connected projector "
                    + "without IT authorisation. Default: mirroring is available.",
                Tags = ["display", "mirroring", "multi-monitor", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMirroring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMirroring")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMirroring", 1)],
            },
            new TweakDef
            {
                Id = "dispadp-disable-resolution-change",
                Label = "Lock Display Resolution via Policy",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets LockDisplayResolution=1 in the DisplayAdapters policy key. Prevents "
                    + "users from changing the screen resolution beyond what has been set by "
                    + "administrators. Ensures that kiosk, point-of-sale, or digital signage "
                    + "displays maintain the correct native resolution at all times. Default: "
                    + "resolution is freely adjustable. Recommended: 1 on fixed-function "
                    + "machines with a designated native resolution.",
                Tags = ["display", "resolution", "kiosk", "lockdown", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LockDisplayResolution", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LockDisplayResolution")],
                DetectOps = [RegOp.CheckDword(Key, "LockDisplayResolution", 1)],
            },
            new TweakDef
            {
                Id = "dispadp-disable-refresh-rate-change",
                Label = "Lock Display Refresh Rate via Policy",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets LockRefreshRate=1 in the DisplayAdapters policy key. Prevents users "
                    + "from changing the monitor refresh rate. Ensures that the validated "
                    + "refresh rate chosen by IT (e.g., 60 Hz for broadcast-safe output or "
                    + "75 Hz for flicker-sensitive users) remains in force. Default: refresh "
                    + "rate is configurable within the ranges supported by the display. "
                    + "Recommended: 1 on studio, broadcast, or accessibility-critical systems.",
                Tags = ["display", "refresh-rate", "lockdown", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LockRefreshRate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LockRefreshRate")],
                DetectOps = [RegOp.CheckDword(Key, "LockRefreshRate", 1)],
            },
            new TweakDef
            {
                Id = "dispadp-disable-color-depth-change",
                Label = "Lock Display Colour Depth via Policy",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets LockColorDepth=1 in the DisplayAdapters policy key. Prevents users "
                    + "from changing the colour depth (bits per pixel) of the display. Forces "
                    + "the system to remain at the IT-set colour depth (typically 32 bpp). "
                    + "Eliminates accidental changes to 16-bit colour mode that degrade UI "
                    + "rendering quality and break applications expecting true-colour output. "
                    + "Default: colour depth is user-configurable.",
                Tags = ["display", "color-depth", "bpp", "lockdown", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LockColorDepth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LockColorDepth")],
                DetectOps = [RegOp.CheckDword(Key, "LockColorDepth", 1)],
            },
        ];
    }

    // ── FocusAssistPolicy ──
    private static class _FocusAssistPolicy
    {
        private const string QhKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\QuietHours";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "fa-disable-quiet-hours",
                    Label = "Disable Focus Assist via Policy",
                    Category = "Display — Display Adapter",
                    Description =
                        "Sets AllowQuietHours=0 to disable Focus Assist (Quiet Hours) via Group Policy. Prevents users from activating Focus Assist manually or via automatic rules. All notifications are always delivered.",
                    Tags = ["focus assist", "quiet hours", "notifications", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Disables Focus Assist via GPO; all notifications always visible.",
                    ApplyOps = [RegOp.SetDword(QhKey, "AllowQuietHours", 0)],
                    RemoveOps = [RegOp.DeleteValue(QhKey, "AllowQuietHours")],
                    DetectOps = [RegOp.CheckDword(QhKey, "AllowQuietHours", 0)],
                },
                new TweakDef
                {
                    Id = "fa-disable-automatic-rules",
                    Label = "Disable Focus Assist Automatic Rules",
                    Category = "Display — Display Adapter",
                    Description =
                        "Sets AllowScheduledQuietHours=0 to prevent Focus Assist from activating automatically based on time-of-day rules, duplicate display, or gaming/fullscreen detection.",
                    Tags = ["focus assist", "automatic rules", "schedule", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Prevents automatic Focus Assist activation; manual toggle still visible unless PolicyAllowQuietHours=0.",
                    ApplyOps = [RegOp.SetDword(QhKey, "AllowScheduledQuietHours", 0)],
                    RemoveOps = [RegOp.DeleteValue(QhKey, "AllowScheduledQuietHours")],
                    DetectOps = [RegOp.CheckDword(QhKey, "AllowScheduledQuietHours", 0)],
                },
                new TweakDef
                {
                    Id = "fa-disable-game-mode-dnd",
                    Label = "Disable Focus Assist in Game Mode",
                    Category = "Display — Display Adapter",
                    Description =
                        "Sets AllowGameModeQuietHours=0 to prevent Windows from automatically enabling Focus Assist when a game is detected as running in fullscreen. Ensures notifications are visible during gaming sessions.",
                    Tags = ["focus assist", "game mode", "gaming", "notifications", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Disables auto-DND during gaming; all notifications visible even in fullscreen games.",
                    ApplyOps = [RegOp.SetDword(QhKey, "AllowGameModeQuietHours", 0)],
                    RemoveOps = [RegOp.DeleteValue(QhKey, "AllowGameModeQuietHours")],
                    DetectOps = [RegOp.CheckDword(QhKey, "AllowGameModeQuietHours", 0)],
                },
                new TweakDef
                {
                    Id = "fa-disable-presentation-dnd",
                    Label = "Disable Focus Assist When Presenting",
                    Category = "Display — Display Adapter",
                    Description =
                        "Sets AllowPresentationModeQuietHours=0 to prevent Windows from automatically activating Focus Assist when a duplicate display (projector/second monitor) is detected. Prevents accidental notification suppression in meeting rooms.",
                    Tags = ["focus assist", "presentation", "display", "notifications", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Disables presentation-mode Focus Assist; important for kiosk or shared display environments.",
                    ApplyOps = [RegOp.SetDword(QhKey, "AllowPresentationModeQuietHours", 0)],
                    RemoveOps = [RegOp.DeleteValue(QhKey, "AllowPresentationModeQuietHours")],
                    DetectOps = [RegOp.CheckDword(QhKey, "AllowPresentationModeQuietHours", 0)],
                },
                new TweakDef
                {
                    Id = "fa-disable-summary-notification",
                    Label = "Disable Focus Assist Summary Notification",
                    Category = "Display — Display Adapter",
                    Description =
                        "Sets AllowSummaryNotification=0 to suppress the 'You missed N notifications while Focus Assist was on' toast that appears after a Focus Assist session ends. Reduces notification clutter on shared machines.",
                    Tags = ["focus assist", "summary", "notification", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Suppresses the post-Focus-Assist summary notification toast.",
                    ApplyOps = [RegOp.SetDword(QhKey, "AllowSummaryNotification", 0)],
                    RemoveOps = [RegOp.DeleteValue(QhKey, "AllowSummaryNotification")],
                    DetectOps = [RegOp.CheckDword(QhKey, "AllowSummaryNotification", 0)],
                },
                new TweakDef
                {
                    Id = "fa-disable-fullscreen-dnd",
                    Label = "Disable Focus Assist in Fullscreen Apps",
                    Category = "Display — Display Adapter",
                    Description =
                        "Sets AllowFullScreenModeQuietHours=0 to prevent Windows from automatically activating Focus Assist when any application is running in fullscreen mode. Ensures notifications are delivered in all display states.",
                    Tags = ["focus assist", "fullscreen", "apps", "notifications", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Prevents auto-DND when any app goes fullscreen; notifications remain visible.",
                    ApplyOps = [RegOp.SetDword(QhKey, "AllowFullScreenModeQuietHours", 0)],
                    RemoveOps = [RegOp.DeleteValue(QhKey, "AllowFullScreenModeQuietHours")],
                    DetectOps = [RegOp.CheckDword(QhKey, "AllowFullScreenModeQuietHours", 0)],
                },
                new TweakDef
                {
                    Id = "fa-lock-priority-list",
                    Label = "Lock Focus Assist Priority List",
                    Category = "Display — Display Adapter",
                    Description =
                        "Sets LockPriorityList=1 to prevent users from modifying the Focus Assist priority list that determines which apps and contacts can break through Focus Assist. Enforces a consistent notification priority on managed devices.",
                    Tags = ["focus assist", "priority", "policy", "lock"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Prevents end-users from adding personal priority contacts/apps; affects notification reach.",
                    ApplyOps = [RegOp.SetDword(QhKey, "LockPriorityList", 1)],
                    RemoveOps = [RegOp.DeleteValue(QhKey, "LockPriorityList")],
                    DetectOps = [RegOp.CheckDword(QhKey, "LockPriorityList", 1)],
                },
                new TweakDef
                {
                    Id = "fa-disable-out-of-hours-rule",
                    Label = "Disable Focus Assist Outside Working Hours Rule",
                    Category = "Display — Display Adapter",
                    Description =
                        "Sets AllowOutOfHoursQuietHours=0 to prevent Focus Assist from automatically activating during hours outside those defined as 'working hours' in the Windows calendar. Ensures uniform notification delivery.",
                    Tags = ["focus assist", "working hours", "schedule", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Disables outside-working-hours automatic Focus Assist rule.",
                    ApplyOps = [RegOp.SetDword(QhKey, "AllowOutOfHoursQuietHours", 0)],
                    RemoveOps = [RegOp.DeleteValue(QhKey, "AllowOutOfHoursQuietHours")],
                    DetectOps = [RegOp.CheckDword(QhKey, "AllowOutOfHoursQuietHours", 0)],
                },
                new TweakDef
                {
                    Id = "fa-disable-first-hour-rule",
                    Label = "Disable Focus Assist First Hour After Resume Rule",
                    Category = "Display — Display Adapter",
                    Description =
                        "Sets AllowFirstHourQuietHours=0 to prevent Windows from enabling Focus Assist for the first hour after the device resumes from sleep or hibernation. Ensures immediate notification delivery after wake.",
                    Tags = ["focus assist", "resume", "sleep", "notifications", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Notifications delivered immediately after device wakes; no first-hour suppression.",
                    ApplyOps = [RegOp.SetDword(QhKey, "AllowFirstHourQuietHours", 0)],
                    RemoveOps = [RegOp.DeleteValue(QhKey, "AllowFirstHourQuietHours")],
                    DetectOps = [RegOp.CheckDword(QhKey, "AllowFirstHourQuietHours", 0)],
                },
                new TweakDef
                {
                    Id = "fa-force-priority-only-mode",
                    Label = "Force Focus Assist Priority-Only Mode",
                    Category = "Display — Display Adapter",
                    Description =
                        "Sets DefaultProfile=1 to configure Focus Assist to only allow notifications from priority contacts and apps through when activated. Prevents the 'Alarms only' level and limits Focus Assist to the least invasive mode.",
                    Tags = ["focus assist", "priority", "mode", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Forces priority-only mode when Focus Assist is active; some notifications still delivered.",
                    ApplyOps = [RegOp.SetDword(QhKey, "DefaultProfile", 1)],
                    RemoveOps = [RegOp.DeleteValue(QhKey, "DefaultProfile")],
                    DetectOps = [RegOp.CheckDword(QhKey, "DefaultProfile", 1)],
                },
            ];
    }

    // ── FontInstallationPolicy ──
    private static class _FontInstallationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
        private const string CtKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore";
        private const string FontKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Fonts";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "fontpol-block-user-font-install",
                    Label = "Block Standard Users from Installing Fonts",
                    Category = "Display — Display Adapter",
                    Description =
                        "Prevents standard (non-administrator) users from installing fonts per-user via the Settings app or drag-and-drop, ensuring font management is controlled by IT and that untrusted fonts (a known exploitation vector) are not installed.",
                    Tags = ["fonts", "installation", "standard-user", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "User-mode font installation blocked; only admins can install fonts system-wide.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockUserFromInstallingFonts", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockUserFromInstallingFonts")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockUserFromInstallingFonts", 1)],
                },
                new TweakDef
                {
                    Id = "fontpol-disable-online-font-provider",
                    Label = "Disable Windows Online Font Provider",
                    Category = "Display — Display Adapter",
                    Description =
                        "Disables the Windows Online Font Provider service that streams fonts from Microsoft's cloud on demand, preventing outbound font download requests and ensuring all fonts used are locally installed and auditable.",
                    Tags = ["fonts", "online-provider", "cloud", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Online font provider disabled; no fonts streamed from Microsoft cloud. All fonts must be pre-installed.",
                    ApplyOps = [RegOp.SetDword(FontKey, "EnableFontProviders", 0)],
                    RemoveOps = [RegOp.DeleteValue(FontKey, "EnableFontProviders")],
                    DetectOps = [RegOp.CheckDword(FontKey, "EnableFontProviders", 0)],
                },
                new TweakDef
                {
                    Id = "fontpol-disable-font-streaming-uap",
                    Label = "Disable Font Streaming for Universal Apps",
                    Category = "Display — Display Adapter",
                    Description =
                        "Prevents Universal Windows Platform (UWP) apps from requesting font streaming from Microsoft's online font provider, ensuring that Store apps cannot silently download fonts as part of rendering pipelines.",
                    Tags = ["fonts", "uwp", "streaming", "cloud", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Font streaming from cloud disabled for UWP apps; fonts must be pre-installed for all Store app rendering.",
                    ApplyOps = [RegOp.SetDword(FontKey, "DisableFontStreamingForUWP", 1)],
                    RemoveOps = [RegOp.DeleteValue(FontKey, "DisableFontStreamingForUWP")],
                    DetectOps = [RegOp.CheckDword(FontKey, "DisableFontStreamingForUWP", 1)],
                },
                new TweakDef
                {
                    Id = "fontpol-block-admin-font-from-web",
                    Label = "Warn Before Installing Fonts Downloaded from the Web",
                    Category = "Display — Display Adapter",
                    Description =
                        "Configures Windows to display a security warning when an administrator attempts to install a font file downloaded from the internet, reducing the risk of admins silently installing font files with embedded exploit code.",
                    Tags = ["fonts", "web-download", "security-warning", "admin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Security warning shown before installing web-downloaded fonts; reduces risk of admin installing malicious fonts.",
                    ApplyOps = [RegOp.SetDword(FontKey, "WarnBeforeInstallingWebFonts", 1)],
                    RemoveOps = [RegOp.DeleteValue(FontKey, "WarnBeforeInstallingWebFonts")],
                    DetectOps = [RegOp.CheckDword(FontKey, "WarnBeforeInstallingWebFonts", 1)],
                },
                new TweakDef
                {
                    Id = "fontpol-block-font-preview",
                    Label = "Block Font Preview in Font Viewer for Untrusted Sources",
                    Category = "Display — Display Adapter",
                    Description =
                        "Prevents standard users from previewing font files from untrusted locations in the Font Viewer, reducing the attack surface for font-parsing vulnerabilities triggered simply by previewing a crafted font file.",
                    Tags = ["fonts", "font-preview", "untrusted", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Font file preview restricted for untrusted sources; reduces attack surface for malicious font parsing.",
                    ApplyOps = [RegOp.SetDword(FontKey, "BlockFontPreviewFromUntrusted", 1)],
                    RemoveOps = [RegOp.DeleteValue(FontKey, "BlockFontPreviewFromUntrusted")],
                    DetectOps = [RegOp.CheckDword(FontKey, "BlockFontPreviewFromUntrusted", 1)],
                },
                new TweakDef
                {
                    Id = "fontpol-disable-font-telemetry",
                    Label = "Disable Font Provider Telemetry to Microsoft",
                    Category = "Display — Display Adapter",
                    Description =
                        "Prevents the Windows font provider service from sending telemetry about font usage, installed font families, and font-related application activity to Microsoft.",
                    Tags = ["fonts", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Font provider telemetry to Microsoft disabled; font usage statistics not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(FontKey, "DisableFontTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(FontKey, "DisableFontTelemetry")],
                    DetectOps = [RegOp.CheckDword(FontKey, "DisableFontTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "fontpol-set-font-antialiasing-cleartype",
                    Label = "Enforce ClearType Font Antialiasing for All Users",
                    Category = "Display — Display Adapter",
                    Description =
                        "Enforces ClearType sub-pixel antialiasing for all user sessions via policy, overriding per-user font smoothing settings to ensure consistent, high-quality text rendering on all LCD displays in the organisation.",
                    Tags = ["fonts", "cleartype", "antialiasing", "rendering", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "ClearType antialiasing enforced for all users; consistent sub-pixel rendering across all LCD monitors.",
                    ApplyOps = [RegOp.SetDword(FontKey, "ForceClearType", 1)],
                    RemoveOps = [RegOp.DeleteValue(FontKey, "ForceClearType")],
                    DetectOps = [RegOp.CheckDword(FontKey, "ForceClearType", 1)],
                },
                new TweakDef
                {
                    Id = "fontpol-disable-eudcedit",
                    Label = "Disable Creation of End User Defined Character (EUDC) Fonts",
                    Category = "Display — Display Adapter",
                    Description =
                        "Prevents users from creating custom EUDC (End User Defined Character) fonts using the EUDC Editor, which would install per-user font registry entries that are not centrally managed.",
                    Tags = ["fonts", "eudc", "custom-characters", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "EUDC font creation disabled; users cannot create custom character fonts via EUDC Editor.",
                    ApplyOps = [RegOp.SetDword(FontKey, "DisableEUDCEditor", 1)],
                    RemoveOps = [RegOp.DeleteValue(FontKey, "DisableEUDCEditor")],
                    DetectOps = [RegOp.CheckDword(FontKey, "DisableEUDCEditor", 1)],
                },
                new TweakDef
                {
                    Id = "fontpol-restrict-font-subsetting",
                    Label = "Restrict Font Subsetting to Prevent Embedded Sensitive Data",
                    Category = "Display — Display Adapter",
                    Description =
                        "Configures Windows font embedding policy to allow printout-only font embedding, preventing applications from creating documents with fully embedded fonts that could be used to covertly exfiltrate data via font steganography.",
                    Tags = ["fonts", "embedding", "subsetting", "data-exfiltration", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Font subsetting restricted to print-only embedding; full font embedding in documents blocked.",
                    ApplyOps = [RegOp.SetDword(FontKey, "RestrictFontSubsetting", 1)],
                    RemoveOps = [RegOp.DeleteValue(FontKey, "RestrictFontSubsetting")],
                    DetectOps = [RegOp.CheckDword(FontKey, "RestrictFontSubsetting", 1)],
                },
                new TweakDef
                {
                    Id = "fontpol-audit-font-install-events",
                    Label = "Audit Font Installation Events in Security Log",
                    Category = "Display — Display Adapter",
                    Description =
                        "Enables Security event log entries for every font installation or removal event on the system, providing change-management visibility into font inventory changes for security and compliance auditing.",
                    Tags = ["fonts", "audit", "event-log", "change-management", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Font install/remove events logged in Security log; font inventory changes auditable for compliance.",
                    ApplyOps = [RegOp.SetDword(FontKey, "AuditFontInstallEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(FontKey, "AuditFontInstallEvents")],
                    DetectOps = [RegOp.CheckDword(FontKey, "AuditFontInstallEvents", 1)],
                },
            ];
    }

    // ── FontProviderPolicy ──
    private static class _FontProviderPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\FontProvider";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "fontprov-disable-online-fonts",
                Label = "Disable Online Font Provider",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "The Windows online font provider downloads fonts from Microsoft's online font store and makes them available to applications through the DirectWrite font API. Disabling the online font provider prevents Windows from connecting to Microsoft's online font service to download cloud-hosted fonts. Online font connections disclose font usage patterns and installed application names to external font services. Enterprise endpoints should only use fonts distributed through managed IT channels rather than dynamically downloading from cloud services. Offline font repositories ensure that document rendering is consistent and does not depend on external network connectivity. Disabling the online provider has no impact on locally installed fonts which continue to function normally.",
                Tags = ["fonts", "online", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableOnlineFontProvider", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOnlineFontProvider")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOnlineFontProvider", 1)],
            },
            new TweakDef
            {
                Id = "fontprov-disable-font-streaming",
                Label = "Disable Font Streaming",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Font streaming allows Windows to download only the portions of a font file needed for immediate rendering rather than downloading the complete font. While efficient for network bandwidth, font streaming creates persistent outbound connections to external font servers during document rendering. Disabling font streaming prevents incremental font download requests from being made during document rendering operations. Streaming requests expose document content characteristics to font provider infrastructure through the specific glyph ranges requested. In air-gapped or strictly controlled environments, preventing external network requests during document rendering is an important isolation property. Enterprise fonts should be fully installed locally to eliminate any dependency on streaming from external services.",
                Tags = ["fonts", "streaming", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFontStreaming", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFontStreaming")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFontStreaming", 1)],
            },
            new TweakDef
            {
                Id = "fontprov-disable-font-download-suggestions",
                Label = "Disable Font Download Suggestions",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Font download suggestions appear in applications when a document contains characters that require fonts not currently installed on the system. Disabling download suggestions prevents Windows from presenting prompts to download fonts from the online store. Font suggestions are consumer-oriented features intended for home use and are not appropriate in enterprise document environments. Suggestion prompts can lead users to inadvertently install fonts from external sources that bypass IT font management. Enterprise font governance requires that all font installations go through the managed software deployment pipeline. Disabling suggestions keeps the user experience consistent and prevents unsanctioned software additions through font installation prompts.",
                Tags = ["fonts", "suggestions", "usability", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFontDownloadSuggestions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFontDownloadSuggestions")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFontDownloadSuggestions", 1)],
            },
            new TweakDef
            {
                Id = "fontprov-disable-update-check",
                Label = "Disable Font Provider Update Checks",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "The font provider periodically checks for updated versions of installed fonts and new font offerings available in the online font catalog. Disabling update checks prevents outbound connections to font provider services to enumerate font catalog changes. Update checks disclose which fonts are installed on the endpoint to external font provider infrastructure. Enterprise patch and update management should handle font updates through controlled channels rather than automatic cloud-driven updates. Preventing update checks reduces unnecessary outbound network connections from managed endpoints. Font rendering and all locally installed font functionality continue normally without font provider update connectivity.",
                Tags = ["fonts", "updates", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFontProviderUpdateChecks", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFontProviderUpdateChecks")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFontProviderUpdateChecks", 1)],
            },
            new TweakDef
            {
                Id = "fontprov-disable-auto-install",
                Label = "Disable Font Auto-Installation",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Font auto-installation silently downloads and installs fonts referenced in documents or web pages when the current font set does not include the required typeface. Disabling font auto-installation prevents new font files from being downloaded and installed without explicit administrator approval. Auto-installed fonts from unknown sources may embed malicious content targeting font rendering vulnerabilities. Font parsing vulnerabilities have historically been exploited through specially crafted font files embedded in documents and web pages. Enterprise font libraries should be curated, validated, and distributed by IT to prevent exposure to malicious font files. Disabling auto-installation ensures font additions require deliberate change management.",
                Tags = ["fonts", "installation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFontAutoInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFontAutoInstall")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFontAutoInstall", 1)],
            },
            new TweakDef
            {
                Id = "fontprov-disable-per-user-fonts",
                Label = "Disable Per-User Font Installation",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Per-user font installation allows standard user accounts to install fonts without administrator privileges by placing them in the user font directory. Disabling per-user font installation prevents non-administrator accounts from installing any fonts on the managed endpoint. Standard users installing fonts bypass IT font governance and could introduce malicious fonts targeting applications and rendering pipelines. Enterprise font management standards require administrator-level approval for all software including font installation. Centralized font distribution ensures all users have a consistent and auditable font library on managed endpoints. Disabling per-user installation ensures font changes require change management review and administrator action.",
                Tags = ["fonts", "per-user", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePerUserFontInstallation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePerUserFontInstallation")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePerUserFontInstallation", 1)],
            },
            new TweakDef
            {
                Id = "fontprov-disable-font-cache",
                Label = "Disable Font Provider Cache",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The font provider maintains a cache of recently used fonts to improve rendering performance when font files are streamed or remotely hosted. Disabling the font provider cache forces font data to be retrieved on every rendering operation rather than using cached data. Cache data from remote font providers may contain residual content that reveals document rendering activity. In secure environments where font rendering history should not persist, disabling the cache provides a privacy benefit. Cache invalidation removes stale font data that could cause rendering inconsistencies after font library changes. Locally installed fonts have their own file system cache managed by the OS and are unaffected by this provider cache setting.",
                Tags = ["fonts", "cache", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFontProviderCache", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFontProviderCache")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFontProviderCache", 1)],
            },
            new TweakDef
            {
                Id = "fontprov-disable-telemetry",
                Label = "Disable Font Provider Telemetry",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Font provider telemetry reports data about font usage patterns, popular font requests, and download success metrics to Microsoft. This helps improve the online font catalog and font delivery infrastructure for future Windows releases. Disabling font provider telemetry prevents font usage statistics from being transmitted to Microsoft's analytics systems. Font usage patterns can correlate with document types and application usage revealing sensitive business activity information. Consumer-facing analytics telemetry is generally not appropriate in enterprise environments under data governance frameworks. All font rendering and locally installed font functionality continues to operate normally without telemetry.",
                Tags = ["fonts", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFontProviderTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFontProviderTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFontProviderTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "fontprov-disable-emoji-font",
                Label = "Disable Cloud Emoji Font Downloads",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Windows can download updated emoji font data from Microsoft's online services to ensure current emoji rendering support for new Unicode code points. Disabling cloud emoji font downloads prevents automatic retrieval of updated emoji rendering data from external services. Emoji font update connections represent unnecessary outbound network traffic from managed enterprise endpoints. Enterprise communication tools using emoji do not require the latest Unicode version for functional operation. Content filtering and web proxy logs can be simplified by preventing font-related cloud connections from endpoints. Disabling cloud emoji font updates has only cosmetic impact and does not affect any functional capability.",
                Tags = ["fonts", "emoji", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCloudEmojiFont", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudEmojiFont")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCloudEmojiFont", 1)],
            },
            new TweakDef
            {
                Id = "fontprov-disable-third-party-provider",
                Label = "Disable Third-Party Font Provider Registration",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Windows font provider infrastructure allows third-party vendors to register custom font providers that deliver additional font content. Disabling third-party font provider registration prevents external font service vendors from integrating their font libraries into the Windows font rendering pipeline. Third-party font providers have varying security review standards and may introduce font files from unvalidated sources. Enterprise endpoints should use only the built-in Microsoft font infrastructure with fonts distributed through IT management. Third-party provider code executing in the rendering pipeline creates an additional attack surface for privilege escalation. Disabling third-party providers ensures only verified Microsoft font infrastructure handles font rendering on managed endpoints.",
                Tags = ["fonts", "third-party", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableThirdPartyFontProvider", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableThirdPartyFontProvider")],
                DetectOps = [RegOp.CheckDword(Key, "DisableThirdPartyFontProvider", 1)],
            },
        ];
    }

    // ── GdiRendererPolicy ──
    private static class _GdiRendererPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Dwm";
        private const string RdsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";
        private const string GdiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\GDI";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "gdipol-enable-dwm-hardware-acceleration",
                    Label = "Enforce DWM Hardware Acceleration is Always On",
                    Category = "Display — Display Adapter",
                    Description =
                        "Ensures the Desktop Window Manager (DWM) uses GPU hardware acceleration for compositing, preventing software fallback rendering that consumes excessive CPU and produces visual artefacts on modern hardware.",
                    Tags = ["dwm", "hardware-acceleration", "gpu", "rendering", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "DWM hardware acceleration enforced; software rendering fallback blocked. GPU required for compositing.",
                    ApplyOps = [RegOp.SetDword(Key, "DisallowRemoteDesktopCompositing", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisallowRemoteDesktopCompositing")],
                    DetectOps = [RegOp.CheckDword(Key, "DisallowRemoteDesktopCompositing", 0)],
                },
                new TweakDef
                {
                    Id = "gdipol-disable-rdp-software-rendering",
                    Label = "Disable Software Rendering for RDP Sessions",
                    Category = "Display — Display Adapter",
                    Description =
                        "Prevents Remote Desktop sessions from falling back to GDI software rendering when a GPU is available, ensuring RemoteFX or hardware-accelerated codec paths are always used for consistent performance.",
                    Tags = ["gdi", "rdp", "software-rendering", "remotedesktop", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "RDP software rendering fallback disabled; RemoteFX / GPU codec path used for remote sessions.",
                    ApplyOps = [RegOp.SetDword(RdsKey, "fDisableSoftwareRendering", 1)],
                    RemoveOps = [RegOp.DeleteValue(RdsKey, "fDisableSoftwareRendering")],
                    DetectOps = [RegOp.CheckDword(RdsKey, "fDisableSoftwareRendering", 1)],
                },
                new TweakDef
                {
                    Id = "gdipol-enable-gdi-scaling",
                    Label = "Enable GDI DPI Scaling for Legacy Applications",
                    Category = "Display — Display Adapter",
                    Description =
                        "Enables system-wide GDI-based DPI scaling for legacy applications that do not declare DPI awareness, preventing blurry rendering of older software on high-DPI monitors without requiring per-app compatibility flags.",
                    Tags = ["gdi", "dpi-scaling", "high-dpi", "legacy-apps", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "GDI DPI scaling enabled for non-DPI-aware apps; older software rendered crisply on high-DPI screens.",
                    ApplyOps = [RegOp.SetDword(GdiKey, "EnableGDIScaling", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdiKey, "EnableGDIScaling")],
                    DetectOps = [RegOp.CheckDword(GdiKey, "EnableGDIScaling", 1)],
                },
                new TweakDef
                {
                    Id = "gdipol-disable-directdraw-hw-acceleration",
                    Label = "Disable DirectDraw Hardware Acceleration",
                    Category = "Display — Display Adapter",
                    Description =
                        "Disables DirectDraw hardware acceleration, forcing all DirectDraw rendering to use the software emulation path. Used in environments where GPU driver instability causes crashes or display corruption.",
                    Tags = ["directdraw", "hardware-acceleration", "gpu-driver", "stability", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "DirectDraw hardware acceleration disabled; DirectDraw falls back to software. Performance impacted.",
                    ApplyOps = [RegOp.SetDword(GdiKey, "DisableDirectDrawHWAcceleration", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdiKey, "DisableDirectDrawHWAcceleration")],
                    DetectOps = [RegOp.CheckDword(GdiKey, "DisableDirectDrawHWAcceleration", 1)],
                },
                new TweakDef
                {
                    Id = "gdipol-set-gdi-batch-limit",
                    Label = "Set GDI Batch Limit to Optimise Rendering Performance",
                    Category = "Display — Display Adapter",
                    Description =
                        "Sets the GDI batching limit to 0 (immediate flush) to ensure all GDI drawing calls are synchronously sent to the display driver, improving rendering correctness on systems with unstable batch coalescing.",
                    Tags = ["gdi", "batch-limit", "rendering", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "GDI batch limit set to 0 (immediate flush); drawing calls not batched. Improves rendering correctness.",
                    ApplyOps = [RegOp.SetDword(GdiKey, "BatchLimit", 0)],
                    RemoveOps = [RegOp.DeleteValue(GdiKey, "BatchLimit")],
                    DetectOps = [RegOp.CheckDword(GdiKey, "BatchLimit", 0)],
                },
                new TweakDef
                {
                    Id = "gdipol-block-gdi-object-table-growth",
                    Label = "Limit Per-Process GDI Object Count to 10000",
                    Category = "Display — Display Adapter",
                    Description =
                        "Sets the per-process GDI object limit to 10000 (down from the default 65536), preventing GDI handle exhaustion attacks where a single process allocates all available GDI handles and crashes other processes.",
                    Tags = ["gdi", "handle-limit", "object-table", "dos-prevention", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "GDI object limit per-process capped at 10000; GDI handle exhaustion attacks prevented.",
                    ApplyOps = [RegOp.SetDword(GdiKey, "MaxGDIObjects", 10000)],
                    RemoveOps = [RegOp.DeleteValue(GdiKey, "MaxGDIObjects")],
                    DetectOps = [RegOp.CheckDword(GdiKey, "MaxGDIObjects", 10000)],
                },
                new TweakDef
                {
                    Id = "gdipol-disable-printer-gdi-metafile",
                    Label = "Disable GDI Printer Metafile Spool Format",
                    Category = "Display — Display Adapter",
                    Description =
                        "Disables the legacy EMF (Enhanced Metafile) spool format for GDI-based printing and forces direct printing via the XPS document pipeline, reducing exposure to EMF file parsing vulnerabilities in the spooler.",
                    Tags = ["gdi", "metafile", "printing", "spooler", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "GDI EMF printer spool format disabled; printing uses XPS pipeline. Legacy GDI-only printers may not work.",
                    ApplyOps = [RegOp.SetDword(GdiKey, "DisableGDIPrinterMetafile", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdiKey, "DisableGDIPrinterMetafile")],
                    DetectOps = [RegOp.CheckDword(GdiKey, "DisableGDIPrinterMetafile", 1)],
                },
                new TweakDef
                {
                    Id = "gdipol-enable-gdi-audit",
                    Label = "Enable GDI Object Creation Audit Logging",
                    Category = "Display — Display Adapter",
                    Description =
                        "Enables lightweight audit logging for GDI object creation and destruction at the policy level, providing visibility into unusual GDI handle consumption patterns that may indicate malicious UI automation or exploitation attempts.",
                    Tags = ["gdi", "audit", "object-creation", "event-log", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "GDI object creation auditing enabled; unusual handle patterns visible for security monitoring.",
                    ApplyOps = [RegOp.SetDword(GdiKey, "EnableGDIObjectAudit", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdiKey, "EnableGDIObjectAudit")],
                    DetectOps = [RegOp.CheckDword(GdiKey, "EnableGDIObjectAudit", 1)],
                },
                new TweakDef
                {
                    Id = "gdipol-disable-gdi-screen-capture",
                    Label = "Block GDI-Based Screen Capture by Standard Applications",
                    Category = "Display — Display Adapter",
                    Description =
                        "Restricts the ability of standard (non-elevated) applications to capture the entire screen via BitBlt from the desktop DC, limiting screen capture to applications with explicit capture permissions.",
                    Tags = ["gdi", "screen-capture", "bitblt", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "GDI full-screen BitBlt blocked for standard apps; screen capture requires explicit permission.",
                    ApplyOps = [RegOp.SetDword(GdiKey, "DisableScreenCaptureViaGDI", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdiKey, "DisableScreenCaptureViaGDI")],
                    DetectOps = [RegOp.CheckDword(GdiKey, "DisableScreenCaptureViaGDI", 1)],
                },
                new TweakDef
                {
                    Id = "gdipol-disable-gdi-telemetry",
                    Label = "Disable GDI Renderer Telemetry Reporting to Microsoft",
                    Category = "Display — Display Adapter",
                    Description =
                        "Prevents the GDI rendering subsystem from sending object usage, rendering performance, and driver compatibility telemetry to Microsoft.",
                    Tags = ["gdi", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "GDI renderer telemetry to Microsoft disabled; rendering stats and driver compat data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(GdiKey, "DisableGDITelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(GdiKey, "DisableGDITelemetry")],
                    DetectOps = [RegOp.CheckDword(GdiKey, "DisableGDITelemetry", 1)],
                },
            ];
    }

    // ── GpuComputePolicy ──
    private static class _GpuComputePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GPU";
        private const string MlKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinML";
        private const string DmlKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DirectML";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "gpucmp-disable-winml-gpu",
                    Label = "Disable Windows ML GPU Inference",
                    Category = "Display — Display Adapter",
                    Description =
                        "Prevents Windows Machine Learning (WinML) from executing inference operations on the GPU, forcing model evaluation to the CPU, which can reduce power consumption and GPU memory pressure on non-AI workstation deployments.",
                    Tags = ["windows-ml", "gpu-inference", "ai", "compute", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WinML GPU inference disabled; all ML model inference runs on CPU. AI apps may be significantly slower.",
                    ApplyOps = [RegOp.SetDword(MlKey, "DisableGPUInference", 1)],
                    RemoveOps = [RegOp.DeleteValue(MlKey, "DisableGPUInference")],
                    DetectOps = [RegOp.CheckDword(MlKey, "DisableGPUInference", 1)],
                },
                new TweakDef
                {
                    Id = "gpucmp-limit-winml-vram",
                    Label = "Limit Windows ML VRAM Usage to 2 GB",
                    Category = "Display — Display Adapter",
                    Description =
                        "Caps the amount of GPU VRAM that Windows ML inference sessions can allocate to 2048 MB, preventing WinML workloads from consuming all available GPU memory and degrading rendering performance of foreground applications.",
                    Tags = ["windows-ml", "vram", "memory-limit", "gpu", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WinML VRAM capped at 2 GB; large ML models that exceed limit fall back to system RAM or fail.",
                    ApplyOps = [RegOp.SetDword(MlKey, "MaxVRAMMB", 2048)],
                    RemoveOps = [RegOp.DeleteValue(MlKey, "MaxVRAMMB")],
                    DetectOps = [RegOp.CheckDword(MlKey, "MaxVRAMMB", 2048)],
                },
                new TweakDef
                {
                    Id = "gpucmp-disable-directml-third-party",
                    Label = "Block Third-Party DirectML Operator Packages",
                    Category = "Display — Display Adapter",
                    Description =
                        "Prevents third-party applications from loading external DirectML operator packages outside of the Windows SDK, reducing the attack surface from unsigned or malicious ML operator DLLs loaded into application GPU compute contexts.",
                    Tags = ["directml", "operator-packages", "third-party", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Third-party DirectML operator packages blocked; only SDK-bundled operators loaded in GPU compute pipelines.",
                    ApplyOps = [RegOp.SetDword(DmlKey, "BlockThirdPartyOperators", 1)],
                    RemoveOps = [RegOp.DeleteValue(DmlKey, "BlockThirdPartyOperators")],
                    DetectOps = [RegOp.CheckDword(DmlKey, "BlockThirdPartyOperators", 1)],
                },
                new TweakDef
                {
                    Id = "gpucmp-disable-winml-telemetry",
                    Label = "Disable Windows ML Telemetry Reporting to Microsoft",
                    Category = "Display — Display Adapter",
                    Description =
                        "Prevents Windows ML from sending model inference statistics, GPU capability, and API usage telemetry to Microsoft, protecting information about AI workload characteristics from cloud disclosure.",
                    Tags = ["windows-ml", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WinML telemetry to Microsoft disabled; inference stats and GPU model data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(MlKey, "DisableWinMLTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(MlKey, "DisableWinMLTelemetry")],
                    DetectOps = [RegOp.CheckDword(MlKey, "DisableWinMLTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "gpucmp-set-gpu-compute-app-priority",
                    Label = "Set Foreground App GPU Compute Priority to High",
                    Category = "Display — Display Adapter",
                    Description =
                        "Configures the GPU compute scheduler to give foreground applications higher compute queue priority than background GPU processes, ensuring interactive AI and graphics applications are not starved by background ML training jobs.",
                    Tags = ["gpu", "compute-priority", "scheduler", "foreground", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Foreground app GPU compute priority elevated; background GPU jobs deprioritised for better interactivity.",
                    ApplyOps = [RegOp.SetDword(Key, "ForegroundComputePriority", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ForegroundComputePriority")],
                    DetectOps = [RegOp.CheckDword(Key, "ForegroundComputePriority", 2)],
                },
                new TweakDef
                {
                    Id = "gpucmp-disable-cuda-in-wsl",
                    Label = "Disable CUDA GPU Passthrough into WSL2",
                    Category = "Display — Display Adapter",
                    Description =
                        "Disables CUDA and DirectX GPU passthrough into WSL2 virtual machines, preventing WSL2 Linux processes from accessing GPU compute resources and potential GPU-level privilege escalation from Linux guest to Windows host.",
                    Tags = ["gpu", "wsl2", "cuda", "gpu-passthrough", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "GPU passthrough to WSL2 disabled; CUDA and DirectX not accessible from Linux WSL2 processes.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWSLGPUPassthrough", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWSLGPUPassthrough")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWSLGPUPassthrough", 1)],
                },
                new TweakDef
                {
                    Id = "gpucmp-restrict-gpu-access-untrusted",
                    Label = "Restrict GPU Compute Access for Untrusted Applications",
                    Category = "Display — Display Adapter",
                    Description =
                        "Enables GPU access filtering for untrusted (non-publisher-verified, non-Store) applications, preventing low-reputation software from accessing GPU compute queues that could be used for crypto-mining without user consent.",
                    Tags = ["gpu", "compute-access", "untrusted-apps", "cryptomining", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "GPU compute access restricted for untrusted apps; crypto-mining by unsigned background apps blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "RestrictComputeForUntrustedApps", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RestrictComputeForUntrustedApps")],
                    DetectOps = [RegOp.CheckDword(Key, "RestrictComputeForUntrustedApps", 1)],
                },
                new TweakDef
                {
                    Id = "gpucmp-enable-compute-audit",
                    Label = "Enable GPU Compute Session Audit Logging",
                    Category = "Display — Display Adapter",
                    Description =
                        "Enables audit logging of GPU compute session creation and destruction events, recording which processes open compute contexts on the GPU for security monitoring of GPU resource usage patterns.",
                    Tags = ["gpu", "compute", "audit", "event-log", "session", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "GPU compute session creation logged; process names and context types recorded for security monitoring.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableComputeSessionAudit", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableComputeSessionAudit")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableComputeSessionAudit", 1)],
                },
                new TweakDef
                {
                    Id = "gpucmp-disable-npn-compute",
                    Label = "Disable NPU Compute Offload for IntelAI / Microsoft NPU",
                    Category = "Display — Display Adapter",
                    Description =
                        "Prevents applications from using the NPU (Neural Processing Unit) for compute offload on Copilot+ and Intel AI Boost hardware, ensuring AI workloads run on the GPU or CPU where execution can be monitored and controlled.",
                    Tags = ["npu", "compute-offload", "ai", "copilot-plus", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NPU compute offload disabled; AI workloads route to GPU/CPU instead of NPU on Copilot+ hardware.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableNPUComputeOffload", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableNPUComputeOffload")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableNPUComputeOffload", 1)],
                },
                new TweakDef
                {
                    Id = "gpucmp-set-vram-reservation",
                    Label = "Reserve GPU VRAM Headroom for System Compositor",
                    Category = "Display — Display Adapter",
                    Description =
                        "Reserves a guaranteed amount of GPU VRAM for the DWM compositor and system UI rendering, preventing GPU compute and ML workloads from exhausting VRAM and causing desktop compositing failures.",
                    Tags = ["gpu", "vram", "reservation", "compositor", "stability", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "VRAM explicitly reserved for system compositor; compute workloads cannot starve DWM of display memory.",
                    ApplyOps = [RegOp.SetDword(Key, "CompositorVRAMReserveMB", 256)],
                    RemoveOps = [RegOp.DeleteValue(Key, "CompositorVRAMReserveMB")],
                    DetectOps = [RegOp.CheckDword(Key, "CompositorVRAMReserveMB", 256)],
                },
            ];
    }

    // ── GraphicsDriversPolicy ──
    private static class _GraphicsDriversPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GraphicsDrivers";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "gfxdrv-disable-dxgi-flip-model",
                Label = "Disable DXGI Flip Model Override",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableFlipModel=1 in the GraphicsDrivers policy key. Reverts DXGI "
                    + "presentation from the optimised Flip Model (DXGI_SWAP_EFFECT_FLIP_*) "
                    + "back to the legacy Blt Model for applications that do not explicitly "
                    + "request a swap-effect. Flip Model reduces present latency and input  "
                    + "lag for games; disabling it is useful only as a workaround for "
                    + "display corruption bugs in specific driver versions. Default: 0.",
                Tags = ["graphics", "dxgi", "flip-model", "display", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFlipModel", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFlipModel")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFlipModel", 1)],
            },
            new TweakDef
            {
                Id = "gfxdrv-disable-mpo",
                Label = "Disable Multi-Plane Overlay",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableMultiplaneOverlay=1 in the GraphicsDrivers policy key. "
                    + "Prevents the display engine from compositing independent window "
                    + "planes (MPO) in the GPU hardware overlay rather than in software. "
                    + "MPO is supposed to reduce GPU load and power; however, several AMD "
                    + "and NVIDIA driver families produce screen flickering, black-flash, "
                    + "or multi-monitor artefacts when MPO is enabled. Default: 0.",
                Tags = ["graphics", "mpo", "overlay", "display", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMultiplaneOverlay", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMultiplaneOverlay")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMultiplaneOverlay", 1)],
            },
            new TweakDef
            {
                Id = "gfxdrv-disable-variable-refresh",
                Label = "Disable Variable Refresh Rate (VRR/FreeSync/G-Sync) Policy",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableVariableRefreshRate=1 in the GraphicsDrivers policy key. "
                    + "Prevents VRR (FreeSync / G-Sync / Adaptive Sync) from being enabled "
                    + "system-wide via the policy layer. VRR is beneficial for gaming but "
                    + "can cause flicker artefacts on some panel firmwares during desktop "
                    + "use and transitions between windowed and full-screen modes. "
                    + "Default: 0. Recommended: 1 only on systems with affected displays.",
                Tags = ["graphics", "vrr", "freesync", "gsync", "display", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableVariableRefreshRate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableVariableRefreshRate")],
                DetectOps = [RegOp.CheckDword(Key, "DisableVariableRefreshRate", 1)],
            },
            new TweakDef
            {
                Id = "gfxdrv-disable-gpu-scheduler",
                Label = "Disable Hardware GPU Scheduler",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Sets HwSchMode=1 in the GraphicsDrivers policy key (1=disabled, 2=enabled). "
                    + "Reverts GPU command scheduling from the Windows Hardware GPU Scheduler "
                    + "(WDDM 2.7+) back to the legacy software scheduler. The hardware "
                    + "scheduler reduces latency for games on modern GPUs; however, some "
                    + "enterprise GPU drivers expose bugs in hardware scheduling that cause "
                    + "intermittent TDR (Timeout Detection and Recovery) events. Default: 2.",
                Tags = ["graphics", "gpu-scheduler", "wddm", "tdr", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "HwSchMode", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "HwSchMode")],
                DetectOps = [RegOp.CheckDword(Key, "HwSchMode", 1)],
            },
            new TweakDef
            {
                Id = "gfxdrv-disable-auto-hdr",
                Label = "Disable Auto HDR Policy Enforcement",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableAutoHdr=1 in the GraphicsDrivers policy key. Prevents "
                    + "Auto HDR from being applied to SDR games and applications at the "
                    + "system-policy level even if a user enables it in Display Settings. "
                    + "Auto HDR can degrade visual quality in games with hand-authored "
                    + "per-material palette choices not designed for HDR range expansion. "
                    + "Default: 0. Recommended: 1 for colour-critical design workstations.",
                Tags = ["graphics", "hdr", "auto-hdr", "display", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoHdr", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoHdr")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoHdr", 1)],
            },
            new TweakDef
            {
                Id = "gfxdrv-disable-dx12-resource-binding",
                Label = "Disable Experimental DX12 Resource Binding",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets DisableExperimentalResourceBinding=1 in the GraphicsDrivers policy "
                    + "key. Opts out of experimental DX12 resource binding tier extensions "
                    + "that the D3D runtime may advertise to applications on newer driver "
                    + "versions before official specification alignment. Experimental "
                    + "binding tiers can trigger spurious validation errors in debug layers "
                    + "and unexpected behaviour in strictly conforming applications. Default: 0.",
                Tags = ["graphics", "dx12", "directx", "resource-binding", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableExperimentalResourceBinding", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableExperimentalResourceBinding")],
                DetectOps = [RegOp.CheckDword(Key, "DisableExperimentalResourceBinding", 1)],
            },
            new TweakDef
            {
                Id = "gfxdrv-disable-telemetry",
                Label = "Disable Graphics Driver Telemetry",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableTelemetry=1 in the GraphicsDrivers policy key. Prevents "
                    + "the WDDM kernel-mode driver framework from emitting graphics "
                    + "performance and crash telemetry events to Microsoft's Watson and "
                    + "CEIP collection pipelines. GPU model, driver version, render API "
                    + "usage, and per-application frame-rate data are among the metrics "
                    + "that these events capture. Default: 0. Recommended: 1.",
                Tags = ["graphics", "telemetry", "privacy", "wddm", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "gfxdrv-disable-preemption",
                Label = "Disable Fine-Grained GPU Preemption",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableFineGrainedPreemption=1 in the GraphicsDrivers policy key. "
                    + "Reverts GPU command-list preemption from the fine-grained "
                    + "(per-triangle / per-dispatch) level to the coarser DMA-packet "
                    + "level supported by all WDDM 2.x GPU hardware. Fine-grained "
                    + "preemption reduces OS responsiveness latency during GPU-intensive "
                    + "workloads but can introduce micro-stutter on certain game workloads "
                    + "with high preemption rates. Default: 0.",
                Tags = ["graphics", "preemption", "wddm", "gpu", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFineGrainedPreemption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFineGrainedPreemption")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFineGrainedPreemption", 1)],
            },
            new TweakDef
            {
                Id = "gfxdrv-disable-d3d12-warp-updates",
                Label = "Disable D3D12 WARP Software Renderer Updates",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableWarpUpdates=1 in the GraphicsDrivers policy key. Blocks "
                    + "background delivery of updated WARP (Windows Advanced Rasterization "
                    + "Platform) software-renderer binaries via Windows Update. WARP "
                    + "updates are small and generally safe but represent an unplanned "
                    + "background write that can interfere with lock-down software "
                    + "inventory compliance checks. Default: 0.",
                Tags = ["graphics", "warp", "d3d12", "updates", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWarpUpdates", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWarpUpdates")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWarpUpdates", 1)],
            },
            new TweakDef
            {
                Id = "gfxdrv-disable-display-required",
                Label = "Disable Display Required Power Request Override",
                Category = "Display — Display Adapter",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableDisplayRequired=1 in the GraphicsDrivers policy key. "
                    + "Prevents the graphics subsystem from issuing a SYSTEM_REQUIRED or "
                    + "DISPLAY_REQUIRED power request that keeps the display on even when "
                    + "the power policy would otherwise dim or blank it. Some full-screen "
                    + "applications and kiosk shells issue spurious power requests; this "
                    + "policy ensures the system power governor retains control. Default: 0.",
                Tags = ["graphics", "power", "display", "sleep", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDisplayRequired", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDisplayRequired")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDisplayRequired", 1)],
            },
        ];
    }

    // ── InputMethodPolicy ──
    private static class _InputMethodPolicy
    {
        private const string IntlPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Control Panel\International";
        private const string TextInput = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TextInput";
        private const string TabletInput = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC";
        private const string ImePol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Control Panel\Desktop";
        private const string LangPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\International";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "impol-disable-language-hotkey",
                Label = "Disable Input Language Switching Hotkeys",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["ime", "language", "hotkey", "input", "keyboard", "group policy"],
                Description =
                    "Disables the keyboard shortcuts used to switch between input languages and keyboard layouts "
                    + "(typically Left-Alt+Shift and Ctrl+Shift). "
                    + "PreventHotKeyFromSwitchingInputLanguage = 1. "
                    + "Prevents accidental language switches in multilingual enterprise environments. "
                    + "Default: hotkeys enabled.",
                ApplyOps = [RegOp.SetDword(IntlPol, "PreventHotKeyFromSwitchingInputLanguage", 1)],
                RemoveOps = [RegOp.SetDword(IntlPol, "PreventHotKeyFromSwitchingInputLanguage", 0)],
                DetectOps = [RegOp.CheckDword(IntlPol, "PreventHotKeyFromSwitchingInputLanguage", 1)],
            },
            new TweakDef
            {
                Id = "impol-restrict-user-locale",
                Label = "Prevent Users from Changing System Locale",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["ime", "language", "locale", "input", "group policy", "enterprise"],
                Description =
                    "Locks the system locale and prevents standard users from changing it via Settings. "
                    + "PreventGeoIdChange = 1. Ensures consistent locale settings for enterprise software "
                    + "that relies on specific regional formats (date, number, currency). "
                    + "Default: users can change locale.",
                ApplyOps = [RegOp.SetDword(IntlPol, "PreventGeoIdChange", 1)],
                RemoveOps = [RegOp.SetDword(IntlPol, "PreventGeoIdChange", 0)],
                DetectOps = [RegOp.CheckDword(IntlPol, "PreventGeoIdChange", 1)],
            },
            new TweakDef
            {
                Id = "impol-disable-touch-keyboard-auto-show",
                Label = "Disable Touch Keyboard Auto-Show",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["touch keyboard", "input", "tablet", "ui", "group policy"],
                Description =
                    "Prevents the touch keyboard (TabTip.exe) from automatically appearing when a text field "
                    + "receives focus without a physical keyboard attached. "
                    + "AllowTouchKeyboardAutoInvokeInDesktopMode = 0. "
                    + "Recommended for touchscreen PCs used in environments where the keyboard should not pop up "
                    + "automatically (kiosk / custom application deployments). Default: auto-show enabled.",
                ApplyOps = [RegOp.SetDword(TextInput, "AllowTouchKeyboardAutoInvokeInDesktopMode", 0)],
                RemoveOps = [RegOp.DeleteValue(TextInput, "AllowTouchKeyboardAutoInvokeInDesktopMode")],
                DetectOps = [RegOp.CheckDword(TextInput, "AllowTouchKeyboardAutoInvokeInDesktopMode", 0)],
            },
            new TweakDef
            {
                Id = "impol-disable-input-personalisation",
                Label = "Disable Input Personalisation Data Collection",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["ime", "input", "personalisation", "privacy", "telemetry", "group policy"],
                Description =
                    "Disables the collection of typing and handwriting data used to personalise "
                    + "autocorrect, handwriting recognition, and speech. "
                    + "RestrictImplicitTextCollection = 1. "
                    + "Prevents the input personalisation service from accumulating keystroke metadata "
                    + "in %APPDATA%\\Microsoft\\InputPersonalization. Default: collection enabled.",
                ApplyOps = [RegOp.SetDword(TextInput, "AllowLinguisticDataCollection", 0)],
                RemoveOps = [RegOp.DeleteValue(TextInput, "AllowLinguisticDataCollection")],
                DetectOps = [RegOp.CheckDword(TextInput, "AllowLinguisticDataCollection", 0)],
            },
            new TweakDef
            {
                Id = "impol-disable-tablet-mode-switch",
                Label = "Disable Automatic Tablet Mode Switching",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["tablet", "input", "ui", "touchscreen", "group policy"],
                Description =
                    "Prevents Windows from automatically switching to Tablet Mode when a keyboard "
                    + "is detached (convertible 2-in-1 devices). "
                    + "DisableTabletModeChangeDialog = 1. "
                    + "Keeps the desktop mode consistently active regardless of hardware configuration. "
                    + "Useful for enterprise convertible deployments running desktop-only LOB applications.",
                ApplyOps = [RegOp.SetDword(TabletInput, "PreventTabletMode", 1)],
                RemoveOps = [RegOp.SetDword(TabletInput, "PreventTabletMode", 0)],
                DetectOps = [RegOp.CheckDword(TabletInput, "PreventTabletMode", 1)],
            },
            new TweakDef
            {
                Id = "impol-disable-handwriting-sharing",
                Label = "Disable Handwriting Model Data Sharing with Microsoft",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["ime", "handwriting", "privacy", "telemetry", "input", "group policy"],
                Description =
                    "Prevents handwriting recognition training data from being shared with Microsoft. "
                    + "AllowHandwritingLMUpdate = 0. "
                    + "Handwriting strokes and corrected words are not sent to Microsoft's cloud model. "
                    + "Default: sharing enabled to improve handwriting recognition. "
                    + "Recommended for devices that process sensitive handwritten data.",
                ApplyOps = [RegOp.SetDword(TextInput, "AllowHandwritingLMUpdate", 0)],
                RemoveOps = [RegOp.DeleteValue(TextInput, "AllowHandwritingLMUpdate")],
                DetectOps = [RegOp.CheckDword(TextInput, "AllowHandwritingLMUpdate", 0)],
            },
            new TweakDef
            {
                Id = "impol-disable-emoji-panel",
                Label = "Disable Emoji Panel (Win+.)",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Tags = ["emoji", "input", "ui", "productivity", "group policy"],
                Description =
                    "Disables the Emoji Panel popup triggered by Win+. (period) or Win+; (semicolon). "
                    + "DisableEmojiInput = 1. "
                    + "Removes a non-essential UI element in locked-down or productivity-focused deployments. "
                    + "Default: Emoji Panel enabled. Symbols/emoji can still be inserted via other methods.",
                ApplyOps = [RegOp.SetDword(TabletInput, "DisableEmojiInput", 1)],
                RemoveOps = [RegOp.SetDword(TabletInput, "DisableEmojiInput", 0)],
                DetectOps = [RegOp.CheckDword(TabletInput, "DisableEmojiInput", 1)],
            },
            new TweakDef
            {
                Id = "impol-block-ime-network",
                Label = "Block IME from Accessing Network Resources",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["ime", "network", "security", "input", "group policy"],
                Description =
                    "Prevents Input Method Editor (IME) processes from accessing network resources. "
                    + "BlockImePlaceholder = 1. Stops third-party or built-in IMEs from making "
                    + "cloud-based word prediction or dictionary queries. "
                    + "Relevant for CJK (Chinese, Japanese, Korean) input environments with privacy requirements.",
                ApplyOps = [RegOp.SetDword(TextInput, "AllowInputDeviceUserInterface", 0)],
                RemoveOps = [RegOp.DeleteValue(TextInput, "AllowInputDeviceUserInterface")],
                DetectOps = [RegOp.CheckDword(TextInput, "AllowInputDeviceUserInterface", 0)],
            },
            new TweakDef
            {
                Id = "impol-disable-voice-typing",
                Label = "Disable Voice Typing (Win+H Dictation)",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["voice typing", "dictation", "speech", "privacy", "input", "group policy"],
                Description =
                    "Disables the voice dictation feature accessible via Win+H. "
                    + "AllowVoiceTyping = 0. "
                    + "Prevents unintended microphone activation via keyboard shortcut in shared or secure environments. "
                    + "Distinct from disabling Cortana voice — this only blocks the dictation shortcut. "
                    + "Default: voice typing enabled on supported hardware.",
                MinBuild = 22000,
                ApplyOps = [RegOp.SetDword(TextInput, "AllowVoiceTyping", 0)],
                RemoveOps = [RegOp.DeleteValue(TextInput, "AllowVoiceTyping")],
                DetectOps = [RegOp.CheckDword(TextInput, "AllowVoiceTyping", 0)],
            },
            new TweakDef
            {
                Id = "impol-disable-cursor-thickness-change",
                Label = "Prevent Users from Changing Cursor Pointer Size",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Tags = ["input", "cursor", "accessibility", "ui", "kiosk", "group policy"],
                Description =
                    "Prevents users from changing the mouse pointer size and colour scheme in Settings. "
                    + "NoPointerSettings = 1 via Desktop policy. "
                    + "Ensures consistent cursor appearance across all user sessions in kiosk and shared PC deployments. "
                    + "Default: users can change pointer size.",
                ApplyOps = [RegOp.SetDword(ImePol, "NoPointerSettings", 1)],
                RemoveOps = [RegOp.SetDword(ImePol, "NoPointerSettings", 0)],
                DetectOps = [RegOp.CheckDword(ImePol, "NoPointerSettings", 1)],
            },
        ];
    }

    // ── InputPersonalizationPolicy ──
    private static class _InputPersonalizationPolicy
    {
        private const string IpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "inpp-restrict-ink-collection",
                    Label = "Restrict Implicit Ink Collection",
                    Category = "Display — Input Method",
                    Description =
                        "Sets RestrictImplicitInkCollection=1 to prevent Windows from collecting ink strokes silently in the background for personalisation purposes. Default: 0 (collection allowed). Recommended for privacy: 1.",
                    Tags = ["input", "ink", "collection", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Stops background ink data collection; handwriting recognition remains functional.",
                    ApplyOps = [RegOp.SetDword(IpKey, "RestrictImplicitInkCollection", 1)],
                    RemoveOps = [RegOp.DeleteValue(IpKey, "RestrictImplicitInkCollection")],
                    DetectOps = [RegOp.CheckDword(IpKey, "RestrictImplicitInkCollection", 1)],
                },
                new TweakDef
                {
                    Id = "inpp-disable-inking-keyboard-personalization",
                    Label = "Disable Inking and Typing Personalization",
                    Category = "Display — Input Method",
                    Description =
                        "Sets AllowInkingAndTypingPersonalization=0 to block the inking and typing personalisation feature that builds a personal dictionary from writing samples. Prevents sharing of handwriting/typing data with Microsoft.",
                    Tags = ["input", "inking", "typing", "personalisation", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Disables inking and typing personalisation; no personal word list built from user data.",
                    ApplyOps = [RegOp.SetDword(IpKey, "AllowInkingAndTypingPersonalization", 0)],
                    RemoveOps = [RegOp.DeleteValue(IpKey, "AllowInkingAndTypingPersonalization")],
                    DetectOps = [RegOp.CheckDword(IpKey, "AllowInkingAndTypingPersonalization", 0)],
                },
                new TweakDef
                {
                    Id = "inpp-disable-user-dictionary",
                    Label = "Disable Cloud User Dictionary Sync",
                    Category = "Display — Input Method",
                    Description =
                        "Sets AllowUserDictionary=0 to prevent the personalised input dictionary from being synced with Microsoft servers. Local dictionary still functions; cloud backup and cross-device sync are blocked.",
                    Tags = ["input", "dictionary", "sync", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Disables dictionary cloud sync; local autocorrect and custom words still work on this device.",
                    ApplyOps = [RegOp.SetDword(IpKey, "AllowUserDictionary", 0)],
                    RemoveOps = [RegOp.DeleteValue(IpKey, "AllowUserDictionary")],
                    DetectOps = [RegOp.CheckDword(IpKey, "AllowUserDictionary", 0)],
                },
                new TweakDef
                {
                    Id = "inpp-disable-ink-learning",
                    Label = "Disable Ink Recognition Learning",
                    Category = "Display — Input Method",
                    Description =
                        "Sets AllowInkRecognitionLearning=0 to prevent the handwriting recogniser from learning and adapting to this user's writing style over time. Useful on shared or kiosk devices where per-user learning is undesirable.",
                    Tags = ["input", "ink", "recognition", "learning", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Static handwriting recogniser; does not adapt to individual writing style.",
                    ApplyOps = [RegOp.SetDword(IpKey, "AllowInkRecognitionLearning", 0)],
                    RemoveOps = [RegOp.DeleteValue(IpKey, "AllowInkRecognitionLearning")],
                    DetectOps = [RegOp.CheckDword(IpKey, "AllowInkRecognitionLearning", 0)],
                },
                new TweakDef
                {
                    Id = "inpp-disable-text-prediction",
                    Label = "Disable Cloud-Based Text Prediction",
                    Category = "Display — Input Method",
                    Description =
                        "Sets AllowTextPrediction=0 to disable cloud-assisted text prediction and autocomplete features. Local offline prediction is unaffected. Reduces data transmission associated with keyboard input.",
                    Tags = ["input", "text prediction", "autocomplete", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Cloud-based prediction disabled; offline autocomplete may still function if enabled locally.",
                    ApplyOps = [RegOp.SetDword(IpKey, "AllowTextPrediction", 0)],
                    RemoveOps = [RegOp.DeleteValue(IpKey, "AllowTextPrediction")],
                    DetectOps = [RegOp.CheckDword(IpKey, "AllowTextPrediction", 0)],
                },
                new TweakDef
                {
                    Id = "inpp-disable-linguistic-collection",
                    Label = "Disable Linguistic Data Collection",
                    Category = "Display — Input Method",
                    Description =
                        "Sets AllowLinguisticDataCollection=0 to block Windows from sending linguistic data (autocorrect feedback, text samples) to Microsoft for improving language models. Complementary to RestrictImplicitTextCollection.",
                    Tags = ["input", "linguistic", "collection", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "No linguistic samples sent to cloud; comprehensive typing privacy measure.",
                    ApplyOps = [RegOp.SetDword(IpKey, "AllowLinguisticDataCollection", 0)],
                    RemoveOps = [RegOp.DeleteValue(IpKey, "AllowLinguisticDataCollection")],
                    DetectOps = [RegOp.CheckDword(IpKey, "AllowLinguisticDataCollection", 0)],
                },
                new TweakDef
                {
                    Id = "inpp-disable-handwriting-telemetry",
                    Label = "Disable Handwriting Error Reporting",
                    Category = "Display — Input Method",
                    Description =
                        "Sets AllowHandwritingErrorReports=0 to prevent the handwriting recognition engine from sending error reports and misrecognition samples to Microsoft. Reduces telemetry from pen-enabled devices.",
                    Tags = ["input", "handwriting", "telemetry", "reporting", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Handwriting error data not sent to Microsoft; local recognition unaffected.",
                    ApplyOps = [RegOp.SetDword(IpKey, "AllowHandwritingErrorReports", 0)],
                    RemoveOps = [RegOp.DeleteValue(IpKey, "AllowHandwritingErrorReports")],
                    DetectOps = [RegOp.CheckDword(IpKey, "AllowHandwritingErrorReports", 0)],
                },
                new TweakDef
                {
                    Id = "inpp-disable-input-data-upload",
                    Label = "Disable Input Data Upload to Microsoft",
                    Category = "Display — Input Method",
                    Description =
                        "Sets AllowInputDataUpload=0 to prevent Windows from uploading any collected input personalisation data (ink, text, voice) to Microsoft's servers. Applies a blanket block on all input-related cloud data transmission.",
                    Tags = ["input", "upload", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Comprehensive block on all input data uploads; all local input features remain functional.",
                    ApplyOps = [RegOp.SetDword(IpKey, "AllowInputDataUpload", 0)],
                    RemoveOps = [RegOp.DeleteValue(IpKey, "AllowInputDataUpload")],
                    DetectOps = [RegOp.CheckDword(IpKey, "AllowInputDataUpload", 0)],
                },
            ];
    }

    // ── KioskAssignedAccess ──
    private static class _KioskAssignedAccess
    {
        private const string SysPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
        private const string WinPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
        private const string AppCompatPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat";
        private const string GpoSys = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization";
        private const string ExplorerPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "kiosk-disable-task-manager",
                Label = "Kiosk: Disable Task Manager",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SysPolicy],
                Tags = ["kiosk", "lockdown", "task-manager", "policy"],
                Description =
                    "Sets DisableTaskMgr=1 in Policies\\System. Prevents users from opening Task Manager "
                    + "via Ctrl+Shift+Esc or right-clicking the taskbar. "
                    + "Default: Task Manager accessible. Recommended for shared/kiosk machines.",
                ApplyOps = [RegOp.SetDword(SysPolicy, "DisableTaskMgr", 1)],
                RemoveOps = [RegOp.DeleteValue(SysPolicy, "DisableTaskMgr")],
                DetectOps = [RegOp.CheckDword(SysPolicy, "DisableTaskMgr", 1)],
            },
            new TweakDef
            {
                Id = "kiosk-disable-registry-editor",
                Label = "Kiosk: Disable Registry Editor Access",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SysPolicy],
                Tags = ["kiosk", "lockdown", "registry-editor", "policy"],
                Description =
                    "Sets DisableRegistryTools=1 in Policies\\System. Blocks users from running regedit.exe. "
                    + "Prevents tampering with registry settings on shared or public machines. "
                    + "Default: regedit accessible to non-admin users in their hive.",
                ApplyOps = [RegOp.SetDword(SysPolicy, "DisableRegistryTools", 1)],
                RemoveOps = [RegOp.DeleteValue(SysPolicy, "DisableRegistryTools")],
                DetectOps = [RegOp.CheckDword(SysPolicy, "DisableRegistryTools", 1)],
            },
            new TweakDef
            {
                Id = "kiosk-disable-cmd",
                Label = "Kiosk: Disable Command Prompt for Standard Users",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SysPolicy],
                Tags = ["kiosk", "lockdown", "cmd", "command-prompt", "policy"],
                Description =
                    "Sets DisableCMD=2 in Policies\\System. Prevents users from running cmd.exe or batch files. "
                    + "Value 2 also disables batch file execution. "
                    + "Default: cmd accessible. Kiosk/shared-device hardening.",
                ApplyOps = [RegOp.SetDword(SysPolicy, "DisableCMD", 2)],
                RemoveOps = [RegOp.DeleteValue(SysPolicy, "DisableCMD")],
                DetectOps = [RegOp.CheckDword(SysPolicy, "DisableCMD", 2)],
            },
            new TweakDef
            {
                Id = "kiosk-disable-run-dialog",
                Label = "Kiosk: Remove Run Dialog from Start Menu",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ExplorerPolicy],
                Tags = ["kiosk", "lockdown", "run-dialog", "shell", "policy"],
                Description =
                    "Sets NoRun=1 in Explorer policy. Removes the Run entry from the Start menu and "
                    + "disables the Win+R keyboard shortcut. "
                    + "Default: Run dialog accessible. Prevents launching arbitrary executables on kiosk.",
                ApplyOps = [RegOp.SetDword(ExplorerPolicy, "NoRun", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplorerPolicy, "NoRun")],
                DetectOps = [RegOp.CheckDword(ExplorerPolicy, "NoRun", 1)],
            },
            new TweakDef
            {
                Id = "kiosk-lock-lock-screen",
                Label = "Kiosk: Prevent Changing Lock Screen Image via Policy",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [GpoSys],
                Tags = ["kiosk", "lockscreen", "personalization", "policy"],
                Description =
                    "Sets NoChangingLockScreen=1 in Personalization policy. Prevents users from changing the "
                    + "lock screen image. Preserves corporate/kiosk branding. "
                    + "Default: user can change the lock screen. Recommended for corporate shared machines.",
                ApplyOps = [RegOp.SetDword(GpoSys, "NoChangingLockScreen", 1)],
                RemoveOps = [RegOp.DeleteValue(GpoSys, "NoChangingLockScreen")],
                DetectOps = [RegOp.CheckDword(GpoSys, "NoChangingLockScreen", 1)],
            },
            new TweakDef
            {
                Id = "kiosk-disable-camera-on-lockscreen",
                Label = "Kiosk: Disable Camera Access from Lock Screen",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [GpoSys],
                Tags = ["kiosk", "lockscreen", "camera", "privacy", "policy"],
                Description =
                    "Sets AllowCameraOnLockScreen=0 in Personalization policy (GPO). Prevents the lock screen "
                    + "camera shortcut from activating the camera without authentication. "
                    + "Default: camera accessible from lock screen. Privacy-relevant for unattended public terminals.",
                ApplyOps = [RegOp.SetDword(GpoSys, "AllowCameraOnLockScreen", 0)],
                RemoveOps = [RegOp.DeleteValue(GpoSys, "AllowCameraOnLockScreen")],
                DetectOps = [RegOp.CheckDword(GpoSys, "AllowCameraOnLockScreen", 0)],
            },
            new TweakDef
            {
                Id = "kiosk-prevent-logoff-shutdown",
                Label = "Kiosk: Remove Shut Down and Restart from Start Menu",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ExplorerPolicy],
                Tags = ["kiosk", "shutdown", "restart", "lockdown", "policy"],
                Description =
                    "Sets NoClose=1 in Explorer policy. Removes the Shut Down, Restart, Sleep, and Log Off "
                    + "options from the Start menu. Users cannot initiate system shutdown. "
                    + "Default: all power options visible. Useful for kiosk sessions requiring admin to power off.",
                ApplyOps = [RegOp.SetDword(ExplorerPolicy, "NoClose", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplorerPolicy, "NoClose")],
                DetectOps = [RegOp.CheckDword(ExplorerPolicy, "NoClose", 1)],
            },
        ];
    }

    // ── KioskBrowserPolicy ──
    private static class _KioskBrowserPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\KioskBrowser";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "kiosk-enable-kiosk-mode",
                Label = "Enable Kiosk Browser Lockdown Mode",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Kiosk browser lockdown mode restricts the browser to a limited interface preventing navigation to unauthorized sites and blocking browser customization. Enabling kiosk mode creates a purpose-specific browsing experience suitable for public terminals, self-service kiosks, and restricted endpoint deployments. Kiosk mode removes the browser address bar, navigation history, developer tools, and other features that could be used to browse unauthorized content. Lockdown mode prevents end users of kiosk terminals from accessing sensitive information or performing unauthorized actions through the browser. Kiosk browser policies integrate with the Windows assigned access feature for single-app kiosk deployments. Organizations deploying kiosk endpoints should combine kiosk browser mode with OS-level kiosk configuration to prevent breakouts.",
                Tags = ["kiosk", "browser", "lockdown", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableKioskMode", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableKioskMode")],
                DetectOps = [RegOp.CheckDword(Key, "EnableKioskMode", 1)],
            },
            new TweakDef
            {
                Id = "kiosk-restrict-allowed-urls",
                Label = "Restrict Kiosk Browser to Allowed URLs Only",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "URL allow-listing restricts kiosk browser navigation to only the websites explicitly approved for kiosk use preventing browsing to unauthorized content. Restricting to allowed URLs creates a whitelist model ensuring kiosk users can only access the specific web applications intended for the deployment. URL restrictions prevent kiosk users from navigating away from authorized applications to social media, external services, or malicious sites. Allowed URL patterns support wildcard matching to accommodate web applications with multiple pages and dynamic URLs. URL restriction enforcement prevents social engineering attacks against kiosk users who might be directed to malicious sites. Allowed URL lists should be maintained and reviewed regularly to ensure only currently needed URLs are included.",
                Tags = ["kiosk", "url-allowlist", "browsing-restriction", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetString(Key, "AllowedUrls", "about:blank")],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowedUrls")],
                DetectOps = [RegOp.CheckString(Key, "AllowedUrls", "about:blank")],
            },
            new TweakDef
            {
                Id = "kiosk-disable-browser-extensions",
                Label = "Disable Browser Extensions in Kiosk Mode",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Browser extensions in kiosk mode could be used to bypass URL restrictions, capture user input, or perform unauthorized actions outside the kiosk purpose. Disabling extensions in kiosk mode ensures that the browser operates in a clean state without third-party code that could compromise kiosk security. Extensions have access to browser internals and could intercept communications between the kiosk user and the target web application. Kiosk deployments should never have user-installed extensions as these are not validated for the kiosk security model. Organizations that require functionality typically provided by extensions should integrate it into the target web application instead. Extension disablement in kiosk mode is a fundamental security requirement for any public-facing terminal deployment.",
                Tags = ["kiosk", "extensions", "browser-security", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableExtensions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableExtensions")],
                DetectOps = [RegOp.CheckDword(Key, "DisableExtensions", 1)],
            },
            new TweakDef
            {
                Id = "kiosk-enable-end-session-button",
                Label = "Enable End Session Button in Kiosk Browser",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "The end session button allows kiosk users to reset the browser session and clear any data from their interaction before the next user accesses the terminal. Enabling the end session button provides a privacy protection mechanism for kiosk users who can explicitly terminate their session after use. Session data including form inputs, cookies, and browsing history from one kiosk user should not be accessible to the next user. The end session button resets the browser to its initial state clearing all user-specific data from the current session. Kiosk terminals without session reset capability risk exposing previous user data to subsequent users creating privacy and security risks. Session reset should also be triggered automatically after a configurable idle period to handle cases where users forget to end their session.",
                Tags = ["kiosk", "session-management", "privacy", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ShowEndSessionButton", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ShowEndSessionButton")],
                DetectOps = [RegOp.CheckDword(Key, "ShowEndSessionButton", 1)],
            },
            new TweakDef
            {
                Id = "kiosk-configure-idle-timeout-reset",
                Label = "Configure Kiosk Browser Idle Timeout Session Reset",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Idle timeout session reset automatically clears kiosk browser state when the terminal has been inactive for a specified period. Configuring idle timeout ensures that abandoned kiosk sessions are automatically cleared preventing the next user from accessing previous user data. Public kiosks require automatic session reset because many users will walk away without explicitly ending their session. Idle reset removes form inputs, session cookies, authentication tokens, and browsing history from abandoned sessions. The idle timeout period should be set to balance user experience with security — too short disrupts slow users while too long exposes session data. Idle reset notifications can warn users before reset occurs giving them time to complete their task or explicitly end the session.",
                Tags = ["kiosk", "idle-timeout", "session-reset", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "IdleTimeInMinutes", 5)],
                RemoveOps = [RegOp.DeleteValue(Key, "IdleTimeInMinutes")],
                DetectOps = [RegOp.CheckDword(Key, "IdleTimeInMinutes", 5)],
            },
            new TweakDef
            {
                Id = "kiosk-block-popups",
                Label = "Block Pop-up Windows in Kiosk Browser",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Pop-up windows in kiosk mode can be used to open unauthorized websites, display misleading content, or trick kiosk users into entering information in malicious forms. Blocking pop-ups in kiosk mode ensures that web applications cannot open secondary browser windows that circumvent URL allow-listing restrictions. Pop-up blocking prevents JavaScript-based navigation attacks where a website attempts to open a new window pointing to a restricted or malicious URL. Web-based popups could also be used to confuse kiosk users and potentially capture sensitive information they believe is part of the legitimate application. Kiosk web applications should be designed to function without pop-ups using in-page modal dialogs instead of spawning separate windows. Pop-up blocking is a fundamental defensive control for any kiosk deployment alongside URL allowlisting.",
                Tags = ["kiosk", "popups", "browser-security", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockPopups", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockPopups")],
                DetectOps = [RegOp.CheckDword(Key, "BlockPopups", 1)],
            },
            new TweakDef
            {
                Id = "kiosk-disable-developer-tools",
                Label = "Disable Developer Tools in Kiosk Browser",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Browser developer tools provide JavaScript console access, DOM inspection, network traffic monitoring, and storage inspection capabilities in kiosk browsers. Disabling developer tools prevents sophisticated kiosk users from manipulating the kiosk web application through JavaScript injection or DOM modification. Developer tool access in kiosk mode could allow users to extract authentication tokens, modify application behavior, or access restricted application functionality. The console in developer tools provides full JavaScript execution capability against the current page context creating serious security risks in kiosk environments. Developer tools access in kiosk mode could be used to exfiltrate data that the kiosk application handles on behalf of users. Developers should have access to developer tools only in isolated kiosk development environments, never in production kiosk deployments.",
                Tags = ["kiosk", "developer-tools", "browser-security", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDeveloperTools", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDeveloperTools")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDeveloperTools", 1)],
            },
            new TweakDef
            {
                Id = "kiosk-disable-download-manager",
                Label = "Disable Browser Download Manager in Kiosk Mode",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "The browser download manager in kiosk mode allows users to save files from web applications to the local kiosk terminal file system. Disabling downloads in kiosk mode prevents data exfiltration from kiosk applications and blocks malware delivery through drive-by downloads. Downloaded files on kiosk terminals persist between sessions potentially storing sensitive user data or malicious executables. Kiosk applications that need to provide documents to users should deliver them through a controlled print workflow rather than file downloads. Blocking download manager access prevents kiosk users from using the terminal as a personal storage device for external files. Download blocking should be enforced at both the browser policy level and the file system permission level for defense-in-depth.",
                Tags = ["kiosk", "downloads", "data-control", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDownloadManager", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDownloadManager")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDownloadManager", 1)],
            },
            new TweakDef
            {
                Id = "kiosk-clear-session-data-on-exit",
                Label = "Clear All Session Data When Kiosk Browser Restarts",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Clearing session data on kiosk browser restart removes all cookies, cache, browsing history, and session storage accumulated during the previous usage session. Clearing data on restart ensures that kiosk terminals always start in a clean state regardless of what the previous user did. Residual session data including authentication cookies could allow the next kiosk user to inherit an authenticated session from the previous user. Session data clearing prevents fingerprinting data accumulated during previous sessions from being used to track users or correlate sessions. Kiosk terminals in high-traffic environments may see hundreds of users per day making automatic data clearing essential for user privacy. Session clearing should be verified by testing that login credentials from previous sessions are not auto-filled for subsequent kiosk users.",
                Tags = ["kiosk", "session-data", "privacy", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DeleteBrowsingDataOnClose", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DeleteBrowsingDataOnClose")],
                DetectOps = [RegOp.CheckDword(Key, "DeleteBrowsingDataOnClose", 1)],
            },
            new TweakDef
            {
                Id = "kiosk-disable-address-bar",
                Label = "Hide Address Bar in Kiosk Browser",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "The browser address bar allows kiosk users to type arbitrary URLs and navigate to sites that may not be included in the approved URL allowlist. Hiding the address bar removes the primary user interface element for unauthorized navigation preventing kiosk users from typing arbitrary destination URLs. Address bar restriction combined with URL allowlisting provides a defense-in-depth approach since even if one control is bypassed the other provides protection. Hiding the address bar also reduces the kiosk terminal interface to only the essential web application elements improving the user experience for intended kiosk functions. URL allowlisting through registry policy should also be enforced independently from the address bar visibility as the bar can still be accessed through keyboard shortcuts. Address bar hiding is a usability and defense-in-depth control that works best as part of a comprehensive kiosk security configuration.",
                Tags = ["kiosk", "address-bar", "navigation-control", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "HideAddressBar", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "HideAddressBar")],
                DetectOps = [RegOp.CheckDword(Key, "HideAddressBar", 1)],
            },
        ];
    }

    // ── LanguageOptionsPolicy ──
    private static class _LanguageOptionsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanguageOptions";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "langopt-disable-language-pack-install",
                Label = "Block User Language Pack Installation",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets BlockUserFromAddingLanguages=1 in the LanguageOptions policy key. "
                    + "Prevents standard users from adding new display languages or keyboard "
                    + "layouts via Settings > Time & Language. Uncontrolled language additions "
                    + "on shared or managed machines can change locale-dependent settings "
                    + "and alter application behaviour for other users. "
                    + "Default: 0. Recommended: 1 on managed workstations.",
                Tags = ["language", "pack", "locale", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockUserFromAddingLanguages", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockUserFromAddingLanguages")],
                DetectOps = [RegOp.CheckDword(Key, "BlockUserFromAddingLanguages", 1)],
            },
            new TweakDef
            {
                Id = "langopt-restrict-language-change",
                Label = "Restrict Language Display Setting Change",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets RestrictLanguageChange=1 in the LanguageOptions policy key. Prevents "
                    + "non-administrative users from changing the Windows display language that "
                    + "governs UI strings, dialogs, and menu text. Allowing arbitrary language "
                    + "switches on shared terminals create accessibility and compliance issues "
                    + "for organisations that require a fixed locale for audit logs. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["language", "display", "locale", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictLanguageChange", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictLanguageChange")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictLanguageChange", 1)],
            },
            new TweakDef
            {
                Id = "langopt-disable-ime-telemetry",
                Label = "Disable IME Telemetry Upload",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets ImeTelemetryEnabled=0 in the LanguageOptions policy key. Stops the "
                    + "Windows Input Method Editor (IME) from transmitting usage data including "
                    + "candidate selection frequency, dictionary look-up patterns, and "
                    + "conversion correction events to Microsoft. IME telemetry can indirectly "
                    + "reveal content typed when using CJK or other IME-based input. "
                    + "Default: 1. Recommended: 0.",
                Tags = ["ime", "telemetry", "privacy", "language", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ImeTelemetryEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ImeTelemetryEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "ImeTelemetryEnabled", 0)],
            },
            new TweakDef
            {
                Id = "langopt-disable-cloud-candidate",
                Label = "Disable IME Cloud Candidate Lookup",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets BlockCloudCandidates=1 in the LanguageOptions policy key. Prevents "
                    + "the Chinese, Japanese, and Korean IME engines from sending keystrokes "
                    + "to Microsoft's cloud candidate service to retrieve extended candidate "
                    + "lists. Cloud candidate queries transmit partial words to an external "
                    + "endpoint for every keystroke, creating a persistent privacy exposure. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["ime", "cloud", "candidate", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockCloudCandidates", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockCloudCandidates")],
                DetectOps = [RegOp.CheckDword(Key, "BlockCloudCandidates", 1)],
            },
            new TweakDef
            {
                Id = "langopt-disable-handwriting-recognition-improvement",
                Label = "Disable Language Handwriting Recognition Improvement",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets BlockHandwritingImprovementProgram=1 in the LanguageOptions policy "
                    + "key. Prevents Windows from enrolling the device in the handwriting "
                    + "recognition improvement programme which collects ink samples. Collected "
                    + "samples include handwritten characters from all users on the machine and "
                    + "are submitted to Microsoft's recognition training pipeline. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["handwriting", "ink", "improvement", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockHandwritingImprovementProgram", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockHandwritingImprovementProgram")],
                DetectOps = [RegOp.CheckDword(Key, "BlockHandwritingImprovementProgram", 1)],
            },
            new TweakDef
            {
                Id = "langopt-disable-ocr-telemetry",
                Label = "Disable Language OCR Telemetry",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets OcrTelemetryEnabled=0 in the LanguageOptions policy key. Prevents "
                    + "the Windows OCR engine from submitting recognition quality metrics and "
                    + "anonymised document-structure statistics to Microsoft. OCR telemetry "
                    + "accumulates document-type patterns that may reveal the nature of "
                    + "business documents processed by the machine. "
                    + "Default: 1. Recommended: 0.",
                Tags = ["ocr", "telemetry", "language", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "OcrTelemetryEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "OcrTelemetryEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "OcrTelemetryEnabled", 0)],
            },
            new TweakDef
            {
                Id = "langopt-disable-speech-recognition-telemetry",
                Label = "Disable Speech Recognition Language Telemetry",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets SpeechRecognitionTelemetryEnabled=0 in the LanguageOptions policy "
                    + "key. Stops Windows Speech Recognition from uploading speech-model "
                    + "adaptation data and recognition-error patterns. Adaptation data encodes "
                    + "voice characteristics unique to the primary user and could be used for "
                    + "speaker profiling in combination with other datasets. "
                    + "Default: 1. Recommended: 0.",
                Tags = ["speech", "recognition", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SpeechRecognitionTelemetryEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SpeechRecognitionTelemetryEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "SpeechRecognitionTelemetryEnabled", 0)],
            },
            new TweakDef
            {
                Id = "langopt-disable-keyboard-telemetry",
                Label = "Disable Keyboard Layout Telemetry",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets KeyboardTelemetryEnabled=0 in the LanguageOptions policy key. "
                    + "Prevents Windows from submitting keyboard-layout switch events and "
                    + "active-layout usage frequency to Microsoft. Keyboard-layout activity "
                    + "data reveals which languages a user is communicating in and at what "
                    + "frequency across sessions. "
                    + "Default: 1. Recommended: 0.",
                Tags = ["keyboard", "layout", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "KeyboardTelemetryEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "KeyboardTelemetryEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "KeyboardTelemetryEnabled", 0)],
            },
            new TweakDef
            {
                Id = "langopt-disable-language-online-update",
                Label = "Disable Automatic Language Pack Online Update",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets BlockLanguagePackUpdatesFromWindowsUpdate=1 in the LanguageOptions "
                    + "policy key. Stops Windows from automatically downloading updated language "
                    + "packs and locale-data files from Windows Update without administrator "
                    + "approval. Unexpected language-pack updates can change font-rendering, "
                    + "date formats, and input method behaviour mid-session. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["language", "update", "windowsupdate", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockLanguagePackUpdatesFromWindowsUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockLanguagePackUpdatesFromWindowsUpdate")],
                DetectOps = [RegOp.CheckDword(Key, "BlockLanguagePackUpdatesFromWindowsUpdate", 1)],
            },
            new TweakDef
            {
                Id = "langopt-disable-language-sync",
                Label = "Disable Language Settings Sync",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DoNotSyncLanguageSettings=1 in the LanguageOptions policy key. "
                    + "Prevents language preferences, keyboard layout order, and regional "
                    + "settings from synchronising to the cloud and propagating to other devices "
                    + "connected to the same Microsoft account. Language settings sync can "
                    + "wrongly reconfigure keyboards on shared machines. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["language", "sync", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DoNotSyncLanguageSettings", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DoNotSyncLanguageSettings")],
                DetectOps = [RegOp.CheckDword(Key, "DoNotSyncLanguageSettings", 1)],
            },
        ];
    }

    // ── MobilityCenterPolicy ──
    private static class _MobilityCenterPolicy
    {
        private const string MobLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\MobilityCenter";
        private const string MobCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\MobilityCenter";
        private const string ExplLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Policies\Explorer";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "mob-disable-mobility-center",
                Label = "Disable Windows Mobility Center (Machine)",
                Category = "Display — Input Method",
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
                Category = "Display — Input Method",
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
                Category = "Display — Input Method",
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
                Category = "Display — Input Method",
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
                Category = "Display — Input Method",
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
                Category = "Display — Input Method",
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
                Category = "Display — Input Method",
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
                Category = "Display — Input Method",
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
                Category = "Display — Input Method",
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
                Category = "Display — Input Method",
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
}
