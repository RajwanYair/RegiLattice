#nullable enable

using System;
using System.Drawing;
using System.Windows.Forms;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Base class for all non-main-window dialogs.
/// Applies common settings: border style, centering, taskbar hiding,
/// maximize/minimize box suppression, and an optional app icon.
/// </summary>
internal abstract class BaseDialog : Form
{
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
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        Icon = AppIcons.AppIcon;
    }

    // ── Shared factory helpers ────────────────────────────────────────────────

    /// <summary>Creates a bold section header label.</summary>
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
}
