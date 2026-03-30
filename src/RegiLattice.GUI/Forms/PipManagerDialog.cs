// RegiLattice.GUI — Forms/PipManagerDialog.cs
using RegiLattice.GUI.PackageManagers;
using Elevation = RegiLattice.Core.Elevation;

namespace RegiLattice.GUI.Forms;

/// <summary>pip (Python) package manager dialog — extends BasePackageManagerDialog.</summary>
internal sealed class PipManagerDialog : BasePackageManagerDialog
{
    private readonly ComboBox _cmbScope = new();
    private readonly ComboBox _cmbPython = new();
    private readonly Label _lblPythonStatus = new();

    // Maps display label → full exe path (or bare name for PATH-resolved entries)
    private readonly Dictionary<string, string> _pythonExes = new(StringComparer.OrdinalIgnoreCase);

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
            new ColumnHeader { Text = "Size", Width = 80 },
            new ColumnHeader { Text = "Status", Width = 140 },
        ];

    protected override Control? BuildScopePanel()
    {
        // Two-row panel: row 0 = Python selector, row 1 = scope selector
        var panel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 66,
            BackColor = AppTheme.Surface,
        };

        // ── Row 0: Python interpreter selector ──────────────────────────
        var lblPy = new Label
        {
            Text = "Python:",
            AutoSize = true,
            Location = new Point(8, 8),
            ForeColor = AppTheme.Fg,
        };
        _cmbPython.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbPython.BackColor = AppTheme.Overlay;
        _cmbPython.ForeColor = AppTheme.Fg;
        _cmbPython.FlatStyle = FlatStyle.Flat;
        _cmbPython.Location = new Point(62, 5);
        _cmbPython.Width = 300;
        _cmbPython.SelectedIndexChanged += async (_, _) => await RefreshAsync();

        _lblPythonStatus.AutoSize = true;
        _lblPythonStatus.Location = new Point(372, 8);
        _lblPythonStatus.ForeColor = AppTheme.FgDim;
        _lblPythonStatus.Text = "Scanning...";

        // ── Row 1: Scope selector ────────────────────────────────────────
        var lblScope = new Label
        {
            Text = "Scope:",
            AutoSize = true,
            Location = new Point(8, 40),
            ForeColor = AppTheme.Fg,
        };
        _cmbScope.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbScope.Items.AddRange(["All packages", "User-level only"]);
        _cmbScope.SelectedIndex = 0;
        _cmbScope.BackColor = AppTheme.Overlay;
        _cmbScope.ForeColor = AppTheme.Fg;
        _cmbScope.FlatStyle = FlatStyle.Flat;
        _cmbScope.Location = new Point(62, 37);
        _cmbScope.Width = 130;
        _cmbScope.SelectedIndexChanged += async (_, _) => await RefreshAsync();

        panel.Controls.AddRange([lblPy, _cmbPython, _lblPythonStatus, lblScope, _cmbScope]);

        // Populate the python combo asynchronously so the dialog opens fast
        _ = PopulatePythonComboAsync();

        return panel;
    }

    private async Task PopulatePythonComboAsync()
    {
        var pythons = await Task.Run(PipManager.FindAllPythons).ConfigureAwait(false);

        if (InvokeRequired)
            Invoke(() => ApplyPythonList(pythons));
        else
            ApplyPythonList(pythons);
    }

    private void ApplyPythonList(IReadOnlyList<string> pythons)
    {
        _pythonExes.Clear();
        _cmbPython.Items.Clear();

        if (pythons.Count == 0)
        {
            _cmbPython.Items.Add("No Python found");
            _cmbPython.SelectedIndex = 0;
            _lblPythonStatus.Text = "no Python with pip found";
            _lblPythonStatus.ForeColor = AppTheme.Red;
            return;
        }

        foreach (string exe in pythons)
        {
            string label = BuildPythonLabel(exe);
            _pythonExes[label] = exe;
            _cmbPython.Items.Add(label);
        }
        _cmbPython.SelectedIndex = 0;
        _lblPythonStatus.Text = $"{pythons.Count} interpreter(s) found";
        _lblPythonStatus.ForeColor = AppTheme.Green;
    }

    /// <summary>Returns a short human-readable label for a python.exe path.</summary>
    private static string BuildPythonLabel(string exePath)
    {
        if (!exePath.Contains('\\') && !exePath.Contains('/'))
            return exePath; // bare name (PATH-resolved)

        string dir = Path.GetDirectoryName(exePath) ?? exePath;

        // Detect scope: user vs system
        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string progFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string? progFilesX86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)");

        string scope;
        if (exePath.StartsWith(localAppData, StringComparison.OrdinalIgnoreCase)
            || exePath.StartsWith(userProfile, StringComparison.OrdinalIgnoreCase))
            scope = "user";
        else if (exePath.StartsWith(progFiles, StringComparison.OrdinalIgnoreCase)
                 || (!string.IsNullOrEmpty(progFilesX86) && exePath.StartsWith(progFilesX86, StringComparison.OrdinalIgnoreCase)))
            scope = "system";
        else
            scope = "";

        // Extract a version segment from the path if it contains "Python3" or similar
        string folderName = Path.GetFileName(dir) ?? dir;
        string versionHint = string.Empty;
        if (folderName.StartsWith("Python", StringComparison.OrdinalIgnoreCase) && folderName.Length > 6)
        {
            // e.g. Python312 → 3.12, Python310 → 3.10, Python39 → 3.9
            string digits = folderName[6..];
            if (digits.Length == 3 && digits.All(char.IsDigit))
                versionHint = $"{digits[0]}.{digits[1..]}"; // 312 → 3.12
            else if (digits.Length == 2 && digits.All(char.IsDigit))
                versionHint = $"{digits[0]}.{digits[1]}";   // 39 → 3.9
        }

        string display = string.IsNullOrEmpty(versionHint) ? exePath : $"Python {versionHint}";
        return string.IsNullOrEmpty(scope) ? $"{display}  ({exePath})" : $"{display}  [{scope}]  ({exePath})";
    }

    /// <summary>Returns the currently selected Python executable, or falls back to FindPython().</summary>
    private string GetSelectedPython()
    {
        string? label = _cmbPython.SelectedItem?.ToString();
        if (label is not null && _pythonExes.TryGetValue(label, out string? exe))
            return exe;
        return PipManager.FindPython() ?? "python";
    }

    protected override async Task RefreshCoreAsync(CancellationToken ct)
    {
        bool userOnly = _cmbScope.SelectedIndex == 1;
        string python = GetSelectedPython();
        var list = await PipManager.ListInstalledAsync(userOnly, python, ct);
        _installedNames = await PipManager.ListInstalledNamesAsync(python, ct);
        _lstInstalled.Items.Clear();
        foreach (var entry in list)
        {
            int paren = entry.IndexOf(" (", StringComparison.Ordinal);
            string name = paren > 0 ? entry[..paren] : entry;
            string version = paren > 0 ? entry[(paren + 2)..].TrimEnd(')') : "";
            var item = new ListViewItem(name) { Tag = name };
            item.SubItems.Add(version);            // Version
            item.SubItems.Add("—");               // Size
            item.SubItems.Add("\u2714 Up to date"); // Status
            item.ForeColor = AppTheme.Fg;
            _lstInstalled.Items.Add(item);
        }
        string scopeLabel = userOnly ? "user-level" : "all";
        SetStatus($"pip: {list.Count} {scopeLabel} package(s)", AppTheme.Green);
        AppendLog($"Found {list.Count} {scopeLabel} package(s) [{python}].", AppTheme.Green);
        RebuildQuickInstallButtons();
        _ = CheckOutdatedAsync(python, ct);
    }

    private async Task CheckOutdatedAsync(string python, CancellationToken ct)
    {
        try
        {
            var outdated = await PipManager.ListOutdatedAsync(python, ct);
            var outdatedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var versionMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var entry in outdated)
            {
                int paren = entry.IndexOf(" (", StringComparison.Ordinal);
                string name = paren > 0 ? entry[..paren] : entry;
                outdatedNames.Add(name);
                if (paren > 0)
                    versionMap[name] = entry[(paren + 2)..].TrimEnd(')');
            }
            foreach (ListViewItem item in _lstInstalled.Items)
            {
                if (item.Tag is string pkgName && outdatedNames.Contains(pkgName))
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

    protected override Task InstallCoreAsync(string name, CancellationToken ct) => PipManager.InstallAsync(name, GetSelectedPython(), ct);

    protected override Task RemoveCoreAsync(string name, CancellationToken ct) => PipManager.UninstallAsync(name, GetSelectedPython(), ct);

    protected override Task UpgradeCoreAsync(string name, CancellationToken ct) => PipManager.UpgradeAsync(name, GetSelectedPython(), ct);
}
