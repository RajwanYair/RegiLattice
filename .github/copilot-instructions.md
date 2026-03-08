# RegiLattice — Copilot Instructions

> Auto-loaded by GitHub Copilot on every chat/agent session in this workspace.
> Keep this file accurate — it is the fastest path to project understanding.
> Last verified: 2026-03-07 (v1.0.0, 1 292 tweaks, 69 categories, ~17 266 tests).

## Quick Facts

| Key         | Value                                                                        |
| ----------- | ---------------------------------------------------------------------------- |
| Language    | Python 3.10+ (stdlib only at runtime)                                        |
| Build       | `hatchling` via `pyproject.toml`                                             |
| Lint        | `ruff` (E, F, W, I, UP, B, SIM, RUF; line-length 150; ignore ARG002)         |
| Type-check  | `mypy --strict`                                                              |
| Test        | `pytest` in `tests/` (~17 300 tests across 21 test files)                    |
| GUI         | tkinter with 4 themes (Catppuccin Mocha/Latte, Nord, Dracula)                |
| Version     | 1.0.0                                                                        |
| Python path | `C:\Users\ryair\AppData\Local\Python\bin\python.exe` (NOT WindowsApps alias) |
| Install     | `pip install -e ".[dev]"`                                                    |

## Architecture at a Glance

```
regilattice/
├── __init__.py          # package version (__version__)
├── __main__.py          # entry: delegates to cli.main()
├── cli.py               # argparse CLI (apply/remove/list/gui/profile)
├── menu.py              # interactive numbered console menu
├── gui.py               # tkinter GUI main window (deferred loading, async corp check)
├── gui_widgets.py       # TweakRow + CategorySection widgets
├── gui_theme.py         # 4-theme support (Catppuccin Mocha/Latte, Nord, Dracula)
├── gui_tooltip.py       # Tooltip widget + description metadata parser
├── gui_dialogs.py       # Import JSON, Export PS1, Scoop Manager, About dialog
├── registry.py          # RegistrySession: winreg wrapper + backup + logging
├── config.py            # user config via ~/.regilattice.toml (AppConfig)
├── corpguard.py         # corporate network detection (domain/AAD/VPN/GPO/SCCM)
├── elevation.py         # UAC elevation helpers (is_admin, request_elevation, run_elevated)
├── deps.py              # lazy-import + auto-install for optional packages
├── analytics.py         # local-only usage analytics (applies, removes, sessions)
├── ratings.py           # local tweak rating system (1-5 stars + notes)
├── locale.py            # i18n string table for UI labels
├── hwinfo.py            # hardware detection (CPU/GPU/RAM/disk), adaptive config
├── marketplace.py       # third-party plugin discovery & loading
└── tweaks/
    ├── __init__.py      # TweakDef dataclass, plugin loader, profiles, batch ops
    ├── _template.py     # contributor guide (not loaded by plugin loader)
    ├── accessibility.py # 69 category modules ...
    ├── ...              # each exports TWEAKS: list[TweakDef]
    └── wsl.py
```

### Plugin Loader (zero-wiring)

`tweaks/__init__.py` auto-discovers every `.py` in `tweaks/` (skipping `_`-prefixed).
Each module must export `TWEAKS: list[TweakDef]`.
**No imports or registration needed** — drop a file and it works.

### TweakDef Dataclass

```python
@dataclass(slots=True)
class TweakDef:
    id: str                                    # unique kebab-case (globally)
    label: str                                 # human name
    category: str                              # UI grouping
    apply_fn: Callable[..., None]              # applies the tweak
    remove_fn: Callable[..., None]             # reverts the tweak
    detect_fn: Callable[[], bool] | None       # True = currently active
    needs_admin: bool = True                   # True → HKLM / UAC
    corp_safe: bool = False                    # True → HKCU-only, safe on corp
    registry_keys: list[str]                   # paths touched (backup/tooltip)
    description: str = ""                      # tooltip / --list text
    tags: list[str]                            # search keywords
    depends_on: list[str]                      # IDs this tweak depends on
    min_build: int = 0                         # minimum Windows build (0 = any)
    side_effects: str = ""                     # what may break when applied
```

