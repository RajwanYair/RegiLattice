using System.Text.Json;

namespace RegiLattice.Core;

/// <summary>
/// Pre-cached <see cref="JsonSerializerOptions"/> instances shared across the
/// codebase. Avoids allocating (and reflecting) a fresh options object on every
/// <see cref="JsonSerializer.Serialize{T}(T, JsonSerializerOptions?)"/> call.
/// </summary>
public static class JsonOptions
{
    /// <summary><c>{ WriteIndented = true }</c> — used by Export, Snapshot, Config, Analytics, etc.</summary>
    public static readonly JsonSerializerOptions Indented = new() { WriteIndented = true };

    /// <summary><c>{ PropertyNameCaseInsensitive = true }</c> — used for lenient deserialization.</summary>
    public static readonly JsonSerializerOptions CaseInsensitive = new() { PropertyNameCaseInsensitive = true };
}
