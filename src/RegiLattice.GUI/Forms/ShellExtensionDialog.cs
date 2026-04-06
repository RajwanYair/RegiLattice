// RegiLattice.GUI — Forms/ShellExtensionDialog.cs
// Windows Explorer shell extension (context-menu handler) manager.
#nullable enable

using System.Diagnostics;
using Microsoft.Win32;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Shell Extension Manager — enumerates COM-registered Explorer shell extensions
/// from HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved.
/// Allows enabling and disabling individual extensions by toggling a "(disabled)" prefix.
/// </summary>
internal sealed class ShellExtensionDialog : BaseDialog
{
    private const string ApprovedKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved";

    private sealed record ShellExtension(string Clsid, string Name, string DllPath, bool Enabled);

    private readonly ListView _list = new()
    {
        View = View.Details,
        FullRowSelect = true,
        MultiSelect = false,
        GridLines = true,
        Dock = DockStyle.Fill,
    };

    private readonly TextBox _searchBox = new()
    {
        PlaceholderText = "Search…",
        Width = 260,
        Height = 24,
    };
    private readonly Label _lblStatus = new()
    {
        Dock = DockStyle.Top,
        Height = 26,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };
    private readonly Button _btnEnable = new()
    {
        Text = "Enable",
        Width = 80,
        Height = 28,
        Enabled = false,
    };
    private readonly Button _btnDisable = new()
    {
        Text = "Disable",
        Width = 80,
        Height = 28,
        Enabled = false,
    };
    private readonly Button _btnRefresh = new()
    {
        Text = "⟳ Refresh",
        Width = 90,
        Height = 28,
    };
    private readonly Button _btnClose = new()
    {
        Text = "Close",
        Width = 75,
        Height = 28,
        DialogResult = DialogResult.Cancel,
    };
    private readonly Panel _btnPanel = new() { Dock = DockStyle.Bottom, Height = 38 };
    private readonly Panel _topBar = new() { Dock = DockStyle.Top, Height = 34 };

    private List<ShellExtension> _all = [];

    public ShellExtensionDialog()
        : base("Shell Extension Manager", new Size(960, 620), resizable: true)
    {
        MinimumSize = new Size(780, 460);
        EnableStandaloneMode();

        _list.Columns.AddRange([
            new ColumnHeader { Text = "Status", Width = 70 },
            new ColumnHeader { Text = "Name / Description", Width = 260 },
            new ColumnHeader { Text = "CLSID", Width = 200 },
            new ColumnHeader { Text = "DLL Path", Width = 340 },
        ]);
        ListViewColumnSorter.AttachTo(_list);
        _list.SelectedIndexChanged += (_, _) => UpdateButtons();

        _searchBox.Location = new Point(6, 4);
        _searchBox.TextChanged += (_, _) => FilterList(_searchBox.Text.Trim());
        _topBar.Controls.Add(_searchBox);
        _topBar.Controls.Add(
            new Label
            {
                Text = "Filter:",
                AutoSize = true,
                Location = new Point(274, 8),
            }
        );

        _btnEnable.Click += (_, _) => ToggleExtension(enable: true);
        _btnDisable.Click += (_, _) => ToggleExtension(enable: false);
        _btnRefresh.Click += async (_, _) => await RefreshAsync();
        _btnClose.Click += (_, _) => Close();

        int x = 8;
        foreach (Button b in new[] { _btnEnable, _btnDisable, _btnRefresh })
        {
            b.Location = new Point(x, 5);
            x += b.Width + 6;
        }
        _btnClose.Location = new Point(_btnPanel.Width - _btnClose.Width - 8, 5);
        _btnPanel.Resize += (_, _) => _btnClose.Location = new Point(_btnPanel.Width - _btnClose.Width - 8, 5);
        _btnPanel.Controls.AddRange([_btnEnable, _btnDisable, _btnRefresh, _btnClose]);

        Controls.Add(_list);
        Controls.Add(_topBar);
        Controls.Add(_lblStatus);
        Controls.Add(_btnPanel);

        _ = RefreshAsync();
    }

