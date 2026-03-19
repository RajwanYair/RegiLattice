// RegiLattice.GUI — Forms/PrivacyDashboardDialog.cs
// Sprint 32: Privacy dashboard — overview of privacy tweak status grouped by category.

#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegiLattice.Core;
using RegiLattice.Core.Models;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Privacy dashboard — shows the applied / not-applied status of all privacy-related
/// tweaks grouped by category, with a "privacy score" and one-click profile application.
/// </summary>
internal sealed class PrivacyDashboardDialog : BaseDialog
{
    // ── Privacy categories shown on the dashboard ────────────────────────────
    private static readonly string[] PrivacyCategories =
    [
        "Privacy",
        "Telemetry Advanced",
        "AI / Copilot",
        "Windows Recall",
        "Windows Update",
        "Cortana & Search",
        "OneDrive",
        "Edge",
        "Browser Common",
        "Phone Link",
        "Widgets & News",
    ];

    // ── Controls ──────────────────────────────────────────────────────────────
    private readonly Panel _scorePanel = new()
    {
        Dock = DockStyle.Top,
        Height = 80,
        Padding = new Padding(12, 8, 12, 4),
    };
    private readonly Label _lblScore = new()
    {
        AutoSize = false,
        Dock = DockStyle.Left,
        Width = 140,
        TextAlign = ContentAlignment.MiddleCenter,
        Font = new Font("Segoe UI", 24f, FontStyle.Bold),
    };
    private readonly Label _lblScoreDesc = new()
    {
        Dock = DockStyle.Fill,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(8, 0, 0, 0),
    };
    private readonly ProgressBar _scoreBar = new()
    {
        Dock = DockStyle.Bottom,
        Height = 12,
        Minimum = 0,
        Maximum = 100,
        Value = 0,
    };
    private readonly ListView _categoryList = new()
    {
        Dock = DockStyle.Fill,
        FullRowSelect = true,
        MultiSelect = false,
        View = View.Details,
        GridLines = false,
    };
    private readonly FlowLayoutPanel _btnPanel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 42,
        Padding = new Padding(6, 6, 6, 4),
        FlowDirection = FlowDirection.LeftToRight,
        WrapContents = false,
    };
    private readonly Button _btnApplyProfile = new()
    {
        Text = "Apply Privacy Profile",
        Width = 160,
        Height = 28,
    };
    private readonly Button _btnRefresh = new()
    {
        Text = "Refresh",
        Width = 80,
        Height = 28,
    };
    private readonly Button _btnClose = new()
    {
        Text = "Close",
        Width = 80,
        Height = 28,
    };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };

    private readonly TweakEngine _engine;
    private CancellationTokenSource _cts = new();
    private bool _busy;

    // ── Construction ──────────────────────────────────────────────────────────
    internal PrivacyDashboardDialog()
        : base("Privacy Dashboard", new Size(720, 560), resizable: true)
    {
        _engine = new TweakEngine();
        _engine.RegisterBuiltins();

        BuildLayout();
        ApplyTheme();

        _btnRefresh.Click += async (_, _) => await RefreshAsync().ConfigureAwait(false);
        _btnClose.Click += (_, _) => Close();
        _btnApplyProfile.Click += async (_, _) => await ApplyPrivacyProfileAsync().ConfigureAwait(false);

        Load += async (_, _) => await RefreshAsync().ConfigureAwait(false);
    }

    // ── Layout ────────────────────────────────────────────────────────────────
    private void BuildLayout()
    {
        _categoryList.Columns.AddRange([
            new ColumnHeader { Text = "Category", Width = 200 },
            new ColumnHeader { Text = "Applied", Width = 80 },
            new ColumnHeader { Text = "Total", Width = 80 },
            new ColumnHeader { Text = "Coverage", Width = 100 },
            new ColumnHeader { Text = "Status", Width = 120 },
        ]);
        ListViewColumnSorter.AttachTo(_categoryList);

        _scorePanel.Controls.AddRange([_lblScore, _lblScoreDesc]);

        var scoreContainer = new Panel
        {
            Dock = DockStyle.Top,
            Height = 100,
            Padding = new Padding(8),
        };
        scoreContainer.Controls.Add(_scoreBar);
        scoreContainer.Controls.Add(_scorePanel);

        _btnPanel.Controls.AddRange([_btnApplyProfile, _btnRefresh, _btnClose]);

        Controls.AddRange([_categoryList, _statusLabel, _btnPanel, scoreContainer]);
    }

    // ── Refresh ────────────────────────────────────────────────────────────────
    private async Task RefreshAsync()
    {
        if (_busy)
            return;
        SetBusy(true, "Scanning privacy settings…");
        try
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();

            // Collect all tweaks for privacy categories
            var allByCategory = _engine.TweaksByCategory();
            var privacyTweaks = new List<TweakDef>();
            foreach (string cat in PrivacyCategories)
            {
                if (allByCategory.TryGetValue(cat, out var catList))
                    privacyTweaks.AddRange(catList);
            }

            if (privacyTweaks.Count == 0)
            {
                _statusLabel.Text = "No privacy tweaks found.";
                return;
            }

            // Detect status in parallel
            var ids = privacyTweaks.Select(t => t.Id).ToList();
            var statusMap = await Task.Run(() => _engine.StatusMap(parallel: true, ids: ids), _cts.Token).ConfigureAwait(false);

            // Group by category
            var rows = new List<(string Category, int Applied, int Total)>();
            foreach (string cat in PrivacyCategories)
            {
                if (!allByCategory.TryGetValue(cat, out var catTweaks) || catTweaks.Count == 0)
                    continue;

                int applied = catTweaks.Count(t => statusMap.TryGetValue(t.Id, out var r) && r == TweakResult.Applied);
                rows.Add((cat, applied, catTweaks.Count));
            }

            // Calculate score
            int totalTweaks = rows.Sum(r => r.Total);
            int totalApplied = rows.Sum(r => r.Applied);
            int score = totalTweaks > 0 ? (totalApplied * 100) / totalTweaks : 0;

            // Update UI on main thread
            Invoke(() =>
            {
                UpdateScorePanel(score, totalApplied, totalTweaks);
                PopulateCategoryList(rows);
                _statusLabel.Text = $"Privacy score: {score}%  ({totalApplied}/{totalTweaks} tweaks applied)";
            });
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void UpdateScorePanel(int score, int applied, int total)
    {
        _lblScore.Text = $"{score}%";
        _lblScore.ForeColor = score switch
        {
            >= 80 => AppTheme.Green,
            >= 50 => AppTheme.Yellow,
            _ => Color.FromArgb(200, 80, 80),
        };

        string rating = score switch
        {
            >= 80 => "Excellent — strong privacy protection",
            >= 60 => "Good — most privacy tweaks applied",
            >= 40 => "Fair — some privacy gaps remain",
            >= 20 => "Weak — many privacy tweaks not applied",
            _ => "Poor — privacy not configured",
        };

        _lblScoreDesc.Text = $"{rating}\n{applied} of {total} privacy tweaks active.";
        _scoreBar.Value = score;
    }

    private void PopulateCategoryList(IEnumerable<(string Category, int Applied, int Total)> rows)
    {
        _categoryList.SuspendLayout();
        _categoryList.BeginUpdate();
        _categoryList.Items.Clear();

        foreach (var (cat, applied, total) in rows)
        {
            int pct = total > 0 ? (applied * 100) / total : 0;
            string status = pct switch
            {
                100 => "Fully protected",
                >= 75 => "Well protected",
                >= 50 => "Partial",
                >= 25 => "Mostly unprotected",
                _ => "Not configured",
            };

            var item = new ListViewItem(cat);
            item.SubItems.Add(applied.ToString());
            item.SubItems.Add(total.ToString());
            item.SubItems.Add($"{pct}%");
            item.SubItems.Add(status);

            item.ForeColor = pct switch
            {
                100 => AppTheme.Green,
                >= 50 => AppTheme.Yellow,
                _ => AppTheme.Fg,
            };

            _categoryList.Items.Add(item);
        }

        _categoryList.EndUpdate();
        _categoryList.ResumeLayout();
    }

    private async Task ApplyPrivacyProfileAsync()
    {
        if (_busy)
            return;
        var answer = MessageBox.Show(
            "Apply the Privacy profile?\n\nThis will apply all privacy-related tweaks.\n\nAdministrator rights are required for machine-scoped tweaks.",
            "Apply Privacy Profile",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        );

        if (answer != DialogResult.Yes)
            return;

        SetBusy(true, "Applying Privacy profile…");
        try
        {
            await Task.Run(
                    () =>
                    {
                        _engine.ApplyProfile("privacy", forceCorp: false, parallel: true);
                    },
                    _cts.Token
                )
                .ConfigureAwait(false);

            await RefreshAsync().ConfigureAwait(false);
            MessageBox.Show("Privacy profile applied successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to apply profile: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void SetBusy(bool busy, string? message = null)
    {
        _busy = busy;
        if (message != null)
            _statusLabel.Text = message;
        _btnRefresh.Enabled = !busy;
        _btnApplyProfile.Enabled = !busy && Elevation.IsAdmin();
        Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
    }

    private void ApplyTheme()
    {
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        _categoryList.BackColor = AppTheme.Surface;
        _categoryList.ForeColor = AppTheme.Fg;
        _statusLabel.BackColor = AppTheme.Overlay;
        _statusLabel.ForeColor = AppTheme.Fg;
        _btnPanel.BackColor = AppTheme.Surface;
        _scorePanel.BackColor = AppTheme.Surface;
        _lblScore.ForeColor = AppTheme.Accent;
        _lblScoreDesc.ForeColor = AppTheme.Fg;
        _scoreBar.ForeColor = AppTheme.Accent;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _cts.Dispose();
        base.Dispose(disposing);
    }
}
