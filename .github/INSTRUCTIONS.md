# regilattice — AI / Contributor Instructions

> Canonical reference for any AI agent, copilot, or human contributor
> working on this repository.

## 1  Project Overview

regilattice is a **Windows 11 registry-tweak toolkit** with three interfaces:

| Interface | Technology | Entry point |
|---|---|---|
| Python console menu | Python 3.10+ | `python -m regilattice` |
| Python GUI | tkinter (stdlib) | `python -m regilattice --gui` |
| Python CLI | argparse | `python -m regilattice apply <id> -y` |

All tweaks are **reversible**, **backed-up before mutation**, and
**blocked on corporate networks** unless explicitly forced.

## 2  Architecture — Plugin-Based TweakDef

Every registry tweak is declared as a **`TweakDef`** dataclass inside
`regilattice/tweaks/` sub-modules (one file per category).  The plugin
loader (`regilattice/tweaks/__init__.py`) auto-discovers every `TweakDef`
instance exported by those sub-modules.

```python
@dataclass
class TweakDef:
    id: str                     # unique slug, e.g. "disable-telemetry"
    label: str                  # human-readable name
    category: str               # grouping for UI
    apply_fn: Callable          # function that applies the tweak
    remove_fn: Callable         # function that reverts the tweak
    detect_fn: Callable → bool  # returns True when the tweak is active
    needs_admin: bool           # True if HKLM / fsutil required
    corp_safe: bool             # True → still allowed on corp networks
    registry_keys: list[str]    # keys touched (for backup/probing)
```

### Adding a new tweak

1. Create or edit a file in `regilattice/tweaks/<category>.py`.
2. Define `apply_*`, `remove_*`, and `detect_*` functions.
3. Append a `TweakDef(...)` instance to the module-level `TWEAKS` list.
4. The GUI, CLI, and menu pick it up automatically — **no manual wiring**.
5. If the tweak touches **any HKLM GPO path** set `corp_safe = False`.

### Corp-safety rule

```
if corp_guard.is_corporate_network() and not tweak.corp_safe:
    → block unless user passes --force / Force checkbox
```

## 3  Coding Standards

### Python
- Python 3.10+ syntax; `from __future__ import annotations`.
- Formatting / linting: **ruff** (line-length 100, PEP 8 baseline).
- Type-checking: **mypy --strict**.
- Tests: **pytest** in `tests/`.
- No third-party runtime deps — stdlib only (tkinter, winreg, ctypes).

### PowerShell
- `#Requires -Version 5.1` header.
- `Set-StrictMode -Version Latest; $ErrorActionPreference = 'Stop'`.
- Shared libs: `Lib-RegiLattice.ps1`, `Lib-BackupRegistry.ps1`,
  `Lib-CorpGuard.ps1`.  Dot-source from `$PSScriptRoot`.
- Every tweak script sources the shared libs, calls `Assert-Elevated`,
  `Confirm-Action`, backs up keys, applies or removes, logs via
  `Write-TurboLog`, then pauses.

### Git
- Conventional Commits: `feat:`, `fix:`, `docs:`, `test:`, `refactor:`,
  `chore:`, `ci:`, `assets:`.
- One logical change per commit.
- Run `ruff check regilattice/ tests/` and `pytest` before pushing.

## 4  State / Undo System

- `regilattice/tweaks/__init__.py` exposes `save_snapshot(path)` and
  `restore_snapshot(path, registry_session)` which serialise/deserialise
  the full tweak state to a JSON file.
- Before every apply/remove, the previous state and registry snapshot
  are saved so the user can **undo** the last action from the GUI.
- The GUI shows a live status badge (✅ Applied / ⚪ Default) per tweak
  by calling each `TweakDef.detect_fn`, rendered as per-row
  ENABLED / DISABLED toggle buttons.

## 5  Corporate Guard

Module: `regilattice/corpguard.py` + `Lib-CorpGuard.ps1`.

