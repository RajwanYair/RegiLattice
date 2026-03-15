using System.Text.RegularExpressions;

namespace RegiLattice.GUI.PackageManagers;

/// <summary>Shared package name validation across all package managers.</summary>
internal static partial class PackageNameValidator
{
    /// <summary>Standard safe name: letters, digits, '.', '_', '-'.</summary>
    [GeneratedRegex(@"^[A-Za-z0-9._\-]+$")]
    private static partial Regex StandardNameRegex();

    /// <summary>Extended safe name: adds square brackets for pip extras syntax.</summary>
    [GeneratedRegex(@"^[A-Za-z0-9._\-\[\]]+$")]
    private static partial Regex ExtendedNameRegex();

    /// <summary>
    /// Validates a package/module name against the allowed character pattern.
    /// Throws <see cref="ArgumentException"/> if invalid.
    /// </summary>
    internal static string Validate(string name, string toolName, bool allowBrackets = false)
    {
        var regex = allowBrackets ? ExtendedNameRegex() : StandardNameRegex();
        if (string.IsNullOrWhiteSpace(name) || !regex.IsMatch(name))
        {
            string allowed = allowBrackets
                ? "only letters, digits, '.', '_', '-', '[]' allowed."
                : "only letters, digits, '.', '_', '-' allowed.";
            throw new ArgumentException($"Invalid {toolName} name '{name}': {allowed}");
        }
        return name;
    }

    /// <summary>
    /// Extracts package names from "Name (Version)" formatted strings.
    /// Shared by Scoop, Pip, and Chocolatey managers.
    /// </summary>
    internal static HashSet<string> ExtractNames(IEnumerable<string> entries)
    {
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var entry in entries)
        {
            int paren = entry.IndexOf(" (", StringComparison.Ordinal);
            names.Add(paren > 0 ? entry[..paren] : entry);
        }
        return names;
    }
}
