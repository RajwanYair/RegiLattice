// RegiLattice.GUI — Forms/DnsSwitcherDialog.cs
// DNS server quick-switch with 8 presets + custom entry, applied via netsh.
#nullable enable

using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using RegiLattice.Core;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Quick DNS server switcher.  Detects active network adapters, displays presets
/// (Cloudflare, Google, Quad9, etc.), and applies the selection via netsh.
/// Note: distinct from DnsOverHttpsDialog which configures Windows 11 DoH.
/// </summary>
internal sealed class DnsSwitcherDialog : BaseDialog
{
    private sealed record DnsPreset(string Name, string Primary, string Secondary, string Description);

    private static readonly IReadOnlyList<DnsPreset> Presets =
    [
        new(
            "Cloudflare  — Privacy-First",
            "1.1.1.1",
            "1.0.0.1",
            "Cloudflare's public DNS. Consistently ranked fastest globally by DNSPerf. "
                + "Does not sell browsing data or use queries for ad targeting. Supports DNSSEC."
        ),
        new(
            "Cloudflare  — Malware Blocking",
            "1.1.1.2",
            "1.0.0.2",
            "Cloudflare DNS with automatic blocking of malware and phishing domains. "
                + "Blocked domains resolve as NXDOMAIN — no configuration required."
        ),
        new(
            "Cloudflare  — All Threats + Adult",
            "1.1.1.3",
            "1.0.0.3",
            "Cloudflare DNS that blocks malware, phishing AND adult content. "
                + "Suitable as a family-safe resolver. Uses the same fast infrastructure."
        ),
        new(
            "Google Public DNS",
            "8.8.8.8",
            "8.8.4.4",
            "Google's public DNS service. Highly reliable and widely used. "
                + "Supports DNSSEC validation, DoH, and DoT. Logs queries for abuse analysis."
        ),
        new(
            "Quad9  — Security + Privacy",
            "9.9.9.9",
            "149.112.112.112",
            "Non-profit resolver that blocks known malicious domains. Does not log IP addresses. "
                + "Operated from Switzerland under Swiss privacy law. DNSSEC validated."
        ),
        new(
            "OpenDNS Home",
            "208.67.222.222",
            "208.67.220.220",
            "Cisco-managed resolver with optional web-based parental controls. "
                + "Blocks phishing by default. Logs queries — see Cisco OpenDNS privacy policy."
        ),
        new(
            "AdGuard DNS  — Ad & Tracker Block",
            "94.140.14.14",
            "94.140.15.15",
            "Blocks ads and trackers at the DNS level across all devices. "
                + "Open-source commitment. Non-logging version available at 94.140.14.140."
        ),
        new(
            "Automatic (DHCP / ISP)",
            "",
            "",
            "Removes manually configured DNS and reverts to automatic (DHCP-assigned) servers. "
                + "Your router or ISP will provide DNS settings automatically."
        ),
    ];

    private readonly ListBox _adapterList = new()
    {
        Dock = DockStyle.Fill,
        IntegralHeight = false,
        HorizontalScrollbar = true,
    };

    private readonly ListView _presetList = new()
    {
        View = View.Details,
        FullRowSelect = true,
        MultiSelect = false,
        GridLines = true,
        Dock = DockStyle.Fill,
    };

