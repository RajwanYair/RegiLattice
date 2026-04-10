using System.Text.Json;
using RegiLattice.Core.Models;

namespace RegiLattice.Core;

/// <summary>
/// Manages saving, loading, and restoring tweak state snapshots.
/// Extracted from <see cref="TweakEngine"/> for single responsibility.
/// </summary>
public sealed class SnapshotManager
{
    private readonly TweakEngine _engine;

    public SnapshotManager(TweakEngine engine)
    {
        _engine = engine;
    }

    /// <summary>Save the current status of all tweaks to a JSON snapshot file.</summary>
    /// <param name="cachedStatus">
    /// Optional pre-computed status map. Pass to skip live <see cref="TweakEngine.StatusMap"/> registry
    /// reads (slow on managed/Intune machines with filter drivers over large tweak sets).
    /// </param>
    public void Save(string path, Dictionary<string, TweakResult>? cachedStatus = null)
    {
        var status = cachedStatus ?? _engine.StatusMap();
        var snapshot = status.ToDictionary(kv => kv.Key, kv => kv.Value.ToString().ToLowerInvariant());
        var json = JsonSerializer.Serialize(snapshot, JsonOptions.Indented);
        File.WriteAllText(path, json);
    }

    /// <summary>Load a snapshot file and return the tweak ID → state dictionary.</summary>
    public static Dictionary<string, string> Load(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? [];
    }

    /// <summary>
    /// Restore tweak states from a snapshot file.
    /// Applies tweaks marked "applied" and removes tweaks marked "notapplied".
    /// Stale snapshot IDs are automatically resolved via <see cref="TweakEngine.ResolveMigration"/>:
    /// renamed IDs redirect to the current tweak; deprecated IDs (null resolution) are skipped.
    /// </summary>
    public Dictionary<string, TweakResult> Restore(string path, bool forceCorp = false)
    {
        var snapshot = Load(path);
        var results = new Dictionary<string, TweakResult>();
        foreach (var (id, state) in snapshot)
        {
            // Phase 6.5: auto-migrate renamed/deprecated snapshot IDs
            var resolvedId = _engine.ResolveMigration(id);
            if (resolvedId is null)
                continue; // deprecated tweak — no replacement, skip silently

            var td = _engine.GetTweak(resolvedId);
            if (td is null)
                continue;

            results[resolvedId] = state switch
            {
                "applied" => _engine.Apply(td, forceCorp: forceCorp),
                "notapplied" => _engine.Remove(td, forceCorp: forceCorp),
                _ => TweakResult.Unknown,
            };
        }
        return results;
    }
}
