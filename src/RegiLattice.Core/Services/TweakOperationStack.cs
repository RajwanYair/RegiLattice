// RegiLattice.Core — Services/TweakOperationStack.cs
// In-session undo/redo stack for tweak operations.
// Tracks up to MaxOps (50) apply/remove operations per GUI session.
// The stack is in-memory only — it is NOT persisted to TweakHistory.

#nullable enable

namespace RegiLattice.Core.Services;

/// <summary>The kind of tweak operation recorded in the undo/redo stack.</summary>
public enum OperationKind
{
    /// <summary>The tweak was applied (registry/config written).</summary>
    Apply,

    /// <summary>The tweak was removed (registry/config reverted).</summary>
    Remove,
}

/// <summary>
/// An immutable record of a single apply or remove operation.
/// Stored in the undo/redo stacks of <see cref="TweakOperationStack"/>.
/// </summary>
public readonly record struct TweakOperation(string TweakId, string Label, OperationKind Kind, DateTimeOffset ExecutedAt);

/// <summary>
/// In-memory undo/redo stack for tweak operations performed in the current GUI session.
/// <para>
/// Design contract:
/// <list type="bullet">
///   <item><see cref="Push"/> records a new user operation and clears the redo stack.</item>
///   <item><see cref="Undo"/> moves the most recent op to the redo stack; caller executes the inverse.</item>
///   <item><see cref="Redo"/> moves the most recently undone op back to the undo stack; caller re-executes it.</item>
///   <item>The undo stack is capped at <see cref="MaxOps"/> entries; the oldest entry is discarded on overflow.</item>
/// </list>
/// </para>
/// </summary>
public sealed class TweakOperationStack
{
    /// <summary>Maximum number of undoable operations retained (oldest trimmed on overflow).</summary>
    public const int MaxOps = 50;

    private readonly Stack<TweakOperation> _undo = new();
    private readonly Stack<TweakOperation> _redo = new();

    // ── Query properties ──────────────────────────────────────────────────────

    /// <summary>True when there is at least one operation that can be undone.</summary>
    public bool CanUndo => _undo.Count > 0;

    /// <summary>True when there is at least one operation that can be re-done.</summary>
    public bool CanRedo => _redo.Count > 0;

    /// <summary>Number of operations currently on the undo stack.</summary>
    public int UndoCount => _undo.Count;

    /// <summary>Number of operations currently on the redo stack.</summary>
    public int RedoCount => _redo.Count;

    /// <summary>
    /// The most recent operation on the undo stack (without popping it).
    /// Null when the undo stack is empty.
    /// Used for "↩ Undo: {Label}" status-bar display.
    /// </summary>
    public TweakOperation? PeekUndo => _undo.Count > 0 ? _undo.Peek() : null;

    /// <summary>
    /// The most recent operation on the redo stack (without popping it).
    /// Null when the redo stack is empty.
    /// Used for "↪ Redo: {Label}" status-bar display.
    /// </summary>
    public TweakOperation? PeekRedo => _redo.Count > 0 ? _redo.Peek() : null;

    // ── Mutation methods ──────────────────────────────────────────────────────

    /// <summary>
    /// Records a newly performed operation.
    /// <para>Always clears the redo stack — branching history is not supported.</para>
    /// <para>Trims the oldest undo entry when the undo stack exceeds <see cref="MaxOps"/>.</para>
    /// </summary>
    /// <param name="op">The operation that was just performed by the user.</param>
    public void Push(TweakOperation op)
    {
        _redo.Clear();
        _undo.Push(op);
        if (_undo.Count > MaxOps)
            TrimToCapacity();
    }

    /// <summary>
    /// Pops the most recent operation from the undo stack and moves it to the redo stack.
    /// </summary>
    /// <returns>
    /// The operation that was popped, or <c>null</c> when the undo stack is empty.
    /// The caller is responsible for executing the <em>inverse</em> of the returned operation:
    /// <list type="bullet">
    ///   <item><see cref="OperationKind.Apply"/> → call Remove on the tweak.</item>
    ///   <item><see cref="OperationKind.Remove"/> → call Apply on the tweak.</item>
    /// </list>
    /// </returns>
    public TweakOperation? Undo()
    {
        if (_undo.Count == 0)
            return null;
        var op = _undo.Pop();
        _redo.Push(op);
        return op;
    }

    /// <summary>
    /// Pops the most recent undone operation from the redo stack and moves it back to the undo stack.
    /// </summary>
    /// <returns>
    /// The operation to re-execute, or <c>null</c> when the redo stack is empty.
    /// The caller is responsible for re-executing the operation as originally performed:
    /// <list type="bullet">
    ///   <item><see cref="OperationKind.Apply"/> → call Apply on the tweak again.</item>
    ///   <item><see cref="OperationKind.Remove"/> → call Remove on the tweak again.</item>
    /// </list>
    /// </returns>
    public TweakOperation? Redo()
    {
        if (_redo.Count == 0)
            return null;
        var op = _redo.Pop();
        _undo.Push(op);
        return op;
    }

    /// <summary>Clears both undo and redo stacks (e.g., on engine reload or session reset).</summary>
    public void Clear()
    {
        _undo.Clear();
        _redo.Clear();
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private void TrimToCapacity()
    {
        // Stack<T>.ToArray() returns in pop order: index 0 = top (most recent).
        // Keep the MaxOps most-recent entries by discarding index MaxOps onwards.
        var items = _undo.ToArray();
        _undo.Clear();
        for (int i = MaxOps - 1; i >= 0; i--)
            _undo.Push(items[i]);
    }
}