### Function Triplet Pattern

Every tweak implements three private functions:

```python
def _apply_<name>(*, require_admin: bool = True) -> None: ...
def _remove_<name>(*, require_admin: bool = True) -> None: ...
def _detect_<name>() -> bool: ...
```

- `require_admin` is a **mandatory keyword-only** parameter (even if unused — ARG002 is suppressed).
- Call `assert_admin(require_admin)` first, then `SESSION.backup(...)`, then `SESSION.set_dword(...)` etc.

### Registry Session API

```python
from regilattice.registry import SESSION

SESSION.set_dword(path, name, value)       # REG_DWORD
SESSION.set_string(path, name, value)      # REG_SZ
SESSION.set_value(path, name, val, type)   # any type
SESSION.read_dword(path, name) → int|None
SESSION.read_string(path, name) → str|None
SESSION.key_exists(path) → bool
SESSION.delete_value(path, name)
SESSION.delete_tree(path)
SESSION.backup(keys_list, label)
SESSION.log(message)
```

Paths use full hive names: `HKEY_LOCAL_MACHINE\...` or `HKEY_CURRENT_USER\...`
(abbreviations `HKLM\...` / `HKCU\...` also accepted).

## Tweak ID Naming Convention

All tweak IDs follow the pattern: `{category_slug}-{descriptive-name}`

Canonical category slugs:

| Slug       | Category                    | Slug       | Category              |
| ---------- | --------------------------- | ---------- | --------------------- |
| `acc`      | Accessibility               | `lo`       | LibreOffice           |
| `adobe`    | Adobe                       | `lock`     | Lock Screen & Login   |
| `ai`       | AI / Copilot                | `m365`     | M365 Copilot          |
| `audio`    | Audio                       | `maint`    | Maintenance           |
| `backup`   | Backup & Recovery           | `media`    | Multimedia            |
| `boot`     | Boot                        | `msstore`  | Microsoft Store       |
| `bt`       | Bluetooth                   | `net`      | Network               |
| `chrome`   | Chrome                      | `night`    | Night Light & Display |
| `clip`     | Clipboard & Drag-Drop       | `notif`    | Notifications         |
| `cloud`    | Cloud Storage               | `od`       | OneDrive              |
| `comm`     | Communication               | `office`   | Office                |
| `cortana`  | Cortana & Search            | `perf`     | Performance           |
| `crash`    | Crash & Diagnostics         | `phone`    | Phone Link            |
| `ctx`      | Context Menu                | `pkg`      | Package Management    |
| `dev`      | Dev Drive / Developer Tools | `power`    | Power                 |
| `display`  | Display                     | `printing` | Printing              |
| `dns`      | DNS & Networking Advanced   | `priv`     | Privacy               |
| `edge`     | Edge                        | `rdp`      | Remote Desktop        |
| `explorer` | Explorer                    | `schtask`  | Scheduled Tasks       |
| `firefox`  | Firefox                     | `scoop`    | Scoop Tools           |
| `font`     | Fonts                       | `sec`      | Security              |
| `fs`       | File System                 | `shell`    | Shell                 |
| `game`     | Gaming                      | `snap`     | Snap & Multitasking   |
| `gpu`      | GPU / Graphics              | `speech`   | Voice Access & Speech |
| `idx`      | Indexing & Search           | `ss`       | Screensaver & Lock    |
| `input`    | Input                       | `startup`  | Startup               |
| `java`     | Java                        | `stor`     | Storage               |
| `svc`      | Services                    | `sys`      | System                |
| `tb`       | Taskbar                     | `telem`    | Telemetry Advanced    |
| `term`     | Windows Terminal            | `touch`    | Touch & Pen           |
| `usb`      | USB & Peripherals           | `virt`     | Virtualization        |
| `vnc`      | RealVNC                     | `vscode`   | VS Code               |
| `w11`      | Windows 11                  | `widgets`  | Widgets & News        |
| `wsl`      | WSL                         | `wu`       | Windows Update        |

## Current Stats (1 292 tweaks, 69 categories, 69 modules)

