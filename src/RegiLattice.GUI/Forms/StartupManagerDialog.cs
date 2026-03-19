#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RegiLattice.Core;
using RegiLattice.Core.Services;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Dialog for reviewing and toggling Windows startup entries from the
/// registry and Startup shell folders.
/// </summary>
internal sealed class StartupManagerDialog : BaseDialog
{
    // ── Controls ─────────────────────────────────────────────────────────────
    private readonly ListView _list = new()
    {
        Dock = DockStyle.Fill,
        FullRowSelect = true,
        MultiSelect = false,
        View = View.Details,
        GridLines = true,
    };
    private readonly Panel _btnPanel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 40,
        Padding = new Padding(6, 6, 6, 4),
    };
    private readonly Button _btnEnable = new()
    {
        Text = "Enable",
        Width = 80,
        Enabled = false,
    };
    private readonly Button _btnDisable = new()
    {
        Text = "Disable",
        Width = 80,
        Enabled = false,
    };
    private readonly Button _btnDelete = new()
    {
        Text = "Delete",
        Width = 80,
        Enabled = false,
    };
    private readonly Button _btnRefresh = new() { Text = "Refresh", Width = 80 };
    private readonly Button _btnClose = new() { Text = "Close", Width = 80 };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };
    private readonly Panel _adminBanner = new()
    {
        Dock = DockStyle.Top,
        Height = 30,
        BackColor = Color.FromArgb(50, 150, 250),
    };
    private readonly Label _adminLabel = new()
    {
        Dock = DockStyle.Fill,
        ForeColor = Color.White,
        TextAlign = ContentAlignment.MiddleCenter,
        Font = new Font("Segoe UI", 9f, FontStyle.Regular),
    };
    private readonly Button _btnExportCsv = new()
    {
        Text = "Export CSV",
        Width = 85,
        Enabled = false,
    };
    private readonly Button _btnOpenLocation = new()
    {
        Text = "Open Loc.",
        Width = 80,
        Enabled = false,
    };

    private IReadOnlyList<StartupEntry> _entries = Array.Empty<StartupEntry>();

    // ── Construction ─────────────────────────────────────────────────────────

    internal StartupManagerDialog()
        : base("Startup Manager", new Size(820, 520), resizable: true)
    {
        MinimumSize = new Size(600, 380);

        BuildColumns();
        BuildButtons();
        BuildAdminBanner();

        Controls.Add(_list);
        Controls.Add(_statusLabel);
        Controls.Add(_btnPanel);
        Controls.Add(_adminBanner);

        _list.SelectedIndexChanged += OnSelectionChanged;
        Load += (_, _) => RefreshList();
    }

    // ── Layout ───────────────────────────────────────────────────────────────

    private void BuildColumns()
    {
        _list.Columns.Add("Name", 180);
        _list.Columns.Add("Status", 70);
        _list.Columns.Add("Location", 120);
        _list.Columns.Add("Command", 380);
    }

    private void BuildButtons()
    {
        _btnEnable.Click += (_, _) => OnToggle(enable: true);
        _btnDisable.Click += (_, _) => OnToggle(enable: false);
        _btnDelete.Click += (_, _) => OnDelete();
        _btnRefresh.Click += (_, _) => RefreshList();
        _btnClose.Click += (_, _) => Close();

        _btnPanel.Controls.AddRange([_btnEnable, _btnDisable, _btnDelete, _btnRefresh, _btnClose]);

        int x = 0;
        foreach (Button b in new[] { _btnEnable, _btnDisable, _btnDelete, _btnRefresh })
        {
            b.Location = new Point(x, 6);
            x += 86;
        }
        _btnClose.Location = new Point(_btnPanel.Width - 86, 6);
        _btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;

        _btnExportCsv.Location = new Point(x, 6);
        _btnOpenLocation.Location = new Point(x + 91, 6);
        _btnPanel.Controls.AddRange([_btnExportCsv, _btnOpenLocation]);
        _btnExportCsv.Click += (_, _) => ExportCsv();
        _btnOpenLocation.Click += (_, _) => OpenFileLocation();
    }

    private void BuildAdminBanner()
    {
        bool isAdmin = Elevation.IsAdmin();
        _adminLabel.Text = isAdmin
            ? "Running as Administrator — all startup locations accessible"
            : "Not running as Administrator — HKLM and all-users entries are read-only";
        if (!isAdmin)
            _adminBanner.BackColor = Color.FromArgb(180, 120, 0);

        _adminBanner.Controls.Add(_adminLabel);
    }

    // ── Data ─────────────────────────────────────────────────────────────────

    private void RefreshList()
    {
        _list.BeginUpdate();
        _list.Items.Clear();

        try
        {
            _entries = StartupManager.GetAllEntries();
        }
        catch (Exception ex)
        {
            SetStatus($"Error loading startup entries: {ex.Message}");
            _entries = Array.Empty<StartupEntry>();
        }

        foreach (StartupEntry e in _entries)
        {
            var item = new ListViewItem(e.Name) { Tag = e };
            item.SubItems.Add(e.IsEnabled ? "Enabled" : "Disabled");
            item.SubItems.Add(LocationLabel(e.Location));
            item.SubItems.Add(e.Command);
            item.ForeColor = e.IsEnabled ? SystemColors.WindowText : SystemColors.GrayText;
            _list.Items.Add(item);
        }

        _list.EndUpdate();
        SetStatus($"{_entries.Count} startup entries loaded.");
        UpdateButtons(null);
        _btnExportCsv.Enabled = _entries.Count > 0;
    }

    // ── Actions ──────────────────────────────────────────────────────────────

    private void OnToggle(bool enable)
    {
        if (SelectedEntry() is not StartupEntry entry)
            return;

        try
        {
            StartupManager.SetEnabled(entry, enable);
            SetStatus($"{(enable ? "Enabled" : "Disabled")}: {entry.Name}");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Could not {(enable ? "enable" : "disable")} '{entry.Name}':\n{ex.Message}",
                "Startup Manager",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }
        finally
        {
            RefreshList();
        }
    }

    private void OnDelete()
    {
        if (SelectedEntry() is not StartupEntry entry)
            return;

        var confirm = MessageBox.Show(
            $"Permanently delete startup entry '{entry.Name}'?\nThis cannot be undone.",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );

        if (confirm != DialogResult.Yes)
            return;

        try
        {
            StartupManager.Delete(entry);
            SetStatus($"Deleted: {entry.Name}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not delete '{entry.Name}':\n{ex.Message}", "Startup Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            RefreshList();
        }
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private StartupEntry? SelectedEntry() => _list.SelectedItems.Count > 0 ? _list.SelectedItems[0].Tag as StartupEntry : null;

    private void OnSelectionChanged(object? sender, EventArgs e) => UpdateButtons(SelectedEntry());

    private void UpdateButtons(StartupEntry? entry)
    {
        _btnEnable.Enabled = entry is { IsEnabled: false };
        _btnDisable.Enabled = entry is { IsEnabled: true };
        _btnDelete.Enabled = entry is not null;
    }

    private void SetStatus(string text) => _statusLabel.Text = text;

    private void ExportCsv()
    {
        using var dlg = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*", FileName = "startup-entries.csv" };
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;
        try
        {
            var lines = new List<string> { "Name,Status,Location,Command" };
            foreach (var e in _entries)
            {
                string status = e.IsEnabled ? "Enabled" : "Disabled";
                string loc = LocationLabel(e.Location);
                string cmd = e.Command.Replace("\"", "\"\"");
                lines.Add($"\"{e.Name}\",\"{status}\",\"{loc}\",\"{cmd}\"");
            }
            File.WriteAllLines(dlg.FileName, lines);
            SetStatus($"Exported {_entries.Count} entries to {Path.GetFileName(dlg.FileName)}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Export failed:\n{ex.Message}", "Export CSV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void OpenFileLocation()
    {
        if (SelectedEntry() is not StartupEntry entry)
            return;
        string cmd = entry.Command.Trim().Trim('"');
        int spaceIdx = cmd.IndexOf(' ');
        string exe = spaceIdx > 0 ? cmd[..spaceIdx] : cmd;
        string? dir = Path.GetDirectoryName(exe);
        if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", dir);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot open location:\n{ex.Message}", "Open Location", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        else
        {
            MessageBox.Show($"Directory not found:\n{dir ?? "(unknown)"}", "Open Location", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private static string LocationLabel(StartupLocation loc) =>
        loc switch
        {
            StartupLocation.RegistryUser => "Registry (User)",
            StartupLocation.RegistryMachine => "Registry (Machine)",
            StartupLocation.FolderUser => "Startup Folder (User)",
            StartupLocation.FolderAllUsers => "Startup Folder (All)",
            _ => loc.ToString(),
        };
}
