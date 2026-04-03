#nullable enable
using System.Drawing.Drawing2D;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Services;

namespace RegiLattice.GUI.Controls;

/// <summary>
/// Home / Analytics dashboard panel.
/// Shows: health score ring, category breakdown bar chart, applied/total counts,
/// quick-action buttons (Smart Scan, Profile Wizard), and recently-applied tweaks.
/// </summary>
internal sealed class DashboardPanel : Panel
{
    // ── State ──────────────────────────────────────────────────────────────
    private TweakEngine? _engine;
    private IReadOnlyDictionary<string, TweakResult>? _statusCache;
    private HealthScore _score = new(0, 0, 0, 0);
    private List<CategoryStat> _catStats = [];
    private List<string> _recentTweaks = [];

    // ── Events ─────────────────────────────────────────────────────────────
    internal event Action? SmartScanRequested;
    internal event Action? ProfileWizardRequested;
    internal event Action? TweaksRequested;      // navigate → Tweaks section

    // ── Sub-controls ───────────────────────────────────────────────────────
    private readonly Button _btnSmartScan;
    private readonly Button _btnProfileWizard;
    private readonly Button _btnViewAll;

    // ── Construction ───────────────────────────────────────────────────────
    internal DashboardPanel()
    {
        SetStyle(
            ControlStyles.ResizeRedraw |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint,
            true);

        AutoScroll = true;
        Padding = new Padding(24, 16, 24, 16);

        // Quick-action buttons
        _btnSmartScan = CreateActionButton("⚡  Smart Scan", AppTheme.Accent);
        _btnSmartScan.Bounds = new Rectangle(24, 260, 160, 36);
        _btnSmartScan.Click += (_, _) => SmartScanRequested?.Invoke();

        _btnProfileWizard = CreateActionButton("🧭  Profile Wizard", AppTheme.Green);
        _btnProfileWizard.Bounds = new Rectangle(196, 260, 160, 36);
        _btnProfileWizard.Click += (_, _) => ProfileWizardRequested?.Invoke();

        _btnViewAll = CreateActionButton("→  View All Tweaks", AppTheme.FgDim);
        _btnViewAll.Bounds = new Rectangle(368, 260, 160, 36);
        _btnViewAll.Click += (_, _) => TweaksRequested?.Invoke();

        Controls.AddRange(new Control[] { _btnSmartScan, _btnProfileWizard, _btnViewAll });
    }

    // ── Public API ─────────────────────────────────────────────────────────
    internal void SetData(
        TweakEngine engine,
        IReadOnlyDictionary<string, TweakResult> statusCache,
        IEnumerable<string> recentTweaks)
    {
        _engine = engine;
        _statusCache = statusCache;

        // Compute health score
        int total   = statusCache.Count;
        int applied = statusCache.Values.Count(r => r == TweakResult.Applied);
        int skipped = statusCache.Values.Count(r =>
            r is TweakResult.SkippedCorp or TweakResult.SkippedBuild or TweakResult.SkippedHw);
        int errors  = statusCache.Values.Count(r => r == TweakResult.Error);
        _score = new HealthScore(total, applied, skipped, errors);

        // Build per-category stats
        _catStats = BuildCategoryStats(engine, statusCache);

        _recentTweaks = recentTweaks.Take(8).ToList();

        RepositionButtons();
        Invalidate();
    }

    internal void ApplyTheme()
    {
        BackColor = AppTheme.Bg;
        foreach (Control c in Controls)
            c.Invalidate();
        Invalidate();
    }

    // ── Helper types ───────────────────────────────────────────────────────
    private readonly record struct HealthScore(int Total, int Applied, int Skipped, int Errors)
    {
        internal int NotApplied => Total - Applied - Skipped - Errors;
        internal float Pct => Total == 0 ? 0f : (float)Applied / Total;
        internal Color Color => Pct switch
        {
            >= 0.80f => AppTheme.Green,
            >= 0.50f => AppTheme.Yellow,
            _ => AppTheme.Red,
        };
        internal string Label => Total == 0 ? "–" : $"{(int)(Pct * 100)}%";
    }

    private readonly record struct CategoryStat(string Name, int Total, int Applied);

    private static List<CategoryStat> BuildCategoryStats(
        TweakEngine engine,
        IReadOnlyDictionary<string, TweakResult> cache)
    {
        var counts = new Dictionary<string, (int total, int applied)>(StringComparer.OrdinalIgnoreCase);
        foreach (var td in engine.AllTweaks())
        {
            var (t, a) = counts.GetValueOrDefault(td.Category);
            bool isApplied = cache.TryGetValue(td.Id, out var r) && r == TweakResult.Applied;
            counts[td.Category] = (t + 1, a + (isApplied ? 1 : 0));
        }
        return counts
            .Select(kv => new CategoryStat(kv.Key, kv.Value.total, kv.Value.applied))
            .OrderByDescending(s => s.Applied)
            .Take(8)
            .ToList();
    }

