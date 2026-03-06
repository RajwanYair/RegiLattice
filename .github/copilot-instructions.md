# RegiLattice — Copilot Instructions

> Auto-loaded by GitHub Copilot on every chat/agent session in this workspace.
> Keep this file accurate — it is the fastest path to project understanding.
> Last verified: 2025-06-22 (v2.0.0, 1 231 tweaks, 64 categories, ~13 695 tests).

## Quick Facts

| Key         | Value                                                                        |
| ----------- | ---------------------------------------------------------------------------- |
| Language    | Python 3.10+ (stdlib only at runtime)                                        |
| Build       | `hatchling` via `pyproject.toml`                                             |
| Lint        | `ruff` (E, F, W, I, UP, B, SIM, RUF; line-length 150; ignore ARG002)         |
| Type-check  | `mypy --strict`                                                              |
| Test        | `pytest` in `tests/` (~13 695 tests)                                         |
| GUI         | tkinter with Catppuccin Mocha dark theme (~1 432 lines)                      |
| Version     | 2.0.0                                                                        |
| Python path | `C:\Users\ryair\AppData\Local\Python\bin\python.exe` (NOT WindowsApps alias) |
| Install     | `pip install -e ".[dev]"`                                                    |

## Architecture at a Glance

```
regilattice/
├── __init__.py          # package version (__version__)
├── __main__.py          # entry: delegates to cli.main()
├── cli.py               # argparse CLI (apply/remove/list/gui/profile)
├── menu.py              # interactive numbered console menu
├── gui.py               # tkinter GUI (~1 432 lines, Catppuccin Mocha theme)
├── registry.py          # RegistrySession: winreg wrapper + backup + logging
├── corpguard.py         # corporate network detection (domain/AAD/VPN/GPO/SCCM)
├── elevation.py         # UAC elevation helpers (is_admin, request_elevation)
├── deps.py              # lazy-import + auto-install for optional packages
└── tweaks/
    ├── __init__.py      # TweakDef dataclass, plugin loader, profiles, batch ops
    ├── _template.py     # contributor guide (not loaded by plugin loader)
    ├── accessibility.py # 64 category modules ...
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

| Slug | Category | Slug | Category |
| --- | --- | --- | --- |
| `acc` | Accessibility | `lo` | LibreOffice |
| `adobe` | Adobe | `lock` | Lock Screen & Login |
| `ai` | AI / Copilot | `m365` | M365 Copilot |
| `audio` | Audio | `maint` | Maintenance |
| `backup` | Backup & Recovery | `media` | Multimedia |
| `boot` | Boot | `msstore` | Microsoft Store |
| `bt` | Bluetooth | `net` | Network |
| `chrome` | Chrome | `notif` | Notifications |
| `clip` | Clipboard & Drag-Drop | `od` | OneDrive |
| `cloud` | Cloud Storage | `office` | Office |
| `comm` | Communication | `perf` | Performance |
| `cortana` | Cortana & Search | `pkg` | Package Management |
| `crash` | Crash & Diagnostics | `power` | Power |
| `ctx` | Context Menu | `printing` | Printing |
| `dev` | Developer Tools | `priv` | Privacy |
| `display` | Display | `rdp` | Remote Desktop |
| `dns` | DNS & Networking Adv | `schtask` | Scheduled Tasks |
| `edge` | Edge | `scoop` | Scoop Tools |
| `explorer` | Explorer | `sec` | Security |
| `firefox` | Firefox | `shell` | Shell |
| `font` | Fonts | `snap` | Snap & Multitasking |
| `fs` | File System | `ss` | Screensaver & Lock |
| `game` | Gaming | `startup` | Startup |
| `gpu` | GPU / Graphics | `stor` | Storage |
| `idx` | Indexing & Search | `svc` | Services |
| `input` | Input | `sys` | System |
| `java` | Java | `tb` | Taskbar |
| `telem` | Telemetry Advanced | `usb` | USB & Peripherals |
| `term` | Windows Terminal | `virt` | Virtualization |
| `vnc` | RealVNC | `vscode` | VS Code |
| `w11` | Windows 11 | `widgets` | Widgets & News |
| `wsl` | WSL | `wu` | Windows Update |

## Current Stats (1 231 tweaks, 64 categories, 64 modules)

| Category              | Tweaks | Category            | Tweaks |
| --------------------- | ------ | ------------------- | ------ |
| Accessibility         | 18     | Lock Screen & Login | 13     |
| Adobe                 | 17     | M365 Copilot        | 15     |
| AI / Copilot          | 15     | Maintenance         | 15     |
| Audio                 | 16     | Microsoft Store     | 12     |
| Backup & Recovery     | 12     | Multimedia          | 12     |
| Bluetooth             | 16     | Network             | 19     |
| Boot                  | 18     | Notifications       | 13     |
| Chrome                | 17     | Office              | 15     |
| Clipboard & Drag-Drop | 12     | OneDrive            | 15     |
| Cloud Storage         | 27     | Package Management  | 19     |
| Communication         | 18     | Performance         | 18     |
| Context Menu          | 12     | Power               | 19     |
| Cortana & Search      | 17     | Printing            | 12     |
| Crash & Diagnostics   | 13     | Privacy             | 14     |
| Developer Tools       | 14     | RealVNC             | 12     |
| Display               | 16     | Remote Desktop      | 13     |
| DNS & Networking Adv  | 14     | Scheduled Tasks     | 13     |
| Edge                  | 15     | Scoop Tools         | 22     |
| Explorer              | 21     | Screensaver & Lock  | 13     |
| File System           | 14     | Security            | 16     |
| Firefox               | 17     | Services            | 17     |
| Fonts                 | 16     | Shell               | 15     |
| Gaming                | 15     | Snap & Multitasking | 14     |
| GPU / Graphics        | 17     | Startup             | 14     |
| Indexing & Search     | 13     | Storage             | 16     |
| Input                 | 15     | System              | 14     |
| Java                  | 13     | Taskbar             | 14     |
| LibreOffice           | 15     | Telemetry Advanced  | 13     |
|                       |        | USB & Peripherals   | 13     |
|                       |        | Virtualization      | 12     |
|                       |        | VS Code             | 16     |
|                       |        | Widgets & News      | 12     |
|                       |        | Windows 11          | 15     |
|                       |        | Windows Terminal    | 13     |
|                       |        | Windows Update      | 17     |
|                       |        | WSL                 | 13     |

## 5 Profiles

| Profile    | Description             | Apply Categories                                                                                        |
| ---------- | ----------------------- | ------------------------------------------------------------------------------------------------------- |
| `business` | Productivity & security | Cloud Storage, Office, Communication, OneDrive, Security, Network, Privacy, Printing, Backup & Recovery |
| `gaming`   | GPU & performance       | Gaming, GPU, Performance, Display, Audio, Network, Power, Services                                      |
| `privacy`  | Max privacy             | Privacy, Cortana & Search, AI / Copilot, Windows 11, Cloud Storage, Communication                       |
| `minimal`  | Lightweight essentials  | Performance, Startup, Maintenance, Boot, Power, Services                                                |
| `server`   | Server hardening        | Network, Power, Services, Security, Virtualization, Backup & Recovery                                   |

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

## GUI Details (`gui.py`, ~1 432 lines)

- Catppuccin Mocha palette: `#1E1E2E` (base), `#89B4FA` (accent), etc.
- Collapsible category sections with tweak counts (applied/total)
- **Scope badges**: USER (green) / MACHINE (blue) / BOTH (yellow) per tweak row
- Search bar + status filter (All / Applied / Default / Unknown) + scope filter (User/Machine/Both)
- Profile selector dropdown (Business / Gaming / Privacy / Minimal / Server)
- Export PS1 button (generates PowerShell script from selected tweaks)
- Import JSON button (load tweak ID list from file to select)
- Threaded execution — never blocks UI thread
- Live status badges via `detect_fn()` with parallel detection (`status_map(parallel=True)`)
- Rich hover tooltips with description, current state, default/recommendation hints, tags, registry keys
- Right-click context menu on tweak rows (Enable/Disable, Copy ID, Copy Registry Key, Select category)
- Recommendation badges (teal "REC" tag) for tweaks with recommendations in description
- Summary stats bar: Applied / Default / Unknown / Recommended / GPO counts
- Per-category Enable All / Disable All buttons
- Toggleable log viewer panel (shows session log inline)
- About dialog with system info and shortcut reference
- Invert Selection button
- Keyboard shortcuts: Ctrl+A/D/F/E/I/L/R, Esc
- `_parse_description_metadata()` extracts `Default:` and `Recommended:` from description text
- `functools.lru_cache` on description parsing for performance
- `tweak_scope()` classifies tweaks as user/machine/both from registry keys

