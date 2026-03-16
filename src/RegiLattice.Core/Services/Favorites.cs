using System.Text.Json;

namespace RegiLattice.Core.Services;

/// <summary>
/// Persists user's favorite (starred/bookmarked) tweak IDs.
/// Stored as a JSON array in %LOCALAPPDATA%\RegiLattice\favorites.json.
/// </summary>
public static class Favorites
{
    private static readonly string FilePath = Path.Combine(AppConfig.ConfigDir, "favorites.json");
    private static readonly object Lock = new();
    private static HashSet<string>? _cache;
    private static bool _dirty;

    /// <summary>Add a tweak ID to favorites.</summary>
    public static void Add(string tweakId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tweakId);
        lock (Lock)
        {
            var set = LoadSet();
            if (set.Add(tweakId))
                _dirty = true;
        }
    }

    /// <summary>Remove a tweak ID from favorites.</summary>
    public static void Remove(string tweakId)
    {
        lock (Lock)
        {
            var set = LoadSet();
            if (set.Remove(tweakId))
                _dirty = true;
        }
    }

    /// <summary>Toggle a tweak — add if not present, remove if present. Returns true if now favorited.</summary>
    public static bool Toggle(string tweakId)
    {
        lock (Lock)
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

    /// <summary>Check if a tweak is favorited.</summary>
    public static bool IsFavorite(string tweakId)
    {
        lock (Lock)
            return LoadSet().Contains(tweakId);
    }

    /// <summary>Get all favorite tweak IDs.</summary>
    public static IReadOnlyList<string> All()
    {
        lock (Lock)
            return LoadSet().OrderBy(id => id, StringComparer.OrdinalIgnoreCase).ToList();
    }

    /// <summary>Get the count of favorites.</summary>
    public static int Count
    {
        get
        {
            lock (Lock)
                return LoadSet().Count;
        }
    }

    /// <summary>Write pending changes to disk.</summary>
    public static void Flush()
    {
        lock (Lock)
        {
            if (!_dirty || _cache is null)
                return;
            Save(_cache);
            _dirty = false;
        }
    }

    /// <summary>Clear all favorites.</summary>
    public static void Clear()
    {
        lock (Lock)
        {
            _cache = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _dirty = true;
        }
    }

    /// <summary>Reset in-memory cache (for testing).</summary>
    public static void Reset()
    {
        lock (Lock)
        {
            _cache = null;
            _dirty = false;
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }

    private static HashSet<string> LoadSet()
    {
        if (_cache is not null)
            return _cache;

        if (!File.Exists(FilePath))
        {
            _cache = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            return _cache;
        }

        try
        {
            var json = File.ReadAllText(FilePath);
            var list = JsonSerializer.Deserialize<List<string>>(json) ?? [];
            _cache = new HashSet<string>(list, StringComparer.OrdinalIgnoreCase);
        }
        catch
        {
            _cache = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        return _cache;
    }

    private static void Save(HashSet<string> data)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        var sorted = data.OrderBy(id => id, StringComparer.OrdinalIgnoreCase).ToList();
        var json = JsonSerializer.Serialize(sorted, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}
