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

    private static readonly Dictionary<string, string> Fr = new(StringComparer.OrdinalIgnoreCase)
    {
        ["apply_all"] = "Tout appliquer",
        ["remove_all"] = "Tout supprimer",
        ["search_placeholder"] = "Rechercher des ajustements\u2026",
        ["status_applied"] = "APPLIQU\u00c9",
        ["status_not_applied"] = "D\u00c9FAUT",
        ["status_unknown"] = "INCONNU",
        ["status_error"] = "ERREUR",
        ["status_skipped_corp"] = "IGNOR\u00c9 (Entreprise)",
        ["status_skipped_build"] = "IGNOR\u00c9 (Build)",
        ["filter_all"] = "Tous",
        ["filter_applied"] = "Appliqu\u00e9s",
        ["filter_default"] = "D\u00e9faut",
        ["filter_unknown"] = "Inconnu",
        ["scope_user"] = "Utilisateur (HKCU)",
        ["scope_machine"] = "Ordinateur (HKLM)",
        ["scope_both"] = "Les deux",
        ["scope_all"] = "Toutes les port\u00e9es",
        ["profile_business"] = "Professionnel",
        ["profile_gaming"] = "Gaming",
        ["profile_privacy"] = "Confidentialit\u00e9",
        ["profile_minimal"] = "Minimal",
        ["profile_server"] = "Serveur",
        ["btn_apply"] = "Appliquer la s\u00e9lection",
        ["btn_remove"] = "Supprimer la s\u00e9lection",
        ["btn_select_all"] = "Tout s\u00e9lectionner",
        ["btn_deselect_all"] = "Tout d\u00e9s\u00e9lectionner",
        ["btn_invert"] = "Inverser la s\u00e9lection",
        ["btn_refresh"] = "Actualiser le statut",
        ["btn_export_ps1"] = "Exporter PS1",
        ["btn_export_json"] = "Exporter JSON",
        ["btn_import_json"] = "Importer JSON",
        ["menu_file"] = "Fichier",
        ["menu_tools"] = "Outils",
        ["menu_view"] = "Affichage",
        ["menu_help"] = "Aide",
        ["about_title"] = "\u00c0 propos de RegiLattice",
        ["scoop_manager"] = "Gestionnaire Scoop",
        ["psmodule_manager"] = "Modules PowerShell",
        ["pip_manager"] = "Paquets pip",
        ["log_panel"] = "Afficher/masquer le journal",
        ["corporate_warning"] = "Environnement d\u2019entreprise d\u00e9tect\u00e9. Certains ajustements sont bloqu\u00e9s.",
        ["admin_required"] = "Privil\u00e8ges administrateur requis.",
        ["confirm_apply"] = "Appliquer {0} ajustements s\u00e9lectionn\u00e9s\u00a0?",
        ["confirm_remove"] = "Supprimer {0} ajustements s\u00e9lectionn\u00e9s\u00a0?",
        ["tweaks_loaded"] = "{0} ajustements charg\u00e9s dans {1} cat\u00e9gories.",
        ["detection_complete"] = "D\u00e9tection du statut termin\u00e9e.",
        ["export_complete"] = "Export\u00e9 vers {0}.",
        ["import_complete"] = "{0} ajustements import\u00e9s depuis {1}.",
    };

    private static readonly Dictionary<string, string> Es = new(StringComparer.OrdinalIgnoreCase)
    {
        ["apply_all"] = "Aplicar todo",
        ["remove_all"] = "Eliminar todo",
        ["search_placeholder"] = "Buscar ajustes\u2026",
        ["status_applied"] = "APLICADO",
        ["status_not_applied"] = "PREDETERMINADO",
        ["status_unknown"] = "DESCONOCIDO",
        ["status_error"] = "ERROR",
        ["status_skipped_corp"] = "OMITIDO (Empresa)",
        ["status_skipped_build"] = "OMITIDO (Build)",
        ["filter_all"] = "Todos",
        ["filter_applied"] = "Aplicados",
        ["filter_default"] = "Predeterminado",
        ["filter_unknown"] = "Desconocido",
        ["scope_user"] = "Usuario (HKCU)",
        ["scope_machine"] = "M\u00e1quina (HKLM)",
        ["scope_both"] = "Ambos",
        ["scope_all"] = "Todos los \u00e1mbitos",
        ["profile_business"] = "Empresarial",
        ["profile_gaming"] = "Gaming",
        ["profile_privacy"] = "Privacidad",
        ["profile_minimal"] = "M\u00ednimo",
        ["profile_server"] = "Servidor",
        ["btn_apply"] = "Aplicar selecci\u00f3n",
        ["btn_remove"] = "Eliminar selecci\u00f3n",
        ["btn_select_all"] = "Seleccionar todo",
        ["btn_deselect_all"] = "Deseleccionar todo",
        ["btn_invert"] = "Invertir selecci\u00f3n",
        ["btn_refresh"] = "Actualizar estado",
        ["btn_export_ps1"] = "Exportar PS1",
        ["btn_export_json"] = "Exportar JSON",
        ["btn_import_json"] = "Importar JSON",
        ["menu_file"] = "Archivo",
        ["menu_tools"] = "Herramientas",
        ["menu_view"] = "Vista",
        ["menu_help"] = "Ayuda",
        ["about_title"] = "Acerca de RegiLattice",
        ["scoop_manager"] = "Gestor Scoop",
        ["psmodule_manager"] = "M\u00f3dulos PowerShell",
        ["pip_manager"] = "Paquetes pip",
        ["log_panel"] = "Alternar panel de registro",
        ["corporate_warning"] = "Entorno corporativo detectado. Algunos ajustes est\u00e1n bloqueados.",
        ["admin_required"] = "Se requieren privilegios de administrador.",
        ["confirm_apply"] = "\u00bfAplicar {0} ajustes seleccionados?",
        ["confirm_remove"] = "\u00bfEliminar {0} ajustes seleccionados?",
        ["tweaks_loaded"] = "{0} ajustes cargados en {1} categor\u00edas.",
        ["detection_complete"] = "Detecci\u00f3n de estado completada.",
        ["export_complete"] = "Exportado a {0}.",
        ["import_complete"] = "{0} ajustes importados desde {1}.",
    };

    private static readonly Dictionary<string, Dictionary<string, string>> BuiltInLocales = new(StringComparer.OrdinalIgnoreCase)
    {
        ["en"] = En,
        ["de"] = De,
        ["fr"] = Fr,
        ["es"] = Es,
    };

    private static Dictionary<string, string> _active = new(En);
    private static readonly Dictionary<string, string> _hotCache = new(StringComparer.OrdinalIgnoreCase);

    public static string T(string key, params object[] args)
    {
        if (!_hotCache.TryGetValue(key, out var template))
        {
            template = _active.GetValueOrDefault(key, key);
            _hotCache[key] = template;
        }

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

        _hotCache.Clear();
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
