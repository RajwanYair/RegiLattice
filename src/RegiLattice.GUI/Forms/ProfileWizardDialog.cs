// RegiLattice.GUI — Forms/ProfileWizardDialog.cs
// 5-question YES/NO recommendation wizard that scores the user's usage patterns
// and suggests the most suitable profile (business / gaming / privacy / minimal / server).
// Accessible from the Tools menu as "Profile Recommendation Wizard…".

namespace RegiLattice.GUI.Forms;

using RegiLattice.Core;

/// <summary>
/// Interactive wizard that asks 5 yes/no questions about the user's
/// machine usage and recommends the optimal starting profile.
/// </summary>
internal sealed class ProfileWizardDialog : Form
{
    // ── Question set ─────────────────────────────────────────────────────────

    private static readonly string[] Questions =
    [
        "Is this a dedicated gaming or high-performance PC?",
        "Is this PC managed by a company, school, or organisation (corporate / enterprise)?",
        "Is privacy your top concern (minimise telemetry, cloud, tracking)?",
        "Is this a laptop where battery life and power efficiency matter?",
        "Is this a server, VM, or headless machine (no primary desktop use)?",
    ];

    // Score contribution: [gaming, business, privacy, minimal, server]
    private static readonly int[][] Weights =
    [
        [5, 0, 0, 0, 0], // gaming machine
        [0, 5, 1, 0, 2], // corporate / enterprise
        [0, 0, 5, 1, 0], // privacy-focused
        [0, 0, 1, 5, 0], // battery / power-efficient
        [0, 2, 1, 2, 5], // server / headless
    ];

    private static readonly string[] ProfileIds = ["gaming", "business", "privacy", "minimal", "server"];
    private static readonly string[] ProfileNames = ["Gaming", "Business", "Privacy", "Minimal", "Server"];

    // ── State ────────────────────────────────────────────────────────────────

    private readonly bool[] _answers = new bool[Questions.Length]; // true = Yes
    private int _currentQ;
    private string _recommendedProfile = "minimal";

    // ── Controls ─────────────────────────────────────────────────────────────

    private readonly Label _lblProgress;
    private readonly Label _lblQuestion;
    private readonly Button _btnYes;
    private readonly Button _btnNo;
    private readonly Panel _resultPanel;
    private readonly Label _lblResult;
    private readonly Button _btnApply;

    internal ProfileWizardDialog()
    {
        Text = "Profile Recommendation Wizard";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(520, 360);
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        Font = AppTheme.Regular;
        Icon = AppIcons.AppIcon;

        // ── Header ───────────────────────────────────────────────────────────
        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 56,
            BackColor = AppTheme.Surface,
            Padding = new Padding(16, 0, 0, 0),
        };
        var lblTitle = new Label
        {
            AutoSize = false,
            Dock = DockStyle.Fill,
            Text = "Profile Recommendation Wizard",
            Font = AppTheme.Title,
            ForeColor = AppTheme.Accent,
            TextAlign = ContentAlignment.MiddleLeft,
        };
        header.Controls.Add(lblTitle);

