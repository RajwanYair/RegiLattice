namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 642 — PolicyBitLocker (BitLocker Drive Encryption Group Policy)
// ─────────────────────────────────────────────────────────────────────────────
internal static class PolicyBitLocker
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE";
    private const string OsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE\OSVolume";
    private const string FdKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FVE\FDVDenyWriteAccess";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "fve-require-device-encryption",
            Label = "Require BitLocker Device Encryption",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enforces BitLocker device encryption on OS volumes via Group Policy. Devices that are not encrypted will be flagged non-compliant by MDM/Intune.",
            Tags = ["bitlocker", "encryption", "fve", "policy", "compliance"],
            RegistryKeys = [Key],
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Full-disk encryption enforced on OS drive; maximum data protection at rest.",
            ApplyOps = [RegOp.SetDword(Key, "RequireDeviceEncryption", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireDeviceEncryption")],
            DetectOps = [RegOp.CheckDword(Key, "RequireDeviceEncryption", 1)],
        },
        new TweakDef
        {
            Id = "fve-deny-write-fixed-notprotected",
            Label = "Block Write Access to Unencrypted Fixed Drives",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Denies write access to fixed data drives that are not protected by BitLocker. Users can still read unencrypted drives but cannot write to them until encryption is applied.",
            Tags = ["bitlocker", "fixed-drive", "write-protect", "fve", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Unencrypted fixed drives become read-only; forces encryption before use.",
            ApplyOps = [RegOp.SetDword(Key, "FDVDenyWriteAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "FDVDenyWriteAccess")],
            DetectOps = [RegOp.CheckDword(Key, "FDVDenyWriteAccess", 1)],
        },
        new TweakDef
        {
            Id = "fve-deny-write-removable-notprotected",
            Label = "Block Write Access to Unencrypted Removable Drives",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Denies write access to removable drives (USB flash drives, external HDDs) that are not BitLocker-protected. Prevents data exfiltration to unencrypted removable media.",
            Tags = ["bitlocker", "removable-drive", "usb", "write-protect", "fve", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Unencrypted USB/removable drives become read-only; data exfiltration blocked.",
            ApplyOps = [RegOp.SetDword(Key, "RDVDenyWriteAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RDVDenyWriteAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RDVDenyWriteAccess", 1)],
        },
        new TweakDef
        {
            Id = "fve-require-startup-pin",
            Label = "Require BitLocker Startup PIN",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Requires a user-defined PIN at startup before BitLocker releases the OS volume. Provides pre-boot authentication that blocks cold-boot attacks even if the TPM is compromised.",
            Tags = ["bitlocker", "pin", "startup", "pre-boot", "fve", "policy", "security"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Startup PIN required; cold-boot and direct memory attacks blocked.",
            ApplyOps = [RegOp.SetDword(Key, "UseAdvancedStartup", 1), RegOp.SetDword(Key, "UseTPMPIN", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "UseAdvancedStartup"), RegOp.DeleteValue(Key, "UseTPMPIN")],
            DetectOps = [RegOp.CheckDword(Key, "UseTPMPIN", 1)],
        },
        new TweakDef
        {
            Id = "fve-set-encryption-method-aes256",
            Label = "Set BitLocker Encryption Method to AES-256",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures BitLocker to use AES-256 (XTS-AES 256-bit) for OS and fixed drives. Provides the highest encryption strength; value 7 = XTS-AES 256, the recommended method for modern Windows.",
            Tags = ["bitlocker", "aes-256", "xts", "encryption-method", "fve", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "XTS-AES 256-bit encryption selected; stronger than default XTS-AES 128.",
            ApplyOps = [RegOp.SetDword(Key, "EncryptionMethodWithXtsFdv", 7), RegOp.SetDword(Key, "EncryptionMethodWithXtsOs", 7)],
            RemoveOps = [RegOp.DeleteValue(Key, "EncryptionMethodWithXtsFdv"), RegOp.DeleteValue(Key, "EncryptionMethodWithXtsOs")],
            DetectOps = [RegOp.CheckDword(Key, "EncryptionMethodWithXtsOs", 7)],
        },
        new TweakDef
        {
            Id = "fve-disable-network-unlock",
            Label = "Disable BitLocker Network Unlock",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables BitLocker Network Unlock, which automatically unlocks drives when the computer boots on a trusted corporate network. Eliminates network-based bypass of pre-boot authentication.",
            Tags = ["bitlocker", "network-unlock", "fve", "policy", "security"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Network-based auto-unlock disabled; PIN/password required even on trusted networks.",
            ApplyOps = [RegOp.SetDword(Key, "DisableNetworkUnlock", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkUnlock")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNetworkUnlock", 1)],
        },
        new TweakDef
        {
            Id = "fve-disable-recovery-to-ad",
            Label = "Disable BitLocker Recovery Key Storage in AD",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents BitLocker recovery keys from being automatically backed up to Active Directory / Entra ID. Useful when a separate, higher-security key escrow solution is mandated.",
            Tags = ["bitlocker", "recovery-key", "active-directory", "fve", "policy", "security"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Recovery keys not stored in AD/Entra; use only when alternate escrow exists.",
            ApplyOps = [RegOp.SetDword(Key, "DoNotBackupToAD", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DoNotBackupToAD")],
            DetectOps = [RegOp.CheckDword(Key, "DoNotBackupToAD", 1)],
        },
        new TweakDef
        {
            Id = "fve-set-min-pin-length",
            Label = "Set BitLocker Minimum PIN Length (8 digits)",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enforces a minimum of 8 digits for the BitLocker startup PIN. Default Windows minimum is 4 digits; 8+ significantly increases brute-force resistance for pre-boot authentication.",
            Tags = ["bitlocker", "pin", "minimum-length", "fve", "policy", "security"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Minimum 8-digit PIN enforced; brute-force attacks against startup PIN made impractical.",
            ApplyOps = [RegOp.SetDword(Key, "MinimumPIN", 8)],
            RemoveOps = [RegOp.DeleteValue(Key, "MinimumPIN")],
            DetectOps = [RegOp.CheckDword(Key, "MinimumPIN", 8)],
        },
        new TweakDef
        {
            Id = "fve-allow-bitlocker-without-tpm",
            Label = "Allow BitLocker Without TPM Chip",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Allows BitLocker to encrypt drives on computers without a TPM chip, using a startup password or USB key instead. Default Windows policy requires TPM; this allows legacy/virtual machines to use BitLocker.",
            Tags = ["bitlocker", "tpm", "no-tpm", "startup-key", "fve", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Non-TPM machines can use BitLocker with password/USB key; useful for VMs.",
            ApplyOps = [RegOp.SetDword(Key, "EnableNonTPM", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableNonTPM")],
            DetectOps = [RegOp.CheckDword(Key, "EnableNonTPM", 1)],
        },
        new TweakDef
        {
            Id = "fve-disable-used-space-only",
            Label = "Enforce Full Drive Encryption (Not Used-Space Only)",
            Category = "Encryption",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces BitLocker to encrypt the entire drive including free space, not just used space. Prevents forensic recovery of previously deleted files from unencrypted free space sectors.",
            Tags = ["bitlocker", "full-encryption", "used-space", "fve", "policy", "forensics"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "All sectors encrypted including free space; forensic recovery of deleted files blocked.",
            ApplyOps = [RegOp.SetDword(Key, "OSAllowedHardwareEncryptionAlgorithms", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "OSAllowedHardwareEncryptionAlgorithms")],
            DetectOps = [RegOp.CheckDword(Key, "OSAllowedHardwareEncryptionAlgorithms", 0)],
        },
    ];
}

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 643 — PolicyWindowsInk (Windows Ink Workspace Group Policy)
// ─────────────────────────────────────────────────────────────────────────────
internal static class PolicyWindowsInk
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "winks-disable-ink-workspace",
            Label = "Disable Windows Ink Workspace",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Ink Workspace button and panel via Group Policy. Removes the ink toolbar from the taskbar and prevents users from accessing pen, sticky notes, and whiteboard tools.",
            Tags = ["ink", "pen", "tablet", "policy", "privacy", "windows-ink"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ink Workspace panel hidden; pen tablet shortcut button removed from taskbar.",
            ApplyOps = [RegOp.SetDword(Key, "AllowWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword(Key, "AllowWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "winks-disable-ink-above-lock",
            Label = "Disable Ink Workspace Above Lock Screen",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks access to Windows Ink Workspace when the device is locked. Prevents unauthenticated users from using ink features including sticky notes from the lock screen.",
            Tags = ["ink", "lock-screen", "policy", "privacy", "windows-ink", "security"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Ink tools blocked on lock screen; no unauthenticated access to pen features.",
            ApplyOps = [RegOp.SetDword(Key, "AllowWindowsInkWorkspace", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword(Key, "AllowWindowsInkWorkspace", 1)],
        },
        new TweakDef
        {
            Id = "winks-disable-ink-suggested-apps",
            Label = "Disable Ink Workspace Suggested Apps",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Microsoft Store app suggestions shown in Windows Ink Workspace. Stops advertising-style prompts that appear alongside pen tools.",
            Tags = ["ink", "suggested-apps", "ads", "policy", "privacy", "windows-ink"],
            RegistryKeys = [Key],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "App suggestions removed from Ink Workspace; no store prompts.",
            ApplyOps = [RegOp.SetDword(Key, "AllowSuggestedAppsInWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowSuggestedAppsInWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword(Key, "AllowSuggestedAppsInWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "winks-disable-ink-touch-keyboard-autoinvoke",
            Label = "Disable Touch Keyboard Auto-Invoke in Ink",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the touch keyboard from automatically appearing when a text field is focused in Windows Ink apps. Reduces accidental keyboard pop-ups on pen-only tablet workflows.",
            Tags = ["ink", "touch-keyboard", "tablet", "policy", "windows-ink"],
            RegistryKeys = [Key],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Touch keyboard no longer auto-invokes in ink context; pen workflow uninterrupted.",
            ApplyOps = [RegOp.SetDword(Key, "TouchKeyboardAutoInvokeEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "TouchKeyboardAutoInvokeEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "TouchKeyboardAutoInvokeEnabled", 0)],
        },
        new TweakDef
        {
            Id = "winks-disable-ink-handwriting-panel",
            Label = "Disable Handwriting Panel",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the handwriting input panel that appears when a text field is focused in tablet/pen mode. Reduces attack surface and prevents handwriting data from being sent to Microsoft for personalization.",
            Tags = ["ink", "handwriting", "tablet", "policy", "privacy", "windows-ink"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Handwriting panel disabled; pen input still works but input panel hidden.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingDataSharing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingDataSharing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingDataSharing", 1)],
        },
        new TweakDef
        {
            Id = "winks-disable-ink-handwriting-error-reports",
            Label = "Disable Handwriting Error Reporting",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from sending handwriting recognition error reports to Microsoft. These reports include ink strokes and are stored on Microsoft servers for handwriting model improvement.",
            Tags = ["ink", "handwriting", "error-report", "privacy", "telemetry", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ink stroke error reports not sent to Microsoft; handwriting data stays local.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingErrorReports", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingErrorReports")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingErrorReports", 1)],
        },
        new TweakDef
        {
            Id = "winks-disable-ink-personalization",
            Label = "Disable Ink Personalization Data Collection",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Microsoft's collection of inking and typing data used to improve autocorrect and word suggestions. Prevents handwriting samples and typed text patterns from being uploaded to Microsoft.",
            Tags = ["ink", "personalization", "privacy", "telemetry", "policy", "data-collection"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ink/typing personalization data collection stopped; no usage samples sent to Microsoft.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization", "RestrictImplicitInkCollection")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1),
            ],
        },
        new TweakDef
        {
            Id = "winks-disable-typing-personalization",
            Label = "Disable Typing Personalization Data Collection",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from collecting implicit text-input data for personalized autocorrect and text prediction. Stops background collection of typed word statistics sent to Microsoft.",
            Tags = ["ink", "typing", "personalization", "privacy", "telemetry", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Implicit typing data collection blocked; autocorrect suggestions use local data only.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization", "RestrictImplicitTextCollection")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 1),
            ],
        },
        new TweakDef
        {
            Id = "winks-disable-ink-learning-mode",
            Label = "Disable Ink Learning Mode",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from learning your ink writing style for personalization. Disables the adaptive handwriting recognition model that builds ink profiles per user.",
            Tags = ["ink", "learning", "personalization", "privacy", "policy", "windows-ink"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Ink style learning disabled; no local ink profile built.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization", "AllowInputPersonalization", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization", "AllowInputPersonalization")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization", "AllowInputPersonalization", 0)],
        },
        new TweakDef
        {
            Id = "winks-disable-ink-workspace-telemetry",
            Label = "Disable Windows Ink Workspace Telemetry",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks telemetry data collection from Windows Ink Workspace usage patterns. Prevents Microsoft from receiving information about which ink tools are used and how frequently.",
            Tags = ["ink", "telemetry", "privacy", "policy", "windows-ink", "data-collection"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ink workspace usage statistics not reported to Microsoft.",
            ApplyOps = [RegOp.SetDword(Key, "AllowWindowsInkWorkspaceTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowWindowsInkWorkspaceTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "AllowWindowsInkWorkspaceTelemetry", 0)],
        },
    ];
}

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 644 — PolicyLocationSensors (Location & Sensors Group Policy)
// ─────────────────────────────────────────────────────────────────────────────
internal static class PolicyLocationSensors
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "locsvc-disable-location",
            Label = "Disable Location Services (System-Wide)",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Location Services system feature via Group Policy. Prevents all applications from accessing the location sensor. Stronger than the per-app user setting.",
            Tags = ["location", "gps", "sensors", "policy", "privacy"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Location access disabled for all apps system-wide; GPS/Wi-Fi geolocation blocked.",
            ApplyOps = [RegOp.SetDword(Key, "DisableLocation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLocation")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLocation", 1)],
        },
        new TweakDef
        {
            Id = "locsvc-disable-location-scripting",
            Label = "Disable Location Scripting API",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents scripts (JScript, VBScript) from accessing the Windows Location API. Blocks web-based or scripting attacks that could request fine-grained location data.",
            Tags = ["location", "scripting", "policy", "privacy", "security"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Script-based location queries blocked; web scripts cannot access GPS/Wi-Fi location.",
            ApplyOps = [RegOp.SetDword(Key, "DisableLocationScripting", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLocationScripting")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLocationScripting", 1)],
        },
        new TweakDef
        {
            Id = "locsvc-disable-sensors",
            Label = "Disable Sensor Platform",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Sensor Platform that allows applications to access ambient light, accelerometer, gyroscope, and other physical sensors. Reduces hardware fingerprinting attack surface.",
            Tags = ["sensors", "accelerometer", "ambient-light", "policy", "privacy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Physical sensor access blocked for all apps; fingerprinting via motion sensors prevented.",
            ApplyOps = [RegOp.SetDword(Key, "DisableSensors", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSensors")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSensors", 1)],
        },
        new TweakDef
        {
            Id = "locsvc-disable-windows-location-provider",
            Label = "Disable Windows Location Provider Service",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Location Provider service that uses Wi-Fi, Bluetooth, and IP address data to estimate geographic location without a GPS chip.",
            Tags = ["location", "wi-fi", "bluetooth", "geolocation", "policy", "privacy"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Wi-Fi/BT based geolocation provider disabled; IP-based location fallback also blocked.",
            ApplyOps = [RegOp.SetDword(Key, "DisableWindowsLocationProvider", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsLocationProvider")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWindowsLocationProvider", 1)],
        },
        new TweakDef
        {
            Id = "locsvc-disable-location-awareness",
            Label = "Disable Network Location Awareness",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows Network Location Awareness (NLA) from uploading network topology data used to improve geolocation services. Stops SSID/BSSID mapping data from being sent to Microsoft.",
            Tags = ["location", "network", "nla", "privacy", "policy", "telemetry"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Network topology data not contributed to Microsoft location database.",
            ApplyOps = [RegOp.SetDword(Key, "DisableNetworkLocationAwareness", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkLocationAwareness")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNetworkLocationAwareness", 1)],
        },
        new TweakDef
        {
            Id = "locsvc-disable-location-telemetry",
            Label = "Disable Location Service Telemetry Upload",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks Windows from uploading location service usage telemetry to Microsoft. Prevents location access frequency, accuracy levels, and app-level location events from being reported.",
            Tags = ["location", "telemetry", "privacy", "policy", "data-collection"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Location usage telemetry not uploaded; no location event data reaches Microsoft.",
            ApplyOps = [RegOp.SetDword(Key, "DisableLocationTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLocationTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLocationTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "locsvc-disable-location-history",
            Label = "Disable Location History Storage",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from storing location history on the device. Location history is used by apps like Maps to show frequently visited places; disabling it keeps no local location log.",
            Tags = ["location", "history", "privacy", "policy", "data-storage"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "No location history stored on device; visited places not recorded.",
            ApplyOps = [RegOp.SetDword(Key, "DisableLocationHistory", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLocationHistory")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLocationHistory", 1)],
        },
        new TweakDef
        {
            Id = "locsvc-disable-geofencing",
            Label = "Disable Geofencing API",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks the Windows Geofencing API that allows applications to define geographic boundaries and trigger events when the device enters or exits them. Prevents background location monitoring by geofencing-aware apps.",
            Tags = ["location", "geofencing", "background", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Geofencing triggers disabled; apps cannot monitor if device enters/exits geographic areas.",
            ApplyOps = [RegOp.SetDword(Key, "DisableGeofencing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableGeofencing")],
            DetectOps = [RegOp.CheckDword(Key, "DisableGeofencing", 1)],
        },
        new TweakDef
        {
            Id = "locsvc-disable-sensor-data-service",
            Label = "Disable Sensor Data Service",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Sensor Data Service that aggregates sensor readings from multiple physical sensors and exposes them to applications. Stopping this service prevents all sensor-based fingerprinting.",
            Tags = ["sensors", "service", "privacy", "policy", "fingerprinting"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Unified sensor aggregation service stopped; no sensor data exposed to apps.",
            ApplyOps = [RegOp.SetDword(Key, "DisableSensorDataService", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSensorDataService")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSensorDataService", 1)],
        },
        new TweakDef
        {
            Id = "locsvc-disable-light-sensor",
            Label = "Disable Ambient Light Sensor",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the ambient light sensor from being accessible to applications via the Windows Sensor API. Prevents light-level data from being used for screen brightness fingerprinting or environment profiling.",
            Tags = ["sensors", "ambient-light", "privacy", "policy", "fingerprinting"],
            RegistryKeys = [Key],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Ambient light sensor data not exposed to apps; environment-based profiling blocked.",
            ApplyOps = [RegOp.SetDword(Key, "DisableAmbientLightSensor", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAmbientLightSensor")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAmbientLightSensor", 1)],
        },
    ];
}

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 645 — PolicyCloudClipboard (Cloud Clipboard & Clipboard History Policy)
// ─────────────────────────────────────────────────────────────────────────────
internal static class PolicyCloudClipboard
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "clipol-disable-clipboard-history",
            Label = "Disable Clipboard History (Win+V)",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Clipboard History feature (Win+V) via Group Policy. Clipboard history stores the last 25 items copied so users can paste previous clips; disabling prevents all copied data from being retained.",
            Tags = ["clipboard", "history", "privacy", "policy", "win+v"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Clipboard history disabled; Win+V shows empty; only current clipboard item retained.",
            ApplyOps = [RegOp.SetDword(Key, "AllowClipboardHistory", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowClipboardHistory")],
            DetectOps = [RegOp.CheckDword(Key, "AllowClipboardHistory", 0)],
        },
        new TweakDef
        {
            Id = "clipol-disable-cross-device-clipboard",
            Label = "Disable Cross-Device Clipboard Sync",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Cloud Clipboard feature that synchronises clipboard content between Windows devices signed into the same Microsoft account. Prevents copied text, images, and documents from being uploaded to Microsoft servers.",
            Tags = ["clipboard", "cloud", "sync", "cross-device", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Clipboard data not synced to cloud or other devices; stays local only.",
            ApplyOps = [RegOp.SetDword(Key, "AllowCrossDeviceClipboard", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCrossDeviceClipboard")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCrossDeviceClipboard", 0)],
        },
        new TweakDef
        {
            Id = "clipol-disable-phone-clipboard-sync",
            Label = "Disable Phone-to-PC Clipboard Sync",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents clipboard content from being shared between a paired Android/iPhone and the Windows PC via Phone Link. Disables the mobile-to-desktop clipboard relay channel.",
            Tags = ["clipboard", "phone", "android", "phone-link", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Phone-to-PC clipboard bridge disabled; mobile clipboard items not transferred to PC.",
            ApplyOps = [RegOp.SetDword(Key, "DisableClipboardForPhone", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardForPhone")],
            DetectOps = [RegOp.CheckDword(Key, "DisableClipboardForPhone", 1)],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-gpt-integration",
            Label = "Disable Clipboard AI / Copilot Integration",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks Windows Copilot and AI features from reading clipboard content for contextual suggestions. Prevents AI models from processing clipboard data including passwords, banking information, or confidential documents inadvertently copied.",
            Tags = ["clipboard", "ai", "copilot", "gpt", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "AI/Copilot clipboard access blocked; sensitive copied data not processed by AI.",
            ApplyOps = [RegOp.SetDword(Key, "AllowCopilotClipboardAccess", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCopilotClipboardAccess")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCopilotClipboardAccess", 0)],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-hello-sync",
            Label = "Disable Clipboard Sync via Windows Hello",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents clipboard content from being relayed between devices using Windows Hello companion device authentication. Stops clipboard sharing initiated through the Hello companion device framework.",
            Tags = ["clipboard", "windows-hello", "sync", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Hello companion-device clipboard relay disabled.",
            ApplyOps = [RegOp.SetDword(Key, "AllowCrossDeviceClipboardViaWindowsHello", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCrossDeviceClipboardViaWindowsHello")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCrossDeviceClipboardViaWindowsHello", 0)],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-rdp-passthrough",
            Label = "Disable RDP Clipboard Passthrough",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks clipboard redirection between an RDP session and the remote machine. Prevents users from pasting data from a remote desktop session into local applications, blocking a common data-exfiltration channel.",
            Tags = ["clipboard", "rdp", "remote-desktop", "exfiltration", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "RDP clipboard redirect blocked; copy-paste between RDP and local machine disabled.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "DisableClipboardRedirection", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "DisableClipboardRedirection"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "DisableClipboardRedirection", 1),
            ],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-remote-viewer",
            Label = "Disable Clipboard in Remote Assistance Sessions",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks clipboard access during Windows Remote Assistance sessions. Prevents the remote assistant from copying sensitive data from the user's clipboard during a coached support session.",
            Tags = ["clipboard", "remote-assistance", "security", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Remote assistance helper cannot access clipboard; data exfiltration during support blocked.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
        },
        new TweakDef
        {
            Id = "clipol-clear-clipboard-on-lock",
            Label = "Clear Clipboard on Screen Lock",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Automatically clears all clipboard history and the current clipboard when the screen locks. Prevents sensitive information (passwords, tokens, PII) from remaining in clipboard after the user leaves their desk.",
            Tags = ["clipboard", "lock-screen", "clear", "privacy", "policy", "security"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Clipboard contents wiped on every screen lock; sensitive data never retained when unattended.",
            ApplyOps = [RegOp.SetDword(Key, "ClearClipboardOnLock", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ClearClipboardOnLock")],
            DetectOps = [RegOp.CheckDword(Key, "ClearClipboardOnLock", 1)],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-audit",
            Label = "Disable Clipboard Audit Logging",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables clipboard operation audit logging in the Windows Security event log. Stops clipboard read/write events from being written to Security log for privacy-focused deployments without audit requirements.",
            Tags = ["clipboard", "audit", "logging", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Clipboard operations not logged to Security audit log.",
            ApplyOps = [RegOp.SetDword(Key, "ClipboardAuditLogging", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "ClipboardAuditLogging")],
            DetectOps = [RegOp.CheckDword(Key, "ClipboardAuditLogging", 0)],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-suggested-actions",
            Label = "Disable Clipboard Smart Actions / Suggested Text Actions",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Clipboard smart actions feature that analyses copied text and suggests contextual actions (add to calendar, call a phone number, open a URL). Stops clipboard content from being sent to local AI processing pipelines.",
            Tags = ["clipboard", "smart-actions", "ai", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Clipboard content analysis for smart actions disabled; no AI text interpretation of copied data.",
            ApplyOps = [RegOp.SetDword(Key, "AllowClipboardSuggestedActions", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowClipboardSuggestedActions")],
            DetectOps = [RegOp.CheckDword(Key, "AllowClipboardSuggestedActions", 0)],
        },
    ];
}

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 646 — PolicyNetworkIsolation (Network Isolation / AppContainer Policy)
// ─────────────────────────────────────────────────────────────────────────────
internal static class PolicyNetworkIsolation
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkIsolation";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "netiso-block-domain-enterprise-exceptions",
            Label = "Block AppContainer Domain Enterprise Exception Bypass",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows Store apps running in AppContainer from adding themselves to the Enterprise authentication exception list. Stops apps from bypassing network isolation to access domain resources.",
            Tags = ["network", "isolation", "appcontainer", "security", "policy", "uwp"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "AppContainer apps cannot self-add to domain exception list; network isolation enforced.",
            ApplyOps = [RegOp.SetDword(Key, "BlockDomainEnterprise", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockDomainEnterprise")],
            DetectOps = [RegOp.CheckDword(Key, "BlockDomainEnterprise", 1)],
        },
        new TweakDef
        {
            Id = "netiso-disable-appcontainer-loopback",
            Label = "Disable AppContainer Loopback Exemption",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Removes the loopback exemption for Windows Store apps (AppContainer), forcing them to go through the network stack for all traffic. By default, AppContainer apps can bypass the loopback for localhost communications; removing this closes the bypass.",
            Tags = ["network", "isolation", "appcontainer", "loopback", "security", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "AppContainer loopback bypass removed; all app traffic goes through network isolation.",
            ApplyOps = [RegOp.SetDword(Key, "DisableLoopback", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLoopback")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLoopback", 1)],
        },
        new TweakDef
        {
            Id = "netiso-require-package-authentication",
            Label = "Require Package Family Authentication for Network Isolation",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Requires that applications declare their Package Family Name (PFN) to access enterprise resources beyond the AppContainer boundary. Strengthens network isolation capability attestation.",
            Tags = ["network", "isolation", "authentication", "pfn", "security", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Apps must declare PFN for enterprise network access; undeclared apps blocked.",
            ApplyOps = [RegOp.SetDword(Key, "RequirePackageAuthentication", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequirePackageAuthentication")],
            DetectOps = [RegOp.CheckDword(Key, "RequirePackageAuthentication", 1)],
        },
        new TweakDef
        {
            Id = "netiso-disable-intranet-lookup",
            Label = "Disable Automatic Intranet Address Classification",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Stops Windows from automatically classifying internal IP ranges as 'intranet' for network isolation purposes. Prevents Store apps from accidentally gaining enterprise network access based on auto-detected IP ranges.",
            Tags = ["network", "isolation", "intranet", "classification", "security", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Auto-intranet detection disabled; only explicitly defined ranges treated as intranet.",
            ApplyOps = [RegOp.SetDword(Key, "DisableIntranetLookup", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableIntranetLookup")],
            DetectOps = [RegOp.CheckDword(Key, "DisableIntranetLookup", 1)],
        },
        new TweakDef
        {
            Id = "netiso-block-proxy-browsing",
            Label = "Block AppContainer Internet Proxy Access",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks Windows Store apps from using the system proxy for internet access. AppContainer-isolated apps are restricted from routing traffic through corporate proxies, preventing proxy-bypass attacks.",
            Tags = ["network", "isolation", "proxy", "appcontainer", "security", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Store apps cannot route through system proxy; direct proxy access from AppContainer blocked.",
            ApplyOps = [RegOp.SetDword(Key, "BlockProxyBrowsing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockProxyBrowsing")],
            DetectOps = [RegOp.CheckDword(Key, "BlockProxyBrowsing", 1)],
        },
        new TweakDef
        {
            Id = "netiso-disable-network-capability-autodeny",
            Label = "Deny AppContainer Network Capability by Default",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the default policy for network capability declarations by AppContainer apps to deny. Apps must be explicitly whitelisted to access network resources rather than being allowed by default.",
            Tags = ["network", "isolation", "appcontainer", "capability", "deny-default", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Default-deny network capability for Store apps; explicit allow-list required.",
            ApplyOps = [RegOp.SetDword(Key, "DenyNetworkByDefault", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DenyNetworkByDefault")],
            DetectOps = [RegOp.CheckDword(Key, "DenyNetworkByDefault", 1)],
        },
        new TweakDef
        {
            Id = "netiso-require-private-network-declaration",
            Label = "Require Explicit Private Network Capability Declaration",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Requires that AppContainer apps explicitly declare the 'privateNetworkClientServer' capability in their manifest to access local network resources. Prevents undeclared apps from silently accessing home/enterprise network devices.",
            Tags = ["network", "isolation", "private-network", "capability", "security", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Private network access requires manifest declaration; undeclared apps blocked from LAN.",
            ApplyOps = [RegOp.SetDword(Key, "RequirePrivateNetworkDeclaration", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequirePrivateNetworkDeclaration")],
            DetectOps = [RegOp.CheckDword(Key, "RequirePrivateNetworkDeclaration", 1)],
        },
        new TweakDef
        {
            Id = "netiso-block-internet-appcontainer",
            Label = "Block AppContainer Direct Internet Access",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks AppContainer-sandboxed apps from making direct internet connections. All internet traffic from Store apps must go through a declared network capability and optionally a firewall policy.",
            Tags = ["network", "isolation", "internet", "appcontainer", "firewall", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Direct internet connections from AppContainer apps blocked; capability declaration required.",
            ApplyOps = [RegOp.SetDword(Key, "BlockInternetAppContainer", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockInternetAppContainer")],
            DetectOps = [RegOp.CheckDword(Key, "BlockInternetAppContainer", 1)],
        },
        new TweakDef
        {
            Id = "netiso-disable-isolation-debug",
            Label = "Disable Network Isolation Debug Mode",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents enabling the Network Isolation debug/diagnostic mode that bypasses AppContainer restrictions for troubleshooting. Debug mode can be used to permanently relax isolation on developer machines.",
            Tags = ["network", "isolation", "debug", "security", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Debug bypass of network isolation disabled; isolation policies cannot be bypassed for diagnosis.",
            ApplyOps = [RegOp.SetDword(Key, "DisableIsolationDebug", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableIsolationDebug")],
            DetectOps = [RegOp.CheckDword(Key, "DisableIsolationDebug", 1)],
        },
        new TweakDef
        {
            Id = "netiso-enable-strict-capability-enforcement",
            Label = "Enforce Strict AppContainer Network Capability Checking",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables strict mode for AppContainer network capability enforcement. In strict mode, any capability not explicitly declared in the app manifest is denied without fallback, tightening the network isolation boundary.",
            Tags = ["network", "isolation", "strict", "capability", "appcontainer", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Strict capability enforcement active; any undeclared network access denied.",
            ApplyOps = [RegOp.SetDword(Key, "StrictCapabilityEnforcement", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "StrictCapabilityEnforcement")],
            DetectOps = [RegOp.CheckDword(Key, "StrictCapabilityEnforcement", 1)],
        },
    ];
}
