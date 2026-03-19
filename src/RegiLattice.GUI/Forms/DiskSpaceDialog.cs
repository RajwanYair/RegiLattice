// RegiLattice.GUI — Forms/DiskSpaceDialog.cs
// Sprint 41: Disk space overview — per-drive breakdown with visual usage bars.

#nullable enable

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Shows a per-drive disk space breakdown with coloured usage bars.
/// Refreshes on demand; double-click a drive to open it in Explorer.
/// </summary>
[SupportedOSPlatform("windows")]
internal sealed class DiskSpaceDialog : BaseDialog
{
    // ── Controls ─────────────────────────────────────────────────────────────
    private readonly Panel _drivePanel = new()
    {
        Dock = DockStyle.Fill,
        AutoScroll = true,
        Padding = new Padding(12, 8, 12, 8),
    };
    private readonly Button _btnRefresh = new()
    {
        Text = "\u21BB  Refresh",
        Width = 100,
        Height = 30,
    };
    private readonly Button _btnClose = new()
    {
        Text = "Close",
        Width = 80,
        Height = 30,
    };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };
    private CancellationTokenSource _cts = new();

    internal DiskSpaceDialog()
        : base("Disk Space Analyser", new Size(540, 440), resizable: true)
    {
        BuildLayout();
        _ = RefreshAsync();
    }

    private void BuildLayout()
    {
        var btnPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 44,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new Padding(8, 6, 8, 6),
        };
        btnPanel.Controls.AddRange([_btnClose, _btnRefresh]);

        Controls.Add(_drivePanel);
        Controls.Add(_statusLabel);
        Controls.Add(btnPanel);

        _btnRefresh.Click += async (_, _) => await RefreshAsync();
        _btnClose.Click += (_, _) => Close();

        AppTheme.Apply(this);
    }

    private async Task RefreshAsync()
    {
        _btnRefresh.Enabled = false;
        _statusLabel.Text = "Scanning drives…";
        _drivePanel.Controls.Clear();

        _cts.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        DriveInfo[] drives = [];
        try
        {
            drives = await Task.Run(() => DriveInfo.GetDrives().Where(d => d.IsReady).ToArray(), token);
        }
        catch (OperationCanceledException) { return; }
        catch (Exception ex)
        {
            _statusLabel.Text = $"Error: {ex.Message}";
            _btnRefresh.Enabled = true;
            return;
        }

        if (token.IsCancellationRequested) return;

        _drivePanel.SuspendLayout();
        foreach (DriveInfo drive in drives.OrderBy(d => d.Name))
            _drivePanel.Controls.Add(BuildDriveRow(drive));
        _drivePanel.ResumeLayout(true);

        long totalFree = drives.Sum(d => d.AvailableFreeSpace);
        long totalAll  = drives.Sum(d => d.TotalSize);
        _statusLabel.Text = $"{drives.Length} drive(s) — {FormatBytes(totalFree)} free of {FormatBytes(totalAll)} total";
        _btnRefresh.Enabled = true;
    }

    private Panel BuildDriveRow(DriveInfo drive)
    {
        long total    = drive.TotalSize;
        long free     = drive.AvailableFreeSpace;
        long used     = total - free;
        int  pct      = total > 0 ? (int)(used * 100L / total) : 0;
        string label  = $"{drive.Name}  [{drive.DriveType}]  {drive.VolumeLabel}";
        string sizes  = $"{FormatBytes(used)} used / {FormatBytes(free)} free / {FormatBytes(total)} total  ({pct}%)";

        // Colour: green <70%, amber <90%, red ≥90%
        Color barColor = pct >= 90 ? Color.FromArgb(243, 139, 168)   // red
                       : pct >= 70 ? Color.FromArgb(249, 226, 175)   // amber
                       :             Color.FromArgb(166, 227, 161);  // green

        var row = new Panel
        {
            Width = _drivePanel.ClientSize.Width - 24,
            Height = 80,
            Margin = new Padding(0, 0, 0, 10),
            Cursor = Cursors.Hand,
            BackColor = AppTheme.Surface,
        };
        row.MouseDoubleClick += (_, _) =>
        {
            try { System.Diagnostics.Process.Start("explorer.exe", drive.Name); }
            catch (Exception) { /* ignore */ }
        };

        var lblName = new Label
        {
            Text = label,
            AutoSize = true,
            Location = new Point(8, 8),
            Font = AppTheme.Bold,
            ForeColor = AppTheme.Fg,
            BackColor = Color.Transparent,
        };
        var lblSizes = new Label
        {
            Text = sizes,
            AutoSize = true,
            Location = new Point(8, 30),
            Font = AppTheme.Regular,
            ForeColor = AppTheme.FgDim,
            BackColor = Color.Transparent,
        };
        // Custom bar drawn via Paint
        var bar = new Panel
        {
            Location = new Point(8, 52),
            Width = (row.Width > 40 ? row.Width - 40 : row.Width) - 16,
            Height = 14,
            BackColor = AppTheme.Overlay,
        };
        bar.Paint += (_, e) =>
        {
            int fillW = pct > 0 ? Math.Max(2, (int)(bar.Width * (double)pct / 100)) : 0;
            using var brush = new LinearGradientBrush(
                new Rectangle(0, 0, Math.Max(1, fillW), bar.Height),
                Color.FromArgb(180, barColor),
                barColor,
                LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(brush, 0, 0, fillW, bar.Height);
        };

        row.Controls.AddRange([lblName, lblSizes, bar]);
        return row;
    }

    private static string FormatBytes(long bytes)
    {
        if (bytes >= 1_099_511_627_776L) return $"{bytes / 1_099_511_627_776.0:F1} TB";
        if (bytes >= 1_073_741_824L)     return $"{bytes / 1_073_741_824.0:F1} GB";
        if (bytes >= 1_048_576L)         return $"{bytes / 1_048_576.0:F0} MB";
        return $"{bytes / 1024.0:F0} KB";
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _cts.Dispose();
        base.Dispose(disposing);
    }
}
