// RegiLattice.GUI — Forms/BootTimeAnalyzerDialog.cs
// Boot time history and startup slowdown analysis from Windows event log.
#nullable enable

using System.Diagnostics.Eventing.Reader;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Boot Time Analyzer — reads Microsoft-Windows-Diagnostics-Performance/Operational.
/// Event ID 100 = successful boot (MainPathBootTime attribute).
/// Event ID 101+ = degradation: a process slowed startup by X ms.
/// Displays last 10 boot durations and top slow-start processes.
/// </summary>
internal sealed class BootTimeAnalyzerDialog : BaseDialog
{
    private sealed record BootRecord(DateTime Time, int TotalMs, int PostBootMs);

    private sealed record DegradedProcess(string Name, int DelayMs, DateTime BootTime);

    private readonly ListView _bootList = new()
    {
        View = View.Details,
        FullRowSelect = true,
        MultiSelect = false,
        GridLines = true,
        Dock = DockStyle.Fill,
        HeaderStyle = ColumnHeaderStyle.Nonclickable,
    };

    private readonly ListView _slowList = new()
    {
        View = View.Details,
        FullRowSelect = true,
        MultiSelect = false,
        GridLines = true,
        Dock = DockStyle.Fill,
        HeaderStyle = ColumnHeaderStyle.Nonclickable,
    };

    private readonly Label _lblSummary = new()
    {
        Dock = DockStyle.Top,
        Height = 28,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(8, 0, 0, 0),
        Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
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

    public BootTimeAnalyzerDialog()
        : base("Boot Time Analyzer", new Size(920, 640), resizable: true)
    {
        MinimumSize = new Size(760, 480);
        EnableStandaloneMode();

        _bootList.Columns.AddRange([
            new ColumnHeader { Text = "Boot Date / Time", Width = 180 },
            new ColumnHeader { Text = "Total Boot (s)", Width = 130 },
            new ColumnHeader { Text = "Post-Boot (s)", Width = 130 },
        ]);

        _slowList.Columns.AddRange([
            new ColumnHeader { Text = "Process / Service", Width = 260 },
            new ColumnHeader { Text = "Delay (ms)", Width = 110 },
            new ColumnHeader { Text = "On Boot", Width = 180 },
        ]);

        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Horizontal,
            SplitterDistance = 220,
        };

        var topGroup = new GroupBox { Text = "Boot History (last 20 boots)", Dock = DockStyle.Fill };
        var botGroup = new GroupBox { Text = "Startup Degradation Events (top slowdowns)", Dock = DockStyle.Fill };
        topGroup.Controls.Add(_bootList);
        botGroup.Controls.Add(_slowList);
        split.Panel1.Controls.Add(topGroup);
        split.Panel2.Controls.Add(botGroup);

        _btnRefresh.Location = new Point(8, 5);
        _btnRefresh.Click += async (_, _) => await LoadAsync();
        _btnClose.Location = new Point(ClientSize.Width - _btnClose.Width - 8, 5);
        _btnPanel.Resize += (_, _) => _btnClose.Location = new Point(_btnPanel.Width - _btnClose.Width - 8, 5);
        _btnClose.Click += (_, _) => Close();
        _btnPanel.Controls.AddRange([_btnRefresh, _btnClose]);

        Controls.Add(split);
        Controls.Add(_lblSummary);
        Controls.Add(_btnPanel);

        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        _btnRefresh.Enabled = false;
        _lblSummary.Text = "Reading Windows event log…";
        try
        {
            (List<BootRecord> boots, List<DegradedProcess> slow) = await Task.Run(ReadEventLog);

            PopulateBootList(boots);
            PopulateSlowList(slow);

            if (boots.Count > 0)
            {
                double avg = boots.Average(b => b.TotalMs / 1000.0);
                int last = boots[^1].TotalMs / 1000;
                _lblSummary.Text = $"Last boot: {last}s  ·  Average (last {boots.Count}): {avg:F1}s  ·  Slow processes: {slow.Count}";
            }
            else
            {
                _lblSummary.Text = "No boot records found. (Event log may require elevation.)";
            }
        }
        catch (UnauthorizedAccessException)
        {
            _lblSummary.Text = "Access denied — run as administrator to read the performance event log.";
        }
        catch (Exception ex)
        {
            _lblSummary.Text = $"Error: {ex.Message}";
        }
        finally
        {
            _btnRefresh.Enabled = true;
        }
    }

