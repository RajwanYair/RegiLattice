// RegiLattice.GUI — Forms/PipManagerDialog.cs
using RegiLattice.GUI.PackageManagers;
using Elevation = RegiLattice.Core.Elevation;

namespace RegiLattice.GUI.Forms;

/// <summary>pip (Python) package manager dialog — extends BasePackageManagerDialog.</summary>
internal sealed class PipManagerDialog : BasePackageManagerDialog
{
    private readonly ComboBox _cmbScope = new();

    protected override string DialogTitle => "\U0001f40d pip Package Manager";
    protected override Icon? DialogIcon => AppIcons.PipIcon;
    protected override string PrereqReadyText => "Python + pip installed and ready";
    protected override string PrereqMissingText => "Python / pip not found";
    protected override string PrereqInstallingText => "Installing Python via winget...";
    protected override string PrereqInstallButtonText => "Install Python";
    protected override string UpgradeText => "Upgrade";
    protected override string PrereqInstallHint => "winget install Python.Python.3.12 --accept-source-agreements --accept-package-agreements";
    protected override string PrereqInstallUrl => "https://www.python.org/downloads/";
    protected override IReadOnlyList<string> PopularPackages => PipManager.PopularPackages;

    protected override bool CheckPrereq() => PipManager.IsPipInstalled();

    protected override Task InstallPrereqAsync(CancellationToken ct) => PipManager.InstallPythonAsync(ct);

    protected override ColumnHeader[] BuildListColumns() =>
        [
            new ColumnHeader { Text = "Package", Width = 220 },
            new ColumnHeader { Text = "Version", Width = 140 },
            new ColumnHeader { Text = "Status", Width = 140 },
        ];

    protected override Control? BuildScopePanel()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 34,
            BackColor = AppTheme.Surface,
            Padding = new Padding(8, 4, 8, 4),
        };
        var lbl = new Label
        {
            Text = "Scope:",
            AutoSize = true,
            Location = new Point(8, 8),
            ForeColor = AppTheme.Fg,
        };
        _cmbScope.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbScope.Items.AddRange(["All packages", "User-level only"]);
        _cmbScope.SelectedIndex = 0;
        _cmbScope.BackColor = AppTheme.Overlay;
        _cmbScope.ForeColor = AppTheme.Fg;
        _cmbScope.FlatStyle = FlatStyle.Flat;
        _cmbScope.Location = new Point(58, 5);
        _cmbScope.Width = 130;
        _cmbScope.SelectedIndexChanged += async (_, _) => await RefreshAsync();
        panel.Controls.AddRange([lbl, _cmbScope]);
        return panel;
    }

    protected override async Task RefreshCoreAsync(CancellationToken ct)
    {
        bool userOnly = _cmbScope.SelectedIndex == 1;
        var list = await PipManager.ListInstalledAsync(userOnly, ct);
        _installedNames = await PipManager.ListInstalledNamesAsync(ct);
        _lstInstalled.Items.Clear();
        foreach (var entry in list)
        {
            int paren = entry.IndexOf(" (", StringComparison.Ordinal);
            string name = paren > 0 ? entry[..paren] : entry;
            string version = paren > 0 ? entry[(paren + 2)..].TrimEnd(')') : "";
            var item = new ListViewItem(name) { Tag = name };
            item.SubItems.Add(version);
            item.SubItems.Add("\u2714 Up to date");
            item.ForeColor = AppTheme.Fg;
            _lstInstalled.Items.Add(item);
        }
        string scopeLabel = userOnly ? "user-level" : "all";
        SetStatus($"pip: {list.Count} {scopeLabel} package(s)", AppTheme.Green);
        AppendLog($"Found {list.Count} {scopeLabel} package(s).", AppTheme.Green);
        RebuildQuickInstallButtons();
        _ = CheckOutdatedAsync(ct);
    }

    private async Task CheckOutdatedAsync(CancellationToken ct)
    {
        try
        {
            var outdated = await PipManager.ListOutdatedAsync(ct);
            var outdatedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var entry in outdated)
            {
                int paren = entry.IndexOf(" (", StringComparison.Ordinal);
                outdatedNames.Add(paren > 0 ? entry[..paren] : entry);
            }
            foreach (ListViewItem item in _lstInstalled.Items)
            {
                if (item.Tag is string pkgName && outdatedNames.Contains(pkgName))
                {
                    item.SubItems[2].Text = "\u26A0 Update available";
                    item.SubItems[2].ForeColor = AppTheme.Yellow;
                }
            }
            SetOutdated(
                outdated.Count > 0 ? $"\u26A0 {outdated.Count} update(s) available" : "\u2714 All packages up to date",
                outdated.Count > 0 ? AppTheme.Yellow : AppTheme.Green
            );
        }
        catch
        {
            SetOutdated("");
        }
    }

    protected override Task InstallCoreAsync(string name, CancellationToken ct) => PipManager.InstallAsync(name, ct);

    protected override Task RemoveCoreAsync(string name, CancellationToken ct) => PipManager.UninstallAsync(name, ct);

    protected override Task UpgradeCoreAsync(string name, CancellationToken ct) => PipManager.UpgradeAsync(name, ct);
}
