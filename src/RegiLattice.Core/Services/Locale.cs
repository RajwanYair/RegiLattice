// RegiLattice.Core — Services/Locale.cs
// i18n string table — replaces Python locale.py.

namespace RegiLattice.Core;

/// <summary>Simple string localization with format support.</summary>
public static class Locale
{
    private static string _current = "en";

    private static readonly Dictionary<string, string> En = new(StringComparer.OrdinalIgnoreCase)
    {
        ["apply_all"] = "Apply All",
        ["remove_all"] = "Remove All",
        ["search_placeholder"] = "Search tweaks\u2026",
        ["status_applied"] = "APPLIED",
        ["status_not_applied"] = "DEFAULT",
        ["status_unknown"] = "UNKNOWN",
        ["status_error"] = "ERROR",
        ["status_skipped_corp"] = "SKIPPED (Corp)",
        ["status_skipped_build"] = "SKIPPED (Build)",
        ["filter_all"] = "All",
        ["filter_applied"] = "Applied",
        ["filter_default"] = "Default",
        ["filter_unknown"] = "Unknown",
        ["scope_user"] = "User (HKCU)",
        ["scope_machine"] = "Machine (HKLM)",
        ["scope_both"] = "Both",
        ["scope_all"] = "All Scopes",
        ["profile_business"] = "Business",
        ["profile_gaming"] = "Gaming",
        ["profile_privacy"] = "Privacy",
        ["profile_minimal"] = "Minimal",
        ["profile_server"] = "Server",
        ["btn_apply"] = "Apply Selected",
        ["btn_remove"] = "Remove Selected",
        ["btn_select_all"] = "Select All",
        ["btn_deselect_all"] = "Deselect All",
        ["btn_invert"] = "Invert Selection",
        ["btn_refresh"] = "Refresh Status",
        ["btn_export_ps1"] = "Export PS1",
        ["btn_export_json"] = "Export JSON",
        ["btn_import_json"] = "Import JSON",
        ["menu_file"] = "File",
        ["menu_tools"] = "Tools",
        ["menu_view"] = "View",
        ["menu_help"] = "Help",
        ["about_title"] = "About RegiLattice",
        ["scoop_manager"] = "Scoop Manager",
        ["psmodule_manager"] = "PowerShell Modules",
        ["pip_manager"] = "pip Packages",
        ["log_panel"] = "Toggle Log Panel",
        ["corporate_warning"] = "Corporate environment detected. Some tweaks are blocked.",
        ["admin_required"] = "Administrator privileges required.",
        ["confirm_apply"] = "Apply {0} selected tweaks?",
        ["confirm_remove"] = "Remove {0} selected tweaks?",
        ["tweaks_loaded"] = "{0} tweaks loaded across {1} categories.",
        ["detection_complete"] = "Status detection complete.",
        ["export_complete"] = "Exported to {0}.",
        ["import_complete"] = "Imported {0} tweaks from {1}.",
    };

    private static readonly Dictionary<string, string> De = new(StringComparer.OrdinalIgnoreCase)
    {
        ["apply_all"] = "Alle anwenden",
        ["remove_all"] = "Alle entfernen",
        ["search_placeholder"] = "Optimierungen suchen\u2026",
        ["status_applied"] = "ANGEWENDET",
        ["status_not_applied"] = "STANDARD",
        ["status_unknown"] = "UNBEKANNT",
        ["status_error"] = "FEHLER",
        ["status_skipped_corp"] = "\u00dcBERSPRUNGEN (Firma)",
        ["status_skipped_build"] = "\u00dcBERSPRUNGEN (Build)",
        ["filter_all"] = "Alle",
        ["filter_applied"] = "Angewendet",
        ["filter_default"] = "Standard",
        ["filter_unknown"] = "Unbekannt",
        ["scope_user"] = "Benutzer (HKCU)",
        ["scope_machine"] = "Computer (HKLM)",
        ["scope_both"] = "Beide",
        ["scope_all"] = "Alle Bereiche",
        ["profile_business"] = "Gesch\u00e4ftlich",
        ["profile_gaming"] = "Gaming",
        ["profile_privacy"] = "Datenschutz",
        ["profile_minimal"] = "Minimal",
        ["profile_server"] = "Server",
        ["btn_apply"] = "Ausgew\u00e4hlte anwenden",
        ["btn_remove"] = "Ausgew\u00e4hlte entfernen",
        ["btn_select_all"] = "Alle ausw\u00e4hlen",
        ["btn_deselect_all"] = "Auswahl aufheben",
        ["btn_invert"] = "Auswahl umkehren",
        ["btn_refresh"] = "Status aktualisieren",
        ["btn_export_ps1"] = "PS1 exportieren",
        ["btn_export_json"] = "JSON exportieren",
        ["btn_import_json"] = "JSON importieren",
        ["menu_file"] = "Datei",
        ["menu_tools"] = "Werkzeuge",
        ["menu_view"] = "Ansicht",
        ["menu_help"] = "Hilfe",
        ["about_title"] = "\u00dcber RegiLattice",
        ["scoop_manager"] = "Scoop-Verwaltung",
        ["psmodule_manager"] = "PowerShell-Module",
        ["pip_manager"] = "pip-Pakete",
        ["log_panel"] = "Protokollbereich umschalten",
        ["corporate_warning"] = "Firmenumgebung erkannt. Einige Optimierungen sind gesperrt.",
        ["admin_required"] = "Administratorrechte erforderlich.",
        ["confirm_apply"] = "{0} ausgew\u00e4hlte Optimierungen anwenden?",
        ["confirm_remove"] = "{0} ausgew\u00e4hlte Optimierungen entfernen?",
        ["tweaks_loaded"] = "{0} Optimierungen in {1} Kategorien geladen.",
        ["detection_complete"] = "Statuserkennung abgeschlossen.",
        ["export_complete"] = "Exportiert nach {0}.",
        ["import_complete"] = "{0} Optimierungen aus {1} importiert.",
    };

    private static readonly Dictionary<string, Dictionary<string, string>> BuiltInLocales = new(StringComparer.OrdinalIgnoreCase)
    {
        ["en"] = En,
        ["de"] = De,
    };

    private static Dictionary<string, string> _active = new(En);

    public static string T(string key, params object[] args)
    {
        var template = _active.GetValueOrDefault(key, key);
        return args.Length > 0 ? string.Format(template, args) : template;
    }

    public static void SetLocale(string name, Dictionary<string, string>? overrides = null)
    {
        _current = name;
        var baseLocale = BuiltInLocales.GetValueOrDefault(name) ?? En;
        _active = new Dictionary<string, string>(baseLocale, StringComparer.OrdinalIgnoreCase);
        if (overrides is not null)
            foreach (var (k, v) in overrides)
                _active[k] = v;
    }

    public static string CurrentLocale => _current;
    public static IReadOnlyCollection<string> AvailableKeys => _active.Keys;
    public static IReadOnlyCollection<string> AvailableLocales => BuiltInLocales.Keys;

    public static void LoadLocaleFile(string path)
    {
        if (!File.Exists(path))
            return;
        var lines = File.ReadAllLines(path);
        var overrides = new Dictionary<string, string>();
        foreach (var line in lines)
        {
            var eq = line.IndexOf('=');
            if (eq > 0)
                overrides[line[..eq].Trim()] = line[(eq + 1)..].Trim();
        }
        SetLocale(Path.GetFileNameWithoutExtension(path), overrides);
    }
}
