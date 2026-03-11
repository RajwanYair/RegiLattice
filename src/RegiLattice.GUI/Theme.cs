namespace RegiLattice.GUI;

/// <summary>Catppuccin Mocha dark-theme colour and font constants shared across all forms.</summary>
internal static class AppTheme
{
    // ── Colours ────────────────────────────────────────────────────────────
    internal static readonly Color Bg       = Color.FromArgb(30,  30,  46);   // base
    internal static readonly Color Surface  = Color.FromArgb(49,  50,  68);   // surface0
    internal static readonly Color Surface2 = Color.FromArgb(88,  91, 112);   // surface2
    internal static readonly Color Fg       = Color.FromArgb(205, 214, 244);  // text
    internal static readonly Color FgDim    = Color.FromArgb(166, 173, 200);  // subtext1
    internal static readonly Color Accent   = Color.FromArgb(137, 180, 250);  // blue
    internal static readonly Color Green    = Color.FromArgb(166, 227, 161);  // green
    internal static readonly Color Red      = Color.FromArgb(243, 139, 168);  // red
    internal static readonly Color Yellow   = Color.FromArgb(249, 226, 175);  // yellow
    internal static readonly Color Overlay  = Color.FromArgb(69,  71,  90);   // overlay0
    internal static readonly Color Success  = Color.FromArgb(64,  160,  43);  // solid green
    internal static readonly Color Danger   = Color.FromArgb(230,  74,  25);  // solid red
    internal static readonly Color Info     = Color.FromArgb(30,  102, 245);  // solid blue

    // ── Fonts ──────────────────────────────────────────────────────────────
    internal static readonly Font Regular  = new("Segoe UI",  9f);
    internal static readonly Font Small    = new("Segoe UI",  8f);
    internal static readonly Font Bold     = new("Segoe UI",  9f, FontStyle.Bold);
    internal static readonly Font Title    = new("Segoe UI", 12f, FontStyle.Bold);
    internal static readonly Font Mono     = new("Consolas",  9f);

    // ── Helpers ────────────────────────────────────────────────────────────
    /// <summary>Recursively apply dark-theme colours to a control tree.</summary>
    internal static void Apply(Control root)
    {
        root.BackColor = Bg;
        root.ForeColor = Fg;
        root.Font      = Regular;
        foreach (Control c in root.Controls)
            Apply(c);
    }

    /// <summary>Style a Button with the given background colour.</summary>
    internal static Button StyledButton(string text, Color bg, Color fg, EventHandler click)
    {
        var btn = new Button
        {
            Text      = text,
            BackColor = bg,
            ForeColor = fg,
            FlatStyle = FlatStyle.Flat,
            Font      = Bold,
            AutoSize  = true,
            Padding   = new Padding(8, 2, 8, 2),
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.Click += click;
        return btn;
    }

    /// <summary>Create a styled ListBox for use inside package manager dialogs.</summary>
    internal static ListBox StyledListBox()
        => new()
        {
            BackColor           = Surface,
            ForeColor           = Fg,
            Font                = Regular,
            SelectionMode       = SelectionMode.One,
            BorderStyle         = BorderStyle.None,
            HorizontalScrollbar = true,
        };

    /// <summary>Create a dark-themed TextBox.</summary>
    internal static TextBox StyledTextBox(int width = 200)
        => new()
        {
            BackColor   = Overlay,
            ForeColor   = Fg,
            Font        = Regular,
            BorderStyle = BorderStyle.FixedSingle,
            Width       = width,
        };
}
