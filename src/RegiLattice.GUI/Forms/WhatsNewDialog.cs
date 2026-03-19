namespace RegiLattice.GUI.Forms;

using RegiLattice.Core;
using RegiLattice.Core.Services;

/// <summary>
/// Shows a "What's New" dialog summarising the latest version changes.
/// Reads the embedded changelog section for the current version.
/// Can be triggered from Help menu or automatically on first launch after upgrade.
/// </summary>
internal sealed class WhatsNewDialog : Form
{
    private const string LastSeenKey = "LastSeenVersion";

    internal WhatsNewDialog()
    {
        Text = "What's New in RegiLattice";
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(580, 500);
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        Font = AppTheme.Regular;

        string version = typeof(TweakEngine).Assembly.GetName().Version?.ToString() ?? "3.5.0";

        // ── Title ──────────────────────────────────────────────────────────
        var lblTitle = new Label
        {
            Text = $"\U0001F389  What's New — v{version}",
            Font = AppTheme.Title,
            ForeColor = AppTheme.Accent,
            AutoSize = false,
            Dock = DockStyle.Top,
            Height = 50,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(12, 0, 0, 0),
        };

        // ── Changelog content ──────────────────────────────────────────────
        var rtb = new RichTextBox
        {
            ReadOnly = true,
            BorderStyle = BorderStyle.None,
            BackColor = AppTheme.Surface,
            ForeColor = AppTheme.Fg,
            Font = AppTheme.Regular,
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
            ScrollBars = RichTextBoxScrollBars.Vertical,
        };
        rtb.Text = BuildChangelogText(version);

        // ── Footer ─────────────────────────────────────────────────────────
        var btnOk = new Button
        {
            Text = "Got it!",
            DialogResult = DialogResult.OK,
            BackColor = AppTheme.Accent,
            ForeColor = AppTheme.Bg,
            FlatStyle = FlatStyle.Flat,
            Font = AppTheme.Bold,
            Size = new Size(120, 36),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
        };
        btnOk.FlatAppearance.BorderSize = 0;

        var chkDontShow = new CheckBox
        {
            Text = "Don't show this automatically",
            AutoSize = true,
            ForeColor = AppTheme.FgDim,
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
            Location = new Point(16, 460),
        };

        var pnlFooter = new Panel
        {
            Height = 52,
            Dock = DockStyle.Bottom,
            BackColor = AppTheme.Bg,
        };
        btnOk.Location = new Point(ClientSize.Width - 140, 8);
        pnlFooter.Controls.Add(btnOk);
        pnlFooter.Controls.Add(chkDontShow);

        Controls.Add(rtb);
        Controls.Add(pnlFooter);
        Controls.Add(lblTitle);

        AcceptButton = btnOk;

        FormClosed += (_, _) =>
        {
            if (chkDontShow.Checked)
            {
                var cfg = AppConfig.Load();
                cfg.LastSeenVersion = version;
                cfg.Save();
            }
        };
    }

    /// <summary>Checks whether the dialog should be shown (version changed since last seen).</summary>
    internal static bool ShouldShow()
    {
        var cfg = AppConfig.Load();
        string current = typeof(TweakEngine).Assembly.GetName().Version?.ToString() ?? "3.5.0";
        return !string.Equals(cfg.LastSeenVersion, current, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Marks the current version as seen so the dialog won't auto-show again.</summary>
    internal static void MarkSeen()
    {
        var cfg = AppConfig.Load();
        cfg.LastSeenVersion = typeof(TweakEngine).Assembly.GetName().Version?.ToString() ?? "3.5.0";
        cfg.Save();
    }

    private static string BuildChangelogText(string version)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"RegiLattice v{version}");
        sb.AppendLine(new string('─', 50));
        sb.AppendLine();
        sb.AppendLine("✨ New Features:");
        sb.AppendLine("  • Network Tools — DNS quick-switch (Cloudflare, Google, Quad9), TCP/IP + Winsock reset");
        sb.AppendLine("  • Startup Manager — view and toggle HKCU/HKLM Run entries and Startup folders");
        sb.AppendLine("  • Service Manager — searchable service list with Start/Stop/Enable/Disable controls");
        sb.AppendLine("  • Preferences dialog — font size, accent colour, log panel, tray behaviour");
        sb.AppendLine();
        sb.AppendLine("📊 Stats:");
        sb.AppendLine("  • Total tweaks: 2,736");
        sb.AppendLine("  • Categories: 92");
        sb.AppendLine("  • Themes: 11");
        sb.AppendLine("  • Tests: 1,671");
        sb.AppendLine();
        sb.AppendLine("🔧 Improvements:");
        sb.AppendLine("  • BaseDialog consolidation — all tool dialogs share common layout/icon/chrome");
        sb.AppendLine("  • What's New dialog shown on first launch after upgrade");
        sb.AppendLine("  • InternalsVisibleTo fix — all 1,671 tests now pass reliably");
        sb.AppendLine();
        sb.AppendLine("💡 Tip: Use Tools menu to access Network Tools, Startup Manager, and Service Manager.");
        return sb.ToString();
    }
}
