// RegiLattice.GUI — Forms/TempFileCleanerDialog.cs
// Sprint 30: Scan well-known temp locations, preview sizes, and selectively delete.
// Sprint 47: +User Downloads location, +age filter (skip files newer than N days).

#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegiLattice.Core;
using RegiLattice.Core.Services;

namespace RegiLattice.GUI.Forms;

/// <summary>Scans Windows temporary-file locations, shows disk savings, and cleans selected groups.</summary>
[SupportedOSPlatform("windows")]
internal sealed class TempFileCleanerDialog : BaseDialog
{
    // ── Well-known locations ──────────────────────────────────────────────────
    private static readonly (string Label, Func<string> GetPath)[] Locations =
    [
        // ── OS / Windows ──────────────────────────────────────────────────────
        ("User Temp (%TEMP%)", () => Path.GetTempPath()),
        ("Windows Temp", () => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp")),
        ("Windows Prefetch", () => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Prefetch")),
        (
            "SoftwareDistribution\\Download",
            () => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"SoftwareDistribution\Download")
        ),
        ("Recycle Bin", () => @"C:\$Recycle.Bin"),
        ("Windows Error Reporting", () => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Windows\WER\ReportQueue")),
        ("User Downloads", () => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")),
        ("Windows Thumbnails Cache", () => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Windows\Explorer")),
        ("Windows Diagnostic Logs", () => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Windows\DiagnosticData")),
        ("BITS Transfer Cache", () => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Microsoft\Network\Downloader")),
        // ── Browsers ──────────────────────────────────────────────────────────
        ("Edge Cache", () => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Edge\User Data\Default\Cache\Cache_Data")),
        ("Chrome Cache", () => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\User Data\Default\Cache\Cache_Data")),
        ("Firefox Cache", () => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Mozilla\Firefox\Profiles")),
        ("Chrome Code Cache", () => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\User Data\Default\Code Cache")),
        ("Edge Code Cache", () => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Edge\User Data\Default\Code Cache")),
        // ── Development tools ─────────────────────────────────────────────────
        (
            "NuGet Cache",
            () => Environment.GetEnvironmentVariable("NUGET_PACKAGES")
                ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @".nuget\packages")
        ),
        (
            "npm Cache",
            () => Environment.GetEnvironmentVariable("NPM_CONFIG_CACHE")
                ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"npm-cache")
        ),
        (
            "pip Cache",
            () => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"pip\Cache")
        ),
        (
            "Maven Local Repository",
            () => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @".m2\repository")
        ),
        (
            "Gradle Cache",
            () => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @".gradle\caches")
        ),
    ];

    // ── Model ─────────────────────────────────────────────────────────────────
    private sealed class ScanGroup
    {
        public required string Label { get; init; }
        public required string Path { get; init; }
        public long FileBytes { get; set; }
        public int FileCount { get; set; }
        public bool Selected { get; set; } = true;
        public string SizeDisplay => FormatBytes(FileBytes);
    }

    // ── Controls ─────────────────────────────────────────────────────────────
    private readonly ListView _list = new()
    {
        Dock = DockStyle.Fill,
        FullRowSelect = true,
        CheckBoxes = true,
        View = View.Details,
        GridLines = true,
    };
    private readonly FlowLayoutPanel _btnPanel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 42,
        FlowDirection = FlowDirection.LeftToRight,
        WrapContents = false,
        Padding = new Padding(6, 6, 6, 4),
    };
    private readonly Button _btnScan = new() { Text = "Scan", Width = 80 };
    private readonly Button _btnClean = new()
    {
        Text = "Clean Selected",
        Width = 110,
        Enabled = false,
    };
    private readonly Button _btnClose = new() { Text = "Close", Width = 80 };
    private readonly NumericUpDown _ageFilter = new()
    {
        Minimum = 0,
        Maximum = 3650,
        Value = 0,
        Width = 60,
        DecimalPlaces = 0,
    };
    private readonly ProgressBar _progress = new()
    {
        Dock = DockStyle.Bottom,
        Height = 6,
        Style = ProgressBarStyle.Marquee,
        Visible = false,
    };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };

    private readonly List<ScanGroup> _groups = [];
    private CancellationTokenSource _cts = new();

    internal TempFileCleanerDialog()
        : base("Temporary File Cleaner", new Size(620, 420), resizable: true)
    {
        BuildControls();
    }

    // ── Construction ──────────────────────────────────────────────────────────
    private void BuildControls()
    {
        if (!Elevation.IsAdmin())
            Controls.Add(CreateWarningBanner("Windows Temp / Prefetch / SoftwareDistribution require administrator privileges."));

        _list.Columns.AddRange([
            new ColumnHeader { Text = "Location", Width = 280 },
            new ColumnHeader { Text = "File Count", Width = 90 },
            new ColumnHeader { Text = "Size", Width = 100 },
            new ColumnHeader { Text = "Path", Width = 300 },
        ]);
        ListViewColumnSorter.AttachTo(_list);

        _btnScan.Click += async (_, _) => await ScanAsync();
        _btnClean.Click += async (_, _) => await CleanAsync();
        _btnClose.Click += (_, _) => Close();

        var ageLabel = new Label
        {
            Text = "Min age (days, 0 = all):",
            AutoSize = true,
            Margin = new Padding(0, 8, 4, 0),
        };
        var topBar = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 40,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            Padding = new Padding(6, 6, 6, 2),
        };
        topBar.Controls.AddRange(new Control[] { ageLabel, _ageFilter });

        _btnPanel.Controls.AddRange(new Control[] { _btnScan, _btnClean, _btnClose });

        Controls.Add(_list);
        Controls.Add(topBar);
        Controls.Add(_statusLabel);
        Controls.Add(_progress);
        Controls.Add(_btnPanel);
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await ScanAsync();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _cts.Cancel();
        base.OnFormClosed(e);
    }

    // ── Scan ──────────────────────────────────────────────────────────────────
    private async Task ScanAsync()
    {
        _btnScan.Enabled = false;
        _btnClean.Enabled = false;
        _progress.Visible = true;
        _statusLabel.Text = "Scanning…";

        _cts.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        _groups.Clear();
        foreach (var (label, getPath) in Locations)
        {
            string path;
            try
            {
                path = getPath();
            }
            catch
            {
                continue;
            }
            _groups.Add(new ScanGroup { Label = label, Path = path });
        }

        await Task.Run(
            () =>
            {
                foreach (var g in _groups)
                {
                    if (token.IsCancellationRequested)
                        return;
                    try
                    {
                        if (!Directory.Exists(g.Path))
                            continue;
                        var files = Directory.EnumerateFiles(g.Path, "*", SearchOption.AllDirectories);
                        int minAgeDays = (int)_ageFilter.Value;
                        DateTime cutoff = minAgeDays > 0 ? DateTime.Now.AddDays(-minAgeDays) : DateTime.MinValue;
                        foreach (var f in files)
                        {
                            if (token.IsCancellationRequested)
                                return;
                            try
                            {
                                var fi = new FileInfo(f);
                                if (minAgeDays > 0 && fi.LastWriteTime > cutoff)
                                    continue;
                                g.FileBytes += fi.Length;
                                g.FileCount++;
                            }
                            catch
                            { /* locked / inaccessible */
                            }
                        }
                    }
                    catch
                    { /* directory inaccessible */
                    }
                }
            },
            token
        );

        if (token.IsCancellationRequested)
            return;

        RefreshList();

        long total = _groups.Sum(g => g.FileBytes);
        _statusLabel.Text = $"Scan complete. Potential savings: {FormatBytes(total)} across {_groups.Count} locations.";
        _btnScan.Enabled = true;
        _btnClean.Enabled = _groups.Any(g => g.FileCount > 0);
        _progress.Visible = false;
    }

    // ── Clean ─────────────────────────────────────────────────────────────────
    private async Task CleanAsync()
    {
        var toClean = _groups.Where(g => g.Selected && g.FileCount > 0).ToList();
        if (toClean.Count == 0)
            return;

        long estimatedBytes = toClean.Sum(g => g.FileBytes);
        var confirm = MessageBox.Show(
            $"Delete approximately {FormatBytes(estimatedBytes)} of temporary files?\n\n" + "This cannot be undone.",
            "Confirm Cleanup",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );
        if (confirm != DialogResult.Yes)
            return;

        _btnScan.Enabled = false;
        _btnClean.Enabled = false;
        _progress.Visible = true;
        _statusLabel.Text = "Cleaning…";

        int deleted = 0;
        long freed = 0;
        await Task.Run(() =>
        {
            foreach (var g in toClean)
            {
                if (!Directory.Exists(g.Path))
                    continue;
                try
                {
                    foreach (var f in Directory.EnumerateFiles(g.Path, "*", SearchOption.AllDirectories))
                    {
                        try
                        {
                            int minAgeDays = (int)_ageFilter.Value;
                            if (minAgeDays > 0 && new FileInfo(f).LastWriteTime > DateTime.Now.AddDays(-minAgeDays))
                                continue;
                            long size = new FileInfo(f).Length;
                            File.Delete(f);
                            deleted++;
                            freed += size;
                        }
                        catch
                        { /* locked / in-use */
                        }
                    }
                    // Remove empty sub-directories but not the root
                    foreach (var dir in Directory.EnumerateDirectories(g.Path, "*", SearchOption.AllDirectories).OrderByDescending(d => d.Length))
                    {
                        try
                        {
                            Directory.Delete(dir);
                        }
                        catch { }
                    }
                }
                catch
                { /* access denied at root */
                }
            }
        });

        _statusLabel.Text = $"Cleaned {deleted} files, freed {FormatBytes(freed)}.";
        _progress.Visible = false;
        await ScanAsync(); // Re-scan to refresh sizes
    }

    // ── Helpers ───────────────────────────────────────────────────────────────
    private void RefreshList()
    {
        _list.BeginUpdate();
        _list.Items.Clear();
        foreach (var g in _groups)
        {
            var item = new ListViewItem(g.Label) { Tag = g, Checked = g.Selected };
            item.SubItems.Add(g.FileCount.ToString("N0"));
            item.SubItems.Add(g.SizeDisplay);
            item.SubItems.Add(g.Path);
            if (g.FileCount == 0)
                item.ForeColor = SystemColors.GrayText;
            _list.Items.Add(item);
        }
        _list.EndUpdate();

        // Sync checkbox state to model
        _list.ItemChecked += (_, e) =>
        {
            if (e.Item.Tag is ScanGroup g)
                g.Selected = e.Item.Checked;
        };
    }

    private static string FormatBytes(long bytes) =>
        bytes switch
        {
            >= 1_073_741_824 => $"{bytes / 1_073_741_824.0:F1} GB",
            >= 1_048_576 => $"{bytes / 1_048_576.0:F1} MB",
            >= 1_024 => $"{bytes / 1_024.0:F1} KB",
            _ => $"{bytes} B",
        };
}
