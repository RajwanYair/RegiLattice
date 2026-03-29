// RegiLattice.Core — Tweaks/WhfbPinPolicy.cs
// Windows Hello for Business PIN complexity, length, history, and expiry controls — Sprint 477.
// Category: "WHfB PIN Policy" | Slug: whfbpin
// Registry: HKLM\SOFTWARE\Policies\Microsoft\PassportForWork\PINComplexity

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WhfbPinPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork\PINComplexity";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "whfbpin-set-minimum-length-8",
                Label = "Set WHfB PIN Minimum Length to 8 Digits",
                Category = "WHfB PIN Policy",
                Description =
                    "Sets the minimum Windows Hello for Business PIN length to 8 characters, exceeding the Windows default of 6 characters and increasing PIN brute-force resistance.",
                Tags = ["whfb", "windows-hello", "pin", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "WHfB PIN must be at least 8 characters; users with shorter PINs must re-set.",
                ApplyOps = [RegOp.SetDword(Key, "MinimumPINLength", 8)],
                RemoveOps = [RegOp.DeleteValue(Key, "MinimumPINLength")],
                DetectOps = [RegOp.CheckDword(Key, "MinimumPINLength", 8)],
            },
            new TweakDef
            {
                Id = "whfbpin-set-maximum-length-16",
                Label = "Set WHfB PIN Maximum Length to 16 Digits",
                Category = "WHfB PIN Policy",
                Description =
                    "Sets the maximum Windows Hello for Business PIN length to 16 characters, balancing usability with security and preventing excessively long PINs that users may forget.",
                Tags = ["whfb", "windows-hello", "pin", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WHfB PIN capped at 16 characters.",
                ApplyOps = [RegOp.SetDword(Key, "MaximumPINLength", 16)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaximumPINLength")],
                DetectOps = [RegOp.CheckDword(Key, "MaximumPINLength", 16)],
            },
            new TweakDef
            {
                Id = "whfbpin-require-uppercase",
                Label = "Require Uppercase Letters in WHfB PIN",
                Category = "WHfB PIN Policy",
                Description =
                    "Requires that WHfB PINs contain at least one uppercase letter when using an alphanumeric PIN, increasing PIN complexity and resistance to dictionary attacks.",
                Tags = ["whfb", "windows-hello", "pin", "complexity", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WHfB PIN must contain uppercase; digits-only PINs disallowed.",
                ApplyOps = [RegOp.SetDword(Key, "UppercaseLetters", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "UppercaseLetters")],
                DetectOps = [RegOp.CheckDword(Key, "UppercaseLetters", 1)],
            },
            new TweakDef
            {
                Id = "whfbpin-require-lowercase",
                Label = "Require Lowercase Letters in WHfB PIN",
                Category = "WHfB PIN Policy",
                Description =
                    "Requires that WHfB PINs contain at least one lowercase letter, enforcing mixed-case alphanumeric PINs for greater entropy.",
                Tags = ["whfb", "windows-hello", "pin", "complexity", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WHfB PIN must contain lowercase; all-uppercase or all-digit PINs disallowed.",
                ApplyOps = [RegOp.SetDword(Key, "LowercaseLetters", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LowercaseLetters")],
                DetectOps = [RegOp.CheckDword(Key, "LowercaseLetters", 1)],
            },
            new TweakDef
            {
                Id = "whfbpin-require-special-chars",
                Label = "Require Special Characters in WHfB PIN",
                Category = "WHfB PIN Policy",
                Description =
                    "Requires at least one special character in Windows Hello for Business PINs, maximising PIN entropy and preventing trivially guessable numeric or alphabetic patterns.",
                Tags = ["whfb", "windows-hello", "pin", "special-chars", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WHfB PIN must include a special character; numeric-only PINs disallowed.",
                ApplyOps = [RegOp.SetDword(Key, "SpecialCharacters", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SpecialCharacters")],
                DetectOps = [RegOp.CheckDword(Key, "SpecialCharacters", 1)],
            },
            new TweakDef
            {
                Id = "whfbpin-set-pin-history-5",
                Label = "Set WHfB PIN History to 5 Previous PINs",
                Category = "WHfB PIN Policy",
                Description =
                    "Prevents reuse of the last 5 WHfB PINs, stopping users from cycling back to recently used PINs immediately after a mandatory PIN change.",
                Tags = ["whfb", "windows-hello", "pin", "history", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Last 5 WHfB PINs remembered; PIN cannot be recycled until 5 unique PINs have been used.",
                ApplyOps = [RegOp.SetDword(Key, "History", 5)],
                RemoveOps = [RegOp.DeleteValue(Key, "History")],
                DetectOps = [RegOp.CheckDword(Key, "History", 5)],
            },
            new TweakDef
            {
                Id = "whfbpin-set-expiry-180-days",
                Label = "Set WHfB PIN Expiry to 180 Days",
                Category = "WHfB PIN Policy",
                Description =
                    "Sets the Windows Hello for Business PIN expiry period to 180 days, requiring periodic PIN rotation to limit the impact of a compromised PIN.",
                Tags = ["whfb", "windows-hello", "pin", "expiry", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WHfB PIN expires after 180 days; users prompted to create a new PIN on expiry.",
                ApplyOps = [RegOp.SetDword(Key, "Expiration", 180)],
                RemoveOps = [RegOp.DeleteValue(Key, "Expiration")],
                DetectOps = [RegOp.CheckDword(Key, "Expiration", 180)],
            },
            new TweakDef
            {
                Id = "whfbpin-require-digits",
                Label = "Require Digits in WHfB Alphanumeric PIN",
                Category = "WHfB PIN Policy",
                Description =
                    "Requires that alphanumeric WHfB PINs contain at least one digit, preventing purely alphabetic PINs and ensuring a minimum numeric component in the PIN.",
                Tags = ["whfb", "windows-hello", "pin", "digits", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WHfB alphanumeric PIN must include at least one digit.",
                ApplyOps = [RegOp.SetDword(Key, "Digits", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "Digits")],
                DetectOps = [RegOp.CheckDword(Key, "Digits", 1)],
            },
            new TweakDef
            {
                Id = "whfbpin-block-simple-patterns",
                Label = "Block Simple/Sequential WHfB PIN Patterns",
                Category = "WHfB PIN Policy",
                Description =
                    "Blocks common sequential (1234, abcd) and repeated-character (1111, aaaa) PIN patterns for WHfB, preventing trivially guessable PINs from being set.",
                Tags = ["whfb", "windows-hello", "pin", "patterns", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Simple PIN patterns blocked; sequential and repeated patterns rejected at PIN creation.",
                ApplyOps = [RegOp.SetDword(Key, "BlockSimplePatterns", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockSimplePatterns")],
                DetectOps = [RegOp.CheckDword(Key, "BlockSimplePatterns", 1)],
            },
            new TweakDef
            {
                Id = "whfbpin-lockout-after-5-failures",
                Label = "Lock Out WHfB PIN After 5 Failed Attempts",
                Category = "WHfB PIN Policy",
                Description =
                    "Locks the WHfB PIN credential after 5 consecutive failed login attempts, requiring a PIN reset via recovery, defending against online brute-force attacks.",
                Tags = ["whfb", "windows-hello", "pin", "lockout", "brute-force", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "WHfB PIN locked after 5 failed attempts; reset required, stopping online PIN guessing.",
                ApplyOps = [RegOp.SetDword(Key, "MaxFailedAttempts", 5)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxFailedAttempts")],
                DetectOps = [RegOp.CheckDword(Key, "MaxFailedAttempts", 5)],
            },
        ];
}
