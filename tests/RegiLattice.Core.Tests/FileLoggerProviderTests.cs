// RegiLattice.Core.Tests — FileLoggerProviderTests.cs
// Tests for FileLoggerProvider (B.4 — file logging sink).

using Microsoft.Extensions.Logging;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

public sealed class FileLoggerProviderTests : IDisposable
{
    private readonly string _logDir;
    private readonly FileLoggerProvider _provider;

    public FileLoggerProviderTests()
    {
        _logDir = Path.Combine(Path.GetTempPath(), $"rl-log-tests-{Guid.NewGuid():N}");
        _provider = new FileLoggerProvider(_logDir, LogLevel.Debug);
    }

    public void Dispose()
    {
        _provider.Dispose();
        if (Directory.Exists(_logDir))
            Directory.Delete(_logDir, recursive: true);
    }

    // ── Construction ────────────────────────────────────────────────────────

    [Fact]
    public void FileLoggerProvider_ImplementsILoggerProvider()
    {
        Assert.IsAssignableFrom<ILoggerProvider>(_provider);
    }

    [Fact]
    public void CurrentLogFilePath_ContainsTodaysDate()
    {
        var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
        Assert.Contains(today, _provider.CurrentLogFilePath);
    }

    [Fact]
    public void CurrentLogFilePath_ContainsLogDirectory()
    {
        Assert.StartsWith(_logDir, _provider.CurrentLogFilePath,
            StringComparison.OrdinalIgnoreCase);
    }

    // ── Logger creation ──────────────────────────────────────────────────────

    [Fact]
    public void CreateLogger_ReturnsNonNull()
    {
        var logger = _provider.CreateLogger("TestCategory");
        Assert.NotNull(logger);
    }

    [Fact]
    public void Logger_IsEnabled_TrueForMinLevelAndAbove()
    {
        var logger = _provider.CreateLogger("Cat");
        Assert.True(logger.IsEnabled(LogLevel.Debug));
        Assert.True(logger.IsEnabled(LogLevel.Information));
        Assert.True(logger.IsEnabled(LogLevel.Warning));
        Assert.True(logger.IsEnabled(LogLevel.Error));
        Assert.True(logger.IsEnabled(LogLevel.Critical));
    }

    [Fact]
    public void Logger_IsEnabled_FalseForTrace_WhenMinLevelIsDebug()
    {
        var logger = _provider.CreateLogger("Cat");
        Assert.False(logger.IsEnabled(LogLevel.Trace));
    }

    // ── Write to file ────────────────────────────────────────────────────────

    [Fact]
    public void Log_Information_WritesToFile()
    {
        var logger = _provider.CreateLogger("TestCat");
        logger.LogInformation("Hello from test");
        Assert.True(File.Exists(_provider.CurrentLogFilePath));
        var content = File.ReadAllText(_provider.CurrentLogFilePath);
        Assert.Contains("Hello from test", content);
    }

    [Fact]
    public void Log_ContainsCategoryName()
    {
        var logger = _provider.CreateLogger("MyTestCategory");
        logger.LogWarning("CategoryTest");
        var content = File.ReadAllText(_provider.CurrentLogFilePath);
        Assert.Contains("MyTestCategory", content);
    }

    [Fact]
    public void Log_ContainsTimestamp()
    {
        var logger = _provider.CreateLogger("Cat");
        logger.LogDebug("TimeCheck");
        var content = File.ReadAllText(_provider.CurrentLogFilePath);
        // Pattern: YYYY-MM-DD HH
        var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
        Assert.Contains(today, content);
    }

    [Fact]
    public void Log_ContainsLevelIndicator()
    {
        var logger = _provider.CreateLogger("Cat");
        logger.LogError("ErrorMsg");
        var content = File.ReadAllText(_provider.CurrentLogFilePath);
        Assert.Contains("[ERR]", content);
    }

    // ── Exception logging ────────────────────────────────────────────────────

    [Fact]
    public void Log_WithException_IncludesExceptionInfo()
    {
        var logger = _provider.CreateLogger("Cat");
        try { throw new InvalidOperationException("test ex"); }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "An error occurred");
        }
        var content = File.ReadAllText(_provider.CurrentLogFilePath);
        Assert.Contains("InvalidOperationException", content);
    }

    // ── Dispose guard ────────────────────────────────────────────────────────

    [Fact]
    public void Write_AfterDispose_DoesNotThrow()
    {
        var provider = new FileLoggerProvider(_logDir, LogLevel.Debug);
        provider.Dispose();
        // Should silently no-op, not throw
        var ex = Record.Exception(() => provider.Write("Cat", LogLevel.Information, "msg", null));
        Assert.Null(ex);
    }
}
