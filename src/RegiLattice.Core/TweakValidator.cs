using RegiLattice.Core.Models;

namespace RegiLattice.Core;

/// <summary>
/// Validates the integrity of registered tweak definitions.
/// Extracted from <see cref="TweakEngine"/> for single responsibility.
/// </summary>
public static class TweakValidator
{
    /// <summary>
    /// Validate a collection of tweaks and return a list of issues found.
    /// Checks: empty IDs/Labels, broken DependsOn references, duplicate IDs, empty ops.
    /// </summary>
    public static IReadOnlyList<string> Validate(IReadOnlyList<TweakDef> allTweaks, Func<string, TweakDef?> tweakLookup)
    {
        var errors = new List<string>();
        var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var td in allTweaks)
        {
            if (string.IsNullOrWhiteSpace(td.Id))
                errors.Add("Tweak with empty Id found");
            else if (!seenIds.Add(td.Id))
                errors.Add($"Duplicate tweak Id: {td.Id}");

            if (string.IsNullOrWhiteSpace(td.Label))
                errors.Add($"{td.Id}: empty Label");

            if (string.IsNullOrWhiteSpace(td.Category))
                errors.Add($"{td.Id}: empty Category");

            if (td.ImpactScore is < 1 or > 5)
                errors.Add($"{td.Id}: ImpactScore {td.ImpactScore} is out of range 1–5");

            if (td.SafetyRating is < 1 or > 5)
                errors.Add($"{td.Id}: SafetyRating {td.SafetyRating} is out of range 1–5");

            foreach (var dep in td.DependsOn)
            {
                if (tweakLookup(dep) is null)
                    errors.Add($"{td.Id}: DependsOn references unknown tweak '{dep}'");
            }

            if (td.ApplyOps.Count == 0 && td.ApplyAction is null)
                errors.Add($"{td.Id}: no ApplyOps or ApplyAction defined");
        }

        // Detect circular dependencies
        foreach (var td in allTweaks.Where(t => t.DependsOn.Count > 0))
        {
            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (HasCircularDep(td.Id, visited, tweakLookup))
                errors.Add($"{td.Id}: circular dependency detected");
        }

        return errors;
    }

    /// <summary>
    /// Finds ApplyOps that target the same registry Path+Name across different tweaks.
    /// Only considers write operations (Set*/Delete*), not detection (Check*) ops.
    /// </summary>
    public static IReadOnlyList<string> DetectDuplicateRegistryOps(IReadOnlyList<TweakDef> allTweaks)
    {
        var warnings = new List<string>();

        // Map "PATH\ValueName" → list of tweak IDs that write there
        var regTargets = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        foreach (var td in allTweaks)
        {
            foreach (var op in td.ApplyOps)
            {
                if (op.Kind is RegOpKind.CheckValue or RegOpKind.CheckMissing or RegOpKind.CheckKeyMissing)
                    continue;

                var key = string.IsNullOrEmpty(op.Name) ? op.Path : $@"{op.Path}\{op.Name}";

                if (!regTargets.TryGetValue(key, out var ids))
                {
                    ids = [];
                    regTargets[key] = ids;
                }
                ids.Add(td.Id);
            }
        }

        foreach (var (regKey, ids) in regTargets)
        {
            if (ids.Count > 1)
            {
                var distinct = ids.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                if (distinct.Count > 1)
                    warnings.Add($"Duplicate registry target '{regKey}' written by: {string.Join(", ", distinct)}");
            }
        }

        return warnings;
    }

    private static bool HasCircularDep(string id, HashSet<string> visited, Func<string, TweakDef?> tweakLookup)
    {
        if (!visited.Add(id))
            return true;
        var td = tweakLookup(id);
        if (td is null)
            return false;
        foreach (var dep in td.DependsOn)
        {
            if (HasCircularDep(dep, new HashSet<string>(visited, StringComparer.OrdinalIgnoreCase), tweakLookup))
                return true;
        }
        return false;
    }
}
