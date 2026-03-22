// RegiLattice.GUI — Forms/DependencyGraphDialog.cs
// Sprint 101 — interactive dependency explorer: what a tweak requires and what requires it.

#nullable enable

using RegiLattice.Core;
using RegiLattice.Core.Models;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Explores tweak dependency relationships. For a selected tweak shows:
/// - Its prerequisite chain (ResolveDependencies — topological order)
/// - Tweaks that depend on it (Dependents — reverse lookup)
/// Also lists all tweaks that have at least one dependency.
/// </summary>
internal sealed class DependencyGraphDialog : Form
{
    private readonly TweakEngine _engine;

    // Search / selection controls
    private TextBox _txtSearch = null!;
    private ListBox _lstAll = null!;

    // Dependency tree
    private TreeView _tree = null!;

    // Detail panel
    private Label _lblDetail = null!;

    // Action buttons
    private Button _btnShowAll = null!;
    private Button _btnClose = null!;

    // All tweaks sorted by label (backing list for _lstAll)
    private List<TweakDef> _allTweaks = [];
    private List<TweakDef> _filtered = [];

    // ── Construction ───────────────────────────────────────────────────

    internal DependencyGraphDialog(TweakEngine engine)
    {
        _engine = engine;
        InitUI();
        ApplyTheme();
        LoadAllTweaks();
    }

    // ── UI Construction ────────────────────────────────────────────────

    private void InitUI()
    {
        SuspendLayout();

        Text = "Dependency Graph Explorer";
        Size = new Size(1000, 680);
        MinimumSize = new Size(780, 500);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.Sizable;
        Font = new Font("Segoe UI", 9f);

        // ── Left panel (tweak selector) ───────────────────────────────
        var pnlLeft = new Panel { Dock = DockStyle.Left, Width = 280, Padding = new Padding(8) };

        _txtSearch = new TextBox
        {
            PlaceholderText = "Search tweaks…",
            Dock = DockStyle.Top,
            Height = 28,
        };

        _btnShowAll = new Button
        {
            Text = "Only show tweaks with deps",
            Dock = DockStyle.Top,
            Height = 28,
            Margin = new Padding(0, 4, 0, 0),
        };

        _lstAll = new ListBox
        {
            Dock = DockStyle.Fill,
            IntegralHeight = false,
            ScrollAlwaysVisible = true,
        };

        pnlLeft.Controls.AddRange([_txtSearch, _btnShowAll, _lstAll]);

        // ── Right panel (tree + detail) ───────────────────────────────
        var pnlRight = new Panel { Dock = DockStyle.Fill, Padding = new Padding(4, 8, 8, 8) };

        _tree = new TreeView
        {
            Dock = DockStyle.Fill,
            ShowLines = true,
            ShowPlusMinus = true,
            FullRowSelect = true,
            HideSelection = false,
        };

        _lblDetail = new Label
        {
            Dock = DockStyle.Bottom,
            Height = 72,
            AutoSize = false,
            TextAlign = ContentAlignment.TopLeft,
            Padding = new Padding(4),
        };

        var pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 38 };
        _btnClose = new Button
        {
            Text = "Close",
            Width = 80,
            Height = 28,
            Anchor = AnchorStyles.Right | AnchorStyles.Top,
            Top = 4,
        };
        _btnClose.Left = pnlButtons.Width - _btnClose.Width - 8;
        _btnClose.Anchor = AnchorStyles.Right | AnchorStyles.Top;
        pnlButtons.Controls.Add(_btnClose);

        pnlRight.Controls.AddRange([_btnClose, _lblDetail, _tree]);

        Controls.AddRange([pnlLeft, pnlRight]);

        // Events
        _txtSearch.TextChanged += (_, _) => ApplyFilter(_txtSearch.Text);
        bool showOnlyDeps = false;
        _btnShowAll.Click += (_, _) =>
        {
            showOnlyDeps = !showOnlyDeps;
            _btnShowAll.Text = showOnlyDeps ? "Show all tweaks" : "Only show tweaks with deps";
            ApplyFilter(_txtSearch.Text, onlyWithDeps: showOnlyDeps);
        };
        _lstAll.SelectedIndexChanged += (_, _) => OnTweakSelected();
        _tree.NodeMouseClick += (_, e) => OnNodeClick(e);
        _btnClose.Click += (_, _) => Close();

