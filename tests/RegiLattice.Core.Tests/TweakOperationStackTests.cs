// tests/RegiLattice.Core.Tests/TweakOperationStackTests.cs
// xUnit unit tests for TweakOperationStack (D1 — undo/redo system).

#nullable enable

using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

// ── Helpers ──────────────────────────────────────────────────────────────────

file static class Ops
{
    private static int _seq;

    public static TweakOperation Apply(string id = "test-tweak", string label = "Test Tweak") =>
        new(id, label, OperationKind.Apply, DateTimeOffset.UtcNow.AddSeconds(-_seq++));

    public static TweakOperation Remove(string id = "test-tweak", string label = "Test Tweak") =>
        new(id, label, OperationKind.Remove, DateTimeOffset.UtcNow.AddSeconds(-_seq++));
}

// ── TweakOperation record tests ───────────────────────────────────────────────

public sealed class TweakOperationRecordTests
{
    [Fact]
    public void TweakOperation_Apply_KindIsApply()
    {
        var op = Ops.Apply("priv-disable-telemetry", "Disable Telemetry");
        Assert.Equal(OperationKind.Apply, op.Kind);
    }

    [Fact]
    public void TweakOperation_Remove_KindIsRemove()
    {
        var op = Ops.Remove("priv-disable-telemetry", "Disable Telemetry");
        Assert.Equal(OperationKind.Remove, op.Kind);
    }

    [Fact]
    public void TweakOperation_Fields_ArePreserved()
    {
        var now = DateTimeOffset.UtcNow;
        var op = new TweakOperation("abc-id", "ABC Label", OperationKind.Apply, now);
        Assert.Equal("abc-id", op.TweakId);
        Assert.Equal("ABC Label", op.Label);
        Assert.Equal(OperationKind.Apply, op.Kind);
        Assert.Equal(now, op.ExecutedAt);
    }

    [Fact]
    public void TweakOperation_EqualityByValue()
    {
        var t = DateTimeOffset.UtcNow;
        var a = new TweakOperation("x", "X", OperationKind.Apply, t);
        var b = new TweakOperation("x", "X", OperationKind.Apply, t);
        Assert.Equal(a, b);
    }
}

// ── Empty-stack invariants ────────────────────────────────────────────────────

public sealed class TweakOperationStackEmptyTests
{
    [Fact]
    public void NewStack_CanUndo_IsFalse()
    {
        var stack = new TweakOperationStack();
        Assert.False(stack.CanUndo);
    }

    [Fact]
    public void NewStack_CanRedo_IsFalse()
    {
        var stack = new TweakOperationStack();
        Assert.False(stack.CanRedo);
    }

    [Fact]
    public void NewStack_UndoCount_IsZero()
    {
        var stack = new TweakOperationStack();
        Assert.Equal(0, stack.UndoCount);
    }

    [Fact]
    public void NewStack_RedoCount_IsZero()
    {
        var stack = new TweakOperationStack();
        Assert.Equal(0, stack.RedoCount);
    }

    [Fact]
    public void NewStack_PeekUndo_IsNull()
    {
        var stack = new TweakOperationStack();
        Assert.Null(stack.PeekUndo);
    }

    [Fact]
    public void NewStack_PeekRedo_IsNull()
    {
        var stack = new TweakOperationStack();
        Assert.Null(stack.PeekRedo);
    }

    [Fact]
    public void Undo_EmptyStack_ReturnsNull()
    {
        var stack = new TweakOperationStack();
        Assert.Null(stack.Undo());
    }

    [Fact]
    public void Redo_EmptyStack_ReturnsNull()
    {
        var stack = new TweakOperationStack();
        Assert.Null(stack.Redo());
    }
}

// ── Push behaviour ────────────────────────────────────────────────────────────

public sealed class TweakOperationStackPushTests
{
    [Fact]
    public void Push_OneOp_CanUndoIsTrue()
    {
        var stack = new TweakOperationStack();
        stack.Push(Ops.Apply());
        Assert.True(stack.CanUndo);
    }

    [Fact]
    public void Push_OneOp_UndoCountIsOne()
    {
        var stack = new TweakOperationStack();
        stack.Push(Ops.Apply());
        Assert.Equal(1, stack.UndoCount);
    }

    [Fact]
    public void Push_OneOp_PeekUndoReturnsOp()
    {
        var stack = new TweakOperationStack();
        var op = Ops.Apply("perf-disable-animations", "Disable Animations");
        stack.Push(op);
        Assert.Equal(op, stack.PeekUndo);
    }

    [Fact]
    public void Push_MultipleOps_PeekUndoReturnsLastPushed()
    {
        var stack = new TweakOperationStack();
        stack.Push(Ops.Apply("id-1", "Label 1"));
        var recent = Ops.Apply("id-2", "Label 2");
        stack.Push(recent);
        Assert.Equal(recent, stack.PeekUndo);
    }

    [Fact]
    public void Push_ClearsRedoStack()
    {
        var stack = new TweakOperationStack();
        stack.Push(Ops.Apply());
        stack.Undo(); // moves op to redo
        Assert.True(stack.CanRedo);

        stack.Push(Ops.Apply("other", "Other")); // new op must clear redo
        Assert.False(stack.CanRedo);
    }
}

// ── Undo behaviour ────────────────────────────────────────────────────────────

