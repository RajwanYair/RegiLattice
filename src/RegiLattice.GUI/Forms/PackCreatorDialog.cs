#nullable enable

// RegiLattice.GUI — Forms/PackCreatorDialog.cs
// 5-step wizard for authoring and exporting community Tweak Packs (T7.2).

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Plugins;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Wizard dialog for creating a community Tweak Pack (.rlpack JSON file).
/// <para>
/// Steps: 1 → Basic info · 2 → Select tweaks · 3 → Metadata ·
///        4 → JSON preview · 5 → Export / Submit
/// </para>
/// </summary>
internal sealed class PackCreatorDialog : BaseDialog
{
    // ── State ──────────────────────────────────────────────────────────────

    private readonly TweakEngine _engine;
    private readonly List<string> _selectedIds = [];

    // ── Wizard controls ───────────────────────────────────────────────────

    private readonly TabControl _tabs = new() { Dock = DockStyle.Fill, Appearance = TabAppearance.FlatButtons };

    // Step 1 — Basic info
    private readonly TextBox _txtName = new() { Width = 300, PlaceholderText = "my-pack (kebab-case)" };
    private readonly TextBox _txtDisplayName = new() { Width = 300, PlaceholderText = "My Awesome Pack" };
    private readonly TextBox _txtAuthor = new() { Width = 200, PlaceholderText = "GitHub username" };
    private readonly TextBox _txtVersion = new()
    {
        Width = 120,
        Text = "1.0.0",
        PlaceholderText = "1.0.0",
    };

    // Step 2 — Tweak selection
    private readonly TextBox _txtSearch = new() { Width = 280, PlaceholderText = "Search tweaks…" };
    private readonly ListView _lstAvailable = new()
    {
        View = View.Details,
        FullRowSelect = true,
        GridLines = true,
        Height = 220,
        Dock = DockStyle.Fill,
    };
    private readonly ListView _lstSelected = new()
    {
        View = View.Details,
        FullRowSelect = true,
        GridLines = true,
        Height = 220,
        Dock = DockStyle.Fill,
    };

    // Step 3 — Metadata
    private readonly TextBox _txtDescription = new()
    {
        Multiline = true,
        Height = 80,
        Width = 400,
        ScrollBars = ScrollBars.Vertical,
        PlaceholderText = "Describe what this pack does (min 20 chars)…",
    };
    private readonly TextBox _txtDownloadUrl = new() { Width = 400, PlaceholderText = "https://github.com/…" };
    private readonly TextBox _txtSha256 = new() { Width = 400, PlaceholderText = "64-char hex SHA-256 of the pack JSON file" };
    private readonly TextBox _txtCategories = new() { Width = 300, PlaceholderText = "Gaming, Performance" };
    private readonly TextBox _txtTags = new() { Width = 300, PlaceholderText = "gaming, latency" };

