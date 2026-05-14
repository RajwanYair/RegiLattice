// RegiLattice.Core — Models/ProfileDef.cs
// Profile definitions matching Python's _PROFILES dict.

namespace RegiLattice.Core.Models;

public sealed class ProfileDef
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required IReadOnlyList<string> ApplyCategories { get; init; }
    public IReadOnlyList<string> SkipCategories { get; init; } = [];

    /// <summary>
    /// When non-empty, only tweaks that have ALL listed tags are included.
    /// Used for tag-driven profiles (e.g. <c>cis-l1</c>, <c>cis-l2</c>).
    /// Category filtering is skipped when this is set.
    /// </summary>
    public IReadOnlyList<string> RequireTags { get; init; } = [];
}
