// RegiLattice.Core — Services/JsonFavoritesRepository.cs
// JSON-backed IFavoritesRepository implementation (B.3 — decouple Favorites from file I/O).

using System.Text.Json;

namespace RegiLattice.Core.Services;

/// <summary>
/// <see cref="IFavoritesRepository"/> backed by a JSON file on disk.
/// Defaults to the user-scoped path used by the static <see cref="Favorites"/> helper;
/// can be constructed with a custom path for testing or multi-profile scenarios.
/// </summary>
public sealed class JsonFavoritesRepository : IFavoritesRepository
{
    private readonly string _filePath;
    private readonly object _lock = new();
    private HashSet<string>? _cache;
    private bool _dirty;

    /// <summary>
    /// Creates a repository backed by the default favorites file
    /// (<c>%LOCALAPPDATA%\RegiLattice\favorites.json</c>).
    /// </summary>
    public JsonFavoritesRepository()
        : this(Path.Combine(AppConfig.ConfigDir, "favorites.json")) { }

    /// <summary>Creates a repository backed by a custom file path (useful in tests).</summary>
    public JsonFavoritesRepository(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        _filePath = filePath;
    }

    /// <inheritdoc/>
    public void Add(string tweakId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tweakId);
        lock (_lock)
        {
            if (LoadSet().Add(tweakId))
                _dirty = true;
        }
    }

    /// <inheritdoc/>
    public void Remove(string tweakId)
    {
        lock (_lock)
        {
            if (LoadSet().Remove(tweakId))
                _dirty = true;
        }
    }

    /// <inheritdoc/>
    public bool Toggle(string tweakId)
    {
        lock (_lock)
        {
            var set = LoadSet();
            if (set.Contains(tweakId))
            {
                set.Remove(tweakId);
                _dirty = true;
                return false;
            }
            set.Add(tweakId);
            _dirty = true;
            return true;
        }
    }

    /// <inheritdoc/>
    public bool IsFavorite(string tweakId)
    {
        lock (_lock)
            return LoadSet().Contains(tweakId);
    }

    /// <inheritdoc/>
    public IReadOnlyList<string> All()
    {
        lock (_lock)
            return LoadSet().OrderBy(id => id, StringComparer.OrdinalIgnoreCase).ToList();
    }

    /// <inheritdoc/>
    public int Count
    {
        get
        {
            lock (_lock)
                return LoadSet().Count;
        }
    }

    /// <inheritdoc/>
    public void Flush()
    {
        lock (_lock)
        {
            if (!_dirty || _cache is null)
                return;
            Save(_cache);
            _dirty = false;
        }
    }

    // ── Internals ────────────────────────────────────────────────────────

    private HashSet<string> LoadSet()
    {
        if (_cache is not null)
            return _cache;

        if (!File.Exists(_filePath))
        {
            _cache = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            return _cache;
        }

        try
        {
            string json = File.ReadAllText(_filePath);
            var list = JsonSerializer.Deserialize<List<string>>(json) ?? [];
            _cache = new HashSet<string>(list, StringComparer.OrdinalIgnoreCase);
        }
        catch
        {
            _cache = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }
        return _cache;
    }

    private void Save(HashSet<string> set)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath) ?? ".");
        string json = JsonSerializer.Serialize(set.OrderBy(id => id, StringComparer.OrdinalIgnoreCase).ToList(), JsonOptions.Indented);
        File.WriteAllText(_filePath, json);
    }
}
