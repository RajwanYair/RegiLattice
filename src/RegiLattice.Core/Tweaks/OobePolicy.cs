#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 245 — OOBE & First-Run Experience Policy (10 tweaks)
// Keys: HKLM\SOFTWARE\Policies\Microsoft\Windows\OOBE
//       HKLM\SOFTWARE\Policies\Microsoft\Windows\Setup
//       HKLM\SOFTWARE\Policies\Microsoft\Windows\Shell
//       HKCU\Software\Policies\Microsoft\Windows\Shell
//       HKLM\SOFTWARE\Policies\Microsoft\Server Manager
//       HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System
internal static class OobePolicy
{
    private const string OobeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\OOBE";
    private const string SetupKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Setup";
    private const string ShellLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Shell";
    private const string ShellCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Shell";
    private const string SrvMgrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Server Manager";
    private const string SystemPolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "oobe-disable-privacy-experience",
            Label = "Disable OOBE Privacy Experience",
            Category = "OOBE & Setup Policy",
            Description =
                "Sets DisablePrivacyExperience=1 in the Windows OOBE policy key. "
                + "Prevents the full-screen privacy settings wizard from appearing on first sign-in for new user accounts "
                + "(covers Diagnostic Data, Inking, Location, and related consent screens). "
                + "Default: absent (privacy wizard shown). Recommended: 1 on domain-joined or company-provisioned devices.",
            Tags = ["oobe", "privacy", "first-run", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Skips the OOBE privacy consent wizard on first sign-in; privacy settings remain at system defaults.",
            ApplyOps = [RegOp.SetDword(OobeKey, "DisablePrivacyExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(OobeKey, "DisablePrivacyExperience")],
            DetectOps = [RegOp.CheckDword(OobeKey, "DisablePrivacyExperience", 1)],
        },
        new TweakDef
        {
            Id = "oobe-skip-user-oobe",
            Label = "Skip User OOBE Page",
            Category = "OOBE & Setup Policy",
            Description =
                "Sets SkipUserOOBE=1 in the Windows OOBE policy key. "
                + "Suppresses the user portion of the Out-of-Box Experience wizard, skipping personalization and "
                + "account setup prompts at first logon for new local users. "
                + "Default: absent (user OOBE shown). Recommended: 1 on pre-provisioned enterprise desktops.",
            Tags = ["oobe", "first-run", "setup", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Skips user-facing OOBE setup prompts; account is still created with system defaults.",
            ApplyOps = [RegOp.SetDword(OobeKey, "SkipUserOOBE", 1)],
            RemoveOps = [RegOp.DeleteValue(OobeKey, "SkipUserOOBE")],
            DetectOps = [RegOp.CheckDword(OobeKey, "SkipUserOOBE", 1)],
        },
        new TweakDef
        {
            Id = "oobe-skip-machine-oobe",
            Label = "Skip Machine OOBE Page",
            Category = "OOBE & Setup Policy",
            Description =
                "Sets SkipMachineOOBE=1 in the Windows OOBE policy key. "
                + "Suppresses the machine-level portion of the OOBE wizard during initial Windows setup, "
                + "skipping device configuration prompts such as region and language when a response answer file is in use. "
                + "Default: absent (machine OOBE shown). Recommended: 1 in MDT/WDS/Autopilot deployments.",
            Tags = ["oobe", "setup", "provisioning", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Skips machine-level OOBE prompts during Windows setup; used mainly in imaging/provisioning scenarios.",
            ApplyOps = [RegOp.SetDword(OobeKey, "SkipMachineOOBE", 1)],
            RemoveOps = [RegOp.DeleteValue(OobeKey, "SkipMachineOOBE")],
            DetectOps = [RegOp.CheckDword(OobeKey, "SkipMachineOOBE", 1)],
        },
        new TweakDef
        {
            Id = "oobe-no-network-connections-wizard",
            Label = "Disable OOBE Network Connections Wizard",
            Category = "OOBE & Setup Policy",
            Description =
                "Sets DisableNetworkConnectionsWizard=1 in the Windows OOBE policy key. "
                + "Suppresses the network connection setup wizard that appears during the OOBE phase, "
                + "useful when network configuration is handled by MDM or answer files. "
                + "Default: absent (network wizard shown). Recommended: 1 in managed deployment scenarios.",
            Tags = ["oobe", "network", "wizard", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Skips the OOBE network setup wizard; network connectivity is handled by provisioning tools.",
            ApplyOps = [RegOp.SetDword(OobeKey, "DisableNetworkConnectionsWizard", 1)],
            RemoveOps = [RegOp.DeleteValue(OobeKey, "DisableNetworkConnectionsWizard")],
            DetectOps = [RegOp.CheckDword(OobeKey, "DisableNetworkConnectionsWizard", 1)],
        },
        new TweakDef
        {
            Id = "oobe-no-first-logon-animation",
            Label = "Disable First Logon Animation",
            Category = "OOBE & Setup Policy",
            Description =
                "Sets ShowFirstLogonAnimation=0 in the Windows Setup policy key. "
                + "Disables the full-screen 'Hi' and 'Getting Windows ready' animation sequence shown to new users on first sign-in, "
                + "reducing the wait time at initial logon. "
                + "Default: absent (animation shown). Recommended: 0 on corporate desktops for faster first-logon.",
            Tags = ["oobe", "animation", "first-logon", "performance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Skips the first-logon animation; new users reach the desktop faster on initial sign-in.",
            ApplyOps = [RegOp.SetDword(SetupKey, "ShowFirstLogonAnimation", 0)],
            RemoveOps = [RegOp.DeleteValue(SetupKey, "ShowFirstLogonAnimation")],
            DetectOps = [RegOp.CheckDword(SetupKey, "ShowFirstLogonAnimation", 0)],
        },
        new TweakDef
        {
            Id = "oobe-no-welcome-screen-lm",
            Label = "Disable Welcome Screen (Machine)",
            Category = "OOBE & Setup Policy",
            Description =
                "Sets NoWelcomeScreen=1 in the machine-scoped Windows Shell policy key. "
                + "Suppresses the Windows Welcome Center / Did You Know tips overlay that could appear post-setup. "
                + "Default: absent. Recommended: 1 on managed enterprise desktops to skip unneeded first-run UI.",
            Tags = ["oobe", "welcome", "shell", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Suppresses the Welcome Center overlay for all users; no functional impact after initial run.",
            ApplyOps = [RegOp.SetDword(ShellLm, "NoWelcomeScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(ShellLm, "NoWelcomeScreen")],
            DetectOps = [RegOp.CheckDword(ShellLm, "NoWelcomeScreen", 1)],
        },
        new TweakDef
        {
            Id = "oobe-no-welcome-screen-user",
            Label = "Disable Welcome Screen (Current User)",
            Category = "OOBE & Setup Policy",
            Description =
                "Sets NoWelcomeScreen=1 in the per-user Windows Shell policy key. "
                + "Hides the Welcome Center / Getting Started experience for the current user. "
                + "Default: absent. Recommended: 1 for individual user profiles on managed systems.",
            Tags = ["oobe", "welcome", "shell", "user", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Suppresses the Welcome Center for this user only; no functional impact.",
            ApplyOps = [RegOp.SetDword(ShellCu, "NoWelcomeScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(ShellCu, "NoWelcomeScreen")],
            DetectOps = [RegOp.CheckDword(ShellCu, "NoWelcomeScreen", 1)],
        },
        new TweakDef
        {
            Id = "oobe-no-server-manager-at-logon",
            Label = "Disable Server Manager Auto-Open at Logon",
            Category = "OOBE & Setup Policy",
            Description =
                "Sets DoNotOpenServerManagerAtLogon=1 in the Server Manager policy key. "
                + "Prevents Windows Server Manager from automatically opening at every administrator logon. "
                + "Applies to Windows Server editions; setting is ignored on Windows Client. "
                + "Default: absent (Server Manager opens at logon). Recommended: 1 on production servers where automatic windows interfere with operations.",
            Tags = ["oobe", "server-manager", "server", "logon", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Server Manager no longer opens automatically at logon; it can still be launched manually.",
            ApplyOps = [RegOp.SetDword(SrvMgrKey, "DoNotOpenServerManagerAtLogon", 1)],
            RemoveOps = [RegOp.DeleteValue(SrvMgrKey, "DoNotOpenServerManagerAtLogon")],
            DetectOps = [RegOp.CheckDword(SrvMgrKey, "DoNotOpenServerManagerAtLogon", 1)],
        },
        new TweakDef
        {
            Id = "oobe-disable-balloon-tips",
            Label = "Disable System Tray Balloon Tips",
            Category = "OOBE & Setup Policy",
            Description =
                "Sets EnableBalloonTips=0 in the machine-side System policy key. "
                + "Suppresses all Action Center / notification area balloon notifications and first-run tip balloons "
                + "that appear after the initial desktop load. "
                + "Default: absent (balloon tips enabled). Recommended: 0 on corporate desktops to reduce user interruptions.",
            Tags = ["oobe", "balloon", "notifications", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Suppresses first-run balloon tips and system tray notifications; Action Center itself is unaffected.",
            ApplyOps = [RegOp.SetDword(ShellLm, "EnableBalloonTips", 0)],
            RemoveOps = [RegOp.DeleteValue(ShellLm, "EnableBalloonTips")],
            DetectOps = [RegOp.CheckDword(ShellLm, "EnableBalloonTips", 0)],
        },
        new TweakDef
        {
            Id = "oobe-disable-upgrade-ui",
            Label = "Disable Windows Upgrade Prompt UI",
            Category = "OOBE & Setup Policy",
            Description =
                "Sets DisableUXFirstRunAnimation=1 in the Windows Setup policy key. "
                + "Suppresses the upgrade experience UX animations and first-run prompts that may appear "
                + "after a major Windows feature update is applied to an existing account. "
                + "Default: absent. Recommended: 1 on managed devices receiving OS updates via WSUS / Autopilot.",
            Tags = ["oobe", "upgrade", "animation", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Suppresses post-upgrade UX first-run animations; system functionality is unaffected.",
            ApplyOps = [RegOp.SetDword(SetupKey, "DisableUXFirstRunAnimation", 1)],
            RemoveOps = [RegOp.DeleteValue(SetupKey, "DisableUXFirstRunAnimation")],
            DetectOps = [RegOp.CheckDword(SetupKey, "DisableUXFirstRunAnimation", 1)],
        },
    ];
}