| Category                  | Tweaks | Category              | Tweaks |
| ------------------------- | ------ | --------------------- | ------ |
| Accessibility             | 20     | Multimedia            | 15     |
| Adobe                     | 20     | Network               | 22     |
| AI / Copilot              | 22     | Night Light & Display | 12     |
| Audio                     | 19     | Notifications         | 16     |
| Backup & Recovery         | 15     | Office                | 20     |
| Bluetooth                 | 19     | OneDrive              | 18     |
| Boot                      | 21     | Package Management    | 21     |
| Chrome                    | 20     | Performance           | 20     |
| Clipboard & Drag-Drop     | 15     | Phone Link            | 14     |
| Cloud Storage             | 30     | Power                 | 21     |
| Communication             | 21     | Printing              | 15     |
| Context Menu              | 15     | Privacy               | 25     |
| Cortana & Search          | 22     | RealVNC               | 15     |
| Crash & Diagnostics       | 16     | Remote Desktop        | 16     |
| Dev Drive                 | 12     | Scheduled Tasks       | 16     |
| Developer Tools           | 17     | Scoop Tools           | 25     |
| Display                   | 19     | Screensaver & Lock    | 16     |
| DNS & Networking Advanced | 16     | Security              | 21     |
| Edge                      | 18     | Services              | 21     |
| Explorer                  | 41     | Shell                 | 20     |
| File System               | 17     | Snap & Multitasking   | 17     |
| Firefox                   | 20     | Startup               | 19     |
| Fonts                     | 19     | Storage               | 19     |
| Gaming                    | 19     | System                | 24     |
| GPU / Graphics            | 19     | Taskbar               | 19     |
| Indexing & Search         | 16     | Telemetry Advanced    | 16     |
| Input                     | 18     | Touch & Pen           | 13     |
| Java                      | 16     | USB & Peripherals     | 16     |
| LibreOffice               | 18     | Virtualization        | 15     |
| Lock Screen & Login       | 16     | VS Code               | 19     |
| M365 Copilot              | 18     | Voice Access & Speech | 13     |
| Maintenance               | 17     | Widgets & News        | 15     |
| Microsoft Store           | 15     | Windows 11            | 29     |
| Multimedia                | 15     | Windows Terminal      | 16     |
|                           |        | Windows Update        | 18     |
|                           |        | WSL                   | 29     |

## 5 Profiles

| Profile    | Categories | Description                                           |
| ---------- | ---------- | ----------------------------------------------------- |
| `business` | 39         | Productivity, security, cloud & workflow (770 tweaks) |
| `gaming`   | 31         | GPU, performance, low-latency, distraction-free (604) |
| `privacy`  | 31         | Telemetry, tracking, cloud & browser data (628)       |
| `minimal`  | 22         | Fast, clean system essentials (430)                   |
| `server`   | 28         | Hardened, headless, uptime & remote mgmt (549)        |

Profiles defined in `tweaks/__init__.py` → `_PROFILES` dict.
Public API: `available_profiles()`, `profile_info(name)`, `tweaks_for_profile(name)`, `apply_profile(name)`.

## Corporate Guard

`corpguard.py` detects corporate environments via:

1. AD domain membership (ctypes `GetComputerNameExW`)
2. Azure AD / Entra ID join (`dsregcmd /status`)
3. VPN adapter keywords (`Get-NetAdapter`)
4. Group Policy registry indicators
5. SCCM / Intune enrollment

If corporate detected → tweaks with `corp_safe=False` are blocked.
Override: `--force` CLI flag or GUI "Force" checkbox (logged).

## GUI Details (split across `gui.py`, `gui_widgets.py`, `gui_theme.py`, `gui_tooltip.py`, `gui_dialogs.py`)

