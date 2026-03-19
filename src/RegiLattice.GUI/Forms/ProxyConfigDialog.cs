// RegiLattice.GUI — Forms/ProxyConfigDialog.cs
// System proxy configuration: WinINet (per-user) and WinHTTP (system-wide).
#nullable enable

using System.Diagnostics;
using Microsoft.Win32;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Proxy configuration dialog.
/// Manages Internet Explorer / WinINet proxy settings (HKCU) and
/// WinHTTP proxy settings (requires elevation) via netsh winhttp.
/// </summary>
internal sealed class ProxyConfigDialog : BaseDialog
{
    private const string IESettingsKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings";

    private readonly CheckBox _chkEnable = new() { Text = "Use a proxy server", AutoSize = true };
    private readonly TextBox _txtServer = new()
    {
        Width = 260,
        PlaceholderText = "host:port",
        Height = 24,
    };
    private readonly TextBox _txtOverride = new()
    {
        Width = 340,
        PlaceholderText = "Bypass list (semicolon-separated, e.g. *.local;10.*;192.168.*)",
        Height = 24,
    };
    private readonly CheckBox _chkBypassLocal = new() { Text = "Bypass proxy server for local addresses", AutoSize = true };
    private readonly Label _lblStatus = new() { AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Regular) };

    private readonly GroupBox _grpWinHttp = new()
    {
        Text = "WinHTTP Proxy (system-wide, requires admin)",
        Height = 80,
        Dock = DockStyle.Fill,
    };
    private readonly RadioButton _rdoWinHttpDirect = new()
    {
        Text = "Direct connection (no proxy)",
        AutoSize = true,
        Location = new Point(16, 22),
    };
    private readonly RadioButton _rdoWinHttpFromIE = new()
    {
        Text = "Import settings from Internet Explorer",
        AutoSize = true,
        Location = new Point(16, 44),
    };

    private readonly Button _btnApplyUser = new()
    {
        Text = "Apply User Proxy",
        Width = 130,
        Height = 28,
    };
    private readonly Button _btnApplyWinHttp = new()
    {
        Text = "Apply WinHTTP",
        Width = 120,
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

    public ProxyConfigDialog()
        : base("Proxy Configuration", new Size(540, 480), resizable: false)
    {
        EnableStandaloneMode();
        BuildLayout();
        LoadSettings();
        WireEvents();
    }

    private void BuildLayout()
    {
        var outerPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 5,
            ColumnCount = 1,
            Padding = new Padding(14, 10, 14, 8),
        };

        // Section: WinINet / User proxy
        var grpUser = new GroupBox
        {
            Text = "Internet Explorer / WinINet Proxy (per-user)",
            Dock = DockStyle.Fill,
            Height = 180,
        };
        var userContent = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            Padding = new Padding(8, 4, 0, 0),
            WrapContents = false,
            AutoScroll = false,
        };

        var rowServer = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.LeftToRight,
            AutoSize = true,
            WrapContents = false,
        };
        rowServer.Controls.Add(
            new Label
            {
                Text = "Proxy server:",
                AutoSize = true,
                Margin = new Padding(0, 4, 8, 0),
            }
        );
        rowServer.Controls.Add(_txtServer);

        var rowOverride = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.LeftToRight,
            AutoSize = true,
            WrapContents = false,
        };
        rowOverride.Controls.Add(
            new Label
            {
                Text = "Bypass list:",
                AutoSize = true,
                Margin = new Padding(0, 4, 8, 0),
            }
        );
        rowOverride.Controls.Add(_txtOverride);

        userContent.Controls.Add(_chkEnable);
        userContent.Controls.Add(rowServer);
        userContent.Controls.Add(rowOverride);
        userContent.Controls.Add(_chkBypassLocal);
        grpUser.Controls.Add(userContent);

        // WinHTTP section
        _grpWinHttp.Controls.Add(_rdoWinHttpDirect);
        _grpWinHttp.Controls.Add(_rdoWinHttpFromIE);
        _rdoWinHttpDirect.Checked = true;

        // Status bar
        var statusPanel = new Panel { Dock = DockStyle.Fill, Height = 26 };
        statusPanel.Controls.Add(_lblStatus);
        _lblStatus.Location = new Point(4, 4);

        // Button row
        var btnRow = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.LeftToRight,
            Dock = DockStyle.Bottom,
            Height = 38,
            WrapContents = false,
        };
        btnRow.Controls.Add(_btnApplyUser);
        btnRow.Controls.Add(_btnApplyWinHttp);
        btnRow.Controls.Add(_btnRefresh);
        btnRow.Controls.Add(_btnClose);

        outerPanel.Controls.Add(grpUser, 0, 0);
        outerPanel.Controls.Add(_grpWinHttp, 0, 1);
        outerPanel.Controls.Add(statusPanel, 0, 2);
        outerPanel.Controls.Add(btnRow, 0, 3);

        Controls.Add(outerPanel);
    }

    private void WireEvents()
    {
        _chkEnable.CheckedChanged += (_, _) =>
        {
            _txtServer.Enabled = _chkEnable.Checked;
            _txtOverride.Enabled = _chkEnable.Checked;
            _chkBypassLocal.Enabled = _chkEnable.Checked;
        };

        _btnApplyUser.Click += (_, _) => ApplyUserProxy();
        _btnApplyWinHttp.Click += async (_, _) => await ApplyWinHttpAsync();
        _btnRefresh.Click += (_, _) => LoadSettings();
        _btnClose.Click += (_, _) => Close();
    }

    private void LoadSettings()
    {
        try
        {
            int enable = (int?)Registry.GetValue(IESettingsKey, "ProxyEnable", 0) ?? 0;
            string server = (string?)Registry.GetValue(IESettingsKey, "ProxyServer", "") ?? "";
            string bypass = (string?)Registry.GetValue(IESettingsKey, "ProxyOverride", "") ?? "";

            _chkEnable.Checked = enable == 1;
            _txtServer.Text = server;

            string clean = bypass.Replace("<local>", "").Trim(';');
            _txtOverride.Text = clean;
            _chkBypassLocal.Checked = bypass.Contains("<local>", StringComparison.OrdinalIgnoreCase);

            _txtServer.Enabled = _chkEnable.Checked;
            _txtOverride.Enabled = _chkEnable.Checked;
            _chkBypassLocal.Enabled = _chkEnable.Checked;

            _lblStatus.Text = enable == 1 ? $"Proxy active: {server}" : "No proxy configured (direct connection).";
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"Error reading settings: {ex.Message}";
        }
    }

    private void ApplyUserProxy()
    {
        try
        {
            Registry.SetValue(IESettingsKey, "ProxyEnable", _chkEnable.Checked ? 1 : 0, RegistryValueKind.DWord);
            Registry.SetValue(IESettingsKey, "ProxyServer", _txtServer.Text.Trim(), RegistryValueKind.String);

            string bypass = _txtOverride.Text.Trim();
            if (_chkBypassLocal.Checked)
                bypass = string.IsNullOrEmpty(bypass) ? "<local>" : bypass + ";<local>";
            Registry.SetValue(IESettingsKey, "ProxyOverride", bypass, RegistryValueKind.String);

            // Notify WinINet that settings changed
            SendNotifyMessage(new IntPtr(0xFFFF), 0x001A, UIntPtr.Zero, IntPtr.Zero); // WM_SETTINGCHANGE
            _lblStatus.Text = "✓ User proxy settings applied.";
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"✗ Failed: {ex.Message}";
        }
    }

    private async Task ApplyWinHttpAsync()
    {
        _btnApplyWinHttp.Enabled = false;
        _lblStatus.Text = "Applying WinHTTP proxy…";
        try
        {
            string args = _rdoWinHttpFromIE.Checked ? "winhttp import proxy source=ie" : "winhttp reset proxy";
            await Task.Run(() => RunNetsh(args));
            _lblStatus.Text = "✓ WinHTTP proxy updated.";
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"✗ WinHTTP failed: {ex.Message}";
        }
        finally
        {
            _btnApplyWinHttp.Enabled = true;
        }
    }

    private static void RunNetsh(string args)
    {
        var info = new ProcessStartInfo("netsh", args)
        {
            UseShellExecute = true, // allows UAC elevation prompt for winhttp
            CreateNoWindow = true,
            Verb = "runas", // request elevation for WinHTTP operations
        };
        using Process? proc = Process.Start(info);
        proc?.WaitForExit();
    }

    [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = false)]
    private static extern bool SendNotifyMessage(IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam);
}
