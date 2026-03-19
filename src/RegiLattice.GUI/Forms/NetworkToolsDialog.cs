// RegiLattice.GUI — Forms/NetworkToolsDialog.cs
// Sprint 27: DNS quick-switch + network repair wizard dialog.

using RegiLattice.Core;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Network Tools dialog (Sprint 27).
/// Provides DNS quick-switch for active adapters and a network repair wizard
/// that runs flush-DNS / TCP-IP reset / Winsock reset.
/// </summary>
internal sealed class NetworkToolsDialog : BaseDialog
{
    private readonly TabControl _tabs;

    // DNS tab controls
    private readonly ComboBox _adapterCombo;
    private readonly ComboBox _dnsPresetCombo;
    private readonly TextBox _primaryDnsBox;
    private readonly TextBox _secondaryDnsBox;
    private readonly Button _btnApplyDns;
    private readonly Button _btnRefreshAdapters;
    private readonly Label _dnsCurrentLabel;

    // Repair tab controls
    private readonly Button _btnFlushDns;
    private readonly Button _btnResetTcpIp;
    private readonly Button _btnResetWinsock;
    private readonly Button _btnFullRepair;
    private readonly RichTextBox _repairLog;

    private CancellationTokenSource _cts = new();

    // Ping tab controls
    private readonly TextBox _pingHostsBox = new()
    {
        Multiline = true,
        ScrollBars = ScrollBars.Vertical,
        PlaceholderText = "Enter hosts, one per line (e.g. 8.8.8.8)",
        Height = 72,
        Dock = DockStyle.Top,
        Font = new System.Drawing.Font("Consolas", 9f),
    };
    private readonly RichTextBox _pingLog = new()
    {
        ReadOnly = true,
        BorderStyle = BorderStyle.None,
        Dock = DockStyle.Fill,
    };
    private readonly Button _btnPingAll = new()
    {
        Text = "\uD83D\uDD01 Ping All",
        Width = 110,
        Height = 30,
    };

    // Traceroute tab controls
    private readonly TextBox _traceHostBox = new()
    {
        PlaceholderText = "Host to trace (e.g. google.com)",
        Dock = DockStyle.Top,
        Height = 26,
        Font = new System.Drawing.Font("Consolas", 9f),
    };
    private readonly RichTextBox _traceLog = new()
    {
        ReadOnly = true,
        BorderStyle = BorderStyle.None,
        Dock = DockStyle.Fill,
    };
    private readonly Button _btnTrace = new()
    {
        Text = "\uD83D\uDD0D Trace Route",
        Width = 120,
        Height = 30,
    };

