// RegiLattice.GUI — Forms/DnsOverHttpsDialog.cs
// Sprint 32: DNS-over-HTTPS (DoH) quick-setup wizard.

#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Microsoft.Win32;
using RegiLattice.Core;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Quick-setup dialog for configuring DNS-over-HTTPS on Windows 11+.
/// Windows 11 supports DoH natively through the network adapter settings in the registry.
/// This dialog edits the DoH template and encryption setting for the primary adapter's DNS.
/// Also provides guidance for Windows 10 (manual browser/adapter configuration).
/// </summary>
[SupportedOSPlatform("windows")]
internal sealed class DnsOverHttpsDialog : BaseDialog
{
    // ── DoH Provider definitions ──────────────────────────────────────────────
    private sealed record DoHProvider(string Name, string Template, string Primary, string Secondary, string Description);

    private static readonly IReadOnlyList<DoHProvider> Providers =
    [
        new DoHProvider(
            "Cloudflare (1.1.1.1)",
            "https://cloudflare-dns.com/dns-query",
            "1.1.1.1",
            "1.0.0.1",
            "Cloudflare's privacy-first DNS with no logging of personal data. Fast, global CDN-backed resolver. "
                + "Also provides 1.1.1.2 (malware blocking) and 1.1.1.3 (malware + adult content)."
        ),
        new DoHProvider(
            "Google (8.8.8.8)",
            "https://dns.google/dns-query",
            "8.8.8.8",
            "8.8.4.4",
            "Google Public DNS — widely used, reliable, fast. Google may log queries for service improvement. " + "Good performance worldwide."
        ),
        new DoHProvider(
            "Quad9 (9.9.9.9)",
            "https://dns.quad9.net/dns-query",
            "9.9.9.9",
            "149.112.112.112",
            "Quad9 blocks malicious domains using threat intelligence from IBM X-Force and other providers. "
                + "Based in Switzerland, strong privacy policy with no logging of IP addresses."
        ),
        new DoHProvider(
            "NextDNS",
            "https://dns.nextdns.io",
            "45.90.28.0",
            "45.90.30.0",
            "NextDNS is a customizable DNS service with parental controls, ad blocking, and detailed query logs. "
                + "Free tier for 300K queries/month. Requires a free account for full features."
        ),
        new DoHProvider(
            "AdGuard DNS",
            "https://dns.adguard.com/dns-query",
            "94.140.14.14",
            "94.140.15.15",
            "AdGuard DNS blocks ads, trackers, and malware at the DNS level. "
                + "No logging of your IP address or queries. Good for ad-free browsing without a browser extension."
        ),
    ];

