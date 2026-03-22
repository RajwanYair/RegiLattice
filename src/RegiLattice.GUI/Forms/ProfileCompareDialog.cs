// RegiLattice.GUI — Forms/ProfileCompareDialog.cs
// Sprint 100 — side-by-side comparison of any two built-in profiles.

#nullable enable

using RegiLattice.Core;
using RegiLattice.Core.Models;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Compares two built-in profiles side-by-side, showing which tweaks are
/// exclusive to each profile and which are shared.  Includes HTML export.
/// </summary>
internal sealed class ProfileCompareDialog : Form
{
    private readonly TweakEngine _engine;

    // Profile selectors
    private ComboBox _cmbProfileA = null!;
    private ComboBox _cmbProfileB = null!;
    private Button _btnCompare = null!;

    // Results
    private ListView _listView = null!;

    // Summary labels
    private Label _lblSummary = null!;

    // Action buttons
    private Button _btnExportHtml = null!;
    private Button _btnClose = null!;

    // Last comparison data for export
    private List<(TweakDef Tweak, bool InA, bool InB)> _rows = [];

    // ── Construction ───────────────────────────────────────────────────

    internal ProfileCompareDialog(TweakEngine engine)
    {
        _engine = engine;
        InitUI();
        ApplyTheme();
        LoadProfiles();
    }

    // ── UI Construction ────────────────────────────────────────────────

