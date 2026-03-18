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
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(580, 500);
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        Font = AppTheme.Regular;

        string version = typeof(TweakEngine).Assembly.GetName().Version?.ToString() ?? "3.4.0";

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
        string current = typeof(TweakEngine).Assembly.GetName().Version?.ToString() ?? "3.4.0";
        return !string.Equals(cfg.LastSeenVersion, current, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Marks the current version as seen so the dialog won't auto-show again.</summary>
    internal static void MarkSeen()
    {
        var cfg = AppConfig.Load();
        cfg.LastSeenVersion = typeof(TweakEngine).Assembly.GetName().Version?.ToString() ?? "3.4.0";
        cfg.Save();
    }

    private static string BuildChangelogText(string version)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"RegiLattice v{version}");
        sb.AppendLine(new string('─', 50));
        sb.AppendLine();
        sb.AppendLine("✨ New Features:");
        sb.AppendLine("  • Search result highlighting — matched text is bold/accent in ListView");
        sb.AppendLine("  • Recently Applied — virtual category shows your last 50 applied tweaks");
        sb.AppendLine("  • What's New dialog — shown on first launch after version upgrade");
        sb.AppendLine("  • Preferences dialog — 4 tabs for full GUI configuration");
        sb.AppendLine();
        sb.AppendLine("🎨 New Tweak Categories:");
        sb.AppendLine("  • Window Appearance — 51 tweaks for title bars, scrollbars, fonts, icons");
        sb.AppendLine("  • System Optimization — 40 tweaks for memory, I/O, kernel tuning");
        sb.AppendLine("  • Desktop Customization — 37 tweaks for Explorer, taskbar, Start menu");
        sb.AppendLine();
        sb.AppendLine("📊 Stats:");
        sb.AppendLine("  • Total tweaks: 2,700+");
        sb.AppendLine("  • Categories: 92+");
        sb.AppendLine("  • Themes: 11");
        sb.AppendLine("  • Tests: 1,600+");
        sb.AppendLine();
        sb.AppendLine("🔧 Improvements:");
        sb.AppendLine("  • Faster shutdown via reduced hung app and service timeouts");
        sb.AppendLine("  • NTFS optimisations (8.3 names, last access, long paths)");
        sb.AppendLine("  • Explorer performance (separate process, Quick Access control)");
        sb.AppendLine("  • Visual effects granular control (per-effect toggles)");
        sb.AppendLine();
        sb.AppendLine("💡 Tip: Use Ctrl+F to search across all categories.");
        return sb.ToString();
    }
}
