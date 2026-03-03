# ⚡ TurboTweak

A best-in-class Windows registry tweak toolkit with **PowerShell scripts**, a **Python CLI/menu**, and a **tkinter GUI**. Designed for power users who want fine-grained control over Windows 11 performance, privacy, and usability.

## Highlights

- **12 tweak categories** — each with an apply + remove script pair
- **3 interfaces** — interactive PS menu, Python console menu, and tkinter GUI
- **Corporate network safety** — auto-detects domain-joined, Azure AD, VPN, and managed machines; blocks tweaks to prevent accidental corporate policy violations
- **Automatic backups** — every registry mutation is backed up before changes
- **Fully reversible** — dedicated remove/restore for every tweak
- **Shared libraries** — `Lib-TurboTweak.ps1`, `Lib-BackupRegistry.ps1`, `Lib-CorpGuard.ps1` eliminate duplication

## Tweak Categories

| Category | Apply | Remove | Admin |
|---|---|---|---|
| Take Ownership context menu | `Add-TakeOwnership.ps1` | `Remove-TakeOwnership.ps1` | ✅ |
| Recent Folders in Quick Access | `Add-RecentFolders.ps1` | `Remove-RecentFolders.ps1` | — |
| Verbose Boot Messages | `Add-VerboseBoot.ps1` | `Remove-VerboseBoot.ps1` | ✅ |
| Performance Tweaks (visual/network) | `Add-Performance.ps1` | `Remove-Performance.ps1` | ✅ |
| Registry Auto-Backup Task | `Add-RegistryBackup.ps1` | `Remove-RegistryBackup.ps1` | ✅ |
| Disable Telemetry | `Add-DisableTelemetry.ps1` | `Remove-DisableTelemetry.ps1` | ✅ |
| Disable Cortana | `Add-DisableCortana.ps1` | `Remove-DisableCortana.ps1` | ✅ |
| Disable Mouse Acceleration | `Add-DisableMouseAccel.ps1` | `Remove-DisableMouseAccel.ps1` | — |
| Disable Game DVR / Game Bar | `Add-DisableGameDVR.ps1` | `Remove-DisableGameDVR.ps1` | ✅ |
| Optimize SvcHost Split (RAM) | `Add-SvcHostSplit.ps1` | `Remove-SvcHostSplit.ps1` | ✅ |
| Disable NTFS Last Access | `Add-DisableLastAccess.ps1` | `Remove-DisableLastAccess.ps1` | ✅ |
| Enable Long Paths (260-char bypass) | `Add-LongPaths.ps1` | `Remove-LongPaths.ps1` | ✅ |

## Requirements

- **Windows 11** (tested 22H2+)
- **PowerShell 5.1+** (PowerShell 7 / pwsh.exe preferred)
- **Python 3.10+** (for the Python CLI/menu/GUI)
- Administrator privileges for HKLM tweaks (scripts auto-elevate)

## Quick Start

### PowerShell Menu
```
Launch-TurboTweak.bat
```
Right-click → Run as administrator. The interactive menu offers all 24 individual options plus batch apply/remove and restore point creation.

### Python Console Menu
```bash
python -m turbotweak
```

### Python GUI
```bash
python -m turbotweak --gui
```
A dark-themed tkinter window with checkboxes for each tweak, progress bar, and category grouping.

### Single Tweak (CLI)
```bash
python -m turbotweak disable-telemetry -y
python -m turbotweak apply-all --assume-yes
python -m turbotweak --list
```

## Corporate Network Safety

TurboTweak automatically detects corporate environments and **blocks all tweaks** to prevent accidental policy violations or network bans:

- **Active Directory** domain membership (WMI / `Win32_ComputerSystem`)
- **Azure AD / Entra ID** join status (`dsregcmd /status`)
- **VPN adapters** — Cisco AnyConnect, GlobalProtect, Zscaler, WireGuard, etc.
- **Group Policy** indicators in the registry
- **SCCM / Intune** management agents

Override with `--force` (CLI) or the "Force" checkbox (GUI) at your own risk.

## Project Structure

```
TurboTweak/
├── Launch-TurboTweak.bat        # Launcher (pwsh → powershell fallback)
├── TurboTweakMenu.ps1           # Interactive PS menu
├── Lib-TurboTweak.ps1           # Shared PS utilities
├── Lib-BackupRegistry.ps1       # Registry backup helper
├── Lib-CorpGuard.ps1            # Corporate network detection (PS)
├── System_Restore_Point.ps1     # Create system restore point
├── Add-*.ps1 / Remove-*.ps1     # Individual tweak scripts (12 pairs)
├── turbotweak/                  # Python package
│   ├── __init__.py
│   ├── __main__.py
│   ├── cli.py                   # argparse CLI entry point
│   ├── menu.py                  # Interactive console menu
│   ├── gui.py                   # tkinter GUI
│   ├── tweaks.py                # Tweak implementations (Python)
│   ├── registry.py              # Registry helpers & session
│   └── corpguard.py             # Corporate network detection (Python)
├── tests/                       # Pester + pytest test suites
├── pyproject.toml               # Python build config (hatchling)
└── README.md
```

## Safety Measures

1. **Registry backups** — saved to OneDrive Documents or local Documents before every change
2. **Confirmation prompts** — every tweak asks for user approval (skip with `-y`)
3. **Logging** — all operations logged to `TurboTweak.log`
4. **System Restore Point** — create one before batch operations
5. **Admin elevation** — scripts auto-elevate when needed; HKCU tweaks run without admin
6. **Corporate guard** — blocks tweaks on domain/managed machines

## Contributing

1. Fork the repository
2. Add new tweak pair (Add-/Remove- scripts + Python function pair)
3. Register in `TurboTweakMenu.ps1`, `turbotweak/tweaks.py`, `turbotweak/cli.py`, `turbotweak/menu.py`, and `turbotweak/gui.py`
4. Include backup calls, error handling, logging, and confirmation prompts
5. Submit a PR

## License

MIT License — see [LICENSE](LICENSE) for details.
