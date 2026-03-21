// RegiLattice.Core — Services/HtmlReportGenerator.cs
// Generates a self-contained HTML tweak-status report with summary cards,
// optional health-score progress bars, and per-category tweak tables.

#nullable enable

using System.Text;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Services;

/// <summary>
/// Generates a fully self-contained HTML report of the current tweak status,
/// styled with the Catppuccin Mocha palette.
/// </summary>
public sealed class HtmlReportGenerator
{
    private readonly TweakEngine _engine;

    public HtmlReportGenerator(TweakEngine engine)
    {
        _engine = engine ?? throw new ArgumentNullException(nameof(engine));
    }

    // ── Public API ───────────────────────────────────────────────────────

    /// <summary>
    /// Builds the HTML report from <paramref name="statusMap"/> and writes it to
    /// <paramref name="outputPath"/> as UTF-8.
    /// </summary>
    /// <param name="outputPath">Destination file path (created or overwritten).</param>
    /// <param name="statusMap">
    /// Tweak ID → <see cref="TweakResult"/> map; typically from
    /// <see cref="TweakEngine.StatusMap"/>.
    /// </param>
    /// <param name="score">
    /// Optional <see cref="HealthScore"/> to include a progress-bar section.
    /// </param>
    public void Generate(string outputPath, IReadOnlyDictionary<string, TweakResult> statusMap, HealthScore? score = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);
        ArgumentNullException.ThrowIfNull(statusMap);

