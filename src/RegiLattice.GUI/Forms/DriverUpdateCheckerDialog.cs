// RegiLattice.GUI — Forms/DriverUpdateCheckerDialog.cs
// Lists installed drivers via WMI Win32_PnPSignedDriver and links to Windows Update.
#nullable enable

using System.Diagnostics;
using System.Management;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Driver Update Checker.
/// Lists all signed drivers via WMI Win32_PnPSignedDriver, shows driver
/// name, version, install date, and manufacturer. Provides buttons to
/// open Device Manager and Windows Update for updating out-of-date drivers.
/// </summary>
internal sealed class DriverUpdateCheckerDialog : BaseDialog
{
    private sealed record DriverInfo(
        string DeviceName,
        string DriverName,
        string Manufacturer,
        string Version,
        string Date,
        string DeviceClass,
        string Status
    );

    private readonly ListView _list = new()
    {
        View = View.Details,
        FullRowSelect = true,
        MultiSelect = false,
        GridLines = true,
        Dock = DockStyle.Fill,
        VirtualMode = false,
    };
    private readonly TextBox _searchBox = new()
    {
        PlaceholderText = "Filter drivers…",
        Width = 240,
        Height = 24,
    };
    private readonly Button _btnRefresh = new()
    {
        Text = "⟳ Refresh",
        Width = 90,
        Height = 28,
    };
    private readonly Button _btnDevMgr = new()
    {
        Text = "Device Manager",
        Width = 130,
        Height = 28,
    };
    private readonly Button _btnWinUpdate = new()
    {
        Text = "Windows Update",
        Width = 120,
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
    private readonly Panel _searchPanel = new() { Dock = DockStyle.Top, Height = 34 };

    private List<DriverInfo> _allDrivers = [];

    public DriverUpdateCheckerDialog()
        : base("Driver Update Checker", new Size(1000, 660), resizable: true)
    {
        MinimumSize = new Size(760, 500);
        EnableStandaloneMode();

        _list.Columns.AddRange([
            new ColumnHeader { Text = "Device Name", Width = 260 },
            new ColumnHeader { Text = "Driver", Width = 200 },
            new ColumnHeader { Text = "Manufacturer", Width = 160 },
            new ColumnHeader { Text = "Version", Width = 120 },
            new ColumnHeader { Text = "Install Date", Width = 110 },
            new ColumnHeader { Text = "Class", Width = 120 },
        ]);

        _searchBox.TextChanged += (_, _) => ApplyFilter();
        _searchBox.Location = new Point(50, 5);
        _searchPanel.Controls.Add(
            new Label
            {
                Text = "Filter:",
                AutoSize = true,
                Location = new Point(6, 8),
            }
        );
        _searchPanel.Controls.Add(_searchBox);

        var btnPanel = new Panel { Dock = DockStyle.Bottom, Height = 38 };
        int bx = 8;
        foreach (Button b in new[] { _btnRefresh, _btnDevMgr, _btnWinUpdate })
        {
            b.Location = new Point(bx, 5);
            bx += b.Width + 6;
        }
        _btnClose.Location = new Point(btnPanel.Width - _btnClose.Width - 8, 5);
        btnPanel.Resize += (_, _) => _btnClose.Location = new Point(btnPanel.Width - _btnClose.Width - 8, 5);

        _btnRefresh.Click += async (_, _) => await LoadDriversAsync();
        _btnDevMgr.Click += (_, _) => Process.Start(new ProcessStartInfo("devmgmt.msc") { UseShellExecute = true });
        _btnWinUpdate.Click += (_, _) => Process.Start(new ProcessStartInfo("ms-settings:windowsupdate") { UseShellExecute = true });
        _btnClose.Click += (_, _) => Close();

        btnPanel.Controls.AddRange(new Control[] { _btnRefresh, _btnDevMgr, _btnWinUpdate, _btnClose });

        Controls.Add(_list);
        Controls.Add(btnPanel);
        Controls.Add(_searchPanel);
        Controls.Add(_lblStatus);

        _ = LoadDriversAsync();
    }

    private async Task LoadDriversAsync()
    {
        _btnRefresh.Enabled = false;
        _lblStatus.Text = "Loading driver list via WMI…";
        _list.Items.Clear();

        _allDrivers = await Task.Run(QueryDrivers);
        ApplyFilter();

        _lblStatus.Text = $"{_allDrivers.Count} drivers found.";
        _btnRefresh.Enabled = true;
    }

    private static List<DriverInfo> QueryDrivers()
    {
        var drivers = new List<DriverInfo>();
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPSignedDriver WHERE DeviceName IS NOT NULL");
            foreach (ManagementObject obj in searcher.Get())
            {
                string name = obj["DeviceName"]?.ToString() ?? "";
                string driver = obj["DriverName"]?.ToString() ?? "";
                string mfr = obj["Manufacturer"]?.ToString() ?? "";
                string version = obj["DriverVersion"]?.ToString() ?? "";
                string dateWmi = obj["DriverDate"]?.ToString() ?? "";
                string cls = obj["DeviceClass"]?.ToString() ?? "";
                string status = obj["Status"]?.ToString() ?? "OK";

                // WMI date format: "20230412000000.000000+000"
                string date = FormatWmiDate(dateWmi);

                if (!string.IsNullOrWhiteSpace(name))
                    drivers.Add(new DriverInfo(name, driver, mfr, version, date, cls, status));
            }
        }
        catch
        {
            // WMI unavailable — return empty list
        }

        return drivers.OrderBy(d => d.DeviceClass).ThenBy(d => d.DeviceName).ToList();
    }

    private void ApplyFilter()
    {
        string q = _searchBox.Text.Trim();
        IEnumerable<DriverInfo> src = string.IsNullOrEmpty(q)
            ? _allDrivers
            : _allDrivers.Where(d =>
                d.DeviceName.Contains(q, StringComparison.OrdinalIgnoreCase)
                || d.Manufacturer.Contains(q, StringComparison.OrdinalIgnoreCase)
                || d.DeviceClass.Contains(q, StringComparison.OrdinalIgnoreCase)
                || d.Version.Contains(q, StringComparison.OrdinalIgnoreCase)
            );

        _list.BeginUpdate();
        _list.Items.Clear();
        foreach (DriverInfo d in src)
        {
            var lvi = new ListViewItem(d.DeviceName);
            lvi.SubItems.Add(d.DriverName);
            lvi.SubItems.Add(d.Manufacturer);
            lvi.SubItems.Add(d.Version);
            lvi.SubItems.Add(d.Date);
            lvi.SubItems.Add(d.DeviceClass);
            if (d.Status != "OK")
                lvi.ForeColor = Color.FromArgb(180, 60, 60);
            _list.Items.Add(lvi);
        }
        _list.EndUpdate();
    }

    private static string FormatWmiDate(string wmiDate)
    {
        if (string.IsNullOrEmpty(wmiDate) || wmiDate.Length < 8)
            return "";
        if (
            DateTime.TryParseExact(
                wmiDate[..8],
                "yyyyMMdd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime dt
            )
        )
            return dt.ToString("yyyy-MM-dd");
        return wmiDate[..8];
    }
}
