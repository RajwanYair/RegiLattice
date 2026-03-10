using RegiLattice.Native.Models;

namespace RegiLattice.Native.Forms;

/// <summary>
/// RegiLattice Native GUI — main window.
///
/// Layout (top→bottom):
///   ToolStrip  — Apply / Remove / Refresh / Status filter combo
///   SplitContainer
///     Left  — TreeView of categories
///     Right — ListView of tweaks in selected category
///   StatusStrip — counters + progress label
/// </summary>
public partial class MainForm : Form
{
    // ── State ──────────────────────────────────────────────────────────────
    private PythonBridge?              _bridge;
    private IReadOnlyList<TweakInfo>   _allTweaks   = [];
    private CancellationTokenSource    _cts         = new();

    // ── Construction ───────────────────────────────────────────────────────
    public MainForm()
    {
        InitializeComponent();
        Text = "RegiLattice — Native GUI";
        Icon = SystemIcons.Application;
    }

    // ── Lifecycle ──────────────────────────────────────────────────────────
    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await InitialiseBridgeAsync();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _cts.Cancel();
        _bridge?.Dispose();
        base.OnFormClosing(e);
    }

    // ── Initialisation ─────────────────────────────────────────────────────
    private async Task InitialiseBridgeAsync()
    {
        SetStatus("Locating Python…");
        try
        {
            string pythonPath = PythonBridge.FindPython();
            _bridge = new PythonBridge(pythonPath);
            SetStatus($"Python: {pythonPath}");
            await RefreshTweaksAsync();
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Python not found",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            SetStatus("Error: Python not found.");
        }
    }

    // ── Actions ────────────────────────────────────────────────────────────
    private async Task RefreshTweaksAsync()
    {
        if (_bridge is null) return;
        SetBusy(true, "Loading tweaks…");
        try
        {
            _allTweaks = await _bridge.LoadTweaksAsync(_cts.Token);
            PopulateTree();
            UpdateCounters();
            SetStatus($"Loaded {_allTweaks.Count} tweaks.");
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load tweaks:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetStatus("Error loading tweaks.");
        }
        finally { SetBusy(false); }
    }

    private async Task ApplySelectedAsync()
    {
        if (_bridge is null) return;
        var selected = GetSelectedTweaks();
        if (selected.Count == 0) return;

        SetBusy(true, $"Applying {selected.Count} tweak(s)…");
        foreach (var tweak in selected)
        {
            var (exitCode, err) = await _bridge.ApplyTweakAsync(tweak.Id, _cts.Token);
            tweak.Status = exitCode == 0 ? "applied" : tweak.Status;
            if (exitCode != 0)
                SetStatus($"Error applying {tweak.Id}: {err.Trim()}");
        }
        RefreshListView();
        UpdateCounters();
        SetBusy(false, $"Applied {selected.Count} tweak(s).");
    }

    private async Task RemoveSelectedAsync()
    {
        if (_bridge is null) return;
        var selected = GetSelectedTweaks();
        if (selected.Count == 0) return;

        SetBusy(true, $"Removing {selected.Count} tweak(s)…");
        foreach (var tweak in selected)
        {
            var (exitCode, err) = await _bridge.RemoveTweakAsync(tweak.Id, _cts.Token);
            tweak.Status = exitCode == 0 ? "not_applied" : tweak.Status;
            if (exitCode != 0)
                SetStatus($"Error removing {tweak.Id}: {err.Trim()}");
        }
        RefreshListView();
        UpdateCounters();
        SetBusy(false, $"Removed {selected.Count} tweak(s).");
    }

    // ── Tree / List population ─────────────────────────────────────────────
    private void PopulateTree()
    {
        _treeView.BeginUpdate();
        _treeView.Nodes.Clear();
        foreach (string cat in _allTweaks.Select(t => t.Category).Distinct().OrderBy(c => c))
        {
            int count   = _allTweaks.Count(t => t.Category == cat);
            int applied = _allTweaks.Count(t => t.Category == cat && t.IsApplied);
            var node    = new TreeNode($"{cat}  ({applied}/{count})") { Tag = cat };
            _treeView.Nodes.Add(node);
        }
        _treeView.EndUpdate();

        if (_treeView.Nodes.Count > 0)
        {
            _treeView.SelectedNode = _treeView.Nodes[0];
            PopulateList((string)_treeView.Nodes[0].Tag!);
        }
    }

    private void PopulateList(string category)
    {
        string filter = _filterCombo.SelectedItem?.ToString() ?? "All";
        _listView.BeginUpdate();
        _listView.Items.Clear();

        IEnumerable<TweakInfo> tweaks = _allTweaks.Where(t => t.Category == category);
        if (filter != "All")
            tweaks = tweaks.Where(t => string.Equals(t.Status, filter, StringComparison.OrdinalIgnoreCase));

        foreach (var tw in tweaks)
        {
            var item = new ListViewItem(tw.Label) { Tag = tw };
            item.SubItems.Add(tw.Status);
            item.SubItems.Add(tw.ScopeBadge);
            item.SubItems.Add(tw.NeedsAdmin ? "Yes" : "No");
            item.SubItems.Add(tw.CorpSafe   ? "Yes" : "No");
            item.ForeColor = tw.IsApplied ? Color.FromArgb(0, 180, 120) : SystemColors.ControlText;
            _listView.Items.Add(item);
        }

        _listView.EndUpdate();
    }

    private void RefreshListView()
    {
        if (_treeView.SelectedNode is { Tag: string cat })
            PopulateList(cat);
    }

    // ── Helpers / Utilities ────────────────────────────────────────────────
    private List<TweakInfo> GetSelectedTweaks()
        => _listView.SelectedItems
                    .Cast<ListViewItem>()
                    .Select(i => (TweakInfo)i.Tag!)
                    .ToList();

    private void UpdateCounters()
    {
        int applied = _allTweaks.Count(t => t.IsApplied);
        _statusLabel.Text = $"Total: {_allTweaks.Count} | Applied: {applied}";
    }

    private void SetStatus(string message)
        => _progressLabel.Text = message;

    private void SetBusy(bool busy, string? message = null)
    {
        _progressBar.Style  = busy ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
        _btnApply.Enabled   = !busy;
        _btnRemove.Enabled  = !busy;
        _btnRefresh.Enabled = !busy;
        if (message is not null) SetStatus(message);
    }

    // ── Event handlers ─────────────────────────────────────────────────────
    private void OnTreeAfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is string cat)
            PopulateList(cat);
    }

    private async void OnApplyClicked(object? sender, EventArgs e)
        => await ApplySelectedAsync();

    private async void OnRemoveClicked(object? sender, EventArgs e)
        => await RemoveSelectedAsync();

    private async void OnRefreshClicked(object? sender, EventArgs e)
        => await RefreshTweaksAsync();

    private void OnFilterChanged(object? sender, EventArgs e)
        => RefreshListView();
}
