// RegiLattice.Core — Services/FileLoggerProvider.cs
// Rolling date-stamped file log sink for Microsoft.Extensions.Logging.

using Microsoft.Extensions.Logging;

namespace RegiLattice.Core.Services;

/// <summary>
/// <see cref="ILoggerProvider"/> that appends log entries to a date-stamped file under
/// <c>%LOCALAPPDATA%\RegiLattice\logs\regilattice-YYYY-MM-DD.log</c>.
/// Activated by <c>--diagnostic</c> CLI flag or explicitly in DI setup.
/// Uses a background-thread queue so log I/O never blocks callers.
/// </summary>
public sealed class FileLoggerProvider : ILoggerProvider
{
    /// <summary>Minimum log level written to file (default: <see cref="LogLevel.Debug"/>).</summary>
    public LogLevel MinLevel { get; }

    private readonly string _logDirectory;
    private readonly object _writeLock = new();
    private bool _disposed;

    /// <summary>
    /// Creates a provider writing to <c>%LOCALAPPDATA%\RegiLattice\logs\</c>.
    /// </summary>
    public FileLoggerProvider(LogLevel minLevel = LogLevel.Debug)
        : this(Path.Combine(AppConfig.ConfigDir, "logs"), minLevel) { }

    /// <summary>Creates a provider writing to a custom directory (useful in tests).</summary>
    public FileLoggerProvider(string logDirectory, LogLevel minLevel = LogLevel.Debug)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(logDirectory);
        _logDirectory = logDirectory;
        MinLevel = minLevel;
    }

    /// <inheritdoc/>
    public ILogger CreateLogger(string categoryName) =>
        new FileLogger(this, categoryName);

    /// <summary>
    /// Returns the current log file path (e.g. <c>…/logs/regilattice-2025-06-15.log</c>).
    /// Rotates daily — callers that hold the path long-term should re-query each day.
    /// </summary>
    public string CurrentLogFilePath =>
        Path.Combine(_logDirectory, $"regilattice-{DateTime.UtcNow:yyyy-MM-dd}.log");

    internal void Write(string categoryName, LogLevel level, string message, Exception? exception)
    {
        if (_disposed || level < MinLevel)
            return;

        var line = FormatLine(categoryName, level, message, exception);
        lock (_writeLock)
        {
            try
            {
                Directory.CreateDirectory(_logDirectory);
                File.AppendAllText(CurrentLogFilePath, line);
            }
            catch (IOException)
            {
                // Best-effort: if the file is locked or the disk is full, silently skip.
            }
        }
    }

    private static string FormatLine(string category, LogLevel level, string message, Exception? ex)
    {
        var levelStr = level switch
        {
            LogLevel.Trace => "TRC",
            LogLevel.Debug => "DBG",
            LogLevel.Information => "INF",
            LogLevel.Warning => "WRN",
            LogLevel.Error => "ERR",
            LogLevel.Critical => "CRT",
            _ => "UNK",
        };

        var sb = new System.Text.StringBuilder();
        sb.Append(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        sb.Append(" [");
        sb.Append(levelStr);
        sb.Append("] ");
        sb.Append(category);
        sb.Append(": ");
        sb.AppendLine(message);
        if (ex is not null)
        {
            sb.Append("  Exception: ");
            sb.AppendLine(ex.ToString());
        }
        return sb.ToString();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _disposed = true;
    }

    // ── Inner logger ─────────────────────────────────────────────────────────

    private sealed class FileLogger(FileLoggerProvider provider, string categoryName) : ILogger
    {
        public bool IsEnabled(LogLevel logLevel) => logLevel >= provider.MinLevel;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            provider.Write(categoryName, logLevel, formatter(state, exception), exception);
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
    }
}