    internal NetworkToolsDialog()
        : base("Network Tools", new Size(560, 480))
    {
        _tabs = new TabControl { Dock = DockStyle.Fill };

        // ── DNS Quick-Switch Tab ─────────────────────────────────────────
        var tabDns = new TabPage("DNS Quick-Switch");

        _adapterCombo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 270 };
        _dnsPresetCombo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200 };
        foreach (var p in DnsPreset.BuiltIn)
            _dnsPresetCombo.Items.Add(p.Name);
        _dnsPresetCombo.Items.Add("Custom…");
        _dnsPresetCombo.SelectedIndex = 0;
        _dnsPresetCombo.SelectedIndexChanged += OnDnsPresetChanged;

        _primaryDnsBox = new TextBox { Width = 160, PlaceholderText = "e.g. 1.1.1.1" };
        _secondaryDnsBox = new TextBox { Width = 160, PlaceholderText = "e.g. 1.0.0.1 (optional)" };
        _dnsCurrentLabel = new Label
        {
            AutoSize = true,
            ForeColor = AppTheme.FgDim,
            Text = "Current: —",
        };
        _btnApplyDns = new Button
        {
            Text = "Apply DNS",
            Width = 120,
            Height = 30,
        };
        _btnApplyDns.Click += OnApplyDnsAsync;
        _btnRefreshAdapters = new Button
        {
            Text = "⟳ Refresh",
            Width = 90,
            Height = 30,
        };
        _btnRefreshAdapters.Click += (_, _) => RefreshAdapters();

        var dnsPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 7,
            Padding = new Padding(12),
        };
        dnsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
        dnsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        var adapterRow = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        adapterRow.Controls.Add(_adapterCombo);
        adapterRow.Controls.Add(_btnRefreshAdapters);

        var note = new Label
        {
            Text = "⚠  Applying DNS requires Administrator. Use 'Automatic (DHCP)' to revert.",
            AutoSize = true,
            ForeColor = AppTheme.Yellow,
            Padding = new Padding(0, 4, 0, 4),
        };

        dnsPanel.Controls.Add(CreateLabel("Adapter:"), 0, 0);
        dnsPanel.Controls.Add(adapterRow, 1, 0);
        dnsPanel.Controls.Add(CreateLabel("Preset:"), 0, 1);
        dnsPanel.Controls.Add(_dnsPresetCombo, 1, 1);
        dnsPanel.Controls.Add(CreateLabel("Primary DNS:"), 0, 2);
        dnsPanel.Controls.Add(_primaryDnsBox, 1, 2);
        dnsPanel.Controls.Add(CreateLabel("Secondary DNS:"), 0, 3);
        dnsPanel.Controls.Add(_secondaryDnsBox, 1, 3);
        dnsPanel.Controls.Add(_dnsCurrentLabel, 0, 4);
        dnsPanel.SetColumnSpan(_dnsCurrentLabel, 2);
        dnsPanel.Controls.Add(note, 0, 5);
        dnsPanel.SetColumnSpan(note, 2);

        var dnsApplyRow = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        dnsApplyRow.Controls.Add(_btnApplyDns);
        dnsPanel.Controls.Add(dnsApplyRow, 1, 6);

        tabDns.Controls.Add(dnsPanel);

        // ── Network Repair Tab ───────────────────────────────────────────
        var tabRepair = new TabPage("Network Repair");

        _btnFlushDns = CreateRepairButton("🧹 Flush DNS Cache", "Runs ipconfig /flushdns — safe, no admin needed.");
        _btnFlushDns.Click += async (_, _) => await RunRepairAsync("Flush DNS Cache", NetworkManager.FlushDnsCacheAsync);

        _btnResetTcpIp = CreateRepairButton("🔄 Reset TCP/IP Stack", "Runs netsh int ip reset — admin required, reboot recommended.");
        _btnResetTcpIp.Click += async (_, _) => await RunRepairAsync("Reset TCP/IP", NetworkManager.ResetTcpIpAsync);

        _btnResetWinsock = CreateRepairButton("🔄 Reset Winsock", "Runs netsh winsock reset — admin required, reboot recommended.");
        _btnResetWinsock.Click += async (_, _) => await RunRepairAsync("Reset Winsock", NetworkManager.ResetWinsockAsync);

        _btnFullRepair = new Button
        {
            Text = "⚡ Full Repair (All Three Steps)",
            Width = 260,
            Height = 34,
            BackColor = AppTheme.Accent,
            ForeColor = AppTheme.Bg,
            FlatStyle = FlatStyle.Flat,
            Font = AppTheme.Bold,
        };
        _btnFullRepair.Click += OnFullRepairAsync;

        _repairLog = new RichTextBox
        {
            ReadOnly = true,
            BackColor = AppTheme.Bg,
            ForeColor = AppTheme.Green,
            Font = AppTheme.Mono,
            BorderStyle = BorderStyle.None,
            ScrollBars = RichTextBoxScrollBars.Vertical,
            Dock = DockStyle.Fill,
            Text = ">> Repair log will appear here.\r\n>> This dialog does NOT automatically reboot — you must restart manually.\r\n",
        };

        var repairTopPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.TopDown,
            AutoSize = true,
            Padding = new Padding(12, 12, 12, 6),
            WrapContents = false,
        };
        repairTopPanel.Controls.Add(_btnFlushDns);
        repairTopPanel.Controls.Add(_btnResetTcpIp);
        repairTopPanel.Controls.Add(_btnResetWinsock);
        repairTopPanel.Controls.Add(new Label { Height = 4 }); // spacer
        repairTopPanel.Controls.Add(_btnFullRepair);

        tabRepair.Controls.Add(_repairLog);
        tabRepair.Controls.Add(repairTopPanel);

        // ── Ping Tab ───────────────────────────────────────────
        var tabPing = new TabPage("Ping");
        var pingTopPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(8, 6, 8, 4),
        };
        var pingBtnRow = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
        pingBtnRow.Controls.Add(_btnPingAll);
        pingTopPanel.Controls.Add(_pingHostsBox);
        pingTopPanel.Controls.Add(pingBtnRow);
        _pingLog.BackColor = AppTheme.Bg;
        _pingLog.ForeColor = AppTheme.Green;
        _pingLog.Font = AppTheme.Mono;
        tabPing.Controls.Add(_pingLog);
        tabPing.Controls.Add(pingTopPanel);
        _btnPingAll.Click += async (_, _) => await RunPingAllAsync();

        // ── Traceroute Tab ───────────────────────────────────
        var tabTrace = new TabPage("Traceroute");
        var traceTopPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(8, 6, 8, 4),
        };
        var traceBtnRow = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
        traceBtnRow.Controls.Add(_btnTrace);
        traceTopPanel.Controls.Add(_traceHostBox);
        traceTopPanel.Controls.Add(traceBtnRow);
        _traceLog.BackColor = AppTheme.Bg;
        _traceLog.ForeColor = AppTheme.Green;
        _traceLog.Font = AppTheme.Mono;
        tabTrace.Controls.Add(_traceLog);
        tabTrace.Controls.Add(traceTopPanel);
        _btnTrace.Click += async (_, _) => await RunTraceAsync();

        // ── Tabs & close button ─────────────────────────────────────
        _tabs.TabPages.AddRange([tabDns, tabRepair, tabPing, tabTrace]);

        var btnClose = new Button
        {
            Text = "Close",
            DialogResult = DialogResult.OK,
            Dock = DockStyle.Bottom,
            Height = 36,
        };

        Controls.Add(_tabs);
        Controls.Add(btnClose);
        AcceptButton = btnClose;
        CancelButton = btnClose;

        ApplyTheme();
        RefreshAdapters();
    }

    // ── DNS helpers ─────────────────────────────────────────────────────

    private void RefreshAdapters()
    {
        string? current = _adapterCombo.SelectedItem as string;
        _adapterCombo.Items.Clear();
        foreach (string name in NetworkManager.GetActiveAdapterNames())
            _adapterCombo.Items.Add(name);

        if (current != null && _adapterCombo.Items.Contains(current))
            _adapterCombo.SelectedItem = current;
        else if (_adapterCombo.Items.Count > 0)
            _adapterCombo.SelectedIndex = 0;

        UpdateCurrentDnsLabel();
    }

    private void UpdateCurrentDnsLabel()
    {
        if (_adapterCombo.SelectedItem is not string adapter)
        {
            _dnsCurrentLabel.Text = "Current: —";
            return;
        }
        var (primary, secondary) = NetworkManager.GetCurrentDns(adapter);
        if (string.IsNullOrEmpty(primary))
            _dnsCurrentLabel.Text = "Current: Automatic (DHCP)";
        else if (string.IsNullOrEmpty(secondary))
            _dnsCurrentLabel.Text = $"Current: {primary}";
        else
            _dnsCurrentLabel.Text = $"Current: {primary}  /  {secondary}";
    }

    private void OnDnsPresetChanged(object? sender, EventArgs e)
    {
        int idx = _dnsPresetCombo.SelectedIndex;
        if (idx < 0 || idx >= DnsPreset.BuiltIn.Count)
        {
            // "Custom…"
            _primaryDnsBox.ReadOnly = false;
            _secondaryDnsBox.ReadOnly = false;
            return;
        }

        var preset = DnsPreset.BuiltIn[idx];
        _primaryDnsBox.Text = preset.Primary;
        _secondaryDnsBox.Text = preset.Secondary;
        bool isAuto = string.IsNullOrEmpty(preset.Primary);
        _primaryDnsBox.ReadOnly = !isAuto ? true : false; // let user edit only if auto
        _secondaryDnsBox.ReadOnly = !isAuto ? true : false;
        // Actually always let user see values but auto-fill them
        _primaryDnsBox.ReadOnly = false;
        _secondaryDnsBox.ReadOnly = false;
    }

    private async void OnApplyDnsAsync(object? sender, EventArgs e)
    {
        if (_adapterCombo.SelectedItem is not string adapter)
        {
            MessageBox.Show("Please select a network adapter.", "No Adapter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetBusy(true);
        try
        {
            var result = await NetworkManager
                .SetDnsAsync(adapter, _primaryDnsBox.Text.Trim(), _secondaryDnsBox.Text.Trim(), _cts.Token)
                .ConfigureAwait(true);

            MessageBox.Show(
                result.Message,
                result.Success ? "DNS Applied" : "DNS Error",
                MessageBoxButtons.OK,
                result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
            );

            UpdateCurrentDnsLabel();
        }
        finally
        {
            SetBusy(false);
        }
    }

    // ── Repair helpers ──────────────────────────────────────────────────

    private async Task RunRepairAsync(string title, Func<CancellationToken, Task<NetworkOperationResult>> op)
    {
        SetBusy(true);
        AppendRepairLog($"[{DateTime.Now:HH:mm:ss}] Running: {title}...");
        try
        {
            var result = await op(_cts.Token).ConfigureAwait(true);
            AppendRepairLog(result.Success ? $"  ✔  {result.Message}" : $"  ✘  {result.Message}");
        }
        catch (OperationCanceledException)
        {
            AppendRepairLog("  Cancelled.");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void OnFullRepairAsync(object? sender, EventArgs e)
    {
        SetBusy(true);
        AppendRepairLog($"[{DateTime.Now:HH:mm:ss}] Starting full network repair...");
        try
        {
            await foreach (var result in NetworkManager.RepairAllAsync(_cts.Token).ConfigureAwait(true))
                AppendRepairLog(result.Success ? $"  ✔  {result.Message}" : $"  ✘  {result.Message}");

            AppendRepairLog("Full repair complete. Reboot is recommended.");
        }
        catch (OperationCanceledException)
        {
            AppendRepairLog("Cancelled.");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void AppendRepairLog(string line)
    {
        if (InvokeRequired)
        {
            Invoke(() => AppendRepairLog(line));
            return;
        }
        _repairLog.AppendText(line + "\r\n");
        _repairLog.ScrollToCaret();
    }

    private void SetBusy(bool busy)
    {
        if (InvokeRequired)
        {
            Invoke(() => SetBusy(busy));
            return;
        }
        _btnApplyDns.Enabled = !busy;
        _btnFlushDns.Enabled = !busy;
        _btnResetTcpIp.Enabled = !busy;
        _btnResetWinsock.Enabled = !busy;
        _btnFullRepair.Enabled = !busy;
        _btnPingAll.Enabled = !busy;
        _btnTrace.Enabled = !busy;
        Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _cts.Cancel();
        _cts.Dispose();
        base.OnFormClosed(e);
    }

    private void ApplyTheme()
    {
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        AppTheme.Apply(this);
        foreach (TabPage page in _tabs.TabPages)
        {
            page.BackColor = AppTheme.Bg;
            page.ForeColor = AppTheme.Fg;
        }
        _repairLog.BackColor = AppTheme.Bg;
        _repairLog.ForeColor = AppTheme.Green;
        _btnFullRepair.BackColor = AppTheme.Accent;
        _btnFullRepair.ForeColor = AppTheme.Bg;
    }

    private static Button CreateRepairButton(string text, string tooltip)
    {
        var btn = new Button
        {
            Text = text,
            Width = 240,
            Height = 30,
            FlatStyle = FlatStyle.Flat,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(4, 0, 0, 0),
        };
        new ToolTip().SetToolTip(btn, tooltip);
        return btn;
    }

    private static Label CreateLabel(string text) =>
        new()
        {
            Text = text,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Padding = new Padding(0, 5, 0, 0),
        };

    private async Task RunPingAllAsync()
    {
        var hosts = _pingHostsBox.Text.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (hosts.Length == 0)
        {
            _pingLog.Text = "Enter at least one host above.";
            return;
        }
        _pingLog.Clear();
        _pingLog.AppendText($"[{DateTime.Now:HH:mm:ss}] Pinging {hosts.Length} host(s)\u2026\r\n\r\n");
        SetBusy(true);
        try
        {
            await Task.Run(
                async () =>
                {
                    using var pinger = new System.Net.NetworkInformation.Ping();
                    foreach (var host in hosts)
                    {
                        try
                        {
                            var reply = await pinger.SendPingAsync(host, 2000);
                            string line =
                                reply.Status == System.Net.NetworkInformation.IPStatus.Success
                                    ? $"  \u2714  {host, -30} {reply.Address}  {reply.RoundtripTime}ms"
                                    : $"  \u2718  {host, -30} {reply.Status}";
                            Invoke(() => _pingLog.AppendText(line + "\r\n"));
                        }
                        catch (Exception ex)
                        {
                            Invoke(() => _pingLog.AppendText($"  \u2718  {host, -30} {ex.Message}\r\n"));
                        }
                    }
                },
                _cts.Token
            );
        }
        catch (OperationCanceledException) { }
        finally
        {
            SetBusy(false);
        }
    }

    private async Task RunTraceAsync()
    {
        var host = _traceHostBox.Text.Trim();
        if (string.IsNullOrEmpty(host))
        {
            _traceLog.Text = "Enter a host to trace above.";
            return;
        }
        _traceLog.Clear();
        _traceLog.AppendText($"[{DateTime.Now:HH:mm:ss}] Tracing route to: {host}\r\n\r\n");
        SetBusy(true);
        try
        {
            await Task.Run(
                async () =>
                {
                    var psi = new System.Diagnostics.ProcessStartInfo("tracert.exe", $"-d -h 20 {host}")
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    };
                    using var proc = System.Diagnostics.Process.Start(psi)!;
                    while (true)
                    {
                        var line = await proc.StandardOutput.ReadLineAsync(_cts.Token).ConfigureAwait(false);
                        if (line is null)
                            break;
                        Invoke(() => _traceLog.AppendText(line + "\r\n"));
                    }
                    await proc.WaitForExitAsync(_cts.Token).ConfigureAwait(false);
                },
                _cts.Token
            );
        }
        catch (OperationCanceledException)
        {
            _traceLog.AppendText("\r\nCancelled.");
        }
        finally
        {
            SetBusy(false);
        }
    }
}