        var html = Build(statusMap, score);
        File.WriteAllText(outputPath, html, Encoding.UTF8);
    }

    /// <summary>
    /// Builds and returns the full HTML string without writing to disk.
    /// Useful for testing or embedding into other output streams.
    /// </summary>
    public string Build(IReadOnlyDictionary<string, TweakResult> statusMap, HealthScore? score = null)
    {
        ArgumentNullException.ThrowIfNull(statusMap);

        var sb = new StringBuilder(capacity: 256 * 1024);
        var byCat = _engine.TweaksByCategory();
        var allTweaks = _engine.AllTweaks();

        int total = allTweaks.Count;
        int applied = statusMap.Count(kv => kv.Value == TweakResult.Applied);
        int notApplied = statusMap.Count(kv => kv.Value == TweakResult.NotApplied);
        int unknown = total - applied - notApplied;

        AppendHeader(sb);

        // Page heading
        sb.AppendLine("<h1>&#129139; RegiLattice Tweak Status Report</h1>");
        sb.AppendLine(
            $"<p class=\"subtitle\">Generated {DateTime.Now:yyyy-MM-dd HH:mm:ss} &mdash; {total} tweaks across {byCat.Count} categories</p>"
        );

        // Summary cards
        sb.AppendLine("<div class=\"summary\">");
        AppendCard(sb, total.ToString(), "Total", "total-val");
        AppendCard(sb, applied.ToString(), "Applied", "applied-val");
        AppendCard(sb, notApplied.ToString(), "Not Applied", "notapplied-val");
        AppendCard(sb, unknown.ToString(), "Unknown / Error", "unknown-val");
        sb.AppendLine("</div>");

        // Optional health score section
        if (score is not null)
            AppendScoreSection(sb, score);

        // Per-category tables
        foreach (var cat in byCat.Keys.Order())
        {
            var tweaks = byCat[cat];
            int catApplied = tweaks.Count(t => statusMap.GetValueOrDefault(t.Id) == TweakResult.Applied);

            sb.AppendLine("<div class=\"cat-section\">");
            sb.AppendLine($"<h2>{Encode(cat)} <span class=\"badge\">{catApplied}/{tweaks.Count}</span></h2>");
            sb.AppendLine("<table><tr><th>ID</th><th>Label</th><th>Status</th><th>Description</th></tr>");

            foreach (var td in tweaks.OrderBy(t => t.Id))
            {
                var st = statusMap.GetValueOrDefault(td.Id, TweakResult.Unknown);
                string stCss = st switch
                {
                    TweakResult.Applied => "st-applied",
                    TweakResult.NotApplied => "st-notapplied",
                    TweakResult.Error => "st-error",
                    _ => "st-unknown",
                };
                sb.AppendLine(
                    $"<tr>"
                        + $"<td class=\"id-cell\">{Encode(td.Id)}</td>"
                        + $"<td>{Encode(td.Label)}</td>"
                        + $"<td class=\"{stCss}\">{st}</td>"
                        + $"<td>{Encode(td.Description)}</td>"
                        + $"</tr>"
                );
            }

            sb.AppendLine("</table></div>");
        }

        sb.AppendLine("</body></html>");
        return sb.ToString();
    }

    // ── Internal helpers ─────────────────────────────────────────────────

    private static void AppendHeader(StringBuilder sb)
    {
        sb.AppendLine(
            """
            <!DOCTYPE html>
            <html lang="en">
            <head>
            <meta charset="utf-8">
            <meta name="viewport" content="width=device-width,initial-scale=1">
            <title>RegiLattice &mdash; Tweak Status Report</title>
            <style>
            :root{
              --bg0:#1e1e2e;--bg1:#181825;--bg2:#313244;
              --text:#cdd6f4;--subtext:#bac2de;
              --green:#a6e3a1;--red:#f38ba8;--yellow:#f9e2af;
              --blue:#89b4fa;--lavender:#b4befe;--peach:#fab387;--mauve:#cba6f7;
            }
            *{box-sizing:border-box;margin:0;padding:0}
            body{background:var(--bg0);color:var(--text);font-family:'Segoe UI',Tahoma,sans-serif;padding:24px}
            h1{color:var(--lavender);margin-bottom:4px;font-size:1.8em}
            .subtitle{color:var(--subtext);margin-bottom:20px;font-size:.9em}
            .summary{display:flex;gap:16px;flex-wrap:wrap;margin-bottom:24px}
            .card{background:var(--bg2);border-radius:8px;padding:14px 20px;min-width:120px;text-align:center}
            .card .val{font-size:2em;font-weight:700;line-height:1}
            .card .lbl{font-size:.75em;color:var(--subtext);margin-top:4px;text-transform:uppercase;letter-spacing:.05em}
            .applied-val{color:var(--green)}.notapplied-val{color:var(--red)}
            .unknown-val{color:var(--yellow)}.total-val{color:var(--blue)}
            .score-section{background:var(--bg2);border-radius:8px;padding:16px 20px;margin-bottom:20px}
            .score-section h2{color:var(--mauve);margin-bottom:10px;font-size:1em;text-transform:uppercase;letter-spacing:.08em}
            .score-bar{display:flex;align-items:center;gap:10px;margin:5px 0}
            .score-bar .dim{width:110px;color:var(--subtext);font-size:.85em}
            .bar-track{flex:1;height:10px;background:var(--bg1);border-radius:5px;overflow:hidden}
            .bar-fill{height:100%;background:var(--mauve);border-radius:5px}
            .overall-fill{background:var(--lavender)}
            .score-num{width:40px;text-align:right;font-size:.85em}
            .cat-section{margin-bottom:24px}
            .cat-section h2{color:var(--peach);margin-bottom:8px;font-size:1em;display:flex;align-items:center;gap:8px}
            .badge{background:var(--bg2);color:var(--subtext);font-size:.75em;padding:2px 8px;border-radius:999px}
            table{width:100%;border-collapse:collapse;background:var(--bg1);border-radius:8px;overflow:hidden}
            th{background:var(--bg2);color:var(--subtext);font-size:.78em;text-transform:uppercase;letter-spacing:.06em;padding:8px 12px;text-align:left}
            td{padding:7px 12px;font-size:.85em;border-top:1px solid var(--bg2)}
            tr:hover td{background:var(--bg2)}
            .st-applied{color:var(--green)}.st-notapplied{color:var(--red)}
            .st-unknown{color:var(--yellow)}.st-error{color:var(--peach)}
            .id-cell{color:var(--blue);font-family:monospace;font-size:.82em}
            </style>
            </head>
            <body>
            """
        );
    }

    private static void AppendCard(StringBuilder sb, string value, string label, string valueCss)
    {
        sb.AppendLine($"<div class=\"card\"><div class=\"val {valueCss}\">{value}</div><div class=\"lbl\">{label}</div></div>");
    }

    private static void AppendScoreSection(StringBuilder sb, HealthScore score)
    {
        sb.AppendLine("<div class=\"score-section\">");
        sb.AppendLine($"<h2>&#129657; Health Score &mdash; {score.Overall}% ({Encode(score.OverallLabel)})</h2>");

        static void Bar(StringBuilder sb, string dim, int val, string? extraCss = null)
        {
            string fillCss = extraCss is not null ? $"bar-fill {extraCss}" : "bar-fill";
            sb.AppendLine(
                $"<div class=\"score-bar\">"
                    + $"<span class=\"dim\">{dim}</span>"
                    + $"<div class=\"bar-track\"><div class=\"{fillCss}\" style=\"width:{val}%\"></div></div>"
                    + $"<span class=\"score-num\">{val}%</span>"
                    + $"</div>"
            );
        }

        Bar(sb, "Privacy", score.Privacy);
        Bar(sb, "Performance", score.Performance);
        Bar(sb, "Security", score.Security);
        Bar(sb, "Stability", score.Stability);
        Bar(sb, "<strong>Overall</strong>", score.Overall, "overall-fill");
        sb.AppendLine("</div>");
    }

    private static string Encode(string s) => s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
}
