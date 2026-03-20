---
name: gui-themes
description: "Add, modify, or debug GUI themes in RegiLattice. Use when adding a new colour theme, changing theme colours, fixing theme application on controls, or understanding the ThemeDef record and Theme engine. Triggers on: 'theme', 'colour', 'dark mode', 'ThemeDef', 'Theme.cs', 'catppuccin', 'add theme', 'style'."
argument-hint: "Theme to add or modify (e.g. 'add Gruvbox Light theme')"
---

# GUI Themes — RegiLattice

## Architecture

All theme logic is in `src/RegiLattice.GUI/Theme.cs`:

```csharp
// ThemeDef record — immutable colour palette
public record ThemeDef(
    string Name,
    Color Background,    // Window/panel background
    Color Surface,       // Card/control background (slightly lighter/darker)
    Color Foreground,    // Primary text
    Color Accent,        // Highlighted items, buttons
    Color AccentHover,   // Button hover state
    Color AccentText,    // Text rendered ON accent colour
    Color Muted,         // Secondary/disabled text
    Color Border,        // Separators and borders
    Color Success,       // Applied/green state
    Color Warning,       // Warning/yellow state
    Color Error,         // Error/red state
    bool  IsDark         // True for dark themes
);
```

## Current 11 Themes

| Name | Style | Key Accent |
|------|-------|-----------|
| Catppuccin Mocha | Dark | Lavender `#CBA6F7` |
| Catppuccin Latte | Light | Lavender `#7287FD` |
| Nord | Dark | Blue `#88C0D0` |
| Dracula | Dark | Purple `#BD93F9` |
| Tokyo Night | Dark | Blue `#7AA2F7` |
| Gruvbox Dark | Dark | Orange `#FE8019` |
| Solarized Dark | Dark | Blue `#268BD2` |
| One Dark Pro | Dark | Blue `#61AFEF` |
| Rosé Pine | Dark | Gold `#F6C177` |
| Everforest | Dark | Green `#A7C080` |
| Cyberpunk | Dark | Cyan `#00F5FF` |

## Adding a New Theme

### 1. Define the ThemeDef in `Theme.cs`
Look for the `_themes` static list and add a new entry:

```csharp
new ThemeDef(
    Name:        "My New Theme",
    Background:  Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E),   // deep background
    Surface:     Color.FromArgb(0xFF, 0x28, 0x28, 0x3D),   // card/panel surface
    Foreground:  Color.FromArgb(0xFF, 0xCD, 0xD6, 0xF4),   // primary text
    Accent:      Color.FromArgb(0xFF, 0x89, 0xB4, 0xFA),   // accent/button
    AccentHover: Color.FromArgb(0xFF, 0x74, 0xC7, 0xEC),   // hover
    AccentText:  Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E),   // text on accent
    Muted:       Color.FromArgb(0xFF, 0x6C, 0x70, 0x86),   // dimmed text
    Border:      Color.FromArgb(0xFF, 0x45, 0x47, 0x5A),   // borders
    Success:     Color.FromArgb(0xFF, 0xA6, 0xE3, 0xA1),   // green/applied
    Warning:     Color.FromArgb(0xFF, 0xF9, 0xE2, 0xAF),   // yellow
    Error:       Color.FromArgb(0xFF, 0xF3, 0x8B, 0xA8),   // red
    IsDark:      true
),
```

### 2. Register in the theme list
The `_themes` list is returned by `AvailableThemes()`. No extra registration needed — just add to the list.

### 3. Write a test in `tests/RegiLattice.GUI.Tests/ThemeTests.cs`
```csharp
[Fact]
public void Theme_MyNewTheme_HasRequiredProperties()
{
    var theme = Theme.GetTheme("My New Theme");
    Assert.NotNull(theme);
    Assert.True(theme.IsDark);
    Assert.Equal("My New Theme", theme.Name);
    // Verify key colours are not default/empty
    Assert.NotEqual(Color.Empty, theme.Background);
    Assert.NotEqual(theme.Background, theme.Surface);  // surface must differ from bg
}
```

## ApplyTheme — How Themes Are Applied

`Theme.ApplyTheme(Control)` recursively walks the control tree:
- Sets `BackColor` / `ForeColor` on panels, labels, buttons, listviews
- Special handling for `RichTextBox` (BackColor must be set AFTER ReadOnly to avoid Windows override)
- `MainForm` re-applies after any dynamic control creation

## System Theme Detection

```csharp
// On startup — follow Windows dark/light preference
Theme.SetTheme(Theme.DetectSystemTheme());
```

`DetectSystemTheme()` reads `HKCU\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize\AppsUseLightTheme` and returns `"Catppuccin Mocha"` (dark) or `"Catppuccin Latte"` (light).

## Theme Persistence

Selected theme name is saved in `AppConfig.Theme` → `%LOCALAPPDATA%\RegiLattice\config.json`.