        ResumeLayout(true);
    }

    // ── Data Loading ───────────────────────────────────────────────────

    private void LoadAllTweaks()
    {
        _allTweaks = _engine.AllTweaks()
            .OrderBy(t => t.Category, StringComparer.OrdinalIgnoreCase)
            .ThenBy(t => t.Label, StringComparer.OrdinalIgnoreCase)
            .ToList();
        ApplyFilter(string.Empty);
    }

    private void ApplyFilter(string query, bool onlyWithDeps = false)
    {
        _filtered = _allTweaks
            .Where(t =>
                (!onlyWithDeps || t.DependsOn.Count > 0) &&
                (string.IsNullOrWhiteSpace(query) ||
                 t.Label.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                 t.Id.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                 t.Category.Contains(query, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        _lstAll.SuspendLayout();
        _lstAll.Items.Clear();
        foreach (TweakDef td in _filtered)
            _lstAll.Items.Add($"[{td.Category}] {td.Label}" + (td.DependsOn.Count > 0 ? " ★" : ""));
        _lstAll.ResumeLayout(true);
        _tree.Nodes.Clear();
        _lblDetail.Text = $"{_filtered.Count} tweaks. ★ = has dependencies.";
    }

    // ── Tree Builder ───────────────────────────────────────────────────

    private void OnTweakSelected()
    {
        int idx = _lstAll.SelectedIndex;
        if (idx < 0 || idx >= _filtered.Count)
            return;
        BuildTree(_filtered[idx]);
    }

    private void BuildTree(TweakDef root)
    {
        _tree.SuspendLayout();
        _tree.BeginUpdate();
        _tree.Nodes.Clear();

        var rootNode = new TreeNode($"⬡  {root.Label}  [{root.Category}]")
        {
            Tag = root.Id,
            NodeFont = new Font(_tree.Font, FontStyle.Bold),
        };

        // ── Prerequisites (what this tweak depends on) ─────────────────
        var depsNode = new TreeNode("▶  Depends on (prerequisites):");
        try
        {
            IReadOnlyList<TweakDef> chain = _engine.ResolveDependencies(root.Id);
            // ResolveDependencies returns the chain in topological order including the root at the end.
            // We want ancestors only (exclude the root itself).
            var prereqs = chain.Where(t => !t.Id.Equals(root.Id, StringComparison.OrdinalIgnoreCase)).ToList();
            if (prereqs.Count == 0)
            {
                depsNode.Nodes.Add("  (none)");
            }
            else
            {
                foreach (TweakDef dep in prereqs)
                    depsNode.Nodes.Add(new TreeNode($"  → {dep.Label}  [{dep.Category}]") { Tag = dep.Id });
            }
        }
        catch (Exception ex)
        {
            depsNode.Nodes.Add($"  Error: {ex.Message}");
        }

        // ── Dependents (what requires this tweak) ─────────────────────
        var needsNode = new TreeNode("◀  Needed by (dependents):");
        try
        {
            IReadOnlyList<TweakDef> deps = _engine.Dependents(root.Id);
            if (deps.Count == 0)
            {
                needsNode.Nodes.Add("  (none)");
            }
            else
            {
                foreach (TweakDef dep in deps)
                    needsNode.Nodes.Add(new TreeNode($"  ← {dep.Label}  [{dep.Category}]") { Tag = dep.Id });
            }
        }
        catch (Exception ex)
        {
            needsNode.Nodes.Add($"  Error: {ex.Message}");
        }

        rootNode.Nodes.AddRange([depsNode, needsNode]);
        _tree.Nodes.Add(rootNode);
        rootNode.ExpandAll();

        _tree.EndUpdate();
        _tree.ResumeLayout(true);

        // Update detail panel
        _lblDetail.Text = $"ID: {root.Id}\n" +
                          $"Category: {root.Category}   |   NeedsAdmin: {root.NeedsAdmin}   |   Scope: {root.Scope}\n" +
                          (string.IsNullOrEmpty(root.Description) ? "" : root.Description);
    }

    private void OnNodeClick(TreeNodeMouseClickEventArgs e)
    {
        if (e.Node?.Tag is not string tweakId)
            return;
        TweakDef? td = _engine.GetTweak(tweakId);
        if (td is null)
            return;

        // Jump to the clicked tweak in the list
        int idx = _filtered.FindIndex(t => t.Id.Equals(tweakId, StringComparison.OrdinalIgnoreCase));
        if (idx >= 0)
        {
            _lstAll.SelectedIndex = idx;
        }
        else
        {
            // Not visible in current filter — update detail panel directly
            BuildTree(td);
        }
    }

    // ── Theme ──────────────────────────────────────────────────────────

    private void ApplyTheme()
    {
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;

        _txtSearch.BackColor = AppTheme.Surface2;
        _txtSearch.ForeColor = AppTheme.Fg;
        _txtSearch.BorderStyle = BorderStyle.FixedSingle;

        _lstAll.BackColor = AppTheme.Surface;
        _lstAll.ForeColor = AppTheme.Fg;

        _tree.BackColor = AppTheme.Surface;
        _tree.ForeColor = AppTheme.Fg;
        _tree.LineColor = AppTheme.Border;

        _lblDetail.BackColor = AppTheme.Surface2;
        _lblDetail.ForeColor = AppTheme.FgDim;

        _btnShowAll.BackColor = AppTheme.Surface2;
        _btnShowAll.ForeColor = AppTheme.FgDim;
        _btnShowAll.FlatStyle = FlatStyle.Flat;
        _btnShowAll.FlatAppearance.BorderSize = 0;

        _btnClose.BackColor = AppTheme.Surface2;
        _btnClose.ForeColor = AppTheme.FgDim;
        _btnClose.FlatStyle = FlatStyle.Flat;
        _btnClose.FlatAppearance.BorderSize = 0;

        foreach (Control c in Controls)
            if (c is Panel p) p.BackColor = AppTheme.Surface;
    }
}
