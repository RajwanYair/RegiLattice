# ⚡ RegiLattice

A comprehensive Windows registry tweak toolkit with **427 tweaks** across **40 categories**, a **plugin architecture**, a **Python CLI**, **interactive console menu**, and a **tkinter GUI**. Designed for power users who want fine-grained control over Windows 11 performance, privacy, usability, and application behaviour.

## Highlights

- **427 tweaks** across 40 categories — each fully reversible with apply + remove
- **Plugin architecture** — auto-discovers tweaks from `regilattice/tweaks/`, easy to extend
- **3 interfaces** — interactive console menu, CLI with flags, and tkinter GUI
- **GUI per-row toggle buttons** — each tweak row shows ENABLED / DISABLED status via `detect_fn`
- **Category grouping & search** — tweaks sorted by category in GUI/menu, live search bar
- **Per-category batch actions** — Enable All / Disable All buttons on each category header
- **Keyboard shortcuts** — Ctrl+F search, Ctrl+A select all, Ctrl+E expand, Esc clear
- **Concurrent batch operations** — `ThreadPoolExecutor`-powered parallel apply/remove
- **UAC elevation** — automatic admin re-launch via `ctypes.ShellExecuteW`
- **Smart dependency management** — auto-installs missing pip packages with cascading fallbacks
- **Corporate network safety** — blocks tweaks on domain-joined, Azure AD, VPN, and managed machines
- **Automatic backups** — every registry mutation is backed up before changes
- **Snapshot/Undo** — save and restore tweak state snapshots (JSON)
- **Tags & descriptions** — every tweak has searchable tags and a human-readable description
- **4 machine profiles** — business, gaming, privacy, minimal
- **Newbie-friendly template** — `_template.py` with step-by-step instructions for adding tweaks
- **4 400+ tests** across 8 test files — full coverage of registry helpers, CLI, GUI, and all tweak modules

## Tweak Categories