    private async Task RefreshAsync()
    {
        _btnRefresh.Enabled = false;
        _lblStatus.Text = "Loading shell extensions…";
        _all = await Task.Run(LoadExtensions);
        FilterList(_searchBox.Text.Trim());
        _lblStatus.Text = $"Found {_all.Count} registered shell extensions.";
        _btnRefresh.Enabled = true;
    }

    private static List<ShellExtension> LoadExtensions()
    {
        var result = new List<ShellExtension>();
        try
        {
            using RegistryKey? key = Registry.LocalMachine.OpenSubKey(ApprovedKey, writable: false);
            if (key is null)
                return result;

            foreach (string valueName in key.GetValueNames())
            {
                string rawName = key.GetValue(valueName)?.ToString() ?? "(no name)";
                bool enabled = !rawName.StartsWith("(disabled)", StringComparison.OrdinalIgnoreCase);
                string displayName = enabled ? rawName : rawName[10..].TrimStart();

                string dllPath = ResolveDllPath(valueName);
                result.Add(new ShellExtension(valueName, displayName, dllPath, enabled));
            }
        }
        catch
        {
            // Registry may be restricted — return whatever we have.
        }
        return result;
    }

    private static string ResolveDllPath(string clsid)
    {
        string[] paths = [$@"CLSID\{clsid}\InprocServer32", $@"Wow6432Node\CLSID\{clsid}\InprocServer32"];
        foreach (string path in paths)
        {
            try
            {
                using RegistryKey? k = Registry.ClassesRoot.OpenSubKey(path, writable: false);
                string? val = k?.GetValue(null)?.ToString();
                if (!string.IsNullOrEmpty(val))
                    return val;
            }
            catch (Exception) { }
        }
        return "(unknown)";
    }

    private void FilterList(string query)
    {
        IEnumerable<ShellExtension> src = string.IsNullOrEmpty(query)
            ? _all
            : _all.Where(e =>
                e.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                || e.Clsid.Contains(query, StringComparison.OrdinalIgnoreCase)
                || e.DllPath.Contains(query, StringComparison.OrdinalIgnoreCase)
            );

        _list.BeginUpdate();
        _list.Items.Clear();
        foreach (ShellExtension ext in src)
        {
            var lvi = new ListViewItem(ext.Enabled ? "✓ On" : "✗ Off");
            lvi.SubItems.Add(ext.Name);
            lvi.SubItems.Add(ext.Clsid);
            lvi.SubItems.Add(ext.DllPath);
            if (!ext.Enabled)
                lvi.ForeColor = Color.Gray;
            lvi.Tag = ext.Clsid;
            _list.Items.Add(lvi);
        }
        _list.EndUpdate();
    }

    private void UpdateButtons()
    {
        bool sel = _list.SelectedItems.Count > 0;
        _btnEnable.Enabled = sel;
        _btnDisable.Enabled = sel;
    }

    private void ToggleExtension(bool enable)
    {
        if (_list.SelectedItems.Count == 0)
            return;
        string clsid = _list.SelectedItems[0].Tag?.ToString() ?? "";
        if (string.IsNullOrEmpty(clsid))
            return;

        try
        {
            using RegistryKey? key = Registry.LocalMachine.OpenSubKey(ApprovedKey, writable: true);
            if (key is null)
            {
                _lblStatus.Text = "✗ Access denied. Run as administrator.";
                return;
            }
            string? current = key.GetValue(clsid)?.ToString() ?? "";
            string updated;
            if (enable)
                updated = current.StartsWith("(disabled)", StringComparison.OrdinalIgnoreCase) ? current[10..].TrimStart() : current;
            else
                updated = current.StartsWith("(disabled)", StringComparison.OrdinalIgnoreCase) ? current : "(disabled)" + current;

            key.SetValue(clsid, updated, RegistryValueKind.String);
            _lblStatus.Text = $"✓ {(enable ? "Enabled" : "Disabled")}: {clsid}";
            _ = RefreshAsync();
        }
        catch (UnauthorizedAccessException)
        {
            _lblStatus.Text = "✗ Access denied. Run as administrator to modify shell extensions.";
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"✗ Error: {ex.Message}";
        }
    }
}
