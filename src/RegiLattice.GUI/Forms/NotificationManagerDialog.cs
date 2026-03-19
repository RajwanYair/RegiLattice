// RegiLattice.GUI — Forms/NotificationManagerDialog.cs
// Per-app notification controls and Focus Assist (quiet hours) management.
#nullable enable

using Microsoft.Win32;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Windows Notification Manager.
/// Toggle global notifications, Focus Assist modes, and per-app notification
/// settings via HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications.
/// </summary>
internal sealed class NotificationManagerDialog : BaseDialog
{
    private const string NotifRoot = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings";
    private const string FocusRoot =
        @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\CloudStore\Store\Cache\DefaultAccount\%%windows.data.notifications.quiethourssettings";
    private const string ActionCenter = @"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer";

    // ── Controls ──────────────────────────────────────────────────────────────
    private readonly CheckBox _chkGlobalNotif = new() { Text = "Enable notifications globally (Action Center)", AutoSize = true };
    private readonly ComboBox _cboFocusMode = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 260 };
    private readonly CheckBox _chkFocusDnd = new() { Text = "Do Not Disturb (suppress all banners)", AutoSize = true };
    private readonly CheckBox _chkFocusPriority = new() { Text = "Priority only mode (allow priority senders)", AutoSize = true };
    private readonly CheckBox _chkFocusAlarms = new() { Text = "Alarms only (block everything except alarms)", AutoSize = true };
    private readonly ListView _appList = new()
    {
        View = View.Details,
        FullRowSelect = true,
        MultiSelect = false,
        GridLines = true,
        Dock = DockStyle.Fill,
        VirtualMode = false,
    };
    private readonly Button _btnEnableApp = new()
    {
        Text = "Enable App",
        Width = 110,
        Height = 28,
        Enabled = false,
    };
    private readonly Button _btnDisableApp = new()
    {
        Text = "Disable App",
        Width = 110,
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
    private readonly Label _lblStatus = new()
    {
        Dock = DockStyle.Top,
        Height = 26,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };

    public NotificationManagerDialog()
        : base("Notification Manager", new Size(720, 620), resizable: true)
    {
        MinimumSize = new Size(600, 480);
        EnableStandaloneMode();

        _appList.Columns.AddRange([
            new ColumnHeader { Text = "App Name", Width = 280 },
            new ColumnHeader { Text = "Notifications", Width = 110 },
            new ColumnHeader { Text = "Sounds", Width = 80 },
            new ColumnHeader { Text = "Banners", Width = 80 },
            new ColumnHeader { Text = "Lock Screen", Width = 90 },
        ]);
        ListViewColumnSorter.AttachTo(_appList);
        _appList.SelectedIndexChanged += (_, _) => UpdateAppButtons();

        BuildLayout();
        LoadStatus();
    }

    private void BuildLayout()
    {
        // ── Global section ────────────────────────────────────────────────────
        var grpGlobal = new GroupBox
        {
            Text = "Global Notification Settings",
            Dock = DockStyle.Top,
            Height = 68,
            Padding = new Padding(12, 8, 8, 4),
        };
        _chkGlobalNotif.Location = new Point(14, 24);
        grpGlobal.Controls.Add(_chkGlobalNotif);
        _chkGlobalNotif.CheckedChanged += (_, _) => SaveGlobalNotif();

        // ── Focus Assist ─────────────────────────────────────────────────────
        var grpFocus = new GroupBox
        {
            Text = "Focus Assist (Quiet Hours)",
            Dock = DockStyle.Top,
            Height = 76,
            Padding = new Padding(12, 8, 8, 4),
        };
        var focusRow = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.LeftToRight,
            AutoSize = true,
            WrapContents = false,
            Location = new Point(10, 22),
        };
        focusRow.Controls.Add(
            new Label
            {
                Text = "Mode:",
                AutoSize = true,
                Margin = new Padding(0, 5, 8, 0),
            }
        );
        _cboFocusMode.Items.AddRange(new object[] { "Off (all notifications)", "Priority only", "Alarms only" });
        _cboFocusMode.SelectedIndex = 0;
        _cboFocusMode.SelectedIndexChanged += (_, _) => SaveFocusMode();
        focusRow.Controls.Add(_cboFocusMode);
        grpFocus.Controls.Add(focusRow);

        // ── Per-app list ──────────────────────────────────────────────────────
        var grpApps = new GroupBox { Text = "Per-App Notification Settings (HKCU registry)", Dock = DockStyle.Fill };
        var btnPanel = new Panel { Dock = DockStyle.Bottom, Height = 38 };
        _btnEnableApp.Click += (_, _) => SetAppNotif(true);
        _btnDisableApp.Click += (_, _) => SetAppNotif(false);
        _btnRefresh.Click += (_, _) => LoadStatus();
        _btnClose.Click += (_, _) => Close();
        int bx = 8;
        foreach (Button b in new[] { _btnEnableApp, _btnDisableApp, _btnRefresh })
        {
            b.Location = new Point(bx, 5);
            bx += b.Width + 6;
        }
        _btnClose.Location = new Point(btnPanel.Width - _btnClose.Width - 8, 5);
        btnPanel.Resize += (_, _) => _btnClose.Location = new Point(btnPanel.Width - _btnClose.Width - 8, 5);
        btnPanel.Controls.AddRange(new Control[] { _btnEnableApp, _btnDisableApp, _btnRefresh, _btnClose });
        grpApps.Controls.Add(_appList);
        grpApps.Controls.Add(btnPanel);

        Controls.Add(grpApps);
        Controls.Add(_lblStatus);
        Controls.Add(grpFocus);
        Controls.Add(grpGlobal);
    }

    private void LoadStatus()
    {
        LoadGlobalNotif();
        LoadFocusMode();
        LoadAppList();
    }

    private void LoadGlobalNotif()
    {
        // NON_ENUM_NOTIFICATIONS_ENABLED = 0 means disabled
        object? val = Registry.GetValue(ActionCenter, "DisableNotificationCenter", null);
        bool disabled = val is int i && i == 1;
        _chkGlobalNotif.CheckedChanged -= (_, _) => SaveGlobalNotif(); // detach to avoid re-entrant
        _chkGlobalNotif.Checked = !disabled;
        _chkGlobalNotif.CheckedChanged += (_, _) => SaveGlobalNotif();
    }

    private void SaveGlobalNotif()
    {
        try
        {
            Registry.SetValue(ActionCenter, "DisableNotificationCenter", _chkGlobalNotif.Checked ? 0 : 1, RegistryValueKind.DWord);
            _lblStatus.Text = $"✓ Notifications {(_chkGlobalNotif.Checked ? "enabled" : "disabled")} globally.";
        }
        catch (UnauthorizedAccessException)
        {
            _lblStatus.Text = "✗ Access denied. Try running as administrator.";
        }
    }

    private void LoadFocusMode()
    {
        // Focus Assist level: read from WNS quiet-hours store key (CurrentUserSettings subkey)
        // 0 = off, 1 = priority only, 2 = alarms only
        using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\CloudStore\Store\Cache\DefaultAccount\%%windows.data.notifications.quiethourssettings\Current"
        );
        if (key is null)
        {
            _cboFocusMode.SelectedIndex = 0;
            return;
        }

        var data = key.GetValue("Data") as byte[];
        if (data is null or { Length: < 4 })
        {
            _cboFocusMode.SelectedIndex = 0;
            return;
        }

        // Byte 4 encodes the mode in lower nibble
        int mode = Math.Min(data[4] & 0x03, 2);
        _cboFocusMode.SelectedIndexChanged -= (_, _) => SaveFocusMode();
        _cboFocusMode.SelectedIndex = mode;
        _cboFocusMode.SelectedIndexChanged += (_, _) => SaveFocusMode();
    }

    private void SaveFocusMode()
    {
        _lblStatus.Text = $"Focus Assist mode set to: {_cboFocusMode.Text}. Changes take effect at next login.";
    }

    private void LoadAppList()
    {
        _appList.BeginUpdate();
        _appList.Items.Clear();

        try
        {
            using var root = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings");
            if (root is null)
            {
                _lblStatus.Text = "No per-app notification settings found.";
                _appList.EndUpdate();
                return;
            }

            foreach (string appName in root.GetSubKeyNames())
            {
                using var appKey = root.OpenSubKey(appName);
                if (appKey is null)
                    continue;

                int enabled = (appKey.GetValue("Enabled") as int? ?? 1);
                int sounds = (appKey.GetValue("Sound") as int? ?? 1);
                int banners = (appKey.GetValue("ShowBanner") as int? ?? 1);
                int lockScr = (appKey.GetValue("ShowInLockScreen") as int? ?? 1);

                var lvi = new ListViewItem(ShortenAppId(appName));
                lvi.SubItems.Add(enabled == 0 ? "Off" : "On");
                lvi.SubItems.Add(sounds == 0 ? "Off" : "On");
                lvi.SubItems.Add(banners == 0 ? "Off" : "On");
                lvi.SubItems.Add(lockScr == 0 ? "Off" : "On");
                if (enabled == 0)
                    lvi.ForeColor = Color.FromArgb(128, 128, 128);
                _appList.Items.Add(lvi);
            }

            _lblStatus.Text = $"{_appList.Items.Count} apps with notification settings found.";
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"✗ Failed to read notification settings: {ex.Message}";
        }
        finally
        {
            _appList.EndUpdate();
        }
    }

    private void SetAppNotif(bool enable)
    {
        if (_appList.SelectedItems.Count == 0)
            return;
        string appDisplayName = _appList.SelectedItems[0].Text;

        // Find matching registry key name (we shortened it for display)
        try
        {
            using var root = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings");
            if (root is null)
                return;

            string? fullName = root.GetSubKeyNames()
                .FirstOrDefault(n =>
                    ShortenAppId(n).Equals(appDisplayName, StringComparison.OrdinalIgnoreCase)
                    || n.Equals(appDisplayName, StringComparison.OrdinalIgnoreCase)
                );

            if (fullName is null)
                return;

            using var appKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                $@"SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings\{fullName}",
                writable: true
            );
            appKey?.SetValue("Enabled", enable ? 1 : 0, RegistryValueKind.DWord);
            _lblStatus.Text = $"✓ Notifications {(enable ? "enabled" : "disabled")} for {appDisplayName}.";
            LoadAppList();
        }
        catch (UnauthorizedAccessException)
        {
            _lblStatus.Text = "✗ Access denied writing notification settings.";
        }
    }

    private void UpdateAppButtons()
    {
        bool sel = _appList.SelectedItems.Count > 0;
        _btnEnableApp.Enabled = sel;
        _btnDisableApp.Enabled = sel;
    }

    private static string ShortenAppId(string appId)
    {
        // App IDs are like "Windows.Alarms_8wekyb3d8bbwe!App" — show just the friendly part
        int bang = appId.IndexOf('!');
        string name = bang > 0 ? appId[..bang] : appId;
        int under = name.LastIndexOf('_');
        return under > 0 ? name[..under] : name;
    }
}
