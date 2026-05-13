// RegiLattice.Core — Services/JsonRatingsRepository.cs
// JSON-backed IRatingsRepository implementation (B.3 — decouple Ratings from file I/O).

using System.Text.Json;

namespace RegiLattice.Core.Services;

/// <summary>
/// <see cref="IRatingsRepository"/> backed by a JSON file on disk.
/// Defaults to the user-scoped path used by the static <see cref="Ratings"/> helper;
/// can be constructed with a custom path for testing or multi-profile scenarios.
/// </summary>
public sealed class JsonRatingsRepository : IRatingsRepository
{
    private readonly string _filePath;
    private readonly object _lock = new();
    private Dictionary<string, TweakRating>? _cache;
    private bool _dirty;

    /// <summary>
    /// Creates a repository backed by the default ratings file
    /// (<c>%LOCALAPPDATA%\RegiLattice\ratings.json</c>).
    /// </summary>
    public JsonRatingsRepository()
        : this(Path.Combine(AppConfig.ConfigDir, "ratings.json")) { }

    /// <summary>Creates a repository backed by a custom file path (useful in tests).</summary>
    public JsonRatingsRepository(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        _filePath = filePath;
    }

    /// <inheritdoc/>
    public void Rate(string tweakId, int stars, string note = "")
    {
        if (stars < 1 || stars > 5)
            throw new ArgumentOutOfRangeException(nameof(stars), "Stars must be 1-5.");
        lock (_lock)
        {
            var dict = LoadDict();
            dict[tweakId] = new TweakRating { Stars = stars, Note = note };
            _dirty = true;
        }
    }

    /// <inheritdoc/>
    public TweakRating? GetRating(string tweakId)
    {
        lock (_lock)
        {
            return LoadDict().GetValueOrDefault(tweakId);
        }
    }

    /// <inheritdoc/>
    public IReadOnlyDictionary<string, TweakRating> AllRatings()
    {
        lock (_lock)
        {
            return new Dictionary<string, TweakRating>(LoadDict());
        }
    }

    /// <inheritdoc/>
    public void RemoveRating(string tweakId)
    {
        lock (_lock)
        {
            var dict = LoadDict();
            if (dict.Remove(tweakId))
                _dirty = true;
        }
    }

    /// <inheritdoc/>
    public IReadOnlyList<(string Id, TweakRating Rating)> TopRated(int n = 10)
    {
        lock (_lock)
        {
            return LoadDict()
                .OrderByDescending(kv => kv.Value.Stars)
                .Take(n)
                .Select(kv => (kv.Key, kv.Value))
                .ToList();
        }
    }

    /// <inheritdoc/>
    public double? AverageRating()
    {
        lock (_lock)
        {
            var dict = LoadDict();
            return dict.Count == 0 ? null : dict.Values.Average(r => r.Stars);
        }
    }

    /// <inheritdoc/>
    public void Flush()
    {
        lock (_lock)
        {
            if (_dirty && _cache is not null)
                PersistToFile(_cache);
            _dirty = false;
        }
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private Dictionary<string, TweakRating> LoadDict()
    {
        if (_cache is not null)
            return _cache;

        if (!File.Exists(_filePath))
        {
            _cache = [];
            return _cache;
        }

        try
        {
            var json = File.ReadAllText(_filePath);
            _cache = JsonSerializer.Deserialize<Dictionary<string, TweakRating>>(json) ?? [];
        }
        catch (Exception ex) when (ex is JsonException or IOException)
        {
            _cache = [];
        }

        return _cache;
    }

    private void PersistToFile(Dictionary<string, TweakRating> data)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        var json = JsonSerializer.Serialize(data, JsonOptions.Indented);
        const int maxRetries = 5;
        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                File.WriteAllText(_filePath, json);
                return;
            }
            catch (IOException) when (attempt < maxRetries - 1)
            {
                Thread.Sleep(60 * (attempt + 1));
            }
        }
    }
}
