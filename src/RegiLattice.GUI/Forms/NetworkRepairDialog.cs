// RegiLattice.GUI — Forms/NetworkRepairDialog.cs
// One-click network repair wizard: TCP/IP reset, Winsock reset, DNS flush, IP renew, etc.
#nullable enable

using System.Diagnostics;
using System.Text;
using RegiLattice.Core;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// One-click network repair wizard.  Runs netsh/ipconfig commands to reset the
/// TCP/IP stack, Winsock catalog, DNS cache, and IP address lease.
/// </summary>
internal sealed class NetworkRepairDialog : BaseDialog
{
    private sealed record RepairAction(string Name, string Description, string Command, string Args, bool RequiresRestart = false);

    private static readonly IReadOnlyList<RepairAction> Actions =
    [
        new(
            "Flush DNS Cache",
            "Clears the DNS resolver cache. Forces the system to look up domain names fresh "
                + "from DNS servers, fixing stale or corrupted DNS entries. Safe to run anytime.",
            "ipconfig",
            "/flushdns"
        ),
        new(
            "Release IP Address",
            "Releases the current DHCP-assigned IP address. Run this before 'Renew IP' to "
                + "force the adapter to request a fresh address from the DHCP server.",
            "ipconfig",
            "/release"
        ),
        new(
            "Renew IP Address",
            "Requests a new IP address lease from the DHCP server. Fixes 'Limited connectivity' "
                + "or 169.254.x.x APIPA address issues after releasing.",
            "ipconfig",
            "/renew"
        ),
        new(
            "Reset TCP Auto-Tuning",
            "Resets TCP receive window auto-tuning level to 'normal'. Fixes slow download speeds "
                + "caused by misconfigured Windows settings or third-party network software.",
            "netsh",
            "int tcp set global autotuninglevel=normal"
        ),
        new(
            "Reset Winsock Catalog",
            "Resets the Winsock catalog to its default clean state. Fixes corrupted socket entries "
                + "caused by malware, failed VPN software, or network stack corruption. "
                + "Requires system restart to take effect.",
            "netsh",
            "winsock reset",
            RequiresRestart: true
        ),
        new(
            "Reset TCP/IP Stack",
            "Rewrites the TCP/IP registry keys and resets all TCP/IP-related settings to defaults. "
                + "Useful after misconfigured network software has altered the stack. "
                + "Requires system restart to take effect.",
            "netsh",
            "int ip reset",
            RequiresRestart: true
        ),
        new(
            "Reset IPv6 Stack",
            "Resets all IPv6 settings to their default configuration. " + "Fixes connectivity issues on networks using IPv6 addressing.",
            "netsh",
            "int ipv6 reset"
        ),
        new(
            "Reset Windows Firewall Policy",
            "Resets all Windows Firewall rules and policies to default configuration. "
                + "Use this when misconfigured rules are blocking applications or causing network issues. "
                + "Note: all custom rules will be deleted.",
            "netsh",
            "advfirewall reset"
        ),
    ];

    private readonly ListView _actionList = new()
    {
        View = View.Details,
        FullRowSelect = true,
        CheckBoxes = true,
        GridLines = true,
        Dock = DockStyle.Fill,
    };

