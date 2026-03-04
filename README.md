# ⚡ RegiLattice

A comprehensive Windows registry tweak toolkit with **plugin architecture**, **PowerShell scripts**, a **Python CLI/menu**, and a **tkinter GUI**. Designed for power users who want fine-grained control over Windows 11 performance, privacy, usability, and application behaviour.

## Highlights

- **40+ tweaks** across 15 categories — each fully reversible with apply + remove
- **Plugin architecture** — auto-discovers tweaks, easy to extend
- **3 interfaces** — interactive PS menu, Python console menu, and tkinter GUI
- **Smart dependency management** — auto-installs missing pip packages with cascading fallbacks
- **Corporate network safety** — auto-detects domain-joined, Azure AD, VPN, and managed machines
- **Automatic backups** — every registry mutation is backed up before changes
- **Snapshot/Undo** — save and restore tweak state snapshots (JSON)
- **Shared libraries** — `Lib-RegiLattice.ps1`, `Lib-BackupRegistry.ps1`, `Lib-CorpGuard.ps1`

## Tweak Categories

| Category | Tweaks | Description |
|---|---|---|
| **AI / Copilot** | Disable Copilot, Disable Recall | Windows 11 AI features |
| **Boot** | Verbose Boot Messages | Startup diagnostics |
| **Chrome** | Background Apps, Telemetry, Auto-Update, HW Accel | Google Chrome policies |
| **Edge** | Startup Boost, Sidebar, Telemetry, Auto-Update | Microsoft Edge policies |
| **Explorer** | Recent Folders in Quick Access | File Explorer enhancements |
| **Firefox** | Telemetry, Pocket, Auto-Update | Mozilla Firefox policies |
| **Gaming** | Disable Game DVR / Game Bar | Gaming performance |
| **Input** | Disable Mouse Acceleration | Raw input |
| **Maintenance** | Registry Auto-Backup | System health |
| **Office** | Telemetry, Start Screen, Connected Experiences | Microsoft Office policies |
| **Package Management** | PS Policy, PSGallery, Scoop, Winget, Pip | Developer tools |
| **Performance** | Startup Delay, SvcHost Split, NTFS Last Access | System performance |
| **Privacy** | Telemetry, Cortana | Windows privacy |
| **Shell** | Take Ownership Context Menu | Right-click enhancements |
| **System** | Long Paths | System capabilities |
| **VS Code** | Telemetry, Auto-Update, Extension Update | VS Code policies |
| **Windows 11** | Widgets, Snap, Context Menu, Lock Screen, WU, Bing, Bloatware, Dark Mode, Notifications | UI debloating |
| **WSL** | Default V2, Auto-Start, Nested Virt, Feature, VM Platform, Mirrored Network | WSL optimisation |

## Requirements

- **Windows 11** (tested 22H2+)
- **PowerShell 5.1+** (PowerShell 7 / pwsh.exe preferred)
- **Python 3.10+** (for the Python CLI/menu/GUI)
- Administrator privileges for HKLM tweaks (scripts auto-elevate)

## Quick Start

### PowerShell Menu
```
Launch-RegiLattice.bat
```
Right-click → Run as administrator. The interactive menu offers all tweaks plus batch apply/remove and restore point creation.

### Python Console Menu
```bash
python -m regilattice
```

### Python GUI
```bash
python -m regilattice --gui
```
A dark-themed tkinter window with checkboxes, status indicators, category grouping, and snapshot management.

### Single Tweak (CLI)
```bash
python -m regilattice apply disable-telemetry -y
python -m regilattice remove all --assume-yes
python -m regilattice --list
python -m regilattice --snapshot state.json
python -m regilattice --restore state.json
```

## Smart Dependency Management

RegiLattice includes a cascading dependency installer (`regilattice.deps`):

```python
from regilattice.deps import lazy_import, require

requests = lazy_import("requests")     # auto-installs if missing
require("rich", "requests")            # ensure all listed packages exist
```

Fallback order: user-space pip → system-wide pip → ensurepip bootstrap → graceful sentinel.

## Corporate Network Safety

RegiLattice automatically detects corporate environments and **blocks all tweaks** to prevent accidental policy violations:

- **Active Directory** domain membership (WMI / ctypes)
- **Azure AD / Entra ID** join status (`dsregcmd /status`)
- **VPN adapters** — Cisco AnyConnect, GlobalProtect, Zscaler, WireGuard, etc.
- **SCCM / Intune** management agents

Override with `--force` (CLI) or the "Force" checkbox (GUI) at your own risk.

## Project Structure

```
RegiLattice/
├── Launch-RegiLattice.bat           # Launcher (pwsh → powershell fallback)
├── RegiLatticeMenu.ps1              # Interactive PS menu
├── Lib-RegiLattice.ps1              # Shared PS utilities
├── Lib-BackupRegistry.ps1           # Registry backup helper
├── Lib-CorpGuard.ps1                # Corporate network detection (PS)
├── System_Restore_Point.ps1         # Create system restore point
├── Add-*.ps1 / Remove-*.ps1        # Individual tweak scripts (12 pairs)
├── regilattice/                     # Python package
│   ├── __init__.py
│   ├── __main__.py                  # python -m regilattice
│   ├── cli.py                       # argparse CLI entry point
│   ├── menu.py                      # Interactive console menu
│   ├── gui.py                       # tkinter GUI (Catppuccin Mocha)
│   ├── deps.py                      # Smart dependency management
│   ├── registry.py                  # Registry helpers & session
│   ├── corpguard.py                 # Corporate network detection
│   └── tweaks/                      # Plugin-based tweak registry
│       ├── __init__.py              # TweakDef dataclass + loader
│       ├── boot.py                  # Boot tweaks
│       ├── chrome.py                # Google Chrome policies
│       ├── copilot.py               # Windows Copilot / Recall
│       ├── edge.py                  # Microsoft Edge policies
│       ├── explorer.py              # Explorer tweaks
│       ├── firefox.py               # Mozilla Firefox policies
│       ├── gaming.py                # Gaming tweaks
│       ├── input.py                 # Input tweaks
│       ├── maintenance.py           # Maintenance tweaks
│       ├── office.py                # Microsoft Office policies
│       ├── performance.py           # Performance tweaks
│       ├── pkgmgmt.py              # Package management tweaks
│       ├── privacy.py               # Privacy tweaks
│       ├── shell.py                 # Shell context menu
│       ├── system.py                # System tweaks
│       ├── vscode.py                # VS Code policies
│       ├── win11.py                 # Windows 11 UI tweaks
│       └── wsl.py                   # WSL tweaks
├── tests/                           # Pester + pytest test suites
├── .github/                         # CI & contributor docs
├── .vscode/                         # VSCode workspace config
├── pyproject.toml                   # Python build config (hatchling)
└── README.md
```

## License

MIT — see [LICENSE](LICENSE) for details.