    private void InitUI()
    {
        SuspendLayout();

        Text = "Profile Comparison";
        Size = new Size(1000, 660);
        MinimumSize = new Size(800, 500);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.Sizable;
        Font = new Font("Segoe UI", 9f);

        // ── Selector row ──────────────────────────────────────────────
        var pnlSelector = new Panel
        {
            Dock = DockStyle.Top,
            Height = 52,
            Padding = new Padding(10, 8, 10, 4),
        };

        var lblA = new Label
        {
            Text = "Profile A:",
            AutoSize = false,
            Width = 70,
            TextAlign = ContentAlignment.MiddleRight,
            Top = 12,
            Left = 0,
        };
        _cmbProfileA = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = 180,
            Top = 10,
            Left = 74,
        };
        var lblB = new Label
        {
            Text = "Profile B:",
            AutoSize = false,
            Width = 70,
            TextAlign = ContentAlignment.MiddleRight,
            Top = 12,
            Left = 274,
        };
        _cmbProfileB = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = 180,
            Top = 10,
            Left = 348,
        };
        _btnCompare = new Button
        {
            Text = "Compare",
            Width = 90,
            Height = 28,
            Top = 9,
            Left = 548,
        };

        pnlSelector.Controls.AddRange([lblA, _cmbProfileA, lblB, _cmbProfileB, _btnCompare]);

        // ── ListView ──────────────────────────────────────────────────
        _listView = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            GridLines = true,
            MultiSelect = false,
            OwnerDraw = false,
        };
        _listView.Columns.Add("#", 40);
        _listView.Columns.Add("Tweak", 320);
        _listView.Columns.Add("Category", 160);
        _listView.Columns.Add("In A", 60, HorizontalAlignment.Center);
        _listView.Columns.Add("In B", 60, HorizontalAlignment.Center);
        _listView.Columns.Add("Status", 130);

        // ── Summary / action bar ──────────────────────────────────────
        var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 48 };
        _lblSummary = new Label
        {
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleLeft,
            Dock = DockStyle.Fill,
            Padding = new Padding(10, 0, 0, 0),
        };
        _btnExportHtml = new Button
        {
            Text = "Export HTML…",
            Width = 110,
            Height = 30,
            Anchor = AnchorStyles.Right | AnchorStyles.Top,
            Top = 8,
            Left = pnlBottom.Width - 240,
        };
        _btnClose = new Button
        {
            Text = "Close",
            Width = 80,
            Height = 30,
            Anchor = AnchorStyles.Right | AnchorStyles.Top,
            Top = 8,
        };
        pnlBottom.Controls.AddRange([_lblSummary, _btnExportHtml, _btnClose]);

        // Anchor action buttons properly after layout
        _btnExportHtml.Anchor = AnchorStyles.Right | AnchorStyles.Top;
        _btnClose.Anchor = AnchorStyles.Right | AnchorStyles.Top;

        Controls.AddRange([pnlSelector, _listView, pnlBottom]);

        // Events
        _btnCompare.Click += (_, _) => RunComparison();
        _btnExportHtml.Click += (_, _) => ExportHtml();
        _btnClose.Click += (_, _) => Close();

        ResumeLayout(true);
    }

    // ── Profile Loader ─────────────────────────────────────────────────

    private void LoadProfiles()
    {
        var names = TweakEngine.Profiles.Select(p => p.Name).ToArray();
        _cmbProfileA.Items.AddRange(names);
        _cmbProfileB.Items.AddRange(names);

        if (names.Length >= 2)
        {
            _cmbProfileA.SelectedIndex = 0;
            _cmbProfileB.SelectedIndex = 1;
        }
    }

    // ── Comparison Logic ───────────────────────────────────────────────

    private void RunComparison()
    {
        if (_cmbProfileA.SelectedItem is not string nameA || _cmbProfileB.SelectedItem is not string nameB)
        {
            MessageBox.Show("Please select both profiles.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var tweaksA = new HashSet<string>(_engine.TweaksForProfile(nameA).Select(t => t.Id), StringComparer.OrdinalIgnoreCase);
        var tweaksB = new HashSet<string>(_engine.TweaksForProfile(nameB).Select(t => t.Id), StringComparer.OrdinalIgnoreCase);

        // Union of all tweak IDs from both profiles, deduplicated, sorted by category then label
        var allIds = tweaksA.Union(tweaksB, StringComparer.OrdinalIgnoreCase).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var allTweaks = _engine
            .AllTweaks()
            .Where(t => allIds.Contains(t.Id))
            .OrderBy(t => t.Category, StringComparer.OrdinalIgnoreCase)
            .ThenBy(t => t.Label, StringComparer.OrdinalIgnoreCase)
            .ToList();

        _rows = allTweaks.Select(t => (t, tweaksA.Contains(t.Id), tweaksB.Contains(t.Id))).ToList();

        Populate(nameA, nameB);
    }

    private void Populate(string nameA, string nameB)
    {
        _listView.SuspendLayout();
        _listView.Items.Clear();

        // Update column headers to profile names
        _listView.Columns[3].Text = $"In {nameA}";
        _listView.Columns[4].Text = $"In {nameB}";

        int onlyInA = 0,
            onlyInB = 0,
            inBoth = 0;

        for (int i = 0; i < _rows.Count; i++)
        {
            var (tweak, inA, inB) = _rows[i];
            string status;
            Color rowColor;

            if (inA && inB)
            {
                status = "Shared";
                rowColor = AppTheme.Surface;
                inBoth++;
            }
            else if (inA)
            {
                status = $"Only in {nameA}";
                rowColor = Color.FromArgb(30, AppTheme.Accent.R, AppTheme.Accent.G, AppTheme.Accent.B);
                onlyInA++;
            }
            else
            {
                status = $"Only in {nameB}";
                rowColor = Color.FromArgb(30, AppTheme.Green.R, AppTheme.Green.G, AppTheme.Green.B);
                onlyInB++;
            }

            var item = new ListViewItem((i + 1).ToString()) { BackColor = rowColor };
            item.SubItems.Add(tweak.Label);
            item.SubItems.Add(tweak.Category);
            item.SubItems.Add(inA ? "✓" : "–");
            item.SubItems.Add(inB ? "✓" : "–");
            item.SubItems.Add(status);
            _listView.Items.Add(item);
        }

        _listView.ResumeLayout(true);

        _lblSummary.Text =
            $"Total: {_rows.Count} tweaks   |   In {nameA}: {onlyInA + inBoth}   |   "
            + $"In {nameB}: {onlyInB + inBoth}   |   Shared: {inBoth}   |   "
            + $"Unique to {nameA}: {onlyInA}   |   Unique to {nameB}: {onlyInB}";
    }

    // ── HTML Export ────────────────────────────────────────────────────

    private void ExportHtml()
    {
        if (_rows.Count == 0)
        {
            MessageBox.Show("Run a comparison first.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        string nameA = _cmbProfileA.SelectedItem as string ?? "A";
        string nameB = _cmbProfileB.SelectedItem as string ?? "B";

        using var sfd = new SaveFileDialog
        {
            Title = "Export Profile Comparison",
            Filter = "HTML file (*.html)|*.html",
            FileName = $"profile-compare-{nameA}-vs-{nameB}.html",
        };

        if (sfd.ShowDialog(this) != DialogResult.OK)
            return;

        int onlyInA = _rows.Count(r => r.InA && !r.InB);
        int onlyInB = _rows.Count(r => !r.InA && r.InB);
        int inBoth = _rows.Count(r => r.InA && r.InB);

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html><head><meta charset=\"utf-8\">");
        sb.AppendLine($"<title>Profile Comparison: {nameA} vs {nameB}</title>");
        sb.AppendLine("<style>");
        sb.AppendLine("body { font-family: Segoe UI, Arial, sans-serif; background:#1e1e2e; color:#cdd6f4; margin:20px; }");
        sb.AppendLine("h1 { color:#89b4fa; } h2 { color:#cba6f7; margin-top:30px; }");
        sb.AppendLine(".summary { display:flex; gap:20px; flex-wrap:wrap; margin:12px 0; }");
        sb.AppendLine(".stat { background:#313244; border-radius:8px; padding:8px 18px; }");
        sb.AppendLine(".stat b { font-size:1.5em; display:block; }");
        sb.AppendLine("table { border-collapse:collapse; width:100%; margin-top:16px; }");
        sb.AppendLine("th { background:#313244; color:#89b4fa; padding:8px; border:1px solid #45475a; text-align:left; }");
        sb.AppendLine("td { padding:7px 8px; border:1px solid #45475a; font-size:0.9em; }");
        sb.AppendLine(".both { background:#1e1e2e; } .only-a { background:#1e2040; } .only-b { background:#1e3030; }");
        sb.AppendLine(".tick { color:#a6e3a1; text-align:center; } .dash { color:#585b70; text-align:center; }");
        sb.AppendLine("footer { margin-top:30px; color:#585b70; font-size:0.8em; }");
        sb.AppendLine("</style></head><body>");
        sb.AppendLine($"<h1>Profile Comparison: <em>{nameA}</em> vs <em>{nameB}</em></h1>");
        sb.AppendLine($"<p>Generated {DateTime.Now:yyyy-MM-dd HH:mm}</p>");
        sb.AppendLine("<div class=\"summary\">");
        sb.AppendLine($"<div class=\"stat\"><b>{_rows.Count}</b>Total tweaks</div>");
        sb.AppendLine($"<div class=\"stat\"><b>{onlyInA + inBoth}</b>In {nameA}</div>");
        sb.AppendLine($"<div class=\"stat\"><b>{onlyInB + inBoth}</b>In {nameB}</div>");
        sb.AppendLine($"<div class=\"stat\"><b>{inBoth}</b>Shared</div>");
        sb.AppendLine($"<div class=\"stat\"><b>{onlyInA}</b>Unique to {nameA}</div>");
        sb.AppendLine($"<div class=\"stat\"><b>{onlyInB}</b>Unique to {nameB}</div>");
        sb.AppendLine("</div>");
        sb.AppendLine("<table>");
        sb.AppendLine($"<tr><th>#</th><th>Tweak</th><th>Category</th><th>In {nameA}</th><th>In {nameB}</th><th>Status</th></tr>");

        for (int i = 0; i < _rows.Count; i++)
        {
            var (tweak, inA, inB) = _rows[i];
            string css = (inA && inB) ? "both" : (inA ? "only-a" : "only-b");
            string status = (inA && inB) ? "Shared" : (inA ? $"Only in {nameA}" : $"Only in {nameB}");
            string tickA = inA ? "<td class=\"tick\">✓</td>" : "<td class=\"dash\">–</td>";
            string tickB = inB ? "<td class=\"tick\">✓</td>" : "<td class=\"dash\">–</td>";
            sb.AppendLine(
                $"<tr class=\"{css}\"><td>{i + 1}</td><td>{System.Net.WebUtility.HtmlEncode(tweak.Label)}</td>"
                    + $"<td>{System.Net.WebUtility.HtmlEncode(tweak.Category)}</td>{tickA}{tickB}"
                    + $"<td>{status}</td></tr>"
            );
        }

        sb.AppendLine("</table>");
        sb.AppendLine("<footer>Generated by RegiLattice Profile Compare</footer>");
        sb.AppendLine("</body></html>");

        try
        {
            File.WriteAllText(sfd.FileName, sb.ToString(), System.Text.Encoding.UTF8);
            MessageBox.Show($"Saved: {sfd.FileName}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (IOException ex)
        {
            MessageBox.Show($"Export failed: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ── Theme ──────────────────────────────────────────────────────────

    private void ApplyTheme()
    {
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;

        _listView.BackColor = AppTheme.Surface;
        _listView.ForeColor = AppTheme.Fg;

        _btnCompare.BackColor = AppTheme.Accent;
        _btnCompare.ForeColor = AppTheme.Fg;
        _btnCompare.FlatStyle = FlatStyle.Flat;
        _btnCompare.FlatAppearance.BorderSize = 0;

        _btnExportHtml.BackColor = AppTheme.Surface2;
        _btnExportHtml.ForeColor = AppTheme.FgDim;
        _btnExportHtml.FlatStyle = FlatStyle.Flat;
        _btnExportHtml.FlatAppearance.BorderSize = 0;

        _btnClose.BackColor = AppTheme.Surface2;
        _btnClose.ForeColor = AppTheme.FgDim;
        _btnClose.FlatStyle = FlatStyle.Flat;
        _btnClose.FlatAppearance.BorderSize = 0;

        _lblSummary.ForeColor = AppTheme.FgDim;
        _lblSummary.BackColor = AppTheme.Surface;

        foreach (Control c in Controls)
            if (c is Panel p)
                p.BackColor = AppTheme.Surface;
    }
}
