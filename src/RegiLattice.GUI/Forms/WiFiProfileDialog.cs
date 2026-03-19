// RegiLattice.GUI — Forms/WiFiProfileDialog.cs
// Wi-Fi profile management: list, export, delete, forget profiles via netsh wlan.
#nullable enable

using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using RegiLattice.Core;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Wi-Fi saved profile manager.  Lists saved Wi-Fi profiles (via netsh wlan),
/// shows security details, allows export/import/delete.
/// </summary>
internal sealed class WiFiProfileDialog : BaseDialog
{
    private sealed record WifiProfile(string Name, string Interface, string Authentication, string Encryption, string ConnectionMode);

    private readonly ListView _profileList = new()
    {
        View = View.Details,
        FullRowSelect = true,
        GridLines = true,
        Dock = DockStyle.Fill,
    };

    private readonly RichTextBox _detailBox = new()
    {
        ReadOnly = true,
        Dock = DockStyle.Fill,
        BackColor = SystemColors.Info,
        ForeColor = SystemColors.InfoText,
        ScrollBars = RichTextBoxScrollBars.Vertical,
    };

    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Top,
        Height = 26,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };
    private readonly Button _btnExport = new()
    {
        Text = "Export Profile…",
        Width = 120,
        Height = 28,
        Enabled = false,
    };
    private readonly Button _btnExportAll = new()
    {
        Text = "Export All…",
        Width = 100,
        Height = 28,
    };
    private readonly Button _btnDelete = new()
    {
        Text = "🗑 Delete",
        Width = 85,
        Height = 28,
        Enabled = false,
    };
    private readonly Button _btnImport = new()
    {
        Text = "Import…",
        Width = 80,
        Height = 28,
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

    private List<WifiProfile> _profiles = [];

    public WiFiProfileDialog()
        : base("Wi-Fi Profile Manager", new Size(880, 600), resizable: true)
    {
        MinimumSize = new Size(740, 480);
        EnableStandaloneMode();

        _profileList.Columns.AddRange([
            new ColumnHeader { Text = "Network Name (SSID)", Width = 240 },
            new ColumnHeader { Text = "Interface", Width = 130 },
            new ColumnHeader { Text = "Authentication", Width = 130 },
            new ColumnHeader { Text = "Encryption", Width = 100 },
            new ColumnHeader { Text = "Auto-Connect", Width = 100 },
        ]);

        _profileList.SelectedIndexChanged += OnSelectionChanged;

        _btnExport.Click += OnExport;
        _btnExportAll.Click += OnExportAll;
        _btnDelete.Click += OnDelete;
        _btnImport.Click += OnImport;
        _btnRefresh.Click += async (_, _) => await RefreshAsync();
        _btnClose.Click += (_, _) => Close();

        _btnPanel.Controls.AddRange([_btnExport, _btnExportAll, _btnDelete, _btnImport, _btnRefresh, _btnClose]);

        BuildLayout();
        LayoutButtons();

        _ = RefreshAsync();
    }

    private void BuildLayout()
    {
        var splitter = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Horizontal,
            SplitterDistance = 280,
        };
        splitter.Panel1.Controls.Add(_profileList);
        splitter.Panel2.Controls.Add(_detailBox);

        Controls.Add(splitter);
        Controls.Add(_statusLabel);
        Controls.Add(_btnPanel);
    }

    private void LayoutButtons()
    {
        int x = 8;
        foreach (Button b in new[] { _btnExport, _btnExportAll, _btnDelete, _btnImport, _btnRefresh })
        {
            b.Location = new Point(x, 5);
            x += b.Width + 6;
        }
        _btnClose.Location = new Point(_btnPanel.Width - _btnClose.Width - 8, 5);
        _btnPanel.Resize += (_, _) => _btnClose.Location = new Point(_btnPanel.Width - _btnClose.Width - 8, 5);
    }

    private async Task RefreshAsync()
    {
        _btnRefresh.Enabled = false;
        _statusLabel.Text = "Loading Wi-Fi profiles…";
        _profiles = await Task.Run(LoadProfiles);

        _profileList.BeginUpdate();
        _profileList.Items.Clear();
        foreach (WifiProfile p in _profiles)
        {
            var lvi = new ListViewItem(p.Name);
            lvi.SubItems.Add(p.Interface);
            lvi.SubItems.Add(p.Authentication);
            lvi.SubItems.Add(p.Encryption);
            lvi.SubItems.Add(p.ConnectionMode == "auto" ? "Yes" : "No");
            _profileList.Items.Add(lvi);
        }
        _profileList.EndUpdate();

        _statusLabel.Text =
            _profiles.Count == 0
                ? "No saved Wi-Fi profiles found. Wi-Fi may not be available on this device."
                : $"{_profiles.Count} saved Wi-Fi profile(s).";
        _btnRefresh.Enabled = true;
    }

    private static List<WifiProfile> LoadProfiles()
    {
        var profiles = new List<WifiProfile>();
        // Get list of all profiles
        string listOutput = RunNetsh("wlan show profiles");

        // Each interface section: "All User Profile     :  SSID name"
        var profileMatches = Regex.Matches(listOutput, @"All User Profile\s*:\s*(.+)", RegexOptions.IgnoreCase);

        // Detect current interface name
        var interfaceMatches = Regex.Matches(listOutput, @"Profiles on interface\s+(.+?):", RegexOptions.IgnoreCase);
        string currentInterface = interfaceMatches.Count > 0 ? interfaceMatches[0].Groups[1].Value.Trim() : "Wi-Fi";
        foreach (Match m in interfaceMatches)
        {
            currentInterface = m.Groups[1].Value.Trim();
            // Count how many profiles follow until next interface
        }

        foreach (Match m in profileMatches)
        {
            string ssid = m.Groups[1].Value.Trim();
            string auth = "",
                enc = "",
                mode = "auto";

            // Get detailed profile info
            string detail = RunNetsh($"wlan show profile name=\"{ssid}\" key=clear");
            auth = Regex.Match(detail, @"Authentication\s*:\s*(.+)").Groups[1].Value.Trim();
            enc = Regex.Match(detail, @"Cipher\s*:\s*(.+)").Groups[1].Value.Trim();
            mode = Regex.Match(detail, @"Connection mode\s*:\s*(.+)").Groups[1].Value.Trim();

            profiles.Add(new WifiProfile(ssid, currentInterface, auth, enc, mode));
        }
        return profiles;
    }

    private void OnSelectionChanged(object? sender, EventArgs e)
    {
        bool sel = _profileList.SelectedItems.Count > 0;
        _btnExport.Enabled = sel;
        _btnDelete.Enabled = sel;

        if (!sel)
            return;
        int idx = _profileList.SelectedItems[0].Index;
        WifiProfile p = _profiles[idx];

        _detailBox.Text =
            $"SSID:            {p.Name}\r\n"
            + $"Interface:       {p.Interface}\r\n"
            + $"Authentication:  {p.Authentication}\r\n"
            + $"Encryption:      {p.Encryption}\r\n"
            + $"Auto-connect:    {(p.ConnectionMode == "auto" ? "Yes" : "No")}\r\n\r\n"
            + "Use 'Export Profile' to save the profile as an XML file.\r\n"
            + "The exported file will include the security key in plain text (key=clear).";
    }

    private void OnExport(object? sender, EventArgs e)
    {
        if (_profileList.SelectedItems.Count == 0)
            return;
        string ssid = _profiles[_profileList.SelectedItems[0].Index].Name;
        ExportProfiles([ssid]);
    }

    private void OnExportAll(object? sender, EventArgs e) => ExportProfiles(_profiles.Select(p => p.Name).ToList());

    private void ExportProfiles(IReadOnlyList<string> ssids)
    {
        using var dlg = new FolderBrowserDialog { Description = "Select folder to save Wi-Fi profile(s)", UseDescriptionForTitle = true };
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        int exported = 0;
        foreach (string ssid in ssids)
        {
            string output = RunNetsh($"wlan export profile name=\"{ssid}\" folder=\"{dlg.SelectedPath}\" key=clear");
            if (output.Contains("successfully exported", StringComparison.OrdinalIgnoreCase))
                exported++;
        }
        _statusLabel.Text = $"✓ {exported} of {ssids.Count} profile(s) exported to: {dlg.SelectedPath}";
    }

    private void OnDelete(object? sender, EventArgs e)
    {
        if (_profileList.SelectedItems.Count == 0)
            return;
        WifiProfile p = _profiles[_profileList.SelectedItems[0].Index];

        var confirm = MessageBox.Show(
            $"Permanently forget saved Wi-Fi profile \"{p.Name}\"?\r\n\r\nYou will need to enter the password to reconnect.",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );
        if (confirm != DialogResult.Yes)
            return;

        RunNetsh($"wlan delete profile name=\"{p.Name}\"");
        _statusLabel.Text = $"✓ Profile \"{p.Name}\" deleted.";
        _ = RefreshAsync();
    }

    private void OnImport(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog { Title = "Import Wi-Fi Profile", Filter = "Wi-Fi Profile (*.xml)|*.xml|All files (*.*)|*.*" };
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        string output = RunNetsh($"wlan add profile filename=\"{dlg.FileName}\"");
        _statusLabel.Text = output.Contains("added", StringComparison.OrdinalIgnoreCase)
            ? $"✓ Profile imported from: {Path.GetFileName(dlg.FileName)}"
            : $"Import result: {output.Trim()}";
        _ = RefreshAsync();
    }

    private static string RunNetsh(string args)
    {
        var info = new ProcessStartInfo("netsh", args)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        using Process? proc = Process.Start(info);
        if (proc is null)
            return "";
        string output = proc.StandardOutput.ReadToEnd();
        proc.WaitForExit();
        return output;
    }
}
