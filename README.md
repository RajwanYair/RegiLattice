# ⚡ RegiLattice

A comprehensive Windows registry tweak toolkit with **113 tweaks** across **27 categories**, a **plugin architecture**, a **Python CLI**, **interactive console menu**, and a **tkinter GUI**. Designed for power users who want fine-grained control over Windows 11 performance, privacy, usability, and application behaviour.

## Highlights

- **113 tweaks** across 27 categories — each fully reversible with apply + remove
- **Plugin architecture** — auto-discovers tweaks from `regilattice/tweaks/`, easy to extend
- **3 interfaces** — interactive console menu, CLI with flags, and tkinter GUI
- **Category grouping & search** — tweaks sorted by category in GUI/menu, live search bar
- **Concurrent batch operations** — `ThreadPoolExecutor`-powered parallel apply/remove
- **UAC elevation** — automatic admin re-launch via `ctypes.ShellExecuteW`
- **Smart dependency management** — auto-installs missing pip packages with cascading fallbacks
- **Corporate network safety** — blocks tweaks on domain-joined, Azure AD, VPN, and managed machines
- **Automatic backups** — every registry mutation is backed up before changes
- **Snapshot/Undo** — save and restore tweak state snapshots (JSON)
- **Tags & descriptions** — every tweak has searchable tags and a human-readable description

## Tweak Categories

| Category | # | Tweaks |
|---|---|---|
| **Adobe** | 4 | Update, Telemetry, JavaScript, Welcome Screen |
| **AI / Copilot** | 2 | Disable Copilot, Disable Recall |
| **Boot** | 1 | Verbose Boot Messages |
| **Chrome** | 4 | Background Apps, Telemetry, Auto-Update, HW Accel |
| **Communication** | 7 | Teams (autostart, GPU), Zoom update, Discord (autostart, hwaccel), Spotify (autostart, hwaccel) |
| **Developer Tools** | 3 | Git Credential Manager, Long Paths, Default Branch |
| **Edge** | 4 | Startup Boost, Sidebar, Telemetry, Auto-Update |
| **Explorer** | 12 | File Extensions, Hidden Files, Super Hidden, This PC, Thumbnails, Title Bar Path, Recent Files, Recent Places, Search History, Gallery, Compact View, Auto Folder Type |
| **Firefox** | 3 | Telemetry, Pocket, Auto-Update |
| **Gaming** | 1 | Disable Game DVR / Game Bar |
| **Input** | 1 | Disable Mouse Acceleration |
| **Java** | 2 | Auto-Update, Web Plugin |
| **LibreOffice** | 4 | Auto-Update, Crash Reporter, Default OOXML, Default Handler |
| **Maintenance** | 1 | Registry Auto-Backup |
| **Network** | 6 | IRP Stack, Nagle Algorithm, Throttle, RDP, DNS-over-HTTPS, Max TCP |
| **Office** | 6 | Telemetry, Start Screen, Connected, HW Accel, Macro Trust, AutoSave — multi-version (2010–365) |
| **OneDrive** | 4 | Autostart, Files On-Demand, Ads, Upload Throttle |
| **Package Management** | 5 | PS Policy, PSGallery, Scoop, Winget, Pip |
| **Performance** | 3 | Startup Delay, SvcHost Split, NTFS Last Access |
| **Power** | 6 | USB Suspend, Hibernation, Prefetch, CPU Scheduling, Fast Startup, System Cache |
| **Privacy** | 8 | Telemetry, Cortana, Activity History, Location, Advertising ID, Camera, Microphone, DiagTrack |
| **RealVNC** | 6 | Encryption, Auth, Idle Timeout, Tray Icon, Viewer Recent, Viewer Scaling |
| **Shell** | 1 | Take Ownership Context Menu |
| **System** | 1 | Long Paths |
| **VS Code** | 3 | Telemetry, Auto-Update, Extension Update |
| **Windows 11** | 9 | Widgets, Snap, Context Menu, Lock Screen, WU, Bing, Bloatware, Dark Mode, Notifications |
| **WSL** | 6 | Default V2, Auto-Start, Nested Virt, Feature, VM Platform, Mirrored Network |

## Requirements

- **Windows 10/11** (tested on 22H2+)
- **Python 3.10+** (3.14 recommended)
- Administrator privileges for HKLM tweaks (auto-elevates via UAC prompt)

## Quick Start

### GUI (Recommended)
```bash
python -m regilattice --gui
```
Dark-themed tkinter window (Catppuccin Mocha) with per-category grouping, live search bar, checkboxes with ON/OFF status, and batch operations.

### Console Menu
```bash
python -m regilattice
```