    private readonly RichTextBox _descBox = new()
    {
        ReadOnly = true,
        Height = 80,
        Dock = DockStyle.Bottom,
        BackColor = SystemColors.Info,
        ForeColor = SystemColors.InfoText,
        BorderStyle = BorderStyle.None,
        ScrollBars = RichTextBoxScrollBars.Vertical,
    };

    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Top,
        Height = 26,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };
    private readonly Button _btnApply = new()
    {
        Text = "Apply →",
        Width = 105,
        Height = 28,
    };
    private readonly Button _btnRefreshAdapters = new()
    {
        Text = "Refresh Adapters",
        Width = 130,
        Height = 28,
    };
    private readonly Button _btnBenchmark = new()
    {
        Text = "⏱ Benchmark",
        Width = 106,
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

    // Custom DNS entry panel
    private readonly TextBox _customPrimary = new()
    {
        Width = 130,
        PlaceholderText = "Primary IP",
        Height = 24,
    };
    private readonly TextBox _customSecondary = new()
    {
        Width = 130,
        PlaceholderText = "Secondary IP",
        Height = 24,
    };
    private readonly Button _btnUseCustom = new()
    {
        Text = "Use Custom",
        Width = 96,
        Height = 26,
    };

    public DnsSwitcherDialog()
        : base("DNS Server Quick-Switch", new Size(900, 620), resizable: true)
    {
        MinimumSize = new Size(760, 500);
        EnableStandaloneMode();

        _presetList.Columns.Add("DNS Preset", 250);
        _presetList.Columns.Add("Primary", 130);
        _presetList.Columns.Add("Secondary", 130);
        ListViewColumnSorter.AttachTo(_presetList);
        foreach (DnsPreset p in Presets)
        {
            var lvi = new ListViewItem(p.Name);
            lvi.SubItems.Add(string.IsNullOrEmpty(p.Primary) ? "(auto)" : p.Primary);
            lvi.SubItems.Add(string.IsNullOrEmpty(p.Secondary) ? "—" : p.Secondary);
            _presetList.Items.Add(lvi);
        }
        _presetList.SelectedIndexChanged += OnPresetSelected;

        _btnApply.Click += async (_, _) => await ApplyAsync();
        _btnRefreshAdapters.Click += (_, _) => PopulateAdapters();
        _btnBenchmark.Click += async (_, _) => await BenchmarkAsync();
        _btnClose.Click += (_, _) => Close();
        _btnUseCustom.Click += async (_, _) => await ApplyCustomDnsAsync();

        _btnPanel.Controls.AddRange([_btnApply, _btnRefreshAdapters, _btnBenchmark, _btnClose]);

        if (!Elevation.IsAdmin())
            Controls.Add(CreateAdminBanner("⚠  Administrator rights required to change DNS settings."));

        PopulateAdapters();
        BuildLayout();
        LayoutButtons();

        _statusLabel.Text = "Select a network adapter and a DNS preset, then click Apply.";
    }

    private void PopulateAdapters()
    {
        _adapterList.Items.Clear();
        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (nic.NetworkInterfaceType is NetworkInterfaceType.Loopback or NetworkInterfaceType.Tunnel)
                continue;
            string statusIcon = nic.OperationalStatus == OperationalStatus.Up ? "● " : "○ ";
            _adapterList.Items.Add($"{statusIcon}{nic.Name}  [{nic.OperationalStatus}]");
        }
        if (_adapterList.Items.Count > 0)
            _adapterList.SelectedIndex = 0;
    }

    private void OnPresetSelected(object? sender, EventArgs e)
    {
        if (_presetList.SelectedItems.Count == 0)
            return;
        int idx = _presetList.SelectedItems[0].Index;
        _descBox.Text = Presets[idx].Description;
    }

    private async Task ApplyAsync()
    {
        if (_adapterList.SelectedItem is null)
        {
            _statusLabel.Text = "⚠  Please select a network adapter.";
            return;
        }
        if (_presetList.SelectedItems.Count == 0)
        {
            _statusLabel.Text = "⚠  Please select a DNS preset.";
            return;
        }

        // Extract adapter name (strip status prefix "● " or "○ " and status suffix)
        string adapterEntry = _adapterList.SelectedItem.ToString()!;
        // "● Ethernet  [Up]" → "Ethernet"
        string adapterName = adapterEntry[2..].Split('[')[0].Trim();

        DnsPreset preset = Presets[_presetList.SelectedItems[0].Index];
        _btnApply.Enabled = false;
        _statusLabel.Text = $"Applying {preset.Name} to \"{adapterName}\"…";

        try
        {
            await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(preset.Primary))
                {
                    // Revert to DHCP
                    RunNetsh($"interface ip set dns \"{adapterName}\" dhcp");
                    RunNetsh($"interface ipv6 set dns \"{adapterName}\" dhcp");
                }
                else
                {
                    RunNetsh($"interface ip set dns \"{adapterName}\" static {preset.Primary} primary");
                    if (!string.IsNullOrEmpty(preset.Secondary))
                        RunNetsh($"interface ip add dns \"{adapterName}\" {preset.Secondary} index=2");
                }
                // Flush DNS cache after change
                RunNetsh("flushdns", "ipconfig");
            });

            _statusLabel.Text =
                $"✓ DNS set to {preset.Name} ({(string.IsNullOrEmpty(preset.Primary) ? "DHCP" : preset.Primary)}) on \"{adapterName}\".";
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"✗ Failed: {ex.Message}";
        }
        finally
        {
            _btnApply.Enabled = true;
        }
    }

    private static void RunNetsh(string args, string cmd = "netsh")
    {
        var info = new ProcessStartInfo("cmd.exe", $"/c {cmd} {args}")
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        using Process? proc = Process.Start(info);
        proc?.WaitForExit();
    }

    private void BuildLayout()
    {
        var adapterLabel = new Label
        {
            Text = "Network Adapters:",
            Dock = DockStyle.Top,
            Height = 22,
            TextAlign = ContentAlignment.BottomLeft,
        };
        var presetLabel = new Label
        {
            Text = "DNS Presets:",
            Dock = DockStyle.Top,
            Height = 22,
            TextAlign = ContentAlignment.BottomLeft,
        };

        // Custom DNS bar (docked bottom, above button panel)
        var customPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 36,
            Padding = new Padding(4, 4, 4, 0),
        };
        var customLabel = new Label
        {
            Text = "Custom:",
            AutoSize = true,
            Location = new Point(4, 8),
        };
        _customPrimary.Location = new Point(60, 5);
        _customSecondary.Location = new Point(196, 5);
        _btnUseCustom.Location = new Point(332, 4);
        customPanel.Controls.AddRange(new Control[] { customLabel, _customPrimary, _customSecondary, _btnUseCustom });

        var leftPanel = new Panel { Dock = DockStyle.Left, Width = 260 };
        leftPanel.Controls.Add(_adapterList);
        leftPanel.Controls.Add(adapterLabel);

        var rightPanel = new Panel { Dock = DockStyle.Fill };
        rightPanel.Controls.Add(_descBox);
        rightPanel.Controls.Add(_presetList);
        rightPanel.Controls.Add(presetLabel);

        Controls.Add(rightPanel);
        Controls.Add(leftPanel);
        Controls.Add(_statusLabel);
        Controls.Add(customPanel);
        Controls.Add(_btnPanel);
    }

    private void LayoutButtons()
    {
        int x = 8;
        foreach (Button b in new[] { _btnApply, _btnRefreshAdapters, _btnBenchmark })
        {
            b.Location = new Point(x, 5);
            x += b.Width + 6;
        }
        _btnClose.Location = new Point(_btnPanel.Width - _btnClose.Width - 8, 5);
        _btnPanel.Resize += (_, _) => _btnClose.Location = new Point(_btnPanel.Width - _btnClose.Width - 8, 5);
    }

    private async Task BenchmarkAsync()
    {
        _btnBenchmark.Enabled = false;
        _statusLabel.Text = "Benchmarking DNS servers…";
        var results = new System.Text.StringBuilder();
        results.AppendLine("DNS Benchmark Results (avg ping over 3 probes):");

        await Task.Run(() =>
        {
            foreach (DnsPreset p in Presets)
            {
                if (string.IsNullOrEmpty(p.Primary))
                    continue;
                long total = 0;
                int success = 0;
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        using var ping = new Ping();
                        var reply = ping.Send(p.Primary, 1500);
                        if (reply.Status == IPStatus.Success)
                        {
                            total += reply.RoundtripTime;
                            success++;
                        }
                        Thread.Sleep(50);
                    }
                    catch { }
                }
                string latency = success > 0 ? $"{total / success} ms" : "timeout";
                results.AppendLine($"  {p.Name, 40}  {latency, 10}  ({p.Primary})");
            }
        });

        _descBox.Text = results.ToString();
        _statusLabel.Text = "Benchmark complete. Select a preset below to apply.";
        _btnBenchmark.Enabled = true;
    }

    private async Task ApplyCustomDnsAsync()
    {
        string primary = _customPrimary.Text.Trim();
        string secondary = _customSecondary.Text.Trim();
        if (string.IsNullOrEmpty(primary))
        {
            _statusLabel.Text = "⚠  Enter a primary DNS IP address.";
            return;
        }
        if (_adapterList.SelectedItem is null)
        {
            _statusLabel.Text = "⚠  Select a network adapter first.";
            return;
        }
        string adapterEntry = _adapterList.SelectedItem.ToString()!;
        string adapterName = adapterEntry[2..].Split('[')[0].Trim();
        _btnUseCustom.Enabled = false;
        _statusLabel.Text = $"Applying custom DNS {primary} to \"{adapterName}\"…";
        try
        {
            await Task.Run(() =>
            {
                RunNetsh($"interface ip set dns \"{adapterName}\" static {primary} primary");
                if (!string.IsNullOrEmpty(secondary))
                    RunNetsh($"interface ip add dns \"{adapterName}\" {secondary} index=2");
                RunNetsh("flushdns", "ipconfig");
            });
            _statusLabel.Text = $"✓ Custom DNS {primary} applied to \"{adapterName}\"";
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"✗ Failed: {ex.Message}";
        }
        finally
        {
            _btnUseCustom.Enabled = true;
        }
    }
}
