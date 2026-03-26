// RegiLattice.Core — Tweaks/PersonalDataEncryptionPolicy.cs
// Personal Data Encryption (PDE) Group Policy controls — Sprint 367.
// Category: "Personal Data Encryption Policy" | Slug: pde
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\PDE
// MinBuild: 22621 (Windows 11 22H2+)
namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PersonalDataEncryptionPolicy
{
    private const string PdeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PDE";
    private const string PdeFoldersKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PDE\ProtectedFolders";
    private const string PdeDeviceKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PDE\Device";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "pde-enable-personal-data-encryption",
            Label = "Enable Personal Data Encryption",
            Category = "Personal Data Encryption Policy",
            Description = "Enables Personal Data Encryption (PDE) on the device, protecting user files in selected folders with keys tied to the signed-in user identity. Requires Windows Hello for Business.",
            Tags = ["pde", "encryption", "personal-data", "security", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Files in protected folders become inaccessible until the user authenticates via Windows Hello; improves data security at rest.",
            RegistryKeys = [PdeKey],
            ApplyOps  = [RegOp.SetDword(PdeKey, "EnablePersonalDataEncryption", 1)],
            RemoveOps = [RegOp.DeleteValue(PdeKey, "EnablePersonalDataEncryption")],
            DetectOps = [RegOp.CheckDword(PdeKey, "EnablePersonalDataEncryption", 1)],
        },
        new TweakDef
        {
            Id = "pde-require-device-encryption-prereq",
            Label = "Require BitLocker as PDE Prerequisite",
            Category = "Personal Data Encryption Policy",
            Description = "Requires BitLocker drive encryption to be active before Personal Data Encryption can be applied to user folders. Ensures defense-in-depth for protected content.",
            Tags = ["pde", "encryption", "bitlocker", "prerequisite", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Enforces layered encryption: BitLocker protects the drive, PDE protects individual user files.",
            RegistryKeys = [PdeKey],
            ApplyOps  = [RegOp.SetDword(PdeKey, "RequireDeviceEncryption", 1)],
            RemoveOps = [RegOp.DeleteValue(PdeKey, "RequireDeviceEncryption")],
            DetectOps = [RegOp.CheckDword(PdeKey, "RequireDeviceEncryption", 1)],
        },
        new TweakDef
        {
            Id = "pde-block-network-content-access",
            Label = "Block PDE Content Access from Network Accounts",
            Category = "Personal Data Encryption Policy",
            Description = "Prevents network service accounts and remote processes from accessing folders protected by Personal Data Encryption, limiting access to the locally signed-in user.",
            Tags = ["pde", "encryption", "network", "access-control", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Stops backup agents or network-based tools from reading PDE-protected files when the owning user is not signed in.",
            RegistryKeys = [PdeKey],
            ApplyOps  = [RegOp.SetDword(PdeKey, "BlockNetworkAccessToPDEContent", 1)],
            RemoveOps = [RegOp.DeleteValue(PdeKey, "BlockNetworkAccessToPDEContent")],
            DetectOps = [RegOp.CheckDword(PdeKey, "BlockNetworkAccessToPDEContent", 1)],
        },
        new TweakDef
        {
            Id = "pde-wipe-keys-on-lock",
            Label = "Wipe PDE Keys on Device Lock",
            Category = "Personal Data Encryption Policy",
            Description = "Instructs Windows to purge in-memory Personal Data Encryption keys when the device screen locks. Files remain encrypted and inaccessible until the user unlocks with Windows Hello.",
            Tags = ["pde", "encryption", "lock-screen", "key-management", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Reduces the window during which PDE keys are resident in memory after the device is left unattended.",
            RegistryKeys = [PdeKey],
            ApplyOps  = [RegOp.SetDword(PdeKey, "WipeKeysOnLock", 1)],
            RemoveOps = [RegOp.DeleteValue(PdeKey, "WipeKeysOnLock")],
            DetectOps = [RegOp.CheckDword(PdeKey, "WipeKeysOnLock", 1)],
        },
        new TweakDef
        {
            Id = "pde-protect-desktop-folder",
            Label = "Enable PDE Protection for Desktop Folder",
            Category = "Personal Data Encryption Policy",
            Description = "Applies Personal Data Encryption to the user's Desktop folder, ensuring files placed on the desktop are encrypted with the user's Windows Hello identity key.",
            Tags = ["pde", "encryption", "desktop", "folder-protection", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Desktop files are frequently targeted; encrypting the Desktop folder prevents offline access by attackers with physical access.",
            RegistryKeys = [PdeFoldersKey],
            ApplyOps  = [RegOp.SetDword(PdeFoldersKey, "ProtectDesktopFolder", 1)],
            RemoveOps = [RegOp.DeleteValue(PdeFoldersKey, "ProtectDesktopFolder")],
            DetectOps = [RegOp.CheckDword(PdeFoldersKey, "ProtectDesktopFolder", 1)],
        },
        new TweakDef
        {
            Id = "pde-protect-documents-folder",
            Label = "Enable PDE Protection for Documents Folder",
            Category = "Personal Data Encryption Policy",
            Description = "Applies Personal Data Encryption to the user's Documents folder. Files are encrypted with user identity keys tied to Windows Hello, preventing offline access without user authentication.",
            Tags = ["pde", "encryption", "documents", "folder-protection", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Documents folder often contains sensitive business data; PDE protection prevents access when the device is lost or stolen.",
            RegistryKeys = [PdeFoldersKey],
            ApplyOps  = [RegOp.SetDword(PdeFoldersKey, "ProtectDocumentsFolder", 1)],
            RemoveOps = [RegOp.DeleteValue(PdeFoldersKey, "ProtectDocumentsFolder")],
            DetectOps = [RegOp.CheckDword(PdeFoldersKey, "ProtectDocumentsFolder", 1)],
        },
        new TweakDef
        {
            Id = "pde-protect-pictures-folder",
            Label = "Enable PDE Protection for Pictures Folder",
            Category = "Personal Data Encryption Policy",
            Description = "Applies Personal Data Encryption to the user's Pictures folder, protecting images and media from offline access on lost or stolen devices.",
            Tags = ["pde", "encryption", "pictures", "folder-protection", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Protects personal images from exposure during device repairs or after physical theft.",
            RegistryKeys = [PdeFoldersKey],
            ApplyOps  = [RegOp.SetDword(PdeFoldersKey, "ProtectPicturesFolder", 1)],
            RemoveOps = [RegOp.DeleteValue(PdeFoldersKey, "ProtectPicturesFolder")],
            DetectOps = [RegOp.CheckDword(PdeFoldersKey, "ProtectPicturesFolder", 1)],
        },
        new TweakDef
        {
            Id = "pde-audit-access-events",
            Label = "Enable PDE Access Audit Events",
            Category = "Personal Data Encryption Policy",
            Description = "Enables Windows to generate audit events when PDE-protected content is accessed or when PDE encryption/decryption operations occur, supporting security monitoring and compliance.",
            Tags = ["pde", "encryption", "audit", "compliance", "event-log"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Provides visibility into who accesses PDE-protected files and when, aiding incident investigation.",
            RegistryKeys = [PdeKey],
            ApplyOps  = [RegOp.SetDword(PdeKey, "EnablePDEAuditEvents", 1)],
            RemoveOps = [RegOp.DeleteValue(PdeKey, "EnablePDEAuditEvents")],
            DetectOps = [RegOp.CheckDword(PdeKey, "EnablePDEAuditEvents", 1)],
        },
        new TweakDef
        {
            Id = "pde-restrict-key-backup",
            Label = "Restrict PDE Key Backup to Organization",
            Category = "Personal Data Encryption Policy",
            Description = "Limits Personal Data Encryption key backup to organization-controlled Microsoft Entra ID (Azure AD) accounts only, preventing personal Microsoft account key escrow.",
            Tags = ["pde", "encryption", "key-backup", "azure-ad", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ensures only the organization's IT department can facilitate key recovery, not personal Microsoft accounts.",
            RegistryKeys = [PdeDeviceKey],
            ApplyOps  = [RegOp.SetDword(PdeDeviceKey, "RestrictKeyBackupToOrganization", 1)],
            RemoveOps = [RegOp.DeleteValue(PdeDeviceKey, "RestrictKeyBackupToOrganization")],
            DetectOps = [RegOp.CheckDword(PdeDeviceKey, "RestrictKeyBackupToOrganization", 1)],
        },
        new TweakDef
        {
            Id = "pde-require-windows-hello-enrolment",
            Label = "Require Windows Hello Enrollment for PDE",
            Category = "Personal Data Encryption Policy",
            Description = "Enforces that users must be enrolled in Windows Hello for Business before Personal Data Encryption can be activated on their device. Prevents PDE deployment without modern authentication.",
            Tags = ["pde", "encryption", "windows-hello", "enrollment", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ensures PDE keys are always bound to strong authentication rather than password-only accounts.",
            RegistryKeys = [PdeDeviceKey],
            ApplyOps  = [RegOp.SetDword(PdeDeviceKey, "RequireWindowsHelloEnrollment", 1)],
            RemoveOps = [RegOp.DeleteValue(PdeDeviceKey, "RequireWindowsHelloEnrollment")],
            DetectOps = [RegOp.CheckDword(PdeDeviceKey, "RequireWindowsHelloEnrollment", 1)],
        },
    ];
}
