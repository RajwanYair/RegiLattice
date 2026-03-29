// RegiLattice.Core — Tweaks/MicrosoftStorePolicy.cs
// Microsoft Store UWP app installation, acquisition, and Store client policy — Sprint 515.
// Category: "Microsoft Store Policy" | Slug: storepol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\WindowsStore

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class MicrosoftStorePolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore";
    private const string AppKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx";
    private const string LicKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppLicense";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "storepol-disable-store-entirely",
            Label        = "Disable Microsoft Store App Entirely",
            Category     = "Microsoft Store Policy",
            Description  = "Completely blocks access to the Microsoft Store application for all users on the machine, preventing app browsing, purchasing, and installation via the Store UI.",
            Tags         = ["store", "disable", "uwp", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Microsoft Store completely disabled; users cannot browse, buy, or install apps from the Store UI.",
            ApplyOps     = [RegOp.SetDword(Key, "RemoveWindowsStore", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "RemoveWindowsStore")],
            DetectOps    = [RegOp.CheckDword(Key, "RemoveWindowsStore", 1)],
        },
        new TweakDef
        {
            Id           = "storepol-disable-auto-app-download",
            Label        = "Disable Automatic App Download and Installation via Store",
            Category     = "Microsoft Store Policy",
            Description  = "Prevents the Microsoft Store from automatically downloading and installing promoted applications in the background, stopping unsolicited app additions to the Start menu without user action.",
            Tags         = ["store", "auto-download", "uwp", "bloat", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Automatic Store app downloads disabled; no apps silently installed via Store promotion pipeline.",
            ApplyOps     = [RegOp.SetDword(Key, "AutoDownload", 2)],
            RemoveOps    = [RegOp.DeleteValue(Key, "AutoDownload")],
            DetectOps    = [RegOp.CheckDword(Key, "AutoDownload", 2)],
        },
        new TweakDef
        {
            Id           = "storepol-require-private-store-only",
            Label        = "Restrict Store to Private (Enterprise) Store Only",
            Category     = "Microsoft Store Policy",
            Description  = "Restricts the Microsoft Store to show only the organisation's Private Store (Microsoft Intune / Business Store), preventing employees from browsing or installing from the public consumer Store catalog.",
            Tags         = ["store", "private-store", "enterprise", "intune", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Only Private/Enterprise Store catalog shown; public consumer apps not accessible via Store.",
            ApplyOps     = [RegOp.SetDword(Key, "RequirePrivateStoreOnly", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "RequirePrivateStoreOnly")],
            DetectOps    = [RegOp.CheckDword(Key, "RequirePrivateStoreOnly", 1)],
        },
        new TweakDef
        {
            Id           = "storepol-disable-store-in-shelf",
            Label        = "Disable Microsoft Store Suggestions in Taskbar (Shelf)",
            Category     = "Microsoft Store Policy",
            Description  = "Prevents the Microsoft Store from displaying app suggestions and promotions in the Windows taskbar shelf and Start menu recommended section, reducing promotional clutter on managed corporate desktops.",
            Tags         = ["store", "shelf", "taskbar", "suggestions", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Store promotional suggestions in taskbar shelf and Start menu disabled; no app promotions displayed.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableStoreShelf", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableStoreShelf")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableStoreShelf", 1)],
        },
        new TweakDef
        {
            Id           = "storepol-disable-appx-sideloading",
            Label        = "Disable Unsigned Appx Package Sideloading",
            Category     = "Microsoft Store Policy",
            Description  = "Prevents installation of unsigned (developer) Appx/MSIX packages that are not from the Microsoft Store or properly signed by a trusted publisher, blocking potential malware distribution via sideloaded UWP packages.",
            Tags         = ["store", "sideloading", "appx", "unsigned", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Unsigned Appx sideloading disabled; only Store-published or enterprise-signed MSIX packages can install.",
            ApplyOps     = [RegOp.SetDword(AppKey, "AllowAllTrustedApps", 0)],
            RemoveOps    = [RegOp.DeleteValue(AppKey, "AllowAllTrustedApps")],
            DetectOps    = [RegOp.CheckDword(AppKey, "AllowAllTrustedApps", 0)],
        },
        new TweakDef
        {
            Id           = "storepol-disable-appx-developer-mode",
            Label        = "Disable Windows Developer Mode Package Installation",
            Category     = "Microsoft Store Policy",
            Description  = "Prevents enabling Windows Developer Mode which would allow unrestricted Appx sideloading without publisher signing requirements, ensuring UWP installation policy restrictions are not bypassed via Developer Mode.",
            Tags         = ["store", "developer-mode", "appx", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Developer Mode blocked via policy; sideloading restrictions cannot be bypassed by enabling Developer Mode.",
            ApplyOps     = [RegOp.SetDword(AppKey, "AllowDevelopmentWithoutDevLicense", 0)],
            RemoveOps    = [RegOp.DeleteValue(AppKey, "AllowDevelopmentWithoutDevLicense")],
            DetectOps    = [RegOp.CheckDword(AppKey, "AllowDevelopmentWithoutDevLicense", 0)],
        },
        new TweakDef
        {
            Id           = "storepol-disable-app-license-acquisition",
            Label        = "Disable Automatic App License Acquisition from Store",
            Category     = "Microsoft Store Policy",
            Description  = "Prevents applications from automatically acquiring new or updated licenses from the Microsoft Store License Service in the background, ensuring license state changes are predictable and do not occur without admin approval.",
            Tags         = ["store", "license", "auto-acquisition", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Automatic app license acquisition disabled; Store license updates require manual trigger or admin action.",
            ApplyOps     = [RegOp.SetDword(LicKey, "DisableAutoLicenseAcquisition", 1)],
            RemoveOps    = [RegOp.DeleteValue(LicKey, "DisableAutoLicenseAcquisition")],
            DetectOps    = [RegOp.CheckDword(LicKey, "DisableAutoLicenseAcquisition", 1)],
        },
        new TweakDef
        {
            Id           = "storepol-disable-store-update-background",
            Label        = "Disable Background App Update via Microsoft Store",
            Category     = "Microsoft Store Policy",
            Description  = "Prevents installed UWP apps from automatically updating in the background via the Store update service, ensuring app version changes go through controlled deployment channels.",
            Tags         = ["store", "auto-update", "background", "uwp", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Store background app updates disabled; UWP apps only updated on explicit user or admin trigger.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableOSUpgrade", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableOSUpgrade")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableOSUpgrade", 0)],
        },
        new TweakDef
        {
            Id           = "storepol-disable-store-telemetry",
            Label        = "Disable Microsoft Store Telemetry to Microsoft",
            Category     = "Microsoft Store Policy",
            Description  = "Prevents the Microsoft Store client from sending browsing history, search queries, purchase activity, and app installation statistics to Microsoft.",
            Tags         = ["store", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Store telemetry to Microsoft disabled; browsing, search, and install data not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableStoreTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableStoreTelemetry")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableStoreTelemetry", 1)],
        },
        new TweakDef
        {
            Id           = "storepol-log-appx-install-events",
            Label        = "Log Appx Package Installation Events in Security Log",
            Category     = "Microsoft Store Policy",
            Description  = "Enables Security event log entries for every Appx/MSIX package installation, update, and removal event, providing a complete audit trail of UWP app deployments on the endpoint.",
            Tags         = ["store", "appx", "audit", "event-log", "install", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Appx package install/update/remove events logged in Security log; full UWP deployment audit trail.",
            ApplyOps     = [RegOp.SetDword(AppKey, "AuditAppxInstallEvents", 1)],
            RemoveOps    = [RegOp.DeleteValue(AppKey, "AuditAppxInstallEvents")],
            DetectOps    = [RegOp.CheckDword(AppKey, "AuditAppxInstallEvents", 1)],
        },
    ];
}
