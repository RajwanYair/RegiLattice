using RegiLattice.Core.Models;

namespace RegiLattice.CLI;

/// <summary>ANSI escape code helpers for coloured terminal output.</summary>
internal static class ConsoleColorizer
{
    internal static bool NoColor { get; set; }

    internal static string Green(string s) => NoColor ? s : $"\x1b[32m{s}\x1b[0m";

    internal static string Red(string s) => NoColor ? s : $"\x1b[31m{s}\x1b[0m";

    internal static string Yellow(string s) => NoColor ? s : $"\x1b[33m{s}\x1b[0m";

    internal static string Dim(string s) => NoColor ? s : $"\x1b[90m{s}\x1b[0m";

    internal static string ColourisedStatus(TweakResult status) =>
        status switch
        {
            TweakResult.Applied => Green(status.ToString()),
            TweakResult.NotApplied => Red(status.ToString()),
            TweakResult.Error => Red(status.ToString()),
            TweakResult.SkippedCorp or TweakResult.SkippedBuild or TweakResult.SkippedHw => Yellow(status.ToString()),
            _ => Dim(status.ToString()),
        };
}
