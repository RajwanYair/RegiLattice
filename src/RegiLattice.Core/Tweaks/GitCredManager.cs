#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

[TweakModule]
internal static class GitCredManager
{
    private const string GcmKey = @"HKEY_CURRENT_USER\Software\Microsoft\GCM";
    private const string GitKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\GitForWindows";
    private const string GitUserKey = @"HKEY_CURRENT_USER\Software\GitForWindows";
    private const string CredStoreKey = @"HKEY_CURRENT_USER\Software\Microsoft\GCM\CredentialStore";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "gitcm-set-windows-credential-store",
            Label = "Git Credential Manager: Use Windows Credential Store",
            Category = "Dev Drive / Developer",
            Tags = ["git", "credential-manager", "windows", "developer", "security"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Credentials stored in Windows Credential Manager are encrypted with the user's key.",
            Description =
                "Sets CredentialStore=wincredman in the Git Credential Manager registry. "
                + "Configures GCM to use the Windows Credential Manager (Windows Credential "
                + "Locker) as the backing credential store. Credentials are encrypted per-user "
                + "and backed up with Windows. Default on Windows. Explicit enforcement.",
            RegistryKeys = [GcmKey],
            ApplyOps = [RegOp.SetString(GcmKey, "CredentialStore", "wincredman")],
            RemoveOps = [RegOp.DeleteValue(GcmKey, "CredentialStore")],
            DetectOps = [RegOp.CheckString(GcmKey, "CredentialStore", "wincredman")],
        },
        new TweakDef
        {
            Id = "gitcm-set-secretservice-credential-store",
            Label = "Git Credential Manager: Use DPAPI Secret Service Store (WSL-Compatible)",
            Category = "Dev Drive / Developer",
            Tags = ["git", "credential-manager", "dpapi", "wsl", "developer"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "DPAPI store works correctly from both Windows and WSL environments.",
            Description =
                "Sets CredentialStore=dpapi in the GCM registry. Uses the DPAPI-encrypted "
                + "file-based credential store. Credentials are stored as DPAPI-encrypted "
                + "files and are accessible from WSL via the GCM helper. "
                + "Recommended when using Git from both Windows and WSL 2.",
            RegistryKeys = [GcmKey],
            ApplyOps = [RegOp.SetString(GcmKey, "CredentialStore", "dpapi")],
            RemoveOps = [RegOp.DeleteValue(GcmKey, "CredentialStore")],
            DetectOps = [RegOp.CheckString(GcmKey, "CredentialStore", "dpapi")],
        },
        new TweakDef
        {
            Id = "gitcm-set-oauth-default-auth",
            Label = "Git Credential Manager: Use OAuth as Default Authentication",
            Category = "Dev Drive / Developer",
            Tags = ["git", "credential-manager", "oauth", "developer", "authentication"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "OAuth tokens are short-lived and scoped — more secure than password-based auth.",
            Description =
                "Sets authority=oauth in the GCM registry. Configures Git Credential Manager "
                + "to use OAuth device flow for authentication against GitHub, Azure DevOps "
                + "and Bitbucket. Avoids storing long-lived passwords.",
            RegistryKeys = [GcmKey],
            ApplyOps = [RegOp.SetString(GcmKey, "authority", "oauth")],
            RemoveOps = [RegOp.DeleteValue(GcmKey, "authority")],
            DetectOps = [RegOp.CheckString(GcmKey, "authority", "oauth")],
        },
        new TweakDef
        {
            Id = "gitcm-enable-always-prompt-credentials",
            Label = "Git Credential Manager: Always Prompt (No Cached Credentials)",
            Category = "Dev Drive / Developer",
            Tags = ["git", "credential-manager", "prompt", "developer", "security"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Forces fresh authentication on every Git network operation — maximises security.",
            Description =
                "Sets alwaysPrompt=true in the GCM registry. Instructs Git Credential Manager "
                + "to always interactively prompt for credentials rather than returning cached "
                + "tokens. Useful on shared machines or when credential refresh is required.",
            RegistryKeys = [GcmKey],
            ApplyOps = [RegOp.SetString(GcmKey, "alwaysPrompt", "true")],
            RemoveOps = [RegOp.DeleteValue(GcmKey, "alwaysPrompt")],
            DetectOps = [RegOp.CheckString(GcmKey, "alwaysPrompt", "true")],
        },
        new TweakDef
        {
            Id = "gitcm-enable-gui-prompts",
            Label = "Git Credential Manager: Use GUI Authentication Dialog",
            Category = "Dev Drive / Developer",
            Tags = ["git", "credential-manager", "gui", "dialog", "developer"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "GUI dialogs support SSO browsers and device code flow more easily than console prompts.",
            Description =
                "Sets guiPrompt=true in the GCM registry. Instructs GCM to use the Windows "
                + "GUI authentication dialogs (browser-based OAuth flow, Windows SSO) rather "
                + "than text-based console prompts. Required for Azure Entra ID / SAML SSO flows.",
            RegistryKeys = [GcmKey],
            ApplyOps = [RegOp.SetString(GcmKey, "guiPrompt", "true")],
            RemoveOps = [RegOp.DeleteValue(GcmKey, "guiPrompt")],
            DetectOps = [RegOp.CheckString(GcmKey, "guiPrompt", "true")],
        },
        new TweakDef
        {
            Id = "gitcm-disable-gui-prompts",
            Label = "Git Credential Manager: Use Console Authentication (Headless/CI Mode)",
            Category = "Dev Drive / Developer",
            Tags = ["git", "credential-manager", "console", "ci", "developer"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Console mode is required in CI/CD pipelines and SSH sessions without a display.",
            Description =
                "Sets guiPrompt=false in the GCM registry. Forces GCM to use console-based "
                + "authentication prompts that work in headless environments, CI/CD agents, "
                + "and remote SSH sessions where no browser or GUI dialog is available.",
            RegistryKeys = [GcmKey],
            ApplyOps = [RegOp.SetString(GcmKey, "guiPrompt", "false")],
            RemoveOps = [RegOp.DeleteValue(GcmKey, "guiPrompt")],
            DetectOps = [RegOp.CheckString(GcmKey, "guiPrompt", "false")],
        },
        new TweakDef
        {
            Id = "gitcm-enable-git-lfs-auth",
            Label = "Git Credential Manager: Enable Credential Pass-Through for Git LFS",
            Category = "Dev Drive / Developer",
            Tags = ["git", "git-lfs", "credential-manager", "developer"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ensures LFS transfers use the same credentials as regular Git operations.",
            Description =
                "Sets AutoDetectLfsStorageBackend=true in the GCM registry. Configures "
                + "Git Credential Manager to automatically detect and pass credentials "
                + "to Git LFS (Large File Storage) transfers without separate authentication. "
                + "Default: enabled. Explicit enforcement prevents LFS auth failures.",
            RegistryKeys = [GcmKey],
            ApplyOps = [RegOp.SetString(GcmKey, "AutoDetectLfsStorageBackend", "true")],
            RemoveOps = [RegOp.DeleteValue(GcmKey, "AutoDetectLfsStorageBackend")],
            DetectOps = [RegOp.CheckString(GcmKey, "AutoDetectLfsStorageBackend", "true")],
        },
        new TweakDef
        {
            Id = "gitcm-disable-update-check",
            Label = "Git Credential Manager: Disable Automatic Update Checks",
            Category = "Dev Drive / Developer",
            Tags = ["git", "credential-manager", "updates", "developer", "privacy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Disabling update checks prevents unexpected version changes and outbound connections.",
            Description =
                "Sets autoUpdate=false in the GCM registry. Prevents Git Credential Manager "
                + "from checking for updates automatically. Useful in locked-down environments "
                + "where software versions are managed by IT, or where network egress is restricted.",
            RegistryKeys = [GcmKey],
            ApplyOps = [RegOp.SetString(GcmKey, "autoUpdate", "false")],
            RemoveOps = [RegOp.DeleteValue(GcmKey, "autoUpdate")],
            DetectOps = [RegOp.CheckString(GcmKey, "autoUpdate", "false")],
        },
        new TweakDef
        {
            Id = "gitcm-set-trace-log-path",
            Label = "Git Credential Manager: Enable GCM Trace Logging",
            Category = "Dev Drive / Developer",
            Tags = ["git", "credential-manager", "trace", "logging", "developer"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Trace logs help diagnose authentication failures with detailed protocol data.",
            Description =
                "Sets trace=1 in the GCM registry. Enables verbose trace logging for "
                + "Git Credential Manager authentication operations. Trace output helps "
                + "diagnose OAuth flow failures, token refresh issues, and network errors.",
            RegistryKeys = [GcmKey],
            ApplyOps = [RegOp.SetString(GcmKey, "trace", "1")],
            RemoveOps = [RegOp.DeleteValue(GcmKey, "trace")],
            DetectOps = [RegOp.CheckString(GcmKey, "trace", "1")],
        },
        new TweakDef
        {
            Id = "gitcm-disable-trace-log",
            Label = "Git Credential Manager: Disable GCM Trace Logging",
            Category = "Dev Drive / Developer",
            Tags = ["git", "credential-manager", "trace", "logging", "developer"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Disabling trace reduces GCM startup overhead in normal operation.",
            Description =
                "Sets trace=0 in the GCM registry. Disables GCM trace logging. "
                + "Recommended for normal operation after authentication issues are resolved "
                + "to avoid performance overhead and verbose log output.",
            RegistryKeys = [GcmKey],
            ApplyOps = [RegOp.SetString(GcmKey, "trace", "0")],
            RemoveOps = [RegOp.DeleteValue(GcmKey, "trace")],
            DetectOps = [RegOp.CheckString(GcmKey, "trace", "0")],
        },
    ];
}
