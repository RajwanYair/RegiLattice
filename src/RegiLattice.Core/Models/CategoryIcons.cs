// RegiLattice.Core — Models/CategoryIcons.cs
// Maps category names to their standard icons.

namespace RegiLattice.Core.Models;

/// <summary>Maps tweak category names to their standard <see cref="CategoryIcon"/>.</summary>
public static class CategoryIcons
{
    private static readonly Dictionary<string, CategoryIcon> Map = new(StringComparer.OrdinalIgnoreCase)
    {
        // Shield — security-related
        ["Security"] = CategoryIcon.Shield,
        ["Defender"] = CategoryIcon.Shield,
        ["Firewall"] = CategoryIcon.Shield,
        ["Encryption"] = CategoryIcon.Shield,

        // Globe — network & browsers
        ["Network"] = CategoryIcon.Globe,
        ["DNS & Networking Advanced"] = CategoryIcon.Globe,
        ["Edge"] = CategoryIcon.Globe,
        ["Chrome"] = CategoryIcon.Globe,
        ["Firefox"] = CategoryIcon.Globe,

        // Monitor — display
        ["Display"] = CategoryIcon.Monitor,
        ["GPU / Graphics"] = CategoryIcon.Monitor,
        ["Night Light & Display"] = CategoryIcon.Monitor,

        // Gear — system internals
        ["System"] = CategoryIcon.Gear,
        ["Boot"] = CategoryIcon.Gear,
        ["Services"] = CategoryIcon.Gear,

        // Lock — privacy
        ["Privacy"] = CategoryIcon.Lock,
        ["Telemetry Advanced"] = CategoryIcon.Lock,
        ["Lock Screen & Login"] = CategoryIcon.Lock,

        // HardDrive — storage
        ["Storage"] = CategoryIcon.HardDrive,
        ["File System"] = CategoryIcon.HardDrive,
        ["Backup & Recovery"] = CategoryIcon.HardDrive,
        ["Recovery"] = CategoryIcon.HardDrive,

        // Cpu — performance & power
        ["Performance"] = CategoryIcon.Cpu,
        ["Power"] = CategoryIcon.Cpu,
        ["Gaming"] = CategoryIcon.Cpu,

        // Keyboard — input
        ["Input"] = CategoryIcon.Keyboard,
        ["Accessibility"] = CategoryIcon.Keyboard,
        ["Touch & Pen"] = CategoryIcon.Keyboard,

        // Speaker — audio
        ["Audio"] = CategoryIcon.Speaker,
        ["Multimedia"] = CategoryIcon.Speaker,
        ["Voice Access & Speech"] = CategoryIcon.Speaker,

        // Cloud — cloud services
        ["Cloud Storage"] = CategoryIcon.Cloud,
        ["OneDrive"] = CategoryIcon.Cloud,

        // App — package management
        ["Microsoft Store"] = CategoryIcon.App,
        ["Package Management"] = CategoryIcon.App,
        ["Scoop Tools"] = CategoryIcon.App,

        // Terminal — dev tools
        ["Shell"] = CategoryIcon.Terminal,
        ["Windows Terminal"] = CategoryIcon.Terminal,
        ["WSL"] = CategoryIcon.Terminal,
        ["Dev Drive / Developer Tools"] = CategoryIcon.Terminal,

        // Mail — communication
        ["Communication"] = CategoryIcon.Mail,
        ["Office"] = CategoryIcon.Mail,
        ["M365 Copilot"] = CategoryIcon.Mail,

        // Palette — UI customization
        ["Fonts"] = CategoryIcon.Palette,
        ["Context Menu"] = CategoryIcon.Palette,
        ["Explorer"] = CategoryIcon.Palette,

        // Notification — alerts
        ["Notifications"] = CategoryIcon.Notification,
        ["Widgets & News"] = CategoryIcon.Notification,

        // Wrench — maintenance
        ["Maintenance"] = CategoryIcon.Wrench,
        ["Scheduled Tasks"] = CategoryIcon.Wrench,
        ["Crash & Diagnostics"] = CategoryIcon.Wrench,

        // Phone — peripherals
        ["Phone Link"] = CategoryIcon.Phone,
        ["Bluetooth"] = CategoryIcon.Phone,
        ["USB & Peripherals"] = CategoryIcon.Phone,

        // Desktop — shell UI
        ["Taskbar"] = CategoryIcon.Desktop,
        ["Snap & Multitasking"] = CategoryIcon.Desktop,
        ["Startup"] = CategoryIcon.Desktop,
        ["Screensaver & Lock"] = CategoryIcon.Desktop,

        // Windows — OS-level
        ["Windows 11"] = CategoryIcon.Windows,
        ["Windows Update"] = CategoryIcon.Windows,

        // Search
        ["Cortana & Search"] = CategoryIcon.Search,
        ["Indexing & Search"] = CategoryIcon.Search,

        // Camera — remote
        ["Remote Desktop"] = CategoryIcon.Camera,
        ["RealVNC"] = CategoryIcon.Camera,
        ["Virtualization"] = CategoryIcon.Camera,

        // Printer
        ["Printing"] = CategoryIcon.Printer,

        // Code — dev apps
        ["VS Code"] = CategoryIcon.Code,
        ["Java"] = CategoryIcon.Code,
        ["LibreOffice"] = CategoryIcon.Code,
        ["Adobe"] = CategoryIcon.Code,

        // AI
        ["AI / Copilot"] = CategoryIcon.Search,
        ["Clipboard & Drag-Drop"] = CategoryIcon.Palette,
    };

