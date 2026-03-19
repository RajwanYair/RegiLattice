// RegiLattice.GUI — Forms/PortScannerDialog.cs
// Sprint 41: TCP port / connectivity tester (Phase 4 item 37).

#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Lets the user enter a hostname/IP and a set of ports, then tests TCP connectivity to each.
/// Also includes a one-click ping test and common-port presets.
/// </summary>
[SupportedOSPlatform("windows")]
internal sealed class PortScannerDialog : BaseDialog
{
    // ── Preset port sets ─────────────────────────────────────────────────────
    private static readonly Dictionary<string, int[]> _presets = new()
    {
        ["Web (80, 443)"] = [80, 443],
        ["SSH / RDP (22, 3389)"] = [22, 3389],
        ["FTP (21, 990)"] = [21, 990],
        ["Mail (25, 110, 143, 587)"] = [25, 110, 143, 587],
        ["Database (1433, 3306, 5432, 27017)"] = [1433, 3306, 5432, 27017],
        ["DNS / DHCP (53, 67, 68)"] = [53, 67, 68],
        ["Common Top 20"] = [21, 22, 23, 25, 53, 80, 110, 135, 139, 143, 443, 445, 993, 995, 1723, 3306, 3389, 5900, 8080, 8443],
    };

    // ── Controls ─────────────────────────────────────────────────────────────
    private readonly TextBox _txtHost = new()
    {
        Text = "localhost",
        Width = 240,
        Anchor = AnchorStyles.Left | AnchorStyles.Right,
    };
    private readonly TextBox _txtPorts = new()
    {
        Text = "80, 443, 22, 3389",
        Width = 240,
        Anchor = AnchorStyles.Left | AnchorStyles.Right,
    };
    private readonly ComboBox _presetCombo = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 240 };
    private readonly Button _btnScan = new()
    {
        Text = "\uD83D\uDD0D  Scan",
        Width = 100,
        Height = 30,
    };
    private readonly Button _btnPing = new()
    {
        Text = "\uD83D\uDCE1  Ping",
        Width = 80,
        Height = 30,
    };
    private readonly Button _btnClose = new()
    {
        Text = "Close",
        Width = 80,
        Height = 30,
    };
    private readonly Button _btnSaveProfile = new()
    {
        Text = "\U0001F4BE Save Profile",
        Width = 110,
        Height = 30,
    };
    private readonly Button _btnLoadProfile = new()
    {
        Text = "\U0001F4C2 Load Profile",
        Width = 110,
        Height = 30,
    };
    private readonly ListView _results = new()
    {
        Dock = DockStyle.Fill,
        View = View.Details,
        FullRowSelect = true,
        GridLines = false,
        BorderStyle = BorderStyle.None,
    };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };
    private CancellationTokenSource _cts = new();

    internal PortScannerDialog()
        : base("Port / Connectivity Tester", new Size(520, 480), resizable: true)
    {
        BuildLayout();
    }

    private void BuildLayout()
    {
        // Form grid
        var grid = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            ColumnCount = 2,
            RowCount = 3,
            Height = 100,
            Padding = new Padding(8, 6, 8, 0),
        };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        grid.Controls.Add(
            new Label
            {
                Text = "Host / IP:",
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
            },
            0,
            0
        );
        grid.Controls.Add(_txtHost, 1, 0);
        grid.Controls.Add(
            new Label
            {
                Text = "Ports (CSV):",
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
            },
            0,
            1
        );
        grid.Controls.Add(_txtPorts, 1, 1);
        grid.Controls.Add(
            new Label
            {
                Text = "Preset:",
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
            },
            0,
            2
        );

        foreach (string key in _presets.Keys)
            _presetCombo.Items.Add(key);
        _presetCombo.SelectedIndex = 0;
        grid.Controls.Add(_presetCombo, 1, 2);

        _results.Columns.Add("Port", 80);
        _results.Columns.Add("Service", 120);
        _results.Columns.Add("Status", 100);
        _results.Columns.Add("Latency", 90);
        ListViewColumnSorter.AttachTo(_results);
        _results.BackColor = AppTheme.Surface;
        _results.ForeColor = AppTheme.Fg;

        var btnPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 44,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new Padding(8, 6, 8, 6),
        };
        btnPanel.Controls.AddRange([_btnClose, _btnScan, _btnPing, _btnLoadProfile, _btnSaveProfile]);

        Controls.Add(_results);
        Controls.Add(grid);
        Controls.Add(_statusLabel);
        Controls.Add(btnPanel);

        _presetCombo.SelectedIndexChanged += OnPresetSelected;
        _btnScan.Click += async (_, _) => await ScanAsync();
        _btnPing.Click += async (_, _) => await PingAsync();
        _btnClose.Click += (_, _) => Close();
        _btnSaveProfile.Click += OnSaveProfile;
        _btnLoadProfile.Click += OnLoadProfile;

        _txtHost.BackColor = AppTheme.Overlay;
        _txtHost.ForeColor = AppTheme.Fg;
        _txtPorts.BackColor = AppTheme.Overlay;
        _txtPorts.ForeColor = AppTheme.Fg;
        _presetCombo.BackColor = AppTheme.Surface;
        _presetCombo.ForeColor = AppTheme.Fg;
        AppTheme.Apply(this);
    }

    private void OnPresetSelected(object? sender, EventArgs e)
    {
        if (_presetCombo.SelectedItem is string key && _presets.TryGetValue(key, out var ports))
            _txtPorts.Text = string.Join(", ", ports);
    }

    private async Task PingAsync()
    {
        string host = _txtHost.Text.Trim();
        if (string.IsNullOrEmpty(host))
            return;
        _statusLabel.Text = $"Pinging {host}…";
        _btnPing.Enabled = false;
        try
        {
            using var ping = new Ping();
            var reply = await Task.Run(() => ping.Send(host, 3000));
            _statusLabel.Text = reply.Status == IPStatus.Success ? $"Ping {host}: {reply.RoundtripTime} ms — Alive" : $"Ping {host}: {reply.Status}";
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"Ping failed: {ex.Message}";
        }
        finally
        {
            _btnPing.Enabled = true;
        }
    }

    private async Task ScanAsync()
    {
        string host = _txtHost.Text.Trim();
        if (string.IsNullOrEmpty(host))
            return;

        var portStrs = _txtPorts.Text.Split([',', ' ', ';'], StringSplitOptions.RemoveEmptyEntries);
        var ports = portStrs.Select(s => int.TryParse(s.Trim(), out int p) ? (int?)p : null).OfType<int>().Distinct().OrderBy(p => p).ToArray();

        if (ports.Length == 0)
        {
            _statusLabel.Text = "No valid ports specified.";
            return;
        }

        _btnScan.Enabled = false;
        _results.Items.Clear();
        _statusLabel.Text = $"Scanning {host} — 0 / {ports.Length}…";

        _cts.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        int done = 0;
        var tasks = ports
            .Select(async port =>
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                string status;
                try
                {
                    if (token.IsCancellationRequested)
                        return;
                    using var tcpClient = new TcpClient();
                    await tcpClient.ConnectAsync(host, port).WaitAsync(TimeSpan.FromSeconds(2), token);
                    sw.Stop();
                    status = "OPEN";
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (Exception)
                {
                    sw.Stop();
                    status = "CLOSED";
                }

                if (token.IsCancellationRequested)
                    return;

                string svc = WellKnownService(port);
                string latency = status == "OPEN" ? $"{sw.ElapsedMilliseconds} ms" : "—";

                if (InvokeRequired)
                {
                    Invoke(() => AddResult(port, svc, status, latency, Interlocked.Increment(ref done), ports.Length));
                }
                else
                {
                    AddResult(port, svc, status, latency, Interlocked.Increment(ref done), ports.Length);
                }
            })
            .ToList();

        await Task.WhenAll(tasks);

        if (!token.IsCancellationRequested)
        {
            int open = _results.Items.Cast<ListViewItem>().Count(i => i.SubItems[2].Text == "OPEN");
            _statusLabel.Text = $"Scan complete — {open} open, {ports.Length - open} closed";
        }

        _btnScan.Enabled = true;
    }

    private void AddResult(int port, string svc, string status, string latency, int done, int total)
    {
        var item = new ListViewItem(port.ToString());
        item.SubItems.Add(svc);
        item.SubItems.Add(status);
        item.SubItems.Add(latency);
        item.ForeColor = status == "OPEN" ? Color.FromArgb(166, 227, 161) : AppTheme.FgDim;
        _results.Items.Add(item);
        _statusLabel.Text = $"Scanning — {done} / {total}…";
    }

    private static string WellKnownService(int port) =>
        port switch
        {
            21 => "FTP",
            22 => "SSH",
            23 => "Telnet",
            25 => "SMTP",
            53 => "DNS",
            67 => "DHCP",
            80 => "HTTP",
            110 => "POP3",
            135 => "RPC",
            139 => "NetBIOS",
            143 => "IMAP",
            443 => "HTTPS",
            445 => "SMB",
            587 => "SMTP/TLS",
            993 => "IMAP/SSL",
            995 => "POP3/SSL",
            1433 => "MS SQL",
            1723 => "PPTP",
            3306 => "MySQL",
            3389 => "RDP",
            5432 => "PostgreSQL",
            5900 => "VNC",
            8080 => "HTTP-alt",
            8443 => "HTTPS-alt",
            27017 => "MongoDB",
            _ => "—",
        };

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _cts.Dispose();
        base.Dispose(disposing);
    }

    // ── Scan profile persistence ─────────────────────────────────

    private static string ProfilesDir =>
        System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RegiLattice", "scan-profiles");

    private void OnSaveProfile(object? sender, EventArgs e)
    {
        string host = _txtHost.Text.Trim();
        string ports = _txtPorts.Text.Trim();
        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(ports))
        {
            _statusLabel.Text = "\u274c Enter a host and ports before saving a profile.";
            return;
        }

        using var input = new Form
        {
            Text = "Save Scan Profile",
            FormBorderStyle = FormBorderStyle.FixedDialog,
            StartPosition = FormStartPosition.CenterParent,
            MaximizeBox = false,
            MinimizeBox = false,
            ClientSize = new Size(340, 110),
            BackColor = AppTheme.Bg,
            ForeColor = AppTheme.Fg,
        };
        var lbl = new Label
        {
            Text = "Profile name:",
            Location = new Point(10, 16),
            AutoSize = true,
        };
        var txt = new TextBox
        {
            Location = new Point(100, 12),
            Width = 220,
            BackColor = AppTheme.Overlay,
            ForeColor = AppTheme.Fg,
        };
        var ok = new Button
        {
            Text = "Save",
            DialogResult = DialogResult.OK,
            Location = new Point(150, 65),
            Width = 75,
        };
        var cancel = new Button
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Location = new Point(235, 65),
            Width = 75,
        };
        input.Controls.AddRange(new Control[] { lbl, txt, ok, cancel });
        input.AcceptButton = ok;
        input.CancelButton = cancel;

        if (input.ShowDialog(this) != DialogResult.OK)
            return;

        string name = txt.Text.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            _statusLabel.Text = "\u274c Profile name cannot be empty.";
            return;
        }

        try
        {
            Directory.CreateDirectory(ProfilesDir);
            string slug = System.Text.RegularExpressions.Regex.Replace(name.ToLowerInvariant(), @"[^a-z0-9\-_]+", "-").Trim('-');
            string path = System.IO.Path.Combine(ProfilesDir, $"{slug}.json");
            var obj = new
            {
                name,
                host,
                ports,
            };
            File.WriteAllText(
                path,
                System.Text.Json.JsonSerializer.Serialize(obj, new System.Text.Json.JsonSerializerOptions { WriteIndented = true })
            );
            _statusLabel.Text = $"\u2705 Profile \u2018{name}\u2019 saved.";
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"\u274c {ex.Message}";
        }
    }

    private void OnLoadProfile(object? sender, EventArgs e)
    {
        try
        {
            Directory.CreateDirectory(ProfilesDir);
            var files = Directory.GetFiles(ProfilesDir, "*.json");
            if (files.Length == 0)
            {
                _statusLabel.Text = "\u274c No saved profiles found.";
                return;
            }

            // Build a selection dialog
            using var dlg = new Form
            {
                Text = "Load Scan Profile",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                ClientSize = new Size(340, 220),
                BackColor = AppTheme.Bg,
                ForeColor = AppTheme.Fg,
            };
            var list = new ListBox
            {
                Dock = DockStyle.Fill,
                BackColor = AppTheme.Surface,
                ForeColor = AppTheme.Fg,
            };
            var profileMap = new Dictionary<string, string>(); // display name -> file path
            foreach (string f in files)
            {
                try
                {
                    var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(f));
                    string displayName = doc.RootElement.TryGetProperty("name", out var n) ? n.GetString() ?? f : f;
                    profileMap[displayName] = f;
                    list.Items.Add(displayName);
                }
                catch
                {
                    list.Items.Add(System.IO.Path.GetFileNameWithoutExtension(f));
                    profileMap[System.IO.Path.GetFileNameWithoutExtension(f)] = f;
                }
            }
            var ok = new Button
            {
                Text = "Load",
                DialogResult = DialogResult.OK,
                Dock = DockStyle.Bottom,
                Height = 32,
            };
            dlg.Controls.AddRange(new Control[] { list, ok });
            dlg.AcceptButton = ok;

            if (dlg.ShowDialog(this) != DialogResult.OK || list.SelectedItem is null)
                return;

            string selected = list.SelectedItem.ToString()!;
            if (!profileMap.TryGetValue(selected, out string? filePath))
                return;

            var data = System.Text.Json.JsonDocument.Parse(File.ReadAllText(filePath)).RootElement;
            if (data.TryGetProperty("host", out var h))
                _txtHost.Text = h.GetString() ?? "";
            if (data.TryGetProperty("ports", out var p))
                _txtPorts.Text = p.GetString() ?? "";
            _statusLabel.Text = $"\u2705 Profile \u2018{selected}\u2019 loaded.";
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"\u274c {ex.Message}";
        }
    }
}
