// RegiLattice.Core — Tweaks/SystemRecoveryOptionsPolicy.cs
// System Recovery Options Group Policy — Sprint 191.
// Controls access to Windows Recovery Environment, startup repair automation,
// Reset This PC, and advanced recovery tools via Group Policy.
// Category: "Recovery Options Policy" | Slug: sysrecpol
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\SystemRecovery

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SystemRecoveryOptionsPolicy
{
    private const string RecKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SystemRecovery";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "sysrecpol-disable-startup-repair",
                Label = "Disable Automatic Startup Repair",
                Category = "Recovery Options Policy",
                Description =
                    "Sets DisableStartupRepair=1 to prevent Windows from automatically launching Startup Repair when repeated boot failures are detected. Useful for controlled boot environments.",
                Tags = ["recovery", "startup-repair", "boot", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 2,
                ImpactNote = "Automatic Startup Repair suppressed; boot failures require manual intervention to diagnose.",
                ApplyOps = [RegOp.SetDword(RecKey, "DisableStartupRepair", 1)],
                RemoveOps = [RegOp.DeleteValue(RecKey, "DisableStartupRepair")],
                DetectOps = [RegOp.CheckDword(RecKey, "DisableStartupRepair", 1)],
            },
            new TweakDef
            {
                Id = "sysrecpol-block-recovery-options-access",
                Label = "Block Access to Recovery Options Menu",
                Category = "Recovery Options Policy",
                Description =
                    "Sets AllowAccessToRecoveryOptions=0 to prevent users from accessing the Windows Recovery Options menu (F8/Shift+F8 at boot). Enhances security by restricting boot-time intervention.",
                Tags = ["recovery", "options", "boot", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 2,
                ImpactNote = "Recovery options menu inaccessible; emergency access locked to administrator-controlled methods.",
                ApplyOps = [RegOp.SetDword(RecKey, "AllowAccessToRecoveryOptions", 0)],
                RemoveOps = [RegOp.DeleteValue(RecKey, "AllowAccessToRecoveryOptions")],
                DetectOps = [RegOp.CheckDword(RecKey, "AllowAccessToRecoveryOptions", 0)],
            },
            new TweakDef
            {
                Id = "sysrecpol-disable-sr-from-recovery",
                Label = "Disable System Restore from Recovery Environment",
                Category = "Recovery Options Policy",
                Description =
                    "Sets DisableSystemRestoreFromRecovery=1 to remove System Restore as an option within the Windows Recovery Environment (WinRE), preventing rollback during recovery sessions.",
                Tags = ["recovery", "system-restore", "winre", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Users cannot use System Restore from WinRE; reduces risk of unauthorized config rollbacks.",
                ApplyOps = [RegOp.SetDword(RecKey, "DisableSystemRestoreFromRecovery", 1)],
                RemoveOps = [RegOp.DeleteValue(RecKey, "DisableSystemRestoreFromRecovery")],
                DetectOps = [RegOp.CheckDword(RecKey, "DisableSystemRestoreFromRecovery", 1)],
            },
            new TweakDef
            {
                Id = "sysrecpol-block-reset-this-pc",
                Label = "Block Reset This PC Option",
                Category = "Recovery Options Policy",
                Description =
                    "Sets DisableResetPC=1 to remove the Reset This PC option from the recovery environment and Settings > Recovery. Prevents full system resets that could wipe enterprise configurations.",
                Tags = ["recovery", "reset", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Reset This PC removed from Settings and WinRE; prevents unauthorized system wipes.",
                ApplyOps = [RegOp.SetDword(RecKey, "DisableResetPC", 1)],
                RemoveOps = [RegOp.DeleteValue(RecKey, "DisableResetPC")],
                DetectOps = [RegOp.CheckDword(RecKey, "DisableResetPC", 1)],
            },
            new TweakDef
            {
                Id = "sysrecpol-block-cmd-in-recovery",
                Label = "Block Command Prompt in Recovery Environment",
                Category = "Recovery Options Policy",
                Description =
                    "Sets DisableCmdInRecovery=1 to remove the Command Prompt option from WinRE Advanced Options. Prevents low-level shell access that could be used to bypass OS security controls.",
                Tags = ["recovery", "command-prompt", "winre", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "WinRE Command Prompt disabled; prevents recovery-time bypass of Windows security features.",
                ApplyOps = [RegOp.SetDword(RecKey, "DisableCmdInRecovery", 1)],
                RemoveOps = [RegOp.DeleteValue(RecKey, "DisableCmdInRecovery")],
                DetectOps = [RegOp.CheckDword(RecKey, "DisableCmdInRecovery", 1)],
            },
            new TweakDef
            {
                Id = "sysrecpol-disable-recovery-ui",
                Label = "Disable Recovery Environment User Interface",
                Category = "Recovery Options Policy",
                Description =
                    "Sets DisableRecoveryUI=1 to suppress the Windows Recovery Environment graphical interface. Recovery actions are restricted to command-line tools or domain-administered methods.",
                Tags = ["recovery", "ui", "winre", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "WinRE graphical UI disabled; reduces attack surface during unattended or kiosk boot scenarios.",
                ApplyOps = [RegOp.SetDword(RecKey, "DisableRecoveryUI", 1)],
                RemoveOps = [RegOp.DeleteValue(RecKey, "DisableRecoveryUI")],
                DetectOps = [RegOp.CheckDword(RecKey, "DisableRecoveryUI", 1)],
            },
            new TweakDef
            {
                Id = "sysrecpol-block-advanced-recovery-tools",
                Label = "Block Advanced Recovery Tools",
                Category = "Recovery Options Policy",
                Description =
                    "Sets BlockAdvancedTools=1 to hide Advanced Recovery Tools such as System Image Recovery, Startup Settings, and UEFI Firmware Settings from the WinRE options menu.",
                Tags = ["recovery", "advanced-tools", "winre", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Advanced WinRE tools hidden; prevents unauthorized UEFI/firmware modifications from recovery.",
                ApplyOps = [RegOp.SetDword(RecKey, "BlockAdvancedTools", 1)],
                RemoveOps = [RegOp.DeleteValue(RecKey, "BlockAdvancedTools")],
                DetectOps = [RegOp.CheckDword(RecKey, "BlockAdvancedTools", 1)],
            },
            new TweakDef
            {
                Id = "sysrecpol-disable-auto-recovery-boot",
                Label = "Disable Automatic Recovery Boot Sequence",
                Category = "Recovery Options Policy",
                Description =
                    "Sets DisableAutoRecoveryBoot=1 to prevent Windows from automatically booting into the recovery environment after consecutive failed normal boots. Boots to error screen instead.",
                Tags = ["recovery", "boot", "automatic", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 2,
                ImpactNote = "Auto-recovery boot disabled; persistent boot failures require manual diagnostics access.",
                ApplyOps = [RegOp.SetDword(RecKey, "DisableAutoRecoveryBoot", 1)],
                RemoveOps = [RegOp.DeleteValue(RecKey, "DisableAutoRecoveryBoot")],
                DetectOps = [RegOp.CheckDword(RecKey, "DisableAutoRecoveryBoot", 1)],
            },
            new TweakDef
            {
                Id = "sysrecpol-hide-recovery-console",
                Label = "Hide Recovery Console Menu Entry",
                Category = "Recovery Options Policy",
                Description =
                    "Sets HideRecoveryConsole=1 to remove the Recovery Console entry from the boot manager and WinRE menus. Prevents direct console access that bypasses normal Windows login.",
                Tags = ["recovery", "console", "boot", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Recovery console option hidden from boot menu; reduces physical-access attack surface.",
                ApplyOps = [RegOp.SetDword(RecKey, "HideRecoveryConsole", 1)],
                RemoveOps = [RegOp.DeleteValue(RecKey, "HideRecoveryConsole")],
                DetectOps = [RegOp.CheckDword(RecKey, "HideRecoveryConsole", 1)],
            },
            new TweakDef
            {
                Id = "sysrecpol-disable-memory-diagnostics",
                Label = "Disable Memory Diagnostics in Recovery",
                Category = "Recovery Options Policy",
                Description =
                    "Sets DisableMemoryDiagnostics=1 to hide the Windows Memory Diagnostic option in WinRE. Prevents access to diagnostics tools that could be misused in shared-access environments.",
                Tags = ["recovery", "memory", "diagnostics", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Memory Diagnostics option hidden in WinRE; standard memory testing still available to admins.",
                ApplyOps = [RegOp.SetDword(RecKey, "DisableMemoryDiagnostics", 1)],
                RemoveOps = [RegOp.DeleteValue(RecKey, "DisableMemoryDiagnostics")],
                DetectOps = [RegOp.CheckDword(RecKey, "DisableMemoryDiagnostics", 1)],
            },
        ];
}
