namespace RegiLattice.GUI.Forms;

using System.Reflection;
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
        Icon = AppIcons.WhatsNewIcon;
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(580, 500);
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        Font = AppTheme.Regular;

        string version = GetCurrentVersion();

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
        AppTheme.Apply3D(this);

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

    /// <summary>Returns the assembly's informational version (e.g. "3.7.0").</summary>
    private static string GetCurrentVersion()
    {
        var asm = typeof(TweakEngine).Assembly;
        return asm.GetCustomAttribute<System.Reflection.AssemblyInformationalVersionAttribute>()?.InformationalVersion
            ?? asm.GetName().Version?.ToString(3)
            ?? "3.7.0";
    }

    /// <summary>Checks whether the dialog should be shown (version changed since last seen).</summary>
    internal static bool ShouldShow()
    {
        var cfg = AppConfig.Load();
        string current = GetCurrentVersion();
        return !string.Equals(cfg.LastSeenVersion, current, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Marks the current version as seen so the dialog won't auto-show again.</summary>
    internal static void MarkSeen()
    {
        var cfg = AppConfig.Load();
        cfg.LastSeenVersion = GetCurrentVersion();
        cfg.Save();
    }

    private static string BuildChangelogText(string version)
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        int tweakCount = engine.AllTweaks().Count;
        int categoryCount = engine.Categories().Count;

        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"RegiLattice v{version}");
        sb.AppendLine(new string('─', 50));
        sb.AppendLine();
        sb.AppendLine($"✨ What's new in v{version}:");
        sb.AppendLine("  • Codebase consolidation: 552 mechanical policy modules merged into");
        sb.AppendLine("    24 domain-grouped Policy files — 665 → 130 modules (−81%)");
        sb.AppendLine("  • Category count streamlined: 637 → 135 categories (−79%)");
        sb.AppendLine("  • All 9,190 tweaks retained — zero content removed");
        sb.AppendLine("  • Proxy-aware update checker — respects system and env-var proxy settings");
        sb.AppendLine("  • Package manager dialogs now show current version → new version for updates");
        sb.AppendLine("  • Temp File Cleaner covers 10 additional app / OS cache locations");
        sb.AppendLine("  • Profile comparison table now auto-runs on open");
        sb.AppendLine("  • Dependency Graph Explorer layout fixed (button panel overlap resolved)");
        sb.AppendLine("  • Profile Recommendation Wizard result screen spacing improved");
        sb.AppendLine();
        sb.AppendLine("📊 Stats:");
        sb.AppendLine($"  • Total tweaks:  {tweakCount:N0}");
        sb.AppendLine($"  • Categories:    {categoryCount}");
        sb.AppendLine("  • Module files:  130");
        sb.AppendLine("  • Themes:        11");
        sb.AppendLine("  • Tests:         2,934");
        sb.AppendLine();
        sb.AppendLine("🔧 Bug fixes:");
        sb.AppendLine("  • Update check now honours machine proxy (corporate networks)");
        sb.AppendLine("  • Profile Comparison: comparison auto-runs when dialog opens");
        sb.AppendLine("  • Dependency Graph: Close button and detail pane no longer overlap the tree");
        sb.AppendLine("  • Profile Wizard: result screen no longer clipped against the header");
        sb.AppendLine();
        sb.AppendLine("💡 Tip: Press Ctrl+F to search tweaks, use Tools menu for system utilities.");
        return sb.ToString();
    }
}
