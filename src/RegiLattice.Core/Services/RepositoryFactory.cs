// RegiLattice.Core — Services/RepositoryFactory.cs
// Factory that creates IFavoritesRepository, IHistoryRepository, and IRatingsRepository
// backed by either JSON (default) or SQLite, depending on AppConfig.DataBackend (C.1).

namespace RegiLattice.Core.Services;

/// <summary>
/// Creates repository instances for the configured data backend.
/// Call <see cref="CreateFavorites"/>, <see cref="CreateHistory"/>, and
/// <see cref="CreateRatings"/> to obtain backend-specific implementations
/// wired to the same database or directory as chosen by <see cref="AppConfig.DataBackend"/>.
/// </summary>
public static class RepositoryFactory
{
    // ── Public factory methods ─────────────────────────────────────────────

    /// <summary>
    /// Creates an <see cref="IFavoritesRepository"/> for the given config.
    /// Returns a <see cref="SqliteFavoritesRepository"/> when
    /// <see cref="AppConfig.DataBackend"/> is <c>"sqlite"</c>,
    /// otherwise a <see cref="JsonFavoritesRepository"/>.
    /// </summary>
    public static IFavoritesRepository CreateFavorites(AppConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);
        return config.DataBackend.Equals("sqlite", StringComparison.OrdinalIgnoreCase)
            ? new SqliteFavoritesRepository(DbPath(config))
            : new JsonFavoritesRepository();
    }

    /// <summary>
    /// Creates an <see cref="IHistoryRepository"/> for the given config.
    /// Returns a <see cref="SqliteHistoryRepository"/> when
    /// <see cref="AppConfig.DataBackend"/> is <c>"sqlite"</c>,
    /// otherwise a <see cref="JsonHistoryRepository"/>.
    /// </summary>
    public static IHistoryRepository CreateHistory(AppConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);
        return config.DataBackend.Equals("sqlite", StringComparison.OrdinalIgnoreCase)
            ? new SqliteHistoryRepository(DbPath(config))
            : new JsonHistoryRepository();
    }

    /// <summary>
    /// Creates an <see cref="IRatingsRepository"/> for the given config.
    /// Returns a <see cref="SqliteRatingsRepository"/> when
    /// <see cref="AppConfig.DataBackend"/> is <c>"sqlite"</c>,
    /// otherwise a <see cref="JsonRatingsRepository"/>.
    /// </summary>
    public static IRatingsRepository CreateRatings(AppConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);
        return config.DataBackend.Equals("sqlite", StringComparison.OrdinalIgnoreCase)
            ? new SqliteRatingsRepository(DbPath(config))
            : new JsonRatingsRepository();
    }

    // ── Internal helpers ───────────────────────────────────────────────────

    private static string DbPath(AppConfig _) =>
        Path.Combine(AppConfig.ConfigDir, "regilattice.db");
}
