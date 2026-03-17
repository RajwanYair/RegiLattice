// RegiLattice.GUI — Forms/ChocolateyManagerDialog.cs
using RegiLattice.GUI.PackageManagers;

namespace RegiLattice.GUI.Forms;

/// <summary>Chocolatey package manager dialog — extends BasePackageManagerDialog.</summary>
internal sealed class ChocolateyManagerDialog : BasePackageManagerDialog
{
    protected override string DialogTitle => "\U0001f36b Chocolatey Package Manager";
    protected override Icon? DialogIcon => AppIcons.ChocolateyIcon;
    protected override string PrereqReadyText => "Chocolatey is installed and ready";
    protected override string PrereqMissingText => "Chocolatey is not installed";
    protected override string PrereqInstallingText => "Installing Chocolatey (may need admin)...";
    protected override string PrereqInstallButtonText => "Install Choco";
    protected override string UpgradeText => "Upgrade";
    protected override IReadOnlyList<string> PopularPackages => ChocolateyManager.PopularPackages;
    protected override bool CheckPrereq() => ChocolateyManager.IsChocoInstalled();
    protected override Task InstallPrereqAsync(CancellationToken ct) => ChocolateyManager.InstallChocoAsync(ct);

    protected override ColumnHeader[] BuildListColumns() =>
    [
        new ColumnHeader { Text = "Package", Width = 220 },
        new ColumnHeader { Text = "Version", Width = 140 },
        new ColumnHeader { Text = "Status", Width = 140 },
    ];

    protected override async Task RefreshCoreAsync(CancellationToken ct)
    {
        var list = await ChocolateyManager.ListInstalledAsync(ct);
        _installedNames = await ChocolateyManager.ListInstalledNamesAsync(ct);
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
        SetStatus($"Chocolatey: {list.Count} package(s) installed", AppTheme.Green);
        AppendLog($"Found {list.Count} installed package(s).", AppTheme.Green);
        RebuildQuickInstallButtons();
        _ = CheckOutdatedAsync(ct);
    }

    private async Task CheckOutdatedAsync(CancellationToken ct)
    {
        try
        {
            var outdated = await ChocolateyManager.ListOutdatedAsync(ct);
            _outdatedNames.Clear();
            foreach (var entry in outdated)
            {
                int paren = entry.IndexOf(" (", StringComparison.Ordinal);
                string name = paren > 0 ? entry[..paren] : entry;
                _outdatedNames.Add(name);
            }
            foreach (ListViewItem item in _lstInstalled.Items)
            {
                if (item.Tag is string pkgName && _outdatedNames.Contains(pkgName))
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

    protected override Task InstallCoreAsync(string name, CancellationToken ct) =>
        ChocolateyManager.InstallAsync(name, ct);

    protected override Task RemoveCoreAsync(string name, CancellationToken ct) =>
        ChocolateyManager.UninstallAsync(name, ct);

    protected override Task UpgradeCoreAsync(string name, CancellationToken ct) =>
        ChocolateyManager.UpgradeAsync(name, ct);
}
