using RegiLattice.Core.Models;

namespace RegiLattice.Core;

/// <summary>
/// Resolves tweak dependency chains using topological sorting.
/// Extracted from <see cref="TweakEngine"/> for single responsibility.
/// </summary>
public static class DependencyResolver
{
    /// <summary>
    /// Resolve the full dependency chain for a tweak, returning them in
    /// topological order (dependencies first, target last).
    /// Throws <see cref="InvalidOperationException"/> on circular dependencies.
    /// </summary>
    public static IReadOnlyList<TweakDef> Resolve(TweakDef td, Func<string, TweakDef?> tweakLookup)
    {
        if (td.DependsOn.Count == 0)
            return [td];

        var result = new List<TweakDef>();
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var inStack = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        TopoVisit(td, visited, inStack, result, tweakLookup);
        return result;
    }

    /// <summary>
    /// Find all tweaks that depend on the given tweak ID (reverse dependency lookup).
    /// </summary>
    public static IReadOnlyList<TweakDef> Dependents(
        string tweakId,
        IReadOnlyList<TweakDef> allTweaks) =>
        allTweaks.Where(t => t.DependsOn.Contains(tweakId, StringComparer.OrdinalIgnoreCase)).ToList();

    private static void TopoVisit(
        TweakDef td,
        HashSet<string> visited,
        HashSet<string> inStack,
        List<TweakDef> result,
        Func<string, TweakDef?> tweakLookup)
    {
        if (inStack.Contains(td.Id))
            throw new InvalidOperationException($"Circular dependency detected involving '{td.Id}'");
        if (!visited.Add(td.Id))
            return;
        inStack.Add(td.Id);

        foreach (var depId in td.DependsOn)
        {
            var dep = tweakLookup(depId);
            if (dep is not null)
                TopoVisit(dep, visited, inStack, result, tweakLookup);
        }

        inStack.Remove(td.Id);
        result.Add(td);
    }
}
