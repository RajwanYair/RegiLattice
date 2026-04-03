#nullable enable
using RegiLattice.GUI.Forms;

namespace RegiLattice.GUI.Controls;

/// <summary>
/// "Packages" section panel — a TabControl that hosts all five package manager
/// dialogs in embedded (non-modal) mode within the main window body.
/// Clicking a tab lazy-loads and embeds the corresponding manager dialog.
/// </summary>
internal sealed class PackagesHubPanel : Panel
{
    // ── Sub-controls ───────────────────────────────────────────────────────
    private readonly TabControl _tabs;
    private readonly TabPage    _tabScoop;
    private readonly TabPage    _tabWinGet;
    private readonly TabPage    _tabChoco;
    private readonly TabPage    _tabPip;
    private readonly TabPage    _tabPS;
    private readonly TabPage    _tabMarketplace;

    // ── Lazy-embedded dialogs (Form used as embedded panel via Parent trick) ─
    private Panel?  _scoopEmbed;
    private Panel?  _wingetEmbed;
    private Panel?  _chocoEmbed;
    private Panel?  _pipEmbed;
    private Panel?  _psEmbed;
    private Panel?  _marketplaceEmbed;

    // ── Construction ───────────────────────────────────────────────────────
    internal PackagesHubPanel()
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

        // Header label
        var header = new Label
        {
            Text      = "Package Managers",
            Font      = new Font(AppTheme.Bold.FontFamily, 14f, FontStyle.Bold),
            ForeColor = AppTheme.Fg,
            BackColor = AppTheme.Bg,
            AutoSize  = true,
            Location  = new Point(24, 14),
        };

        var subLabel = new Label
        {
            Text      = "Manage packages from Scoop, WinGet, Chocolatey, pip, and PowerShell Gallery.",
            Font      = new Font(AppTheme.Bold.FontFamily, 9f, FontStyle.Regular),
            ForeColor = AppTheme.FgDim,
            BackColor = AppTheme.Bg,
            AutoSize  = true,
            Location  = new Point(24, 42),
        };

        // Tabs
        _tabScoop       = new TabPage("🪣  Scoop");
        _tabWinGet      = new TabPage("📦  WinGet");
        _tabChoco       = new TabPage("🍫  Chocolatey");
        _tabPip         = new TabPage("🐍  pip");
        _tabPS          = new TabPage("⚡  PowerShell");
        _tabMarketplace = new TabPage("🛒  Marketplace");

        _tabs = new TabControl
        {
            Dock     = DockStyle.Fill,
            Font     = new Font(AppTheme.Bold.FontFamily, 9f, FontStyle.Regular),
            DrawMode = TabDrawMode.OwnerDrawFixed,
            ItemSize = new Size(130, 30),
        };
        _tabs.TabPages.AddRange(new TabPage[]
        {
            _tabScoop, _tabWinGet, _tabChoco, _tabPip, _tabPS, _tabMarketplace,
        });
        _tabs.DrawItem     += OnDrawTab;
        _tabs.SelectedIndexChanged += OnTabSelected;

        // Header panel
        var headerPanel = new Panel
        {
            Dock   = DockStyle.Top,
            Height = 64,
        };
        headerPanel.Controls.AddRange(new Control[] { subLabel, header });
        headerPanel.BackColor = AppTheme.Bg;

        Controls.Add(_tabs);
        Controls.Add(headerPanel);
    }

    // ── Lazy-load embedded managers ────────────────────────────────────────
    private void OnTabSelected(object? sender, EventArgs e)
    {
        switch (_tabs.SelectedIndex)
        {
            case 0: EnsureEmbedded(ref _scoopEmbed,       _tabScoop,       CreateScoopEmbed);       break;
            case 1: EnsureEmbedded(ref _wingetEmbed,      _tabWinGet,      CreateWinGetEmbed);      break;
            case 2: EnsureEmbedded(ref _chocoEmbed,       _tabChoco,       CreateChocoEmbed);       break;
            case 3: EnsureEmbedded(ref _pipEmbed,         _tabPip,         CreatePipEmbed);         break;
            case 4: EnsureEmbedded(ref _psEmbed,          _tabPS,          CreatePSEmbed);          break;
            case 5: EnsureEmbedded(ref _marketplaceEmbed, _tabMarketplace, CreateMarketplaceEmbed); break;
        }
    }

    private static void EnsureEmbedded(ref Panel? embed, TabPage tab, Func<Panel> factory)
    {
        if (embed is not null) return;
        embed = factory();
        embed.Dock = DockStyle.Fill;
        tab.Controls.Add(embed);
        tab.BackColor = AppTheme.Bg;
    }

    // Each Create*Embed opens the corresponding dialog, then reparents its
    // content into a plain Panel so it embeds seamlessly in the tab.
    private static Panel CreateScoopEmbed()
        => BuildEmbeddedPanel(new ScoopManagerDialog());

    private static Panel CreateWinGetEmbed()
        => BuildEmbeddedPanel(new WinGetManagerDialog());

    private static Panel CreateChocoEmbed()
        => BuildEmbeddedPanel(new ChocolateyManagerDialog());

    private static Panel CreatePipEmbed()
        => BuildEmbeddedPanel(new PipManagerDialog());

    private static Panel CreatePSEmbed()
        => BuildEmbeddedPanel(new PSModuleManagerDialog());

    private static Panel CreateMarketplaceEmbed()
        => BuildEmbeddedPanel(new MarketplaceDialog());

    /// <summary>
    /// Reparents the Form's controls into a Panel so the dialog can be
    /// displayed inside a tab without a window chrome or modal pump.
    /// This works because WinForms Forms are just Panels at the Win32 level.
    /// </summary>
    private static Panel BuildEmbeddedPanel(Form dlg)
    {
        var container = new Panel
        {
            BackColor = AppTheme.Bg,
        };

        // Force the form to create its controls
        dlg.TopLevel = false;
        dlg.FormBorderStyle = FormBorderStyle.None;
        dlg.Dock    = DockStyle.Fill;
        dlg.BackColor = AppTheme.Bg;
        dlg.Parent  = container;
        dlg.Visible = true;

        container.Controls.Add(dlg);
        return container;
    }

    // ── Owner-drawn tabs ───────────────────────────────────────────────────
    private void OnDrawTab(object? sender, DrawItemEventArgs e)
    {
        var tab = _tabs.TabPages[e.Index];
        bool selected = e.Index == _tabs.SelectedIndex;
        var r = e.Bounds;

        var g = e.Graphics;
        g.Clear(selected ? AppTheme.Surface : AppTheme.Bg);

        if (selected)
        {
            using var accentBrush = new SolidBrush(AppTheme.Accent);
            g.FillRectangle(accentBrush, new Rectangle(r.Left, r.Bottom - 2, r.Width, 2));
        }

        using var textBrush = new SolidBrush(selected ? AppTheme.Fg : AppTheme.FgDim);
        using var font = selected
            ? new Font(AppTheme.Bold.FontFamily, 9f, FontStyle.Bold)
            : new Font(AppTheme.Bold.FontFamily, 9f, FontStyle.Regular);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString(tab.Text, font, textBrush, r, sf);
    }

    internal void ApplyTheme()
    {
        BackColor = AppTheme.Bg;
        _tabs.BackColor = AppTheme.Bg;
        foreach (TabPage tp in _tabs.TabPages)
            tp.BackColor = AppTheme.Bg;
        Invalidate(true);
    }
}
