// RegiLattice.GUI — Forms/FirewallRulesDialog.cs
// Simplified Windows Firewall rule viewer and toggle (inbound/outbound rules).
#nullable enable

using System.Diagnostics;
using System.Management;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Simplified Windows Firewall rule manager.
/// Lists inbound and outbound rules with enable/disable controls.
/// Uses netsh advfirewall for operations (no COM interop required).
/// </summary>
internal sealed class FirewallRulesDialog : BaseDialog
{
    private sealed record FirewallRule(
        string Name,
        string Direction,
        string Action,
        string Enabled,
        string Profile,
        string Protocol,
        string LocalPort,
        string Program
    );

    private readonly TabControl _tabs = new() { Dock = DockStyle.Fill };
    private readonly TabPage _tabInbound = new("Inbound Rules");
    private readonly TabPage _tabOutbound = new("Outbound Rules");

    private readonly ListView _inboundList = CreateRuleList();
    private readonly ListView _outboundList = CreateRuleList();

    private readonly TextBox _searchBox = new()
    {
        PlaceholderText = "Search rules…",
        Width = 280,
        Height = 24,
    };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Top,
        Height = 26,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };
    private readonly Button _btnEnable = new()
    {
        Text = "Enable Rule",
        Width = 105,
        Height = 28,
        Enabled = false,
    };
    private readonly Button _btnDisable = new()
    {
        Text = "Disable Rule",
        Width = 105,
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
    private readonly Panel _searchPanel = new() { Dock = DockStyle.Top, Height = 34 };

    private List<FirewallRule> _inbound = [];
    private List<FirewallRule> _outbound = [];

    private static ListView CreateRuleList() =>
        new()
        {
            View = View.Details,
            FullRowSelect = true,
            MultiSelect = false,
            GridLines = true,
            Dock = DockStyle.Fill,
            VirtualMode = false,
        };

    public FirewallRulesDialog()
        : base("Windows Firewall Rules", new Size(1040, 640), resizable: true)
    {
        MinimumSize = new Size(860, 500);
        EnableStandaloneMode();

        foreach (ListView lv in new[] { _inboundList, _outboundList })
        {
            lv.Columns.AddRange([
                new ColumnHeader { Text = "Rule Name", Width = 280 },
                new ColumnHeader { Text = "Action", Width = 70 },
                new ColumnHeader { Text = "Enabled", Width = 70 },
                new ColumnHeader { Text = "Profile", Width = 85 },
                new ColumnHeader { Text = "Protocol", Width = 75 },
                new ColumnHeader { Text = "Local Port", Width = 90 },
                new ColumnHeader { Text = "Program", Width = 200 },
            ]);
            lv.SelectedIndexChanged += OnSelectionChanged;
        }

        _searchBox.TextChanged += (_, _) => FilterCurrentTab();
        _searchPanel.Controls.Add(
            new Label
            {
                Text = "Filter:",
                AutoSize = true,
                Location = new Point(6, 5),
            }
        );
        _searchBox.Location = new Point(50, 4);
        _searchPanel.Controls.Add(_searchBox);

        _tabInbound.Controls.Add(_inboundList);
        _tabOutbound.Controls.Add(_outboundList);
        _tabs.TabPages.AddRange([_tabInbound, _tabOutbound]);
        _tabs.SelectedIndexChanged += (_, _) => UpdateButtons();

        _btnEnable.Click += async (_, _) => await SetRuleStateAsync(enabled: true);
        _btnDisable.Click += async (_, _) => await SetRuleStateAsync(enabled: false);
        _btnRefresh.Click += async (_, _) => await RefreshAsync();
        _btnClose.Click += (_, _) => Close();

        _btnPanel.Controls.AddRange([_btnEnable, _btnDisable, _btnRefresh, _btnClose]);

        Controls.Add(_tabs);
        Controls.Add(_searchPanel);
        Controls.Add(_statusLabel);
        Controls.Add(_btnPanel);

        LayoutButtons();
        _ = RefreshAsync();
    }

    private void LayoutButtons()
    {
        int x = 8;
        foreach (Button b in new[] { _btnEnable, _btnDisable, _btnRefresh })
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
        _statusLabel.Text = "Loading firewall rules…";

        (_inbound, _outbound) = await Task.Run(LoadRules);
        PopulateList(_inboundList, _inbound);
        PopulateList(_outboundList, _outbound);

        _statusLabel.Text = $"Inbound: {_inbound.Count} rules  ·  Outbound: {_outbound.Count} rules";
        _btnRefresh.Enabled = true;
    }

    private static (List<FirewallRule> inbound, List<FirewallRule> outbound) LoadRules()
    {
        var inbound = new List<FirewallRule>();
        var outbound = new List<FirewallRule>();

        // Parse inbound
        inbound.AddRange(ParseNetshOutput(RunNetsh("advfirewall firewall show rule name=all dir=in"), "In"));
        outbound.AddRange(ParseNetshOutput(RunNetsh("advfirewall firewall show rule name=all dir=out"), "Out"));
        return (inbound, outbound);
    }

    private static IEnumerable<FirewallRule> ParseNetshOutput(string output, string direction)
    {
        // Split on blank lines (each rule is separated by blank line)
        string[] sections = output.Split(["\r\n\r\n", "\n\n"], StringSplitOptions.RemoveEmptyEntries);
        foreach (string section in sections)
        {
            string name = ExtractField(section, "Rule Name");
            if (string.IsNullOrEmpty(name) || name == "Rule Name")
                continue;
            yield return new FirewallRule(
                name,
                direction,
                ExtractField(section, "Action"),
                ExtractField(section, "Enabled"),
                ExtractField(section, "Profiles"),
                ExtractField(section, "Protocol"),
                ExtractField(section, "LocalPort"),
                ShortenProgram(ExtractField(section, "Program"))
            );
        }
    }

    private static string ExtractField(string block, string field)
    {
        foreach (string line in block.Split('\n'))
        {
            int colon = line.IndexOf(':');
            if (colon < 0)
                continue;
            string key = line[..colon].Trim();
            if (key.Equals(field, StringComparison.OrdinalIgnoreCase))
                return line[(colon + 1)..].Trim();
        }
        return "";
    }

    private static string ShortenProgram(string prog)
    {
        if (string.IsNullOrEmpty(prog) || prog == "Any")
            return "Any";
        return Path.GetFileName(prog);
    }

    private static void PopulateList(ListView lv, IEnumerable<FirewallRule> rules)
    {
        lv.BeginUpdate();
        lv.Items.Clear();
        foreach (FirewallRule r in rules)
        {
            var lvi = new ListViewItem(r.Name);
            lvi.SubItems.Add(r.Action);
            lvi.SubItems.Add(r.Enabled);
            lvi.SubItems.Add(r.Profile);
            lvi.SubItems.Add(r.Protocol);
            lvi.SubItems.Add(r.LocalPort);
            lvi.SubItems.Add(r.Program);
            if (r.Enabled.Equals("No", StringComparison.OrdinalIgnoreCase))
                lvi.ForeColor = Color.FromArgb(128, 128, 128);
            else if (r.Action.Equals("Block", StringComparison.OrdinalIgnoreCase))
                lvi.ForeColor = Color.FromArgb(200, 60, 60);
            lv.Items.Add(lvi);
        }
        lv.EndUpdate();
    }

    private void FilterCurrentTab()
    {
        string q = _searchBox.Text.Trim();
        ListView lv = _tabs.SelectedIndex == 0 ? _inboundList : _outboundList;
        List<FirewallRule> src = _tabs.SelectedIndex == 0 ? _inbound : _outbound;

        if (string.IsNullOrEmpty(q))
        {
            PopulateList(lv, src);
            return;
        }
        PopulateList(
            lv,
            src.Where(r =>
                r.Name.Contains(q, StringComparison.OrdinalIgnoreCase)
                || r.Program.Contains(q, StringComparison.OrdinalIgnoreCase)
                || r.Action.Contains(q, StringComparison.OrdinalIgnoreCase)
            )
        );
    }

    private void OnSelectionChanged(object? sender, EventArgs e) => UpdateButtons();

    private void UpdateButtons()
    {
        ListView lv = _tabs.SelectedIndex == 0 ? _inboundList : _outboundList;
        bool sel = lv.SelectedItems.Count > 0;
        _btnEnable.Enabled = sel;
        _btnDisable.Enabled = sel;
    }

    private async Task SetRuleStateAsync(bool enabled)
    {
        ListView lv = _tabs.SelectedIndex == 0 ? _inboundList : _outboundList;
        if (lv.SelectedItems.Count == 0)
            return;
        string name = lv.SelectedItems[0].Text;

        _btnEnable.Enabled = false;
        _btnDisable.Enabled = false;
        _statusLabel.Text = $"{(enabled ? "Enabling" : "Disabling")} rule \"{name}\"…";

        try
        {
            await Task.Run(() => RunNetsh($"advfirewall firewall set rule name=\"{name}\" new enable={(enabled ? "yes" : "no")}"));
            _statusLabel.Text = $"✓ Rule \"{name}\" {(enabled ? "enabled" : "disabled")}.";
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"✗ Failed: {ex.Message}";
            UpdateButtons();
        }
    }

    private static string RunNetsh(string args)
    {
        var info = new ProcessStartInfo("netsh", args)
        {
            RedirectStandardOutput = true,
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
