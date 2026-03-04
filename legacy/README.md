# Legacy PowerShell Scripts

These scripts are the **original PowerShell implementation** of RegiLattice
(formerly TurboTweak). They have been fully superseded by the Python package
`regilattice` and are retained here for historical reference only.

## Replacement Mapping

| PowerShell Script | Python Equivalent |
|---|---|
| `Lib-RegiLattice.ps1` | `regilattice/registry.py` + `regilattice/elevation.py` |
| `Lib-BackupRegistry.ps1` | `regilattice.registry.SESSION.backup()` |
| `Lib-CorpGuard.ps1` | `regilattice/corpguard.py` |
| `RegiLatticeMenu.ps1` | `regilattice/menu.py` |
| `Launch-RegiLattice.bat` | `python -m regilattice` or `Launch-RegiLattice.bat` (root) |
| `System_Restore_Point.ps1` | `regilattice.tweaks.maintenance.create_restore_point()` |
| `Add-*.ps1` / `Remove-*.ps1` | `regilattice/tweaks/` plugin modules |

## Using the Python Version

```bash
# Interactive menu
python -m regilattice

# GUI
python -m regilattice --gui

# CLI
python -m regilattice apply take-ownership
python -m regilattice --list
```
