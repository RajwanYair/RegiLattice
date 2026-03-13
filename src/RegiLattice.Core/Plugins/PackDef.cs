// RegiLattice.Core — Plugins/PackDef.cs
// Immutable model for a Tweak Pack (community plugin).

namespace RegiLattice.Core.Plugins;

/// <summary>
/// Metadata for a Tweak Pack — a community-authored bundle of declarative tweaks.
/// Packs are JSON-only (no .dll execution) and installed to %LOCALAPPDATA%\RegiLattice\packs\.
/// </summary>
public sealed record PackDef
{
    public required string Name { get; init; }
    public required string DisplayName { get; init; }
    public required string Version { get; init; }
    public required string Author { get; init; }
    public string Description { get; init; } = "";
    public int TweakCount { get; init; }
    public IReadOnlyList<string> Categories { get; init; } = [];
    public IReadOnlyList<string> Tags { get; init; } = [];
    public string Sha256 { get; init; } = "";
    public string DownloadUrl { get; init; } = "";
    public string MinRegiLatticeVersion { get; init; } = "3.3.0";
    public int MinWindowsBuild { get; init; }
}
