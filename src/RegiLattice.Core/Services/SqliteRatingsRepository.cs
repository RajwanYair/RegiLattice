// RegiLattice.Core — Services/SqliteRatingsRepository.cs
// SQLite-backed IRatingsRepository implementation (C.1 — SQLite data backend).
//
// Schema: "ratings" table keyed by tweak_id with star and note columns.
// Thread-safety: all mutations are serialized via _lock + WAL journal mode.

using Microsoft.Data.Sqlite;

namespace RegiLattice.Core.Services;

/// <summary>
/// <see cref="IRatingsRepository"/> backed by a SQLite database.
/// The database file defaults to <c>%LOCALAPPDATA%\RegiLattice\regilattice.db</c>;
/// a custom path may be supplied for testing or portable mode.
/// </summary>
public sealed class SqliteRatingsRepository : IRatingsRepository
{
    private readonly string _dbPath;
    private readonly object _lock = new();

    // ── Construction ──────────────────────────────────────────────────────

    /// <summary>Creates a repository backed by the default shared database.</summary>
    public SqliteRatingsRepository()
        : this(Path.Combine(AppConfig.ConfigDir, "regilattice.db")) { }

    /// <summary>Creates a repository backed by a custom database file.</summary>
    public SqliteRatingsRepository(string dbPath)
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
            CREATE TABLE IF NOT EXISTS ratings (
                tweak_id TEXT NOT NULL PRIMARY KEY,
                stars     INTEGER NOT NULL CHECK(stars BETWEEN 1 AND 5),
                note      TEXT    NOT NULL DEFAULT ''
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

    // ── IRatingsRepository ────────────────────────────────────────────────

    /// <inheritdoc/>
    public void Rate(string tweakId, int stars, string note = "")
    {
        if (stars < 1 || stars > 5)
            throw new ArgumentOutOfRangeException(nameof(stars), "Stars must be 1–5.");
        lock (_lock)
        {
            using var con = OpenConnection();
            using var cmd = con.CreateCommand();
            cmd.CommandText = """
                INSERT INTO ratings (tweak_id, stars, note)
                VALUES (@id, @stars, @note)
                ON CONFLICT(tweak_id) DO UPDATE SET stars = excluded.stars, note = excluded.note;
                """;
            cmd.Parameters.AddWithValue("@id", tweakId);
            cmd.Parameters.AddWithValue("@stars", stars);
            cmd.Parameters.AddWithValue("@note", note ?? "");
            cmd.ExecuteNonQuery();
        }
    }

    /// <inheritdoc/>
    public TweakRating? GetRating(string tweakId)
    {
        lock (_lock)
        {
            using var con = OpenConnection();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT stars, note FROM ratings WHERE tweak_id = @id;";
            cmd.Parameters.AddWithValue("@id", tweakId ?? "");
            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;
            return new TweakRating { Stars = reader.GetInt32(0), Note = reader.GetString(1) };
        }
    }

    /// <inheritdoc/>
    public IReadOnlyDictionary<string, TweakRating> AllRatings()
    {
        lock (_lock)
        {
            using var con = OpenConnection();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT tweak_id, stars, note FROM ratings;";
            using var reader = cmd.ExecuteReader();
            var dict = new Dictionary<string, TweakRating>(StringComparer.OrdinalIgnoreCase);
            while (reader.Read())
                dict[reader.GetString(0)] = new TweakRating { Stars = reader.GetInt32(1), Note = reader.GetString(2) };
            return dict;
        }
    }

    /// <inheritdoc/>
    public void RemoveRating(string tweakId)
    {
        lock (_lock)
        {
            using var con = OpenConnection();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "DELETE FROM ratings WHERE tweak_id = @id;";
            cmd.Parameters.AddWithValue("@id", tweakId ?? "");
            cmd.ExecuteNonQuery();
        }
    }

    /// <inheritdoc/>
    public IReadOnlyList<(string Id, TweakRating Rating)> TopRated(int n = 10)
    {
        lock (_lock)
        {
            using var con = OpenConnection();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT tweak_id, stars, note FROM ratings ORDER BY stars DESC LIMIT @n;";
            cmd.Parameters.AddWithValue("@n", n);
            using var reader = cmd.ExecuteReader();
            var list = new List<(string, TweakRating)>();
            while (reader.Read())
                list.Add((reader.GetString(0), new TweakRating { Stars = reader.GetInt32(1), Note = reader.GetString(2) }));
            return list.AsReadOnly();
        }
    }

    /// <inheritdoc/>
    public double? AverageRating()
    {
        lock (_lock)
        {
            using var con = OpenConnection();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT AVG(CAST(stars AS REAL)) FROM ratings;";
            var result = cmd.ExecuteScalar();
            return result is DBNull or null ? null : (double)result;
        }
    }

    /// <inheritdoc/>
    public void Flush()
    {
        // No-op: SQLite writes are immediate (WAL mode).
    }
}