    private readonly RichTextBox _logBox = new()
    {
        ReadOnly = true,
        BackColor = Color.FromArgb(15, 15, 15),
        ForeColor = Color.LightGray,
        Font = new Font("Consolas", 9f),
        Dock = DockStyle.Fill,
        ScrollBars = RichTextBoxScrollBars.Vertical,
    };

    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Top,
        Height = 26,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };

    private readonly Button _btnRunSelected = new()
    {
        Text = "▶  Run Selected",
        Width = 140,
        Height = 28,
    };
    private readonly Button _btnRunAll = new()
    {
        Text = "▶▶  Run All",
        Width = 110,
        Height = 28,
    };
    private readonly Button _btnSelectAll = new()
    {
        Text = "Check All",
        Width = 90,
        Height = 28,
    };
    private readonly Button _btnClear = new()
    {
        Text = "Clear Log",
        Width = 85,
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

    public NetworkRepairDialog()
        : base("Network Repair Wizard", new Size(860, 640), resizable: true)
    {
        MinimumSize = new Size(720, 500);
        EnableStandaloneMode();

        _actionList.Columns.Add("Repair Action", 200);
        _actionList.Columns.Add("Description", 440);
        _actionList.Columns.Add("Restart?", 70, HorizontalAlignment.Center);
        ListViewColumnSorter.AttachTo(_actionList);

        foreach (RepairAction a in Actions)
        {
            string desc = a.Description.Length > 100 ? a.Description[..100] + "…" : a.Description;
            var lvi = new ListViewItem(a.Name) { Checked = true };
            lvi.SubItems.Add(desc);
            lvi.SubItems.Add(a.RequiresRestart ? "⚠ Yes" : "—");
            _actionList.Items.Add(lvi);
        }

        _statusLabel.Text = $"{Actions.Count} repair operations available.";
        _logBox.AppendText("Select operations above and click Run Selected or Run All.\r\n");

        _btnRunSelected.Click += async (_, _) => await RunActionsAsync(selectedOnly: true);
        _btnRunAll.Click += async (_, _) => await RunActionsAsync(selectedOnly: false);
        _btnSelectAll.Click += (_, _) =>
        {
            bool allChecked = _actionList.Items.Cast<ListViewItem>().All(i => i.Checked);
            foreach (ListViewItem lvi in _actionList.Items)
                lvi.Checked = !allChecked;
        };
        _btnClear.Click += (_, _) =>
        {
            _logBox.Clear();
            _logBox.AppendText("Log cleared.\r\n");
        };
        _btnClose.Click += (_, _) => Close();

        _btnPanel.Controls.AddRange([_btnRunSelected, _btnRunAll, _btnSelectAll, _btnClear, _btnClose]);

        if (!Elevation.IsAdmin())
            Controls.Add(CreateAdminBanner("⚠  Administrator rights required. Run as Administrator for all operations to succeed."));

        BuildLayout();
        LayoutButtons();
    }

    private void BuildLayout()
    {
        var splitter = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Horizontal,
            SplitterDistance = 220,
        };
        splitter.Panel1.Controls.Add(_actionList);
        splitter.Panel2.Controls.Add(_logBox);

        Controls.Add(splitter);
        Controls.Add(_statusLabel);
        Controls.Add(_btnPanel);
    }

    private void LayoutButtons()
    {
        int x = 8;
        foreach (Button b in new[] { _btnRunSelected, _btnRunAll, _btnSelectAll, _btnClear })
        {
            b.Location = new Point(x, 5);
            x += b.Width + 6;
        }
        _btnClose.Location = new Point(_btnPanel.Width - _btnClose.Width - 8, 5);
        _btnPanel.Resize += (_, _) => _btnClose.Location = new Point(_btnPanel.Width - _btnClose.Width - 8, 5);
    }

    private async Task RunActionsAsync(bool selectedOnly)
    {
        _btnRunSelected.Enabled = false;
        _btnRunAll.Enabled = false;
        int ran = 0,
            failed = 0;

        for (int i = 0; i < _actionList.Items.Count; i++)
        {
            if (selectedOnly && !_actionList.Items[i].Checked)
                continue;

            RepairAction action = Actions[i];
            AppendLog($"\r\n▶  {action.Name}", Color.Cyan);

            try
            {
                string output = await Task.Run(() => RunCmdCommand(action.Command, action.Args));
                if (!string.IsNullOrWhiteSpace(output))
                    AppendLog(output.TrimEnd(), Color.LightGray);
                AppendLog("✓ Done", Color.LimeGreen);
                ran++;
            }
            catch (Exception ex)
            {
                AppendLog($"✗ Error: {ex.Message}", Color.Tomato);
                failed++;
            }
        }

        AppendLog("\r\n─────────────────────────────────────────", Color.DimGray);
        AppendLog($"Completed: {ran} succeeded, {failed} failed.", failed > 0 ? Color.Orange : Color.LimeGreen);

        bool needsRestart = Actions.Where((_, idx) => !selectedOnly || _actionList.Items[idx].Checked).Any(a => a.RequiresRestart);
        if (needsRestart && ran > 0)
            AppendLog("⚠  Some operations require a system restart to take full effect.", Color.Gold);

        _statusLabel.Text = $"Last run: {ran} succeeded, {failed} failed.{(needsRestart && ran > 0 ? "  Restart recommended." : "")}";
        _btnRunSelected.Enabled = true;
        _btnRunAll.Enabled = true;
    }

    private static string RunCmdCommand(string command, string args)
    {
        var info = new ProcessStartInfo("cmd.exe", $"/c {command} {args}")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        using Process? proc = Process.Start(info);
        if (proc is null)
            return "(process failed to start)";
        var sb = new StringBuilder();
        sb.Append(proc.StandardOutput.ReadToEnd());
        sb.Append(proc.StandardError.ReadToEnd());
        proc.WaitForExit();
        return sb.ToString();
    }

    private void AppendLog(string text, Color color)
    {
        if (InvokeRequired)
        {
            Invoke(() => AppendLog(text, color));
            return;
        }
        _logBox.SelectionStart = _logBox.TextLength;
        _logBox.SelectionLength = 0;
        _logBox.SelectionColor = color;
        _logBox.AppendText(text + "\r\n");
        _logBox.ScrollToCaret();
    }
}
