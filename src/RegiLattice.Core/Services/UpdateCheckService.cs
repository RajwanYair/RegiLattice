// RegiLattice.Core — Services/UpdateCheckService.cs
// Auto-update check via GitHub Releases API (Phase 10 #97).

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RegiLattice.Core;

/// <summary>Represents the result of an update availability check.</summary>
public sealed record UpdateInfo
{
    public bool UpdateAvailable { get; init; }
    public string CurrentVersion { get; init; } = "";
    public string LatestVersion { get; init; } = "";
    public string ReleaseNotes { get; init; } = "";
    public string DownloadUrl { get; init; } = "";
    public DateTime? PublishedAt { get; init; }
    public string? Error { get; init; }
}

/// <summary>Checks GitHub Releases for a newer version of RegiLattice.</summary>
public static class UpdateCheckService
{
    private const string GitHubApiUrl = "https://api.github.com/repos/RajwanYair/RegiLattice/releases/latest";

    /// <summary>Returns the currently running assembly version (e.g. "3.5.0").</summary>
    public static string CurrentVersion
    {
        get
        {
            var v = Assembly.GetExecutingAssembly().GetName().Version;
            return v is null ? "0.0.0" : $"{v.Major}.{v.Minor}.{v.Build}";
        }
    }

    /// <summary>
    /// Queries the GitHub Releases API and returns update availability info.
    /// Respects system proxy settings and HTTPS_PROXY / HTTP_PROXY environment variables.
    /// Returns an <see cref="UpdateInfo"/> with <c>Error</c> set on network/parse failures.
    /// </summary>
    public static async Task<UpdateInfo> CheckAsync(CancellationToken ct = default)
    {
        var current = CurrentVersion;
        try
        {
            using var http = CreateHttpClient(current);
            var json = await http.GetStringAsync(GitHubApiUrl, ct).ConfigureAwait(false);
            var release = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);

            if (release is null)
                return Error(current, "Failed to parse GitHub API response.");

            var tag = release.TagName?.TrimStart('v') ?? "";
            bool newer = CompareVersions(tag, current) > 0;

            return new UpdateInfo
            {
                UpdateAvailable = newer,
                CurrentVersion = current,
                LatestVersion = tag,
                ReleaseNotes = release.Body ?? "",
                DownloadUrl = release.HtmlUrl ?? "",
                PublishedAt = release.PublishedAt,
            };
        }
        catch (OperationCanceledException)
        {
            return Error(current, "Update check was cancelled.");
        }
        catch (HttpRequestException ex)
        {
            return Error(current, $"Network error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Error(current, $"Unexpected error: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates an <see cref="HttpClient"/> that honours the system proxy and
    /// HTTPS_PROXY / HTTP_PROXY environment variables (common in corporate environments).
    /// </summary>
    private static HttpClient CreateHttpClient(string version)
    {
        var handler = new HttpClientHandler
        {
            UseProxy = true,
            UseDefaultCredentials = true,
            PreAuthenticate = true,
        };

        // Honour HTTPS_PROXY / HTTP_PROXY env vars (set by corporate proxy tools or wsl2 forwarders)
        string? envProxy =
            Environment.GetEnvironmentVariable("HTTPS_PROXY")
            ?? Environment.GetEnvironmentVariable("https_proxy")
            ?? Environment.GetEnvironmentVariable("HTTP_PROXY")
            ?? Environment.GetEnvironmentVariable("http_proxy");

        if (!string.IsNullOrEmpty(envProxy) && Uri.TryCreate(envProxy, UriKind.Absolute, out var proxyUri))
        {
            handler.Proxy = new WebProxy(proxyUri) { UseDefaultCredentials = true };
        }
        else
        {
            // Fall back to the system-configured proxy (WinINet / WinHTTP)
            handler.Proxy = WebRequest.GetSystemWebProxy();
        }

        var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(20) };
        http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("RegiLattice", version));
        return http;
    }

    private static UpdateInfo Error(string current, string message) =>
        new()
        {
            CurrentVersion = current,
            LatestVersion = "",
            Error = message,
        };

    /// <summary>Compares two semver strings — returns positive if <paramref name="a"/> &gt; <paramref name="b"/>.</summary>
    public static int CompareVersions(string a, string b)
    {
        if (!Version.TryParse(a, out var va))
            va = new Version(0, 0, 0);
        if (!Version.TryParse(b, out var vb))
            vb = new Version(0, 0, 0);
        return va.CompareTo(vb);
    }
}

// ---- JSON model & source-gen context ----

internal sealed class GitHubRelease
{
    [JsonPropertyName("tag_name")]
    public string? TagName { get; init; }

    [JsonPropertyName("body")]
    public string? Body { get; init; }

    [JsonPropertyName("html_url")]
    public string? HtmlUrl { get; init; }

    [JsonPropertyName("published_at")]
    public DateTime? PublishedAt { get; init; }
}

[JsonSerializable(typeof(GitHubRelease))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
internal sealed partial class UpdateCheckContext : JsonSerializerContext { }