    /// <summary>Get the icon for a category name. Returns <see cref="CategoryIcon.Gear"/> as fallback.</summary>
    public static CategoryIcon GetIcon(string category)
        => Map.GetValueOrDefault(category, CategoryIcon.Gear);

    /// <summary>Get the Unicode symbol for a category icon (for CLI/text display).</summary>
    public static string GetSymbol(CategoryIcon icon) => icon switch
    {
        CategoryIcon.Shield => "\U0001F6E1",       // 🛡
        CategoryIcon.Globe => "\U0001F310",         // 🌐
        CategoryIcon.Monitor => "\U0001F5A5",       // 🖥
        CategoryIcon.Gear => "\u2699\uFE0F",        // ⚙️
        CategoryIcon.Lock => "\U0001F512",          // 🔒
        CategoryIcon.HardDrive => "\U0001F4BE",     // 💾
        CategoryIcon.Cpu => "\u26A1",               // ⚡
        CategoryIcon.Keyboard => "\u2328\uFE0F",    // ⌨️
        CategoryIcon.Speaker => "\U0001F50A",       // 🔊
        CategoryIcon.Cloud => "\u2601\uFE0F",       // ☁️
        CategoryIcon.App => "\U0001F4E6",           // 📦
        CategoryIcon.Terminal => "\U0001F4BB",       // 💻
        CategoryIcon.Mail => "\U0001F4E7",          // 📧
        CategoryIcon.Palette => "\U0001F3A8",       // 🎨
        CategoryIcon.Notification => "\U0001F514",  // 🔔
        CategoryIcon.Wrench => "\U0001F527",        // 🔧
        CategoryIcon.Phone => "\U0001F4F1",         // 📱
        CategoryIcon.Desktop => "\U0001F5A5",       // 🖥
        CategoryIcon.Windows => "\U0001FA9F",       // 🪟
        CategoryIcon.Search => "\U0001F50D",        // 🔍
        CategoryIcon.Camera => "\U0001F4F7",        // 📷
        CategoryIcon.Printer => "\U0001F5A8",       // 🖨
        CategoryIcon.Code => "\U0001F4DD",          // 📝
        _ => "\u2699\uFE0F",                        // ⚙️
    };

    /// <summary>Get the Unicode symbol for a tweak kind (Registry/Command/FileConfig).</summary>
    public static string GetKindSymbol(TweakKind kind) => kind switch
    {
        TweakKind.Registry => "\U0001F5C3",   // 🗃 (registry)
        TweakKind.Command => "\u25B6\uFE0F",  // ▶️ (command)
        TweakKind.FileConfig => "\U0001F4C4", // 📄 (file config)
        _ => "\u2699\uFE0F",                  // ⚙️
    };
}