    // ── Paint ──────────────────────────────────────────────────────────────
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode    = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        g.Clear(AppTheme.Bg);

        int w = ClientSize.Width;
        int pad = 24;

        // ── Section: page title ─────────────────────────────────────────
        using var titleFont = new Font(AppTheme.Bold.FontFamily, 14f, FontStyle.Bold);
        using var titleBrush = new SolidBrush(AppTheme.Fg);
        g.DrawString("System Overview", titleFont, titleBrush, pad, 16);

        // ── Row 1: Stat cards ───────────────────────────────────────────
        int cardY   = 52;
        int cardH   = 90;
        int cardGap = 12;
        int nCards  = 4;
        int cardW   = (w - pad * 2 - cardGap * (nCards - 1)) / nCards;

        DrawStatCard(g, new Rectangle(pad, cardY, cardW, cardH),
            _score.Label, "Health Score", _score.Color);
        DrawStatCard(g, new Rectangle(pad + (cardW + cardGap), cardY, cardW, cardH),
            _score.Applied.ToString(), "Applied", AppTheme.Green);
        DrawStatCard(g, new Rectangle(pad + (cardW + cardGap) * 2, cardY, cardW, cardH),
            _score.NotApplied.ToString(), "Not Applied", AppTheme.FgDim);
        DrawStatCard(g, new Rectangle(pad + (cardW + cardGap) * 3, cardY, cardW, cardH),
            _score.Total.ToString(), "Total Tweaks", AppTheme.Accent);

        // ── Row 2: Health ring + category bars ──────────────────────────
        int row2Y = cardY + cardH + 20;
        int row2H = 160;

        // Health score donut ring (left)
        int ringSize = 130;
        var ringRect = new Rectangle(pad, row2Y + (row2H - ringSize) / 2, ringSize, ringSize);
        DrawHealthRing(g, ringRect, _score);

        // Category bar chart (right of ring)
        int barsX = pad + ringSize + 24;
        int barsW = w - barsX - pad;
        DrawCategoryBars(g, new Rectangle(barsX, row2Y, barsW, row2H));

        // ── Section title: Quick Actions ────────────────────────────────
        int actY = row2Y + row2H + 16;
        using var subFont = new Font(AppTheme.Bold.FontFamily, 10f, FontStyle.Bold);
        using var dimBrush = new SolidBrush(AppTheme.FgDim);
        g.DrawString("Quick Actions", subFont, dimBrush, pad, actY);

        // Buttons positioned just below (updated in RepositionButtons)
        RepositionButtons(pad, actY + 24);