**Checks (in order):**
1. AD domain membership (ctypes `GetComputerNameExW` + WMI fallback)
2. Azure AD / Entra ID join (`dsregcmd /status`)
3. VPN adapter keywords (`Get-NetAdapter`)
4. Group Policy registry indicators
5. SCCM / Intune management enrollment

**Rules:**
- If any check is positive → `is_corporate_network() == True`.
- Tweaks with `corp_safe = False` are blocked.
- `corp_safe = True` tweaks (HKCU-only, no GPO) are allowed even on
  corp networks.
- Override: `--force` flag / GUI "Force" checkbox (logged).

## 6  File Layout

```
regilattice/
├── .github/
│   ├── INSTRUCTIONS.md          ← this file
│   ├── SKILLS.md                ← reusable patterns
│   └── workflows/
│       └── python.yml           ← ruff + mypy + pytest CI
├── regilattice/
│   ├── __init__.py
│   ├── __main__.py
│   ├── cli.py                   ← argparse CLI
│   ├── menu.py                  ← console menu
│   ├── gui.py                   ← tkinter GUI
│   ├── deps.py                  ← smart dependency management
│   ├── elevation.py             ← UAC elevation helpers
│   ├── registry.py              ← winreg helpers & session
│   ├── corpguard.py             ← corporate network detection
│   └── tweaks/                  ← plugin-based tweak registry (34 modules)
│       ├── __init__.py          ← TweakDef dataclass, plugin loader, save/restore snapshot
│       ├── accessibility.py     ← Accessibility & visual aids (7)
│       ├── adobe.py             ← Adobe Reader / Acrobat (6)
│       ├── bluetooth.py         ← Bluetooth power & audio (5)
│       ├── boot.py              ← Boot tweaks (3)
│       ├── chrome.py            ← Google Chrome policies (6)
│       ├── communication.py     ← Teams, Zoom, Discord, Spotify, Slack (9)
│       ├── copilot.py           ← Windows Copilot / Recall (4)
│       ├── cortana.py           ← Cortana & Search (6)
│       ├── defender.py          ← Windows Security / Defender (7)
│       ├── edge.py              ← Microsoft Edge policies (6)
│       ├── explorer.py          ← Windows Explorer (14)
│       ├── firefox.py           ← Mozilla Firefox policies (5)
│       ├── gaming.py            ← Gaming tweaks (3)
│       ├── gitconfig.py         ← Git for Windows (5)
│       ├── input.py             ← Input tweaks (3)
│       ├── java.py              ← Java runtime (4)
│       ├── libreoffice.py       ← LibreOffice / OpenOffice (6)
│       ├── maintenance.py       ← Registry auto-backup & cleanup (3)
│       ├── network.py           ← Network / connectivity (8)
│       ├── office.py            ← Microsoft Office multi-version (8)
│       ├── onedrive.py          ← OneDrive policies (6)
│       ├── performance.py       ← System performance (5)
│       ├── pkgmgmt.py           ← Package managers (7)
│       ├── power.py             ← Power management (8)
│       ├── privacy.py           ← Windows privacy (10)
│       ├── realvnc.py           ← RealVNC Server & Viewer (8)
│       ├── services.py          ← Windows services (6)
│       ├── shell.py             ← Shell context menu (3)
│       ├── startup.py           ← Startup programs (6)
│       ├── system.py            ← System capabilities (3)
│       ├── vscode.py            ← VS Code policies (5)
│       ├── win11.py             ← Windows 11 UI debloating (11)
│       ├── windowsupdate.py     ← Windows Update policies (8)
│       └── wsl.py               ← WSL optimisation (8)
├── tests/                       ← 2 267 tests across 8 test files
├── pyproject.toml
└── README.md
```

## 7  GUI Guidelines

- **Dark theme** inspired by WinUI 3 / Fluent Design.
- Mica-style background tones (`#1E1E2E`), rounded card look.
- Two-state toggle per tweak: shows live status via `detect_fn`.
- Category headers, scrollable list, progress bar, status bar.
- Threaded execution — never block the UI thread.
- "Create Restore Point" action always available.
