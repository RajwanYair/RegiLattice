// RegiLattice.GUI — Forms/FirstRunWizardDialog.cs
// 3-panel first-run wizard: profile selection → dry-run toggle → confirmation.
// Shown once on the very first launch.  Use ShouldShow() / MarkSeen() guards.

namespace RegiLattice.GUI.Forms;

using RegiLattice.Core;
using RegiLattice.Core.Services;

/// <summary>
/// A 3-step first-run wizard that guides new users through choosing an initial
/// profile, toggling dry-run mode, and confirming their choices.
/// </summary>
internal sealed class FirstRunWizardDialog : Form
{
    // Wizard pages (0-indexed)
    private const int PageProfile = 0;
    private const int PageDryRun = 1;
    private const int PageConfirm = 2;
    private const int TotalPages = 3;

    private int _page;

    // Profile choice
    private readonly RadioButton[] _profileRadios;
    private readonly string[] _profileIds = ["minimal", "privacy", "business", "gaming", "server"];
    private readonly string[] _profileNames = ["Minimal", "Privacy", "Business", "Gaming", "Server"];
    private readonly string[] _profileDescs =
    [
        "Fast, clean essentials — lightweight desktop or SFF PC.",
        "Maximum telemetry removal and cloud isolation (private user).",
        "Productivity, security, cloud & workflow (work laptop).",
        "GPU, low-latency, game mode — dedicated gaming rig.",
        "Hardened, headless, remote management (home server / VM).",
    ];

    // Dry-run toggle
    private readonly CheckBox _chkDryRun;

    // Page container
    private readonly Panel[] _pages;

    // Nav buttons
    private readonly Button _btnBack;
    private readonly Button _btnNext;
    private readonly Button _btnFinish;

    // Result
    public string? SelectedProfile { get; private set; }
    public bool EnableDryRun { get; private set; }

    internal FirstRunWizardDialog()
    {
        Text = "Welcome to RegiLattice — Quick Setup";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(560, 420);
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        Font = AppTheme.Regular;
        Icon = AppIcons.AppIcon;

        // ── Header strip ───────────────────────────────────────────────────
        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 64,
            BackColor = AppTheme.Surface,
            Padding = new Padding(16, 0, 0, 0),
        };
        var lblTitle = new Label
        {
            AutoSize = false,
            Dock = DockStyle.Fill,
            Text = "RegiLattice — First-Run Setup",
            Font = AppTheme.Title,
            ForeColor = AppTheme.Accent,
            TextAlign = ContentAlignment.MiddleLeft,
        };
        header.Controls.Add(lblTitle);

