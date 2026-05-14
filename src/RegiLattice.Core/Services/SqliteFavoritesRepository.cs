// RegiLattice.Core — Services/SqliteFavoritesRepository.cs
// SQLite-backed IFavoritesRepository implementation (C.1 — SQLite data backend).
//
// Schema: single table "favorites" with a text primary-key column "tweak_id".
// Thread-safety: all mutations are serialized via _lock + WAL journal mode.

using Microsoft.Data.Sqlite;

namespace RegiLattice.Core.Services;

/// <summary>
/// <see cref="IFavoritesRepository"/> backed by a SQLite database.
/// The database file defaults to <c>%LOCALAPPDATA%\RegiLattice\regilattice.db</c>;
/// a custom path may be supplied for testing or portable mode.
/// </summary>
public sealed class SqliteFavoritesRepository : IFavoritesRepository
{
    private readonly string _dbPath;
    private readonly object _lock = new();

    // ── Construction ──────────────────────────────────────────────────────

    /// <summary>Creates a repository backed by the default shared database.</summary>
    public SqliteFavoritesRepository()
        : this(Path.Combine(AppConfig.ConfigDir, "regilattice.db")) { }

    /// <summary>Creates a repository backed by a custom database file.</summary>
    public SqliteFavoritesRepository(string dbPath)
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
            CREATE TABLE IF NOT EXISTS favorites (
                tweak_id TEXT NOT NULL PRIMARY KEY
            );
            """;
        cmd.ExecuteNonQuery();
    }

    private SqliteConnection OpenConnection()
    {
        var con = new SqliteConnection($"Data Source={_dbPath}");
        con.Open();
        return con;
    }

    // ── IFavoritesRepository ──────────────────────────────────────────────

    /// <inheritdoc/>
    public void Add(string tweakId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tweakId);
        lock (_lock)
        {
            using var con = OpenConnection();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "INSERT OR IGNORE INTO favorites (tweak_id) VALUES (@id);";
            cmd.Parameters.AddWithValue("@id", tweakId);
            cmd.ExecuteNonQuery();
        }
    }

    /// <inheritdoc/>
    public void Remove(string tweakId)
    {
        lock (_lock)
        {
            using var con = OpenConnection();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "DELETE FROM favorites WHERE tweak_id = @id;";
            cmd.Parameters.AddWithValue("@id", tweakId ?? "");
            cmd.ExecuteNonQuery();
        }
    }

    /// <inheritdoc/>
    public bool Toggle(string tweakId)
    {
        lock (_lock)
        {
            using var con = OpenConnection();

            using (var check = con.CreateCommand())
            {
                check.CommandText = "SELECT COUNT(1) FROM favorites WHERE tweak_id = @id;";
                check.Parameters.AddWithValue("@id", tweakId);
                long count = (long)(check.ExecuteScalar() ?? 0L);
                if (count > 0)
                {
                    using var del = con.CreateCommand();
                    del.CommandText = "DELETE FROM favorites WHERE tweak_id = @id;";
                    del.Parameters.AddWithValue("@id", tweakId);
                    del.ExecuteNonQuery();
                    return false;
                }
            }

            using var ins = con.CreateCommand();
            ins.CommandText = "INSERT OR IGNORE INTO favorites (tweak_id) VALUES (@id);";
            ins.Parameters.AddWithValue("@id", tweakId);
            ins.ExecuteNonQuery();
            return true;
        }
    }

    /// <inheritdoc/>
    public bool IsFavorite(string tweakId)
    {
        lock (_lock)
        {
            using var con = OpenConnection();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT COUNT(1) FROM favorites WHERE tweak_id = @id;";
            cmd.Parameters.AddWithValue("@id", tweakId ?? "");
            return (long)(cmd.ExecuteScalar() ?? 0L) > 0;
        }
    }

    /// <inheritdoc/>
    public IReadOnlyList<string> All()
    {
        lock (_lock)
        {
            using var con = OpenConnection();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT tweak_id FROM favorites ORDER BY tweak_id;";
            using var reader = cmd.ExecuteReader();
            var list = new List<string>();
            while (reader.Read())
                list.Add(reader.GetString(0));
            return list.AsReadOnly();
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
                cmd.CommandText = "SELECT COUNT(1) FROM favorites;";
                return (int)(long)(cmd.ExecuteScalar() ?? 0L);
            }
        }
    }

    /// <summary>Remove all favorites.</summary>
    public void Clear()
    {
        lock (_lock)
        {
            using var con = OpenConnection();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "DELETE FROM favorites;";
            cmd.ExecuteNonQuery();
        }
    }

    /// <inheritdoc/>
    public void Flush()
    {
        // No-op: SQLite writes are immediate (WAL mode).
    }
}