- 4 colour themes: Catppuccin Mocha (default), Catppuccin Latte, Nord, Dracula — switchable at runtime
- Deferred loading: window appears instantly, categories loaded in batches of 4
- Async corporate detection: corp check runs in background thread, never blocks UI
- Collapsible category sections with tweak counts (applied/total)
- **Scope badges**: USER (green) / MACHINE (blue) / BOTH (yellow) per tweak row
- Search bar + status filter (All / Applied / Default / Unknown) + scope filter (User/Machine/Both)
- Profile selector dropdown (Business / Gaming / Privacy / Minimal / Server)
- Theme selector dropdown
- Export PS1 button (generates PowerShell script from selected tweaks)
- Import JSON button (load tweak ID list from file to select)
- Scoop Tools Manager dialog (install/remove packages)
- Threaded execution — never blocks UI thread
- Live status badges via `detect_fn()` with parallel detection (`status_map(parallel=True)`)
- Rich hover tooltips with description, current state, default/recommendation hints, tags, registry keys
- Right-click context menu on tweak rows (Enable/Disable, Copy ID, Copy Registry Key, Select category)
- Recommendation badges (teal "REC" tag) for tweaks with recommendations in description
- Summary stats bar: Applied / Default / Unknown / Recommended / GPO counts
- Per-category Enable All / Disable All buttons with risk/scope/profile badges
- Toggleable log viewer panel (shows session log inline)
- About dialog with system info and shortcut reference
- Invert Selection button
- Keyboard shortcuts: Ctrl+A/D/F/E/I/L/R, Up/Down/Space, Esc
- Keyboard row navigation: Up/Down arrows move focus, Space toggles selection
- Auto-refreshing log panel (every 2s while visible)
- Lazy tooltip creation via `text_fn` callable (deferred until first hover)
- GPO check caching via `functools.lru_cache` (avoids repeated winreg lookups)
- `parse_description_metadata()` extracts `Default:` and `Recommended:` from description text
- `functools.lru_cache` on description parsing for performance
- `tweak_scope()` classifies tweaks as user/machine/both from registry keys

## Test Infrastructure

- `tests/conftest.py`: `dry_session` fixture (RegistrySession with `_dry_run=True`), `all_tweaks_list` session fixture
- `tests/test_tweaks_smoke.py`: auto-parametrized over all tweaks — tests triplet signatures, ID uniqueness, detect_fn callability, `require_admin` kwarg validation
- `tests/test_tweaks_init.py`: plugin loader, categories, profiles, search, batch ops, snapshot round-trip
- `tests/test_gui_theme.py`: theme switching, colour attribute validation
- `tests/test_gui_tooltip.py`: tooltip text building, metadata parsing, recommendation detection
- `tests/test_gui_widgets.py`: tweak scope classification
- `tests/test_gui_dialogs.py`: PS1 export, JSON import, about dialog
- `tests/test_cli.py`: CLI argument parsing and commands
- `tests/test_config.py`: AppConfig loading
- `tests/test_deps.py`: lazy_import, install_package, require
- `tests/test_elevation.py`: is_admin, request_elevation, run_elevated
- `tests/test_menu.py`: interactive console menu
- `tests/test_registry.py`: RegistrySession helpers and backup
- `tests/test_corpguard.py`: corporate network detection
- `tests/test_analytics.py`: usage analytics (record, stats, reset)
- `tests/test_ratings.py`: tweak rating system (rate, retrieve, remove)
- `tests/test_locale.py`: i18n string table (translate, locale switch, file load)
- `tests/test_marketplace.py`: plugin discovery, loading, version check
- `tests/test_integration.py`: end-to-end CLI and batch operation tests
- `tests/test_property.py`: hypothesis property-based tests
- `tests/test_benchmarks.py`: performance benchmarks for core operations
- `tests/test_hwinfo.py`: hardware detection probes, adaptive workers/batch, profile suggestion, summary
- `tests/test_registry.py`: RegistrySession helpers and backup
- `tests/test_corpguard.py`: corporate network detection
- Known: `test_registry.py::TestBackup::test_backup_creates_directory` may fail with WinError 6 (handle issue, environment-specific)

## Adding a New Tweak — Checklist

1. Create/edit `regilattice/tweaks/<category>.py`
2. Add `_KEY = r"HKEY_..."` constants at module top
3. Implement `_apply_X`, `_remove_X`, `_detect_X` triplet
4. Append `TweakDef(...)` to module-level `TWEAKS` list
5. Ensure `id` is **globally unique** kebab-case
6. Run: `python -m pytest tests/test_tweaks_smoke.py -x --tb=short`
7. Run: `python -m ruff check regilattice/ tests/`
8. No edits to `__init__.py` needed — auto-discovered

