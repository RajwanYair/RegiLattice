// RegiLattice.Core — Services/AutoUpdater.cs
// Checks for new releases on GitHub Releases (REST v3 API).
// Safe: read-only HTTPS GET, no credentials, no PII sent.

#nullable enable

using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using RegiLattice.Core;

namespace RegiLattice.Core.Services;

/// <summary>
/// Checks GitHub Releases for a newer version of RegiLattice.
/// All network access is isolated here — the rest of the codebase
/// stays offline-safe.
/// </summary>
public static class AutoUpdater
{
    // GitHub REST API — no auth required for public repos.
    private const string ReleasesApiUrl = "https://api.github.com/repos/RajwanYair/RegiLattice/releases/latest";

    // User-Agent is required by GitHub; include the current version so they
    // can analyse adoption (no PII).
    private static readonly string _userAgent = $"RegiLattice/{CurrentVersion} (https://github.com/RajwanYair/RegiLattice)";

    /// <summary>Current assembly version, e.g. "4.0.0".</summary>
    public static string CurrentVersion =>
        Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0] // strip git hash suffix if present
        ?? Assembly.GetExecutingAssembly().GetName().Version?.ToString(3)
        ?? "0.0.0";

    /// <summary>
    /// Queries the GitHub Releases API for the latest published release.
    /// </summary>
    /// <param name="timeout">HTTP timeout (default 10 s).</param>
    /// <returns>
    /// An <see cref="UpdateInfo"/> record.
    /// If the check fails (network unavailable, rate-limit, etc.) the method
    /// returns a record with <see cref="UpdateInfo.UpdateAvailable"/> = false
    /// and the exception message in <see cref="UpdateInfo.ReleaseNotes"/>.
    /// </returns>
    public static async Task<UpdateInfo> CheckAsync(TimeSpan? timeout = null)
    {
        var current = CurrentVersion;
        try
        {
            using var http = new HttpClient();
            http.Timeout = timeout ?? TimeSpan.FromSeconds(10);
            http.DefaultRequestHeaders.UserAgent.ParseAdd(_userAgent);
            http.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");

            var json = await http.GetStringAsync(ReleasesApiUrl).ConfigureAwait(false);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var tagName = root.TryGetProperty("tag_name", out var tagProp) ? tagProp.GetString() ?? "" : "";

            // Strip leading 'v' → "v4.1.0" → "4.1.0"
            var latest = tagName.TrimStart('v');

            var body = root.TryGetProperty("body", out var bodyProp) ? bodyProp.GetString() ?? "" : "";

            // Find the Windows x64 installer or CLI download URL from assets.
            var downloadUrl = "";
            if (root.TryGetProperty("assets", out var assets))
            {
                foreach (var asset in assets.EnumerateArray())
                {
                    if (!asset.TryGetProperty("browser_download_url", out var urlProp))
                        continue;
                    var url = urlProp.GetString() ?? "";
                    // Prefer the MSI / EXE installer, fall back to any ZIP.
                    if (url.EndsWith(".msi", StringComparison.OrdinalIgnoreCase) || url.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                    {
                        downloadUrl = url;
                        break;
                    }
                    if (url.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) && downloadUrl.Length == 0)
                        downloadUrl = url;
                }
            }

            // Fall back to the GitHub release page URL.
            if (downloadUrl.Length == 0 && root.TryGetProperty("html_url", out var htmlUrlProp))
                downloadUrl = htmlUrlProp.GetString() ?? "";

            var available = IsNewer(latest, current);
            return new UpdateInfo
            {
                CurrentVersion = current,
                LatestVersion = latest,
                DownloadUrl = downloadUrl,
                ReleaseNotes = body,
                UpdateAvailable = available,
            };
        }
        catch (Exception ex)
        {
            return new UpdateInfo
            {
                CurrentVersion = current,
                LatestVersion = current,
                ReleaseNotes = $"Update check failed: {ex.Message}",
                UpdateAvailable = false,
            };
        }
    }

    // ── Version comparison ────────────────────────────────────────────────

    /// <summary>
    /// Returns <see langword="true"/> when <paramref name="candidate"/> is
    /// strictly newer than <paramref name="current"/> using semantic versioning
    /// (major.minor.patch).
    /// </summary>
    internal static bool IsNewer(string candidate, string current)
    {
        static string Strip(string v) => v.TrimStart('v');
        if (!Version.TryParse(Strip(candidate), out var cand))
            return false;
        if (!Version.TryParse(Strip(current), out var curr))
            return false;
        return cand > curr;
    }
}
