using System.Text.Json;
using System.Text.Json.Serialization;

namespace RegiLattice.CLI;

// ── Source-generated JSON context (trim-safe) ────────────────────────────────
// Replaces reflection-based JsonSerializer calls with compile-time metadata,
// eliminating all IL2026 trim analysis warnings from PublishTrimmed.

[JsonSourceGenerationOptions(WriteIndented = true, PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(List<ProfileInfo>))]
[JsonSerializable(typeof(List<UserProfileDto>))]
[JsonSerializable(typeof(StatsReportDto))]
[JsonSerializable(typeof(Dictionary<string, int>))]
[JsonSerializable(typeof(List<CategoryReportDto>))]
[JsonSerializable(typeof(List<TweakStatusDto>))]
[JsonSerializable(typeof(List<TweakListItemDto>))]
[JsonSerializable(typeof(List<TweakSearchItemDto>))]
[JsonSerializable(typeof(ProfileApplyResultDto))]
[JsonSerializable(typeof(TweakDetailDto))]
[JsonSerializable(typeof(TweakSkipDto))]
[JsonSerializable(typeof(TweakActionDto))]
[JsonSerializable(typeof(BatchRecipeResultDto))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(Dictionary<string, JsonElement>))]
[JsonSerializable(typeof(BatchRecipe))]
internal sealed partial class CliJsonContext : JsonSerializerContext;

// ── CLI JSON DTOs ────────────────────────────────────────────────────────────

internal sealed record UserProfileDto(string Name, string Description, int TweakCount, string CreatedAt);

internal sealed class StatsReportDto
{
    public int TotalTweaks { get; init; }
    public int Categories { get; init; }
    public int Profiles { get; init; }
    public required Dictionary<string, int> Scopes { get; init; }
    public int CorpSafe { get; init; }
    public int NeedsAdmin { get; init; }
    public int HasDetect { get; init; }
    public int HasDescription { get; init; }
    public int HasDependsOn { get; init; }
    public int QuickWins { get; init; }
    public required Dictionary<int, int> ImpactDistribution { get; init; }
    public required Dictionary<int, int> SafetyDistribution { get; init; }
    public required Dictionary<string, int> CategoryCounts { get; init; }
}

internal sealed record CategoryReportTweakDto(string Id, string Label, [property: JsonPropertyName("status")] string Status);

internal sealed record CategoryReportDto(
    [property: JsonPropertyName("category")] string Category,
    [property: JsonPropertyName("tweaks")] List<CategoryReportTweakDto> Tweaks
);

internal sealed record TweakStatusDto(string Id, string Status, string? Label);

internal sealed record TweakListItemDto(string Id, string Label, string Category, bool NeedsAdmin, bool CorpSafe);

internal sealed record TweakSearchItemDto(string Id, string Label, string Category, IReadOnlyList<string> Tags);

internal sealed record TweakResultItemDto(string Id, string Status);

internal sealed record ProfileApplyResultDto(string Profile, int Applied, int Total, bool DryRun, List<TweakResultItemDto> Results);

internal sealed record TweakDetailDto(string Id, string Label, string Category, string Status, bool NeedsAdmin, bool CorpSafe);

internal sealed record TweakSkipDto(string Id, bool Skipped, string Reason, string Status);

internal sealed record TweakActionDto(string Id, string Label, string Mode, string Status, bool DryRun);

internal sealed class BatchRecipeResultDto
{
    [JsonPropertyName("recipe")]
    public string? Recipe { get; init; }

    [JsonPropertyName("totalSteps")]
    public int TotalSteps { get; init; }

    [JsonPropertyName("successCount")]
    public int SuccessCount { get; init; }

    [JsonPropertyName("failureCount")]
    public int FailureCount { get; init; }

    [JsonPropertyName("rolledBack")]
    public bool RolledBack { get; init; }

    [JsonPropertyName("steps")]
    public required List<RecipeStepResult> Steps { get; init; }
}