| Category | # | Tweaks |
|---|---|---|
| **Accessibility** | 12 | Sticky/Toggle/Filter/Narrator/Magnifier Keys, Dark Mode, Animations, ClearType, Scroll Bar, High Contrast, OSK, Sound Sentry |
| **Adobe** | 11 | Auto-Update, Telemetry, JavaScript, Welcome Screen, Protected Mode, Cloud Services, CC Files, Genuine Check, Crash Reporter, Home Screen, Font Sync |
| **AI / Copilot** | 11 | Disable Copilot, Recall, AI Tips, Copilot Edge, Copilot Button, Bing Chat, Recall Policy, Taskbar, Start Suggestions, AI Widgets |
| **Audio** | 10 | System Sounds, Startup Sound, Ducking, Enhancements, Spatial Audio, Notifications, Exclusive Mode, Low Battery Sound, Speech Recognition |
| **Bluetooth** | 10 | Power Management, Service, A2DP Audio/Sink, Discoverability, LE Low-Latency, Handsfree, OBEX, PAN, Serial |
| **Boot** | 10 | Verbose Boot, Splash, Timeout, Num Lock, Boot Log, WinRE, Auto-Repair, Ignore Failures, Secure Boot Check |
| **Chrome** | 11 | Background Apps, Telemetry, Auto-Update, HW Accel, Sign-In, Sync, Secure DNS, Spell Check, Third-Party Cookies, Autofill Passwords |
| **Cloud Storage** | 12 | Dropbox (autostart, update, LAN sync), Google Drive (autostart, update, bandwidth), iCloud (autostart, photos), Box, MEGA, pCloud, Overlay Optimise |
| **Communication** | 9 | Teams (autostart, GPU), Zoom (update, auto-video), Discord (autostart, hwaccel), Spotify (autostart, hwaccel), Slack |
| **Cortana & Search** | 11 | Lock Screen Cortana, Web Search, Search Highlights, Taskbar Search, Disable Cortana, Cloud Search, Search Indexing, Find My Files, Search Location |
| **Developer Tools** | 10 | Git Credential Manager, Long Paths, Default Branch, autocrlf, Default Editor, fsmonitor, Parallel Checkout, GC Auto, Delta Cache |
| **Display** | 11 | DPI Scaling, ClearType, Force 96 DPI, Dark Mode (Apps/System), Transparency, Animations, Wallpaper Compression, Accent Title Bars, Edge Swipe, Adaptive Brightness |
| **Edge** | 11 | Startup Boost, Sidebar, Shopping, Telemetry, Auto-Update, First-Run, Password Manager, Sync, Rewards, Third-Party Cookies |
| **Explorer** | 17 | File Extensions, Hidden Files, Super Hidden, This PC, Thumbnails, Title Bar Path, Recent Files/Places, Search History, Gallery, Compact View, Auto Folder Type, Breadcrumb Bar, Folder Merge, Thumbnail Performance, Status Bar, PowerShell Here |
| **Firefox** | 11 | Telemetry, Studies, Pocket, Auto-Update, Crash Reporter, Default Browser, Feedback, Captive Portal, DNS over HTTPS, Extension Recommendations, Password Reveal |
| **Fonts** | 12 | ClearType, Font Smoothing, Antialiasing, Segoe UI Default, Edge Font Download, Untrusted Fonts, FontCache Service, FontCache3, ClearType Tuning, Natural Contrast, WPF HW Text, IE Zone Download |
| **GPU / Graphics** | 12 | HW GPU Scheduling, VRR, GPU Power Preference, Shader Cache, WDDM Priority, GPU Preemption, Game-Mode GPU, Fullscreen Optimisations, Game Bar Overlay, NVIDIA TDR, MPO Disable |
| **Gaming** | 11 | Game DVR, Game Bar, Game Mode, FSO, Xbox Monitoring, Game Bar Tips, Input Redirect, GPU Scheduling, Network Throttling, Nagle's Algorithm |
| **Input** | 11 | Mouse Acceleration, Keyboard Repeat, Sticky Keys, Caps Lock to Ctrl, Auto-Correct, Filter Keys, Toggle Keys, Enhanced Pointer, Mouse Scroll, Keyboard Delay, Touch Keyboard |
| **Java** | 9 | Auto-Update, Web Plugin, Usage Tracking, High DPI, Sponsor Offers, Security High, Error Reporting, Tip of Day, Certificate Revoke |
| **LibreOffice** | 11 | Auto-Update, Crash Reporter, Default OOXML, Default Handler, Recovery, Start Center, Online Update, News, Macros, Feedback |
| **Maintenance** | 11 | Registry Auto-Backup, Defrag, Crash Dumps, Download Cleanup, Event Log, Maintenance Wakeup, Disk Diagnostics, Error Reporting, Superfetch, Compatibility Assistant, Storage Sense |
| **Network** | 8 | IRP Stack, Nagle Algorithm, Throttle, RDP, DNS-over-HTTPS, Max TCP, Wi-Fi Sense, NetBIOS |
| **Office** | 8 | Telemetry, Start Screen, Connected, HW Accel, Macro Trust, AutoRecover, LinkedIn, UI Animations — multi-version (2010–365) |
| **OneDrive** | 11 | Autostart, Files On-Demand, Ads, Upload/Download Throttle, Personal Sync, Known Folder Move, Office Collaboration, Silent Config |
| **Package Management** | 15 | PS Policy, PSGallery, Scoop, Winget, Pip --user, Pip Cache, npm Offline, Pip Require Venv, Pip Version Check, Pip Timeout, Pip Trusted Host, System Pip Index/Cache/Trust/Venv |
| **Performance** | 12 | Visual Effects, SvcHost Split, NTFS Last Access, Transparency, Background UWP, Animations, Menu Delay, Search Protocol, Large System Cache, Paging Executive, Processor Scheduling, NTFS Encryption |
| **Power** | 8 | USB Suspend, Hibernation, Prefetch, CPU Scheduling, Fast Startup, System Cache, Power Throttling, NTFS Timestamp |
| **Privacy** | 10 | Telemetry, Cortana, Activity History, Location, Advertising ID, Camera, Microphone, DiagTrack, Speech Recognition, Inking Personalization |
| **RealVNC** | 8 | Encryption, Auth, Idle Timeout, Tray Icon, Viewer Recent, Viewer Scaling, Blank Screen, Clipboard Sharing |
| **Scoop Tools** | 16 | Aria2, Dark, Git, 7zip, Notepad++, VS Code, Python, Node, Grep, Gsudo, Sysinternals, WinMerge, WinDirStat, Curl, Wget, Fzf |
| **Security** | 9 | Sample Submission, PUA Protection, SmartScreen, Exploit Telemetry, Scan CPU Limit, Notifications, Dev Folder Exclusions, Controlled Folder Access, Network Protection |
| **Services** | 8 | DiagTrack, Search Indexer, Error Reporting, Print Spooler, SysMain, Diagnostic Service, Biometric, Fax |
| **Shell** | 11 | Take Ownership, Open CMD Here, Get File Hash, PowerShell Here, Copy Path, Classic Context Menu, Recent Files, Frequent Folders, Compact Explorer, Show Extensions, Show Hidden |
| **Startup** | 10 | Startup Delay, Skype, Edge Startup, Store Suggested, Teams, Cortana, Startup Sound, Login Background, Lock Screen, First Logon Animation |
| **System** | 10 | Long Paths, Reserved Storage, Remote Assistance, UAC Dimming, Verbose Boot Status, AutoPlay, Storage Sense, Activity History, Clipboard History, Admin Shares |
| **VS Code** | 12 | Telemetry, Auto-Update, Extension Update, A/B Experiments, Settings Sync, Startup Editor, Recommendations, Telemetry Policy, Crash Reporter, Extension Gallery |
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

