namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyDesktop
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

    // ── ModernStandbyPolicy ──
    private static class _ModernStandbyPolicy
    {
        private const string MsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ModernStandby";
        private const string PwrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power\PowerSettings";
        private const string PwrSleepKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "mstandby-disable-connected-standby",
                    Label = "Disable Modern Standby (S0 Low-Power Idle) — Use S3 Sleep",
                    Category = "Display — Input Method",
                    Description =
                        "Disables Modern Standby (S0ix) and falls back to the traditional S3 sleep state. S0 keeps the network and background apps active during sleep, which can interfere with security tools, drain battery unexpectedly, and create wake-on-network attack surfaces.",
                    Tags = ["modern-standby", "s0", "s3-sleep", "power", "disable"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Forces S3 sleep where hardware supports it. Network and background activity cease during sleep — improves battery life on older HW but disables instant-on and wake-on-LAN in S0. Some OEM hardware only supports S0 and cannot fall back.",
                    RegistryKeys = [MsKey],
                    ApplyOps = [RegOp.SetDword(MsKey, "AllowStandby", 0)],
                    RemoveOps = [RegOp.DeleteValue(MsKey, "AllowStandby")],
                    DetectOps = [RegOp.CheckDword(MsKey, "AllowStandby", 0)],
                },
                new TweakDef
                {
                    Id = "mstandby-block-network-during-standby",
                    Label = "Block Network Activity During Modern Standby",
                    Category = "Display — Input Method",
                    Description =
                        "Prevents the NIC from remaining active and processing network packets while the device is in Modern Standby. Reduces the attack surface from wake-on-LAN exploitation, rogue DHCP offers, and directed broadcast attacks arriving while the user is away.",
                    Tags = ["modern-standby", "network", "wifi", "attack-surface", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Push notifications, live tiles, and scheduled background sync will not occur while the device is in standby. Recommended for shared, high-security environments.",
                    RegistryKeys = [MsKey],
                    ApplyOps = [RegOp.SetDword(MsKey, "NetworkActivityAllowed", 0)],
                    RemoveOps = [RegOp.DeleteValue(MsKey, "NetworkActivityAllowed")],
                    DetectOps = [RegOp.CheckDword(MsKey, "NetworkActivityAllowed", 0)],
                },
                new TweakDef
                {
                    Id = "mstandby-disable-smart-standby",
                    Label = "Disable Adaptive Smart Standby Adjustments",
                    Category = "Display — Input Method",
                    Description =
                        "Disables the intelligent standby system that dynamically adjusts deep-sleep exit rates based on historical usage patterns. When disabled, the system uses fixed configured timeouts rather than ML-driven adaptive transitions.",
                    Tags = ["modern-standby", "adaptive", "smart-standby", "power", "predictable"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Produces deterministic standby behaviour at the cost of optimal power efficiency. Useful for kiosk and fixed-use devices where predictable power cycling is preferred.",
                    RegistryKeys = [MsKey],
                    ApplyOps = [RegOp.SetDword(MsKey, "DisableSmartStandby", 1)],
                    RemoveOps = [RegOp.DeleteValue(MsKey, "DisableSmartStandby")],
                    DetectOps = [RegOp.CheckDword(MsKey, "DisableSmartStandby", 1)],
                },
                new TweakDef
                {
                    Id = "mstandby-disable-background-tasks-in-standby",
                    Label = "Disable Background Task Execution During Modern Standby",
                    Category = "Display — Input Method",
                    Description =
                        "Prevents application background tasks from running while the system is in Modern Standby. Background tasks in S0 consume battery, can trigger wake-locks that prevent deep sleep, and may leak user data via cloud sync while the device appears powered off.",
                    Tags = ["modern-standby", "background-tasks", "battery", "privacy", "s0"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Suppresses background app refresh during standby; notifications and cloud sync resume on user wake. Significantly improves battery life on devices with aggressive background app models.",
                    RegistryKeys = [MsKey],
                    ApplyOps = [RegOp.SetDword(MsKey, "AllowBackgroundTasksInStandby", 0)],
                    RemoveOps = [RegOp.DeleteValue(MsKey, "AllowBackgroundTasksInStandby")],
                    DetectOps = [RegOp.CheckDword(MsKey, "AllowBackgroundTasksInStandby", 0)],
                },
                new TweakDef
                {
                    Id = "mstandby-disable-maintenance-in-standby",
                    Label = "Disable Automatic Maintenance Execution During Standby",
                    Category = "Display — Input Method",
                    Description =
                        "Prevents the Windows Automatic Maintenance scheduler from running maintenance tasks (Disk Defrag, Windows Defender scans, app updates) while the device is in Modern Standby. Avoids unexpected disk I/O, CPU wake, and battery drain during standby periods.",
                    Tags = ["modern-standby", "maintenance", "automatic-maintenance", "battery", "scheduling"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Maintenance tasks (including Defender scans) will defer to the next active session. Track that maintenance completes during awake sessions to avoid indefinite deferral.",
                    RegistryKeys = [MsKey],
                    ApplyOps = [RegOp.SetDword(MsKey, "AllowMaintenanceDuringStandby", 0)],
                    RemoveOps = [RegOp.DeleteValue(MsKey, "AllowMaintenanceDuringStandby")],
                    DetectOps = [RegOp.CheckDword(MsKey, "AllowMaintenanceDuringStandby", 0)],
                },
                new TweakDef
                {
                    Id = "mstandby-require-fast-startup-disabled",
                    Label = "Disable Hybrid Shutdown / Fast Startup (Hiberboot)",
                    Category = "Display — Input Method",
                    Description =
                        "Disables Hybrid Shutdown (Fast Startup / Hiberboot) which persists kernel session to the hibernate file across reboots. Hiberboot bypasses full driver reinitialisation and can leave security tools in stale state; full cold boot is safer and more predictable.",
                    Tags = ["modern-standby", "fast-startup", "hiberboot", "hibernate", "cold-boot"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Shutdown/startup will be slightly slower but every boot is a clean cold boot. Recommended for compliance-sensitive environments and shared machines.",
                    RegistryKeys = [PwrKey],
                    ApplyOps = [RegOp.SetDword(PwrKey, "HiberbootEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(PwrKey, "HiberbootEnabled")],
                    DetectOps = [RegOp.CheckDword(PwrKey, "HiberbootEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "mstandby-set-idle-standby-timeout",
                    Label = "Set Plugged-In Idle-to-Standby Timeout to 30 Minutes",
                    Category = "Display — Input Method",
                    Description =
                        "Configures the AC (plugged-in) idle timeout before the system enters Modern Standby or sleep to 30 minutes (1800 seconds). Reduces the window in which an unattended unlocked workstation is physically accessible before it locks and suspends.",
                    Tags = ["modern-standby", "idle-timeout", "screen-lock", "power", "physical-security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "30-minute AC idle timeout before sleep is a reasonable physical-security baseline for workstations. Pairs with screen lock and credential timeout policies.",
                    RegistryKeys = [PwrSleepKey],
                    ApplyOps = [RegOp.SetDword(PwrSleepKey, "IdleTimeoutAC", 1800)],
                    RemoveOps = [RegOp.DeleteValue(PwrSleepKey, "IdleTimeoutAC")],
                    DetectOps = [RegOp.CheckDword(PwrSleepKey, "IdleTimeoutAC", 1800)],
                },
                new TweakDef
                {
                    Id = "mstandby-block-wake-timers-in-standby",
                    Label = "Block Programmatic Wake Timers During Modern Standby",
                    Category = "Display — Input Method",
                    Description =
                        "Prevents applications and scheduled tasks from setting wake timers that force the system out of Modern Standby. Rogue or poorly coded applications can use wake timers to keep the device powered on continuously; blocking timers enforces true standby.",
                    Tags = ["modern-standby", "wake-timer", "power", "battery", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Blocks all application-set wake timers during standby. Windows Update maintenance wake timers are a notable exception — it may still wake for critical updates depending on policy.",
                    RegistryKeys = [PwrSleepKey],
                    ApplyOps = [RegOp.SetDword(PwrSleepKey, "AllowWakeTimers", 0)],
                    RemoveOps = [RegOp.DeleteValue(PwrSleepKey, "AllowWakeTimers")],
                    DetectOps = [RegOp.CheckDword(PwrSleepKey, "AllowWakeTimers", 0)],
                },
                new TweakDef
                {
                    Id = "mstandby-disable-wol-in-standby",
                    Label = "Disable Wake-on-LAN During Modern Standby",
                    Category = "Display — Input Method",
                    Description =
                        "Prevents the NIC from processing Wake-on-LAN (WoL) magic packets while the device is in Modern Standby. Eliminates the network-based remote-wake attack surface; an attacker with network access cannot remotely wake and attack the device.",
                    Tags = ["modern-standby", "wake-on-lan", "wol", "network", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Disables WoL in standby; remote power-on via network magic packet will not work while in S0. BIOS/UEFI WoL may override this — also disable WoL there for full protection.",
                    RegistryKeys = [MsKey],
                    ApplyOps = [RegOp.SetDword(MsKey, "WakeOnLanAllowed", 0)],
                    RemoveOps = [RegOp.DeleteValue(MsKey, "WakeOnLanAllowed")],
                    DetectOps = [RegOp.CheckDword(MsKey, "WakeOnLanAllowed", 0)],
                },
                new TweakDef
                {
                    Id = "mstandby-require-password-on-resume",
                    Label = "Require Password When Resuming from Modern Standby",
                    Category = "Display — Input Method",
                    Description =
                        "Forces credential re-entry when the device resumes from Modern Standby or sleep. Without this policy the screen may stay unlocked after resume, exposing the session to physical access attacks on shared or temporarily unattended machines.",
                    Tags = ["modern-standby", "password-resume", "screen-lock", "credential", "physical-security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Ensures the screen is locked on every standby resume, requiring Windows Hello or password to re-enter the session. This is a standard physical-security baseline.",
                    RegistryKeys = [PwrSleepKey],
                    ApplyOps = [RegOp.SetDword(PwrSleepKey, "PromptPasswordOnWakeup", 1)],
                    RemoveOps = [RegOp.DeleteValue(PwrSleepKey, "PromptPasswordOnWakeup")],
                    DetectOps = [RegOp.CheckDword(PwrSleepKey, "PromptPasswordOnWakeup", 1)],
                },
            ];
    }

    // ── PenWorkspaceGpoPolicy ──
    private static class _PenWorkspaceGpoPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PenWorkspace";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "penws-disable-pen-workspace",
                Label = "Disable Pen Workspace",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets PenWorkspaceButtonDesiredVisibility=0 in the PenWorkspace policy key. "
                    + "Hides the Pen Workspace button from the taskbar and prevents the floating "
                    + "Pen Workspace panel from launching. Pen Workspace aggregates Windows Ink, "
                    + "Sticky Notes, and Screen Sketch into a sidebar. On devices without a "
                    + "pen or stylus this button serves no purpose. "
                    + "Default: not set (shown on pen-equipped devices). Recommended: 0.",
                Tags = ["pen", "workspace", "taskbar", "ink", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PenWorkspaceButtonDesiredVisibility", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "PenWorkspaceButtonDesiredVisibility")],
                DetectOps = [RegOp.CheckDword(Key, "PenWorkspaceButtonDesiredVisibility", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-above-lock",
                Label = "Disable Pen Workspace Above Lock Screen",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets UserEducationInAboveLockAllowed=0 in the PenWorkspace policy key. "
                    + "Prevents the Windows Ink Workspace and associated onboarding prompts "
                    + "from appearing on the lock screen. Applications shown above the lock "
                    + "screen are accessible without authentication, creating a potential "
                    + "information-disclosure or bypass surface. "
                    + "Default: 1 (allowed). Recommended: 0 on security-hardened systems.",
                Tags = ["pen", "workspace", "lockscreen", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "UserEducationInAboveLockAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "UserEducationInAboveLockAllowed")],
                DetectOps = [RegOp.CheckDword(Key, "UserEducationInAboveLockAllowed", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-touch-keyboard-onboarding",
                Label = "Disable Touch Keyboard Onboarding",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets TouchKeyboardOnboardingAllowed=0 in the PenWorkspace policy key. "
                    + "Suppresses the promotional 'Try the new touch keyboard' onboarding banner "
                    + "that appears in the touch keyboard session. The banner interrupts "
                    + "workflow on tablet form-factor devices and is purely marketing-oriented. "
                    + "Default: 1 (shown). Recommended: 0.",
                Tags = ["pen", "touch", "keyboard", "onboarding", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TouchKeyboardOnboardingAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "TouchKeyboardOnboardingAllowed")],
                DetectOps = [RegOp.CheckDword(Key, "TouchKeyboardOnboardingAllowed", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-handwriting-panel",
                Label = "Disable Handwriting Panel",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets PenWorkspaceHandwritingEnabled=0 in the PenWorkspace policy key. "
                    + "Disables the floating handwriting input panel that appears near text "
                    + "fields when a stylus approaches the screen. This panel intercepts "
                    + "stylus input before the active application and translates strokes to "
                    + "text via Windows Ink. Disabling it may improve stylus performance in "
                    + "drawing or annotation applications. Default: 1. Recommended: 0.",
                Tags = ["pen", "handwriting", "ink", "input", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PenWorkspaceHandwritingEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "PenWorkspaceHandwritingEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "PenWorkspaceHandwritingEnabled", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-workspace-telemetry",
                Label = "Disable Pen Workspace Telemetry",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets PenWorkspaceTelemetryAllowed=0 in the PenWorkspace policy key. "
                    + "Stops Windows Ink Workspace from transmitting usage analytics covering "
                    + "which Ink apps were launched, pen interaction rates, stylus hardware "
                    + "model, and session durations to Microsoft's telemetry pipeline. "
                    + "These signals accumulate a detailed device-usage profile. "
                    + "Default: 1. Recommended: 0.",
                Tags = ["pen", "workspace", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PenWorkspaceTelemetryAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "PenWorkspaceTelemetryAllowed")],
                DetectOps = [RegOp.CheckDword(Key, "PenWorkspaceTelemetryAllowed", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-ink-replay",
                Label = "Disable Ink Replay Logging",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets InkReplayEnabled=0 in the PenWorkspace policy key. Disables the "
                    + "Windows Ink replay feature that records the full sequence of pen strokes "
                    + "so they can be animated back at playback speed. Stroke replay data is "
                    + "stored as a journal that fully reconstructs handwritten content and can "
                    + "expose sensitive notes or signatures if the device is compromised. "
                    + "Default: 1. Recommended: 0.",
                Tags = ["pen", "ink", "replay", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "InkReplayEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "InkReplayEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "InkReplayEnabled", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-pen-promo",
                Label = "Disable Pen Workspace Hardware Promo",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets AllowSuggestedAppsInWindowsInkWorkspace=0 in the PenWorkspace policy "
                    + "key. Removes the 'Suggested Apps' section from Windows Ink Workspace "
                    + "that promotes pen-optimised Store apps. Suggested apps load metadata "
                    + "from the Microsoft Store CDN at every Workspace open, adding network "
                    + "latency and transmitting device pen-hardware telemetry. "
                    + "Default: 1. Recommended: 0.",
                Tags = ["pen", "workspace", "promo", "store", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowSuggestedAppsInWindowsInkWorkspace", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowSuggestedAppsInWindowsInkWorkspace")],
                DetectOps = [RegOp.CheckDword(Key, "AllowSuggestedAppsInWindowsInkWorkspace", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-dictation",
                Label = "Disable Ink Dictation Button",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets AllowWindowsInkWorkspace=0 via the AllowWindowsInkWorkspaceValue "
                    + "policy in the PenWorkspace policy key. Removes the microphone-dictation "
                    + "shortcut button from the touch keyboard and handwriting panel, preventing "
                    + "accidental activation of speech input that streams audio to the Windows "
                    + "speech recognition service. "
                    + "Default: 2 (only above lock). Recommended: 0.",
                Tags = ["pen", "dictation", "voice", "speech", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowWindowsInkWorkspace", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowWindowsInkWorkspace")],
                DetectOps = [RegOp.CheckDword(Key, "AllowWindowsInkWorkspace", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-sticky-notes-lock",
                Label = "Disable Sticky Notes on Lock Screen",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets StickyNotesOnLockScreenAllowed=0 in the PenWorkspace policy key. "
                    + "Prevents Sticky Notes from appearing on the lock screen, which would "
                    + "allow anyone near the device to view note content without authentication. "
                    + "Users who store passwords, addresses, or meeting details in Sticky Notes "
                    + "are particularly exposed by lock-screen visibility. "
                    + "Default: 1. Recommended: 0.",
                Tags = ["pen", "stickynotes", "lockscreen", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "StickyNotesOnLockScreenAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "StickyNotesOnLockScreenAllowed")],
                DetectOps = [RegOp.CheckDword(Key, "StickyNotesOnLockScreenAllowed", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-pencil-button-shortcut",
                Label = "Disable Pen Button Shortcut to Workspace",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets PenButtonDesiredAction=2 in the PenWorkspace policy key. Changes the "
                    + "pen barrel-button shortcut from launching Windows Ink Workspace (default) "
                    + "to a no-op action, preventing accidental Workspace activations while "
                    + "drawing in design and annotation applications. Setting value 2 disables "
                    + "the button's system action entirely, leaving it for application-defined "
                    + "handling. Default: not set. Recommended: 2 on professional artist workstations.",
                Tags = ["pen", "button", "shortcut", "workspace", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PenButtonDesiredAction", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "PenButtonDesiredAction")],
                DetectOps = [RegOp.CheckDword(Key, "PenButtonDesiredAction", 2)],
            },
        ];
    }

    // ── PersonalizationLockPolicy ──
    private static class _PersonalizationLockPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "plock-disable-lock-screen",
                    Label = "Disable Interactive Lock Screen",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Removes the interactive lock screen and bypasses Cortana, search, and media controls on the lock screen. Users must enter credentials immediately. Default: enabled. Recommended: 1 (disabled) in kiosk or high-security environments.",
                    Tags = ["lock-screen", "personalization", "kiosk", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Lock screen removed; sign-in prompt shown immediately. Cortana and media controls on lock screen are unavailable.",
                    ApplyOps = [RegOp.SetDword(Key, "NoLockScreen", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoLockScreen")],
                    DetectOps = [RegOp.CheckDword(Key, "NoLockScreen", 1)],
                },
                new TweakDef
                {
                    Id = "plock-enforce-lock-screen-image",
                    Label = "Enforce Corporate Lock Screen Image",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Forces a specific corporate lock screen image path and prevents users from changing it. Ensures brand-consistent or security-warning lock screens. Default: not enforced. Recommended: set path in LockScreenImage value.",
                    Tags = ["lock-screen", "image", "corporate", "branding", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "All users see the same lock screen image; individual customisation is blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "LockScreenImageEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LockScreenImageEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "LockScreenImageEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "plock-block-user-lock-screen-change",
                    Label = "Block User From Changing Lock Screen",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Prevents non-admin users from changing the lock screen image or slide show. Enforces IT-managed lock screen content. Default: not controlled. Recommended: 1 in managed environments.",
                    Tags = ["lock-screen", "user-restriction", "personalization", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Users cannot change lock screen via Settings; the IT-set lock screen image persists.",
                    ApplyOps = [RegOp.SetDword(Key, "PreventChangingLockScreen", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreventChangingLockScreen")],
                    DetectOps = [RegOp.CheckDword(Key, "PreventChangingLockScreen", 1)],
                },
                new TweakDef
                {
                    Id = "plock-disable-lock-screen-camera",
                    Label = "Disable Camera on Lock Screen",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Prevents the camera from being activated from the lock screen without unlocking. Closes the camera-access-without-authentication attack surface. Default: 0. Recommended: 1 (disabled).",
                    Tags = ["lock-screen", "camera", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Camera cannot be accessed from lock screen; must unlock first.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowLockScreenCamera", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowLockScreenCamera")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowLockScreenCamera", 0)],
                },
                new TweakDef
                {
                    Id = "plock-disable-lock-screen-toast",
                    Label = "Disable Toast Notifications on Lock Screen",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Prevents toast notifications from displaying on the lock screen, hiding message previews and alert content from unauthenticated view. Default: enabled. Recommended: 1 (disabled) for data protection.",
                    Tags = ["lock-screen", "notifications", "toast", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Notification content not visible from lock screen; users must log in to see notifications.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowLockScreenToastNotifications", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowLockScreenToastNotifications")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowLockScreenToastNotifications", 0)],
                },
                new TweakDef
                {
                    Id = "plock-set-auto-slideshow",
                    Label = "Disable Lock Screen Slideshow",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables the lock screen slideshow feature that cycles through user-selected photos. Enforces a static lock screen image and prevents unintended photo disclosure on unattended PCs. Default: enabled. Recommended: 1 (disabled).",
                    Tags = ["lock-screen", "slideshow", "photos", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Lock screen does not rotate photos from the user's pictures library.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLockScreenSlideshow", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLockScreenSlideshow")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLockScreenSlideshow", 1)],
                },
                new TweakDef
                {
                    Id = "plock-block-desktop-theme-change",
                    Label = "Block Users From Changing Desktop Theme",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Prevents standard users from applying custom desktop themes, wallpapers, or colour schemes. Enforces consistent corporate visual identity. Default: not controlled. Recommended: 1 in kiosk/call-centre environments.",
                    Tags = ["desktop", "theme", "personalization", "corporate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Users cannot customise wallpaper or theme via Settings; admin-set theme is enforced.",
                    ApplyOps = [RegOp.SetDword(Key, "NoChangingTheme", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoChangingTheme")],
                    DetectOps = [RegOp.CheckDword(Key, "NoChangingTheme", 1)],
                },
                new TweakDef
                {
                    Id = "plock-disable-color-change",
                    Label = "Disable User Windows Accent Colour Change",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Prevents users from changing the Windows accent colour used in title bars, taskbar, and UI highlights. Enforces brand-consistent UI colouring set by IT policy. Default: not controlled. Recommended: 1 in corporate environments.",
                    Tags = ["accent-color", "personalization", "ui", "corporate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Accent colour picker in Settings is disabled; IT-defined colour is enforced.",
                    ApplyOps = [RegOp.SetDword(Key, "NoColorChange", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoColorChange")],
                    DetectOps = [RegOp.CheckDword(Key, "NoColorChange", 1)],
                },
                new TweakDef
                {
                    Id = "plock-disable-transparency-effects",
                    Label = "Disable Windows Transparency Effects via Policy",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables the acrylic transparency effects in Windows title bars and taskbar via Group Policy. Reduces GPU compositing overhead on resource-constrained hardware. Default: enabled. Recommended: 1 (disabled) on VMs and thin clients.",
                    Tags = ["transparency", "acrylic", "performance", "personalization", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Windows UI renders without blur/transparency; minor performance improvement on low-end hardware.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTransparencyEffects", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTransparencyEffects")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTransparencyEffects", 1)],
                },
            ];
    }

    // ── PersonalizationPolicy ──
    private static class _PersonalizationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization";
        private const string SysPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "prsnlz-disable-lock-screen-overlays",
                Label = "Disable Lock Screen App Notification Overlays",
                Category = "Display — Personalization Lock",
                Description =
                    "Removes application notification badges (email count, calendar events, alarms) from the lock screen. Reduces information leakage to unauthenticated observers.",
                Tags = ["lock-screen", "notifications", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents notification badges from leaking information to unauthenticated observers.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LockScreenOverlaysDisabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LockScreenOverlaysDisabled")],
                DetectOps = [RegOp.CheckDword(Key, "LockScreenOverlaysDisabled", 1)],
            },
            new TweakDef
            {
                Id = "prsnlz-force-default-lock-screen",
                Label = "Force Default Lock Screen Image",
                Category = "Display — Personalization Lock",
                Description =
                    "Prevents users from customising the lock screen image. Forces the Windows default lock screen, blocking user-selected photos or Windows Spotlight images.",
                Tags = ["lock-screen", "personalization", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Enforces a uniform lock screen image organisation-wide.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ForceDefaultLockScreen", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ForceDefaultLockScreen")],
                DetectOps = [RegOp.CheckDword(Key, "ForceDefaultLockScreen", 1)],
            },
            new TweakDef
            {
                Id = "prsnlz-prevent-wallpaper-change",
                Label = "Prevent Desktop Wallpaper Changes",
                Category = "Display — Personalization Lock",
                Description =
                    "Prevents users from changing the desktop wallpaper via Settings or Control Panel. Enforces a consistent corporate desktop appearance.",
                Tags = ["wallpaper", "personalization", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Locks desktop wallpaper to a corporate standard.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoDesktopBackground", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoDesktopBackground")],
                DetectOps = [RegOp.CheckDword(Key, "NoDesktopBackground", 1)],
            },
            new TweakDef
            {
                Id = "prsnlz-hide-background-settings",
                Label = "Hide Background/Wallpaper Settings Page",
                Category = "Display — Personalization Lock",
                Description =
                    "Removes the background/wallpaper tab from Display Properties in Control Panel, preventing users from accessing wallpaper settings.",
                Tags = ["wallpaper", "control-panel", "policy", "personalization"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides the wallpaper tab from Control Panel Display Properties.",
                RegistryKeys = [SysPolicy],
                ApplyOps = [RegOp.SetDword(SysPolicy, "NoDispBackgroundPage", 1)],
                RemoveOps = [RegOp.DeleteValue(SysPolicy, "NoDispBackgroundPage")],
                DetectOps = [RegOp.CheckDword(SysPolicy, "NoDispBackgroundPage", 1)],
            },
            new TweakDef
            {
                Id = "prsnlz-hide-screensaver-settings",
                Label = "Hide Screensaver Settings Page",
                Category = "Display — Personalization Lock",
                Description =
                    "Removes the screensaver tab from Display Properties in Control Panel, preventing users from changing screensaver settings.",
                Tags = ["screensaver", "control-panel", "policy", "personalization"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides the screensaver tab from Control Panel Display Properties.",
                RegistryKeys = [SysPolicy],
                ApplyOps = [RegOp.SetDword(SysPolicy, "NoDispScrSavPage", 1)],
                RemoveOps = [RegOp.DeleteValue(SysPolicy, "NoDispScrSavPage")],
                DetectOps = [RegOp.CheckDword(SysPolicy, "NoDispScrSavPage", 1)],
            },
            new TweakDef
            {
                Id = "prsnlz-hide-appearance-settings",
                Label = "Hide Appearance Settings Page",
                Category = "Display — Personalization Lock",
                Description =
                    "Removes the Appearance tab from Display Properties in Control Panel, preventing users from changing colour scheme and system visual style.",
                Tags = ["appearance", "control-panel", "policy", "personalization"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides the Appearance tab from Control Panel Display Properties.",
                RegistryKeys = [SysPolicy],
                ApplyOps = [RegOp.SetDword(SysPolicy, "NoDispAppearancePage", 1)],
                RemoveOps = [RegOp.DeleteValue(SysPolicy, "NoDispAppearancePage")],
                DetectOps = [RegOp.CheckDword(SysPolicy, "NoDispAppearancePage", 1)],
            },
            new TweakDef
            {
                Id = "prsnlz-prevent-color-change",
                Label = "Prevent System Colour Scheme Changes",
                Category = "Display — Personalization Lock",
                Description =
                    "Blocks users from changing the Windows colour scheme (accent colours, dark/light theme selection) via Settings or Control Panel.",
                Tags = ["theme", "colors", "control-panel", "policy", "personalization"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Locks the Windows colour scheme to the admin-set value.",
                RegistryKeys = [SysPolicy],
                ApplyOps = [RegOp.SetDword(SysPolicy, "NoColorChoice", 1)],
                RemoveOps = [RegOp.DeleteValue(SysPolicy, "NoColorChoice")],
                DetectOps = [RegOp.CheckDword(SysPolicy, "NoColorChoice", 1)],
            },
        ];
    }

    // ── PlayToDevicePolicy ──
    private static class _PlayToDevicePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PlayToReceiver";
        private const string WsdKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WSD";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "playtodev-disable-play-to-receiver",
                    Label = "Play To: Disable Windows Play To Receiver Feature",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets NotAllowPlayToReceiver=1 in PlayToReceiver machine policy. Disables the Windows 'Play To' receiver capability that allows other DLNA-compatible devices on the same network to push media content to this PC for playback. "
                        + "'Play To' opens this device as a DLNA media renderer, listening for UPnP/DLNA control point commands from any device on the local network. On a corporate network, this allows any DLNA-capable device (including personal mobile phones) to push multimedia content to corporate workstations without authentication. Disabling the receiver prevents the device from accepting unsolicited media content pushes.",
                    Tags = ["dlna", "play-to", "receiver", "media", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Disables DLNA Play To receiver; workstation cannot accept media pushes from devices on local network.",
                    ApplyOps = [RegOp.SetDword(Key, "NotAllowPlayToReceiver", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NotAllowPlayToReceiver")],
                    DetectOps = [RegOp.CheckDword(Key, "NotAllowPlayToReceiver", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-disable-play-to-sender",
                    Label = "Play To: Disable Windows Play To Media Source Sending",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets DisablePlayTo=1 in PlayToReceiver machine policy. Disables the ability for users to use this PC as a 'Play To' source — sending media from Windows Media Player, Photos, or other DLNA-compatible applications to an external DLNA renderer. "
                        + "Using this PC as a 'Play To' sender connects to UPnP devices on the network and pushes streaming data to them. On a corporate network, this could be used to stream sensitive video content from a corporate machine to a personal smart TV, Chromecast, or other unmanaged renderer. Disabling 'Play To' sender functionality prevents this data exfiltration vector.",
                    Tags = ["dlna", "play-to", "sender", "media", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks DLNA media sending; screen/media content not streamable from this PC to external renderers.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePlayTo", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePlayTo")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePlayTo", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-block-wsd-device-discovery",
                    Label = "Play To: Block WSD (Web Services on Devices) Network Discovery",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets DisableWSDDiscovery=1 in WSD machine policy. Prevents the Windows WSD (Web Services on Devices) stack from announcing this device to the local network and from probing for WSD-compatible peripherals — including networked printers, scanners, and media renderers. "
                        + "WSD uses multicast UDP probes (WS-Discovery) that announce the device's presence to all LAN segments. These broadcasts leak device identity, OS version, and service capabilities to all network listeners. Disabling WSD discovery reduces the device's network footprint.",
                    Tags = ["wsd", "discovery", "network", "multicast", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote =
                        "Blocks WSD/WS-Discovery multicast probes; device not discoverable via WSD protocol. May affect network printer discovery.",
                    ApplyOps = [RegOp.SetDword(WsdKey, "DisableWSDDiscovery", 1)],
                    RemoveOps = [RegOp.DeleteValue(WsdKey, "DisableWSDDiscovery")],
                    DetectOps = [RegOp.CheckDword(WsdKey, "DisableWSDDiscovery", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-block-wsd-printer-discovery",
                    Label = "Play To: Block WSD-Based Printer Auto-Discovery",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets DisableWSDPrinting=1 in WSD machine policy. Prevents auto-discovery and installation of WSD-connected network printers. "
                        + "WSD printer discovery installs printers from the local network automatically without user approval by default on domain-joined machines. On enterprise networks with centralised print server management, rogue WSD printers could intercept print jobs if employees accidentally redirect documents to an unauthorised WSD printer device near them. Disabling WSD printer discovery enforces exclusively server-managed print queue access.",
                    Tags = ["wsd", "printer", "discovery", "print", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Blocks WSD printer auto-install; print queues must be added manually or via GPO print server.",
                    ApplyOps = [RegOp.SetDword(WsdKey, "DisableWSDPrinting", 1)],
                    RemoveOps = [RegOp.DeleteValue(WsdKey, "DisableWSDPrinting")],
                    DetectOps = [RegOp.CheckDword(WsdKey, "DisableWSDPrinting", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-block-wsd-scanner-discovery",
                    Label = "Play To: Block WSD-Based Scanner Auto-Discovery",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets DisableWSDScanning=1 in WSD machine policy. Prevents automatic discovery and installation of network scanners that advertise themselves via WSD/WIA (Windows Image Acquisition). "
                        + "WSD scanner discovery installs scanning drivers and opens WIA sessions to any WSD-compatible scanner found on the network. Unauthorised scanners on the network could be configured to receive forwarded scan-to-email jobs from misconfigured endpoints. Disabling WSD scanner discovery prevents unsolicited scanner driver installation and ensures scanning hardware is explicitly approved by IT.",
                    Tags = ["wsd", "scanner", "discovery", "wia", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Blocks WSD scanner auto-discovery; scanners require manual or GPO-managed WIA configuration.",
                    ApplyOps = [RegOp.SetDword(WsdKey, "DisableWSDScanning", 1)],
                    RemoveOps = [RegOp.DeleteValue(WsdKey, "DisableWSDScanning")],
                    DetectOps = [RegOp.CheckDword(WsdKey, "DisableWSDScanning", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-disable-play-to-dmr-advertisement",
                    Label = "Play To: Disable DLNA Digital Media Renderer Advertisement",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets NotAdvertisePlayToDevice=1 in PlayToReceiver machine policy. Prevents this Windows PC from advertising itself as a DLNA Digital Media Renderer (DMR) on the local network. "
                        + "DMR advertisement broadcasts multicast UPnP SSDP announcements that include the device's name, model, IP address, and capabilities to all devices on the network. This advertisement allows any DLNA control point (phone, tablet, smart TV) to discover and push media to the PC. Suppressing the advertisement hides the device from DLNA discovery without fully disabling the network stack services.",
                    Tags = ["dlna", "dmr", "advertisement", "ssdp", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Suppresses DLNA DMR advertisement; PC not visible to DLNA control points on the network.",
                    ApplyOps = [RegOp.SetDword(Key, "NotAdvertisePlayToDevice", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NotAdvertisePlayToDevice")],
                    DetectOps = [RegOp.CheckDword(Key, "NotAdvertisePlayToDevice", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-disable-media-sharing-network-access",
                    Label = "Play To: Disable Media Library Network Sharing",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets DisableMediaSharing=1 in PlayToReceiver machine policy. Prevents this PC from sharing its media library (pictures, videos, music) with other devices on the network via the Windows Media Player network sharing service. "
                        + "Media library sharing exposes the contents of the user's Documents, Pictures, and Music folders to any UPnP/DLNA media renderer on the local network without per-file access controls. On corporate networks, user Documents folders frequently contain sensitive files that the file-sharing component includes in its media index.",
                    Tags = ["media", "sharing", "library", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks media library network sharing; document folders not exposed via DLNA/UPnP media server.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableMediaSharing", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableMediaSharing")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableMediaSharing", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-restrict-play-to-local-subnet-only",
                    Label = "Play To: Restrict Play To and DLNA to Local Subnet Only",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets AllowedNetworkScopes=1 in PlayToReceiver machine policy. Restricts the DLNA/Play To feature to the local subnet only, preventing cross-subnet media streaming and rendering. "
                        + "Limiting Play To to the local subnet ensures that media streaming sessions cannot traverse network routers into other VLANs or the wide internet. This is the least-restrictive enterprise hardening option for organisations where DLNA is permitted for AV room systems on a dedicated VLAN but must not be accessible from corporate workstation VLANs.",
                    Tags = ["dlna", "subnet", "scope", "network-segmentation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "DLNA scoped to local subnet only; cross-VLAN and internet-routed media streaming blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowedNetworkScopes", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowedNetworkScopes")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowedNetworkScopes", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-disable-device-play-auto-start",
                    Label = "Play To: Disable Auto-Start of Play To Service at Logon",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets DisableAutoStart=1 in PlayToReceiver machine policy. Prevents the Windows Play To Receiver service from starting automatically at user logon. "
                        + "The Play To receiver service starts in the background and maintains a network listener even when neither party has initiated a media session. This background process consumes memory, CPU, and network port capacity. Disabling auto-start ensures the service only runs when explicitly invoked, reducing the device's idle network service footprint.",
                    Tags = ["dlna", "service", "auto-start", "startup", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Play To service not auto-started; listener not running unless explicitly launched.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoStart", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoStart")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoStart", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-disable-wsd-host-discovery",
                    Label = "Play To: Disable WSD Function Discovery Host (FDHOST) Network Broadcast",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets DisableFunctionDiscoveryHostBroadcast=1 in WSD machine policy. Prevents the Function Discovery Host service from broadcasting WSD host announcements that advertise this machine's services (web services, UPnP capabilities) to other devices on the network. "
                        + "Function Discovery is the mechanism Windows uses to populate the Network window in Explorer. Broadcasting the host's function discovery metadata exposes its installed services and capabilities to all LAN listeners. On hardened workstations, eliminating unnecessary network announcements reduces the device's identifiable surface in passive network reconnaissance.",
                    Tags = ["wsd", "fdhost", "broadcast", "discovery", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Suppresses WSD function discovery host broadcasts; device less identifiable via passive LAN scanning.",
                    ApplyOps = [RegOp.SetDword(WsdKey, "DisableFunctionDiscoveryHostBroadcast", 1)],
                    RemoveOps = [RegOp.DeleteValue(WsdKey, "DisableFunctionDiscoveryHostBroadcast")],
                    DetectOps = [RegOp.CheckDword(WsdKey, "DisableFunctionDiscoveryHostBroadcast", 1)],
                },
            ];
    }

    // ── ScreenSaverSecurityPolicy ──
    private static class _ScreenSaverSecurityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScreenSaver";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "scrsvr-enable-screensaver",
                    Label = "Enforce Screen Saver Activation",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Mandates the screen saver is enabled for all users, ensuring the screen locks after inactivity. Prevents unattended desktop access. Default: not enforced. Recommended: 1 in all managed environments.",
                    Tags = ["screensaver", "lock", "session", "timeout", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Screen saver activates after the configured timeout; users cannot disable via Settings.",
                    ApplyOps = [RegOp.SetDword(Key, "ScreenSaverEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ScreenSaverEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "ScreenSaverEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "scrsvr-require-password-resume",
                    Label = "Require Password on Screen Saver Resume",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Forces Windows to prompt for a password when resuming from the screen saver. Prevents access to an unattended unlocked session. Default: disabled. Recommended: 1 (enabled) for compliance.",
                    Tags = ["screensaver", "password", "security", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Critical access control: an unattended screen always requires re-authentication.",
                    ApplyOps = [RegOp.SetDword(Key, "PasswordProtect", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PasswordProtect")],
                    DetectOps = [RegOp.CheckDword(Key, "PasswordProtect", 1)],
                },
                new TweakDef
                {
                    Id = "scrsvr-set-timeout-seconds",
                    Label = "Set Screen Saver Inactivity Timeout (600s)",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets the screen saver activation delay to 600 seconds (10 minutes) of inactivity. Balances user productivity against security for typical office environments. Default: not set. Recommended: 600.",
                    Tags = ["screensaver", "timeout", "inactivity", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Screen saver activates after 10 minutes of inactivity; adjust timeout per risk posture.",
                    ApplyOps = [RegOp.SetDword(Key, "ScreenSaveTimeOut", 600)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ScreenSaveTimeOut")],
                    DetectOps = [RegOp.CheckDword(Key, "ScreenSaveTimeOut", 600)],
                },
                new TweakDef
                {
                    Id = "scrsvr-block-user-timeout-change",
                    Label = "Block Users From Changing Screen Saver Timeout",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Prevents users from modifying the screen saver wait time in Display Properties. Ensures the IT-mandated timeout is respected. Default: not controlled. Recommended: 1.",
                    Tags = ["screensaver", "timeout", "user-restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Screen saver timeout control is greyed out in user Settings.",
                    ApplyOps = [RegOp.SetDword(Key, "NoDispScrSavPage", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoDispScrSavPage")],
                    DetectOps = [RegOp.CheckDword(Key, "NoDispScrSavPage", 1)],
                },
                new TweakDef
                {
                    Id = "scrsvr-block-user-ss-change",
                    Label = "Block Users From Changing Screen Saver",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Prevents users from selecting a different screen saver. The IT-assigned screen saver (blank or corporate-branded) remains fixed. Default: not controlled. Recommended: 1.",
                    Tags = ["screensaver", "user-restriction", "personalization", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Screen saver selection list is hidden; the configured screen saver is fixed.",
                    ApplyOps = [RegOp.SetDword(Key, "NoScreenSaverChange", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoScreenSaverChange")],
                    DetectOps = [RegOp.CheckDword(Key, "NoScreenSaverChange", 1)],
                },
                new TweakDef
                {
                    Id = "scrsvr-use-blank-ss",
                    Label = "Force Blank (Black) Screen Saver",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Forces the blank/black screen saver as the mandatory screen saver. Avoids animation CPU overhead and prevents screen burn-in from animated screensavers. Default: user-selected. Recommended: scrnsave.scr (blank).",
                    Tags = ["screensaver", "blank", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Screen saver is blank (black); no CPU/GPU cycles used animating screensaver graphics.",
                    ApplyOps = [RegOp.SetDword(Key, "UseBlankScreenSaver", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "UseBlankScreenSaver")],
                    DetectOps = [RegOp.CheckDword(Key, "UseBlankScreenSaver", 1)],
                },
                new TweakDef
                {
                    Id = "scrsvr-disable-user-password-change",
                    Label = "Block Users From Disabling SS Password Requirement",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Prevents users from unchecking the 'On resume, display logon screen' option in screen saver settings. Ensures password-on-resume cannot be silently disabled. Default: not controlled. Recommended: 1.",
                    Tags = ["screensaver", "password", "user-restriction", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "The password-on-resume checkbox in Screen Saver Settings is greyed out and locked enabled.",
                    ApplyOps = [RegOp.SetDword(Key, "NoPasswordOnResume", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoPasswordOnResume")],
                    DetectOps = [RegOp.CheckDword(Key, "NoPasswordOnResume", 0)],
                },
                new TweakDef
                {
                    Id = "scrsvr-min-screen-timeout-30s",
                    Label = "Enforce Minimum 30-Second Screen Saver Wait",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets a minimum screen saver activation delay of 30 seconds, preventing users from setting it too low (causing frequent screen lock during active use). Default: not set. Recommended: 30.",
                    Tags = ["screensaver", "timeout", "minimum", "usability", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Screen saver will not activate in less than 30 seconds; prevents productivity-breaking too-aggressive locking.",
                    ApplyOps = [RegOp.SetDword(Key, "MinScreenSaveTimeOut", 30)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MinScreenSaveTimeOut")],
                    DetectOps = [RegOp.CheckDword(Key, "MinScreenSaveTimeOut", 30)],
                },
                new TweakDef
                {
                    Id = "scrsvr-max-inactivity-3600s",
                    Label = "Enforce Maximum 3600-Second Inactivity Limit",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Caps the maximum screen saver inactivity wait to 3600 seconds (1 hour). Prevents users from setting an excessively long timeout that would leave unattended sessions unlocked. Default: not set. Recommended: 3600.",
                    Tags = ["screensaver", "timeout", "maximum", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Unattended sessions cannot remain unlocked for more than 1 hour regardless of user setting.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxScreenSaveTimeOut", 3600)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxScreenSaveTimeOut")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxScreenSaveTimeOut", 3600)],
                },
                new TweakDef
                {
                    Id = "scrsvr-grace-period-zero",
                    Label = "Set Screen Saver Lock Grace Period to Zero",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets the grace period (seconds after screen saver starts before lock is enforced) to 0. Ensures immediate lock without a bypass window. Default: 5. Recommended: 0.",
                    Tags = ["screensaver", "grace-period", "lock", "immediate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Moving the mouse immediately after screen saver starts will require re-authentication; no grace bypass window.",
                    ApplyOps = [RegOp.SetDword(Key, "ScreenSaverGracePeriod", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ScreenSaverGracePeriod")],
                    DetectOps = [RegOp.CheckDword(Key, "ScreenSaverGracePeriod", 0)],
                },
            ];
    }

    // ── SharedClipboardControlPolicy ──
    private static class _SharedClipboardControlPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "shrdclip-disable-phone-link",
                    Label = "Disable Phone Link Clipboard Share",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables clipboard sharing between Windows and a linked Android/iOS phone via the Phone Link (Your Phone) app, preventing cross-device clipboard leakage.",
                    Tags = ["clipboard", "phone-link", "sharing", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Phone Link clipboard sync disabled; clipboard data stays on the PC.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePhoneLinkClipboard", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneLinkClipboard")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePhoneLinkClipboard", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-disable-sync-across-devices",
                    Label = "Disable Clipboard Sync Across Devices",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables device-to-device clipboard synchronization at the policy level, complementing AllowCrossDeviceClipboard by blocking back-end sync service.",
                    Tags = ["clipboard", "sync", "devices", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Cross-device clipboard sync blocked at policy level.",
                    ApplyOps = [RegOp.SetDword(Key, "ClipboardSyncBlock", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ClipboardSyncBlock")],
                    DetectOps = [RegOp.CheckDword(Key, "ClipboardSyncBlock", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-disable-cloud-clipboard",
                    Label = "Disable Cloud Clipboard Sync",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables cloud clipboard synchronization feature that uploads clipboard contents to Microsoft's cloud for cross-device access.",
                    Tags = ["clipboard", "cloud", "sync", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Cloud clipboard disabled; clipboard items not uploaded to Microsoft cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCloudClipboardContent", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudClipboardContent")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCloudClipboardContent", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-disable-tooltip-ads",
                    Label = "Disable Clipboard History Tooltip Ads",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Hides promotional tooltips and advertisements shown in the clipboard history panel (Win+V) that encourage enabling cloud clipboard or other Microsoft services.",
                    Tags = ["clipboard", "tooltip", "ads", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Removes clipboard promotional tooltips; no functional impact.",
                    ApplyOps = [RegOp.SetDword(Key, "HideClipboardTooltips", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "HideClipboardTooltips")],
                    DetectOps = [RegOp.CheckDword(Key, "HideClipboardTooltips", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-block-microsoft-apps",
                    Label = "Block Clipboard Access by Microsoft Apps",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Blocks Microsoft first-party applications from accessing the clipboard history API, reducing telemetry and data collection from clipboard contents.",
                    Tags = ["clipboard", "microsoft-apps", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Microsoft apps clipboard access restricted; some features may degrade.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockMicrosoftApplicationsClipboard", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockMicrosoftApplicationsClipboard")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockMicrosoftApplicationsClipboard", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-disable-contextual-suggestions",
                    Label = "Disable Contextual Suggestions in Clipboard",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables contextual content suggestions (e.g., smart replies, URL previews) that appear in the clipboard history panel based on clipboard content analysis.",
                    Tags = ["clipboard", "suggestions", "privacy", "cloud-content", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Clipboard content not analysed for suggestions; fully private.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableClipboardContextSuggestions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableClipboardContextSuggestions")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableClipboardContextSuggestions", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-disable-telemetry",
                    Label = "Disable Clipboard Telemetry",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables telemetry data collection about clipboard usage patterns, preventing clipboard interaction metadata from being sent to Microsoft.",
                    Tags = ["clipboard", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Clipboard usage telemetry stopped; no functional impact.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableClipboardDiagnosticData", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableClipboardDiagnosticData")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableClipboardDiagnosticData", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-block-sensitive-content-detection",
                    Label = "Block Sensitive Clipboard Content Detection",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables content scanning of clipboard items for sensitive data classification (DLP-style detection) by cloud-connected services.",
                    Tags = ["clipboard", "sensitive", "dlp", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Clipboard content not scanned for sensitive data by cloud services.",
                    ApplyOps = [RegOp.SetDword(Key2, "BlockClipboardSensitiveContent", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "BlockClipboardSensitiveContent")],
                    DetectOps = [RegOp.CheckDword(Key2, "BlockClipboardSensitiveContent", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-disable-uwp-clipboard-api",
                    Label = "Disable Clipboard API for UWP Apps",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables the WinRT clipboard API access for Universal Windows Platform (UWP) apps, preventing packaged apps from silently reading or writing the clipboard.",
                    Tags = ["clipboard", "uwp", "api", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "UWP apps cannot access clipboard API; clipboard-dependent UWP features break.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableUwpClipboardAPI", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableUwpClipboardAPI")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableUwpClipboardAPI", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-restrict-same-process-paste",
                    Label = "Restrict Clipboard Paste to Same Process",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Restricts the clipboard paste operation so that clipboard data can only be pasted within the same process that originally wrote it, preventing cross-process data leakage via clipboard.",
                    Tags = ["clipboard", "paste", "isolation", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "Cross-process paste restricted; applications that rely on clipboard sharing between processes will break.",
                    ApplyOps = [RegOp.SetDword(Key2, "RestrictSameProcessClipboard", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "RestrictSameProcessClipboard")],
                    DetectOps = [RegOp.CheckDword(Key2, "RestrictSameProcessClipboard", 1)],
                },
            ];
    }

    // ── ShellRestrictionsPolicy ──
    private static class _ShellRestrictionsPolicy
    {
        private const string Pol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "shellrst-no-find-command",
                Label = "Remove Find/Search from Start Menu",
                Category = "Display — Personalization Lock",
                Description =
                    "Sets NoFind=1 in Policies\\Explorer. Removes the Find/Search shortcut and menu item from the Start menu. Prevents quick enumeration of file system contents via the built-in search dialog. Default: Find is visible.",
                Tags = ["shell", "restriction", "search", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Pol, "NoFind", 1)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NoFind")],
                DetectOps = [RegOp.CheckDword(Pol, "NoFind", 1)],
            },
            new TweakDef
            {
                Id = "shellrst-no-logoff-menu",
                Label = "Remove Log Off from Start Menu",
                Category = "Display — Personalization Lock",
                Description =
                    "Sets NoLogoff=1 in Policies\\Explorer. Removes the Log Off entry from the Start menu, preventing users from signing out via the Start menu shortcut. Session management must be performed through other means (Task Manager, CTRL+ALT+DEL).",
                Tags = ["shell", "restriction", "logoff", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Pol, "NoLogoff", 1)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NoLogoff")],
                DetectOps = [RegOp.CheckDword(Pol, "NoLogoff", 1)],
            },
            new TweakDef
            {
                Id = "shellrst-no-desktop-icons",
                Label = "Hide All Desktop Icons",
                Category = "Display — Personalization Lock",
                Description =
                    "Sets NoDesktop=1 in Policies\\Explorer. Removes all icons from the desktop surface including This PC, Recycle Bin, and user-placed shortcuts. Desktop background is still visible. Used to create clean-slate kiosk or thin-client desktops.",
                Tags = ["shell", "restriction", "desktop", "policy", "kiosk"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Pol, "NoDesktop", 1)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NoDesktop")],
                DetectOps = [RegOp.CheckDword(Pol, "NoDesktop", 1)],
            },
            new TweakDef
            {
                Id = "shellrst-no-drives-page",
                Label = "Remove Drives Tab from Computer Properties",
                Category = "Display — Personalization Lock",
                Description =
                    "Sets NoDrivesPage=1 in Policies\\Explorer. Removes the Drives tab from the Hardware and Storage area in System Properties, preventing detailed enumeration of physical drive properties. Reduces information leakage on shared systems.",
                Tags = ["shell", "restriction", "drives", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Pol, "NoDrivesPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NoDrivesPage")],
                DetectOps = [RegOp.CheckDword(Pol, "NoDrivesPage", 1)],
            },
            new TweakDef
            {
                Id = "shellrst-no-control-panel-applets",
                Label = "Block All Control Panel Applets",
                Category = "Display — Personalization Lock",
                Description =
                    "Sets NoCplApplets=1 in Policies\\Explorer. Prevents all Control Panel .cpl applets from launching, including Display, Sound, Network, System, etc. Combined with the GPO applet allow-list this creates a restricted-access Control Panel for standard users.",
                Tags = ["shell", "restriction", "controlpanel", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Pol, "NoCplApplets", 1)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NoCplApplets")],
                DetectOps = [RegOp.CheckDword(Pol, "NoCplApplets", 1)],
            },
            new TweakDef
            {
                Id = "shellrst-no-display-cpl",
                Label = "Hide Display Control Panel Applet",
                Category = "Display — Personalization Lock",
                Description =
                    "Sets NoDispCPL=1 in Policies\\Explorer. Prevents users from opening the Display applet from Control Panel or the desktop right-click menu, blocking wallpaper, resolution, and colour depth changes. Used to enforce corporate visual standards.",
                Tags = ["shell", "restriction", "display", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Pol, "NoDispCPL", 1)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NoDispCPL")],
                DetectOps = [RegOp.CheckDword(Pol, "NoDispCPL", 1)],
            },
            new TweakDef
            {
                Id = "shellrst-restrict-run-list",
                Label = "Enable DisallowRun Application Restriction",
                Category = "Display — Personalization Lock",
                Description =
                    "Sets DisallowRun=1 in Policies\\Explorer. Activates the DisallowRun enforcement mode, which blocks execution of any application names listed under the adjacent DisallowRun sub-key. Enables per-application deny-listing without requiring AppLocker or WDAC.",
                Tags = ["shell", "restriction", "allowlist", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Pol, "DisallowRun", 1)],
                RemoveOps = [RegOp.DeleteValue(Pol, "DisallowRun")],
                DetectOps = [RegOp.CheckDword(Pol, "DisallowRun", 1)],
            },
            new TweakDef
            {
                Id = "shellrst-no-network-neighborhood",
                Label = "Hide Network Neighborhood from Explorer",
                Category = "Display — Personalization Lock",
                Description =
                    "Sets NoNetHood=1 in Policies\\Explorer. Removes the Network Neighborhood (Network Places) icon from Explorer and the desktop. Users can still access UNC paths directly; this only removes the browsable discovery pane that enumerates visible network shares.",
                Tags = ["shell", "restriction", "network", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Pol, "NoNetHood", 1)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NoNetHood")],
                DetectOps = [RegOp.CheckDword(Pol, "NoNetHood", 1)],
            },
        ];
    }

    // ── ShutdownOptionsPolicy ──
    private static class _ShutdownOptionsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ShutdownOptions";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "shtdwn-disable-shutdown-on-ctrl-alt-del",
                Label = "Disable Shutdown from Ctrl+Alt+Del Screen",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets NoShutdownOnCtrlAltDel=1 in the ShutdownOptions policy key. "
                    + "Removes the Shut down button from the Ctrl+Alt+Del secure attention "
                    + "sequence screen, preventing non-admin users from shutting down or "
                    + "restarting the machine without appropriate privilege. On shared "
                    + "workstations and call-centre desktops accidental shutdown of "
                    + "production machines causes service disruption. Default: 0.",
                Tags = ["shutdown", "ctrl-alt-del", "security", "lockdown", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoShutdownOnCtrlAltDel", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoShutdownOnCtrlAltDel")],
                DetectOps = [RegOp.CheckDword(Key, "NoShutdownOnCtrlAltDel", 1)],
            },
            new TweakDef
            {
                Id = "shtdwn-require-shutdown-reason",
                Label = "Require Shutdown Reason and Comment",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets ShutdownReasonOn=1 in the ShutdownOptions policy key. Forces the "
                    + "shutdown dialog to display a reason code drop-down and optional "
                    + "comment field before accepting a restart or shutdown command. "
                    + "Mandatory shutdown reason codes create an audit trail of who shut "
                    + "down a system and why, which is valuable in production server "
                    + "environments and shared workstations. Default: 0.",
                Tags = ["shutdown", "reason", "audit", "compliance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ShutdownReasonOn", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ShutdownReasonOn")],
                DetectOps = [RegOp.CheckDword(Key, "ShutdownReasonOn", 1)],
            },
            new TweakDef
            {
                Id = "shtdwn-disable-restart-apps",
                Label = "Disable App Restart After Reboot",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableAppRestart=1 in the ShutdownOptions policy key. Prevents "
                    + "Windows from re-launching applications registered in the RunOnce "
                    + "restart list after a reboot. Some application installers and updaters "
                    + "use the restart application list to auto-launch their product after "
                    + "a reboot; disabling this keeps the post-reboot session clean and "
                    + "consistent in enterprise imaging workflows. Default: 0.",
                Tags = ["shutdown", "restart", "apps", "runonce", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAppRestart", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAppRestart")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAppRestart", 1)],
            },
            new TweakDef
            {
                Id = "shtdwn-disable-automatic-restart",
                Label = "Disable Automatic Restart After BSOD",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets DisableAutomaticRestart=1 in the ShutdownOptions policy key. "
                    + "Prevents the system from automatically rebooting after a fatal Stop "
                    + "error (Blue Screen of Death). Automatic restart hides the bugcheck "
                    + "code and error details from the user before they can note the stop "
                    + "code; disabling it allows engineers to read and photograph the BSOD "
                    + "screen and correlate it with kernel dump analysis. Default: 0.",
                Tags = ["shutdown", "bsod", "crash", "restart", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutomaticRestart", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutomaticRestart")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutomaticRestart", 1)],
            },
            new TweakDef
            {
                Id = "shtdwn-disable-legacy-logoff-script",
                Label = "Disable Legacy Logoff Script Delay",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets MaxWaitForScriptDelay=0 in the ShutdownOptions policy key. Sets "
                    + "the maximum time the system will wait for legacy Group Policy logoff "
                    + "or shutdown scripts to complete before forcing termination to 0 "
                    + "seconds. Long-running logoff scripts delay shutdown chains in VDI "
                    + "environments and can prevent clean hyper-visor-level snapshotting "
                    + "during overnight maintenance windows. Default: 600.",
                Tags = ["shutdown", "script", "logoff", "delay", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxWaitForScriptDelay", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxWaitForScriptDelay")],
                DetectOps = [RegOp.CheckDword(Key, "MaxWaitForScriptDelay", 0)],
            },
            new TweakDef
            {
                Id = "shtdwn-disable-forced-reboot-notification",
                Label = "Disable Forced Reboot Notification Banner",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableForcedRebootNotification=1 in the ShutdownOptions policy "
                    + "key. Suppresses the notification banner that warns users of an "
                    + "imminent forced restart scheduled by Windows Update or administrator "
                    + "policy. While intended to inform users, in unattended VDI and "
                    + "server contexts the notification triggers user-interactive dialogs "
                    + "that block automated shutdown orchestration scripts. Default: 0.",
                Tags = ["shutdown", "reboot", "notification", "wus", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableForcedRebootNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableForcedRebootNotification")],
                DetectOps = [RegOp.CheckDword(Key, "DisableForcedRebootNotification", 1)],
            },
            new TweakDef
            {
                Id = "shtdwn-disable-power-button-action",
                Label = "Disable Power Button Shutdown",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisablePowerButton=1 in the ShutdownOptions policy key. Prevents "
                    + "the physical power button from triggering a shutdown or hibernate "
                    + "action, regardless of the power plan's button-press setting. On "
                    + "point-of-sale terminals, kiosk devices, and embedded panels the "
                    + "power button is often inadvertently pressed during normal operation "
                    + "causing unexpected downtime. Default: 0.",
                Tags = ["shutdown", "power-button", "kiosk", "hardware", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePowerButton", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePowerButton")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePowerButton", 1)],
            },
            new TweakDef
            {
                Id = "shtdwn-disable-restart-button-start",
                Label = "Disable Restart Option in Start Menu",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoRestartFromStartMenu=1 in the ShutdownOptions policy key. "
                    + "Removes the Restart option from the Start Menu power button flyout, "
                    + "preventing standard users from restarting the system from the desktop. "
                    + "On thin-client terminals and locked-down workstations, restart should "
                    + "only be initiated by IT administrators through remote management "
                    + "tools or scheduled maintenance windows. Default: 0.",
                Tags = ["shutdown", "start-menu", "restart", "lockdown", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoRestartFromStartMenu", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoRestartFromStartMenu")],
                DetectOps = [RegOp.CheckDword(Key, "NoRestartFromStartMenu", 1)],
            },
            new TweakDef
            {
                Id = "shtdwn-disable-hibernate-option",
                Label = "Disable Hibernate Option in Shutdown Menu",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableHibernate=1 in the ShutdownOptions policy key. Removes "
                    + "the Hibernate entry from the shutdown and power flyout menus. "
                    + "Hibernate writes the full memory contents to the hiberfil.sys "
                    + "pagefile, which may contain credentials, encryption keys, and "
                    + "sensitive process data as unencrypted pages unless UEFI provides "
                    + "a sealed hibernation key. Default: 0. Recommended: 1 on shared machines.",
                Tags = ["shutdown", "hibernate", "security", "power", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableHibernate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableHibernate")],
                DetectOps = [RegOp.CheckDword(Key, "DisableHibernate", 1)],
            },
            new TweakDef
            {
                Id = "shtdwn-log-shutdown-events",
                Label = "Enable Shutdown Event Logging",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets LogShutdownEvents=1 in the ShutdownOptions policy key. Enables "
                    + "recording of shutdown, restart, and logoff events with user identity, "
                    + "timestamp, reason code, and any administrator comment to the "
                    + "Security event log. Event log evidence of shutdown sequences is "
                    + "critical for forensic timelines in incident response and for "
                    + "demonstrating change-control compliance in audits. Default: 0.",
                Tags = ["shutdown", "logging", "audit", "event-log", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LogShutdownEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LogShutdownEvents")],
                DetectOps = [RegOp.CheckDword(Key, "LogShutdownEvents", 1)],
            },
        ];
    }

    // ── SidebarGadgetsPolicy ──
    private static class _SidebarGadgetsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sidebar";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "sidebar-turn-off-sidebar",
                Label = "Sidebar Policy: Turn Off Windows Sidebar",
                Category = "Display — Personalization Lock",
                Description =
                    "Disables the Windows Sidebar and all desktop gadgets via Group Policy. Prevents users from running the sidebar process (Sidebar.exe) on Windows Vista/7/8 and legacy gadgets on Windows 10/11.",
                Tags = ["sidebar", "gadgets", "legacy", "disable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables legacy sidebar process; removes gadget execution surface.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TurnOffSidebar", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TurnOffSidebar")],
                DetectOps = [RegOp.CheckDword(Key, "TurnOffSidebar", 1)],
            },
            new TweakDef
            {
                Id = "sidebar-block-unsupported-packages",
                Label = "Sidebar Policy: Block Unsupported Gadget Packages",
                Category = "Display — Personalization Lock",
                Description =
                    "Prevents execution of gadget packages that are not explicitly supported by the installed Windows version. Unsupported gadget packages can contain vulnerabilities or unsigned code.",
                Tags = ["sidebar", "gadgets", "packages", "block", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents unsigned/unsupported gadget packages from running.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TurnOffUnsupportedPackages", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TurnOffUnsupportedPackages")],
                DetectOps = [RegOp.CheckDword(Key, "TurnOffUnsupportedPackages", 1)],
            },
            new TweakDef
            {
                Id = "sidebar-disable-user-gadgets",
                Label = "Sidebar Policy: Disable Per-User Gadget Execution",
                Category = "Display — Personalization Lock",
                Description =
                    "Prevents individual users from installing and running desktop gadgets. Removes gadgets from the right-click desktop context menu and disables the gadget installation dialog.",
                Tags = ["sidebar", "gadgets", "user", "disable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes per-user gadget installation from the desktop context menu.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableUserGadgets", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableUserGadgets")],
                DetectOps = [RegOp.CheckDword(Key, "DisableUserGadgets", 1)],
            },
            new TweakDef
            {
                Id = "sidebar-disable-auto-update",
                Label = "Sidebar Policy: Disable Gadget Metadata Auto-Update",
                Category = "Display — Personalization Lock",
                Description =
                    "Prevents Windows gadgets from automatically downloading updated metadata from the Windows Live Gallery or third-party gadget feeds. Reduces network activity and potential data exfiltration.",
                Tags = ["sidebar", "gadgets", "auto-update", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents gadgets from downloading live content updates.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TurnOffAutoUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TurnOffAutoUpdate")],
                DetectOps = [RegOp.CheckDword(Key, "TurnOffAutoUpdate", 1)],
            },
            new TweakDef
            {
                Id = "sidebar-block-from-running",
                Label = "Sidebar Policy: Block Sidebar Process from Running",
                Category = "Display — Personalization Lock",
                Description =
                    "Blocks the Windows Sidebar process (sidebar.exe) from launching. CVE-2013-0088 and other CVEs affect Windows gadgets — disabling the process is a defence-in-depth mitigation.",
                Tags = ["sidebar", "gadgets", "block", "process", "cve", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Mitigates CVE-2013-0088 and related gadget CVEs by blocking sidebar.exe execution.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockFromRunning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockFromRunning")],
                DetectOps = [RegOp.CheckDword(Key, "BlockFromRunning", 1)],
            },
            new TweakDef
            {
                Id = "sidebar-require-signed-packages",
                Label = "Sidebar Policy: Require Signed Gadget Packages",
                Category = "Display — Personalization Lock",
                Description =
                    "Enforces digital signature verification for all gadget packages before execution. Prevents loading of unsigned or tampered gadgets that could execute arbitrary code.",
                Tags = ["sidebar", "gadgets", "signatures", "signing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enforces digital signature verification before any gadget executes.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireGadgetSignatures", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireGadgetSignatures")],
                DetectOps = [RegOp.CheckDword(Key, "RequireGadgetSignatures", 1)],
            },
            new TweakDef
            {
                Id = "sidebar-disable-web-gadgets",
                Label = "Sidebar Policy: Disable Internet-Connected Gadgets",
                Category = "Display — Personalization Lock",
                Description =
                    "Prevents gadgets from connecting to the internet to fetch live content (weather, news, finance widgets). Eliminates a data exfiltration channel and mitigates web content injection risks.",
                Tags = ["sidebar", "gadgets", "web", "internet", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks gadgets from fetching live internet content; eliminates exfiltration channel.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableOnlineGadgetContent", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOnlineGadgetContent")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOnlineGadgetContent", 1)],
            },
            new TweakDef
            {
                Id = "sidebar-disable-desktop-gadgets",
                Label = "Sidebar Policy: Disable Desktop Gadgets",
                Category = "Display — Personalization Lock",
                Description =
                    "Removes desktop gadget functionality entirely, including the right-click 'Gadgets' menu entry on the desktop. Enforces a clean desktop policy on managed enterprise endpoints.",
                Tags = ["sidebar", "gadgets", "desktop", "disable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes desktop gadget functionality and context menu entry.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDesktopGadgets", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDesktopGadgets")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDesktopGadgets", 1)],
            },
            new TweakDef
            {
                Id = "sidebar-disable-third-party-gadgets",
                Label = "Sidebar Policy: Disable Third-Party Gadget Installation",
                Category = "Display — Personalization Lock",
                Description =
                    "Prevents users from installing gadgets from third-party sources or URLs. Restricts gadget sources to the built-in Windows Gallery only, reducing the attack surface via malicious gadget packages.",
                Tags = ["sidebar", "gadgets", "third-party", "installation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Restricts gadget sources to Windows Gallery only; blocks third-party malicious packages.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowThirdPartyGadgets", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowThirdPartyGadgets")],
                DetectOps = [RegOp.CheckDword(Key, "AllowThirdPartyGadgets", 0)],
            },
            new TweakDef
            {
                Id = "sidebar-disable-gadget-gallery",
                Label = "Sidebar Policy: Disable Gadget Gallery Access",
                Category = "Display — Personalization Lock",
                Description =
                    "Prevents access to the Windows Gadget Gallery (the built-in gadget browser). Removes the ability to browse and install new gadgets from both the OS gallery and online sources.",
                Tags = ["sidebar", "gadgets", "gallery", "lockdown", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents browsing and installing new gadgets from OS gallery and online sources.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableGadgetGallery", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableGadgetGallery")],
                DetectOps = [RegOp.CheckDword(Key, "DisableGadgetGallery", 1)],
            },
        ];
    }

    // ── StartMenuModernPolicy ──
    private static class _StartMenuModernPolicy
    {
        private const string ExplPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";

        private const string SmExp = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StartMenuExperience";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "smmod-disable-recent-apps-in-start",
                Label = "Start Menu: Disable recently added apps list in Start",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets DisableRecentAppsInStart=1 in StartMenuExperience policy. Hides the 'Recently "
                    + "added' section at the top of the Start menu application list.",
                Tags = ["start-menu", "recent-apps", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SmExp, "DisableRecentAppsInStart", 1)],
                RemoveOps = [RegOp.DeleteValue(SmExp, "DisableRecentAppsInStart")],
                DetectOps = [RegOp.CheckDword(SmExp, "DisableRecentAppsInStart", 1)],
            },
            new TweakDef
            {
                Id = "smmod-disable-frequently-used-programs",
                Label = "Start Menu: Disable frequently used programs list",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets NoFrequentUsedPrograms=1 in Explorer policy. Prevents Windows from tracking and "
                    + "displaying the most frequently launched applications in the Start menu.",
                Tags = ["start-menu", "frequent-apps", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(ExplPol, "NoFrequentUsedPrograms", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplPol, "NoFrequentUsedPrograms")],
                DetectOps = [RegOp.CheckDword(ExplPol, "NoFrequentUsedPrograms", 1)],
            },
            new TweakDef
            {
                Id = "smmod-hide-people-bar",
                Label = "Taskbar: Hide the People bar (contacts flyout)",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets HidePeopleBar=1 in StartMenuExperience policy. Removes the People button from "
                    + "the taskbar, hiding the contacts/people flyout feature.",
                Tags = ["taskbar", "people", "contacts", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SmExp, "HidePeopleBar", 1)],
                RemoveOps = [RegOp.DeleteValue(SmExp, "HidePeopleBar")],
                DetectOps = [RegOp.CheckDword(SmExp, "HidePeopleBar", 1)],
            },
            new TweakDef
            {
                Id = "smmod-disable-start-recent-docs",
                Label = "Start Menu: Disable recent documents history in Start",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets NoRecentDocsMenu=1 in Explorer policy. Stops Windows from tracking and showing "
                    + "a recent-documents shortcut list in the Start menu and jump lists.",
                Tags = ["start-menu", "recent-docs", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(ExplPol, "NoRecentDocsMenu", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplPol, "NoRecentDocsMenu")],
                DetectOps = [RegOp.CheckDword(ExplPol, "NoRecentDocsMenu", 1)],
            },
            new TweakDef
            {
                Id = "smmod-disable-start-app-suggestions",
                Label = "Start Menu: Disable app suggestions / promoted apps in Start",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets DisableRecommendedAppsInStart=1 in StartMenuExperience policy. Removes "
                    + "Microsoft-promoted app suggestions and advertisements from the Start menu.",
                Tags = ["start-menu", "suggestions", "ads", "debloat", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SmExp, "DisableRecommendedAppsInStart", 1)],
                RemoveOps = [RegOp.DeleteValue(SmExp, "DisableRecommendedAppsInStart")],
                DetectOps = [RegOp.CheckDword(SmExp, "DisableRecommendedAppsInStart", 1)],
            },
            new TweakDef
            {
                Id = "smmod-disable-start-recommended-section",
                Label = "Start Menu: Disable the Recommended section in Windows 11 Start",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets DisableRecommendedItemsInStart=1 in StartMenuExperience policy. Hides the "
                    + "'Recommended' tile area at the bottom of the Windows 11 Start menu.",
                Tags = ["start-menu", "recommended", "w11", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SmExp, "DisableRecommendedItemsInStart", 1)],
                RemoveOps = [RegOp.DeleteValue(SmExp, "DisableRecommendedItemsInStart")],
                DetectOps = [RegOp.CheckDword(SmExp, "DisableRecommendedItemsInStart", 1)],
            },
            new TweakDef
            {
                Id = "smmod-disable-preview-pane",
                Label = "Explorer: Disable the Preview Pane in File Explorer",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets NoPreviewPane=1 in Explorer policy. Disables the Preview Pane panel that "
                    + "renders a file preview on the right side of File Explorer windows.",
                Tags = ["explorer", "preview-pane", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(ExplPol, "NoPreviewPane", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplPol, "NoPreviewPane")],
                DetectOps = [RegOp.CheckDword(ExplPol, "NoPreviewPane", 1)],
            },
            new TweakDef
            {
                Id = "smmod-disable-details-pane",
                Label = "Explorer: Disable the Details Pane in File Explorer",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets NoDetailsPane=1 in Explorer policy. Removes the Details Pane that displays "
                    + "file metadata (size, dates, author) on the right side of File Explorer.",
                Tags = ["explorer", "details-pane", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(ExplPol, "NoDetailsPane", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplPol, "NoDetailsPane")],
                DetectOps = [RegOp.CheckDword(ExplPol, "NoDetailsPane", 1)],
            },
            new TweakDef
            {
                Id = "smmod-disable-taskbar-msa-notification",
                Label = "Taskbar: Disable MSA sign-in notification badge on taskbar",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets DisableMSANotification=1 in StartMenuExperience policy. Suppresses the "
                    + "notification badge that prompts users to sign in with a Microsoft Account.",
                Tags = ["taskbar", "msa", "notification", "debloat", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SmExp, "DisableMSANotification", 1)],
                RemoveOps = [RegOp.DeleteValue(SmExp, "DisableMSANotification")],
                DetectOps = [RegOp.CheckDword(SmExp, "DisableMSANotification", 1)],
            },
            new TweakDef
            {
                Id = "smmod-no-machine-boot-uninstall",
                Label = "Start Menu: Preserve pinned items across machine boot (no uninstall prompt)",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets NoMachineBootUninstall=1 in Explorer policy. Prevents Windows from prompting to "
                    + "remove pinned Start menu items for apps that were uninstalled on another user profile.",
                Tags = ["start-menu", "pin", "uninstall", "boot", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(ExplPol, "NoMachineBootUninstall", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplPol, "NoMachineBootUninstall")],
                DetectOps = [RegOp.CheckDword(ExplPol, "NoMachineBootUninstall", 1)],
            },
        ];
    }

    // ── SudoWindowsPolicy ──
    private static class _SudoWindowsPolicy
    {
        private const string SudoKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sudo";
        private const string ElevationConfigKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ElevationConfig";
        private const string UacPoliciesKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\UAC";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sudopol-disable-sudo",
                    Label = "Disable Sudo for Windows",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Prevents users from using the 'sudo' command in Windows to run programs with elevated privileges from a standard terminal. Enforces traditional UAC elevation only.",
                    Tags = ["sudo", "elevation", "uac", "security", "windows-11"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Prevents privilege escalation via sudo from standard terminals; users must use dedicated elevated sessions.",
                    RegistryKeys = [SudoKey],
                    ApplyOps = [RegOp.SetDword(SudoKey, "Enabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(SudoKey, "Enabled")],
                    DetectOps = [RegOp.CheckDword(SudoKey, "Enabled", 0)],
                },
                new TweakDef
                {
                    Id = "sudopol-force-new-window",
                    Label = "Force Sudo to Open New Window",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "When sudo is permitted, forces elevated processes to launch in a new, separate console window rather than running inline in the calling shell. Increases visibility of elevated sessions.",
                    Tags = ["sudo", "elevation", "new-window", "uac", "windows-11"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Elevated process runs in a clearly separate window, reducing confusion about which shell context is privileged.",
                    RegistryKeys = [SudoKey],
                    ApplyOps = [RegOp.SetDword(SudoKey, "SudoMode", 0)],
                    RemoveOps = [RegOp.DeleteValue(SudoKey, "SudoMode")],
                    DetectOps = [RegOp.CheckDword(SudoKey, "SudoMode", 0)],
                },
                new TweakDef
                {
                    Id = "sudopol-disable-inline-mode",
                    Label = "Disable Sudo Inline Execution Mode",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Prevents the 'inline' execution mode of sudo where the elevated process shares the calling terminal session. Inline mode can mask privilege escalation; this policy requires isolated execution.",
                    Tags = ["sudo", "elevation", "inline-mode", "security", "windows-11"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Inline sudo blurs the boundary between privileged and non-privileged sessions; disabling it is recommended for corporate environments.",
                    RegistryKeys = [SudoKey],
                    ApplyOps = [RegOp.SetDword(SudoKey, "AllowInlineMode", 0)],
                    RemoveOps = [RegOp.DeleteValue(SudoKey, "AllowInlineMode")],
                    DetectOps = [RegOp.CheckDword(SudoKey, "AllowInlineMode", 0)],
                },
                new TweakDef
                {
                    Id = "sudopol-disable-input-disabled-mode",
                    Label = "Disable Sudo Input-Disabled Mode",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Prevents the 'input disabled' mode of sudo, which runs an elevated process with stdin closed. This mode is useful for non-interactive elevated scripts but may bypass certain security controls.",
                    Tags = ["sudo", "elevation", "input-disabled", "security", "windows-11"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Prevents automated elevated scripts from running silently via sudo in environments where operator oversight is required.",
                    RegistryKeys = [SudoKey],
                    ApplyOps = [RegOp.SetDword(SudoKey, "AllowInputDisabledMode", 0)],
                    RemoveOps = [RegOp.DeleteValue(SudoKey, "AllowInputDisabledMode")],
                    DetectOps = [RegOp.CheckDword(SudoKey, "AllowInputDisabledMode", 0)],
                },
                new TweakDef
                {
                    Id = "sudopol-require-admin-group-membership",
                    Label = "Restrict Sudo to Local Administrators Group",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Ensures that only users who are members of the local Administrators group can use sudo for Windows, preventing standard users from attempting elevation via sudo.",
                    Tags = ["sudo", "elevation", "admin-group", "access-control", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Provides an explicit access gate: even if sudo is enabled on the device, non-admin users receive a denial.",
                    RegistryKeys = [SudoKey],
                    ApplyOps = [RegOp.SetDword(SudoKey, "RequireAdminGroupMembership", 1)],
                    RemoveOps = [RegOp.DeleteValue(SudoKey, "RequireAdminGroupMembership")],
                    DetectOps = [RegOp.CheckDword(SudoKey, "RequireAdminGroupMembership", 1)],
                },
                new TweakDef
                {
                    Id = "sudopol-enable-audit-events",
                    Label = "Enable Sudo Elevation Audit Events",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Configures Windows to write an audit log entry whenever a process is elevated via sudo. Audit entries include the calling user, the target executable, and the elevation timestamp.",
                    Tags = ["sudo", "elevation", "audit", "compliance", "event-log"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Produces an accountable record of every sudo elevation, supporting incident response and SOC monitoring.",
                    RegistryKeys = [SudoKey],
                    ApplyOps = [RegOp.SetDword(SudoKey, "EnableAuditEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(SudoKey, "EnableAuditEvents")],
                    DetectOps = [RegOp.CheckDword(SudoKey, "EnableAuditEvents", 1)],
                },
                new TweakDef
                {
                    Id = "sudopol-block-network-elevated-processes",
                    Label = "Block Elevated Processes from Accessing Network",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Restricts network access for processes elevated via sudo, preventing elevated shells from making outbound network connections. Limits lateral movement potential from elevated contexts.",
                    Tags = ["sudo", "elevation", "network", "security", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "An elevated process with network access can pivot to other systems; this policy reduces the blast radius of a compromised sudo-elevated session.",
                    RegistryKeys = [ElevationConfigKey],
                    ApplyOps = [RegOp.SetDword(ElevationConfigKey, "BlockNetworkFromElevatedProcesses", 1)],
                    RemoveOps = [RegOp.DeleteValue(ElevationConfigKey, "BlockNetworkFromElevatedProcesses")],
                    DetectOps = [RegOp.CheckDword(ElevationConfigKey, "BlockNetworkFromElevatedProcesses", 1)],
                },
                new TweakDef
                {
                    Id = "sudopol-enforce-credential-prompt",
                    Label = "Always Prompt for Credentials on Sudo Elevation",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Requires the user to enter explicit credentials (password or Windows Hello) before each sudo elevation, even within an existing authenticated session. Prevents silent re-elevation.",
                    Tags = ["sudo", "elevation", "credential-prompt", "uac", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Satisfies 'explicit approval' audit requirements by ensuring the user actively authenticates for each elevated action.",
                    RegistryKeys = [ElevationConfigKey],
                    ApplyOps = [RegOp.SetDword(ElevationConfigKey, "AlwaysPromptCredentialsOnElevation", 1)],
                    RemoveOps = [RegOp.DeleteValue(ElevationConfigKey, "AlwaysPromptCredentialsOnElevation")],
                    DetectOps = [RegOp.CheckDword(ElevationConfigKey, "AlwaysPromptCredentialsOnElevation", 1)],
                },
                new TweakDef
                {
                    Id = "sudopol-log-elevated-command-line",
                    Label = "Log Command-Line Arguments for Sudo-Elevated Processes",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Enables command-line logging for all processes elevated through sudo, recording the full command line in the Windows event log. Aids forensic investigation of elevation abuse.",
                    Tags = ["sudo", "elevation", "command-line", "audit", "forensics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Command-line data in event logs reveals what was actually run with elevated privileges, not just that elevation occurred.",
                    RegistryKeys = [ElevationConfigKey],
                    ApplyOps = [RegOp.SetDword(ElevationConfigKey, "LogElevatedCommandLine", 1)],
                    RemoveOps = [RegOp.DeleteValue(ElevationConfigKey, "LogElevatedCommandLine")],
                    DetectOps = [RegOp.CheckDword(ElevationConfigKey, "LogElevatedCommandLine", 1)],
                },
                new TweakDef
                {
                    Id = "sudopol-block-unapproved-shells",
                    Label = "Block Sudo Elevation from Unapproved Shell Hosts",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Restricts sudo elevation to approved shell host executables (Windows Terminal, PowerShell 7, cmd.exe). Prevents elevation from unusual hosts such as scripting engines or third-party terminals.",
                    Tags = ["sudo", "elevation", "shell", "allowlist", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Reduces attack surface by ensuring only known-good terminal applications can initiate sudo elevation requests.",
                    RegistryKeys = [UacPoliciesKey],
                    ApplyOps = [RegOp.SetDword(UacPoliciesKey, "RestrictSudoToApprovedHosts", 1)],
                    RemoveOps = [RegOp.DeleteValue(UacPoliciesKey, "RestrictSudoToApprovedHosts")],
                    DetectOps = [RegOp.CheckDword(UacPoliciesKey, "RestrictSudoToApprovedHosts", 1)],
                },
            ];
    }

    // ── SystemShutdown ──
    private static class _SystemShutdown
    {
        private const string WinLogon = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";

        private const string CurrentVersion = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion";

        private const string PoliciesSystem = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

        private const string PoliciesExplorer = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";

        private const string PowerSettings = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power";

        private const string SessionManager = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "shdn-reduce-wait-to-kill-timeout",
                Label = "Reduce WaitToKillServiceTimeout to 5 Seconds",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["shutdown", "service", "kill timeout", "speed"],
                Description =
                    "Reduces the time Windows waits for services to stop during shutdown "
                    + "from the default 20,000 ms to 5,000 ms. Speeds up shutdown at the "
                    + "cost of slightly less graceful service termination.",
                ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "WaitToKillServiceTimeout", "5000")],
                RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "WaitToKillServiceTimeout", "20000")],
                DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "WaitToKillServiceTimeout", "5000")],
            },
            new TweakDef
            {
                Id = "shdn-reduce-hung-app-timeout",
                Label = "Reduce HungAppTimeout to 4 Seconds",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["shutdown", "hung app", "timeout", "speed"],
                Description =
                    "Reduces the time Windows waits before showing the 'This application is "
                    + "not responding' prompt during shutdown. HungAppTimeout=4000 ms "
                    + "(default is 5000 ms). Slightly quicker dialog trigger.",
                ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "4000")],
                RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "5000")],
                DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "4000")],
            },
            new TweakDef
            {
                Id = "shdn-enable-auto-end-tasks",
                Label = "Enable AutoEndTasks (Kill Hung Apps on Logout)",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["shutdown", "auto end tasks", "hung app", "logout"],
                Description =
                    "Enables AutoEndTasks=1 so Windows automatically terminates applications "
                    + "that are hanging during logout or shutdown without waiting for user "
                    + "confirmation. Speeds up shutdown considerably.",
                ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "AutoEndTasks", "1")],
                RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "AutoEndTasks", "0")],
                DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "AutoEndTasks", "1")],
            },
            new TweakDef
            {
                Id = "shdn-disable-shutdown-event-tracker",
                Label = "Disable Shutdown Event Tracker (No Reason Required)",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["shutdown", "event tracker", "policy", "server"],
                Description =
                    "Disables the Shutdown Event Tracker that asks administrators why they "
                    + "are shutting down or restarting. Normally enabled only on Windows Server. "
                    + "ShutdownReasonUI=0.",
                ApplyOps = [RegOp.SetDword(PoliciesSystem, "ShutdownReasonUI", 0)],
                RemoveOps = [RegOp.DeleteValue(PoliciesSystem, "ShutdownReasonUI")],
                DetectOps = [RegOp.CheckDword(PoliciesSystem, "ShutdownReasonUI", 0)],
            },
            new TweakDef
            {
                Id = "shdn-suppress-logoff-scripts-run-at-shutdown",
                Label = "Run Logoff Scripts Simultaneously with Shutdown Scripts",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["shutdown", "logoff", "scripts", "gpo"],
                Description =
                    "Configures logoff and shutdown scripts to run simultaneously rather "
                    + "than sequentially. Reduces total script execution time during logout. "
                    + "RunLogonScriptSync=0.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "RunLogonScriptSync", 0)],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "RunLogonScriptSync"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "RunLogonScriptSync", 0),
                ],
            },
            new TweakDef
            {
                Id = "shdn-suppress-logoff-slow-scripts-ui",
                Label = "Disable 'Slow Script' Warning at Shutdown",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Tags = ["shutdown", "gpo", "slow script", "ui"],
                Description =
                    "Hides the 'Please wait for the <script> to finish' message shown when "
                    + "GPO logoff/shutdown scripts exceed MaxGPOScriptWait. Prevents the UI "
                    + "from blocking shutdown on domain machines with slow scripts.",
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideShutdownScripts", 0),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideShutdownScripts"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideShutdownScripts", 0),
                ],
            },
        ];
    }

    // ── TabletPcInputPolicy ──
    private static class _TabletPcInputPolicy
    {
        private const string TabPC = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC";
        private const string TabWin = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "tabpol-prevent-handwriting-data-sharing",
                Label = "Prevent Handwriting Data Sharing with Microsoft",
                Category = "Display — Start Menu Modern",
                Description = "Prevents Windows from sharing handwriting recognition data with Microsoft to improve handwriting recognition.",
                Tags = ["tablet", "privacy", "handwriting", "group-policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabPC, "PreventHandwritingDataSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(TabPC, "PreventHandwritingDataSharing")],
                DetectOps = [RegOp.CheckDword(TabPC, "PreventHandwritingDataSharing", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-prevent-handwriting-error-reports",
                Label = "Prevent Handwriting Error Reporting",
                Category = "Display — Start Menu Modern",
                Description = "Stops Windows from sending handwriting recognition error reports to Microsoft.",
                Tags = ["tablet", "privacy", "handwriting", "group-policy", "telemetry"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabPC, "PreventHandwritingErrorReports", 1)],
                RemoveOps = [RegOp.DeleteValue(TabPC, "PreventHandwritingErrorReports")],
                DetectOps = [RegOp.CheckDword(TabPC, "PreventHandwritingErrorReports", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-disable-ink-ball-game",
                Label = "Disable InkBall Game",
                Category = "Display — Start Menu Modern",
                Description = "Removes the InkBall game from the Start menu and blocks access via Group Policy.",
                Tags = ["tablet", "debloat", "group-policy", "games"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabPC, "DisableInkBall", 1)],
                RemoveOps = [RegOp.DeleteValue(TabPC, "DisableInkBall")],
                DetectOps = [RegOp.CheckDword(TabPC, "DisableInkBall", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-turn-off-passwordless",
                Label = "Turn Off Tablet PC Passwordless Experience",
                Category = "Display — Start Menu Modern",
                Description = "Disables the passwordless login experience on Tablet PC, requiring a password for sign-in.",
                Tags = ["tablet", "security", "group-policy", "login"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabPC, "TurnOffPwdlessExperience", 1)],
                RemoveOps = [RegOp.DeleteValue(TabPC, "TurnOffPwdlessExperience")],
                DetectOps = [RegOp.CheckDword(TabPC, "TurnOffPwdlessExperience", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-prevent-handwriting-personalization",
                Label = "Prevent Handwriting Personalization Collection",
                Category = "Display — Start Menu Modern",
                Description =
                    "Blocks Windows from collecting typed and handwriting data to build a personalized dictionary for handwriting recognition.",
                Tags = ["tablet", "privacy", "handwriting", "group-policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabWin, "PreventHandwritingPersonalization", 1)],
                RemoveOps = [RegOp.DeleteValue(TabWin, "PreventHandwritingPersonalization")],
                DetectOps = [RegOp.CheckDword(TabWin, "PreventHandwritingPersonalization", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-disable-pen-training-support",
                Label = "Disable Pen Training and Support",
                Category = "Display — Start Menu Modern",
                Description = "Turns off the Tablet PC pen training and pen support documentation from the Tablet PC optional components.",
                Tags = ["tablet", "debloat", "group-policy", "pen"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabWin, "DisablePenTrainingAndSupport", 1)],
                RemoveOps = [RegOp.DeleteValue(TabWin, "DisablePenTrainingAndSupport")],
                DetectOps = [RegOp.CheckDword(TabWin, "DisablePenTrainingAndSupport", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-turn-off-pen-feedback",
                Label = "Turn Off Pen Haptic Feedback",
                Category = "Display — Start Menu Modern",
                Description = "Disables haptic and visual ink feedback when using a digital pen on a touch display.",
                Tags = ["tablet", "pen", "group-policy", "input"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabWin, "TurnOffPenFeedback", 1)],
                RemoveOps = [RegOp.DeleteValue(TabWin, "TurnOffPenFeedback")],
                DetectOps = [RegOp.CheckDword(TabWin, "TurnOffPenFeedback", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-disable-touch-input",
                Label = "Disable Touch Input (Tablet PC Policy)",
                Category = "Display — Start Menu Modern",
                Description =
                    "Disables all touch-based input processing via Group Policy. Useful for kiosk or hardened deployments without touch screens.",
                Tags = ["tablet", "touch", "group-policy", "input", "kiosk"],
                NeedsAdmin = true,
                CorpSafe = false,
                ApplyOps = [RegOp.SetDword(TabWin, "DisableTouchInput", 1)],
                RemoveOps = [RegOp.DeleteValue(TabWin, "DisableTouchInput")],
                DetectOps = [RegOp.CheckDword(TabWin, "DisableTouchInput", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-disable-touchscreen-scroll",
                Label = "Disable Touchscreen Panning and Scrolling Inertia",
                Category = "Display — Start Menu Modern",
                Description =
                    "Disables momentum scrolling (inertia) and panning on touchscreens to reduce accidental scrolling in productivity apps.",
                Tags = ["tablet", "touch", "group-policy", "input"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabWin, "DisablePanningFeedback", 1)],
                RemoveOps = [RegOp.DeleteValue(TabWin, "DisablePanningFeedback")],
                DetectOps = [RegOp.CheckDword(TabWin, "DisablePanningFeedback", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-disable-flick-gestures",
                Label = "Disable Pen and Touch Flick Gestures",
                Category = "Display — Start Menu Modern",
                Description = "Disables pen and touch flick gestures (quick swipe shortcuts) system-wide via Group Policy.",
                Tags = ["tablet", "touch", "pen", "group-policy", "input"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabWin, "DisableFlicksFeature", 1)],
                RemoveOps = [RegOp.DeleteValue(TabWin, "DisableFlicksFeature")],
                DetectOps = [RegOp.CheckDword(TabWin, "DisableFlicksFeature", 1)],
            },
        ];
    }

    // ── TouchpadGestures ──
    private static class _TouchpadGestures
    {
        private const string Ptp = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad";

        private const string PtpSettings = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad\Settings";

        private const string EaseTouchpad = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad\Status";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "tpad-disable-three-finger-slide-task-view",
                Label = "Disable Three-Finger Slide (Task View / Switch Apps)",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["touchpad", "gestures", "three-finger", "task view"],
                Description =
                    "Disables the three-finger swipe gesture that triggers Task View on "
                    + "upward swipe or switches apps on horizontal swipe. "
                    + "Value 0 = disabled for three-finger swipe actions.",
                ApplyOps = [RegOp.SetDword(Ptp, "ThreeFingerSlideEnabled", 0)],
                RemoveOps = [RegOp.SetDword(Ptp, "ThreeFingerSlideEnabled", 1)],
                DetectOps = [RegOp.CheckDword(Ptp, "ThreeFingerSlideEnabled", 0)],
            },
            new TweakDef
            {
                Id = "tpad-disable-four-finger-slide",
                Label = "Disable Four-Finger Slide (Virtual Desktop Navigation)",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["touchpad", "gestures", "four-finger", "virtual desktops"],
                Description =
                    "Disables the four-finger horizontal swipe that switches between virtual "
                    + "desktops. Useful on compact touchpads where four-finger gestures are "
                    + "easily triggered accidentally.",
                ApplyOps = [RegOp.SetDword(Ptp, "FourFingerSlideEnabled", 0)],
                RemoveOps = [RegOp.SetDword(Ptp, "FourFingerSlideEnabled", 1)],
                DetectOps = [RegOp.CheckDword(Ptp, "FourFingerSlideEnabled", 0)],
            },
            new TweakDef
            {
                Id = "tpad-reverse-scroll-direction",
                Label = "Enable Reverse (Natural) Scroll Direction",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Tags = ["touchpad", "scroll", "natural scroll", "direction"],
                Description =
                    "Reverses the touchpad scroll direction to match natural/trackpad-style "
                    + "scrolling (content follows finger direction, like macOS). "
                    + "ReverseScrollingEnabled=1.",
                ApplyOps = [RegOp.SetDword(Ptp, "ReverseScrollingEnabled", 1)],
                RemoveOps = [RegOp.SetDword(Ptp, "ReverseScrollingEnabled", 0)],
                DetectOps = [RegOp.CheckDword(Ptp, "ReverseScrollingEnabled", 1)],
            },
            new TweakDef
            {
                Id = "tpad-disable-pinch-to-zoom",
                Label = "Disable Pinch-to-Zoom Gesture",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["touchpad", "pinch", "zoom", "gesture"],
                Description =
                    "Disables the pinch-to-zoom gesture on Precision Touchpad. Useful for "
                    + "users who accidentally trigger zoom while scrolling or using two-finger "
                    + "gestures. ZoomEnabled=0.",
                ApplyOps = [RegOp.SetDword(Ptp, "ZoomEnabled", 0)],
                RemoveOps = [RegOp.SetDword(Ptp, "ZoomEnabled", 1)],
                DetectOps = [RegOp.CheckDword(Ptp, "ZoomEnabled", 0)],
            },
            new TweakDef
            {
                Id = "tpad-set-sensitivity-most-sensitive",
                Label = "Set Touchpad Sensitivity to Most Sensitive",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["touchpad", "sensitivity", "cursor", "speed"],
                Description =
                    "Sets touchpad cursor speed/sensitivity to maximum (10) for fast, "
                    + "responsive cursor movement across large displays. "
                    + "CursorSpeed=10.",
                ApplyOps = [RegOp.SetDword(Ptp, "CursorSpeed", 10)],
                RemoveOps = [RegOp.SetDword(Ptp, "CursorSpeed", 5)],
                DetectOps = [RegOp.CheckDword(Ptp, "CursorSpeed", 10)],
            },
            new TweakDef
            {
                Id = "tpad-disable-edge-swipe",
                Label = "Disable Edge Swipe for Action Center / Widgets",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["touchpad", "edge swipe", "action center", "widgets"],
                Description =
                    "Disables the right-edge swipe gesture that opens the Action Center "
                    + "and swipe from left that shows widgets. Prevents accidental panel "
                    + "openings on touch-sensitive laptop bezels. "
                    + "EdgeEnabled=0.",
                ApplyOps = [RegOp.SetDword(Ptp, "EdgeEnabled", 0)],
                RemoveOps = [RegOp.SetDword(Ptp, "EdgeEnabled", 1)],
                DetectOps = [RegOp.CheckDword(Ptp, "EdgeEnabled", 0)],
            },
        ];
    }

    // ── VideoCapturePolicy ──
    private static class _VideoCapturePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\VideoCapture";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "vcap-disable-video-capture",
                Label = "Disable Video Capture Device Access",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Sets DisableVideoCapture=1 in the VideoCapture policy key. Blocks all "
                    + "application-level access to video capture hardware (webcams, capture cards, "
                    + "virtual cameras). Applications requesting the Camera device class are denied "
                    + "at the policy layer before the privacy permission prompt. Stronger than "
                    + "per-app toggles; applies universally. Default: 0. Recommended: 1 on "
                    + "kiosk, conference room, or regulated-data machines.",
                Tags = ["video-capture", "camera", "webcam", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableVideoCapture", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableVideoCapture")],
                DetectOps = [RegOp.CheckDword(Key, "DisableVideoCapture", 1)],
            },
            new TweakDef
            {
                Id = "vcap-disable-screen-capture",
                Label = "Disable Screen Capture via Policy",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Sets DisableScreenCapture=1 in the VideoCapture policy key. Prevents "
                    + "applications from using screen-capture APIs (Desktop Duplication API, "
                    + "Graphics.CaptureItem) to record the screen contents. Blocks tools such "
                    + "as OBS, Teams recording, and screenshot utilities at the policy layer. "
                    + "Default: 0. Recommended: 1 on machines handling sensitive classified "
                    + "or commercially confidential data.",
                Tags = ["video-capture", "screen-capture", "screenshot", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableScreenCapture", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableScreenCapture")],
                DetectOps = [RegOp.CheckDword(Key, "DisableScreenCapture", 1)],
            },
            new TweakDef
            {
                Id = "vcap-disable-broadcast",
                Label = "Disable Live Broadcast Capture",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableBroadcast=1 in the VideoCapture policy key. Blocks applications "
                    + "from using Windows broadcast APIs to stream game or desktop content to "
                    + "external platforms (Twitch, YouTube, Beam). These broadcast sessions can "
                    + "inadvertently expose corporate data if a game running on a managed device "
                    + "captures an adjacent application window. "
                    + "Default: 0. Recommended: 1 on corporate gaming or shared workstations.",
                Tags = ["video-capture", "broadcast", "streaming", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBroadcast", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBroadcast")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBroadcast", 1)],
            },
            new TweakDef
            {
                Id = "vcap-disable-game-capture",
                Label = "Disable Game DVR-style Video Capture",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableGameCapture=1 in the VideoCapture policy key. Prevents the "
                    + "GameDVR / Xbox Game Bar capture subsystem from recording gameplay video "
                    + "clips and screenshots via the VideoCapture pipeline. Frees GPU encoder "
                    + "headroom reserved for background capture. This policy targets the capture "
                    + "backend, supplementing the GameDVR GP setting which only hides the UI. "
                    + "Default: 0. Recommended: 1 on non-gaming workstations.",
                Tags = ["video-capture", "game-dvr", "xbox", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableGameCapture", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableGameCapture")],
                DetectOps = [RegOp.CheckDword(Key, "DisableGameCapture", 1)],
            },
            new TweakDef
            {
                Id = "vcap-disable-audio-capture",
                Label = "Disable Audio Capture with Video",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableAudioCapture=1 in the VideoCapture policy key. Prevents "
                    + "applications from pairing microphone or system-audio capture with video "
                    + "capture sessions. Without audio capture, recording tools can still "
                    + "capture video but produce silent clips. Reduces the exposure surface "
                    + "for audio-based eavesdropping via legitimate recording applications. "
                    + "Default: 0. Recommended: 1 on open-plan or regulated office seats.",
                Tags = ["video-capture", "audio", "microphone", "recording", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAudioCapture", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAudioCapture")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAudioCapture", 1)],
            },
            new TweakDef
            {
                Id = "vcap-require-admin-for-capture",
                Label = "Require Admin Rights for Video Capture",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets RequireAdminForCapture=1 in the VideoCapture policy key. Elevates "
                    + "the required privilege level for video capture operations so that only "
                    + "processes running with administrative rights can activate capture devices. "
                    + "Standard user applications, including browser-based conferencing tools, "
                    + "cannot access capture without elevation. Effective on shared machines. "
                    + "Default: 0. Recommended: 1 on high-security shared workstations.",
                Tags = ["video-capture", "admin", "privilege", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireAdminForCapture", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForCapture")],
                DetectOps = [RegOp.CheckDword(Key, "RequireAdminForCapture", 1)],
            },
            new TweakDef
            {
                Id = "vcap-disable-camera-telemetry",
                Label = "Disable Camera Capture Telemetry",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableCaptureTelemetry=1 in the VideoCapture policy key. Stops "
                    + "Windows from sending camera and capture device usage events to Microsoft. "
                    + "These events include which applications activated the camera, session "
                    + "duration, and device identifiers. The data informs Windows quality "
                    + "improvements but may be unwanted on privacy-sensitive deployments. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["video-capture", "telemetry", "camera", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCaptureTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCaptureTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCaptureTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "vcap-disable-virtual-camera",
                Label = "Disable Virtual Camera Device Access",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets DisableVirtualCamera=1 in the VideoCapture policy key. Blocks "
                    + "applications from accessing virtual camera devices installed by software "
                    + "such as OBS Virtual Camera, NDI Tools, or ManyCam. Virtual cameras can "
                    + "function as a transparency layer that bypasses physical camera policies "
                    + "by injecting pre-recorded or manipulated video into conferencing tools. "
                    + "Default: 0. Recommended: 1 on compliance-requiring conferencing setups.",
                Tags = ["video-capture", "virtual-camera", "obs", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableVirtualCamera", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableVirtualCamera")],
                DetectOps = [RegOp.CheckDword(Key, "DisableVirtualCamera", 1)],
            },
            new TweakDef
            {
                Id = "vcap-disable-media-capture-api",
                Label = "Disable MediaCapture API for UWP Apps",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableMediaCaptureAPI=1 in the VideoCapture policy key. Prevents "
                    + "UWP applications from using the Windows.Media.Capture.MediaCapture API "
                    + "to access camera and microphone hardware. Most modern Microsoft Store "
                    + "conferencing and imaging apps use this API. Blocking it forces those apps "
                    + "to request fallback devices or fail gracefully. "
                    + "Default: 0. Recommended: 1 on locked-down app environments.",
                Tags = ["video-capture", "uwp", "media-capture", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMediaCaptureAPI", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMediaCaptureAPI")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMediaCaptureAPI", 1)],
            },
            new TweakDef
            {
                Id = "vcap-disable-background-capture",
                Label = "Disable Background Video Capture",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableBackgroundCapture=1 in the VideoCapture policy key. Prevents "
                    + "applications that have been sent to the background from continuing to "
                    + "hold an active video capture session. Normally a minimised app retains "
                    + "the camera even when the user switches away. This policy releases the "
                    + "device when the capturing application loses focus, ensuring the camera "
                    + "indicator light extinguishes. Default: 0. Recommended: 1.",
                Tags = ["video-capture", "background", "camera", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBackgroundCapture", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBackgroundCapture")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBackgroundCapture", 1)],
            },
        ];
    }

    // ── VirtualKeyboardPolicy ──
    private static class _VirtualKeyboardPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\VirtualKeyboard";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "vkbd-disable-touch-keyboard",
                Label = "Disable Automatic Touch Keyboard Pop-Up",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableTouchKeyboard=1 in the VirtualKeyboard policy key. Prevents "
                    + "the Windows touch keyboard from appearing automatically when the user "
                    + "taps on a text input field in tablet mode or when no physical keyboard "
                    + "is detected. On hybrid devices used in docked/keyboard mode the "
                    + "automatic pop-up interrupts workflows and requires manual dismissal. "
                    + "Default: 0. Recommended: 1 on non-tablet machines.",
                Tags = ["touch", "keyboard", "virtual", "tablet", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTouchKeyboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTouchKeyboard")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTouchKeyboard", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-emoji-panel",
                Label = "Disable Emoji Panel (Win+.)",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableEmojiPanel=1 in the VirtualKeyboard policy key. Removes the "
                    + "emoji and special-characters picker that opens via Windows + period (.) "
                    + "or Windows + semicolon (;). On production workstations the emoji panel "
                    + "is an unnecessary distraction; the keyboard shortcut is easily triggered "
                    + "accidentally during fast typing. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["emoji", "panel", "keyboard", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableEmojiPanel", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableEmojiPanel")],
                DetectOps = [RegOp.CheckDword(Key, "DisableEmojiPanel", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-keyboard-sound",
                Label = "Disable Virtual Keyboard Key Click Sound",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets DisableKeyboardSound=1 in the VirtualKeyboard policy key. Mutes the "
                    + "click sound effect played each time a key on the on-screen touch keyboard "
                    + "is pressed. In quiet office or conference environments the click sounds "
                    + "are disruptive; the system-wide policy prevents users from re-enabling "
                    + "them. Default: 0. Recommended: 1.",
                Tags = ["keyboard", "sound", "click", "virtual", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableKeyboardSound", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableKeyboardSound")],
                DetectOps = [RegOp.CheckDword(Key, "DisableKeyboardSound", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-handwriting-button",
                Label = "Disable Touch Keyboard Handwriting Button",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableHandwritingButton=1 in the VirtualKeyboard policy key. Removes "
                    + "the stylus/pen button from the touch keyboard toolbar that switches "
                    + "from the key grid to the free-form handwriting input mode. On devices "
                    + "without a digitiser pen, the button serves no purpose and confuses "
                    + "users who activate it by mistake. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["keyboard", "handwriting", "button", "virtual", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableHandwritingButton", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableHandwritingButton")],
                DetectOps = [RegOp.CheckDword(Key, "DisableHandwritingButton", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-keyboard-telemetry",
                Label = "Disable Virtual Keyboard Telemetry",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableKeyboardTelemetry=1 in the VirtualKeyboard policy key. Stops "
                    + "the touch keyboard from reporting usage statistics including layout "
                    + "preference, session duration, and interaction rates to Microsoft's "
                    + "telemetry pipeline. Keyboard telemetry is collected continuously and "
                    + "contributes to the same diagnostic data pipeline as other Windows "
                    + "telemetry even when the user has opted out. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["keyboard", "telemetry", "privacy", "virtual", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableKeyboardTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableKeyboardTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableKeyboardTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-fullscreen-keyboard",
                Label = "Disable Full-Screen Keyboard in Desktop Apps",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableFullScreenKeyboard=1 in the VirtualKeyboard policy key. "
                    + "Prevents the touch keyboard from expanding to a full-screen mode when "
                    + "a text field gains focus in a desktop (Win32) application. Full-screen "
                    + "keyboard mode obscures the application window entirely and requires "
                    + "manual collapse, disrupting productivity on hybrid devices. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["keyboard", "fullscreen", "virtual", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFullScreenKeyboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFullScreenKeyboard")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFullScreenKeyboard", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-keyboard-animations",
                Label = "Disable Touch Keyboard Animations",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets DisableKeyboardAnimations=1 in the VirtualKeyboard policy key. "
                    + "Removes the slide and fade animations for touch keyboard show/hide "
                    + "transitions. On lower-end hardware or at high refresh rates the "
                    + "animation frame budget competes with foreground application rendering. "
                    + "Removing animations also improves perceived keyboard responsiveness. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["keyboard", "animation", "performance", "virtual", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableKeyboardAnimations", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableKeyboardAnimations")],
                DetectOps = [RegOp.CheckDword(Key, "DisableKeyboardAnimations", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-voice-dictation-key",
                Label = "Disable Voice Dictation Key on Touch Keyboard",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableVoiceDictationKey=1 in the VirtualKeyboard policy key. Removes "
                    + "the microphone button from the touch keyboard that activates the Windows "
                    + "voice dictation mode. Voice dictation streams audio to the Windows "
                    + "speech recognition service; disabling the toolbar button prevents "
                    + "unintentional activation in environments where microphone use is "
                    + "restricted. Default: 0. Recommended: 1.",
                Tags = ["keyboard", "voice", "dictation", "microphone", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableVoiceDictationKey", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableVoiceDictationKey")],
                DetectOps = [RegOp.CheckDword(Key, "DisableVoiceDictationKey", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-split-keyboard",
                Label = "Disable Split Touch Keyboard Mode",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets DisableSplitKeyboard=1 in the VirtualKeyboard policy key. Disables "
                    + "the split-keyboard layout that separates the keyboard into two thumb-"
                    + "typing halves at the screen edges. On non-tablet devices the split "
                    + "keyboard is an unneeded variant that users may accidentally activate via "
                    + "the keyboard settings menu, requiring manual restoration. "
                    + "Default: 0. Recommended: 1 on non-tablet form factors.",
                Tags = ["keyboard", "split", "tablet", "virtual", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSplitKeyboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSplitKeyboard")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSplitKeyboard", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-wide-keyboard",
                Label = "Disable Wide Touch Keyboard Layout",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets DisableWideKeyboard=1 in the VirtualKeyboard policy key. Removes "
                    + "the wide (full-width undocked) touch keyboard variant from the layout "
                    + "picker. The wide layout is designed for Surface-style devices lying flat; "
                    + "on conventional desktops it covers most of the screen without a "
                    + "productivity benefit. Removing the option simplifies the layout menu. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["keyboard", "wide", "layout", "virtual", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWideKeyboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWideKeyboard")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWideKeyboard", 1)],
            },
        ];
    }

    // ── WddmDriverPolicy ──
    private static class _WddmDriverPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers";
        private const string ScKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler";
        private const string PolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Display";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wddmpol-block-display-driver-fallback",
                    Label = "Block Fallback to Microsoft Basic Display Adapter",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Prevents Windows from falling back to the Microsoft Basic Display Adapter (2048×1152 VESA-only) when the GPU driver crashes, maintaining the last known working display driver and attempting recovery instead.",
                    Tags = ["wddm", "basic-display", "driver-fallback", "recovery", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "VGA-mode fallback blocked; driver crash triggers recovery, not basic display. May yield blank screen.",
                    ApplyOps = [RegOp.SetDword(PolKey, "DisableBasicDisplayDriverFallback", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "DisableBasicDisplayDriverFallback")],
                    DetectOps = [RegOp.CheckDword(PolKey, "DisableBasicDisplayDriverFallback", 1)],
                },
                new TweakDef
                {
                    Id = "wddmpol-disable-dxgi-flip-discard",
                    Label = "Disable Presentation Model Flip-Discard Optimisation",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Disables the DXGI flip-discard presentation model that reuses swap chain surfaces, falling back to flip-sequential for maximum frame ordering correctness in trading and video production environments where tearing prevention is critical.",
                    Tags = ["wddm", "dxgi", "flip-discard", "presentation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Flip-discard presentation disabled; flip-sequential used. Maximum frame ordering correctness at slight perf cost.",
                    ApplyOps = [RegOp.SetDword(PolKey, "DisableFlipDiscard", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "DisableFlipDiscard")],
                    DetectOps = [RegOp.CheckDword(PolKey, "DisableFlipDiscard", 1)],
                },
                new TweakDef
                {
                    Id = "wddmpol-log-tdr-events",
                    Label = "Log GPU TDR Recovery Events to System Event Log",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Enables System event log entries (EventID 4101, Display driver stopped responding and has recovered) for GPU TDR events, providing a history of GPU hangs and recovery cycles for diagnostics.",
                    Tags = ["wddm", "tdr", "event-log", "audit", "gpu-stability", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "GPU TDR events logged in System log; driver hang frequency visible for GPU stability diagnostics.",
                    ApplyOps = [RegOp.SetDword(Key, "TdrLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "TdrLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "TdrLogging", 1)],
                },
                new TweakDef
                {
                    Id = "wddmpol-disable-gpu-driver-telemetry",
                    Label = "Disable WDDM GPU Driver Telemetry to Microsoft",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Prevents the Windows Display Driver Model from sending GPU driver crash reports, TDR telemetry, and hardware capability telemetry to Microsoft, protecting GPU model/driver version information from cloud disclosure.",
                    Tags = ["wddm", "telemetry", "privacy", "gpu", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WDDM GPU telemetry to Microsoft disabled; GPU model, driver version, and TDR events not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(PolKey, "DisableGPUTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "DisableGPUTelemetry")],
                    DetectOps = [RegOp.CheckDword(PolKey, "DisableGPUTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "wddmpol-enable-virtual-display",
                    Label = "Enable Virtual Display Adapter for Headless Operation",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Enables the Windows virtual display adapter (IndirectDisplay) for headless server scenarios, providing a software display output that supports RDP and remote management tools without a physical GPU or monitor.",
                    Tags = ["wddm", "virtual-display", "headless", "rdp", "server", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Virtual display adapter enabled for headless RDP; servers without physical GPU get a software display.",
                    ApplyOps = [RegOp.SetDword(PolKey, "EnableVirtualDisplayAdapter", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "EnableVirtualDisplayAdapter")],
                    DetectOps = [RegOp.CheckDword(PolKey, "EnableVirtualDisplayAdapter", 1)],
                },
                new TweakDef
                {
                    Id = "wddmpol-set-gpu-priority-realtime",
                    Label = "Set GPU Work Item Priority to Normal for System Processes",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Configures the WDDM scheduler to run system and background GPU work items at Normal priority, preventing GPU starvation of foreground applications by long-running background ML or compute workloads.",
                    Tags = ["wddm", "gpu-priority", "scheduler", "background", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Background GPU work items set to Normal priority; foreground app rendering not starved by compute jobs.",
                    ApplyOps = [RegOp.SetDword(ScKey, "BackgroundGPUPriority", 1)],
                    RemoveOps = [RegOp.DeleteValue(ScKey, "BackgroundGPUPriority")],
                    DetectOps = [RegOp.CheckDword(ScKey, "BackgroundGPUPriority", 1)],
                },
            ];
    }

    // ── WiaImageAcquisitionPolicy ──
    private static class _WiaImageAcquisitionPolicy
    {
        private const string ScanKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Scanner";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "imgacquire-disable-wia-service",
                Label = "Image Acquisition: Disable WIA (Windows Image Acquisition) Service",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables the Windows Image Acquisition (WIA) service via Group Policy, preventing scanners, cameras, and other WIA-compatible imaging devices from automatically launching the scanning wizard when connected. WIA devices can auto-trigger Windows Explorer and photo import dialogs. On managed workstations where scanning occurs through dedicated document management software, disabling WIA eliminates uncontrolled ad-hoc scanning to unmanaged locations.",
                Tags = ["image acquisition", "wia", "scanner", "camera", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "STINoInteractiveMode", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "STINoInteractiveMode")],
                DetectOps = [RegOp.CheckDword(ScanKey, "STINoInteractiveMode", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables WIA interactive mode; connected scanners will not auto-launch import dialogs.",
            },
            new TweakDef
            {
                Id = "imgacquire-restrict-user-device-install",
                Label = "Image Acquisition: Restrict User-Initiated Device Installation for Cameras",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Prevents standard users from installing WIA-compatible cameras and imaging devices without administrator approval. Without this policy, plugging in a consumer camera can trigger a Plug-and-Play installation that adds imaging device drivers and WIA entries to the system. On managed environments, device drivers should only be installed through approved software deployment channels.",
                Tags = ["image acquisition", "wia", "camera", "device install", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "RestrictUserDeviceInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "RestrictUserDeviceInstall")],
                DetectOps = [RegOp.CheckDword(ScanKey, "RestrictUserDeviceInstall", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents user-triggered camera/scanner driver installation; admin elevation required for new imaging devices.",
            },
            new TweakDef
            {
                Id = "imgacquire-disable-transferring-without-policy",
                Label = "Image Acquisition: Disable Image Transfer Without Policy Approval",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Blocks WIA from transferring images from connected cameras, scanners, or memory cards to the local filesystem without an approved destination policy being applied. Without this control, users can freely dump images from connected devices to any local folder, bypassing document management systems and creating unmanaged data. Enabling this policy ensures all image transfer operations occur through sanctioned software.",
                Tags = ["image acquisition", "wia", "image transfer", "data control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "DisableTransferWithoutPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "DisableTransferWithoutPolicy")],
                DetectOps = [RegOp.CheckDword(ScanKey, "DisableTransferWithoutPolicy", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote =
                    "Blocks ad-hoc image transfers from cameras/scanners; images must be transferred through approved document management software.",
            },
            new TweakDef
            {
                Id = "imgacquire-disable-scan-to-fax",
                Label = "Image Acquisition: Disable Scan-to-Fax WIA Feature",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables the WIA Scan-to-Fax destination that allows users to scan a document directly to a fax number through a Windows Fax and Scan workflow. Scan-to-fax functionality can be exploited to exfiltrate documents outside the organisation's content inspection boundary — fax transmissions often bypass DLP controls that monitor email and file-share uploads. Disabling this destination ensures all document workflows go through monitored channels.",
                Tags = ["image acquisition", "scan to fax", "wia", "data loss prevention", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "DisableScanToFax", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "DisableScanToFax")],
                DetectOps = [RegOp.CheckDword(ScanKey, "DisableScanToFax", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes Scan-to-Fax from WIA destinations; Windows Fax and Scan direct-to-scanner functionality is unaffected.",
            },
            new TweakDef
            {
                Id = "imgacquire-disable-autoplay-camera",
                Label = "Image Acquisition: Disable AutoPlay for Camera Devices",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Prevents Windows AutoPlay from automatically launching when a camera or memory card is inserted, suppressing the dialog that asks what action to take (view photos, import as a folder, etc.). AutoPlay-triggered actions can automatically copy images from connected devices to default Photos or OneDrive locations without user awareness. Disabling AutoPlay ensures device connections require deliberate user action.",
                Tags = ["image acquisition", "autoplay", "camera", "memory card", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "DisableAutoPlayCamera", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "DisableAutoPlayCamera")],
                DetectOps = [RegOp.CheckDword(ScanKey, "DisableAutoPlayCamera", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses AutoPlay dialog for cameras and memory cards; no automatic photo import.",
            },
            new TweakDef
            {
                Id = "imgacquire-require-driver-signing",
                Label = "Image Acquisition: Require Signed Drivers for WIA Devices",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Enforces that only digitally-signed drivers can be loaded for WIA imaging devices. Unsigned WIA device drivers are a known malware vector — adversaries have used crafted WIA drivers to establish persistent kernel-mode access. Requiring driver signing ensures that all imaging device drivers are verifiable against Microsoft's WHQL certificate chain or a trusted enterprise root CA.",
                Tags = ["image acquisition", "wia", "driver signing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "RequireSignedDrivers", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "RequireSignedDrivers")],
                DetectOps = [RegOp.CheckDword(ScanKey, "RequireSignedDrivers", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Requires WHQL or enterprise-signed WIA drivers; unsigned legacy scanner drivers will fail to load.",
            },
            new TweakDef
            {
                Id = "imgacquire-disable-scan-to-sharepoint",
                Label = "Image Acquisition: Disable Scan-to-SharePoint WIA Feature",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables the WIA Scan-to-SharePoint destination that allows users to scan documents and automatically upload them to a SharePoint document library via the Windows Scan app. Scan-to-SharePoint can bypass normal document governance workflows by depositing files directly into collaboration sites without metadata tagging, classification, or legal-hold review. Disabling this destination ensures all scanned documents go through the organisation's records management system.",
                Tags = ["image acquisition", "scan to sharepoint", "wia", "document governance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "DisableScanToSharePoint", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "DisableScanToSharePoint")],
                DetectOps = [RegOp.CheckDword(ScanKey, "DisableScanToSharePoint", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes Scan-to-SharePoint from WIA destination list; manual upload to SharePoint via browser is unaffected.",
            },
            new TweakDef
            {
                Id = "imgacquire-disable-scanner-to-network",
                Label = "Image Acquisition: Disable Scan-to-Network Share Feature",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables the built-in scan-to-network-share facility in the WIA STI (Still Image) architecture that allows scanners with FTP/SMB push capability to send files directly to a Windows shared folder. Scan-to-network-share bypasses normal document management channels and can be used to exfiltrate documents to unauthorized UNC paths. Managed scanning environments should use dedicated secure document capture software instead.",
                Tags = ["image acquisition", "scanner", "network share", "data transfer", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "DisableScanToNetworkShare", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "DisableScanToNetworkShare")],
                DetectOps = [RegOp.CheckDword(ScanKey, "DisableScanToNetworkShare", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables scan-to-SMB/FTP from the WIA stack; dedicated scanning software configured by IT continues to work.",
            },
            new TweakDef
            {
                Id = "imgacquire-block-scan-to-email",
                Label = "Image Acquisition: Block Scan-to-Email Functionality",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Prevents the Windows Fax and Scan application and WIA-connected scanners from using the scan-to-email feature, which attaches scanned documents directly to email drafts. Scan-to-email can bypass DLP (Data Loss Prevention) policies by sending scanned documents through the default email client without content inspection. In regulated environments, document distribution must be controlled through DLP-aware channels.",
                Tags = ["image acquisition", "scanner", "email", "dlp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "DisableScanToEmail", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "DisableScanToEmail")],
                DetectOps = [RegOp.CheckDword(ScanKey, "DisableScanToEmail", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks scan-to-email; users must save scanned documents to approved locations and attach them manually.",
            },
            new TweakDef
            {
                Id = "imgacquire-restrict-scan-destination",
                Label = "Image Acquisition: Restrict Scan Destination to Approved Paths Only",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Enforces that WIA scan operations can only save documents to pre-approved local paths or managed network shares defined in Group Policy. Without this restriction, users can direct scanned content to removable drives, personal cloud sync folders (OneDrive, Dropbox), or mapped drives outside the corporate network perimeter. Restricting destinations ensures all scanned documents are stored in auditable, managed locations.",
                Tags = ["image acquisition", "scanner", "destination", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "RestrictScanDestinations", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "RestrictScanDestinations")],
                DetectOps = [RegOp.CheckDword(ScanKey, "RestrictScanDestinations", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Limits scan destinations to IT-approved paths; scans to arbitrary local or cloud paths are blocked.",
            },
        ];
    }

    // ── WindowsAccessibilityPolicy ──
    private static class _WindowsAccessibilityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Accessibility";
        private const string MagnKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Accessibility\Magnifier";
        private const string NarratorKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Accessibility\Narrator";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "a11ypol-disable-serial-keys",
                Label = "Accessibility Policy: Disable Serial Keys Support",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables Serial Keys accessibility support, which allows alternative input devices (joysticks, switches) connected to the serial port. Disabling reduces the attack surface on managed endpoints without physical accessibility hardware.",
                Tags = ["accessibility", "serial-keys", "input", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Reduces attack surface on managed endpoints without accessibility hardware.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSerialKeysSupport", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSerialKeysSupport")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSerialKeysSupport", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-sound-sentry",
                Label = "Accessibility Policy: Disable SoundSentry Visual Flash",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables SoundSentry, which flashes the screen or a window when a critical sound plays. On enterprise environments with active CAD/3D rendering, unexpected screen flashes can interfere with rendering workflows.",
                Tags = ["accessibility", "soundsentry", "flash", "audio", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Prevents unexpected screen flashes interfering with CAD/3D rendering workflows.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSoundSentryFunctionality", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSoundSentryFunctionality")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSoundSentryFunctionality", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-high-contrast-hotkey",
                Label = "Accessibility Policy: Disable High Contrast Mode Hotkey",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Prevents users from accidentally enabling High Contrast mode via the Left Alt+Left Shift+Print Screen keyboard shortcut. Avoids unexpected UI colour inversions that can disrupt productivity applications.",
                Tags = ["accessibility", "high-contrast", "hotkey", "keyboard", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents accidental Alt+Shift+PrtScr activation of high contrast mode.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableHighContrastHotKey", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableHighContrastHotKey")],
                DetectOps = [RegOp.CheckDword(Key, "DisableHighContrastHotKey", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-toggle-keys",
                Label = "Accessibility Policy: Disable Toggle Keys Hotkey",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables activation of Toggle Keys (a beep when pressing Caps Lock, Num Lock, or Scroll Lock) via the Num Lock hotkey shortcut. Prevents unexpected beeping on endpoints with shared keyboards.",
                Tags = ["accessibility", "toggle-keys", "keyboard", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents accidental beeping when Num Lock key is pressed on shared keyboards.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableToggleKeysHotKey", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableToggleKeysHotKey")],
                DetectOps = [RegOp.CheckDword(Key, "DisableToggleKeysHotKey", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-sticky-keys-hotkey",
                Label = "Accessibility Policy: Disable Sticky Keys Hotkey",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables the Sticky Keys prompt when Shift is pressed 5 times. Sticky Keys can interrupt gaming and productivity workflows when activated accidentally, and is better enabled via Settings if needed.",
                Tags = ["accessibility", "sticky-keys", "keyboard", "hotkey", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents 5\u00d7Shift shortcut from interrupting gaming and productivity workflows.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableStickyKeysHotKey", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableStickyKeysHotKey")],
                DetectOps = [RegOp.CheckDword(Key, "DisableStickyKeysHotKey", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-filter-keys-hotkey",
                Label = "Accessibility Policy: Disable Filter Keys Hotkey",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables the Filter Keys shortcut activated by holding the Right Shift key for 8 seconds. Filter Keys can cause significant input delay if triggered accidentally.",
                Tags = ["accessibility", "filter-keys", "keyboard", "hotkey", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents 8-second Shift hold from triggering keyboard input delay mode.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFilterKeysHotKey", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFilterKeysHotKey")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFilterKeysHotKey", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-bounce-keys",
                Label = "Accessibility Policy: Disable Bounce Keys for Keyboard Repeat",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables Bounce Keys (Filter Keys variant) that ignores brief multiple key presses. While useful for accessibility, this setting can reduce keyboard responsiveness when not needed.",
                Tags = ["accessibility", "bounce-keys", "filter-keys", "keyboard", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Restores normal keyboard repeat; disables brief multiple-keypress filtering.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBounceKeyboardSettings", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBounceKeyboardSettings")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBounceKeyboardSettings", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-mouse-keys-hotkey",
                Label = "Accessibility Policy: Disable Mouse Keys Hotkey",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables activation of Mouse Keys via the Left Alt+Left Shift+Num Lock shortcut. Mouse Keys redirects numpad input to pointer movement, which is a common source of unexpected mouse behaviour on laptops.",
                Tags = ["accessibility", "mouse-keys", "hotkey", "numpad", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents numpad-to-mouse redirect being accidentally activated on laptops.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMouseKeysHotKey", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMouseKeysHotKey")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMouseKeysHotKey", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-magnifier-startup",
                Label = "Accessibility Policy: Disable Magnifier Auto-Start",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Prevents Windows Magnifier from starting automatically when a user signs in. Magnifier auto-start is sometimes triggered by a registry artefact on downgraded or re-imaged systems.",
                Tags = ["accessibility", "magnifier", "startup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents Magnifier from auto-starting on re-imaged systems.",
                RegistryKeys = [MagnKey],
                ApplyOps = [RegOp.SetDword(MagnKey, "StartMinimized", 1)],
                RemoveOps = [RegOp.DeleteValue(MagnKey, "StartMinimized")],
                DetectOps = [RegOp.CheckDword(MagnKey, "StartMinimized", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-narrator-startup",
                Label = "Accessibility Policy: Disable Narrator Auto-Start on Sign-In",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Prevents Windows Narrator (screen reader) from starting automatically at sign-in. Narrator auto-activation can be triggered by accessibility registry artefacts on shared or re-used endpoints.",
                Tags = ["accessibility", "narrator", "screen-reader", "startup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents Narrator screen reader from auto-activating on shared or re-used endpoints.",
                RegistryKeys = [NarratorKey],
                ApplyOps = [RegOp.SetDword(NarratorKey, "DisableNarratorAutoStart", 1)],
                RemoveOps = [RegOp.DeleteValue(NarratorKey, "DisableNarratorAutoStart")],
                DetectOps = [RegOp.CheckDword(NarratorKey, "DisableNarratorAutoStart", 1)],
            },
        ];
    }

    // ── WindowsInkWorkspaceAdvPolicy ──
    private static class _WindowsInkWorkspaceAdvPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "inkwsadv-restrict-ink-workspace-on-lockscreen",
                Label = "Restrict Windows Ink Workspace Access from the Lock Screen",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Restricting Windows Ink Workspace access from the lock screen prevents users who are not authenticated to the device from launching ink workspace features that may expose note content or access note-taking functionality with user data. Lock screen accessible features bypass authentication requirements and any data that can be accessed from the lock screen represents a risk of unauthorized access to user information. The default Windows configuration allows Ink Workspace notes to be accessed from the lock screen through the taskbar which could expose previously created notes to physical access attackers. Restricting lock screen access to Ink Workspace is particularly important for shared devices and devices in physical locations accessible to people outside the organization. Users who frequently take notes using the stylus workflow should be informed that lock screen notes will not be accessible and alternative note-taking approaches should be available. Lock screen feature restrictions should be tested for consistency on all device types including Surface devices that have stylus-focused features.",
                Tags = ["ink-workspace", "lock-screen", "access-restriction", "authentication", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowWindowsInkWorkspaceOnLockScreen", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowWindowsInkWorkspaceOnLockScreen")],
                DetectOps = [RegOp.CheckDword(Key, "AllowWindowsInkWorkspaceOnLockScreen", 0)],
            },
            new TweakDef
            {
                Id = "inkwsadv-restrict-ink-workspace-to-approved-users",
                Label = "Restrict Windows Ink Workspace Feature Access to Authorized User Groups",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Restricting Windows Ink Workspace to authorized user groups ensures that the feature is available only to employees who have a legitimate business need for pen input capabilities aligned with their role such as design healthcare or field operations functions. Unrestricted access to Ink Workspace on all endpoints makes the feature available to users who do not use pen input and creates an unnecessary attack surface for potential vulnerabilities in the ink processing stack. Role-based feature availability reduces the endpoint attack surface by disabling features that are not required for specific job functions while maintaining availability for users with legitimate use cases. Ink Workspace capability restrictions align with application allowlisting principles extending feature allowlisting beyond application execution to complex OS features. User group membership for ink workspace access authorization should be maintained as part of the identity management and access governance process. Devices that are not configured with digitizer hardware should have ink workspace disabled by default to avoid providing no-op features that consume resources.",
                Tags = ["ink-workspace", "user-restriction", "role-based-access", "attack-surface", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictInkWorkspaceToApprovedUsers", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictInkWorkspaceToApprovedUsers")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictInkWorkspaceToApprovedUsers", 1)],
            },
            new TweakDef
            {
                Id = "inkwsadv-disable-ink-personalization-data-collection",
                Label = "Disable Ink and Typing Personalization Data Collection for Windows Ink",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Disabling ink and typing personalization data collection prevents Windows from sending handwriting samples stylus gesture patterns and ink input data to Microsoft services for handwriting recognition improvement and personalization. Handwriting recognition data transmitted to cloud services for personalization may include sensitive information written with a stylus such as signatures handwritten notes and sketches containing confidential content. Ink personalization data represents a category of behavioral biometric data that is sensitive from both privacy and security perspectives as handwriting patterns can uniquely identify individuals. Organizations with strict data minimization requirements should disable ink personalization collection to prevent unnecessary transmission of user behavioral data to cloud services. Users who rely on accurate handwriting recognition for productivity should be informed that disabling personalization may reduce recognition accuracy over time as the local model will not improve through cloud training. Disabling ink personalization collection is consistent with broader Windows telemetry and data collection minimization policies in enterprise environments.",
                Tags = ["ink-personalization", "data-collection", "privacy", "handwriting", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableInkPersonalizationData", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInkPersonalizationData")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInkPersonalizationData", 1)],
            },
            new TweakDef
            {
                Id = "inkwsadv-enforce-ink-clipboard-restrictions",
                Label = "Enforce Clipboard Restrictions for Ink Content Sharing Between Applications",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Enforcing clipboard restrictions for ink content prevents handwritten ink from being copied and pasted between applications in ways that could bypass data loss prevention controls configured to monitor or restrict text data transfer. Ink content transferred through the clipboard may bypass DLP policies that inspect text-format clipboard data because ink is represented as a different data format that may not be inspected by the same DLP mechanisms. Organizations with strict DLP requirements should evaluate whether ink clipboard sharing creates a bypass channel for sensitive data transfer that circumvents text-based DLP controls. Clipboard restrictions for ink content should be evaluated in the context of the organization's broader clipboard management policy including cloud clipboard synchronization controls. Users who use ink input for legitimate workflow reasons including annotations and sketching should be made aware of any restrictions on ink clipboard sharing that affect their productivity. Ink data format controls should be reviewed periodically as Windows updates may introduce new ink data formats that affect the applicability of existing clipboard restriction policies.",
                Tags = ["ink-clipboard", "dlp", "data-transfer", "clipboard-restriction", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceInkClipboardRestrictions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceInkClipboardRestrictions")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceInkClipboardRestrictions", 1)],
            },
            new TweakDef
            {
                Id = "inkwsadv-block-ink-workspace-third-party-apps",
                Label = "Block Third-Party Applications from Integrating with Windows Ink Workspace",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Blocking third-party application integration with Windows Ink Workspace prevents unapproved applications from registering themselves as ink workspace providers or intercepting ink input streams through the workspace integration API. Third-party ink workspace integrations that have not been vetted by organizational security review may collect ink input data including handwritten sensitive content through their integration with the ink pipeline. Malicious applications posing as legitimate ink workspace tools can use the integration API to capture all ink input data as a form of keylogging for stylus-entered text. Organizations should evaluate legitimate third-party ink application requirements through the normal software approval process rather than allowing unrestricted third-party ink workspace integration. Approved pen-input applications should be deployed through device management platforms with appropriate application version controls to ensure only vetted application versions can access ink workspace integration points. Security testing of the ink workspace third-party integration API should be included in application security assessments for applications that request ink workspace integration capabilities.",
                Tags = ["ink-workspace", "third-party-apps", "integration-security", "input-protection", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockThirdPartyInkWorkspaceApps", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockThirdPartyInkWorkspaceApps")],
                DetectOps = [RegOp.CheckDword(Key, "BlockThirdPartyInkWorkspaceApps", 1)],
            },
            new TweakDef
            {
                Id = "inkwsadv-disable-cloud-ink-sync",
                Label = "Disable Synchronization of Windows Ink Notes to Cloud Storage Services",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Disabling cloud synchronization of Windows Ink notes prevents handwritten digital ink sketches and annotations from being automatically uploaded to cloud storage services where they are subject to third-party data handling and may be accessible from devices not managed by the organization. Cloud sync of ink notes can transmit sensitive handwritten content outside the organizational security boundary including annotated documents contract documents and handwritten notes taken during confidential meetings. Ink note synchronization disabled through policy ensures that notes remain on the device until the user explicitly chooses to share them through approved channels. Organizations that have approved OneDrive or other enterprise cloud storage for document synchronization should configure ink sync to use only approved enterprise storage rather than disabling all sync. Data sovereignty requirements may prohibit automatic cloud sync of data including handwritten notes to cloud regions or providers that do not meet organizational compliance requirements. Users should be informed about the cloud sync policy for ink notes and provided with guidance on how to share ink content through approved methods when collaboration requires information sharing.",
                Tags = ["ink-sync", "cloud-storage", "data-protection", "note-taking", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCloudInkSync", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudInkSync")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCloudInkSync", 1)],
            },
            new TweakDef
            {
                Id = "inkwsadv-enforce-stylus-firmware-attestation",
                Label = "Enforce Firmware Attestation Checks for Connected Stylus and Pen Devices",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Stylus and pen device firmware attestation ensures that pen devices connected to enterprise systems have firmware that has been validated as authentic and unmodified preventing compromised pen firmware from injecting false input events or exploiting pen driver vulnerabilities. Pen and stylus devices connect through USB or Bluetooth interfaces and the driver surface that processes pen input represents an attack vector for specially crafted devices with malicious firmware. Supply chain attacks targeting peripheral firmware have demonstrated that hardware devices can be compromised to execute code in the context of the operating system's hardware input processing. Firmware attestation requirements for pen devices should be part of the organization's broader peripheral device security policy that includes hardware device approval and firmware management. Organizations should define a list of approved pen device models that have been security-evaluated and use device management controls to restrict use of pen devices to approved models. Security monitoring for pen device connection events should be implemented to detect use of unauthorized or unusual pen devices.",
                Tags = ["stylus-firmware", "attestation", "peripheral-security", "supply-chain", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceStylusFirmwareAttestation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceStylusFirmwareAttestation")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceStylusFirmwareAttestation", 1)],
            },
            new TweakDef
            {
                Id = "inkwsadv-audit-ink-workspace-activity",
                Label = "Enable Audit Logging for Windows Ink Workspace Activation and Feature Use",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Enabling audit logging for Windows Ink Workspace activity records when the Ink Workspace is activated which features are used and what applications receive ink input providing a behavioral baseline for ink workspace usage patterns. Anomalous ink workspace activity including activation at unusual times screen-capturing ink operations or large volumes of ink note creation may indicate data harvesting or unauthorized use of pen input features. Audit logs for Ink Workspace should include user identity device identifier timestamp and feature used to support investigation of security incidents involving ink input workflows. Organizations can use ink workspace audit data to identify users who would benefit from additional training on ink security policies or who may be using ink features in ways that create security risks. Ink workspace activity monitoring should be proportionate to the sensitivity of data that users with ink access handle with more intensive monitoring for users with access to highly sensitive information. Audit events from ink workspace usage should be integrated with user and entity behavior analytics platforms to contextualize ink activity within the broader user behavioral profile.",
                Tags = ["ink-workspace", "audit-logging", "behavioral-analytics", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditInkWorkspaceActivity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditInkWorkspaceActivity")],
                DetectOps = [RegOp.CheckDword(Key, "AuditInkWorkspaceActivity", 1)],
            },
            new TweakDef
            {
                Id = "inkwsadv-restrict-ink-workspace-on-shared-devices",
                Label = "Apply Strict Ink Workspace Restrictions on Shared and Kiosk Devices",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Applying strict Ink Workspace restrictions on shared devices and kiosk configurations prevents previous users' ink notes and sketches from being accessible to subsequent users of the same device minimizing data leakage between user sessions on shared infrastructure. Shared devices that do not clear Ink Workspace content between user sessions can expose handwritten notes taken by previous authenticated users to users who authenticate later. Kiosk configurations should have Ink Workspace disabled entirely unless pen input is an integral part of the kiosk application's intended function. Session isolation for ink workspace data on shared devices should ensure that ink content from one user session is not accessible in another user's session context. Organizations deploying hot-desking or shared workstation environments should configure ink workspace policies appropriate for the shared use context including automatic note clearing on session end. Ink workspace configuration for shared devices should be validated as part of the device provisioning process to ensure that shared use policies are applied correctly.",
                Tags = ["ink-workspace", "shared-devices", "kiosk", "session-isolation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictInkWorkspaceOnSharedDevices", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictInkWorkspaceOnSharedDevices")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictInkWorkspaceOnSharedDevices", 1)],
            },
        ];
    }

    // ── WindowsSearchAdv ──
    private static class _WindowsSearchAdv
    {
        private const string SearchPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search";

        private const string SearchUser = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search";

        private const string SearchInternal = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search";

        private const string SearchResults = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "search-disable-web-results-policy",
                Label = "Disable Web Results in Search via Policy",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["search", "web", "policy", "bing", "privacy"],
                Description =
                    "Enforces disabling of internet search results in Windows Search via "
                    + "Group Policy. Persists across user profile changes and applies to all "
                    + "users on the system.",
                ApplyOps = [RegOp.SetDword(SearchPolicy, "DisableWebSearch", 1)],
                RemoveOps = [RegOp.DeleteValue(SearchPolicy, "DisableWebSearch")],
                DetectOps = [RegOp.CheckDword(SearchPolicy, "DisableWebSearch", 1)],
            },
            new TweakDef
            {
                Id = "search-disable-safe-search",
                Label = "Disable SafeSearch Filter in Windows Search",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["search", "safesearch", "web", "bing"],
                Description =
                    "Disables the SafeSearch filter (adult content filtering) from Windows "
                    + "Search web results. Value 0 = Off. Requires web search to be enabled. "
                    + "Only affects Windows Search Bing integration.",
                ApplyOps = [RegOp.SetDword(SearchUser, "SafeSearchMode", 0)],
                RemoveOps = [RegOp.SetDword(SearchUser, "SafeSearchMode", 1)],
                DetectOps = [RegOp.CheckDword(SearchUser, "SafeSearchMode", 0)],
            },
            new TweakDef
            {
                Id = "search-disable-find-my-files",
                Label = "Disable Enhanced 'Find My Files' Deep Search",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["search", "find my files", "enhanced", "indexing"],
                Description =
                    "Disables the 'Find My Files' enhanced indexing mode that deeply indexes "
                    + "all files including non-indexed locations. Reduces background disk I/O "
                    + "from extensive indexing sweeps.",
                ApplyOps = [RegOp.SetDword(SearchUser, "DeviceHistoryEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(SearchUser, "DeviceHistoryEnabled")],
                DetectOps = [RegOp.CheckDword(SearchUser, "DeviceHistoryEnabled", 0)],
            },
            new TweakDef
            {
                Id = "search-disable-recent-activities-search",
                Label = "Disable Recent Activities in Search Results",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["search", "recent", "activity", "privacy"],
                Description =
                    "Disables recently opened files and apps from appearing in Windows Search "
                    + "results. Prevents search from surfacing your recent activity to other "
                    + "users on shared machines.",
                ApplyOps = [RegOp.SetDword(SearchUser, "History", 0)],
                RemoveOps = [RegOp.SetDword(SearchUser, "History", 1)],
                DetectOps = [RegOp.CheckDword(SearchUser, "History", 0)],
            },
            new TweakDef
            {
                Id = "search-disable-device-sync-search",
                Label = "Disable Cross-Device Search Sync",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["search", "sync", "device", "privacy", "cloud"],
                Description =
                    "Disables Windows Search syncing query history and results across "
                    + "devices connected to the same Microsoft account. Keeps search history "
                    + "local to this machine only.",
                ApplyOps = [RegOp.SetDword(SearchPolicy, "AllowCortana", 0)],
                RemoveOps = [RegOp.DeleteValue(SearchPolicy, "AllowCortana")],
                DetectOps = [RegOp.CheckDword(SearchPolicy, "AllowCortana", 0)],
            },
        ];
    }

    // ── WindowsSearchIndexingAdvancedPolicy ──
    private static class _WindowsSearchIndexingAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wsidx-prevent-remote-queries",
                    Label = "Prevent Remote Search Queries via Windows Search",
                    Category = "Display — Wia Image Acquisition",
                    Description =
                        "Blocks remote clients from querying the local Windows Search index over the network. Default: allowed. Recommended: disabled for workstations.",
                    Tags = ["search", "indexing", "remote", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Prevents network-based search queries against local index; local search unaffected.",
                    ApplyOps = [RegOp.SetDword(Key, "PreventRemoteQueries", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreventRemoteQueries")],
                    DetectOps = [RegOp.CheckDword(Key, "PreventRemoteQueries", 1)],
                },
                new TweakDef
                {
                    Id = "wsidx-disable-safe-search",
                    Label = "Set Search SafeSearch to Strict via Policy",
                    Category = "Display — Wia Image Acquisition",
                    Description = "Enforces SafeSearch strict mode for web results in Windows Search. Applies via Group Policy. Default: moderate.",
                    Tags = ["search", "safe-search", "content-filter", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "SafeSearch forced to strict; only affects web result filtering.",
                    ApplyOps = [RegOp.SetDword(Key, "ConnectedSearchSafeSearch", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConnectedSearchSafeSearch")],
                    DetectOps = [RegOp.CheckDword(Key, "ConnectedSearchSafeSearch", 3)],
                },
            ];
    }

    // ── VirtualizationPolicy ──
    private static class _VirtualizationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "virtz-restrict-hyper-v-management-to-admins",
                Label = "Restrict Hyper-V Management Operations to Administrator Accounts",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Restricting Hyper-V management to administrator accounts prevents standard users from creating modifying or deleting virtual machines that could be used to run unauthorized software within a virtualized environment. Standard users with Hyper-V management access could create virtual machines that bypass organizational security controls applied to host systems. Unauthorized virtual machines are difficult to monitor and may not have security software installed creating blind spots in endpoint protection coverage. Hyper-V management access should be limited to IT administrators who have a documented business need to create and manage virtual machines. Organizations should implement least-privilege principles for Hyper-V management using delegated administration where possible to grant only the specific capabilities required for each administrative role. Audit logging for Hyper-V management operations should track all VM creation deletion and configuration changes by administrator.",
                Tags = ["hyper-v", "virtualization", "admin-restriction", "least-privilege", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictManagementToAdmins", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictManagementToAdmins")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictManagementToAdmins", 1)],
            },
            new TweakDef
            {
                Id = "virtz-disable-hyper-v-on-workstations",
                Label = "Disable Hyper-V Virtualization Platform on Standard Enterprise Workstations",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Disabling Hyper-V on standard workstations that do not have a documented business requirement for local virtualization reduces attack surface by removing the virtualization infrastructure that could be used for malicious purposes. Hyper-V enabled workstations can be used by attackers to run virtual machines that bypass host-based security controls and operate as isolated systems on the corporate network. Virtualization-based security features like Credential Guard and Device Guard require Hyper-V to be present so disabling Hyper-V must be weighed against the security benefits those features provide. Organizations should evaluate whether disabling Hyper-V is appropriate for their security model or whether keeping it enabled primarily for VBS security features is the better configuration. Developer workstations and IT administration systems that have legitimate virtualization requirements should be exempted from the general workstation policy. Disabling Hyper-V may affect WSL 2 which uses Hyper-V technology; alternatives using WSLg or WSL 1 should be evaluated if WSL is required.",
                Tags = ["hyper-v", "workstation", "virtualization", "attack-surface", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableHyperVOnWorkstations", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableHyperVOnWorkstations")],
                DetectOps = [RegOp.CheckDword(Key, "DisableHyperVOnWorkstations", 1)],
            },
            new TweakDef
            {
                Id = "virtz-enforce-synthetic-device-security",
                Label = "Enforce Security Configuration for Hyper-V Synthetic Devices",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Hyper-V synthetic devices provide paravirtualized device interfaces between guest virtual machines and the hypervisor that can be exploited to break guest isolation if not configured securely. Enforcing security configuration for synthetic devices ensures that guest VMs cannot exploit vulnerabilities in device emulation to gain access to host resources or hypervisor memory. Synthetic network adapters storage controllers and video adapters each have configurable security parameters that should be set to the most restrictive values appropriate for the VM workload. Organizations should apply the principle of least capability to Hyper-V VMs granting only the synthetic devices needed for the VM's function. Security configuration for synthetic devices should be audited as part of the VM provisioning process to ensure that all new VMs are configured with appropriate device security settings. Guest VM security configurations should be periodically reviewed to identify VMs that have accumulated unnecessary synthetic device capabilities.",
                Tags = ["hyper-v", "synthetic-devices", "vm-security", "isolation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceSyntheticDeviceSecurity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceSyntheticDeviceSecurity")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceSyntheticDeviceSecurity", 1)],
            },
            new TweakDef
            {
                Id = "virtz-enable-vm-snapshot-encryption",
                Label = "Enable Encryption of Virtual Machine Snapshots and Saved States",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Virtual machine snapshots and saved states contain complete memory images of running VMs that may include encryption keys credentials and sensitive application data. Encrypting VM snapshots and saved states ensures that this sensitive data is protected if snapshot files are accessed outside the Hyper-V management context. VM snapshot files stored on shared storage or backup media are particularly vulnerable to unauthorized access if they are not encrypted at the VM level. Shielded VMs in Hyper-V provide the highest level of protection including encryption of VM configuration snapshots and saved states using Host Guardian Service key management. Organizations should implement VM encryption for any virtual machine that processes sensitive regulated data including financial HR and healthcare applications. Key management for VM encryption should be integrated with the organizational key management infrastructure to ensure proper key lifecycle management.",
                Tags = ["hyper-v", "vm-encryption", "snapshots", "saved-states", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSnapshotEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSnapshotEncryption")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSnapshotEncryption", 1)],
            },
            new TweakDef
            {
                Id = "virtz-restrict-vm-clipboard-sharing",
                Label = "Restrict Clipboard Sharing Between Hyper-V Guest and Host",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Clipboard sharing between Hyper-V guests and the host system creates a data transfer channel that can leak sensitive data between VMs and the host or allow malicious code injection through the clipboard. Restricting clipboard sharing prevents accidental data leakage between isolated VMs running different workloads or between VM environments and the host system. VMs running untrusted content or isolated high-security workloads should have clipboard sharing disabled to prevent data from crossing VM isolation boundaries. Malicious applications running in a VM can use clipboard injection to execute code on the host if the user pastes clipboard content from the VM into host applications. Organizations should evaluate clipboard sharing requirements for each VM type and allow it only where legitimate workflow requirements justify the associated risk. Monitoring clipboard sharing events can help detect attempts to use clipboard as a data exfiltration channel between isolated environments.",
                Tags = ["hyper-v", "clipboard", "data-isolation", "vm-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictClipboardSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictClipboardSharing")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictClipboardSharing", 1)],
            },
            new TweakDef
            {
                Id = "virtz-audit-vm-management-operations",
                Label = "Enable Audit Logging for All Hyper-V Virtual Machine Management Operations",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Hyper-V management audit logging records all VM lifecycle events including VM creation startup shutdown deletion and configuration changes providing accountability for VM management operations. Audit trails for VM management are important for detecting unauthorized VM creation that could be used for shadow IT or malicious purposes. VM deletion events should be monitored closely as deletion of VMs may indicate evidence destruction in the context of security incidents. Audit events for Hyper-V management should include the identity of the administrator performing the operation the timestamp and the details of the changed configuration. Organizations should forward Hyper-V audit events to centralized SIEM for correlation with other administrative activity and identity events. VM management audit logs should be retained for a period appropriate to the organization's compliance requirements.",
                Tags = ["hyper-v", "audit", "vm-management", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditVMManagementOperations", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditVMManagementOperations")],
                DetectOps = [RegOp.CheckDword(Key, "AuditVMManagementOperations", 1)],
            },
            new TweakDef
            {
                Id = "virtz-enforce-secure-boot-for-vms",
                Label = "Enforce Secure Boot Configuration for Hyper-V Generation 2 VMs",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Enforcing Secure Boot for Hyper-V Generation 2 virtual machines ensures that VMs only boot operating system images that are signed with trusted certificates preventing rootkit-level malware from persisting in VM boot configurations. Guest VM Secure Boot uses Hyper-V virtual firmware to validate the boot chain signature exactly as physical Secure Boot does on bare metal systems. Malicious modifications to VM boot sectors or boot loaders are prevented by Secure Boot enforcement which is particularly important for VMs that are created from templates or imported from external sources. Guest Secure Boot should be configured with appropriate templates for the VM's operating system type. Organizations should define and enforce VM templates that include Secure Boot configuration to ensure all provisioned VMs have this protection enabled from creation. VMs that fail Secure Boot validation should not be allowed to start and alerts should be generated for investigation.",
                Tags = ["hyper-v", "secure-boot", "vm-security", "boot-integrity", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceSecureBootForVMs", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceSecureBootForVMs")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceSecureBootForVMs", 1)],
            },
            new TweakDef
            {
                Id = "virtz-restrict-vm-network-access",
                Label = "Restrict Hyper-V Virtual Machine Network Access Configuration",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Restricting VM network access through Hyper-V policy limits which network segments virtual machines can be connected to preventing VMs from accessing sensitive network segments that are not appropriate for their function. Virtual machine network placement should be deliberately configured to provide access only to the network segments required for the VM's workload. Production VMs should be isolated from development and test VMs at the network layer to prevent cross-contamination between environments. VMs that process sensitive data should be on network segments with monitoring and DLP capabilities to detect unauthorized data access. Unrestricted VM network configuration allows administrators to connect VMs to any available virtual switch without network policy review which can lead to inadvertent bypassing of network segmentation controls. Network access configuration for Hyper-V VMs should be part of the VM provisioning review process including approval of new network connections.",
                Tags = ["hyper-v", "vm-network", "segmentation", "isolation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictVMNetworkAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictVMNetworkAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictVMNetworkAccess", 1)],
            },
            new TweakDef
            {
                Id = "virtz-block-nested-virtualization",
                Label = "Block Nested Virtualization in Hyper-V Guest Virtual Machines",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Nested virtualization allows virtual machines to run their own hypervisor and create nested VMs which can be used to create isolated execution environments that bypass host security monitoring. Blocking nested virtualization prevents the use of VM-within-VM configurations that increase complexity and make security monitoring and policy enforcement more difficult. Nested virtualization is exploited in some containerization attacks where attackers use nested Hyper-V to create isolated containers that bypass host security controls. The reduced visibility into nested VM operations makes incident investigation significantly more difficult when security events originate from within nested environments. Organizations that have specific legitimate requirements for nested virtualization should isolate those systems and apply additional monitoring rather than broadly enabling nested virtualization. Testing and development environments that require nested virtualization for specific use cases should be treated as high-risk systems with compensating controls.",
                Tags = ["hyper-v", "nested-virtualization", "vm-security", "isolation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockNestedVirtualization", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockNestedVirtualization")],
                DetectOps = [RegOp.CheckDword(Key, "BlockNestedVirtualization", 1)],
            },
            new TweakDef
            {
                Id = "virtz-configure-vm-memory-protection",
                Label = "Configure Enhanced Memory Protection for Hyper-V Virtual Machines",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Enhanced memory protection for Hyper-V VMs ensures that virtual machine memory is isolated from unauthorized access by other VMs and the host management OS in ways that go beyond standard hypervisor isolation. Hypervisor-protected code integrity uses the hypervisor security boundary to protect kernel memory from modification by malicious code running in the VM. Memory protection features ensure that VM memory cannot be directly accessed by processes on the host even when the host has Hyper-V management privileges without going through the hypervisor management API. Shielded VMs provide the highest level of memory protection by encrypting VM memory and preventing host administrators from directly inspecting VM memory contents. Organizations that run VMs with sensitive workloads including regulated data or privileged authentication components should evaluate the appropriate level of memory protection for each VM type. Regular security reviews of VM memory protection configuration ensure that protection levels remain appropriate as VM workloads and threat models evolve.",
                Tags = ["hyper-v", "memory-protection", "vm-isolation", "hypervisor-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableEnhancedMemoryProtection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableEnhancedMemoryProtection")],
                DetectOps = [RegOp.CheckDword(Key, "EnableEnhancedMemoryProtection", 1)],
            },
        ];
    }
}

