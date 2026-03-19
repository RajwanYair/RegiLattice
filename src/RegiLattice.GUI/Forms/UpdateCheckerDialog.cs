// RegiLattice.GUI — Forms/UpdateCheckerDialog.cs
// Sprint 45: Check for application updates against GitHub Releases API.

#nullable enable

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegiLattice.Core;
using RegiLattice.Core.Services;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Shows the current version, queries the GitHub Releases API for the latest release,
/// and offers a direct download link if an update is available.
/// </summary>
internal sealed class UpdateCheckerDialog : BaseDialog
{
    // ── Controls ─────────────────────────────────────────────────────────────
    private readonly Label _lblCurrent = new() { AutoSize = true };
    private readonly Label _lblLatest = new() { AutoSize = true, Text = "Checking…" };
    private readonly Label _lblStatus = new()
    {
        Dock = DockStyle.Top,
        Height = 26,
        TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
        Padding = new System.Windows.Forms.Padding(6, 0, 0, 0),
    };
    private readonly RichTextBox _notesBox = new()
    {
        ReadOnly = true,
        BackColor = AppTheme.Surface,
        ForeColor = AppTheme.FgDim,
        Font = AppTheme.Mono,
        BorderStyle = BorderStyle.None,
        ScrollBars = RichTextBoxScrollBars.Vertical,
        Dock = DockStyle.Fill,
        Text = "(Release notes will appear here after check completes)",
    };
    private readonly Button _btnDownload = new()
    {
        Text = "⬇  Download Latest",
        Width = 160,
        Height = 30,
        Enabled = false,
        BackColor = AppTheme.Accent,
        ForeColor = AppTheme.Bg,
        FlatStyle = FlatStyle.Flat,
    };
    private readonly Button _btnCheck = new()
    {
        Text = "Check Again",
        Width = 120,
        Height = 30,
    };
    private readonly Button _btnClose = new()
    {
        Text = "Close",
        Width = 80,
        Height = 30,
        DialogResult = DialogResult.Cancel,
    };

    private string? _downloadUrl;
    private CancellationTokenSource _cts = new();

    // ── Construction ─────────────────────────────────────────────────────────

    internal UpdateCheckerDialog()
        : base("Check for Updates", new System.Drawing.Size(520, 380))
    {
        BuildLayout();
        Load += async (_, _) => await CheckAsync();
    }

    private void BuildLayout()
    {
        var table = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 2,
            RowCount = 3,
            Padding = new System.Windows.Forms.Padding(12, 8, 12, 4),
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        table.Controls.Add(CreateLabel("Current version:"), 0, 0);
        _lblCurrent.Text = $"v{UpdateCheckService.CurrentVersion}";
        table.Controls.Add(_lblCurrent, 1, 0);

        table.Controls.Add(CreateLabel("Latest release:"), 0, 1);
        table.Controls.Add(_lblLatest, 1, 1);

        table.Controls.Add(_lblStatus, 0, 2);
        table.SetColumnSpan(_lblStatus, 2);

        var notesGroup = new GroupBox
        {
            Text = "Release Notes",
            Dock = DockStyle.Fill,
            Padding = new System.Windows.Forms.Padding(6),
        };
        notesGroup.Controls.Add(_notesBox);

        var btnPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 46,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new System.Windows.Forms.Padding(8, 6, 8, 6),
        };
        btnPanel.Controls.AddRange([_btnClose, _btnCheck, _btnDownload]);

        Controls.Add(notesGroup);
        Controls.Add(table);
        Controls.Add(btnPanel);

        _btnCheck.Click += async (_, _) => await CheckAsync();
        _btnDownload.Click += OnDownload;
        _btnClose.Click += (_, _) => Close();

        AppTheme.Apply(this);
    }

    // ── Logic ─────────────────────────────────────────────────────────────────

    private async Task CheckAsync()
    {
        _btnCheck.Enabled = false;
        _btnDownload.Enabled = false;
        _lblLatest.Text = "Checking…";
        _lblStatus.Text = "";
        _notesBox.Text = "Contacting GitHub…";

        _cts.Cancel();
        _cts = new CancellationTokenSource();

        UpdateInfo result;
        try
        {
            result = await UpdateCheckService.CheckAsync(_cts.Token).ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        if (result.Error is not null)
        {
            _lblLatest.Text = "— (error)";
            _lblStatus.Text = $"⚠  {result.Error}";
            _notesBox.Text = result.Error;
            _btnCheck.Enabled = true;
            return;
        }

        _lblLatest.Text = $"v{result.LatestVersion}  (released {result.PublishedAt:d MMM yyyy})";
        _notesBox.Text = string.IsNullOrWhiteSpace(result.ReleaseNotes) ? "(No release notes provided)" : result.ReleaseNotes;

        if (result.UpdateAvailable)
        {
            _lblStatus.Text = $"✅  Update available: v{result.CurrentVersion} → v{result.LatestVersion}";
            _lblStatus.ForeColor = AppTheme.Green;
            _downloadUrl = result.DownloadUrl;
            _btnDownload.Enabled = !string.IsNullOrEmpty(_downloadUrl);
        }
        else
        {
            _lblStatus.Text = "✔  You are up to date.";
            _lblStatus.ForeColor = AppTheme.Fg;
        }

        _btnCheck.Enabled = true;
    }

    private void OnDownload(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_downloadUrl))
            return;

        try
        {
            Process.Start(new ProcessStartInfo(_downloadUrl) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not open download URL:\n{ex.Message}", "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private static Label CreateLabel(string text) =>
        new()
        {
            Text = text,
            AutoSize = true,
            ForeColor = AppTheme.FgDim,
            Padding = new System.Windows.Forms.Padding(0, 4, 0, 4),
        };

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _cts.Dispose();
        base.Dispose(disposing);
    }
}