    // ── Controls ──────────────────────────────────────────────────────────────
    private readonly ListView _providerList = new()
    {
        Dock = DockStyle.Fill,
        FullRowSelect = true,
        MultiSelect = false,
        View = View.Details,
        GridLines = true,
    };
    private readonly RichTextBox _descBox = new()
    {
        Dock = DockStyle.Bottom,
        Height = 90,
        ReadOnly = true,
        BorderStyle = BorderStyle.FixedSingle,
        Font = new Font(SystemFonts.DefaultFont?.FontFamily ?? new FontFamily("Segoe UI"), 8.5f),
    };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };
    private readonly FlowLayoutPanel _btnPanel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 42,
        Padding = new Padding(6, 6, 6, 4),
        FlowDirection = FlowDirection.LeftToRight,
    };
    private readonly Button _btnApply = new() { Text = "Configure Selected DoH", Width = 160 };
    private readonly Button _btnDisable = new() { Text = "Disable DoH", Width = 100 };
    private readonly Button _btnClose = new() { Text = "Close", Width = 80 };

    private readonly Label _infoLabel = new()
    {
        Dock = DockStyle.Top,
        Height = 50,
        Padding = new Padding(8, 6, 8, 0),
        Text =
            "Windows 11 supports DNS-over-HTTPS natively. Select a provider below and click Configure. "
            + "This writes the DoH template to the registry. Open Settings → Network → DNS to verify.",
    };

    // ── Construction ──────────────────────────────────────────────────────────
    internal DnsOverHttpsDialog()
        : base("DNS-over-HTTPS Quick Setup", new Size(740, 520), resizable: true)
    {
        BuildLayout();
        PopulateProviders();

        _btnApply.Click += OnApply;
        _btnDisable.Click += OnDisable;
        _btnClose.Click += (_, _) => Close();
        _providerList.SelectedIndexChanged += OnSelectionChanged;
        _providerList.DoubleClick += OnApply;

        if (!Elevation.IsAdmin())
            Controls.Add(CreateAdminBanner("⚠  Administrator rights required to configure DNS settings."));
    }

    // ── Layout ────────────────────────────────────────────────────────────────
    private void BuildLayout()
    {
        _providerList.Columns.AddRange([
            new ColumnHeader { Text = "Provider", Width = 200 },
            new ColumnHeader { Text = "Primary DNS", Width = 120 },
            new ColumnHeader { Text = "Secondary DNS", Width = 120 },
            new ColumnHeader { Text = "Status", Width = 140 },
        ]);
        ListViewColumnSorter.AttachTo(_providerList);

        _btnPanel.Controls.AddRange([_btnApply, _btnDisable, _btnClose]);
        Controls.AddRange([_infoLabel, _providerList, _descBox, _statusLabel, _btnPanel]);
    }

    // ── Populate ──────────────────────────────────────────────────────────────
    private void PopulateProviders()
    {
        string? activeTemplate = ReadCurrentDoHTemplate();

        _providerList.BeginUpdate();
        _providerList.Items.Clear();

        foreach (var p in Providers)
        {
            bool isActive =
                activeTemplate != null
                && activeTemplate.Contains(p.Template.Replace("https://", "").Split('/')[0], StringComparison.OrdinalIgnoreCase);
            var lvi = new ListViewItem(p.Name) { Tag = p };
            lvi.SubItems.Add(p.Primary);
            lvi.SubItems.Add(p.Secondary);
            lvi.SubItems.Add(isActive ? "✓ Active" : "—");
            if (isActive)
            {
                lvi.Font = new Font(
                    SystemFonts.DefaultFont ?? SystemFonts.MessageBoxFont ?? new Font(FontFamily.GenericSansSerif, 9f),
                    FontStyle.Bold
                );
                lvi.ForeColor = Color.ForestGreen;
            }
            _providerList.Items.Add(lvi);
        }

        _providerList.EndUpdate();
        _statusLabel.Text = activeTemplate is null
            ? "No DoH template currently configured, using standard DNS."
            : $"Active DoH template: {activeTemplate}";
    }

    // ── Registry Helpers ──────────────────────────────────────────────────────
    private const string DnsClientPath = @"SYSTEM\CurrentControlSet\Services\Dnscache\Parameters";

    private static string? ReadCurrentDoHTemplate()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(DnsClientPath);
            return key?.GetValue("DohTemplate") as string;
        }
        catch
        {
            return null;
        }
    }

    // ── Apply / Disable ───────────────────────────────────────────────────────
    private void OnApply(object? sender, EventArgs e)
    {
        if (_providerList.SelectedItems.Count == 0)
        {
            MessageBox.Show("Please select a DNS provider.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!Elevation.IsAdmin())
        {
            MessageBox.Show(
                "Administrator rights required to configure DNS settings.",
                "Access Denied",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
            return;
        }

        if (_providerList.SelectedItems[0].Tag is not DoHProvider provider)
            return;

        try
        {
            using var key = Registry.LocalMachine.CreateSubKey(DnsClientPath, writable: true);

            // Set DoH template and enable automatic upgrade
            key.SetValue("DohTemplate", provider.Template, RegistryValueKind.String);
            key.SetValue("EnableAutoDoh", 2, RegistryValueKind.DWord); // 2 = automatic

            _statusLabel.Text = $"✓ Configured DoH: {provider.Name} ({provider.Template})";
            PopulateProviders(); // refresh status
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"Error: {ex.Message}";
        }
    }

    private void OnDisable(object? sender, EventArgs e)
    {
        if (!Elevation.IsAdmin())
        {
            MessageBox.Show("Administrator rights required.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(DnsClientPath, writable: true);
            if (key != null)
            {
                key.DeleteValue("DohTemplate", throwOnMissingValue: false);
                key.DeleteValue("EnableAutoDoh", throwOnMissingValue: false);
            }
            _statusLabel.Text = "DoH disabled — reverted to standard DNS.";
            PopulateProviders();
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"Error: {ex.Message}";
        }
    }

    private void OnSelectionChanged(object? sender, EventArgs e)
    {
        if (_providerList.SelectedItems.Count > 0 && _providerList.SelectedItems[0].Tag is DoHProvider p)
            _descBox.Text = p.Description;
    }
}
