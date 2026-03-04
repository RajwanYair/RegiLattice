# ⚡ RegiLattice

A comprehensive Windows registry tweak toolkit with **212 tweaks** across **34 categories**, a **plugin architecture**, a **Python CLI**, **interactive console menu**, and a **tkinter GUI**. Designed for power users who want fine-grained control over Windows 11 performance, privacy, usability, and application behaviour.

## Highlights

- **212 tweaks** across 34 categories — each fully reversible with apply + remove
- **Plugin architecture** — auto-discovers tweaks from `regilattice/tweaks/`, easy to extend
- **3 interfaces** — interactive console menu, CLI with flags, and tkinter GUI
- **GUI per-row toggle buttons** — each tweak row shows ENABLED / DISABLED status via `detect_fn`
- **Category grouping & search** — tweaks sorted by category in GUI/menu, live search bar
- **Concurrent batch operations** — `ThreadPoolExecutor`-powered parallel apply/remove
- **UAC elevation** — automatic admin re-launch via `ctypes.ShellExecuteW`
- **Smart dependency management** — auto-installs missing pip packages with cascading fallbacks
- **Corporate network safety** — blocks tweaks on domain-joined, Azure AD, VPN, and managed machines
- **Automatic backups** — every registry mutation is backed up before changes
- **Snapshot/Undo** — save and restore tweak state snapshots (JSON)
- **Tags & descriptions** — every tweak has searchable tags and a human-readable description
- **2 267 tests** across 8 test files — full coverage of registry helpers, CLI, GUI, and all tweak modules

## Tweak Categories

| Category | # | Tweaks |
|---|---|---|
| **Accessibility** | 7 | Sticky/Toggle/Filter Keys, Dark Mode, Animations, ClearType, Scroll Bar Width, Narrator, High Contrast |
| **Adobe** | 6 | Auto-Update, Telemetry, JavaScript, Welcome Screen, Protected Mode, Cloud Services |
| **AI / Copilot** | 4 | Disable Copilot, Disable Recall, AI Tips, Copilot in Edge |
| **Bluetooth** | 5 | Power Management, Service to Manual, A2DP Audio, Discoverability, LE Low-Latency |
| **Boot** | 3 | Verbose Boot, Splash Logo, Boot Menu Timeout |
| **Chrome** | 6 | Background Apps, Telemetry, Auto-Update, HW Accel, Sign-In & Sync, Secure DNS |
| **Communication** | 9 | Teams (autostart, GPU), Zoom (update, auto-video), Discord (autostart, hwaccel), Spotify (autostart, hwaccel), Slack |
| **Cortana & Search** | 6 | Lock Screen Cortana, Web Search, Search Highlights, Taskbar Search Box, Disable Cortana, Cloud Content Search |
| **Developer Tools** | 5 | Git Credential Manager, Long Paths, Default Branch, autocrlf, Default Editor |
| **Edge** | 6 | Startup Boost, Sidebar & Shopping, Telemetry, Auto-Update, First-Run, Password Manager |
| **Explorer** | 14 | File Extensions, Hidden Files, Super Hidden, This PC, Thumbnails, Title Bar Path, Recent Files, Recent Places, Search History, Gallery, Compact View, Auto Folder Type, Breadcrumb Bar, Folder Merge |
| **Firefox** | 5 | Telemetry & Studies, Pocket, Auto-Update, Crash Reporter, Default Browser Check |
| **Gaming** | 3 | Game DVR / Game Bar, Game Mode, Fullscreen Optimizations |
| **Input** | 3 | Mouse Acceleration, Keyboard Repeat Rate, Sticky Keys Prompt |
| **Java** | 4 | Auto-Update, Web Plugin, Usage Tracking, High DPI Scaling |
| **LibreOffice** | 6 | Auto-Update, Crash Reporter, Default OOXML, Default Handler, Recovery, Start Center |
| **Maintenance** | 3 | Registry Auto-Backup, Scheduled Defrag, Crash Memory Dumps |
| **Network** | 8 | IRP Stack, Nagle Algorithm, Throttle, RDP, DNS-over-HTTPS, Max TCP, Wi-Fi Sense, NetBIOS |
| **Office** | 8 | Telemetry, Start Screen, Connected, HW Accel, Macro Trust, AutoRecover, LinkedIn, UI Animations — multi-version (2010–365) |
| **OneDrive** | 6 | Autostart, Files On-Demand, Ads, Upload Throttle, Personal Sync, Known Folder Move |
| **Package Management** | 7 | PS Policy, PSGallery, Scoop, Winget, Pip --user, Pip Cache, npm Offline |
| **Performance** | 5 | Visual Effects, SvcHost Split, NTFS Last Access, Transparency Effects, Background UWP Apps |
| **Power** | 8 | USB Suspend, Hibernation, Prefetch, CPU Scheduling, Fast Startup, System Cache, Power Throttling, NTFS Timestamp |
| **Privacy** | 10 | Telemetry, Cortana, Activity History, Location, Advertising ID, Camera, Microphone, DiagTrack, Speech Recognition, Inking Personalization |
| **RealVNC** | 8 | Encryption, Auth, Idle Timeout, Tray Icon, Viewer Recent, Viewer Scaling, Blank Screen, Clipboard Sharing |
| **Security** | 7 | Sample Submission, PUA Protection, SmartScreen, Exploit Telemetry, Scan CPU Limit, Notifications, Dev Folder Exclusions |
| **Services** | 6 | DiagTrack, Search Indexer, Error Reporting, Print Spooler, SysMain, Diagnostic Service |
| **Shell** | 3 | Take Ownership, Open CMD Here, Get File Hash |
| **Startup** | 6 | Startup Delay, Skype, Edge Startup, Store Suggested Apps, Teams, Cortana |
| **System** | 3 | Long Paths, Reserved Storage, Remote Assistance |
| **VS Code** | 5 | Telemetry, Auto-Update, Extension Update, A/B Experiments, Settings Sync |
| **Windows 11** | 11 | Widgets, Snap Assist, Context Menu, Lock Screen, WU Auto-Restart, Bing Search, Bloatware, Dark Mode, Notifications, Snap Layout Flyout, Chat Icon |
| **Windows Update** | 8 | Delivery Optimization, Quality Deferral, Feature Deferral, Exclude Drivers, Auto-Restart, Notify-Only, WaaS Medic, Update Orchestrator |
| **WSL** | 8 | Default V2, Auto-Start, Nested Virt, Feature, VM Platform, Mirrored Network, Memory Reclaim, DNS Tunneling |

