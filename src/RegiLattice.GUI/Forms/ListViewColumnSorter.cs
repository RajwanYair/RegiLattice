// RegiLattice.GUI — Forms/ListViewColumnSorter.cs
// Provides click-to-sort and per-column filter dropdown for the main ListView.

using System.Collections;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// IComparer for ListView that supports ascending/descending sort on any column.
/// Detects numeric values and sorts them numerically; otherwise uses case-insensitive string compare.
/// </summary>
internal sealed class ListViewColumnSorter : IComparer
{
    internal int ColumnIndex { get; set; }
    internal SortOrder Order { get; set; } = SortOrder.None;

    public int Compare(object? x, object? y)
    {
        if (Order == SortOrder.None)
            return 0;
        if (x is not ListViewItem a || y is not ListViewItem b)
            return 0;

        string textA = ColumnIndex < a.SubItems.Count ? a.SubItems[ColumnIndex].Text : "";
        string textB = ColumnIndex < b.SubItems.Count ? b.SubItems[ColumnIndex].Text : "";

        int result;
        if (int.TryParse(textA, out int numA) && int.TryParse(textB, out int numB))
            result = numA.CompareTo(numB);
        else
            result = string.Compare(textA, textB, StringComparison.OrdinalIgnoreCase);

        return Order == SortOrder.Descending ? -result : result;
    }
}

/// <summary>
/// Dropdown filter popup attached to a ListView column header.
/// Shows unique values in that column and lets the user check/uncheck to filter rows.
/// </summary>
internal sealed class ColumnFilterPopup : Form
{
    private readonly CheckedListBox _checkedList = new();
    private readonly Button _btnApply;
    private readonly Button _btnClear;
    private readonly TextBox _txtSearch;

    /// <summary>Set of checked (visible) values after user presses Apply.</summary>
    internal HashSet<string> SelectedValues { get; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>True if the user applied a filter (not cancelled).</summary>
    internal bool Applied { get; private set; }

    internal ColumnFilterPopup(IEnumerable<string> uniqueValues, HashSet<string>? previousFilter)
    {
        Text = "Filter";
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        StartPosition = FormStartPosition.Manual;
        ShowInTaskbar = false;
        ClientSize = new Size(220, 320);
        BackColor = AppTheme.Surface;
        ForeColor = AppTheme.Fg;
        Font = AppTheme.Regular;
        TopMost = true;

        _txtSearch = new TextBox
        {
            Dock = DockStyle.Top,
            BackColor = AppTheme.Overlay,
            ForeColor = AppTheme.Fg,
            PlaceholderText = "Search...",
            Height = 24,
        };
        _txtSearch.TextChanged += OnSearchChanged;

        _checkedList.Dock = DockStyle.Fill;
        _checkedList.BackColor = AppTheme.Bg;
        _checkedList.ForeColor = AppTheme.Fg;
        _checkedList.BorderStyle = BorderStyle.None;
        _checkedList.CheckOnClick = true;
        _checkedList.Font = AppTheme.Small;

        var sorted = uniqueValues.OrderBy(v => v, StringComparer.OrdinalIgnoreCase).ToList();
        foreach (string val in sorted)
        {
            bool isChecked = previousFilter is null || previousFilter.Contains(val);
            _checkedList.Items.Add(val, isChecked);
        }

        var btnPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 34,
            BackColor = AppTheme.Surface,
        };

        _btnApply = new Button
        {
            Text = "Apply",
            BackColor = AppTheme.Green,
            ForeColor = AppTheme.Bg,
            FlatStyle = FlatStyle.Flat,
            Size = new Size(65, 26),
            Location = new Point(4, 4),
        };
        _btnApply.Click += (_, _) =>
        {
            SelectedValues.Clear();
            for (int i = 0; i < _checkedList.Items.Count; i++)
            {
                if (_checkedList.GetItemChecked(i))
                    SelectedValues.Add(_checkedList.Items[i]?.ToString() ?? "");
            }
            Applied = true;
            Close();
        };

        _btnClear = new Button
        {
            Text = "Clear Filter",
            BackColor = AppTheme.Red,
            ForeColor = AppTheme.Bg,
            FlatStyle = FlatStyle.Flat,
            Size = new Size(80, 26),
            Location = new Point(74, 4),
        };
        _btnClear.Click += (_, _) =>
        {
            SelectedValues.Clear();
            Applied = true;
            Close();
        };

        var btnCancel = new Button
        {
            Text = "Cancel",
            BackColor = AppTheme.Overlay,
            ForeColor = AppTheme.Fg,
            FlatStyle = FlatStyle.Flat,
            Size = new Size(58, 26),
            Location = new Point(158, 4),
        };
        btnCancel.Click += (_, _) => Close();

        btnPanel.Controls.AddRange([_btnApply, _btnClear, btnCancel]);
        Controls.AddRange([_checkedList, _txtSearch, btnPanel]);

        Deactivate += (_, _) => Close();
    }

    private void OnSearchChanged(object? sender, EventArgs e)
    {
        string query = _txtSearch.Text.Trim();
        _checkedList.BeginUpdate();

        if (query.Length == 0)
        {
            // Show all items — can't easily show/hide in CheckedListBox, so we just scroll
            _checkedList.EndUpdate();
            return;
        }

        // Uncheck items that don't match the search
        for (int i = 0; i < _checkedList.Items.Count; i++)
        {
            string text = _checkedList.Items[i]?.ToString() ?? "";
            bool matches = text.Contains(query, StringComparison.OrdinalIgnoreCase);
            if (!matches && _checkedList.GetItemChecked(i))
                _checkedList.SetItemChecked(i, false);
        }
        _checkedList.EndUpdate();
    }
}
