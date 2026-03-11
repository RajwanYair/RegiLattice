// RegiLattice.Core — Services/AppConfig.cs
// User configuration — replaces Python config.py. Uses JSON for simplicity.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace RegiLattice.Core;

public sealed class AppConfig
{
    [JsonPropertyName("force_corp")]
    public bool ForceCorp { get; set; }

    [JsonPropertyName("max_workers")]
    public int MaxWorkers { get; set; } = 8;

    [JsonPropertyName("backup_dir")]
    public string BackupDir { get; set; } = "";

    [JsonPropertyName("auto_backup")]
    public bool AutoBackup { get; set; } = true;

    [JsonPropertyName("theme")]
    public string Theme { get; set; } = "catppuccin-mocha";

    [JsonPropertyName("locale")]
    public string Locale { get; set; } = "en";

    [JsonPropertyName("check_tool_updates")]
    public bool CheckToolUpdates { get; set; } = true;

    /// <summary>Default config directory: %LOCALAPPDATA%\RegiLattice</summary>
    public static string ConfigDir => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RegiLattice");

    public static string DefaultConfigPath => Path.Combine(ConfigDir, "config.json");

    public static AppConfig Load(string? path = null)
    {
        path ??= DefaultConfigPath;
        if (!File.Exists(path)) return new AppConfig();
        try
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
        }
        catch { return new AppConfig(); }
    }

    public void Save(string? path = null)
    {
        path ??= DefaultConfigPath;
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }
}
