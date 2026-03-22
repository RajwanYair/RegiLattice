// RegiLattice.Core — Services/ComplianceReportExporter.cs
// Sprint 102 — exports a per-category HTML compliance/health report.

#nullable enable

using RegiLattice.Core.Models;

namespace RegiLattice.Core.Services;

/// <summary>
/// Generates an HTML compliance report that summarises which tweaks have been
/// applied and which are still pending, grouped by category.
/// </summary>
public static class ComplianceReportExporter
{
    /// <summary>
    /// Generates the HTML report and writes it to <paramref name="outputPath"/>.
    /// </summary>
    /// <param name="engine">Loaded TweakEngine (must have RegisterBuiltins called).</param>
    /// <param name="statusMap">
    ///     Known tweak statuses.  Tweaks missing from this map are treated as Unknown.
    /// </param>
    /// <param name="outputPath">Destination .html file path.</param>
    public static void ExportHtml(
        TweakEngine engine,
        IReadOnlyDictionary<string, TweakResult>? statusMap,
        string outputPath)
    {
        ArgumentNullException.ThrowIfNull(engine);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);

        string html = BuildHtml(engine, statusMap ?? new Dictionary<string, TweakResult>());
        File.WriteAllText(outputPath, html, System.Text.Encoding.UTF8);
    }

    /// <summary>
    /// Generates the HTML report and returns it as a string.
    /// </summary>
    public static string BuildHtml(
        TweakEngine engine,
        IReadOnlyDictionary<string, TweakResult>? statusMap = null)
    {
        ArgumentNullException.ThrowIfNull(engine);

        IReadOnlyDictionary<string, TweakResult> status =
            statusMap ?? new Dictionary<string, TweakResult>();

        // ── Aggregate stats per category ──────────────────────────────
        var allTweaks = engine.AllTweaks();
        int totalTweaks = allTweaks.Count;
        int applied = 0, pending = 0, unknown = 0;

        var byCategory = new Dictionary<string, List<(TweakDef Tweak, TweakResult Status)>>(
            StringComparer.OrdinalIgnoreCase);

        foreach (TweakDef td in allTweaks)
        {
            TweakResult r = status.ContainsKey(td.Id) ? status[td.Id] : TweakResult.Unknown;
            if (r == TweakResult.Applied) applied++;
            else if (r == TweakResult.NotApplied) pending++;
            else unknown++;

            if (!byCategory.TryGetValue(td.Category, out var list))
            {
                list = [];
                byCategory[td.Category] = list;
            }
            list.Add((td, r));
        }

        int evaluated = applied + pending;
        int healthPct = evaluated > 0 ? (int)Math.Round(applied * 100.0 / evaluated) : 0;

        // ── Build HTML ─────────────────────────────────────────────────
        var sb = new System.Text.StringBuilder(64 * 1024);

        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html><head><meta charset=\"utf-8\">");
        sb.AppendLine("<title>RegiLattice Compliance Report</title>");
        sb.AppendLine("<style>");
        sb.AppendLine("*{box-sizing:border-box;margin:0;padding:0}");
        sb.AppendLine("body{font-family:'Segoe UI',Arial,sans-serif;background:#1e1e2e;color:#cdd6f4;padding:24px}");
        sb.AppendLine("h1{color:#89b4fa;margin-bottom:6px}");
        sb.AppendLine(".meta{color:#585b70;font-size:.85em;margin-bottom:24px}");
        sb.AppendLine(".summary{display:flex;gap:16px;flex-wrap:wrap;margin-bottom:32px}");
        sb.AppendLine(".stat{background:#313244;border-radius:10px;padding:12px 20px;min-width:140px}");
        sb.AppendLine(".stat b{display:block;font-size:2em;line-height:1.1}");
        sb.AppendLine(".stat.green b{color:#a6e3a1}.stat.red b{color:#f38ba8}");
        sb.AppendLine(".stat.yellow b{color:#f9e2af}.stat.blue b{color:#89b4fa}");
        sb.AppendLine(".gauge{background:#313244;border-radius:10px;padding:12px 20px;flex:1;min-width:200px}");
        sb.AppendLine(".gauge-bar{height:20px;background:#45475a;border-radius:6px;overflow:hidden;margin-top:6px}");
        sb.AppendLine(".gauge-fill{height:100%;border-radius:6px;background:linear-gradient(90deg,#a6e3a1,#89b4fa)}");
        sb.AppendLine("h2{color:#cba6f7;margin:20px 0 8px;font-size:1.1em}");
        sb.AppendLine("table{border-collapse:collapse;width:100%;margin-bottom:24px;font-size:.88em}");
        sb.AppendLine("th{background:#313244;color:#89b4fa;padding:7px 10px;border:1px solid #45475a;text-align:left}");
        sb.AppendLine("td{padding:6px 10px;border:1px solid #45475a;vertical-align:top}");
        sb.AppendLine(".r-applied{background:#1a2e1a}.r-pending{background:#2e1a1a}.r-unknown{background:#1e1e2e}");
        sb.AppendLine(".badge{display:inline-block;border-radius:4px;padding:1px 7px;font-size:.82em;font-weight:600}");
        sb.AppendLine(".b-applied{background:#a6e3a1;color:#1e1e2e}.b-pending{background:#f38ba8;color:#1e1e2e}");
        sb.AppendLine(".b-unknown{background:#585b70;color:#cdd6f4}");
        sb.AppendLine("footer{margin-top:40px;color:#585b70;font-size:.8em}");
        sb.AppendLine("</style></head><body>");

        // Header
        sb.AppendLine("<h1>RegiLattice Compliance Report</h1>");
        sb.AppendLine($"<p class=\"meta\">Generated: {DateTime.Now:yyyy-MM-dd HH:mm} — {totalTweaks:N0} tweaks across {byCategory.Count} categories</p>");

        // Summary cards
        sb.AppendLine("<div class=\"summary\">");
        sb.AppendLine($"<div class=\"stat blue\"><b>{totalTweaks:N0}</b>Total tweaks</div>");
        sb.AppendLine($"<div class=\"stat green\"><b>{applied:N0}</b>Applied</div>");
        sb.AppendLine($"<div class=\"stat red\"><b>{pending:N0}</b>Pending</div>");
        sb.AppendLine($"<div class=\"stat yellow\"><b>{unknown:N0}</b>Unknown</div>");
        sb.AppendLine($"<div class=\"gauge\"><b>Health: {healthPct}%</b>");
        sb.AppendLine($"<div class=\"gauge-bar\"><div class=\"gauge-fill\" style=\"width:{healthPct}%\"></div></div></div>");
        sb.AppendLine("</div>");

        // Per-category tables
        foreach (string cat in byCategory.Keys.OrderBy(c => c, StringComparer.OrdinalIgnoreCase))
        {
            var rows = byCategory[cat];
            int catApplied = rows.Count(r => r.Status == TweakResult.Applied);
            int catPending = rows.Count(r => r.Status == TweakResult.NotApplied);
            int catTotal = rows.Count;

            sb.AppendLine($"<h2>{System.Net.WebUtility.HtmlEncode(cat)}</h2>");
            sb.AppendLine($"<table><tr>" +
                          $"<th>Tweak</th><th>Status</th><th>Scope</th><th>Admin</th><th>Description</th></tr>");

            foreach ((TweakDef td, TweakResult r) in rows.OrderBy(x => x.Tweak.Label, StringComparer.OrdinalIgnoreCase))
            {
                string rowCss = r switch
                {
                    TweakResult.Applied => "r-applied",
                    TweakResult.NotApplied => "r-pending",
                    _ => "r-unknown",
                };
                string badge = r switch
                {
                    TweakResult.Applied => "<span class=\"badge b-applied\">Applied</span>",
                    TweakResult.NotApplied => "<span class=\"badge b-pending\">Pending</span>",
                    _ => "<span class=\"badge b-unknown\">Unknown</span>",
                };

                sb.AppendLine($"<tr class=\"{rowCss}\">" +
                              $"<td>{System.Net.WebUtility.HtmlEncode(td.Label)}</td>" +
                              $"<td>{badge}</td>" +
                              $"<td>{td.Scope}</td>" +
                              $"<td>{(td.NeedsAdmin ? "Yes" : "No")}</td>" +
                              $"<td>{System.Net.WebUtility.HtmlEncode(td.Description)}</td></tr>");
            }

            sb.AppendLine("</table>");
        }

        sb.AppendLine("<footer>Generated by RegiLattice Compliance Report Exporter</footer>");
        sb.AppendLine("</body></html>");
        return sb.ToString();
    }
}