        // ── Page host ───────────────────────────────────────────────────────
        var pageHost = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(16),
            BackColor = AppTheme.Bg,
        };

        // Build profile radio buttons for Page 0
        _profileRadios = new RadioButton[_profileIds.Length];
        var profilePage = BuildProfilePage();
        _chkDryRun = new CheckBox();
        var dryRunPage = BuildDryRunPage();
        var confirmPage = BuildConfirmPage();

        _pages = [profilePage, dryRunPage, confirmPage];
        foreach (var p in _pages)
        {
            p.Dock = DockStyle.Fill;
            pageHost.Controls.Add(p);
        }

        // ── Footer / nav buttons ────────────────────────────────────────────
        var footer = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 52,
            BackColor = AppTheme.Surface,
        };

        _btnBack = new Button
        {
            Text = "← Back",
            Width = 90,
            Height = 32,
            Left = 260,
            Top = 10,
            BackColor = AppTheme.Surface2,
            ForeColor = AppTheme.Fg,
            FlatStyle = FlatStyle.Flat,
            Enabled = false,
        };
        _btnBack.FlatAppearance.BorderColor = AppTheme.Border;

        _btnNext = new Button
        {
            Text = "Next →",
            Width = 90,
            Height = 32,
            Left = 360,
            Top = 10,
            BackColor = AppTheme.Accent,
            ForeColor = AppTheme.Bg,
            FlatStyle = FlatStyle.Flat,
        };
        _btnNext.FlatAppearance.BorderColor = AppTheme.Accent;

        _btnFinish = new Button
        {
            Text = "Finish ✓",
            Width = 90,
            Height = 32,
            Left = 460,
            Top = 10,
            BackColor = AppTheme.Success,
            ForeColor = AppTheme.Bg,
            FlatStyle = FlatStyle.Flat,
            Visible = false,
        };
        _btnFinish.FlatAppearance.BorderColor = AppTheme.Success;

        var btnSkip = new Button
        {
            Text = "Skip",
            Width = 70,
            Height = 32,
            Left = 10,
            Top = 10,
            BackColor = AppTheme.Surface2,
            ForeColor = AppTheme.FgDim,
            FlatStyle = FlatStyle.Flat,
        };
        btnSkip.FlatAppearance.BorderColor = AppTheme.Border;

        footer.Controls.AddRange([btnSkip, _btnBack, _btnNext, _btnFinish]);

        _btnBack.Click += (_, _) => Navigate(-1);
        _btnNext.Click += (_, _) => Navigate(+1);
        _btnFinish.Click += OnFinish;
        btnSkip.Click += (_, _) =>
        {
            DialogResult = DialogResult.Cancel;
            Close();
        };

        Controls.AddRange([header, footer, pageHost]);
        ShowPage(0);
        AppTheme.Apply3D(this);
    }

    // ── Page builders ────────────────────────────────────────────────────────

    private Panel BuildProfilePage()
    {
        var pnl = new Panel { BackColor = AppTheme.Bg };

        var lbl = new Label
        {
            Text = "Step 1 of 3: Choose a starting profile",
            Font = AppTheme.Bold,
            ForeColor = AppTheme.Fg,
            AutoSize = false,
            Size = new Size(520, 28),
            Location = new Point(0, 0),
        };
        var sub = new Label
        {
            Text = "Select the profile that best describes how you use this PC. " + "You can change or fine-tune individual tweaks after setup.",
            AutoSize = false,
            ForeColor = AppTheme.FgDim,
            Size = new Size(520, 36),
            Location = new Point(0, 32),
        };
        pnl.Controls.AddRange([lbl, sub]);

        for (int i = 0; i < _profileIds.Length; i++)
        {
            var rb = new RadioButton
            {
                Text = $"{_profileNames[i]}  —  {_profileDescs[i]}",
                AutoSize = false,
                Size = new Size(510, 30),
                Location = new Point(6, 76 + i * 36),
                ForeColor = AppTheme.Fg,
                BackColor = AppTheme.Bg,
                Checked = i == 0,
            };
            _profileRadios[i] = rb;
            pnl.Controls.Add(rb);
        }
        return pnl;
    }

    private Panel BuildDryRunPage()
    {
        var pnl = new Panel { BackColor = AppTheme.Bg };

        var lbl = new Label
        {
            Text = "Step 2 of 3: Dry-Run mode",
            Font = AppTheme.Bold,
            ForeColor = AppTheme.Fg,
            AutoSize = false,
            Size = new Size(520, 28),
            Location = new Point(0, 0),
        };
        var desc = new Label
        {
            Text =
                "Dry-Run mode lets you preview all changes without writing anything to the registry.\n\n"
                + "Enable it if you want to examine what the selected profile would do before committing.\n\n"
                + "You can always toggle Dry-Run later from the File menu.",
            AutoSize = false,
            ForeColor = AppTheme.FgDim,
            Size = new Size(510, 100),
            Location = new Point(0, 34),
        };

        _chkDryRun.Text = "Enable Dry-Run mode (preview only — no registry changes)";
        _chkDryRun.AutoSize = false;
        _chkDryRun.Size = new Size(520, 28);
        _chkDryRun.Location = new Point(6, 150);
        _chkDryRun.ForeColor = AppTheme.Accent;
        _chkDryRun.BackColor = AppTheme.Bg;
        _chkDryRun.Checked = false;

        pnl.Controls.AddRange([lbl, desc, _chkDryRun]);
        return pnl;
    }

    private Panel BuildConfirmPage()
    {
        var pnl = new Panel { BackColor = AppTheme.Bg };

        var lbl = new Label
        {
            Text = "Step 3 of 3: Ready to go",
            Font = AppTheme.Bold,
            ForeColor = AppTheme.Fg,
            AutoSize = false,
            Size = new Size(520, 28),
            Location = new Point(0, 0),
        };
        var desc = new Label
        {
            AutoSize = false,
            ForeColor = AppTheme.FgDim,
            Size = new Size(510, 220),
            Location = new Point(0, 34),
        };
        // Updated dynamically in ShowPage()
        desc.Tag = "confirm-desc";
        pnl.Controls.AddRange([lbl, desc]);
        return pnl;
    }

    // ── Navigation ────────────────────────────────────────────────────────────

    private void Navigate(int delta)
    {
        var next = _page + delta;
        if (next < 0 || next >= TotalPages)
            return;
        ShowPage(next);
    }

    private void ShowPage(int index)
    {
        _page = index;
        for (int i = 0; i < _pages.Length; i++)
            _pages[i].Visible = (i == index);

        _btnBack.Enabled = index > 0;
        _btnNext.Visible = index < TotalPages - 1;
        _btnFinish.Visible = index == TotalPages - 1;

        // Update confirm page text when landing on it
        if (index == PageConfirm)
        {
            var selectedName = _profileNames[SelectedProfileIndex()];
            var desc = _pages[PageConfirm].Controls.OfType<Label>().FirstOrDefault(l => l.Tag?.ToString() == "confirm-desc");
            if (desc is not null)
            {
                desc.Text =
                    $"Your choice:  {selectedName} profile\n"
                    + $"Dry-Run mode: {(_chkDryRun.Checked ? "Enabled (preview only)" : "Disabled (real changes)")}\n\n"
                    + "Click Finish to apply these settings and open the main window.\n\n"
                    + "The profile will not be applied automatically — use the Profile menu "
                    + "or the Apply Profile button in the toolbar to apply tweaks.";
            }
        }
    }

    private int SelectedProfileIndex()
    {
        for (int i = 0; i < _profileRadios.Length; i++)
            if (_profileRadios[i].Checked)
                return i;
        return 0;
    }

    // ── Finish ────────────────────────────────────────────────────────────────

    private void OnFinish(object? sender, EventArgs e)
    {
        int idx = SelectedProfileIndex();
        SelectedProfile = _profileIds[idx];
        EnableDryRun = _chkDryRun.Checked;
        DialogResult = DialogResult.OK;
        Close();
    }

    // ── Static guards ─────────────────────────────────────────────────────────

    /// <summary>
    /// Returns <see langword="true"/> when the wizard should be shown on this launch.
    /// </summary>
    internal static bool ShouldShow()
    {
        var cfg = AppConfig.Load();
        return cfg.FirstRunWizardPending;
    }

    /// <summary>
    /// Persists that the wizard has been seen, suppressing it on future launches.
    /// </summary>
    internal static void MarkSeen()
    {
        var cfg = AppConfig.Load();
        cfg.FirstRunWizardPending = false;
        cfg.Save();
    }
}