        // ── Body ─────────────────────────────────────────────────────────────
        var body = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(24, 16, 24, 8),
            BackColor = AppTheme.Bg,
        };

        _lblProgress = new Label
        {
            AutoSize = false,
            Text = $"Question 1 of {Questions.Length}",
            ForeColor = AppTheme.FgDim,
            Font = AppTheme.Small,
            Size = new Size(460, 20),
            Location = new Point(0, 0),
        };
        _lblQuestion = new Label
        {
            AutoSize = false,
            Text = Questions[0],
            ForeColor = AppTheme.Fg,
            Font = AppTheme.Bold,
            Size = new Size(460, 60),
            Location = new Point(0, 26),
        };
        _btnYes = new Button
        {
            Text = "Yes",
            Size = new Size(110, 36),
            Location = new Point(0, 104),
            BackColor = AppTheme.Success,
            ForeColor = AppTheme.Bg,
            FlatStyle = FlatStyle.Flat,
        };
        _btnYes.FlatAppearance.BorderColor = AppTheme.Success;

        _btnNo = new Button
        {
            Text = "No",
            Size = new Size(110, 36),
            Location = new Point(124, 104),
            BackColor = AppTheme.Surface2,
            ForeColor = AppTheme.Fg,
            FlatStyle = FlatStyle.Flat,
        };
        _btnNo.FlatAppearance.BorderColor = AppTheme.Border;

        // Result panel (initially hidden) — offset by Padding.Top so it clears below the header
        _resultPanel = new Panel
        {
            Size = new Size(460, 200),
            Location = new Point(0, 16),
            BackColor = AppTheme.Bg,
            Visible = false,
        };
        _lblResult = new Label
        {
            AutoSize = false,
            Size = new Size(460, 110),
            Location = new Point(0, 0),
            ForeColor = AppTheme.Fg,
            Font = AppTheme.Bold,
        };
        _btnApply = new Button
        {
            Text = "Use this profile →",
            Size = new Size(180, 36),
            Location = new Point(0, 120),
            BackColor = AppTheme.Accent,
            ForeColor = AppTheme.Bg,
            FlatStyle = FlatStyle.Flat,
        };
        _btnApply.FlatAppearance.BorderColor = AppTheme.Accent;
        var btnDismiss = new Button
        {
            Text = "Close",
            Size = new Size(100, 36),
            Location = new Point(192, 120),
            BackColor = AppTheme.Surface2,
            ForeColor = AppTheme.Fg,
            FlatStyle = FlatStyle.Flat,
        };
        btnDismiss.FlatAppearance.BorderColor = AppTheme.Border;
        _resultPanel.Controls.AddRange([_lblResult, _btnApply, btnDismiss]);

        body.Controls.AddRange([_lblProgress, _lblQuestion, _btnYes, _btnNo, _resultPanel]);

        _btnYes.Click += (_, _) => HandleAnswer(true);
        _btnNo.Click += (_, _) => HandleAnswer(false);
        _btnApply.Click += OnApply;
        btnDismiss.Click += (_, _) =>
        {
            DialogResult = DialogResult.Cancel;
            Close();
        };

        Controls.AddRange([header, body]);
        AppTheme.Apply3D(this);
    }

    // ── Question logic ───────────────────────────────────────────────────────

    private void HandleAnswer(bool yes)
    {
        _answers[_currentQ] = yes;
        _currentQ++;

        if (_currentQ < Questions.Length)
        {
            _lblProgress.Text = $"Question {_currentQ + 1} of {Questions.Length}";
            _lblQuestion.Text = Questions[_currentQ];
        }
        else
        {
            ShowResult();
        }
    }

    private void ShowResult()
    {
        _recommendedProfile = ComputeRecommendation();
        var idx = Array.IndexOf(ProfileIds, _recommendedProfile);
        var name = idx >= 0 ? ProfileNames[idx] : "Minimal";

        _lblResult.Text =
            $"Recommended profile: {name}\n\n"
            + $"Based on your answers, the \"{name}\" profile best matches your usage.\n"
            + "Click \"Use this profile\" to save this preference.";

        _lblProgress.Visible = false;
        _lblQuestion.Visible = false;
        _btnYes.Visible = false;
        _btnNo.Visible = false;
        _resultPanel.Visible = true;
    }

    private string ComputeRecommendation()
    {
        var scores = new int[ProfileIds.Length];
        for (int q = 0; q < Questions.Length; q++)
        {
            if (_answers[q])
                for (int p = 0; p < ProfileIds.Length; p++)
                    scores[p] += Weights[q][p];
        }
        int bestIdx = 0;
        for (int p = 1; p < scores.Length; p++)
            if (scores[p] > scores[bestIdx])
                bestIdx = p;
        return ProfileIds[bestIdx];
    }

    // ── Apply ────────────────────────────────────────────────────────────────

    private void OnApply(object? sender, EventArgs e)
    {
        var cfg = AppConfig.Load();
        cfg.DefaultProfile = _recommendedProfile;
        cfg.Save();
        DialogResult = DialogResult.OK;
        Close();
    }

    // ── Result accessor ──────────────────────────────────────────────────────

    /// <summary>Profile ID the wizard recommended (set after the user finishes all questions).</summary>
    internal string RecommendedProfile => _recommendedProfile;
}
