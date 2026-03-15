namespace RegiLattice.Core.Tweaks;

/// <summary>
/// Shared registry hive constants used across all tweak modules.
/// Prefer these over per-module private constants.
/// </summary>
internal static class Hive
{
    internal const string LM = @"HKEY_LOCAL_MACHINE";
    internal const string CU = @"HKEY_CURRENT_USER";
}