### Machine Profiles
```bash
python -m regilattice --profile business   # productivity, security & cloud tweaks
python -m regilattice --profile gaming     # GPU, performance & network tweaks
python -m regilattice --profile privacy    # maximum privacy — disables telemetry/tracking
python -m regilattice --profile minimal    # lightweight essentials (performance, boot, startup)
```

Profiles apply an opinionated set of tweaks based on the machine's purpose. Use `--profile business` on work machines, `--profile gaming` on gaming rigs, `--profile privacy` for max telemetry blocking, or `--profile minimal` for a light cleanup.

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
│   └── tweaks/                      # Plugin-based tweak registry (40 modules)
│       ├── __init__.py              # TweakDef dataclass + loader + profiles
│       ├── _template.py             # Newbie guide — copy to add a new tweak
│       ├── accessibility.py         # Accessibility & visual aids (12)
│       ├── adobe.py                 # Adobe Reader / Acrobat (11)
│       ├── audio.py                 # Audio & sound settings (10)
│       ├── bluetooth.py             # Bluetooth power & audio (10)
│       ├── boot.py                  # Boot tweaks (10)
│       ├── chrome.py                # Google Chrome policies (11)
│       ├── cloudstorage.py          # Cloud Storage — Dropbox, GDrive, iCloud (12)
│       ├── communication.py         # Teams, Zoom, Discord, Spotify, Slack (9)
│       ├── copilot.py               # Windows Copilot / Recall (11)
│       ├── cortana.py               # Cortana & Search (11)
│       ├── defender.py              # Windows Security / Defender
│       ├── display.py               # Display, DPI, themes (11)
│       ├── edge.py                  # Microsoft Edge policies (11)
│       ├── explorer.py              # Windows Explorer (17)
│       ├── firefox.py               # Mozilla Firefox policies (11)
│       ├── fonts.py                 # Font rendering & ClearType (12)
│       ├── gaming.py                # Gaming tweaks (11)
│       ├── gitconfig.py             # Git for Windows (10)
│       ├── gpu.py                   # GPU / Graphics optimisation (12)
│       ├── input.py                 # Input tweaks (11)
│       ├── java.py                  # Java runtime (9)
│       ├── libreoffice.py           # LibreOffice / OpenOffice (11)
│       ├── maintenance.py           # Registry auto-backup & cleanup (11)
│       ├── network.py               # Network / connectivity (8)
│       ├── office.py                # Microsoft Office multi-version (8)
│       ├── onedrive.py              # OneDrive policies (11)
│       ├── performance.py           # System performance (12)
│       ├── pkgmgmt.py              # Package managers (pip, npm, scoop, winget) (15)
│       ├── power.py                 # Power management (8)
│       ├── privacy.py               # Windows privacy (10)
│       ├── realvnc.py               # RealVNC Server & Viewer (8)
│       ├── scoop_tools.py           # Scoop package installer (16)
│       ├── services.py              # Windows services (8)
│       ├── shell.py                 # Shell context menu (11)
│       ├── startup.py               # Startup programs (10)
│       ├── system.py                # System capabilities (10)
│       ├── vscode.py                # VS Code policies (12)
│       ├── win11.py                 # Windows 11 UI debloating (11)
│       ├── windowsupdate.py         # Windows Update policies (8)
│       └── wsl.py                   # WSL optimisation (8)
├── tests/                           # pytest test suites
├── legacy/                          # Archived PowerShell scripts
│   ├── README.md                    # Legacy docs
│   └── powershell/                  # 29 original PS1 scripts
├── .github/                         # CI & contributor docs
└── .vscode/                         # VSCode workspace config
```

## Adding a Custom Tweak

**Easiest way:** copy `regilattice/tweaks/_template.py` to a new file (e.g.
`regilattice/tweaks/mytweaks.py`) and follow the numbered steps inside.
The template includes three working example tweaks with detailed comments.

Also see **[.github/workflow.md](.github/workflow.md)** for the full plugin
guide and **[.github/skills.md](.github/skills.md)** for reusable patterns.

**Quick version:** create a `.py` file in `regilattice/tweaks/` exporting a
`TWEAKS` list:

```python
from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

def _apply(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("MyApp: enable fancy mode")
    SESSION.backup([r"HKCU\Software\MyApp"], "FancyMode")
    SESSION.set_dword(r"HKCU\Software\MyApp", "FancyMode", 1)

def _remove(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
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