## Requirements

- **Windows 10/11** (tested on 22H2+)
- **Python 3.10+** (3.14 recommended)
- Administrator privileges for HKLM tweaks (auto-elevates via UAC prompt)

## Quick Start

### GUI (Recommended)
```bash
python -m regilattice --gui
```
Dark-themed tkinter window (Catppuccin Mocha) with per-category grouping, live search bar, per-row toggle buttons showing ENABLED / DISABLED status, and batch operations.

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
│   └── tweaks/                      # Plugin-based tweak registry (34 modules)
│       ├── __init__.py              # TweakDef dataclass + loader
│       ├── accessibility.py         # Accessibility & visual aids
│       ├── adobe.py                 # Adobe Reader / Acrobat
│       ├── bluetooth.py             # Bluetooth power & audio
│       ├── boot.py                  # Boot tweaks
│       ├── chrome.py                # Google Chrome policies
│       ├── communication.py         # Teams, Zoom, Discord, Spotify, Slack
│       ├── copilot.py               # Windows Copilot / Recall
│       ├── cortana.py               # Cortana & Search
│       ├── defender.py              # Windows Security / Defender
│       ├── edge.py                  # Microsoft Edge policies
│       ├── explorer.py              # Windows Explorer (14 tweaks)
│       ├── firefox.py               # Mozilla Firefox policies
│       ├── gaming.py                # Gaming tweaks
│       ├── gitconfig.py             # Git for Windows
│       ├── input.py                 # Input tweaks
│       ├── java.py                  # Java runtime
│       ├── libreoffice.py           # LibreOffice / OpenOffice
│       ├── maintenance.py           # Registry auto-backup & cleanup
│       ├── network.py               # Network / connectivity
│       ├── office.py                # Microsoft Office (multi-version)
│       ├── onedrive.py              # OneDrive policies
│       ├── performance.py           # System performance
│       ├── pkgmgmt.py              # Package managers
│       ├── power.py                 # Power management
│       ├── privacy.py               # Windows privacy (10 tweaks)
│       ├── realvnc.py               # RealVNC Server & Viewer
│       ├── services.py              # Windows services
│       ├── shell.py                 # Shell context menu
│       ├── startup.py               # Startup programs
│       ├── system.py                # System capabilities
│       ├── vscode.py                # VS Code policies
│       ├── win11.py                 # Windows 11 UI debloating
│       ├── windowsupdate.py         # Windows Update policies
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
