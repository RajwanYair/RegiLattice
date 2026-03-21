// RegiLattice.GUI — Forms/HostsFileManagerDialog.cs
// Sprint 30: Simple hosts-file editor — view, add, toggle, and remove entries.
// Sprint 47: +Import from URL, +Export as .bat blocker.

#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegiLattice.Core;
using RegiLattice.Core.Services;

namespace RegiLattice.GUI.Forms;

/// <summary>Read / edit Windows hosts file with enable-disable (comment) toggling.</summary>
[SupportedOSPlatform("windows")]
internal sealed class HostsFileManagerDialog : BaseDialog
{
    private static readonly string HostsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts");

    // ── Model ─────────────────────────────────────────────────────────────────
    private sealed record HostsEntry(int LineIndex, string Ip, string Host, bool Enabled, string Comment);

    // ── Controls ─────────────────────────────────────────────────────────────
    private readonly TextBox _searchBox = new() { Width = 220, PlaceholderText = "Filter hosts…" };
    private readonly ListView _list = new()
    {
        Dock = DockStyle.Fill,
        FullRowSelect = true,
        MultiSelect = true,
        View = View.Details,
        GridLines = true,
    };
    private readonly Panel _topPanel = new()
    {
        Dock = DockStyle.Top,
        Height = 42,
        Padding = new Padding(6),
    };
    private readonly FlowLayoutPanel _btnPanel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 42,
        FlowDirection = FlowDirection.LeftToRight,
        WrapContents = false,
        Padding = new Padding(6, 6, 6, 4),
    };
    private readonly Button _btnAdd = new() { Text = "Add…", Width = 76 };
    private readonly Button _btnToggle = new()
    {
        Text = "Enable/Disable",
        Width = 110,
        Enabled = false,
    };
    private readonly Button _btnDelete = new()
    {
        Text = "Delete",
        Width = 76,
        Enabled = false,
    };
    private readonly Button _btnSave = new()
    {
        Text = "Save",
        Width = 76,
        Enabled = false,
    };
    private readonly Button _btnRefresh = new() { Text = "Refresh", Width = 76 };
    private readonly Button _btnImportUrl = new() { Text = "Import URL…", Width = 96 };
    private readonly Button _btnExportBat = new() { Text = "Export .bat", Width = 88 };
    private readonly Button _btnClose = new() { Text = "Close", Width = 76 };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };

    // Raw lines from the file (preserved so we write back exactly)
    private string[] _rawLines = [];
    private List<HostsEntry> _allEntries = [];
    private bool _dirty;

    internal HostsFileManagerDialog()
        : base("Hosts File Manager", new Size(720, 500), resizable: true)
    {
        BuildControls();
    }

    // ── Construction ──────────────────────────────────────────────────────────
    private void BuildControls()
    {
        if (!Elevation.IsAdmin())
            Controls.Add(CreateAdminBanner("Saving requires administrator privileges. Run as administrator to make changes."));

        var searchLabel = new Label
        {
            Text = "Search:",
            AutoSize = true,
            Margin = new Padding(0, 8, 4, 0),
        };
        _searchBox.TextChanged += (_, _) => FilterList();
        var flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
        };
        flow.Controls.AddRange(new Control[] { searchLabel, _searchBox });
        _topPanel.Controls.Add(flow);

        _list.Columns.AddRange([
            new ColumnHeader { Text = "IP Address", Width = 140 },
            new ColumnHeader { Text = "Hostname", Width = 240 },
            new ColumnHeader { Text = "Status", Width = 80 },
            new ColumnHeader { Text = "Comment", Width = 160 },
        ]);
        ListViewColumnSorter.AttachTo(_list);
        _list.SelectedIndexChanged += OnSelectionChanged;

        _btnAdd.Click += OnAddEntry;
        _btnToggle.Click += async (_, _) => await ToggleSelectedAsync();
        _btnDelete.Click += OnDeleteSelected;
        _btnSave.Click += async (_, _) => await SaveHostsFileAsync();
        _btnRefresh.Click += async (_, _) => await LoadFileAsync();
        _btnImportUrl.Click += async (_, _) => await OnImportFromUrlAsync();
        _btnExportBat.Click += OnExportAsBat;
        _btnClose.Click += (_, _) => Close();

        _btnPanel.Controls.AddRange(
            new Control[] { _btnAdd, _btnToggle, _btnDelete, _btnSave, _btnRefresh, _btnImportUrl, _btnExportBat, _btnClose }
        );

        Controls.Add(_list);
        Controls.Add(_topPanel);
        Controls.Add(_statusLabel);
        Controls.Add(_btnPanel);
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await LoadFileAsync();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (_dirty)
        {
            var result = MessageBox.Show(
                "You have unsaved changes. Save before closing?",
                "Unsaved Changes",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning
            );
            if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
            if (result == DialogResult.Yes)
            {
                // Run sync on close — small file, acceptable
                try
                {
                    File.WriteAllLines(HostsPath, _rawLines);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Save failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        base.OnFormClosing(e);
    }

    // ── Data loading ──────────────────────────────────────────────────────────
    private async Task LoadFileAsync()
    {
        _btnRefresh.Enabled = false;
        _statusLabel.Text = "Reading hosts file…";

        try
        {
            _rawLines = await File.ReadAllLinesAsync(HostsPath);
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"Error: {ex.Message}";
            _btnRefresh.Enabled = true;
            return;
        }

        _allEntries = ParseLines(_rawLines);
        FilterList();
        _statusLabel.Text = $"{_allEntries.Count} host entries found. Path: {HostsPath}";
        _btnRefresh.Enabled = true;
        SetDirty(false);
    }

    private static List<HostsEntry> ParseLines(string[] lines)
    {
        var result = new List<HostsEntry>();
        for (int i = 0; i < lines.Length; i++)
        {
            string raw = lines[i];
            bool enabled = !raw.TrimStart().StartsWith('#');
            string working = enabled ? raw : raw.TrimStart('#').TrimStart();
            // Strip inline comment
            string comment = "";
            int hashIdx = working.IndexOf('#');
            if (hashIdx >= 0)
            {
                comment = working[(hashIdx + 1)..].Trim();
                working = working[..hashIdx].Trim();
            }
            var parts = working.Split((char[])[' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                continue;
            result.Add(new HostsEntry(i, parts[0], parts[1], enabled, comment));
        }
        return result;
    }

    // ── Filter ────────────────────────────────────────────────────────────────
    private void FilterList()
    {
        string search = _searchBox.Text.Trim();
        var filtered = _allEntries.AsEnumerable();
        if (!string.IsNullOrEmpty(search))
            filtered = filtered.Where(e =>
                e.Ip.Contains(search, StringComparison.OrdinalIgnoreCase)
                || e.Host.Contains(search, StringComparison.OrdinalIgnoreCase)
                || e.Comment.Contains(search, StringComparison.OrdinalIgnoreCase)
            );

        _list.BeginUpdate();
        _list.Items.Clear();
        foreach (var e in filtered)
        {
            var item = new ListViewItem(e.Ip) { Tag = e };
            item.SubItems.Add(e.Host);
            item.SubItems.Add(e.Enabled ? "Active" : "Disabled");
            item.SubItems.Add(e.Comment);
            item.ForeColor = e.Enabled ? SystemColors.WindowText : SystemColors.GrayText;
            _list.Items.Add(item);
        }
        _list.EndUpdate();
    }

    // ── Actions ───────────────────────────────────────────────────────────────
    private void OnAddEntry(object? sender, EventArgs e)
    {
        using var dlg = new HostsAddDialog();
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        // Append new line
        var newLines = _rawLines.ToList();
        string newLine = $"{dlg.IpAddress}\t{dlg.Hostname}";
        if (!string.IsNullOrEmpty(dlg.Comment))
            newLine += $"\t# {dlg.Comment}";
        newLines.Add(newLine);
        _rawLines = [.. newLines];
        _allEntries = ParseLines(_rawLines);
        FilterList();
        SetDirty(true);
    }

    private async Task ToggleSelectedAsync()
    {
        var selected = _list.SelectedItems.Cast<ListViewItem>().Select(i => i.Tag as HostsEntry).Where(e => e is not null).ToList();

        await Task.Run(() =>
        {
            foreach (var e in selected)
            {
                string raw = _rawLines[e!.LineIndex];
                _rawLines[e.LineIndex] = e.Enabled ? $"# {raw.TrimStart()}" : raw.TrimStart('#').TrimStart();
            }
        });

        _allEntries = ParseLines(_rawLines);
        FilterList();
        SetDirty(true);
    }

    private void OnDeleteSelected(object? sender, EventArgs e)
    {
        var selected = _list.SelectedItems.Cast<ListViewItem>().Select(i => i.Tag as HostsEntry).Where(e => e is not null).ToList();
        if (selected.Count == 0)
            return;

        var result = MessageBox.Show($"Delete {selected.Count} entry/entries?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (result != DialogResult.Yes)
            return;

        var indicesToRemove = selected.Select(e => e!.LineIndex).ToHashSet();
        _rawLines = _rawLines.Where((_, i) => !indicesToRemove.Contains(i)).ToArray();
        _allEntries = ParseLines(_rawLines);
        FilterList();
        SetDirty(true);
    }

    private async Task SaveHostsFileAsync()
    {
        _btnSave.Enabled = false;
        _statusLabel.Text = "Saving…";
        try
        {
            await File.WriteAllLinesAsync(HostsPath, _rawLines);
            SetDirty(false);
            _statusLabel.Text = "Saved successfully.";
        }
        catch (UnauthorizedAccessException)
        {
            _statusLabel.Text = "Access denied. Run as administrator to save hosts file.";
            MessageBox.Show(
                "Cannot save hosts file.\nRun RegiLattice as administrator.",
                "Access Denied",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"Error: {ex.Message}";
        }
        _btnSave.Enabled = _dirty;
    }

    // ── Sprint 47 Enhancements ─────────────────────────────────────────────────
    private async Task OnImportFromUrlAsync()
    {
        using var promptDlg = new HostsUrlPromptDialog();
        if (promptDlg.ShowDialog(this) != DialogResult.OK)
            return;

        string url = promptDlg.Url;
        _statusLabel.Text = "Downloading…";
        _btnImportUrl.Enabled = false;
        try
        {
            using var http = new HttpClient();
            http.Timeout = TimeSpan.FromSeconds(15);
            string content = await http.GetStringAsync(url);
            var newLines = content.Split(["\r\n", "\n"], StringSplitOptions.None).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

            // Merge: append only lines not already present
            var existing = new HashSet<string>(_rawLines, StringComparer.OrdinalIgnoreCase);
            int added = 0;
            var merged = _rawLines.ToList();
            foreach (string line in newLines)
            {
                if (!existing.Contains(line))
                {
                    merged.Add(line);
                    existing.Add(line);
                    added++;
                }
            }
            _rawLines = [.. merged];
            _allEntries = ParseLines(_rawLines);
            FilterList();
            SetDirty(true);
            _statusLabel.Text = $"Imported {added} new line(s) from URL.";
        }
        catch (HttpRequestException ex)
        {
            _statusLabel.Text = $"Download failed: {ex.Message}";
            MessageBox.Show($"Failed to download:\n{ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (TaskCanceledException)
        {
            _statusLabel.Text = "Download timed out.";
        }
        finally
        {
            _btnImportUrl.Enabled = true;
        }
    }

    private void OnExportAsBat(object? sender, EventArgs e)
    {
        if (_allEntries.Count == 0)
        {
            _statusLabel.Text = "No entries to export.";
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Title = "Export as .bat Hosts Blocker",
            Filter = "Batch file (*.bat)|*.bat|All files (*.*)|*.*",
            FileName = "apply-hosts-blocklist.bat",
        };
        if (sfd.ShowDialog(this) != DialogResult.OK)
            return;

        var sb = new StringBuilder();
        sb.AppendLine("@echo off");
        sb.AppendLine(":: Auto-generated hosts blocker — apply with administrator rights");
        sb.AppendLine();
        foreach (var entry in _allEntries.Where(e => e.Enabled))
        {
            string comment = string.IsNullOrEmpty(entry.Comment) ? "" : $"  rem {entry.Comment}";
            sb.AppendLine($"echo {entry.Ip}  {entry.Host}>> \"%SystemRoot%\\System32\\drivers\\etc\\hosts\"");
        }
        sb.AppendLine();
        sb.AppendLine("ipconfig /flushdns");
        sb.AppendLine("echo Done.");
        try
        {
            File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.ASCII);
            _statusLabel.Text = $"Exported {_allEntries.Count(e => e.Enabled)} active entries to {Path.GetFileName(sfd.FileName)}";
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"Export failed: {ex.Message}";
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────
    private void SetDirty(bool value)
    {
        _dirty = value;
        _btnSave.Enabled = value;
    }

    private void OnSelectionChanged(object? sender, EventArgs e)
    {
        bool any = _list.SelectedItems.Count > 0;
        _btnToggle.Enabled = any;
        _btnDelete.Enabled = any;
    }
}

// ── Sprint 47: URL Prompt Dialog ───────────────────────────────────────────────
[SupportedOSPlatform("windows")]
internal sealed class HostsUrlPromptDialog : Form
{
    internal string Url => _urlBox.Text.Trim();

    private readonly TextBox _urlBox = new() { Width = 400, PlaceholderText = "https://raw.githubusercontent.com/StevenBlack/hosts/master/hosts" };

    internal HostsUrlPromptDialog()
    {
        Text = "Import Hosts from URL";
        Icon = AppIcons.AppIcon;
        Size = new Size(480, 140);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = MinimizeBox = false;

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2,
            Padding = new Padding(10),
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        layout.Controls.Add(
            new Label
            {
                Text = "URL:",
                AutoSize = true,
                Margin = new Padding(0, 8, 4, 0),
            },
            0,
            0
        );
        layout.Controls.Add(_urlBox, 1, 0);

        var btnOk = new Button
        {
            Text = "Download",
            DialogResult = DialogResult.OK,
            Width = 90,
        };
        var btnCancel = new Button
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Width = 80,
        };
        var btns = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
        btns.Controls.AddRange(new Control[] { btnCancel, btnOk });
        layout.Controls.Add(btns, 1, 1);

        Controls.Add(layout);
        AcceptButton = btnOk;
        CancelButton = btnCancel;
        AppTheme.Apply3D(this);
    }
}

// ── Inline Add-Entry Dialog ───────────────────────────────────────────────────
[SupportedOSPlatform("windows")]
internal sealed class HostsAddDialog : Form
{
    internal string IpAddress => _ipBox.Text.Trim();
    internal string Hostname => _hostBox.Text.Trim();
    internal string Comment => _commentBox.Text.Trim();

    private readonly TextBox _ipBox = new() { Width = 160 };
    private readonly TextBox _hostBox = new() { Width = 220 };
    private readonly TextBox _commentBox = new() { Width = 220 };

    internal HostsAddDialog()
    {
        Text = "Add Hosts Entry";
        Icon = AppIcons.AppIcon;
        Size = new Size(380, 200);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = MinimizeBox = false;

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            Padding = new Padding(10),
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        layout.Controls.Add(
            new Label
            {
                Text = "IP Address:",
                AutoSize = true,
                Margin = new Padding(0, 8, 4, 0),
            },
            0,
            0
        );
        layout.Controls.Add(_ipBox, 1, 0);
        layout.Controls.Add(
            new Label
            {
                Text = "Hostname:",
                AutoSize = true,
                Margin = new Padding(0, 8, 4, 0),
            },
            0,
            1
        );
        layout.Controls.Add(_hostBox, 1, 1);
        layout.Controls.Add(
            new Label
            {
                Text = "Comment:",
                AutoSize = true,
                Margin = new Padding(0, 8, 4, 0),
            },
            0,
            2
        );
        layout.Controls.Add(_commentBox, 1, 2);

        var btnOk = new Button
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Width = 80,
        };
        var btnCancel = new Button
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Width = 80,
        };
        var btns = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
        btns.Controls.AddRange(new Control[] { btnCancel, btnOk });
        layout.Controls.Add(btns, 1, 3);

        Controls.Add(layout);
        AcceptButton = btnOk;
        CancelButton = btnCancel;
        AppTheme.Apply3D(this);
    }
}
