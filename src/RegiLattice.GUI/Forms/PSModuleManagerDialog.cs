// RegiLattice.GUI — Forms/PSModuleManagerDialog.cs
using RegiLattice.GUI.PackageManagers;

namespace RegiLattice.GUI.Forms;

/// <summary>PowerShell module manager dialog — extends BasePackageManagerDialog.</summary>
internal sealed class PSModuleManagerDialog : BasePackageManagerDialog
{
    private readonly ComboBox _cmbScope = new();

    protected override string DialogTitle => "\u26a1 PowerShell Modules Manager";
    protected override Icon? DialogIcon => AppIcons.PSModuleIcon;
    protected override string PrereqReadyText => "PowerShellGet is available";
    protected override string PrereqMissingText => "PowerShellGet module not found";
    protected override string PrereqInstallingText => "Installing PowerShellGet...";
    protected override string PrereqInstallButtonText => "Install PSGet";
    protected override string UpgradeText => "Update";
    protected override string PrereqInstallHint => "Install-Module PowerShellGet -Force -AllowClobber -Scope CurrentUser";
    protected override string PrereqInstallUrl => "https://learn.microsoft.com/en-us/powershell/scripting/gallery/installing-psget";
    protected override IReadOnlyList<string> PopularPackages => PSModuleManager.PopularModules;

    protected override bool CheckPrereq() => PSModuleManager.IsPowerShellGetAvailable();

    protected override Task InstallPrereqAsync(CancellationToken ct) => PSModuleManager.InstallPowerShellGetAsync(ct);

    protected override ColumnHeader[] BuildListColumns() =>
        [
            new ColumnHeader { Text = "Module", Width = 240 },
            new ColumnHeader { Text = "Version", Width = 140 },
            new ColumnHeader { Text = "Size", Width = 80 },
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
        _cmbScope.Items.AddRange(["CurrentUser", "AllUsers"]);
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
        string scope = _cmbScope.SelectedItem?.ToString() ?? "CurrentUser";
        var list = await PSModuleManager.ListInstalledAsync(scope, ct);
        _installedNames = await PSModuleManager.ListInstalledNamesAsync(scope, ct);
        _lstInstalled.Items.Clear();
        foreach (var m in list)
        {
            var item = new ListViewItem(m) { Tag = m };
            item.SubItems.Add(""); // Version
            item.SubItems.Add("…"); // Size (computed lazily)
            item.SubItems.Add("Installed"); // Status
            item.ForeColor = AppTheme.Fg;
            _lstInstalled.Items.Add(item);
        }
        SetStatus($"Scope: {scope} \u2014 {list.Count} module(s)", AppTheme.Fg);
        AppendLog($"Found {list.Count} module(s) in scope '{scope}'.", AppTheme.Green);
        RebuildQuickInstallButtons();
        _ = CheckOutdatedAsync(scope, ct);
        _ = CheckSizesAsync(scope, ct);
    }

    private async Task CheckOutdatedAsync(string scope, CancellationToken ct)
    {
        try
        {
            var outdated = await PSModuleManager.ListOutdatedAsync(scope, ct);
            _outdatedNames.Clear();
            var versionMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var entry in outdated)
            {
                int paren = entry.IndexOf(" (", StringComparison.Ordinal);
                string moduleName = paren > 0 ? entry[..paren] : entry;
                _outdatedNames.Add(moduleName);
                if (paren > 0)
                    versionMap[moduleName] = entry[(paren + 2)..].TrimEnd(')').Replace(" -> ", " \u2192 ");
            }
            foreach (ListViewItem item in _lstInstalled.Items)
            {
                if (item.Tag is string name && _outdatedNames.Contains(name))
                {
                    item.SubItems[3].Text = versionMap.TryGetValue(name, out string? vLabel) ? $"\u26A0 {vLabel}" : "\u26A0 Update available";
                    item.SubItems[3].ForeColor = AppTheme.Yellow;
                }
            }
            SetOutdated(
                outdated.Count > 0 ? $"\u26A0 {outdated.Count} update(s) available" : "\u2714 All modules up to date",
                outdated.Count > 0 ? AppTheme.Yellow : AppTheme.Green
            );
        }
        catch
        {
            SetOutdated("");
        }
    }

    protected override Task InstallCoreAsync(string name, CancellationToken ct)
    {
        string scope = _cmbScope.SelectedItem?.ToString() ?? "CurrentUser";
        return PSModuleManager.InstallAsync(name, scope, ct);
    }

    protected override Task RemoveCoreAsync(string name, CancellationToken ct) => PSModuleManager.RemoveAsync(name, ct);

    protected override Task UpgradeCoreAsync(string name, CancellationToken ct)
    {
        string scope = _cmbScope.SelectedItem?.ToString() ?? "CurrentUser";
        return PSModuleManager.UpdateAsync(name, scope, ct);
    }

    private async Task CheckSizesAsync(string scope, CancellationToken ct)
    {
        try
        {
            var bases = await PSModuleManager.GetModuleBasesAsync(scope, ct);
            var sizes = await PSModuleManager.GetInstalledSizesAsync(bases, ct);
            if (InvokeRequired)
                Invoke(() => ApplySizes(sizes));
            else
                ApplySizes(sizes);
        }
        catch
        { /* ignore — size is optional */
        }
    }

    private void ApplySizes(Dictionary<string, string> sizes)
    {
        foreach (ListViewItem item in _lstInstalled.Items)
        {
            if (item.Tag is string name && sizes.TryGetValue(name, out string? size))
                item.SubItems[2].Text = size;
        }
    }
}
