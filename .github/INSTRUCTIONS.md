# regilattice — AI / Contributor Instructions

> Canonical reference for any AI agent, copilot, or human contributor
> working on this repository.

## 1  Project Overview

regilattice is a **Windows 11 registry-tweak toolkit** with three interfaces:

| Interface | Technology | Entry point |
|---|---|---|
| PowerShell menu | PS 5.1+ | `Launch-RegiLattice.bat` → `RegiLatticeMenu.ps1` |
| Python console menu | Python 3.10+ | `python -m regilattice` |
| Python GUI | tkinter (stdlib) | `python -m regilattice --gui` |

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

- `regilattice/state.py` persists tweak status to
  `~/.regilattice/state.json`.
- Before every apply/remove, the previous state and registry snapshot
  are saved so the user can **undo** the last action from the GUI.
- The GUI shows a live status badge (✅ Applied / ⚪ Default) per tweak
  by calling each `TweakDef.detect_fn`.

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
│   ├── WORKFLOW.md              ← dev workflow
│   └── workflows/
│       ├── powershell.yml       ← PSScriptAnalyzer CI
│       └── python.yml           ← ruff + pytest CI
├── regilattice/
│   ├── __init__.py
│   ├── __main__.py
│   ├── cli.py                   ← argparse CLI
│   ├── menu.py                  ← console menu
│   ├── gui.py                   ← tkinter GUI
│   ├── registry.py              ← winreg helpers & session
│   ├── corpguard.py             ← corporate network detection
│   ├── state.py                 ← JSON state/undo persistence
│   └── tweaks/
│       ├── __init__.py          ← plugin loader
│       ├── shell.py             ← Take Ownership
│       ├── explorer.py          ← Recent Folders
│       ├── boot.py              ← Verbose Boot
│       ├── performance.py       ← Perf tweaks, SvcHost, Last Access
│       ├── privacy.py           ← Telemetry, Cortana
│       ├── input.py             ← Mouse Acceleration
│       ├── gaming.py            ← Game DVR
│       ├── system.py            ← Long Paths, Registry Backup
│       └── wsl.py               ← WSL optimisation tweaks
├── tests/
├── *.ps1                        ← PowerShell tweak scripts
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
