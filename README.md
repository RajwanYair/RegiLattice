# ⚡ RegiLattice

A comprehensive Windows registry tweak toolkit with **1 228 tweaks** across **64 categories**, a **zero-wiring plugin architecture**, a **Python CLI**, **interactive console menu**, and a **tkinter GUI** (Catppuccin Mocha dark theme). Designed for power users who want fine-grained control over Windows 10/11 performance, privacy, usability, and application behaviour.

## Highlights

- **1 228 tweaks** across 64 categories — each fully reversible with apply + remove
- **Plugin architecture** — auto-discovers tweaks from `regilattice/tweaks/`, no registration needed
- **3 interfaces** — interactive console menu, CLI with flags, and tkinter GUI
- **GUI** — 4 switchable themes (Catppuccin Mocha/Latte, Nord, Dracula), menu bar (File/Edit/View/Help), zebra-striped rows, collapsible categories, scope badges (USER/MACHINE/BOTH), recommendation badges, rich hover tooltips, live search with status/scope filters
- **5 machine profiles** — business, gaming, privacy, minimal, server
- **Dry-run mode** — preview changes without touching the registry (`--dry-run`)
- **Snapshot & diff** — save/restore tweak state (JSON), compare snapshots (`--snapshot-diff`)
- **Dependency graph** — tweaks can declare `depends_on` ordering; batch ops respect topological order
- **Config file** — persistent defaults via `~/.regilattice.toml` (`--config`)
- **Concurrent batch operations** — `ThreadPoolExecutor`-powered parallel apply/remove/detect
- **UAC elevation** — automatic admin re-launch via `ctypes.ShellExecuteW`
- **Corporate network safety** — blocks tweaks on domain-joined, Azure AD, VPN, and managed machines
- **Automatic backups** — every registry mutation is backed up before changes with rollback on error
- **Export PowerShell** — generate `.ps1` scripts from selected tweaks for portable deployment
- **~16 400 tests** across 14 test files — full smoke, CLI, GUI, and engine coverage

## Architecture

```mermaid
graph LR
    subgraph Interfaces
        CLI[cli.py]
        Menu[menu.py]
        GUI[gui.py]
    end

    subgraph Core
        TI[tweaks/__init__.py<br/>TweakDef · TweakExecutor<br/>ProfileDef · _topo_sort]
        REG[registry.py<br/>RegistrySession]
        CFG[config.py<br/>AppConfig]
        CG[corpguard.py]
    end

    subgraph Plugins
        P1[performance.py]
        P2[privacy.py]
        P3[gaming.py]
        PN[... 61 more]
    end

    CLI --> TI
    Menu --> TI
    GUI --> TI
    TI --> REG
    TI --> CG
    CLI --> CFG
    P1 --> TI
    P2 --> TI
    P3 --> TI
    PN --> TI
    REG -->|winreg / reg.exe| WR[(Windows Registry)]
```

## Tweak Categories (64)

| Category              | #  | Category              | #  |
|-----------------------|----|-----------------------|----|
| Accessibility         | 20 | Multimedia            | 15 |
| Adobe                 | 20 | Network               | 22 |
| AI / Copilot          | 22 | Notifications         | 16 |
| Audio                 | 19 | Office                | 20 |
| Backup & Recovery     | 15 | OneDrive              | 18 |
| Bluetooth             | 19 | Package Management    | 21 |
| Boot                  | 21 | Performance           | 20 |
| Chrome                | 20 | Power                 | 21 |
| Clipboard & Drag-Drop | 15 | Printing              | 15 |
| Cloud Storage         | 30 | Privacy               | 25 |
| Communication         | 21 | RealVNC               | 15 |
| Context Menu          | 15 | Remote Desktop        | 16 |
| Cortana & Search      | 22 | Scheduled Tasks       | 16 |
| Crash & Diagnostics   | 16 | Scoop Tools           | 25 |
| Developer Tools       | 17 | Screensaver & Lock    | 16 |
| Display               | 19 | Security              | 21 |
| DNS & Networking Advanced | 16 | Services           | 21 |
| Edge                  | 18 | Shell                 | 20 |
| Explorer              | 41 | Snap & Multitasking   | 17 |
| File System           | 17 | Startup               | 19 |
| Firefox               | 20 | Storage               | 19 |
| Fonts                 | 19 | System                | 24 |
| Gaming                | 19 | Taskbar               | 19 |
| GPU / Graphics        | 19 | Telemetry Advanced    | 16 |
| Indexing & Search     | 16 | USB & Peripherals     | 16 |
| Input                 | 18 | Virtualization        | 15 |
| Java                  | 16 | VS Code               | 19 |
| LibreOffice           | 18 | Widgets & News        | 15 |
| Lock Screen & Login   | 16 | Windows 11            | 29 |
| M365 Copilot          | 18 | Windows Terminal       | 16 |
| Maintenance           | 17 | Windows Update        | 18 |
| Microsoft Store       | 15 | WSL                   | 29 |

## Requirements

- **Windows 10/11** (tested on 22H2+)
- **Python 3.10+** (3.14 recommended)
- Administrator privileges for HKLM tweaks (auto-elevates via UAC prompt)

## Quick Start

### GUI (Recommended)
```bash
python -m regilattice --gui
```
Tkinter window with 4 themes (Catppuccin Mocha default), menu bar, zebra-striped rows, per-category grouping, live search bar, scope badges (USER/MACHINE/BOTH), recommendation badges, per-row toggle buttons, and batch operations.

