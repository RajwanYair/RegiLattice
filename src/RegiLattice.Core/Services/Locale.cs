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

    private static readonly Dictionary<string, string> He = new(StringComparer.OrdinalIgnoreCase)
    {
        ["apply_all"] = "\u05d4\u05d7\u05dc \u05d4\u05db\u05dc",
        ["remove_all"] = "\u05d4\u05e1\u05e8 \u05d4\u05db\u05dc",
        ["search_placeholder"] = "\u05d7\u05e4\u05e9 \u05d0\u05d9\u05e4\u05d5\u05e0\u05d9\u05dd\u2026",
        ["status_applied"] = "\u05de\u05d5\u05e4\u05e2\u05dc",
        ["status_not_applied"] = "\u05d1\u05e8\u05d9\u05e8\u05ea \u05de\u05d7\u05d3\u05dc",
        ["status_unknown"] = "\u05dc\u05d0 \u05d9\u05d3\u05d5\u05e2",
        ["status_error"] = "\u05e9\u05d2\u05d9\u05d0\u05d4",
        ["status_skipped_corp"] = "\u05d3\u05d5\u05dc\u05d2 (\u05d7\u05d1\u05e8\u05d4)",
        ["status_skipped_build"] = "\u05d3\u05d5\u05dc\u05d2 (Build)",
        ["filter_all"] = "\u05d4\u05db\u05dc",
        ["filter_applied"] = "\u05de\u05d5\u05e4\u05e2\u05dc\u05d9\u05dd",
        ["filter_default"] = "\u05d1\u05e8\u05d9\u05e8\u05ea \u05de\u05d7\u05d3\u05dc",
        ["filter_unknown"] = "\u05dc\u05d0 \u05d9\u05d3\u05d5\u05e2",
        ["scope_user"] = "\u05de\u05e9\u05ea\u05de\u05e9 (HKCU)",
        ["scope_machine"] = "\u05de\u05d7\u05e9\u05d1 (HKLM)",
        ["scope_both"] = "\u05e9\u05e0\u05d9\u05d4\u05dd",
        ["scope_all"] = "\u05db\u05dc \u05d4\u05d8\u05d5\u05d5\u05d7\u05d9\u05dd",
        ["profile_business"] = "\u05e2\u05e1\u05e7\u05d9",
        ["profile_gaming"] = "\u05d2\u05d9\u05d9\u05de\u05d9\u05e0\u05d2",
        ["profile_privacy"] = "\u05e4\u05e8\u05d8\u05d9\u05d5\u05ea",
        ["profile_minimal"] = "\u05de\u05d9\u05e0\u05d9\u05de\u05dc\u05d9",
        ["profile_server"] = "\u05e9\u05e8\u05ea",
        ["btn_apply"] = "\u05d4\u05d7\u05dc \u05d1\u05d7\u05d9\u05e8\u05d4",
        ["btn_remove"] = "\u05d4\u05e1\u05e8 \u05d1\u05d7\u05d9\u05e8\u05d4",
        ["btn_select_all"] = "\u05d1\u05d7\u05e8 \u05d4\u05db\u05dc",
        ["btn_deselect_all"] = "\u05d1\u05d8\u05dc \u05d1\u05d7\u05d9\u05e8\u05d4",
        ["btn_invert"] = "\u05d4\u05e4\u05d5\u05da \u05d1\u05d7\u05d9\u05e8\u05d4",
        ["btn_refresh"] = "\u05e8\u05e2\u05e0\u05df \u05e1\u05d8\u05d8\u05d5\u05e1",
        ["btn_export_ps1"] = "\u05d9\u05d9\u05e6\u05d0 PS1",
        ["btn_export_json"] = "\u05d9\u05d9\u05e6\u05d0 JSON",
        ["btn_import_json"] = "\u05d9\u05d9\u05d1\u05d0 JSON",
        ["menu_file"] = "\u05e7\u05d5\u05d1\u05e5",
        ["menu_tools"] = "\u05db\u05dc\u05d9\u05dd",
        ["menu_view"] = "\u05ea\u05e6\u05d5\u05d2\u05d4",
        ["menu_help"] = "\u05e2\u05d6\u05e8\u05d4",
        ["about_title"] = "\u05d0\u05d5\u05d3\u05d5\u05ea RegiLattice",
        ["scoop_manager"] = "\u05e0\u05d9\u05d4\u05d5\u05dc Scoop",
        ["psmodule_manager"] = "\u05de\u05d5\u05d3\u05d5\u05dc\u05d9 PowerShell",
        ["pip_manager"] = "\u05d7\u05d1\u05d9\u05dc\u05d5\u05ea pip",
        ["log_panel"] = "\u05d4\u05d7\u05dc\u05e3/\u05d4\u05e1\u05ea\u05e8 \u05dc\u05d5\u05d7 \u05e2\u05d5\u05e7\u05d1\u05d9\u05df",
        ["corporate_warning"] =
            "\u05d6\u05d5\u05d4\u05ea\u05d4 \u05e1\u05d1\u05d9\u05d1\u05ea \u05d0\u05e8\u05d2\u05d5\u05df. \u05d7\u05dc\u05e7 \u05de\u05d4\u05d0\u05d9\u05e4\u05d5\u05e0\u05d9\u05dd \u05d7\u05e1\u05d5\u05de\u05d9\u05dd.",
        ["admin_required"] = "\u05d3\u05e8\u05d5\u05e9\u05d5\u05ea \u05d4\u05e8\u05e9\u05d0\u05d5\u05ea \u05de\u05e0\u05d4\u05dc.",
        ["confirm_apply"] = "\u05dc\u05d4\u05d7\u05d9\u05dc {0} \u05d0\u05d9\u05e4\u05d5\u05e0\u05d9\u05dd \u05e0\u05d1\u05d7\u05e8\u05d9\u05dd?",
        ["confirm_remove"] = "\u05dc\u05d4\u05e1\u05d9\u05e8 {0} \u05d0\u05d9\u05e4\u05d5\u05e0\u05d9\u05dd \u05e0\u05d1\u05d7\u05e8\u05d9\u05dd?",
        ["tweaks_loaded"] =
            "{0} \u05d0\u05d9\u05e4\u05d5\u05e0\u05d9\u05dd \u05d8\u05d5\u05e2\u05e0\u05d5 \u05d1-{1} \u05e7\u05d8\u05d2\u05d5\u05e8\u05d9\u05d5\u05ea.",
        ["detection_complete"] = "\u05d6\u05d9\u05d4\u05d5\u05d9 \u05d4\u05e1\u05d8\u05d8\u05d5\u05e1 \u05d4\u05d5\u05e9\u05dc\u05dd.",
        ["export_complete"] = "\u05d9\u05d5\u05d9\u05d9\u05e6\u05d0 \u05d0\u05dc {0}.",
        ["import_complete"] = "\u05d9\u05d5\u05d1\u05d0\u05d5 {0} \u05d0\u05d9\u05e4\u05d5\u05e0\u05d9\u05dd \u05de-{1}.",
    };

    private static readonly Dictionary<string, string> Ja = new(StringComparer.OrdinalIgnoreCase)
    {
        ["apply_all"] = "\u3059\u3079\u3066\u9069\u7528",
        ["remove_all"] = "\u3059\u3079\u3066\u524a\u9664",
        ["search_placeholder"] = "\u8abf\u6574\u3092\u691c\u7d22\u2026",
        ["status_applied"] = "\u9069\u7528\u6e08\u307f",
        ["status_not_applied"] = "\u30c7\u30d5\u30a9\u30eb\u30c8",
        ["status_unknown"] = "\u4e0d\u660e",
        ["status_error"] = "\u30a8\u30e9\u30fc",
        ["status_skipped_corp"] = "\u30b9\u30ad\u30c3\u30d7 (\u6cd5\u4eba)",
        ["status_skipped_build"] = "\u30b9\u30ad\u30c3\u30d7 (Build)",
        ["filter_all"] = "\u3059\u3079\u3066",
        ["filter_applied"] = "\u9069\u7528\u6e08\u307f",
        ["filter_default"] = "\u30c7\u30d5\u30a9\u30eb\u30c8",
        ["filter_unknown"] = "\u4e0d\u660e",
        ["scope_user"] = "\u30e6\u30fc\u30b6\u30fc (HKCU)",
        ["scope_machine"] = "\u30de\u30b7\u30f3 (HKLM)",
        ["scope_both"] = "\u4e21\u65b9",
        ["scope_all"] = "\u3059\u3079\u3066\u306e\u30b9\u30b3\u30fc\u30d7",
        ["profile_business"] = "\u30d3\u30b8\u30cd\u30b9",
        ["profile_gaming"] = "\u30b2\u30fc\u30df\u30f3\u30b0",
        ["profile_privacy"] = "\u30d7\u30e9\u30a4\u30d0\u30b7\u30fc",
        ["profile_minimal"] = "\u30df\u30cb\u30de\u30eb",
        ["profile_server"] = "\u30b5\u30fc\u30d0\u30fc",
        ["btn_apply"] = "\u9078\u629e\u3092\u9069\u7528",
        ["btn_remove"] = "\u9078\u629e\u3092\u524a\u9664",
        ["btn_select_all"] = "\u3059\u3079\u3066\u9078\u629e",
        ["btn_deselect_all"] = "\u9078\u629e\u3092\u89e3\u9664",
        ["btn_invert"] = "\u9078\u629e\u3092\u53cd\u8ee2",
        ["btn_refresh"] = "\u30b9\u30c6\u30fc\u30bf\u30b9\u3092\u66f4\u65b0",
        ["btn_export_ps1"] = "PS1 \u3092\u30a8\u30af\u30b9\u30dd\u30fc\u30c8",
        ["btn_export_json"] = "JSON \u3092\u30a8\u30af\u30b9\u30dd\u30fc\u30c8",
        ["btn_import_json"] = "JSON \u3092\u30a4\u30f3\u30dd\u30fc\u30c8",
        ["menu_file"] = "\u30d5\u30a1\u30a4\u30eb",
        ["menu_tools"] = "\u30c4\u30fc\u30eb",
        ["menu_view"] = "\u8868\u793a",
        ["menu_help"] = "\u30d8\u30eb\u30d7",
        ["about_title"] = "RegiLattice \u306b\u3064\u3044\u3066",
        ["scoop_manager"] = "Scoop \u30de\u30cd\u30fc\u30b8\u30e3\u30fc",
        ["psmodule_manager"] = "PowerShell \u30e2\u30b8\u30e5\u30fc\u30eb",
        ["pip_manager"] = "pip \u30d1\u30c3\u30b1\u30fc\u30b8",
        ["log_panel"] = "\u30ed\u30b0\u30d1\u30cd\u30eb\u306e\u5207\u308a\u66ff\u3048",
        ["corporate_warning"] = "\u6cd5\u4eba\u74b0\u5883\u304c\u691c\u51fa\u3055\u308c\u307e\u3057\u305f\u3002\u4e00\u90e8\u306e\u8abf\u6574\u306f\u30d6\u30ed\u30c3\u30af\u3055\u308c\u3066\u3044\u307e\u3059\u3002",
        ["admin_required"] = "\u7ba1\u7406\u8005\u6a29\u9650\u304c\u5fc5\u8981\u3067\u3059\u3002",
        ["confirm_apply"] = "\u9078\u629e\u3057\u305f {0} \u500b\u306e\u8abf\u6574\u3092\u9069\u7528\u3057\u307e\u3059\u304b\uff1f",
        ["confirm_remove"] = "\u9078\u629e\u3057\u305f {0} \u500b\u306e\u8abf\u6574\u3092\u524a\u9664\u3057\u307e\u3059\u304b\uff1f",
        ["tweaks_loaded"] = "{1} \u30ab\u30c6\u30b4\u30ea\u306b {0} \u500b\u306e\u8abf\u6574\u3092\u8aad\u307f\u8fbc\u307f\u307e\u3057\u305f\u3002",
        ["detection_complete"] = "\u30b9\u30c6\u30fc\u30bf\u30b9\u691c\u51fa\u304c\u5b8c\u4e86\u3057\u307e\u3057\u305f\u3002",
        ["export_complete"] = "{0} \u306b\u30a8\u30af\u30b9\u30dd\u30fc\u30c8\u3057\u307e\u3057\u305f\u3002",
        ["import_complete"] = "{1} \u304b\u3089 {0} \u500b\u306e\u8abf\u6574\u3092\u30a4\u30f3\u30dd\u30fc\u30c8\u3057\u307e\u3057\u305f\u3002",
    };

    private static readonly Dictionary<string, Dictionary<string, string>> BuiltInLocales = new(StringComparer.OrdinalIgnoreCase)
    {
        ["en"] = En,
        ["de"] = De,
        ["fr"] = Fr,
        ["es"] = Es,
        ["he"] = He,
        ["ja"] = Ja,
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