## Test Infrastructure

- `tests/conftest.py`: `dry_session` fixture (RegistrySession with `_dry_run=True`), `all_tweaks_list` session fixture
- `tests/test_tweaks_smoke.py`: auto-parametrized over all tweaks — tests triplet signatures, ID uniqueness, detect_fn callability
- `tests/test_tweaks_init.py`: plugin loader, categories, profiles, search, batch ops
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

| File                  | Purpose           | Key exports                                                                                                                                                                                       |
| --------------------- | ----------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `__init__.py` (root)  | Version           | `__version__`                                                                                                                                                                                     |
| `__main__.py`         | Entry point       | delegates to `cli.main()`                                                                                                                                                                         |
| `cli.py`              | argparse CLI      | `main()`                                                                                                                                                                                          |
| `menu.py`             | Console menu      | `Menu` class                                                                                                                                                                                      |
| `gui.py`              | Tkinter GUI       | `RegiLatticeGUI` class                                                                                                                                                                            |
| `registry.py`         | Registry wrapper  | `SESSION`, `RegistrySession`, `assert_admin`, `is_windows`, `platform_summary`                                                                                                                    |
| `corpguard.py`        | Corp detection    | `is_corporate_network()`, `assert_not_corporate()`, `corp_guard_status()`, `CorporateNetworkError`                                                                                                |
| `elevation.py`        | UAC helpers       | `is_admin()`, `request_elevation()`                                                                                                                                                               |
| `deps.py`             | Lazy imports      | `lazy_import()`                                                                                                                                                                                   |
| `tweaks/__init__.py`  | Core engine       | `TweakDef`, `all_tweaks()`, `get_tweak()`, `categories()`, `tweaks_by_category()`, `search_tweaks()`, `apply_profile()`, `status_map()`, `tweak_scope()`, `save_snapshot()`, `restore_snapshot()` |
| `tweaks/_template.py` | Contributor guide | (not loaded)                                                                                                                                                                                      |
