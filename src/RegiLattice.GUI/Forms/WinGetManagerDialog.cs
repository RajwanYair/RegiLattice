// RegiLattice.GUI — Forms/WinGetManagerDialog.cs
using RegiLattice.GUI.PackageManagers;

namespace RegiLattice.GUI.Forms;

/// <summary>WinGet package manager dialog — extends BasePackageManagerDialog.</summary>
internal sealed class WinGetManagerDialog : BasePackageManagerDialog
{
    private readonly Button _btnSearch = new() { Text = "Search" };

    protected override string DialogTitle => "\U0001f4e6 WinGet Package Manager";
    protected override Icon? DialogIcon => AppIcons.WinGetIcon;
    protected override string PrereqReadyText => "WinGet is installed and ready";
    protected override string PrereqMissingText => "WinGet is not available";
    protected override string PrereqInstallingText => "Registering App Installer...";
    protected override string PrereqInstallButtonText => "Install WinGet";
    protected override string UpgradeText => "Upgrade";
    protected override string PrereqInstallHint => "Add-AppxPackage -RegisterByFamilyName -MainPackage Microsoft.DesktopAppInstaller_8wekyb3d8bbwe";
    protected override string PrereqInstallUrl => "https://aka.ms/getwinget";
    protected override IReadOnlyList<string> PopularPackages => WinGetManager.PopularPackages;

    protected override bool CheckPrereq() => WinGetManager.IsWinGetInstalled();

    protected override Task InstallPrereqAsync(CancellationToken ct) => WinGetManager.InstallWinGetAsync(ct);

    protected override ColumnHeader[] BuildListColumns() =>
        [
            new ColumnHeader { Text = "Package", Width = 300 },
            new ColumnHeader { Text = "Version", Width = 140 },
            new ColumnHeader { Text = "Status", Width = 140 },
        ];

    protected override void AddExtraButtons(Panel ctrlPanel, ref int x)
    {
        _btnSearch.BackColor = AppTheme.Accent;
        _btnSearch.ForeColor = AppTheme.Bg;
        _btnSearch.FlatStyle = FlatStyle.Flat;
        _btnSearch.Location = new Point(x, 7);
        _btnSearch.Size = new Size(84, 28);
        _btnSearch.Click += async (_, _) => await SearchAsync();
        ctrlPanel.Controls.Add(_btnSearch);
        x += 90;
    }

    protected override async Task RefreshCoreAsync(CancellationToken ct)
    {
        var list = await WinGetManager.ListInstalledAsync(ct);
        _installedNames = await WinGetManager.ListInstalledNamesAsync(ct);
        _lstInstalled.Items.Clear();
        foreach (var entry in list)
        {
            var item = new ListViewItem(entry) { Tag = entry };
            item.SubItems.Add("");
            item.SubItems.Add("\u2714 Up to date");
            item.ForeColor = AppTheme.Fg;
            _lstInstalled.Items.Add(item);
        }
        SetStatus($"winget: {list.Count} package(s) installed", AppTheme.Green);
        AppendLog($"Found {list.Count} installed package(s).", AppTheme.Green);
        RebuildQuickInstallButtons();
        _ = CheckOutdatedAsync(ct);
    }

    private async Task CheckOutdatedAsync(CancellationToken ct)
    {
        try
        {
            var outdated = await WinGetManager.ListOutdatedAsync(ct);
            _outdatedNames.Clear();
            foreach (var entry in outdated)
                _outdatedNames.Add(entry);
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

    private async Task SearchAsync()
    {
        string query = _txtName.Text.Trim();
        if (query.Length == 0)
            return;
        SetBusy(true, $"Searching for '{query}'...");
        AppendLog($"Searching: {query}");
        try
        {
            var list = await WinGetManager.SearchAsync(query, _cts.Token);
            _lstInstalled.Items.Clear();
            foreach (var entry in list)
            {
                var item = new ListViewItem(entry) { Tag = entry };
                item.SubItems.Add("");
                item.SubItems.Add(_installedNames.Contains(entry) ? "\u2714 Installed" : "");
                item.ForeColor = AppTheme.Fg;
                _lstInstalled.Items.Add(item);
            }
            SetStatus($"Search: {list.Count} result(s) for '{query}'", AppTheme.Accent);
            AppendLog($"Search returned {list.Count} result(s).", AppTheme.Accent);
        }
        catch (Exception ex)
        {
            SetStatus($"Error: {ex.Message}", AppTheme.Red);
            AppendLog($"Error: {ex.Message}", AppTheme.Red);
        }
        finally
        {
            SetBusy(false);
        }
    }

    protected override Task InstallCoreAsync(string name, CancellationToken ct) => WinGetManager.InstallAsync(name, ct);

    protected override Task RemoveCoreAsync(string name, CancellationToken ct) => WinGetManager.UninstallAsync(name, ct);

    protected override Task UpgradeCoreAsync(string name, CancellationToken ct) => WinGetManager.UpgradeAsync(name, ct);
}
