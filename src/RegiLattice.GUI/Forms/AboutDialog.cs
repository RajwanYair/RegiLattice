using RegiLattice.Core;

namespace RegiLattice.GUI.Forms;

/// <summary>About dialog showing version, system info, and keyboard shortcuts.</summary>
internal sealed class AboutDialog : Form
{
    internal AboutDialog(int tweakCount, int categoryCount, bool isCorporate)
    {
        Text = "About RegiLattice";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(560, 620);
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        Font = AppTheme.Regular;

        string version = typeof(TweakEngine).Assembly.GetName().Version?.ToString() ?? "2.0.0";
        string osVer = Environment.OSVersion.ToString();
        string machineName = Environment.MachineName;
        string userName = Environment.UserName;
        string corpStatus = isCorporate ? "Yes (corp-unsafe tweaks blocked)" : "No";
        bool isAdmin = Elevation.IsAdmin();
        string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RegiLattice", "regilattice.log");

        // ── Title ──────────────────────────────────────────────────────────
        var lblTitle = new Label
        {
            Text = "About RegiLattice",
            Font = AppTheme.Title,
            ForeColor = AppTheme.Accent,
            Location = new Point(16, 12),
            AutoSize = true,
        };

        // ── Info box ───────────────────────────────────────────────────────
        string info = $"""
            Version     : {version}
            Tweaks      : {tweakCount} across {categoryCount} categories
            Theme       : {AppTheme.CurrentThemeName()}
            Runtime     : .NET {Environment.Version}

            Platform    : {osVer}
            Build       : {TweakEngine.WindowsBuild()}
            Machine     : {machineName}  ({userName})
            Admin       : {(isAdmin ? "Yes" : "No")}
            Corporate   : {corpStatus}
            Log         : {logPath}
            """;

        var txtInfo = new RichTextBox
        {
            ReadOnly = true,
            BorderStyle = BorderStyle.None,
            BackColor = AppTheme.Surface,
            ForeColor = AppTheme.Fg,
            Font = AppTheme.Mono,
            Text = info,
            Location = new Point(16, 48),
            Size = new Size(528, 180),
            ScrollBars = RichTextBoxScrollBars.Vertical,
        };

        // ── Hardware info ──────────────────────────────────────────────────
        var lblHardware = new Label
        {
            Text = "Hardware",
            Font = AppTheme.Bold,
            ForeColor = AppTheme.Accent,
            Location = new Point(16, 238),
            AutoSize = true,
        };

        string hwInfo;
        try
        {
            hwInfo = HardwareInfo.Summary();
        }
        catch
        {
            hwInfo = "Hardware detection unavailable.";
        }

        var txtHardware = new RichTextBox
        {
            ReadOnly = true,
            BorderStyle = BorderStyle.None,
            BackColor = AppTheme.Surface,
            ForeColor = AppTheme.Fg,
            Font = AppTheme.Mono,
            Text = hwInfo,
            Location = new Point(16, 262),
            Size = new Size(528, 40),
        };

        // ── Shortcuts ──────────────────────────────────────────────────────
        var lblShortcuts = new Label
        {
            Text = "Keyboard Shortcuts",
            Font = AppTheme.Bold,
            ForeColor = AppTheme.Accent,
            Location = new Point(16, 312),
            AutoSize = true,
        };

        string shortcuts = """
            Ctrl+Enter  Apply selected tweaks
            Ctrl+Del    Remove selected tweaks
            F5          Refresh status
            Ctrl+F      Focus search bar
            Esc         Clear search
            Ctrl+A      Select all in current view
            Ctrl+D      Deselect all
            Ctrl+I      Invert selection
            Ctrl+L      Toggle log panel
            Ctrl+E      Expand all categories
            Ctrl+Shift+E  Export selected as PowerShell script
            Ctrl+Shift+J  Export selected IDs as JSON
            Ctrl+Shift+I  Import tweak IDs from JSON
            """;

        var txtShortcuts = new RichTextBox
        {
            ReadOnly = true,
            BorderStyle = BorderStyle.None,
            BackColor = AppTheme.Surface,
            ForeColor = AppTheme.Fg,
            Font = AppTheme.Mono,
            Text = shortcuts,
            Location = new Point(16, 336),
            Size = new Size(528, 230),
        };

        // ── OK button ──────────────────────────────────────────────────────
        var btnOk = AppTheme.StyledButton("OK", AppTheme.Accent, AppTheme.Bg, (_, _) => Close());
        btnOk.Location = new Point(228, 578);
        btnOk.Size = new Size(100, 30);
        AcceptButton = btnOk;

        Controls.AddRange([lblTitle, txtInfo, lblHardware, txtHardware, lblShortcuts, txtShortcuts, btnOk]);
    }
}
