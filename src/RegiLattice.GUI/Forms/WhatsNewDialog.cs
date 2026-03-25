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
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"RegiLattice v{version}");
        sb.AppendLine(new string('─', 50));
        sb.AppendLine();
        sb.AppendLine($"✨ New in v{version}:");
        sb.AppendLine("  • 800+ new Group Policy tweaks across 80 new policy modules");
        sb.AppendLine("  • New modules: AppReadiness, DataSense, PageFile, VolumeShadowCopy,");
        sb.AppendLine("    RestartManager, SharedPC, NetworkList, SensorService, Telephony,");
        sb.AppendLine("    PrintManagement, AppContainer, NetworkQoS, DNS Secure, PortableDevices,");
        sb.AppendLine("    FontProvider, AppXPackaging, DataIntegrity, NTFS, CertValidation,");
        sb.AppendLine("    AdhocNetwork, PrinterGPO, RPC, Licensing, WindowsContainers, Flighting,");
        sb.AppendLine("    CapabilityAccess, DDE, NetworkAdapter, IPv6, EventTracing, Processor,");
        sb.AppendLine("    CodeSigning, TrustProvider, SMBEncryption, DomainIsolation, CacheManager,");
        sb.AppendLine("    ObjectAccess, StoragePool, FileShare, NetworkProfile, WLAN, AppxBundle,");
        sb.AppendLine("    DeploymentServices, LegacyAuth, AppGuard, KioskBrowser, DeviceEnrollment,");
        sb.AppendLine("    MemoryIntegrity, AuditEvent, UserRights, Compartment, ServiceAccount,");
        sb.AppendLine("    SecureChannel, CredentialManager, PrintSpoolAdv, NetBIOS, WindowsHelloAdv,");
        sb.AppendLine("    ActiveSetup, CBSUpdate, DesktopAnalytics, TPMAdvanced, AppSilo,");
        sb.AppendLine("    LockdownBrowsing, RemoteCredentialGuard, CrashDumps, EnterpriseResource,");
        sb.AppendLine("    NetCfg, SecureConnections, WindowsPerformance, HolographicDevice,");
        sb.AppendLine("    Virtualization, TokenPrivilege, CloudPrint, WindowsSandbox and more");
        sb.AppendLine("  • Fixed: font crash on startup when non-default font size was saved");
        sb.AppendLine();
        sb.AppendLine("📊 Stats:");
        sb.AppendLine("  • Total tweaks: 6,875+");
        sb.AppendLine("  • Categories: 400+");
        sb.AppendLine("  • Themes: 11");
        sb.AppendLine("  • Tests: 2,063+");
        sb.AppendLine();
        sb.AppendLine("🔧 Recent Fixes:");
        sb.AppendLine("  • ApplyTheme() now correctly updates font on TreeView and ListView");
        sb.AppendLine("    after dynamic font-size changes (prevented startup crash)");
        sb.AppendLine("  • What's New dialog now reflects actual current version");
        sb.AppendLine();
        sb.AppendLine("💡 Tip: Press Ctrl+F to search tweaks, use Tools menu for system utilities.");
        return sb.ToString();
    }
}
