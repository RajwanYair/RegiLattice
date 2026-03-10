namespace RegiLattice.Native.Forms;

/// <summary>About dialog showing version, system info, and keyboard shortcuts.</summary>
internal sealed class AboutDialog : Form
{
    internal AboutDialog(int tweakCount, int categoryCount, string pythonPath, bool isCorporate)
    {
        Text            = "About RegiLattice";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        StartPosition   = FormStartPosition.CenterParent;
        ClientSize      = new Size(560, 500);
        BackColor       = AppTheme.Bg;
        ForeColor       = AppTheme.Fg;
        Font            = AppTheme.Regular;

        string version       = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.2";
        string osVer         = Environment.OSVersion.ToString();
        string machineName   = Environment.MachineName;
        string userName      = Environment.UserName;
        string corpStatus    = isCorporate ? "Yes (corp-unsafe tweaks blocked)" : "No";
        string logPath       = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "RegiLattice", "regilattice.log");

        // ── Title ──────────────────────────────────────────────────────────
        var lblTitle = new Label
        {
            Text      = "RegiLattice — Native GUI",
            Font      = AppTheme.Title,
            ForeColor = AppTheme.Accent,
            Location  = new Point(16, 12),
            AutoSize  = true,
        };

        // ── Info box ───────────────────────────────────────────────────────
        string info = $"""
            Version     : {version}
            Tweaks      : {tweakCount} across {categoryCount} categories
            
            Platform    : {osVer}
            Machine     : {machineName}  ({userName})
            Corporate   : {corpStatus}
            Python      : {pythonPath}
            Log         : {logPath}
            """;

        var txtInfo = new RichTextBox
        {
            ReadOnly   = true,
            BorderStyle = BorderStyle.None,
            BackColor  = AppTheme.Surface,
            ForeColor  = AppTheme.Fg,
            Font       = AppTheme.Mono,
            Text       = info,
            Location   = new Point(16, 48),
            Size       = new Size(528, 155),
            ScrollBars = RichTextBoxScrollBars.Vertical,
        };

        // ── Shortcuts ─────────────────────────────────────────────────────
        var lblShortcuts = new Label
        {
            Text      = "Keyboard Shortcuts",
            Font      = AppTheme.Bold,
            ForeColor = AppTheme.Accent,
            Location  = new Point(16, 216),
            AutoSize  = true,
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
            ReadOnly    = true,
            BorderStyle = BorderStyle.None,
            BackColor   = AppTheme.Surface,
            ForeColor   = AppTheme.Fg,
            Font        = AppTheme.Mono,
            Text        = shortcuts,
            Location    = new Point(16, 240),
            Size        = new Size(528, 200),
        };

        // ── OK button ──────────────────────────────────────────────────────
        var btnOk = AppTheme.StyledButton("OK", AppTheme.Accent, AppTheme.Bg, (_, _) => Close());
        btnOk.Location = new Point(228, 454);
        btnOk.Size     = new Size(100, 30);
        AcceptButton   = btnOk;

        Controls.AddRange([lblTitle, txtInfo, lblShortcuts, txtShortcuts, btnOk]);
    }
}