### Console Menu
```bash
python -m regilattice
```
Two-level interactive menu: browse categories, then select tweaks within each category.

### CLI
```bash
python -m regilattice apply disable-telemetry -y
python -m regilattice remove all --assume-yes
python -m regilattice --list
python -m regilattice --dry-run apply all
python -m regilattice --snapshot state.json
python -m regilattice --restore state.json
python -m regilattice --snapshot-diff before.json after.json
python -m regilattice --list-profiles
python -m regilattice --categories
python -m regilattice --tags
```

### Machine Profiles
```bash
python -m regilattice --profile business   # 23 categories — productivity, security, cloud & workflow
python -m regilattice --profile gaming     # 20 categories — GPU, performance, low-latency, distraction-free
python -m regilattice --profile privacy    # 21 categories — telemetry, tracking, cloud & browser data
python -m regilattice --profile minimal    # 15 categories — fast, clean system operation essentials
python -m regilattice --profile server     # 19 categories — hardened, headless, uptime & remote mgmt
```

### Windows Launcher
```
Launch-RegiLattice.ps1              # auto-detects Python, passes CLI args
Launch-RegiLattice.ps1 --gui        # launch GUI directly
```

## Corporate Network Safety

Automatically detects corporate environments and **blocks non-safe tweaks** to prevent policy violations:

- **Active Directory** domain membership (ctypes `GetComputerNameExW`)
- **Azure AD / Entra ID** join status (`dsregcmd /status`)
- **VPN adapters** — Cisco AnyConnect, GlobalProtect, Zscaler, WireGuard, etc.
- **Group Policy** registry indicators
- **SCCM / Intune** management agents

Override with `--force` (CLI) or the "Force" checkbox (GUI) at your own risk.

## Project Structure

```
RegiLattice/
├── pyproject.toml                   # Build config (hatchling)
├── Launch-RegiLattice.ps1           # PowerShell launcher
├── regilattice/                     # Python package
│   ├── __init__.py                  # Package version
│   ├── __main__.py                  # python -m regilattice -> cli.main()
│   ├── cli.py                       # argparse CLI entry point
│   ├── menu.py                      # Interactive console menu
│   ├── gui.py                       # tkinter GUI (Catppuccin Mocha, ~1 281 lines)
│   ├── gui_theme.py                 # Theme constants (colours, fonts)
│   ├── gui_tooltip.py               # Tooltip widget + description metadata parser
│   ├── gui_widgets.py               # TweakRow + CategorySection widgets
│   ├── gui_dialogs.py               # Import/export/about dialogs
│   ├── config.py                    # ~/.regilattice.toml support
│   ├── deps.py                      # Smart dependency management
│   ├── elevation.py                 # UAC elevation helpers
│   ├── registry.py                  # RegistrySession: winreg wrapper + backup + logging
│   ├── corpguard.py                 # Corporate network detection
│   └── tweaks/                      # Plugin-based tweak registry (64 modules)
│       ├── __init__.py              # TweakDef, TweakExecutor, ProfileDef, plugin loader
│       ├── _template.py             # Contributor guide -- copy to add a new tweak
│       ├── accessibility.py         # Accessibility (20 tweaks)
│       ├── ...                      # 62 more category modules, auto-discovered
│       └── wsl.py                   # WSL (29 tweaks)
├── tests/                           # pytest suites (~16 400 tests across 14 files)
│   ├── conftest.py                  # dry_session fixture, all_tweaks_list
│   ├── test_tweaks_smoke.py         # Auto-parametrized over all tweaks
│   ├── test_tweaks_init.py          # Plugin loader, profiles, batch ops
│   ├── test_cli.py                  # CLI argument parsing and commands
│   ├── test_config.py               # AppConfig loading
│   ├── test_corpguard.py            # Corporate network detection
│   ├── test_deps.py                 # lazy_import, install_package, require
│   ├── test_elevation.py            # UAC elevation helpers
│   ├── test_gui_dialogs.py          # PS1 export, JSON import, about dialog
│   ├── test_gui_theme.py            # Theme switching, colour validation
│   ├── test_gui_tooltip.py          # Tooltip text, metadata parsing
│   ├── test_gui_widgets.py          # Tweak scope classification
│   ├── test_menu.py                 # Interactive console menu
│   └── test_registry.py             # RegistrySession helpers and backup
├── .github/                         # CI, templates, architecture docs
└── .vscode/                         # Workspace settings
```

## Adding a Custom Tweak

Copy `regilattice/tweaks/_template.py` to a new file and follow the numbered steps inside.
See [CONTRIBUTING.md](CONTRIBUTING.md) for the full guide.

**Quick version:** create a `.py` file in `regilattice/tweaks/` exporting a `TWEAKS` list:

```python
from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

_KEY = r"HKEY_CURRENT_USER\Software\MyApp"

def _apply(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY], "FancyMode")
    SESSION.set_dword(_KEY, "FancyMode", 1)

def _remove(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_KEY, "FancyMode")

def _detect() -> bool:
    return SESSION.read_dword(_KEY, "FancyMode") == 1

TWEAKS = [
    TweakDef(
        id="myapp-fancy-mode",
        label="Enable Fancy Mode",
        category="My App",
        apply_fn=_apply,
        remove_fn=_remove,
        detect_fn=_detect,
        needs_admin=False,
        registry_keys=[_KEY],
        description="Enables Fancy Mode in MyApp.",
        tags=["myapp", "fancy", "ui"],
    ),
]
```

The plugin loader discovers it automatically -- no registration code needed.

## License

MIT -- see [LICENSE](LICENSE) for details.
