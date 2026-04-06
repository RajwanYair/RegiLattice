#nullable enable
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Phase 2.2 — Keyboard shortcut cheatsheet.
/// Non-modal, themed, filterable two-column grid of all keyboard shortcuts.
/// </summary>
internal sealed class KeyboardShortcutsDialog : Form
{
    // ── Shortcut data ─────────────────────────────────────────────────────
    private static readonly (string Keys, string Action, string Group)[] _shortcuts =
    [
        // Navigation & Search
        ("Ctrl+F",          "Focus search box",             "Navigation"),
        ("Esc",             "Clear search / close dialog",  "Navigation"),
        ("F5",              "Refresh tweak status",          "Navigation"),
        ("Ctrl+A",          "Select all tweaks",             "Navigation"),
        ("Ctrl+D",          "Deselect all tweaks",           "Navigation"),
        ("Ctrl+I",          "Invert selection",              "Navigation"),

        // Tweak Operations
        ("Ctrl+Enter",      "Apply selected tweaks",         "Operations"),
        ("Ctrl+Delete",     "Remove selected tweaks",        "Operations"),
        ("Ctrl+Z",          "Undo last operation",           "Operations"),
        ("Ctrl+Y",          "Redo last operation",           "Operations"),
        ("Space",           "Toggle checked state",          "Operations"),

        // View & Panels
        ("F1",              "Show keyboard shortcuts",       "View"),
        ("Ctrl+L",          "Toggle log panel",              "View"),
        ("Ctrl+Shift+P",    "Open Preferences",              "View"),

        // File & Export
        ("Ctrl+Shift+E",    "Export as PowerShell (.ps1)",   "File"),
        ("Ctrl+Shift+J",    "Export selected IDs as JSON",   "File"),
        ("Ctrl+Shift+R",    "Export as .REG file",           "File"),
        ("Ctrl+Shift+I",    "Import tweak IDs from JSON",    "File"),
    ];

    // ── Controls ──────────────────────────────────────────────────────────
    private readonly TextBox _filterBox;
    private readonly ListView _listView;

    internal KeyboardShortcutsDialog()
    {
        Text = "Keyboard Shortcuts — RegiLattice";
        Size = new Size(640, 480);
        MinimumSize = new Size(480, 360);
        FormBorderStyle = FormBorderStyle.Sizable;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        ShowInTaskbar = false;
        KeyPreview = true;

        // Filter box
        var filterLabel = new Label
        {
            Text = "Filter:",
            AutoSize = true,
            Location = new Point(12, 14),
            ForeColor = AppTheme.Fg,
            Font = AppTheme.Regular,
        };

        _filterBox = new TextBox
        {
            Location = new Point(60, 10),
            Width = 220,
            BackColor = AppTheme.Surface,
            ForeColor = AppTheme.Fg,
            BorderStyle = BorderStyle.FixedSingle,
            Font = AppTheme.Regular,
        };
        _filterBox.TextChanged += (_, _) => ApplyFilter();

        // ListView
        _listView = new ListView
        {
            Location = new Point(12, 44),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            View = View.Details,
            FullRowSelect = true,
            GridLines = false,
            BorderStyle = BorderStyle.None,
            BackColor = AppTheme.Surface,
            ForeColor = AppTheme.Fg,
            Font = AppTheme.Regular,
            HeaderStyle = ColumnHeaderStyle.Nonclickable,
        };
        _listView.Columns.AddRange(new[]
        {
            new ColumnHeader { Text = "Keys",   Width = 160 },
            new ColumnHeader { Text = "Action", Width = 280 },
            new ColumnHeader { Text = "Group",  Width = 120 },
        });

        // Close button
        var btnClose = new Button
        {
            Text = "Close",
            DialogResult = DialogResult.OK,
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
            BackColor = AppTheme.Surface2,
            ForeColor = AppTheme.Fg,
            FlatStyle = FlatStyle.Flat,
            Font = AppTheme.Regular,
        };
        btnClose.FlatAppearance.BorderColor = AppTheme.Border;

        Controls.AddRange(new Control[] { filterLabel, _filterBox, _listView, btnClose });
        AcceptButton = btnClose;
        CancelButton = btnClose;

        // Position close button after controls are added
        Resize += (_, _) => btnClose.Location = new Point(Width - btnClose.Width - 20, Height - btnClose.Height - 45);
        btnClose.Size = new Size(80, 28);

        KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        };

        PopulateList(_shortcuts);
        btnClose.Location = new Point(Width - btnClose.Width - 20, Height - btnClose.Height - 45);
    }

    private void ApplyFilter()
    {
        var q = _filterBox.Text.Trim();
        var filtered = string.IsNullOrEmpty(q)
            ? _shortcuts
            : _shortcuts.Where(s =>
                s.Keys.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                s.Action.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                s.Group.Contains(q, StringComparison.OrdinalIgnoreCase))
              .ToArray();
        PopulateList(filtered);
    }

    private void PopulateList((string Keys, string Action, string Group)[] items)
    {
        _listView.BeginUpdate();
        _listView.Items.Clear();
        foreach (var (k, a, g) in items)
        {
            var lvi = new ListViewItem([k, a, g])
            {
                BackColor = AppTheme.Surface,
                ForeColor = AppTheme.Fg,
            };
            _listView.Items.Add(lvi);
        }
        _listView.EndUpdate();
    }
}