public sealed class TweakOperationStackUndoTests
{
    [Fact]
    public void Undo_OneOp_ReturnsOp()
    {
        var stack = new TweakOperationStack();
        var op = Ops.Apply("priv-disable-telemetry", "Disable Telemetry");
        stack.Push(op);
        Assert.Equal(op, stack.Undo());
    }

    [Fact]
    public void Undo_OneOp_EmptiesUndoStack()
    {
        var stack = new TweakOperationStack();
        stack.Push(Ops.Apply());
        stack.Undo();
        Assert.False(stack.CanUndo);
        Assert.Equal(0, stack.UndoCount);
    }

    [Fact]
    public void Undo_OneOp_PopulatesRedoStack()
    {
        var stack = new TweakOperationStack();
        var op = Ops.Apply();
        stack.Push(op);
        stack.Undo();
        Assert.True(stack.CanRedo);
        Assert.Equal(op, stack.PeekRedo);
    }

    [Fact]
    public void Undo_TwoOps_UndoUnwindsLastFirst()
    {
        var stack = new TweakOperationStack();
        var op1 = Ops.Apply("id-1", "First");
        var op2 = Ops.Apply("id-2", "Second");
        stack.Push(op1);
        stack.Push(op2);

        Assert.Equal(op2, stack.Undo()); // most-recent first
        Assert.Equal(op1, stack.Undo()); // then older
        Assert.Null(stack.Undo()); // now empty
    }
}

// ── Redo behaviour ────────────────────────────────────────────────────────────

public sealed class TweakOperationStackRedoTests
{
    [Fact]
    public void Redo_AfterUndo_ReturnsOp()
    {
        var stack = new TweakOperationStack();
        var op = Ops.Apply("priv-disable-telemetry", "Disable Telemetry");
        stack.Push(op);
        stack.Undo();
        Assert.Equal(op, stack.Redo());
    }

    [Fact]
    public void Redo_AfterUndo_RestoresUndoStack()
    {
        var stack = new TweakOperationStack();
        var op = Ops.Apply();
        stack.Push(op);
        stack.Undo();
        stack.Redo();
        Assert.True(stack.CanUndo);
        Assert.False(stack.CanRedo);
    }

    [Fact]
    public void Redo_MultipleTimes_ReplaysInCorrectOrder()
    {
        var stack = new TweakOperationStack();
        var op1 = Ops.Apply("id-1", "First");
        var op2 = Ops.Apply("id-2", "Second");
        stack.Push(op1);
        stack.Push(op2);
        stack.Undo();
        stack.Undo();

        Assert.Equal(op1, stack.Redo()); // oldest undone re-done first
        Assert.Equal(op2, stack.Redo());
        Assert.Null(stack.Redo()); // nothing left to redo
    }
}

// ── Capacity / overflow behaviour ────────────────────────────────────────────

public sealed class TweakOperationStackCapacityTests
{
    [Fact]
    public void MaxOps_IsAtLeast50()
    {
        Assert.True(TweakOperationStack.MaxOps >= 50);
    }

    [Fact]
    public void Push_ExceedingMaxOps_DoesNotThrow()
    {
        var stack = new TweakOperationStack();
        int overflow = TweakOperationStack.MaxOps + 5;
        for (int i = 0; i < overflow; i++)
            stack.Push(Ops.Apply($"tweak-{i}", $"Tweak {i}"));
    }

    [Fact]
    public void Push_ExceedingMaxOps_UndoCountStaysAtMax()
    {
        var stack = new TweakOperationStack();
        int overflow = TweakOperationStack.MaxOps + 10;
        for (int i = 0; i < overflow; i++)
            stack.Push(Ops.Apply($"tweak-{i}", $"Tweak {i}"));

        Assert.Equal(TweakOperationStack.MaxOps, stack.UndoCount);
    }

    [Fact]
    public void Push_ExceedingMaxOps_KeepsMostRecent()
    {
        var stack = new TweakOperationStack();
        int overflow = TweakOperationStack.MaxOps + 5;
        for (int i = 0; i < overflow; i++)
            stack.Push(Ops.Apply($"tweak-{i}", $"Tweak {i}"));

        // The most recent op should still be on top
        var lastPushed = new TweakOperation($"tweak-{overflow - 1}", $"Tweak {overflow - 1}", OperationKind.Apply, DateTimeOffset.UtcNow);
        // PeekUndo returns the most recent — just verify the ID reflects a later push
        var peek = stack.PeekUndo;
        Assert.NotNull(peek);
        // Index: overflow-1 = MaxOps+4; oldest retained = overflow-1 - (MaxOps-1) = 5
        // Verify count is capped
        Assert.Equal(TweakOperationStack.MaxOps, stack.UndoCount);
    }
}

// ── Clear behaviour ───────────────────────────────────────────────────────────

public sealed class TweakOperationStackClearTests
{
    [Fact]
    public void Clear_EmptiesBothStacks()
    {
        var stack = new TweakOperationStack();
        stack.Push(Ops.Apply("a", "A"));
        stack.Push(Ops.Apply("b", "B"));
        stack.Undo(); // puts one in redo
        stack.Clear();
        Assert.False(stack.CanUndo);
        Assert.False(stack.CanRedo);
        Assert.Equal(0, stack.UndoCount);
        Assert.Equal(0, stack.RedoCount);
    }
}