    // Step 4 — JSON preview
    private readonly RichTextBox _rtbJson = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        Font = new Font("Consolas", 9f),
        ScrollBars = RichTextBoxScrollBars.Both,
        WordWrap = false,
    };

    // Step 5 — Export / Submit
    private readonly Label _lblValidation = new()
    {
        AutoSize = false,
        Dock = DockStyle.Top,
        Height = 80,
        ForeColor = Color.Tomato,
    };
    private readonly Button _btnExport = new()
    {
        Text = "Export .rlpack…",
        Width = 160,
        Height = 34,
    };
    private readonly Button _btnOpenUrl = new()
    {
        Text = "Open Submission URL",
        Width = 200,
        Height = 34,
    };
    private readonly Label _lblExportStatus = new() { AutoSize = true };

    // ── NavBar ────────────────────────────────────────────────────────────

    private readonly Button _btnPrev = new()
    {
        Text = "← Back",
        Width = 90,
        Height = 30,
        Enabled = false,
    };
    private readonly Button _btnNext = new()
    {
        Text = "Next →",
        Width = 90,
        Height = 30,
    };
    private readonly Label _lblStep = new() { AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };

    // ── Constructor ───────────────────────────────────────────────────────

    public PackCreatorDialog(TweakEngine engine)
        : base("Pack Creator Studio", new Size(760, 580), resizable: true)
    {
        _engine = engine;
        BuildLayout();
        PopulateAvailableTweaks("");
        UpdateNavButtons();
        AppTheme.Apply(this);
    }

    // ── Layout builders ───────────────────────────────────────────────────

    private void BuildLayout()
    {
        // Steps
        _tabs.TabPages.Add(BuildStep1());
        _tabs.TabPages.Add(BuildStep2());
        _tabs.TabPages.Add(BuildStep3());
        _tabs.TabPages.Add(BuildStep4());
        _tabs.TabPages.Add(BuildStep5());
        _tabs.TabPages[0].Text = "1 · Basic Info";
        _tabs.TabPages[1].Text = "2 · Tweaks";
        _tabs.TabPages[2].Text = "3 · Metadata";
        _tabs.TabPages[3].Text = "4 · Preview";
        _tabs.TabPages[4].Text = "5 · Export";
        _tabs.Selecting += Tabs_Selecting;
        _tabs.Selected += Tabs_Selected;

        // Nav bar
        _btnPrev.Click += BtnPrev_Click;
        _btnNext.Click += BtnNext_Click;
        FlowLayoutPanel nav = new()
        {
            Dock = DockStyle.Bottom,
            FlowDirection = FlowDirection.RightToLeft,
            Height = 40,
            Padding = new Padding(4),
        };
        nav.Controls.AddRange([_btnNext, _btnPrev, _lblStep]);

        Controls.AddRange([_tabs, nav]);
    }

    private TabPage BuildStep1()
    {
        TableLayoutPanel tbl = new() { Dock = DockStyle.Fill, ColumnCount = 2 };
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tbl.Padding = new Padding(16);
        tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
        tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
        tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
        tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
        tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        tbl.Controls.Add(Label("Pack ID (slug)"), 0, 0);
        tbl.Controls.Add(_txtName, 1, 0);
        tbl.Controls.Add(Label("Display Name"), 0, 1);
        tbl.Controls.Add(_txtDisplayName, 1, 1);
        tbl.Controls.Add(Label("Author"), 0, 2);
        tbl.Controls.Add(_txtAuthor, 1, 2);
        tbl.Controls.Add(Label("Version"), 0, 3);
        tbl.Controls.Add(_txtVersion, 1, 3);

        foreach (Control c in tbl.Controls)
            if (c is TextBox tb)
                tb.Dock = DockStyle.None;

        TabPage page = new();
        page.Controls.Add(tbl);
        return page;
    }

    private TabPage BuildStep2()
    {
        _lstAvailable.Columns.Add("ID", 220);
        _lstAvailable.Columns.Add("Label", 220);
        _lstAvailable.Columns.Add("Category", 140);
        _lstAvailable.DoubleClick += LstAvailable_DoubleClick;

        _lstSelected.Columns.Add("ID", 220);
        _lstSelected.Columns.Add("Label", 220);
        _lstSelected.Columns.Add("Category", 140);
        _lstSelected.DoubleClick += LstSelected_DoubleClick;

        Button btnAdd = new()
        {
            Text = "Add →",
            Width = 80,
            Height = 28,
        };
        Button btnRemove = new()
        {
            Text = "← Remove",
            Width = 90,
            Height = 28,
        };
        btnAdd.Click += (_, _) => AddSelected();
        btnRemove.Click += (_, _) => RemoveSelected();

        _txtSearch.TextChanged += (_, _) => PopulateAvailableTweaks(_txtSearch.Text.Trim());

        FlowLayoutPanel searchRow = new()
        {
            Dock = DockStyle.Top,
            Height = 36,
            FlowDirection = FlowDirection.LeftToRight,
        };
        searchRow.Controls.AddRange([Label("Search:"), _txtSearch]);

        FlowLayoutPanel arrows = new()
        {
            FlowDirection = FlowDirection.TopDown,
            Width = 100,
            Dock = DockStyle.Fill,
            Padding = new Padding(8, 80, 8, 0),
        };
        arrows.Controls.AddRange([btnAdd, btnRemove]);

        Panel left = new() { Dock = DockStyle.Fill };
        left.Controls.AddRange([_lstAvailable]);

        Panel right = new() { Dock = DockStyle.Fill };
        right.Controls.Add(_lstSelected);

        TableLayoutPanel split = new() { Dock = DockStyle.Fill, ColumnCount = 3 };
        split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
        split.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
        split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
        split.Controls.Add(left, 0, 0);
        split.Controls.Add(arrows, 1, 0);
        split.Controls.Add(right, 2, 0);

        TabPage page = new();
        page.Controls.Add(split);
        page.Controls.Add(searchRow);
        return page;
    }

    private TabPage BuildStep3()
    {
        TableLayoutPanel tbl = new() { Dock = DockStyle.Fill, ColumnCount = 2 };
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tbl.Padding = new Padding(16);
        for (int i = 0; i < 6; i++)
            tbl.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        tbl.Controls.Add(Label("Description"), 0, 0);
        tbl.Controls.Add(_txtDescription, 1, 0);
        tbl.Controls.Add(Label("Download URL"), 0, 1);
        tbl.Controls.Add(_txtDownloadUrl, 1, 1);
        tbl.Controls.Add(Label("SHA-256"), 0, 2);
        tbl.Controls.Add(_txtSha256, 1, 2);
        tbl.Controls.Add(Label("Categories"), 0, 3);
        tbl.Controls.Add(_txtCategories, 1, 3);
        tbl.Controls.Add(Label("Tags"), 0, 4);
        tbl.Controls.Add(_txtTags, 1, 4);

        TabPage page = new();
        page.Controls.Add(tbl);
        return page;
    }

    private TabPage BuildStep4()
    {
        TabPage page = new();
        page.Controls.Add(_rtbJson);
        return page;
    }

    private TabPage BuildStep5()
    {
        _btnExport.Click += BtnExport_Click;
        _btnOpenUrl.Click += BtnOpenUrl_Click;

        _lblValidation.Text = "Tap 'Next' on step 3 to re-validate.";
        _lblValidation.Dock = DockStyle.Top;
        _lblValidation.Height = 100;

        FlowLayoutPanel buttons = new()
        {
            Dock = DockStyle.Top,
            Height = 50,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(4),
        };
        buttons.Controls.AddRange([_btnExport, _btnOpenUrl, _lblExportStatus]);

        TabPage page = new();
        page.Controls.AddRange([buttons, _lblValidation]);
        return page;
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    private static Label Label(string text) =>
        new()
        {
            Text = text,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Padding = new Padding(0, 6, 8, 0),
        };

    private void PopulateAvailableTweaks(string query)
    {
        _lstAvailable.BeginUpdate();
        _lstAvailable.Items.Clear();
        IEnumerable<TweakDef> tweaks = string.IsNullOrWhiteSpace(query) ? _engine.AllTweaks() : _engine.Search(query);
        foreach (TweakDef t in tweaks.OrderBy(x => x.Category).ThenBy(x => x.Label))
        {
            if (_selectedIds.Contains(t.Id))
                continue;
            _lstAvailable.Items.Add(new ListViewItem([t.Id, t.Label, t.Category]) { Tag = t.Id });
        }
        _lstAvailable.EndUpdate();
    }

    private void AddSelected()
    {
        foreach (ListViewItem item in _lstAvailable.SelectedItems)
        {
            string id = (string)item.Tag!;
            if (!_selectedIds.Contains(id))
            {
                _selectedIds.Add(id);
                _lstSelected.Items.Add(new ListViewItem([item.SubItems[0].Text, item.SubItems[1].Text, item.SubItems[2].Text]) { Tag = id });
            }
        }
        PopulateAvailableTweaks(_txtSearch.Text.Trim());
    }

    private void RemoveSelected()
    {
        foreach (ListViewItem item in _lstSelected.SelectedItems)
        {
            string id = (string)item.Tag!;
            _selectedIds.Remove(id);
        }
        foreach (ListViewItem item in _lstSelected.SelectedItems.Cast<ListViewItem>().ToList())
            _lstSelected.Items.Remove(item);
        PopulateAvailableTweaks(_txtSearch.Text.Trim());
    }

    private void LstAvailable_DoubleClick(object? sender, EventArgs e) => AddSelected();

    private void LstSelected_DoubleClick(object? sender, EventArgs e) => RemoveSelected();

    private PackDef BuildPackDef() =>
        new PackDef
        {
            Name = _txtName.Text.Trim(),
            DisplayName = _txtDisplayName.Text.Trim(),
            Author = _txtAuthor.Text.Trim(),
            Version = _txtVersion.Text.Trim(),
            Description = _txtDescription.Text.Trim(),
            DownloadUrl = _txtDownloadUrl.Text.Trim(),
            Sha256 = _txtSha256.Text.Trim().ToLowerInvariant(),
            TweakCount = _selectedIds.Count,
            Categories = _txtCategories.Text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
            Tags = _txtTags.Text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
        };

    private string BuildJsonPreview()
    {
        PackDef pack = BuildPackDef();
        var obj = new
        {
            pack.Name,
            pack.DisplayName,
            pack.Version,
            pack.Author,
            pack.Description,
            pack.TweakCount,
            pack.Categories,
            pack.Tags,
            pack.DownloadUrl,
            pack.Sha256,
            TweakIds = _selectedIds.AsReadOnly(),
        };
        return JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
    }

    private void RefreshJsonPreview()
    {
        _rtbJson.Text = BuildJsonPreview();
    }

    private void RefreshValidation()
    {
        PackDef pack = BuildPackDef();
        PackSubmissionValidation result = PackSubmissionService.Validate(pack);
        if (result.IsValid)
        {
            _lblValidation.ForeColor = Color.SeaGreen;
            _lblValidation.Text = $"✓ Pack is ready for export. {_selectedIds.Count} tweaks selected.";
            _btnExport.Enabled = true;
            _btnOpenUrl.Enabled = true;
        }
        else
        {
            _lblValidation.ForeColor = Color.Tomato;
            _lblValidation.Text = "Validation errors:\n" + string.Join("\n  • ", result.Errors.Prepend("  • "));
            _btnExport.Enabled = false;
            _btnOpenUrl.Enabled = false;
        }
    }

    // ── Navigation ────────────────────────────────────────────────────────

    private void UpdateNavButtons()
    {
        int idx = _tabs.SelectedIndex;
        _btnPrev.Enabled = idx > 0;
        _btnNext.Text = idx == _tabs.TabCount - 1 ? "Finish" : "Next →";
        _btnNext.Enabled = idx < _tabs.TabCount - 1;
        _lblStep.Text = $"Step {idx + 1} of {_tabs.TabCount}";
    }

    private void Tabs_Selecting(object? sender, TabControlCancelEventArgs e)
    {
        // Block user clicking tabs directly — use Prev/Next only
        if (e.TabPageIndex != _tabs.SelectedIndex)
            e.Cancel = true;
    }

    private void Tabs_Selected(object? sender, TabControlEventArgs e) => UpdateNavButtons();

    private void BtnPrev_Click(object? sender, EventArgs e)
    {
        if (_tabs.SelectedIndex > 0)
            _tabs.SelectedIndex--;
    }

    private void BtnNext_Click(object? sender, EventArgs e)
    {
        int idx = _tabs.SelectedIndex;
        if (idx == 3) // About to enter step 5 — rebuild preview + validate
        {
            RefreshJsonPreview();
            RefreshValidation();
        }
        if (idx < _tabs.TabCount - 1)
            _tabs.SelectedIndex++;
    }

    // ── Step 5 actions ─────────────────────────────────────────────────────

    private void BtnExport_Click(object? sender, EventArgs e)
    {
        using SaveFileDialog dlg = new()
        {
            Title = "Export Tweak Pack",
            Filter = "RegiLattice Pack (*.rlpack)|*.rlpack|JSON (*.json)|*.json",
            FileName = _txtName.Text.Trim() + ".rlpack",
        };
        if (dlg.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            File.WriteAllText(dlg.FileName, BuildJsonPreview());
            _lblExportStatus.ForeColor = Color.SeaGreen;
            _lblExportStatus.Text = "Exported ✓";
        }
        catch (Exception ex)
        {
            _lblExportStatus.ForeColor = Color.Tomato;
            _lblExportStatus.Text = $"Error: {ex.Message}";
        }
    }

    private void BtnOpenUrl_Click(object? sender, EventArgs e)
    {
        try
        {
            PackDef pack = BuildPackDef();
            string url = PackSubmissionService.BuildSubmissionUrl(pack);
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            _lblExportStatus.ForeColor = Color.SeaGreen;
            _lblExportStatus.Text = "Opened browser ✓";
        }
        catch (Exception ex)
        {
            _lblExportStatus.ForeColor = Color.Tomato;
            _lblExportStatus.Text = $"Error: {ex.Message}";
        }
    }
}