## Common Pitfalls

- **Duplicate IDs**: The loader raises `ValueError` at import time. Each `id` must be unique across ALL modules.
- **En-dashes/smart quotes**: ruff RUF001/RUF003 flags Unicode confusables. Use plain ASCII hyphens `-` and quotes `"`.
- **ARG002 suppressed**: `require_admin` kwarg is part of the API contract; OK to leave unused in HKCU-only tweaks.
- **Python path**: Use `C:\Users\ryair\AppData\Local\Python\bin\python.exe`, not the WindowsApps alias.
- **Line length**: 150 chars (configured in pyproject.toml and .vscode/settings.json).

## File-by-File Quick Ref

| File                  | Purpose             | Key exports                                                                                                                                                                                       |
| --------------------- | ------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `__init__.py` (root)  | Version             | `__version__`                                                                                                                                                                                     |
| `__main__.py`         | Entry point         | delegates to `cli.main()`                                                                                                                                                                         |
| `cli.py`              | argparse CLI        | `main()`                                                                                                                                                                                          |
| `menu.py`             | Console menu        | `Menu` class                                                                                                                                                                                      |
| `gui.py`              | Tkinter GUI         | `RegiLatticeGUI` class, `launch()`                                                                                                                                                                |
| `gui_widgets.py`      | Row/section widgets | `TweakRow`, `CategorySection`                                                                                                                                                                     |
| `gui_theme.py`        | Theme engine        | `set_theme()`, `available_themes()`, `current_theme()`, colour & font constants                                                                                                                   |
| `gui_tooltip.py`      | Tooltips            | `Tooltip`, `build_tooltip_text()`, `has_recommendation()`, `parse_description_metadata()`                                                                                                         |
| `gui_dialogs.py`      | Dialogs             | `import_json_selection()`, `export_powershell()`, `open_scoop_manager()`, `show_about()`                                                                                                          |
| `config.py`           | User config         | `AppConfig`, `load_config()`                                                                                                                                                                      |
| `registry.py`         | Registry wrapper    | `SESSION`, `RegistrySession`, `assert_admin`, `is_windows`, `platform_summary`                                                                                                                    |
| `corpguard.py`        | Corp detection      | `is_corporate_network()`, `assert_not_corporate()`, `corp_guard_status()`, `CorporateNetworkError`                                                                                                |
| `elevation.py`        | UAC helpers         | `is_admin()`, `request_elevation()`, `run_elevated()`, `ensure_admin_or_elevate()`                                                                                                                |
| `deps.py`             | Lazy imports        | `lazy_import()`, `install_package()`, `require()`                                                                                                                                                 |
| `analytics.py`        | Usage analytics     | `record_apply()`, `record_remove()`, `record_session()`, `get_stats()`, `top_tweaks()`                                                                                                            |
| `ratings.py`          | Rating system       | `rate_tweak()`, `get_rating()`, `all_ratings()`, `remove_rating()`, `top_rated()`                                                                                                                 |
| `locale.py`           | i18n string table   | `t()`, `set_locale()`, `load_locale_file()`, `current_locale()`, `available_keys()`                                                                                                               |
| `hwinfo.py`           | Hardware detection  | `detect_hardware()`, `detect_cpu()`, `detect_gpus()`, `detect_memory()`, `detect_disk()`, `suggest_profile()`, `hardware_summary()`, `HWProfile`                                                  |
| `marketplace.py`      | Plugin marketplace  | `discover_plugins()`, `load_plugin()`, `loaded_plugins()`, `unload_plugin()`, `PluginMeta`                                                                                                        |
| `tweaks/__init__.py`  | Core engine         | `TweakDef`, `all_tweaks()`, `get_tweak()`, `categories()`, `tweaks_by_category()`, `search_tweaks()`, `apply_profile()`, `status_map()`, `tweak_scope()`, `save_snapshot()`, `restore_snapshot()` |
| `tweaks/_template.py` | Contributor guide   | (not loaded)                                                                                                                                                                                      |
