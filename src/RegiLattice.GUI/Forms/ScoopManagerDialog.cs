// RegiLattice.GUI — Forms/ScoopManagerDialog.cs
using RegiLattice.GUI.PackageManagers;

namespace RegiLattice.GUI.Forms;

/// <summary>Scoop package manager dialog — extends BasePackageManagerDialog.</summary>
internal sealed class ScoopManagerDialog : BasePackageManagerDialog
{
    protected override string DialogTitle => "\U0001f944 Scoop Package Manager";
    protected override Icon? DialogIcon => AppIcons.ScoopIcon;
    protected override string PrereqReadyText => "Scoop is installed and ready";
    protected override string PrereqMissingText => "Scoop is not installed";
    protected override string PrereqInstallingText => "Installing Scoop...";
    protected override string PrereqInstallButtonText => "Install Scoop";
    protected override string UpgradeText => "Upgrade";
    protected override string PrereqInstallHint => "Set-ExecutionPolicy RemoteSigned -Scope CurrentUser -Force; irm get.scoop.sh | iex";
    protected override string PrereqInstallUrl => "https://scoop.sh";
    protected override IReadOnlyList<string> PopularPackages => ScoopManager.PopularTools;

    protected override bool CheckPrereq() => ScoopManager.IsScoopInstalled();

    protected override Task InstallPrereqAsync(CancellationToken ct) => ScoopManager.InstallScoopAsync(ct);

    protected override ColumnHeader[] BuildListColumns() =>
        [
            new ColumnHeader { Text = "Package", Width = 220 },
            new ColumnHeader { Text = "Version", Width = 140 },
            new ColumnHeader { Text = "Size", Width = 80 },
            new ColumnHeader { Text = "Status", Width = 140 },
        ];

    protected override async Task RefreshCoreAsync(CancellationToken ct)
    {
        var list = await ScoopManager.ListInstalledAsync(ct);
        _installedNames = await ScoopManager.ListInstalledNamesAsync(ct);
        _lstInstalled.Items.Clear();
        foreach (var entry in list)
        {
            int paren = entry.IndexOf(" (", StringComparison.Ordinal);
            string name = paren > 0 ? entry[..paren] : entry;
            string version = paren > 0 ? entry[(paren + 2)..].TrimEnd(')') : "";
            var item = new ListViewItem(name) { Tag = name };
            item.SubItems.Add(version);            // Version
            item.SubItems.Add("…");               // Size (computed lazily)
            item.SubItems.Add("\u2714 Up to date"); // Status
            item.ForeColor = AppTheme.Fg;
            _lstInstalled.Items.Add(item);
        }
        SetStatus(list.Count > 0 ? $"Scoop: {list.Count} package(s) installed" : "Scoop: installed \u2014 no packages", AppTheme.Green);
        AppendLog($"Found {list.Count} installed package(s).", AppTheme.Green);
        RebuildQuickInstallButtons();
        _ = CheckOutdatedAsync(ct);
        _ = CheckSizesAsync(ct);
    }

    private async Task CheckOutdatedAsync(CancellationToken ct)
    {
        try
        {
            var outdated = await ScoopManager.ListOutdatedAsync(ct);
            _outdatedNames.Clear();
            var versionMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var entry in outdated)
            {
                int paren = entry.IndexOf(" (", StringComparison.Ordinal);
                string name = paren > 0 ? entry[..paren] : entry;
                _outdatedNames.Add(name);
                if (paren > 0)
                    versionMap[name] = entry[(paren + 2)..].TrimEnd(')');
            }
            foreach (ListViewItem item in _lstInstalled.Items)
            {
                if (item.Tag is string pkgName && _outdatedNames.Contains(pkgName))
                {
                    item.SubItems[3].Text = versionMap.TryGetValue(pkgName, out string? vLabel)
                        ? $"\u26A0 {vLabel}"
                        : "\u26A0 Update available";
                    item.SubItems[3].ForeColor = AppTheme.Yellow;
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

    protected override Task InstallCoreAsync(string name, CancellationToken ct) => ScoopManager.InstallAsync(name, ct);

    protected override Task RemoveCoreAsync(string name, CancellationToken ct) => ScoopManager.UninstallAsync(name, ct);

    protected override Task UpgradeCoreAsync(string name, CancellationToken ct) => ScoopManager.UpgradeAsync(name, ct);

    private async Task CheckSizesAsync(CancellationToken ct)
    {
        try
        {
            var sizes = await ScoopManager.GetInstalledSizesAsync(_installedNames, ct);
            if (InvokeRequired)
                Invoke(() => ApplySizes(sizes));
            else
                ApplySizes(sizes);
        }
        catch { /* ignore — size is optional */ }
    }

    private void ApplySizes(Dictionary<string, string> sizes)
    {
        foreach (ListViewItem item in _lstInstalled.Items)
        {
            if (item.Tag is string pkgName && sizes.TryGetValue(pkgName, out string? size))
                item.SubItems[2].Text = size;
        }
    }
}