### CLI
```bash
python -m regilattice apply disable-telemetry -y
python -m regilattice remove all --assume-yes
python -m regilattice --list
python -m regilattice --snapshot state.json
python -m regilattice --restore state.json
```

### Windows Launcher
```
Launch-RegiLattice.bat          # auto-detects Python, passes CLI args
Launch-RegiLattice.bat --gui    # launch GUI directly
```

## Smart Dependency Management

```python
from regilattice.deps import lazy_import, require

requests = lazy_import("requests")     # auto-installs if missing
require("rich", "requests")            # ensure all listed packages exist
```

Fallback order: user-space pip → system-wide pip → ensurepip bootstrap → graceful sentinel.

## Corporate Network Safety

Automatically detects corporate environments and **blocks all tweaks** to prevent policy violations:

- **Active Directory** domain membership (WMI / ctypes)
- **Azure AD / Entra ID** join status (`dsregcmd /status`)
- **VPN adapters** — Cisco AnyConnect, GlobalProtect, Zscaler, WireGuard, etc.
- **SCCM / Intune** management agents

Override with `--force` (CLI) or the "Force" checkbox (GUI) at your own risk.

## Project Structure

```
RegiLattice/
├── Launch-RegiLattice.bat           # Python launcher (.bat)
├── pyproject.toml                   # Build config (hatchling)
├── README.md
├── LICENSE
├── regilattice/                     # Python package
│   ├── __init__.py
│   ├── __main__.py                  # python -m regilattice
│   ├── cli.py                       # argparse CLI entry point
│   ├── menu.py                      # Interactive console menu
│   ├── gui.py                       # tkinter GUI (Catppuccin Mocha)
│   ├── deps.py                      # Smart dependency management
│   ├── elevation.py                 # UAC elevation helpers
│   ├── registry.py                  # Registry helpers & session
│   ├── corpguard.py                 # Corporate network detection
│   └── tweaks/                      # Plugin-based tweak registry (27 modules)
│       ├── __init__.py              # TweakDef dataclass + loader
│       ├── adobe.py                 # Adobe Reader / Acrobat
│       ├── boot.py                  # Boot tweaks
│       ├── chrome.py                # Google Chrome policies
│       ├── communication.py         # Teams, Zoom, Discord, Spotify
│       ├── copilot.py               # Windows Copilot / Recall
│       ├── edge.py                  # Microsoft Edge policies
│       ├── explorer.py              # Windows Explorer (12 tweaks)
│       ├── firefox.py               # Mozilla Firefox policies
│       ├── gaming.py                # Gaming tweaks
│       ├── gitconfig.py             # Git for Windows
│       ├── input.py                 # Input tweaks
│       ├── java.py                  # Java runtime
│       ├── libreoffice.py           # LibreOffice / OpenOffice
│       ├── maintenance.py           # Registry auto-backup
│       ├── network.py               # Network / connectivity
│       ├── office.py                # Microsoft Office (multi-version)
│       ├── onedrive.py              # OneDrive policies
│       ├── performance.py           # System performance
│       ├── pkgmgmt.py              # Package managers
│       ├── power.py                 # Power management
│       ├── privacy.py               # Windows privacy (8 tweaks)
│       ├── realvnc.py               # RealVNC Server & Viewer
│       ├── shell.py                 # Shell context menu
│       ├── system.py                # System capabilities
│       ├── vscode.py                # VS Code policies
│       ├── win11.py                 # Windows 11 UI debloating
│       └── wsl.py                   # WSL optimisation
├── tests/                           # pytest test suites
├── legacy/                          # Archived PowerShell scripts
│   ├── README.md                    # Legacy docs
│   └── powershell/                  # 29 original PS1 scripts
├── .github/                         # CI & contributor docs
└── .vscode/                         # VSCode workspace config
```

## Adding a Custom Tweak

Create a new `.py` file in `regilattice/tweaks/` exporting a `TWEAKS` list:

```python
from regilattice.registry import SESSION
from regilattice.tweaks import TweakDef

def _apply():
    SESSION.set_dword(r"HKCU\Software\MyApp", "FancyMode", 1)

def _remove():
    SESSION.delete_value(r"HKCU\Software\MyApp", "FancyMode")

def _detect() -> bool:
    return SESSION.read_dword(r"HKCU\Software\MyApp", "FancyMode") == 1

TWEAKS = [
    TweakDef(
        id="myapp-fancy-mode",
        label="Enable Fancy Mode",
        category="My App",
        apply_fn=_apply,
        remove_fn=_remove,
        detect_fn=_detect,
        needs_admin=False,
        description="Enables Fancy Mode in MyApp.",
        tags=["myapp", "fancy", "ui"],
    ),
]
```

The plugin loader discovers it automatically — no registration code needed.

## License

MIT — see [LICENSE](LICENSE) for details.
