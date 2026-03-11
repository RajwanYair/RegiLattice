// RegiLattice.Core — Models/ProfileDef.cs
// Profile definitions matching Python's _PROFILES dict.

namespace RegiLattice.Core.Models;

public sealed class ProfileDef
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required IReadOnlyList<string> ApplyCategories { get; init; }
    public IReadOnlyList<string> SkipCategories { get; init; } = [];
}
