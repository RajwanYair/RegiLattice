#nullable enable

using System;
using System.Drawing;
using System.Windows.Forms;
using RegiLattice.Core;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Base class for all non-main-window dialogs.
/// Applies common settings: border style, centering, taskbar hiding,
/// maximize/minimize box suppression, and an optional app icon.
/// Supports both embedded (shown from MainForm) and standalone (launched via --tool) modes.
/// </summary>
internal abstract class BaseDialog : Form
{
    // Tracks whether this dialog was started in standalone mode
    // (Application.Run called on it directly, not shown from a parent form).
#pragma warning disable CS0414 // assigned but never read — consumed by EnableStandaloneMode
    private bool _standaloneMode;
#pragma warning restore CS0414

    /// <summary>
    /// Initialises common dialog properties.
    /// </summary>
    /// <param name="title">Form title shown in the title bar.</param>
    /// <param name="size">Initial client area size.</param>
    /// <param name="resizable">
    ///   When <c>true</c> the border is <see cref="FormBorderStyle.Sizable"/>;
    ///   otherwise <see cref="FormBorderStyle.FixedDialog"/>.
    /// </param>
    protected BaseDialog(string title, Size size, bool resizable = false)
    {
        Text = title;
        Size = size;
        FormBorderStyle = resizable ? FormBorderStyle.Sizable : FormBorderStyle.FixedDialog;
        MaximizeBox = resizable;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        ShowInTaskbar = false;
        Icon = AppIcons.AppIcon;
    }

    /// <summary>
    /// Configures this dialog for standalone mode: shows in taskbar,
    /// allows the window state to be minimised, and applies the current theme.
    /// Call this before <see cref="Application.Run(Form)"/>.
    /// </summary>
    internal void EnableStandaloneMode()
    {
        _standaloneMode = true;
        ShowInTaskbar = true;
        MinimizeBox = true;
        AppTheme.Apply(this);
    }

    /// <summary>
    /// Apply 3D depth effects after all derived-class controls have been added.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        AppTheme.Apply3D(this);
        base.OnLoad(e);
    }

    // ── Shared factory helpers ────────────────────────────────────────────────

    /// <summary>Creates a bold, accent-coloured section header label.</summary>
    protected static Label CreateSectionHeader(string text) =>
        new()
        {
            Text = text,
            AutoSize = true,
            Font = AppTheme.Bold,
            ForeColor = AppTheme.Accent,
        };

    /// <summary>Creates a standard single-line label.</summary>
    protected static Label CreateLabel(string text, int width = 0) =>
        new()
        {
            Text = text,
            AutoSize = width == 0,
            Width = width,
        };

    /// <summary>
    /// Creates a <see cref="FlowLayoutPanel"/> configured for left-to-right
    /// button rows with automatic wrapping.
    /// </summary>
    protected static FlowLayoutPanel CreateButtonRow() =>
        new()
        {
            Dock = DockStyle.Bottom,
            Height = 42,
            Padding = new Padding(6, 6, 6, 4),
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
        };

    /// <summary>Creates a standard dialog action button.</summary>
    protected static Button CreateButton(string text, int width = 86) => new() { Text = text, Width = width };

    /// <summary>
    /// Creates a themed admin/elevation banner panel.
    /// Shows a blue info strip explaining that elevation is needed.
    /// </summary>
    /// <param name="message">The message to display in the banner.</param>
    protected static Panel CreateAdminBanner(string message = "Some operations require administrator privileges.")
    {
        var banner = new Panel
        {
            Dock = DockStyle.Top,
            Height = 28,
            BackColor = Color.FromArgb(50, 120, 200),
        };
        var lbl = new Label
        {
            Dock = DockStyle.Fill,
            Text = "\uD83D\uDEE1 " + message,
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter,
            Padding = new Padding(6, 0, 0, 0),
        };
        banner.Controls.Add(lbl);
        return banner;
    }

    /// <summary>
    /// Creates a warning banner panel (amber).
    /// </summary>
    protected static Panel CreateWarningBanner(string message)
    {
        var banner = new Panel
        {
            Dock = DockStyle.Top,
            Height = 28,
            BackColor = Color.FromArgb(180, 120, 0),
        };
        var lbl = new Label
        {
            Dock = DockStyle.Fill,
            Text = "\u26A0 " + message,
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter,
            Padding = new Padding(6, 0, 0, 0),
        };
        banner.Controls.Add(lbl);
        return banner;
    }
}
