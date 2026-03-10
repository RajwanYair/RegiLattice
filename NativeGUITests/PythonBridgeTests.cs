using RegiLattice.Native;
using Xunit;

namespace NativeGUITests;

/// <summary>Unit tests for PythonBridge helpers (no subprocess, no I/O).</summary>
public sealed class PythonBridgeTests
{
    // ── QuoteArg (via reflection since it's private — test indirectly) ─────
    // The easiest approach: test FindPython returns a non-empty path when
    // Python is available, or throws a well-formed exception when it isn't.

    [Fact]
    public void FindPython_ReturnsStringOrThrowsInvalidOp()
    {
        // On a CI machine without Python, the code throws InvalidOperationException.
        // On a dev machine with Python, it returns a non-empty, well-formed path.
        try
        {
            string path = PythonBridge.FindPython();
            Assert.False(string.IsNullOrWhiteSpace(path));
            Assert.EndsWith("python.exe", path, StringComparison.OrdinalIgnoreCase);
        }
        catch (InvalidOperationException ex)
        {
            Assert.Contains("Python", ex.Message);
        }
    }

    [Fact]
    public void Constructor_NullPath_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new PythonBridge(null!));
    }

    [Fact]
    public void Constructor_ValidPath_DoesNotThrow()
    {
        var bridge = new PythonBridge("python.exe");
        Assert.NotNull(bridge);
        bridge.Dispose();
    }

    [Fact]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        var bridge = new PythonBridge("python.exe");
        bridge.Dispose();
        bridge.Dispose();   // must not throw
    }
}
