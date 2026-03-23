// RegiLattice.Core — Plugins/PackSubmissionService.cs
// Builds GitHub issue URLs for community Tweak Pack submissions (T7.5).
// No credentials required — opens in the user's browser.

using System.Text;
using System.Web;

namespace RegiLattice.Core.Plugins;

/// <summary>Validation result from <see cref="PackSubmissionService.Validate"/>.</summary>
public sealed record PackSubmissionValidation(bool IsValid, IReadOnlyList<string> Errors);

/// <summary>
/// Helpers for the community Tweak Pack submission workflow.
/// Builds a pre-filled GitHub issues URL so a contributor can submit
/// their pack for review without any API credentials.
/// </summary>
public static class PackSubmissionService
{
    // URL of the community submission GitHub issue template.
    private const string IssuesBaseUrl = "https://github.com/RajwanYair/RegiLattice/issues/new?template=pack-submission.yml";

    // ── Validation ────────────────────────────────────────────────────────

    /// <summary>
    /// Validate a <see cref="PackDef"/> for community submission readiness.
    /// Returns a <see cref="PackSubmissionValidation"/> describing any errors found.
    /// </summary>
    public static PackSubmissionValidation Validate(PackDef pack)
    {
        ArgumentNullException.ThrowIfNull(pack);

        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(pack.Name))
            errors.Add("Name is required.");
        else if (!System.Text.RegularExpressions.Regex.IsMatch(pack.Name, @"^[a-z0-9][a-z0-9\-]{1,62}$"))
            errors.Add("Name must be lowercase kebab-case (a-z, 0-9, hyphens) and 2–63 chars.");

        if (string.IsNullOrWhiteSpace(pack.DisplayName))
            errors.Add("DisplayName is required.");
        else if (pack.DisplayName.Length > 80)
            errors.Add("DisplayName must be 80 characters or fewer.");

        if (string.IsNullOrWhiteSpace(pack.Author))
            errors.Add("Author (GitHub username) is required.");

        if (string.IsNullOrWhiteSpace(pack.Version))
            errors.Add("Version is required.");
        else if (!System.Text.RegularExpressions.Regex.IsMatch(pack.Version, @"^\d+\.\d+\.\d+$"))
            errors.Add("Version must follow Semantic Versioning (e.g., 1.0.0).");

        if (string.IsNullOrWhiteSpace(pack.DownloadUrl))
            errors.Add("DownloadUrl is required for a community submission.");
        else if (
            !Uri.TryCreate(pack.DownloadUrl, UriKind.Absolute, out var uri) || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
        )
            errors.Add("DownloadUrl must be an absolute https:// URL.");

        if (string.IsNullOrWhiteSpace(pack.Description))
            errors.Add("Description is required.");
        else if (pack.Description.Length < 20)
            errors.Add("Description must be at least 20 characters.");

        if (pack.TweakCount < 1)
            errors.Add("TweakCount must be at least 1.");

        if (string.IsNullOrWhiteSpace(pack.Sha256))
            errors.Add("Sha256 checksum is required for verification.");
        else if (pack.Sha256.Length != 64 || !pack.Sha256.All(c => char.IsAsciiHexDigit(c)))
            errors.Add("Sha256 must be a 64-character lowercase hex string.");

        return new PackSubmissionValidation(errors.Count == 0, errors);
    }

    // ── URL Builder ───────────────────────────────────────────────────────

    /// <summary>
    /// Build a GitHub issue URL pre-filled with the pack's metadata.
    /// The caller should open this URL in the system browser so the contributor
    /// can finalize and submit the issue without any API credentials.
    /// </summary>
    public static string BuildSubmissionUrl(PackDef pack)
    {
        ArgumentNullException.ThrowIfNull(pack);

        // The GitHub issue form (pack-submission.yml) expects these query params:
        //   title=  → issue title
        //   body=   → fallback body for templates without query-param support
        // We encode the pack metadata into the title so reviewers see it immediately.

        string title = $"[Pack Submission] {pack.DisplayName} v{pack.Version} by @{pack.Author}";

        var body = new StringBuilder();
        body.AppendLine("### Pack Metadata");
        body.AppendLine();
        body.AppendLine($"**Name**: `{pack.Name}`");
        body.AppendLine($"**Display Name**: {pack.DisplayName}");
        body.AppendLine($"**Version**: {pack.Version}");
        body.AppendLine($"**Author**: @{pack.Author}");
        body.AppendLine($"**Tweak Count**: {pack.TweakCount}");
        if (pack.Categories.Count > 0)
            body.AppendLine($"**Categories**: {string.Join(", ", pack.Categories)}");
        if (pack.Tags.Count > 0)
            body.AppendLine($"**Tags**: {string.Join(", ", pack.Tags)}");
        body.AppendLine();
        body.AppendLine("### Description");
        body.AppendLine();
        body.AppendLine(pack.Description);
        body.AppendLine();
        body.AppendLine("### Download");
        body.AppendLine();
        body.AppendLine($"**URL**: {pack.DownloadUrl}");
        if (!string.IsNullOrWhiteSpace(pack.Sha256))
            body.AppendLine($"**SHA-256**: `{pack.Sha256}`");
        body.AppendLine();
        body.AppendLine("---");
        body.AppendLine("*Generated by RegiLattice Pack Creator Studio*");

        string url = $"{IssuesBaseUrl}&title={HttpUtility.UrlEncode(title)}&body={HttpUtility.UrlEncode(body.ToString())}";
        return url;
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    /// <summary>Sanitise a pack name to a lowercase kebab-case slug.</summary>
    public static string SanitizeName(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return "my-pack";

        string lower = raw.Trim().ToLowerInvariant();
        // Replace spaces and non-slug chars with hyphens
        string slug = System.Text.RegularExpressions.Regex.Replace(lower, @"[^a-z0-9]+", "-").Trim('-');
        return slug.Length == 0 ? "my-pack" : slug[..Math.Min(slug.Length, 63)];
    }
}
