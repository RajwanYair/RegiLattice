// RegiLattice.GUI — Forms/ComplianceTrendDialog.cs
// Sprint 128: Compliance Trend Dashboard — GDI+ line chart of compliance score
// and violation count over time, drawn from ComplianceHistory.

#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;
using RegiLattice.Core;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Displays a line chart of historical compliance scores and violation counts
/// sourced from <see cref="ComplianceHistory"/>.
/// The chart is rendered entirely with GDI+ — no external charting library.
/// </summary>
[SupportedOSPlatform("windows")]
internal sealed class ComplianceTrendDialog : BaseDialog
{
    // ── Chart metrics ────────────────────────────────────────────────────────
    private const int PadL = 60; // left axis space
    private const int PadR = 20; // right margin
    private const int PadT = 30; // top margin
    private const int PadB = 50; // bottom axis space

    // ── Data ─────────────────────────────────────────────────────────────────
    private IReadOnlyList<ComplianceHistoryEntry> _history = [];

    // ── Controls ─────────────────────────────────────────────────────────────
    private readonly Panel _chartPanel;
    private readonly Label _lblTitle;
    private readonly Label _lblNoData;
    private readonly Label _lblSummary;
    private readonly FlowLayoutPanel _legend;
    private readonly Button _btnRefresh;
    private readonly Button _btnClear;
    private readonly Button _btnClose;
    private readonly FlowLayoutPanel _btnPanel;

    // ── Chart mode: score % or violation count ───────────────────────────────
    private bool _showViolations;
    private readonly CheckBox _chkMode;

    // ── Construction ─────────────────────────────────────────────────────────
    internal ComplianceTrendDialog()
        : base("Compliance Trend Dashboard", new Size(780, 520), resizable: true)
    {
        MinimumSize = new Size(520, 380);

        // Title label
        _lblTitle = new Label
        {
            Text = "Compliance History",
            Font = new Font(SystemFonts.DefaultFont.FontFamily, 11f, FontStyle.Bold),
            Dock = DockStyle.Top,
            Height = 34,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(10, 0, 0, 0),
        };

        // Chart area
        _chartPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
        _chartPanel.Paint += (_, e) => DrawChart(e.Graphics, _chartPanel.ClientRectangle);

        // No-data overlay
        _lblNoData = new Label
        {
            Text = "No compliance history yet.\nRun --compliance-history to generate entries.",
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill,
            Font = new Font(SystemFonts.DefaultFont.FontFamily, 9f),
            ForeColor = Color.Gray,
            Visible = false,
        };
        _chartPanel.Controls.Add(_lblNoData);

        // Summary strip
        _lblSummary = new Label
        {
            Dock = DockStyle.Top,
            Height = 22,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(10, 0, 0, 0),
            Font = new Font(SystemFonts.DefaultFont.FontFamily, 8.5f),
        };

        // Legend row
        _legend = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 24,
            Padding = new Padding(8, 4, 0, 0),
        };
        _legend.Controls.Add(MakeLegendSwatch(Color.SteelBlue, "Compliance %"));
        _legend.Controls.Add(MakeLegendSwatch(Color.Tomato, "Violations"));

        // Chart-mode toggle
        _chkMode = new CheckBox
        {
            Text = "Show violation count instead of %",
            AutoSize = true,
            Dock = DockStyle.Top,
            Margin = new Padding(8, 4, 0, 0),
        };
        _chkMode.CheckedChanged += (_, _) =>
        {
            _showViolations = _chkMode.Checked;
            _chartPanel.Invalidate();
        };

        // Buttons
        _btnRefresh = new Button { Text = "Refresh", Width = 80 };
        _btnClear = new Button { Text = "Clear History", Width = 100 };
        _btnClose = new Button { Text = "Close", Width = 80 };
        _btnRefresh.Click += (_, _) => LoadData();
        _btnClear.Click += OnClearHistory;
        _btnClose.Click += (_, _) => Close();

