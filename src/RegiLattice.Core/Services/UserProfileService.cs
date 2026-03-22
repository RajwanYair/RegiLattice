// RegiLattice.Core — Services/UserProfileService.cs
// Manages user-defined custom profiles stored as JSON files in
// %LOCALAPPDATA%\RegiLattice\profiles\<name>.json (or portable equivalent).
// Complements the 5 built-in profiles — users can create, rename, clone,
// and delete their own profiles.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace RegiLattice.Core;

/// <summary>A user-defined custom profile persisted to disk.</summary>
public sealed record UserProfile
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("description")]
    public string Description { get; init; } = "";

    [JsonPropertyName("tweak_ids")]
    public IReadOnlyList<string> TweakIds { get; init; } = [];

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [JsonPropertyName("modified_at")]
    public DateTime ModifiedAt { get; init; } = DateTime.UtcNow;
}

/// <summary>
/// Persists and manages user-defined custom profiles.
/// Profiles are stored as individual JSON files under
/// <c>%LOCALAPPDATA%\RegiLattice\profiles\</c> (or the portable equivalent).
/// </summary>
public static class UserProfileService
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    // ── Path helpers ─────────────────────────────────────────────────────

    /// <summary>Directory where user profile JSON files are stored.</summary>
    public static string ProfilesDir => Path.Combine(AppConfig.ConfigDir, "profiles");

    /// <summary>Full path for a named profile file.</summary>
    public static string ProfilePath(string name) => Path.Combine(ProfilesDir, $"{SanitizeName(name)}.json");

    // ── Public API ───────────────────────────────────────────────────────

    /// <summary>
    /// Returns all user-defined profiles from disk, sorted by name.
    /// Returns an empty list when no profiles exist or the directory is missing.
    /// </summary>
    public static IReadOnlyList<UserProfile> GetProfiles()
    {
        if (!Directory.Exists(ProfilesDir))
            return [];

        var profiles = new List<UserProfile>();
        foreach (string file in Directory.EnumerateFiles(ProfilesDir, "*.json"))
        {
            var profile = LoadFromFile(file);
            if (profile is not null)
                profiles.Add(profile);
        }

        profiles.Sort((a, b) => StringComparer.OrdinalIgnoreCase.Compare(a.Name, b.Name));
        return profiles.AsReadOnly();
    }

    /// <summary>
    /// Returns a single user profile by name, or <c>null</c> if not found.
    /// Name matching is case-insensitive.
    /// </summary>
    public static UserProfile? GetProfile(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return LoadFromFile(ProfilePath(name));
    }

    /// <summary>Returns <c>true</c> when a profile with the given name exists on disk.</summary>
    public static bool Exists(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return File.Exists(ProfilePath(name));
    }

    /// <summary>
    /// Creates a new user profile and persists it to disk.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when a profile with the same name already exists.
    /// </exception>
    public static UserProfile Create(string name, string description, IReadOnlyList<string> tweakIds)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(tweakIds);

        if (Exists(name))
            throw new InvalidOperationException($"A profile named '{name}' already exists.");

        var profile = new UserProfile
        {
            Name = name.Trim(),
            Description = description.Trim(),
            TweakIds = tweakIds,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
        };

        Flush(profile);
        return profile;
    }

    /// <summary>
    /// Persists an existing (or new) profile to disk, replacing any
    /// previous file with the same sanitised name.
    /// </summary>
    public static void Save(UserProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        Flush(profile with { ModifiedAt = DateTime.UtcNow });
    }

    /// <summary>
    /// Updates the tweak list of an existing profile and saves it.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the named profile does not exist.</exception>
    public static UserProfile Update(string name, IReadOnlyList<string> tweakIds, string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(tweakIds);

        var existing = GetProfile(name) ?? throw new InvalidOperationException($"Profile '{name}' not found.");

        var updated = existing with { TweakIds = tweakIds, Description = description ?? existing.Description, ModifiedAt = DateTime.UtcNow };

        Flush(updated);
        return updated;
    }

    /// <summary>
    /// Renames a profile (moves the JSON file and updates the Name field).
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the source profile does not exist or the target name is already taken.
    /// </exception>
    public static UserProfile Rename(string name, string newName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(newName);

        var existing = GetProfile(name) ?? throw new InvalidOperationException($"Profile '{name}' not found.");

        if (Exists(newName))
            throw new InvalidOperationException($"A profile named '{newName}' already exists.");

        var renamed = existing with { Name = newName.Trim(), ModifiedAt = DateTime.UtcNow };
        Flush(renamed);
        Delete(name);
        return renamed;
    }

    /// <summary>
    /// Clones an existing profile under a new name.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the source profile does not exist or the target name is already taken.
    /// </exception>
    public static UserProfile Clone(string name, string newName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(newName);

        var existing = GetProfile(name) ?? throw new InvalidOperationException($"Profile '{name}' not found.");

        if (Exists(newName))
            throw new InvalidOperationException($"A profile named '{newName}' already exists.");

        var clone = existing with { Name = newName.Trim(), CreatedAt = DateTime.UtcNow, ModifiedAt = DateTime.UtcNow };

        Flush(clone);
        return clone;
    }

    /// <summary>Deletes a user profile by name.  No-op if the profile does not exist.</summary>
    public static void Delete(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        string path = ProfilePath(name);
        if (File.Exists(path))
            File.Delete(path);
    }

    /// <summary>Deletes all user profiles.</summary>
    public static void DeleteAll()
    {
        if (Directory.Exists(ProfilesDir))
        {
            foreach (string file in Directory.EnumerateFiles(ProfilesDir, "*.json"))
                File.Delete(file);
        }
    }

    // ── Internal helpers ─────────────────────────────────────────────────

    private static void Flush(UserProfile profile)
    {
        Directory.CreateDirectory(ProfilesDir);
        string path = ProfilePath(profile.Name);
        File.WriteAllText(path, JsonSerializer.Serialize(profile, JsonOpts));
    }

    private static UserProfile? LoadFromFile(string path)
    {
        if (!File.Exists(path))
            return null;
        try
        {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<UserProfile>(json, JsonOpts);
        }
        catch (Exception ex) when (ex is IOException or JsonException or UnauthorizedAccessException)
        {
            return null;
        }
    }

    private static string SanitizeName(string name)
    {
        // Replace characters that are invalid in file names with underscores.
        var invalid = Path.GetInvalidFileNameChars();
        return string.Create(
            name.Length,
            (name, invalid),
            static (span, state) =>
            {
                for (int i = 0; i < state.name.Length; i++)
                    span[i] = Array.IndexOf(state.invalid, state.name[i]) >= 0 ? '_' : state.name[i];
            }
        );
    }
}