        // ── Section: Recent Activity ─────────────────────────────────────
        int recentY = actY + 24 + 46;
        DrawRecentActivity(g, pad, recentY, w - pad * 2);
    }

    private static void DrawStatCard(Graphics g, Rectangle r, string value, string label, Color accent)
    {
        // Card background
        using var surfBrush = new SolidBrush(AppTheme.Surface);
        AppTheme.FillRoundedRect(g, surfBrush, r, 10);

        // Left accent stripe
        using var accentBrush = new SolidBrush(accent);
        AppTheme.FillRoundedRect(g, accentBrush, new Rectangle(r.Left, r.Top + 10, 3, r.Height - 20), 2);

        // Value
        using var valFont = new Font(AppTheme.Bold.FontFamily, 22f, FontStyle.Bold);
        using var valBrush = new SolidBrush(accent);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString(value, valFont, valBrush, new RectangleF(r.Left + 10, r.Top + 10, r.Width - 14, r.Height * 0.6f), sf);

        // Label
        using var lblFont = new Font(AppTheme.Bold.FontFamily, 8.5f, FontStyle.Regular);
        using var lblBrush = new SolidBrush(AppTheme.FgDim);
        var lsf = new StringFormat { Alignment = StringAlignment.Center };
        g.DrawString(label, lblFont, lblBrush, new RectangleF(r.Left, r.Top + r.Height * 0.62f, r.Width, r.Height * 0.38f), lsf);
    }

    private static void DrawHealthRing(Graphics g, Rectangle r, HealthScore score)
    {
        float sweep = score.Pct * 360f;
        int thick = 12;
        var trackRect = r;

        // Track (background ring)
        using var trackPen = new Pen(AppTheme.Surface, thick);
        g.DrawArc(trackPen, trackRect, -90, 360);

        // Progress arc
        if (sweep > 0)
        {
            using var progressPen = new Pen(score.Color, thick) { StartCap = LineCap.Round, EndCap = LineCap.Round };
            g.DrawArc(progressPen, trackRect, -90, sweep);
        }

        // Center text
        using var pctFont = new Font(AppTheme.Bold.FontFamily, 16f, FontStyle.Bold);
        using var pctBrush = new SolidBrush(score.Color);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString(score.Label, pctFont, pctBrush, r, sf);
    }

    private void DrawCategoryBars(Graphics g, Rectangle r)
    {
        if (_catStats.Count == 0)
        {
            using var noDataBrush = new SolidBrush(AppTheme.FgDim);
            using var noDataFont = new Font(AppTheme.Bold.FontFamily, 9f, FontStyle.Regular);
            g.DrawString("Run Refresh to load stats", noDataFont, noDataBrush, r.X, r.Y + 20);
            return;
        }

        using var catFont  = new Font(AppTheme.Bold.FontFamily, 8.5f, FontStyle.Regular);
        using var catBrush = new SolidBrush(AppTheme.FgDim);

        int barH   = 12;
        int rowH   = r.Height / Math.Max(_catStats.Count, 1);
        int labelW = 110;
        int maxBarW = r.Width - labelW - 48;

        for (int i = 0; i < _catStats.Count; i++)
        {
            var stat = _catStats[i];
            int rowY = r.Y + i * rowH;
            int midY = rowY + (rowH - barH) / 2;

            // Category name
            string shortName = stat.Name.Length > 15 ? stat.Name[..15] + "…" : stat.Name;
            g.DrawString(shortName, catFont, catBrush, r.X, midY);

            // Track
            var trackRect = new Rectangle(r.X + labelW, midY, maxBarW, barH);
            using var trackBrush = new SolidBrush(AppTheme.Surface);
            AppTheme.FillRoundedRect(g, trackBrush, trackRect, 4);

            // Progress
            float fillFrac = stat.Total == 0 ? 0 : (float)stat.Applied / stat.Total;
            int fillW = (int)(fillFrac * maxBarW);
            if (fillW > 0)
            {
                Color barColor = fillFrac >= 0.8f ? AppTheme.Green
                              : fillFrac >= 0.4f ? AppTheme.Accent
                              : AppTheme.FgDim;
                using var barBrush = new SolidBrush(barColor);
                AppTheme.FillRoundedRect(g, barBrush, new Rectangle(trackRect.X, midY, fillW, barH), 4);
            }

            // Count
            using var countBrush = new SolidBrush(AppTheme.FgDim);
            g.DrawString($"{stat.Applied}/{stat.Total}", catFont, countBrush,
                r.X + labelW + maxBarW + 6, midY);
        }
    }

    private void DrawRecentActivity(Graphics g, int x, int y, int width)
    {
        using var hFont  = new Font(AppTheme.Bold.FontFamily, 10f, FontStyle.Bold);
        using var dimBrush = new SolidBrush(AppTheme.FgDim);
        g.DrawString("Recent Activity", hFont, dimBrush, x, y);

        if (_recentTweaks.Count == 0)
        {
            using var noFont  = new Font(AppTheme.Bold.FontFamily, 9f, FontStyle.Regular);
            using var noBrush = new SolidBrush(AppTheme.FgDim);
            g.DrawString("No recent tweaks applied yet.", noFont, noBrush, x, y + 24);
            return;
        }

        int rowH = 26;
        for (int i = 0; i < _recentTweaks.Count; i++)
        {
            int rowY = y + 22 + i * rowH;
            // Row background
            if (i % 2 == 0)
            {
                using var rowBrush = new SolidBrush(AppTheme.Surface);
                AppTheme.FillRoundedRect(g, rowBrush, new Rectangle(x, rowY, width, rowH - 2), 4);
            }

            string tweakId = _recentTweaks[i];
            string label   = _engine?.GetTweak(tweakId)?.Label ?? tweakId;

            using var dotBrush = new SolidBrush(AppTheme.Green);
            g.FillEllipse(dotBrush, x + 8, rowY + 9, 6, 6);

            using var itemFont  = new Font(AppTheme.Bold.FontFamily, 9f, FontStyle.Regular);
            using var itemBrush = new SolidBrush(AppTheme.Fg);
            g.DrawString(label, itemFont, itemBrush, x + 22, rowY + 5);
        }
    }

    // ── Button positioning ─────────────────────────────────────────────────
    private void RepositionButtons(int x = 24, int y = 260)
    {
        if (InvokeRequired) { BeginInvoke(() => RepositionButtons(x, y)); return; }
        _btnSmartScan.Location    = new Point(x, y);
        _btnProfileWizard.Location = new Point(x + 172, y);
        _btnViewAll.Location      = new Point(x + 344, y);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        Invalidate();
    }

    // ── Button factory ─────────────────────────────────────────────────────
    private static Button CreateActionButton(string text, Color accent)
    {
        var btn = new Button
        {
            Text      = text,
            FlatStyle = FlatStyle.Flat,
            Font      = new Font(AppTheme.Bold.FontFamily, 9f, FontStyle.Bold),
            ForeColor = accent,
            BackColor = Color.FromArgb(35, accent),
            Size      = new Size(160, 36),
            Cursor    = Cursors.Hand,
            TextAlign = ContentAlignment.MiddleCenter,
        };
        btn.FlatAppearance.BorderColor = accent;
        btn.FlatAppearance.BorderSize  = 1;
        btn.FlatAppearance.MouseOverBackColor  = Color.FromArgb(60, accent);
        btn.FlatAppearance.MouseDownBackColor  = Color.FromArgb(80, accent);
        return btn;
    }
}