        _btnPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 42,
            Padding = new Padding(6, 6, 6, 4),
            FlowDirection = FlowDirection.LeftToRight,
        };
        _btnPanel.Controls.AddRange(new Control[] { _btnRefresh, _btnClear, _btnClose });

        // Layout
        Controls.Add(_chartPanel);
        Controls.Add(_chkMode);
        Controls.Add(_legend);
        Controls.Add(_lblSummary);
        Controls.Add(_lblTitle);
        Controls.Add(_btnPanel);

        LoadData();
    }

    // ── Data loading ──────────────────────────────────────────────────────────

    private void LoadData()
    {
        _history = ComplianceHistory.GetHistory();
        UpdateSummary();
        bool hasData = _history.Count > 0;
        _lblNoData.Visible = !hasData;
        _chartPanel.Invalidate();
    }

    private void UpdateSummary()
    {
        if (_history.Count == 0)
        {
            _lblSummary.Text = "No data.";
            return;
        }

        int total = _history.Count;
        int compliantDays = _history.Count(e => e.IsCompliant);
        double avgViolations = _history.Average(e => e.ViolationCount);
        var latest = _history[^1];

        _lblSummary.Text =
            $"Entries: {total}  |  Compliant: {compliantDays}/{total}  |  "
            + $"Avg Violations: {avgViolations:F1}  |  "
            + $"Last check: {latest.CheckedAt.ToLocalTime():yyyy-MM-dd HH:mm}  "
            + $"({(latest.IsCompliant ? "✔ Compliant" : $"✘ {latest.ViolationCount} violations")})";
    }

    // ── GDI+ chart rendering ──────────────────────────────────────────────────

    private void DrawChart(Graphics g, Rectangle bounds)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        // Background
        using var bgBrush = new SolidBrush(Color.WhiteSmoke);
        g.FillRectangle(bgBrush, bounds);

        if (_history.Count == 0)
            return;

        // Chart area
        var chart = new Rectangle(
            bounds.X + PadL,
            bounds.Y + PadT,
            Math.Max(1, bounds.Width - PadL - PadR),
            Math.Max(1, bounds.Height - PadT - PadB)
        );

        // Grid + Y axis
        DrawAxesAndGrid(g, chart);

        // Data line
        if (_history.Count == 1)
        {
            // Single point — draw a dot only
            double val = GetValue(_history[0]);
            double maxY = GetMaxY();
            float cx = chart.Left + chart.Width / 2f;
            float cy = ValueToY(val, maxY, chart);
            using var pointBrush = new SolidBrush(_showViolations ? Color.Tomato : Color.SteelBlue);
            g.FillEllipse(pointBrush, cx - 4, cy - 4, 8, 8);
        }
        else
        {
            DrawDataLine(g, chart);
        }

        // X-axis date labels
        DrawXLabels(g, chart);
    }

    private void DrawAxesAndGrid(Graphics g, Rectangle chart)
    {
        double maxY = GetMaxY();
        int gridLines = 4;
        using var gridPen = new Pen(Color.LightGray, 1f) { DashStyle = DashStyle.Dash };
        using var axisPen = new Pen(Color.DimGray, 1f);
        using var axisBrush = new SolidBrush(Color.DimGray);
        using var axisFont = new Font(FontFamily.GenericSansSerif, 7.5f);

        // Y-axis line
        g.DrawLine(axisPen, chart.Left, chart.Top, chart.Left, chart.Bottom);
        // X-axis line
        g.DrawLine(axisPen, chart.Left, chart.Bottom, chart.Right, chart.Bottom);

        for (int i = 0; i <= gridLines; i++)
        {
            double fraction = (double)i / gridLines;
            float y = chart.Bottom - (float)(fraction * chart.Height);
            g.DrawLine(gridPen, chart.Left, y, chart.Right, y);

            double labelVal = fraction * maxY;
            string label = _showViolations ? $"{(int)labelVal}" : $"{labelVal:F0}%";
            var sz = g.MeasureString(label, axisFont);
            g.DrawString(label, axisFont, axisBrush, chart.Left - sz.Width - 4, y - sz.Height / 2);
        }

        // Y-axis title
        using var titleFont = new Font(FontFamily.GenericSansSerif, 7.5f);
        string axisTitle = _showViolations ? "Violations" : "Compliance %";
        var titleSz = g.MeasureString(axisTitle, titleFont);
        var titleState = g.Save();
        g.TranslateTransform(chart.Left - 48, chart.Top + chart.Height / 2f + titleSz.Width / 2);
        g.RotateTransform(-90);
        g.DrawString(axisTitle, titleFont, axisBrush, 0, 0);
        g.Restore(titleState);
    }

    private void DrawDataLine(Graphics g, Rectangle chart)
    {
        double maxY = GetMaxY();
        int n = _history.Count;
        var points = new PointF[n];

        for (int i = 0; i < n; i++)
        {
            float x = chart.Left + (float)i / (n - 1) * chart.Width;
            float y = ValueToY(GetValue(_history[i]), maxY, chart);
            points[i] = new PointF(x, y);
        }

        // Fill area under line
        var fillPts = new PointF[n + 2];
        fillPts[0] = new PointF(points[0].X, chart.Bottom);
        for (int i = 0; i < n; i++)
            fillPts[i + 1] = points[i];
        fillPts[n + 1] = new PointF(points[n - 1].X, chart.Bottom);

        Color lineColor = _showViolations ? Color.Tomato : Color.SteelBlue;
        using var fillBrush = new SolidBrush(Color.FromArgb(40, lineColor));
        g.FillPolygon(fillBrush, fillPts);

        // Line
        using var linePen = new Pen(lineColor, 2f) { LineJoin = LineJoin.Round };
        g.DrawLines(linePen, points);

        // Data point dots + tooltip-style labels for compliance threshold changes
        using var dotBrush = new SolidBrush(lineColor);
        using var badDotBrush = new SolidBrush(Color.FromArgb(180, 200, 0, 0));
        for (int i = 0; i < n; i++)
        {
            bool ok = _history[i].IsCompliant;
            var brush = ok ? dotBrush : badDotBrush;
            g.FillEllipse(brush, points[i].X - 3, points[i].Y - 3, 6, 6);
        }
    }

    private void DrawXLabels(Graphics g, Rectangle chart)
    {
        int n = _history.Count;
        int maxLabels = Math.Min(n, chart.Width / 55);
        if (maxLabels < 2)
            maxLabels = 2;

        using var labelFont = new Font(FontFamily.GenericSansSerif, 7f);
        using var labelBrush = new SolidBrush(Color.DimGray);
        using var axisPen = new Pen(Color.DimGray, 1f);

        var step = Math.Max(1, (n - 1) / (maxLabels - 1));
        for (int i = 0; i < n; i += step)
        {
            float x = n == 1 ? chart.Left + chart.Width / 2f : chart.Left + (float)i / (n - 1) * chart.Width;

            // Tick
            g.DrawLine(axisPen, x, chart.Bottom, x, chart.Bottom + 4);

            // Date label (rotated 45°)
            string lbl = _history[i].CheckedAt.ToLocalTime().ToString("MM/dd\nHH:mm");
            var sz = g.MeasureString(lbl, labelFont);
            var state = g.Save();
            g.TranslateTransform(x, chart.Bottom + 6);
            g.RotateTransform(35);
            g.DrawString(lbl, labelFont, labelBrush, 0, 0);
            g.Restore(state);
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private double GetValue(ComplianceHistoryEntry e) =>
        _showViolations ? e.ViolationCount
        : e.TotalChecked > 0 ? (double)(e.TotalChecked - e.ViolationCount) / e.TotalChecked * 100.0
        : 0.0;

    private double GetMaxY()
    {
        if (_history.Count == 0)
            return 100;
        double max = _history.Max(e => GetValue(e));
        if (!_showViolations)
            return 100;
        return max <= 0 ? 10 : Math.Ceiling(max * 1.1);
    }

    private static float ValueToY(double value, double maxY, Rectangle chart)
    {
        float fraction = maxY > 0 ? (float)(value / maxY) : 0f;
        fraction = Math.Clamp(fraction, 0f, 1f);
        return chart.Bottom - fraction * chart.Height;
    }

    private static Label MakeLegendSwatch(Color c, string text)
    {
        var lbl = new Label
        {
            Text = $"■ {text}",
            AutoSize = true,
            ForeColor = c,
            Margin = new Padding(8, 0, 0, 0),
            Font = new Font(SystemFonts.DefaultFont.FontFamily, 8.5f),
        };
        return lbl;
    }

    private void OnClearHistory(object? sender, EventArgs e)
    {
        var result = MessageBox.Show(
            "Clear all compliance history?\nThis action cannot be undone.",
            "Confirm Clear",
            MessageBoxButtons.OKCancel,
            MessageBoxIcon.Warning
        );
        if (result != DialogResult.OK)
            return;

        ComplianceHistory.Clear();
        LoadData();
    }
}