    private static (List<BootRecord>, List<DegradedProcess>) ReadEventLog()
    {
        var boots = new List<BootRecord>();
        var slow = new List<DegradedProcess>();

        const string channel = "Microsoft-Windows-Diagnostics-Performance/Operational";
        const string query = "*[System[(EventID=100 or EventID=101 or EventID=102 or EventID=103)]]";

        EventLogQuery evQuery;
        try
        {
            evQuery = new EventLogQuery(channel, PathType.LogName, query) { ReverseDirection = true };
        }
        catch
        {
            return (boots, slow);
        }

        using var reader = new EventLogReader(evQuery);
        int count = 0;
        while (reader.ReadEvent() is EventRecord ev && count < 500)
        {
            count++;
            try
            {
                if (ev.Id == 100)
                {
                    int mainPath = ParseXmlProperty(ev, "MainPathBootTime");
                    int postBoot = ParseXmlProperty(ev, "PostBootTime");
                    if (ev.TimeCreated.HasValue)
                        boots.Add(new BootRecord(ev.TimeCreated.Value.ToLocalTime(), mainPath, postBoot));
                }
                else if (ev.Id is 101 or 102 or 103)
                {
                    string proc = GetXmlProperty(ev, "ProcessName", fallback: GetXmlProperty(ev, "FileName", ""));
                    int delay = ParseXmlProperty(ev, "Duration");
                    if (!string.IsNullOrEmpty(proc) && delay > 0 && ev.TimeCreated.HasValue)
                        slow.Add(new DegradedProcess(System.IO.Path.GetFileName(proc), delay, ev.TimeCreated.Value.ToLocalTime()));
                }
            }
            catch { }
        }

        boots = [.. boots.TakeLast(20).OrderBy(b => b.Time)];
        slow = [.. slow.OrderByDescending(s => s.DelayMs).Take(50)];
        return (boots, slow);
    }

    private static string GetXmlProperty(EventRecord ev, string name, string fallback = "")
    {
        try
        {
            string xml = ev.ToXml();
            string tag = $"<{name}>";
            int start = xml.IndexOf(tag, StringComparison.OrdinalIgnoreCase);
            if (start < 0)
                return fallback;
            start += tag.Length;
            int end = xml.IndexOf('<', start);
            return end < 0 ? fallback : xml[start..end];
        }
        catch
        {
            return fallback;
        }
    }

    private static int ParseXmlProperty(EventRecord ev, string name) => int.TryParse(GetXmlProperty(ev, name, "0"), out int v) ? v : 0;

    private void PopulateBootList(List<BootRecord> boots)
    {
        _bootList.BeginUpdate();
        _bootList.Items.Clear();
        foreach (BootRecord b in boots)
        {
            var lvi = new ListViewItem(b.Time.ToString("yyyy-MM-dd HH:mm"));
            lvi.SubItems.Add($"{b.TotalMs / 1000.0:F1}");
            lvi.SubItems.Add($"{b.PostBootMs / 1000.0:F1}");
            if (b.TotalMs > 60_000)
                lvi.ForeColor = Color.FromArgb(200, 80, 30);
            else if (b.TotalMs > 30_000)
                lvi.ForeColor = Color.FromArgb(180, 150, 0);
            _bootList.Items.Add(lvi);
        }
        _bootList.EndUpdate();
    }

    private void PopulateSlowList(List<DegradedProcess> slow)
    {
        _slowList.BeginUpdate();
        _slowList.Items.Clear();
        foreach (DegradedProcess s in slow)
        {
            var lvi = new ListViewItem(s.Name);
            lvi.SubItems.Add($"{s.DelayMs:N0}");
            lvi.SubItems.Add(s.BootTime.ToString("yyyy-MM-dd HH:mm"));
            if (s.DelayMs > 5000)
                lvi.ForeColor = Color.FromArgb(200, 80, 30);
            _slowList.Items.Add(lvi);
        }
        _slowList.EndUpdate();
    }
}
