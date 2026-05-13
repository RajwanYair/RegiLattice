// RegiLattice.Core — Services/IHistoryRepository.cs
// Abstraction over tweak operation history persistence (B.3 — decouple TweakHistory from file I/O).

using RegiLattice.Core.Models;

namespace RegiLattice.Core.Services;

/// <summary>
/// Provides read/write access to the rolling history of tweak apply/remove/update operations.
/// Decouple storage from consumers so the GUI and CLI can receive this via DI
/// and tests can use an in-memory implementation without touching the filesystem.
/// </summary>
public interface IHistoryRepository
{
    /// <summary>Record a tweak apply operation.</summary>
    void RecordApply(string tweakId, TweakResult result);

    /// <summary>Record a tweak remove operation.</summary>
    void RecordRemove(string tweakId, TweakResult result);

    /// <summary>Record a tweak update operation.</summary>
    void RecordUpdate(string tweakId, TweakResult result);

    /// <summary>Record a generic operation by name.</summary>
    void Record(string tweakId, string action, string result);

    /// <summary>
    /// Returns the most recent history entries across all tweaks,
    /// up to <paramref name="count"/> entries ordered newest-first.
    /// </summary>
    IReadOnlyList<HistoryEntry> Recent(int count = 20);

    /// <summary>Returns all history entries for a specific tweak ID, newest-first.</summary>
    IReadOnlyList<HistoryEntry> ForTweak(string tweakId);

    /// <summary>Clear all history entries from both memory and the backing store.</summary>
    void Clear();

    /// <summary>Total number of stored history entries.</summary>
    int Count { get; }

    /// <summary>Flush any pending in-memory changes to the backing store.</summary>
    void Flush();
}
