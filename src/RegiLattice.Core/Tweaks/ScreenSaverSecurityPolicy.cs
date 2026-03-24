// RegiLattice.Core — Tweaks/ScreenSaverSecurityPolicy.cs
// Screen Saver security and session timeout machine-scope GPO controls — Sprint 203.
// Enforces mandatory screen savers, password-on-resume, and screen timeout settings.
// Category: "Screen Saver Security Policy" | Slug: scrsvr
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScreenSaver

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ScreenSaverSecurityPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScreenSaver";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "scrsvr-enable-screensaver",
                Label = "Enforce Screen Saver Activation",
                Category = "Screen Saver Security Policy",
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
                Category = "Screen Saver Security Policy",
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
                Category = "Screen Saver Security Policy",
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
                Category = "Screen Saver Security Policy",
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
                Category = "Screen Saver Security Policy",
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
                Category = "Screen Saver Security Policy",
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
                Category = "Screen Saver Security Policy",
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
                Category = "Screen Saver Security Policy",
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
                Category = "Screen Saver Security Policy",
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
                Category = "Screen Saver Security Policy",
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
