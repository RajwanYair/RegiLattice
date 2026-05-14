// RegiLattice.Core — Services/SqliteHistoryRepository.cs
// SQLite-backed IHistoryRepository implementation (C.1 — SQLite data backend).
//
// Schema: "history" table with columns matching HistoryEntry fields.
// Rows are capped at MaxEntries by deleting the oldest when the limit is exceeded.
// Thread-safety: all mutations are serialized via _lock + WAL journal mode.

using Microsoft.Data.Sqlite;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Services;

/// <summary>
/// <see cref="IHistoryRepository"/> backed by a SQLite database.
/// The database file defaults to <c>%LOCALAPPDATA%\RegiLattice\regilattice.db</c>;
/// a custom path may be supplied for testing or portable mode.
/// Keeps at most <see cref="MaxEntries"/> entries; oldest are dropped when the cap is reached.
/// </summary>
public sealed class SqliteHistoryRepository : IHistoryRepository
{
    /// <summary>Maximum entries to retain (mirrors <see cref="JsonHistoryRepository.MaxEntries"/>).</summary>
    public const int MaxEntries = 500;

    private readonly string _dbPath;
    private readonly object _lock = new();
    private readonly string _sessionId = Guid.NewGuid().ToString("N")[..8];

    // ── Construction ──────────────────────────────────────────────────────

    /// <summary>Creates a repository backed by the default shared database.</summary>
    public SqliteHistoryRepository()
        : this(Path.Combine(AppConfig.ConfigDir, "regilattice.db")) { }

    /// <summary>Creates a repository backed by a custom database file.</summary>
    public SqliteHistoryRepository(string dbPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dbPath);
        _dbPath = dbPath;
        EnsureSchema();
    }

    // ── Schema ────────────────────────────────────────────────────────────

    private void EnsureSchema()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_dbPath) ?? ".");
        using var con = OpenConnection();
        using var cmd = con.CreateCommand();
        cmd.CommandText = """
            PRAGMA journal_mode=WAL;
            CREATE TABLE IF NOT EXISTS history (
                rowid       INTEGER PRIMARY KEY AUTOINCREMENT,
                tweak_id    TEXT    NOT NULL,
                action      TEXT    NOT NULL,
                result      TEXT    NOT NULL,
                timestamp   TEXT    NOT NULL,
                username    TEXT,
                machine     TEXT,
                session_id  TEXT
            );
            CREATE INDEX IF NOT EXISTS idx_history_tweak ON history(tweak_id);
            """;
        cmd.ExecuteNonQuery();
    }

    private SqliteConnection OpenConnection()
    {
        var con = new SqliteConnection($"Data Source={_dbPath}");
        con.Open();
        return con;
    }

    // ── IHistoryRepository ────────────────────────────────────────────────

    /// <inheritdoc/>
    public void RecordApply(string tweakId, TweakResult result) =>
        Record(tweakId, "apply", result.ToString());

    /// <inheritdoc/>
    public void RecordRemove(string tweakId, TweakResult result) =>
        Record(tweakId, "remove", result.ToString());

    /// <inheritdoc/>
    public void RecordUpdate(string tweakId, TweakResult result) =>
        Record(tweakId, "update", result.ToString());

    /// <inheritdoc/>
    public void Record(string tweakId, string action, string result)
    {
        lock (_lock)
        {
            using var con = OpenConnection();
            using var txn = con.BeginTransaction();

            using (var ins = con.CreateCommand())
            {
                ins.Transaction = txn;
                ins.CommandText = """
                    INSERT INTO history (tweak_id, action, result, timestamp, username, machine, session_id)
                    VALUES (@tid, @act, @res, @ts, @usr, @mac, @sid);
                    """;
                ins.Parameters.AddWithValue("@tid", tweakId);
                ins.Parameters.AddWithValue("@act", action);
                ins.Parameters.AddWithValue("@res", result);
                ins.Parameters.AddWithValue("@ts", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                ins.Parameters.AddWithValue("@usr", (object?)Environment.UserName ?? DBNull.Value);
                ins.Parameters.AddWithValue("@mac", (object?)Environment.MachineName ?? DBNull.Value);
                ins.Parameters.AddWithValue("@sid", (object?)_sessionId ?? DBNull.Value);
                ins.ExecuteNonQuery();
            }

            // Trim to MaxEntries
            using (var trim = con.CreateCommand())
            {
                trim.Transaction = txn;
                trim.CommandText = $"""
                    DELETE FROM history WHERE rowid IN (
                        SELECT rowid FROM history ORDER BY rowid ASC LIMIT MAX(0, (SELECT COUNT(1) FROM history) - {MaxEntries})
                    );
                    """;
                trim.ExecuteNonQuery();
            }

            txn.Commit();
        }
    }

    /// <inheritdoc/>
    public IReadOnlyList<HistoryEntry> Recent(int count = 20)
    {
        lock (_lock)
        {
            using var con = OpenConnection();
            using var cmd = con.CreateCommand();
            cmd.CommandText = """
                SELECT tweak_id, action, result, timestamp, username, machine, session_id
                FROM history
                ORDER BY rowid DESC
                LIMIT @cnt;
                """;
            cmd.Parameters.AddWithValue("@cnt", count);
            return ReadEntries(cmd);
        }
    }

    /// <inheritdoc/>
    public IReadOnlyList<HistoryEntry> ForTweak(string tweakId)
    {
        lock (_lock)
        {
            using var con = OpenConnection();
            using var cmd = con.CreateCommand();
            cmd.CommandText = """
                SELECT tweak_id, action, result, timestamp, username, machine, session_id
                FROM history
                WHERE tweak_id = @tid
                ORDER BY rowid DESC;
                """;
            cmd.Parameters.AddWithValue("@tid", tweakId);
            return ReadEntries(cmd);
        }
    }

    /// <inheritdoc/>
    public void Clear()
    {
        lock (_lock)
        {
            using var con = OpenConnection();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "DELETE FROM history;";
            cmd.ExecuteNonQuery();
        }
    }

    /// <inheritdoc/>
    public int Count
    {
        get
        {
            lock (_lock)
            {
                using var con = OpenConnection();
                using var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT COUNT(1) FROM history;";
                return (int)(long)(cmd.ExecuteScalar() ?? 0L);
            }
        }
    }

    /// <inheritdoc/>
    public void Flush()
    {
        // No-op: SQLite writes are immediate (WAL mode).
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    private static IReadOnlyList<HistoryEntry> ReadEntries(SqliteCommand cmd)
    {
        using var reader = cmd.ExecuteReader();
        var list = new List<HistoryEntry>();
        while (reader.Read())
        {
            list.Add(new HistoryEntry
            {
                TweakId = reader.GetString(0),
                Action = reader.GetString(1),
                Result = reader.GetString(2),
                Timestamp = reader.GetString(3),
                Username = reader.IsDBNull(4) ? null : reader.GetString(4),
                MachineName = reader.IsDBNull(5) ? null : reader.GetString(5),
                SessionId = reader.IsDBNull(6) ? null : reader.GetString(6),
            });
        }
        return list.AsReadOnly();
    }
}
