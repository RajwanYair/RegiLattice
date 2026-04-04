namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 637 — PolicyFido (FIDO2 / WebAuthn security-key policy)
// ─────────────────────────────────────────────────────────────────────────────
internal static class PolicyFido
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FIDO";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "fido-enable-security-key-signin",
            Label = "Enable FIDO2 Security Key Sign-In",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables FIDO2 hardware security key logon at the Windows sign-in screen. Users can authenticate with a YubiKey, Titan Key, or any CTAP2-compliant device instead of a password.",
            Tags = ["fido", "security-key", "authentication", "mfa", "policy", "webauthn"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Hardware security key sign-in enabled; strong phishing-resistant MFA at logon.",
            ApplyOps = [RegOp.SetDword(Key, "EnableFIDODeviceLogon", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableFIDODeviceLogon")],
            DetectOps = [RegOp.CheckDword(Key, "EnableFIDODeviceLogon", 1)],
        },
        new TweakDef
        {
            Id = "fido-disable-security-key-signin",
            Label = "Block FIDO2 Security Key Sign-In",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks FIDO2 hardware security key logon via Group Policy. Useful for environments where all authentication must go through managed identity providers.",
            Tags = ["fido", "security-key", "block", "policy", "authentication"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "FIDO2 hardware key sign-in blocked; use only when alternative MFA is mandated.",
            ApplyOps = [RegOp.SetDword(Key, "EnableFIDODeviceLogon", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableFIDODeviceLogon")],
            DetectOps = [RegOp.CheckDword(Key, "EnableFIDODeviceLogon", 0)],
        },
        new TweakDef
        {
            Id = "fido-require-user-presence",
            Label = "Require User Presence for FIDO Authentication",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enforces that FIDO2 authentication always requires a physical user-presence test (button touch). Prevents silent automated authentication without user interaction.",
            Tags = ["fido", "user-presence", "policy", "authentication", "security"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "FIDO key must be physically touched; silent automated use blocked.",
            ApplyOps = [RegOp.SetDword(Key, "EnablePresenceRequired", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnablePresenceRequired")],
            DetectOps = [RegOp.CheckDword(Key, "EnablePresenceRequired", 1)],
        },
        new TweakDef
        {
            Id = "fido-enable-nfc-security-keys",
            Label = "Enable NFC FIDO2 Security Keys",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Allows NFC-based FIDO2 security keys at Windows sign-in via the FIDO device logon policy. Enables contactless hardware authentication for environments using NFC-capable keys.",
            Tags = ["fido", "nfc", "security-key", "authentication", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "NFC FIDO2 keys enabled for logon; contactless hardware MFA supported.",
            ApplyOps = [RegOp.SetDword(Key, "EnableNFCFIDODevices", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableNFCFIDODevices")],
            DetectOps = [RegOp.CheckDword(Key, "EnableNFCFIDODevices", 1)],
        },
        new TweakDef
        {
            Id = "fido-disable-nfc-security-keys",
            Label = "Block NFC FIDO2 Security Keys",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks NFC-based FIDO2 security keys via Group Policy. Use when only USB FIDO keys are approved for your environment.",
            Tags = ["fido", "nfc", "block", "policy", "authentication"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "NFC FIDO2 keys blocked; only USB keys permitted for logon.",
            ApplyOps = [RegOp.SetDword(Key, "EnableNFCFIDODevices", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableNFCFIDODevices")],
            DetectOps = [RegOp.CheckDword(Key, "EnableNFCFIDODevices", 0)],
        },
        new TweakDef
        {
            Id = "fido-enable-ble-security-keys",
            Label = "Enable Bluetooth FIDO2 Security Keys",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Allows Bluetooth LE-based FIDO2 security keys at Windows sign-in. Enables wireless hardware authentication for environments permitting BLE hardware security keys.",
            Tags = ["fido", "bluetooth", "ble", "security-key", "authentication", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "BLE FIDO2 keys enabled for logon; wireless hardware MFA supported.",
            ApplyOps = [RegOp.SetDword(Key, "EnableBLEFIDODevices", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableBLEFIDODevices")],
            DetectOps = [RegOp.CheckDword(Key, "EnableBLEFIDODevices", 1)],
        },
        new TweakDef
        {
            Id = "fido-disable-ble-security-keys",
            Label = "Block Bluetooth FIDO2 Security Keys",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks Bluetooth LE FIDO2 security keys via Group Policy. Eliminates wireless hardware key sign-in attack surface in high-security environments.",
            Tags = ["fido", "bluetooth", "ble", "block", "policy", "authentication"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "BLE FIDO2 keys blocked; only wired/NFC keys allowed.",
            ApplyOps = [RegOp.SetDword(Key, "EnableBLEFIDODevices", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableBLEFIDODevices")],
            DetectOps = [RegOp.CheckDword(Key, "EnableBLEFIDODevices", 0)],
        },
        new TweakDef
        {
            Id = "fido-require-attestation",
            Label = "Require FIDO2 Key Attestation",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Requires attestation from FIDO2 security keys during registration. Ensures only manufacturer-verified hardware can be enrolled, blocking counterfeit or unapproved keys.",
            Tags = ["fido", "attestation", "policy", "authentication", "hardware", "security"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Only attested FIDO2 keys with verified manufacturing credentials can be registered.",
            ApplyOps = [RegOp.SetDword(Key, "RequireAttestation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireAttestation")],
            DetectOps = [RegOp.CheckDword(Key, "RequireAttestation", 1)],
        },
        new TweakDef
        {
            Id = "fido-enable-enterprise-attestation",
            Label = "Enable FIDO2 Enterprise Attestation",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables enterprise attestation for FIDO2 security keys. Allows the relying party to receive full manufacturer attestation data for auditing and key inventory management.",
            Tags = ["fido", "enterprise", "attestation", "policy", "authentication", "audit"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Full attestation data available to relying party; key inventory auditable.",
            ApplyOps = [RegOp.SetDword(Key, "EnableEnterpriseAttestation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableEnterpriseAttestation")],
            DetectOps = [RegOp.CheckDword(Key, "EnableEnterpriseAttestation", 1)],
        },
        new TweakDef
        {
            Id = "fido-disable-roaming-credentials",
            Label = "Disable FIDO2 Roaming Credentials",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents FIDO2 credentials from roaming between devices via Windows credential manager sync. Keeps authentication credentials strictly bound to the enrolled hardware key and device pair.",
            Tags = ["fido", "roaming", "credentials", "policy", "authentication", "isolation"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "FIDO2 credentials isolated to the device; no cross-device sync.",
            ApplyOps = [RegOp.SetDword(Key, "DisableRoamingCredentials", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRoamingCredentials")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRoamingCredentials", 1)],
        },
    ];
}

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 638 — PolicyWindowsHello (Windows Hello for Business PIN policy)
// ─────────────────────────────────────────────────────────────────────────────
internal static class PolicyWindowsHello
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";
    private const string PinKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork\PINComplexity";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "whi-disable-windows-hello",
            Label = "Disable Windows Hello for Business",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Hello for Business provisioning via Group Policy. Prevents users from enrolling biometrics or PINs as a primary corporate credential. Does not affect local PIN sign-in.",
            Tags = ["windows-hello", "whfb", "policy", "authentication", "disable"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Windows Hello for Business provisioning blocked; password-based domain auth required.",
            ApplyOps = [RegOp.SetDword(Key, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "Enabled")],
            DetectOps = [RegOp.CheckDword(Key, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "whi-require-tpm",
            Label = "Require TPM for Windows Hello",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Requires a Trusted Platform Module (TPM) chip to provision Windows Hello for Business. Prevents Hello enrollment on devices without hardware security — credentials are bound to the TPM.",
            Tags = ["windows-hello", "tpm", "policy", "authentication", "hardware"],
            RegistryKeys = [Key],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Hello credentials require TPM hardware; software-only Hello blocked.",
            ApplyOps = [RegOp.SetDword(Key, "RequireSecurityDevice", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireSecurityDevice")],
            DetectOps = [RegOp.CheckDword(Key, "RequireSecurityDevice", 1)],
        },
        new TweakDef
        {
            Id = "whi-set-pin-min-length-8",
            Label = "Set Windows Hello PIN Minimum Length to 8",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enforces a minimum PIN length of 8 digits for Windows Hello. Short PINs are statistically easier to guess; 8 characters provides a baseline against brute-force attacks.",
            Tags = ["windows-hello", "pin", "complexity", "policy", "authentication"],
            RegistryKeys = [PinKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Hello PIN must be at least 8 characters; brute-force resistance improved.",
            ApplyOps = [RegOp.SetDword(PinKey, "MinimumPINLength", 8)],
            RemoveOps = [RegOp.DeleteValue(PinKey, "MinimumPINLength")],
            DetectOps = [RegOp.CheckDword(PinKey, "MinimumPINLength", 8)],
        },
        new TweakDef
        {
            Id = "whi-set-pin-max-length-127",
            Label = "Set Windows Hello PIN Maximum Length to 127",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the maximum Windows Hello PIN length to 127 characters. Allows extremely long PINs for high-security deployments that treat the PIN as a passphrase.",
            Tags = ["windows-hello", "pin", "complexity", "policy", "passphrase"],
            RegistryKeys = [PinKey],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Hello PIN up to 127 chars; passphrase-length PINs permitted.",
            ApplyOps = [RegOp.SetDword(PinKey, "MaximumPINLength", 127)],
            RemoveOps = [RegOp.DeleteValue(PinKey, "MaximumPINLength")],
            DetectOps = [RegOp.CheckDword(PinKey, "MaximumPINLength", 127)],
        },
        new TweakDef
        {
            Id = "whi-require-pin-digits",
            Label = "Require Digits in Windows Hello PIN",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Requires at least one digit (0–9) in the Windows Hello PIN. Combined with uppercase/lowercase/special requirements, significantly increases PIN entropy.",
            Tags = ["windows-hello", "pin", "digits", "complexity", "policy"],
            RegistryKeys = [PinKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Hello PIN must include at least one digit; prevents all-alpha PINs.",
            ApplyOps = [RegOp.SetDword(PinKey, "Digits", 1)],
            RemoveOps = [RegOp.DeleteValue(PinKey, "Digits")],
            DetectOps = [RegOp.CheckDword(PinKey, "Digits", 1)],
        },
        new TweakDef
        {
            Id = "whi-require-pin-uppercase",
            Label = "Require Uppercase Letters in Windows Hello PIN",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Requires at least one uppercase letter in the Windows Hello PIN. Enforces mixed-case alphanumeric PIN composition for improved entropy.",
            Tags = ["windows-hello", "pin", "uppercase", "complexity", "policy"],
            RegistryKeys = [PinKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Hello PIN must include at least one uppercase letter.",
            ApplyOps = [RegOp.SetDword(PinKey, "UppercaseLetters", 1)],
            RemoveOps = [RegOp.DeleteValue(PinKey, "UppercaseLetters")],
            DetectOps = [RegOp.CheckDword(PinKey, "UppercaseLetters", 1)],
        },
        new TweakDef
        {
            Id = "whi-require-pin-lowercase",
            Label = "Require Lowercase Letters in Windows Hello PIN",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Requires at least one lowercase letter in the Windows Hello PIN. Combined with digit and uppercase requirements, PIN entropy is substantially increased.",
            Tags = ["windows-hello", "pin", "lowercase", "complexity", "policy"],
            RegistryKeys = [PinKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Hello PIN must include at least one lowercase letter.",
            ApplyOps = [RegOp.SetDword(PinKey, "LowercaseLetters", 1)],
            RemoveOps = [RegOp.DeleteValue(PinKey, "LowercaseLetters")],
            DetectOps = [RegOp.CheckDword(PinKey, "LowercaseLetters", 1)],
        },
        new TweakDef
        {
            Id = "whi-require-pin-special-chars",
            Label = "Require Special Characters in Windows Hello PIN",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Requires at least one special character in the Windows Hello PIN (!@#$%^&* etc.). Maximum PIN entropy mode — treat the PIN like a strong password.",
            Tags = ["windows-hello", "pin", "special-chars", "complexity", "policy"],
            RegistryKeys = [PinKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Hello PIN must include at least one special character.",
            ApplyOps = [RegOp.SetDword(PinKey, "SpecialCharacters", 1)],
            RemoveOps = [RegOp.DeleteValue(PinKey, "SpecialCharacters")],
            DetectOps = [RegOp.CheckDword(PinKey, "SpecialCharacters", 1)],
        },
        new TweakDef
        {
            Id = "whi-set-pin-expiry-90-days",
            Label = "Set Windows Hello PIN Expiry to 90 Days",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures Windows Hello PINs to expire after 90 days, prompting users to create a new PIN. Aligns with common password-rotation compliance requirements.",
            Tags = ["windows-hello", "pin", "expiry", "rotation", "policy", "compliance"],
            RegistryKeys = [PinKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Hello PIN expires after 90 days; periodic rotation enforced.",
            ApplyOps = [RegOp.SetDword(PinKey, "PINExpirationPeriod", 90)],
            RemoveOps = [RegOp.DeleteValue(PinKey, "PINExpirationPeriod")],
            DetectOps = [RegOp.CheckDword(PinKey, "PINExpirationPeriod", 90)],
        },
        new TweakDef
        {
            Id = "whi-set-pin-history-5",
            Label = "Enforce Windows Hello PIN History (5 Previous)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents reuse of the last 5 Windows Hello PINs. Combined with expiry, prevents users from cycling through a small set of known PINs.",
            Tags = ["windows-hello", "pin", "history", "reuse", "policy", "compliance"],
            RegistryKeys = [PinKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Last 5 Hello PINs remembered; immediate re-use blocked.",
            ApplyOps = [RegOp.SetDword(PinKey, "PINHistory", 5)],
            RemoveOps = [RegOp.DeleteValue(PinKey, "PINHistory")],
            DetectOps = [RegOp.CheckDword(PinKey, "PINHistory", 5)],
        },
    ];
}

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 639 — PolicyEntraId (Microsoft Entra ID / Azure AD device policy)
// ─────────────────────────────────────────────────────────────────────────────
internal static class PolicyEntraId
{
    private const string JoinKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin";
    private const string MsaKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftAccount";
    private const string CloudKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";
    private const string SysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "entra-block-workplace-join",
            Label = "Block Entra ID Workplace Join",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks Workplace Join (Entra ID / Azure AD device registration) via Group Policy. Prevents personal devices from registering with the organisation's Azure AD tenant without explicit IT approval.",
            Tags = ["entra", "azure-ad", "workplace-join", "device-registration", "policy"],
            RegistryKeys = [JoinKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Workplace Join (device registration) blocked; IT controls device enrollment.",
            ApplyOps = [RegOp.SetDword(JoinKey, "BlockAADWorkplaceJoin", 1)],
            RemoveOps = [RegOp.DeleteValue(JoinKey, "BlockAADWorkplaceJoin")],
            DetectOps = [RegOp.CheckDword(JoinKey, "BlockAADWorkplaceJoin", 1)],
        },
        new TweakDef
        {
            Id = "entra-disable-auto-workplace-join",
            Label = "Disable Automatic Entra ID Workplace Join",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic Workplace Join for domain-joined hybrid devices. Prevents the OS from silently registering the device with Azure AD during domain join or user sign-in.",
            Tags = ["entra", "azure-ad", "workplace-join", "auto-join", "policy"],
            RegistryKeys = [JoinKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Auto Workplace Join disabled; manual enrollment required.",
            ApplyOps = [RegOp.SetDword(JoinKey, "autoWorkplaceJoin", 0)],
            RemoveOps = [RegOp.DeleteValue(JoinKey, "autoWorkplaceJoin")],
            DetectOps = [RegOp.CheckDword(JoinKey, "autoWorkplaceJoin", 0)],
        },
        new TweakDef
        {
            Id = "entra-block-microsoft-accounts",
            Label = "Block Microsoft Consumer Accounts",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks Microsoft consumer account (MSA) sign-in and authentication via Group Policy. Forces all account activity through managed work/school accounts only — no personal microsoft.com accounts.",
            Tags = ["entra", "microsoft-account", "msa", "block", "policy", "corporate"],
            RegistryKeys = [MsaKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Personal Microsoft accounts blocked; only work/school accounts allowed.",
            ApplyOps = [RegOp.SetDword(MsaKey, "DisableUserAuth", 1)],
            RemoveOps = [RegOp.DeleteValue(MsaKey, "DisableUserAuth")],
            DetectOps = [RegOp.CheckDword(MsaKey, "DisableUserAuth", 1)],
        },
        new TweakDef
        {
            Id = "entra-disable-cloud-consumer-experience",
            Label = "Disable Microsoft Consumer Cloud Experiences",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks consumer-oriented Microsoft cloud experiences (OneDrive ads, Cortana suggestions, cross-device sync prompts) via Group Policy. Ensures managed devices focus on enterprise services only.",
            Tags = ["entra", "cloud", "consumer", "experience", "policy", "corporate"],
            RegistryKeys = [CloudKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Consumer cloud experiences and cross-device prompts disabled on managed devices.",
            ApplyOps = [RegOp.SetDword(CloudKey, "DisableConsumerAccountStateContent", 1)],
            RemoveOps = [RegOp.DeleteValue(CloudKey, "DisableConsumerAccountStateContent")],
            DetectOps = [RegOp.CheckDword(CloudKey, "DisableConsumerAccountStateContent", 1)],
        },
        new TweakDef
        {
            Id = "entra-disable-device-enrollment-user",
            Label = "Prevent User-Initiated MDM Enrollment",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents end users from initiating MDM (Intune/Azure AD) device enrollment from the Settings app. Enrollment must be performed by IT through approved provisioning flows.",
            Tags = ["entra", "mdm", "enrollment", "intune", "policy", "corporate"],
            RegistryKeys = [SysKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "User-initiated MDM enrollment blocked; IT-managed provisioning only.",
            ApplyOps = [RegOp.SetDword(SysKey, "DisableMDMEnrollment", 1)],
            RemoveOps = [RegOp.DeleteValue(SysKey, "DisableMDMEnrollment")],
            DetectOps = [RegOp.CheckDword(SysKey, "DisableMDMEnrollment", 1)],
        },
        new TweakDef
        {
            Id = "entra-disable-adding-work-accounts",
            Label = "Block Adding Work or School Accounts",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks users from adding additional work or school accounts via Settings > Accounts > Access work or school. Ensures devices have exactly one managed identity, preventing shadow account issues.",
            Tags = ["entra", "work-account", "school-account", "policy", "corporate", "identity"],
            RegistryKeys = [SysKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Adding secondary work/school accounts blocked from user Settings.",
            ApplyOps = [RegOp.SetDword(SysKey, "AllowWorkplaceJoinViaSetup", 0)],
            RemoveOps = [RegOp.DeleteValue(SysKey, "AllowWorkplaceJoinViaSetup")],
            DetectOps = [RegOp.CheckDword(SysKey, "AllowWorkplaceJoinViaSetup", 0)],
        },
        new TweakDef
        {
            Id = "entra-disable-find-my-device",
            Label = "Disable Find My Device (Entra Policy)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Find My Device feature on Entra-joined corporate devices via Group Policy. Prevents the device location from being reported to Microsoft / the user's Microsoft account, appropriate for managed shared devices.",
            Tags = ["entra", "find-my-device", "location", "policy", "privacy", "corporate"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FindMyDevice"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Find My Device disabled; device location not reported to Microsoft.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FindMyDevice", "FindMyDeviceEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FindMyDevice", "FindMyDeviceEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\FindMyDevice", "FindMyDeviceEnabled", 0)],
        },
        new TweakDef
        {
            Id = "entra-prevent-privacy-settings-prompt",
            Label = "Suppress Privacy Settings Prompt at Entra Sign-In",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the privacy settings diagnostic prompt that appears for new users at first sign-in on Entra-joined devices. IT policies govern data settings; users should not be prompted to override them.",
            Tags = ["entra", "privacy", "oobe", "policy", "first-run", "prompt"],
            RegistryKeys = [SysKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Privacy settings prompt suppressed at first logon; corporate policy governs settings.",
            ApplyOps = [RegOp.SetDword(SysKey, "PreventUserPrivacySettingPrompt", 1)],
            RemoveOps = [RegOp.DeleteValue(SysKey, "PreventUserPrivacySettingPrompt")],
            DetectOps = [RegOp.CheckDword(SysKey, "PreventUserPrivacySettingPrompt", 1)],
        },
        new TweakDef
        {
            Id = "entra-block-setup-next-steps",
            Label = "Block Entra Sign-In Setup Next-Steps Prompt",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks the 'What do you want to do next?' setup prompt shown after users complete the initial Entra ID sign-in flow. Streamlines the managed first-logon experience in corporate environments.",
            Tags = ["entra", "setup", "oobe", "first-run", "policy", "corporate"],
            RegistryKeys = [SysKey],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Post-sign-in setup prompt suppressed; users go directly to desktop.",
            ApplyOps = [RegOp.SetDword(SysKey, "BlockNextSteps", 1)],
            RemoveOps = [RegOp.DeleteValue(SysKey, "BlockNextSteps")],
            DetectOps = [RegOp.CheckDword(SysKey, "BlockNextSteps", 1)],
        },
        new TweakDef
        {
            Id = "entra-block-phone-link",
            Label = "Block Phone Link App on Entra-Managed Devices",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Phone Link (Your Phone) application on Entra-joined managed devices via Group Policy. Prevents personal mobile device data — messages, notifications, photos, calls — from being accessible on corporate PCs.",
            Tags = ["entra", "phone-link", "your-phone", "policy", "data-protection", "corporate"],
            RegistryKeys = [SysKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Phone Link blocked; personal phone data cannot be accessed from corporate PC.",
            ApplyOps = [RegOp.SetDword(SysKey, "AllowPhoneLink", 0)],
            RemoveOps = [RegOp.DeleteValue(SysKey, "AllowPhoneLink")],
            DetectOps = [RegOp.CheckDword(SysKey, "AllowPhoneLink", 0)],
        },
    ];
}

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 640 — PolicyKerberos (Kerberos authentication hardening policy)
// ─────────────────────────────────────────────────────────────────────────────
internal static class PolicyKerberos
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Kerberos\Parameters";
    private const string LsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "kerberos-disable-des-encryption",
            Label = "Disable DES Kerberos Encryption",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Removes DES (des-cbc-crc and des-cbc-md5) from the Kerberos supported encryption types. DES is cryptographically broken; this forces AES-256/AES-128 and RC4 only.",
            Tags = ["kerberos", "des", "encryption", "policy", "authentication", "hardening"],
            RegistryKeys = [Key],
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "DES Kerberos encryption removed; AES-256/AES-128 and RC4 are used instead.",
            ApplyOps = [RegOp.SetDword(Key, "SupportedEncryptionTypes", 0x7FFFFFFC)],
            RemoveOps = [RegOp.DeleteValue(Key, "SupportedEncryptionTypes")],
            DetectOps = [RegOp.CheckDword(Key, "SupportedEncryptionTypes", 0x7FFFFFFC)],
        },
        new TweakDef
        {
            Id = "kerberos-require-aes256-only",
            Label = "Require AES-256 Kerberos Encryption Only",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Restricts Kerberos to AES-256-CTS-HMAC-SHA1-96 only (0x10 = AES256). The strongest standard Kerberos cipher. Incompatible with legacy systems that only support RC4 or DES.",
            Tags = ["kerberos", "aes256", "encryption", "policy", "authentication", "hardening"],
            RegistryKeys = [Key],
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "Only AES-256 Kerberos allowed; legacy RC4/DES systems will fail authentication.",
            ApplyOps = [RegOp.SetDword(Key, "SupportedEncryptionTypes", 0x10)],
            RemoveOps = [RegOp.DeleteValue(Key, "SupportedEncryptionTypes")],
            DetectOps = [RegOp.CheckDword(Key, "SupportedEncryptionTypes", 0x10)],
        },
        new TweakDef
        {
            Id = "kerberos-enable-aes-and-rc4",
            Label = "Allow AES-256 + AES-128 + RC4 Kerberos",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures Kerberos to support AES-256, AES-128, and RC4-HMAC while explicitly excluding DES. Balanced security for environments with mixed legacy and modern systems.",
            Tags = ["kerberos", "aes", "rc4", "encryption", "policy", "authentication"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "AES-256+AES-128+RC4 allowed; DES excluded. Suitable for most mixed environments.",
            ApplyOps = [RegOp.SetDword(Key, "SupportedEncryptionTypes", 0x7C)],
            RemoveOps = [RegOp.DeleteValue(Key, "SupportedEncryptionTypes")],
            DetectOps = [RegOp.CheckDword(Key, "SupportedEncryptionTypes", 0x7C)],
        },
        new TweakDef
        {
            Id = "kerberos-enable-claims",
            Label = "Enable Kerberos Claims Authentication",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Kerberos claims-based access control (Dynamic Access Control). Allows Kerberos tickets to carry user and device claim attributes used for resource-based authorisation decisions.",
            Tags = ["kerberos", "claims", "dac", "dynamic-access-control", "policy", "authentication"],
            RegistryKeys = [LsaKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Kerberos tickets carry claims; Dynamic Access Control policies can be applied.",
            ApplyOps = [RegOp.SetDword(LsaKey, "EnableCbacAndArmor", 1)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "EnableCbacAndArmor")],
            DetectOps = [RegOp.CheckDword(LsaKey, "EnableCbacAndArmor", 1)],
        },
        new TweakDef
        {
            Id = "kerberos-enable-resource-sid-compression",
            Label = "Enable Kerberos Resource SID Compression",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Kerberos SID compression for resource group SIDs in service tickets. Reduces Kerberos ticket size when users belong to many groups, preventing ticket-too-large (KDC_ERR_RESPONSE_TOO_BIG) failures.",
            Tags = ["kerberos", "sid", "compression", "policy", "authentication", "performance"],
            RegistryKeys = [LsaKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "SID compression enabled; Kerberos tickets smaller for users with many group memberships.",
            ApplyOps = [RegOp.SetDword(LsaKey, "KdcUseRequestedEtypesForTickets", 1)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "KdcUseRequestedEtypesForTickets")],
            DetectOps = [RegOp.CheckDword(LsaKey, "KdcUseRequestedEtypesForTickets", 1)],
        },
        new TweakDef
        {
            Id = "kerberos-disable-rc4",
            Label = "Disable RC4-HMAC Kerberos Encryption",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Removes RC4-HMAC from the Kerberos supported encryption list. RC4 is considered weak (NIST deprecated 2015). Only AES-256 and AES-128 are retained. May break legacy NAS or non-Microsoft Kerberos implementations.",
            Tags = ["kerberos", "rc4", "encryption", "policy", "hardening", "nist"],
            RegistryKeys = [Key],
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "RC4 Kerberos removed; AES-only. May break legacy Samba/NFS/MIT Kerberos systems.",
            ApplyOps = [RegOp.SetDword(Key, "SupportedEncryptionTypes", 0x18)],
            RemoveOps = [RegOp.DeleteValue(Key, "SupportedEncryptionTypes")],
            DetectOps = [RegOp.CheckDword(Key, "SupportedEncryptionTypes", 0x18)],
        },
        new TweakDef
        {
            Id = "kerberos-set-max-ticket-age-10h",
            Label = "Set Kerberos Max Ticket Age to 10 Hours",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the maximum Kerberos TGT lifetime to 10 hours. Short ticket lifetimes limit the window of opportunity for a stolen ticket to be used in pass-the-ticket attacks.",
            Tags = ["kerberos", "ticket", "age", "lifetime", "policy", "pass-the-ticket"],
            RegistryKeys = [LsaKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Kerberos TGT lifetime 10 h; stolen tickets expire faster.",
            ApplyOps = [RegOp.SetDword(LsaKey, "MaxTicketAge", 10)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "MaxTicketAge")],
            DetectOps = [RegOp.CheckDword(LsaKey, "MaxTicketAge", 10)],
        },
        new TweakDef
        {
            Id = "kerberos-set-max-renew-age-7d",
            Label = "Set Kerberos Ticket Renewal Limit to 7 Days",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the maximum Kerberos TGT renewal period to 7 days. After 7 days, users must re-authenticate fully. Aligns with weekly credential attestation requirements.",
            Tags = ["kerberos", "ticket", "renewal", "policy", "authentication"],
            RegistryKeys = [LsaKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "TGT cannot be renewed after 7 days; full re-authentication required weekly.",
            ApplyOps = [RegOp.SetDword(LsaKey, "MaxRenewAge", 7)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "MaxRenewAge")],
            DetectOps = [RegOp.CheckDword(LsaKey, "MaxRenewAge", 7)],
        },
        new TweakDef
        {
            Id = "kerberos-set-clock-skew-3min",
            Label = "Set Kerberos Max Clock Skew to 3 Minutes",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Reduces Kerberos tolerance for clock drift to 3 minutes (from the default 5). Tighter clock synchronisation reduces the window for replay attacks that exploit clock skew.",
            Tags = ["kerberos", "clock-skew", "replay-attack", "policy", "ntp", "hardening"],
            RegistryKeys = [LsaKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "3-minute clock skew tolerance; tighter replay-attack window. Requires reliable NTP.",
            ApplyOps = [RegOp.SetDword(LsaKey, "SkewTime", 3)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "SkewTime")],
            DetectOps = [RegOp.CheckDword(LsaKey, "SkewTime", 3)],
        },
        new TweakDef
        {
            Id = "kerberos-enable-kdc-proxy",
            Label = "Enable Kerberos KDC Proxy",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables the Kerberos KDC Proxy (KKDCP) for clients that cannot reach the domain controller directly. Authentication is tunnelled over HTTPS to the proxy, supporting remote workers without VPN.",
            Tags = ["kerberos", "kdc-proxy", "kkdcp", "remote", "policy", "vpn-alternative"],
            RegistryKeys = [LsaKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "KDC proxy enabled; Kerberos auth works over HTTPS for remoting without full VPN.",
            ApplyOps = [RegOp.SetDword(LsaKey, "UseKdcProxy", 1)],
            RemoveOps = [RegOp.DeleteValue(LsaKey, "UseKdcProxy")],
            DetectOps = [RegOp.CheckDword(LsaKey, "UseKdcProxy", 1)],
        },
    ];
}

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 641 — PolicyAppInstaller (App Installer / WinGet MSIX policy)
// ─────────────────────────────────────────────────────────────────────────────
internal static class PolicyAppInstaller
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "appinst-disable-app-installer",
            Label = "Disable App Installer (WinGet / MSIX)",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows App Installer (WinGet) entirely via Group Policy. Prevents sideloading of MSIX packages and use of the appinstaller:// URI scheme. Enforces managed software distribution only.",
            Tags = ["app-installer", "winget", "msix", "sideloading", "policy", "security"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "App Installer disabled; no MSIX sideloading or winget use permitted.",
            ApplyOps = [RegOp.SetDword(Key, "EnableAppInstaller", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableAppInstaller")],
            DetectOps = [RegOp.CheckDword(Key, "EnableAppInstaller", 0)],
        },
        new TweakDef
        {
            Id = "appinst-disable-local-manifest",
            Label = "Disable App Installer Local Manifest Files",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks the use of local YAML manifest files with App Installer / WinGet. Prevents `winget install --manifest local.yaml` for arbitrary package installation from local files.",
            Tags = ["app-installer", "winget", "manifest", "local", "policy", "sideloading"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Local manifest install (`--manifest`) blocked; approved sources only.",
            ApplyOps = [RegOp.SetDword(Key, "EnableLocalManifestFiles", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableLocalManifestFiles")],
            DetectOps = [RegOp.CheckDword(Key, "EnableLocalManifestFiles", 0)],
        },
        new TweakDef
        {
            Id = "appinst-disable-hash-override",
            Label = "Disable App Installer Hash Override",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the `--ignore-security-hash` flag from bypassing App Installer's SHA-256 checksum validation. Ensures all installed packages match their verified hash — no tampered packages.",
            Tags = ["app-installer", "winget", "hash", "integrity", "policy", "security"],
            RegistryKeys = [Key],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Hash verification cannot be bypassed; tampered packages will fail to install.",
            ApplyOps = [RegOp.SetDword(Key, "EnableHashOverride", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableHashOverride")],
            DetectOps = [RegOp.CheckDword(Key, "EnableHashOverride", 0)],
        },
        new TweakDef
        {
            Id = "appinst-disable-msappinstaller-protocol",
            Label = "Disable ms-appinstaller:// URI Protocol",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the ms-appinstaller:// URI scheme. This protocol has been exploited to deliver malware — legitimate hosting via Teams/Edge link redirects to MSIX packages. Blocking it prevents click-to-install phishing attacks.",
            Tags = ["app-installer", "msix", "protocol", "uri", "phishing", "malware", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "ms-appinstaller:// URI blocked; click-to-MSIX delivery attack vector removed.",
            ApplyOps = [RegOp.SetDword(Key, "EnableMSAppInstallerProtocol", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableMSAppInstallerProtocol")],
            DetectOps = [RegOp.CheckDword(Key, "EnableMSAppInstallerProtocol", 0)],
        },
        new TweakDef
        {
            Id = "appinst-disable-experimental-features",
            Label = "Disable App Installer Experimental Features",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents users from enabling experimental / preview features in WinGet via Group Policy. Experimental features are untested and may contain undisclosed bugs or attack surface.",
            Tags = ["app-installer", "winget", "experimental", "policy", "security"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "WinGet experimental features blocked; only stable functionality permitted.",
            ApplyOps = [RegOp.SetDword(Key, "EnableExperimentalFeatures", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableExperimentalFeatures")],
            DetectOps = [RegOp.CheckDword(Key, "EnableExperimentalFeatures", 0)],
        },
        new TweakDef
        {
            Id = "appinst-disable-settings-page",
            Label = "Disable App Installer Settings Page",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Hides the settings page within App Installer / WinGet, preventing users from changing configuration. Locks down the managed state of the installer on corporate devices.",
            Tags = ["app-installer", "winget", "settings", "lock", "policy", "corporate"],
            RegistryKeys = [Key],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "App Installer settings page hidden; IT manages configuration.",
            ApplyOps = [RegOp.SetDword(Key, "EnableSettings", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSettings")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSettings", 0)],
        },
        new TweakDef
        {
            Id = "appinst-disable-store-source",
            Label = "Block Microsoft Store App Installer Source",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Removes the Microsoft Store as a WinGet package source via Group Policy. Forces all package installations to use internal corporate repositories only, preventing unapproved Store app installations.",
            Tags = ["app-installer", "winget", "microsoft-store", "source", "policy", "corporate"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Microsoft Store source removed from WinGet; only corporate repos allowed.",
            ApplyOps = [RegOp.SetDword(Key, "EnableMicrosoftStoreSource", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableMicrosoftStoreSource")],
            DetectOps = [RegOp.CheckDword(Key, "EnableMicrosoftStoreSource", 0)],
        },
        new TweakDef
        {
            Id = "appinst-disable-local-archive-install",
            Label = "Block App Installer Local Archive Installation",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks WinGet from installing applications from local archive files (ZIP, tar.gz). Prevents circumventing approved sources by extracting and installing from locally downloaded archives.",
            Tags = ["app-installer", "winget", "archive", "zip", "policy", "sideloading"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Local archive installs blocked; ZIP/tar package sideloading prevented.",
            ApplyOps = [RegOp.SetDword(Key, "EnableLocalArchiveInstall", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableLocalArchiveInstall")],
            DetectOps = [RegOp.CheckDword(Key, "EnableLocalArchiveInstall", 0)],
        },
        new TweakDef
        {
            Id = "appinst-set-source-update-interval-1h",
            Label = "Set App Installer Source Update Interval to 1 Hour",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the WinGet package source auto-update interval to 1 hour. More frequent source refreshes ensure the package catalogue is always current, reducing the window where a compromised package version could be served.",
            Tags = ["app-installer", "winget", "source", "update", "interval", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Source index refreshed hourly; stale package catalogue window minimised.",
            ApplyOps = [RegOp.SetDword(Key, "SourceAutoUpdateInterval", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SourceAutoUpdateInterval")],
            DetectOps = [RegOp.CheckDword(Key, "SourceAutoUpdateInterval", 1)],
        },
        new TweakDef
        {
            Id = "appinst-enable-bypass-cert-pinning",
            Label = "Enable Certificate Pin Bypass for Microsoft Store",
            Category = "Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Allows App Installer to bypass certificate pinning when connecting to the Microsoft Store source. Required in corporate environments that use TLS-intercepting proxies; without this, MSIX downloads may fail.",
            Tags = ["app-installer", "winget", "certificate", "pinning", "proxy", "corporate", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 2,
            SafetyRating = 3,
            ImpactNote = "Certificate pinning bypassed for Store; needed behind TLS-inspection proxies.",
            ApplyOps = [RegOp.SetDword(Key, "EnableBypassCertificatePinningForMicrosoftStore", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableBypassCertificatePinningForMicrosoftStore")],
            DetectOps = [RegOp.CheckDword(Key, "EnableBypassCertificatePinningForMicrosoftStore", 1)],
        },
    ];
}

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
