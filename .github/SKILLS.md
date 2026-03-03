# TurboTweak — Reusable Skills & Patterns

## Skill 1: Registry Read/Write via winreg

```python
from turbotweak.registry import SESSION

# Write a DWORD
SESSION.set_dword(r"HKLM\SOFTWARE\...", "ValueName", 1)

# Write a string
SESSION.set_string(r"HKCU\SOFTWARE\...", "ValueName", "data")

# Read a DWORD (returns int | None)
val = SESSION.read_dword(r"HKLM\...", "ValueName")

# Delete a value
SESSION.delete_value(r"HKLM\...", "ValueName")

# Delete an entire key tree
SESSION.delete_tree(r"HKLM\SOFTWARE\...\SomeKey")

# Check key existence
exists = SESSION.key_exists(r"HKLM\SOFTWARE\...\SomeKey")
```

## Skill 2: Backup Before Mutation

Always back up keys **before** making changes:

```python
SESSION.backup([r"HKLM\...", r"HKCU\..."], "LabelForBackup")
```

Backups are stored as `.reg` exports in `OneDrive/RegistryBackups/`
or `~/Documents/RegistryBackups/`.

## Skill 3: Admin Elevation Check

```python
from turbotweak.registry import assert_admin

def my_tweak(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)  # raises AdminRequirementError if not elevated
    ...
```

In PowerShell:
```powershell
. "$PSScriptRoot\Lib-TurboTweak.ps1"
if (Assert-Elevated -Required) { return }
```

## Skill 4: Corporate Network Detection

```python
from turbotweak.corpguard import is_corporate_network, assert_not_corporate

# Check passively
if is_corporate_network():
    print("Corp detected")

# Block with exception
assert_not_corporate(force=False)  # raises CorporateNetworkError
```

## Skill 5: Tweak Status Detection (detect_fn)

Every tweak must have a detection function that returns `True` when
the tweak is currently active:

```python
def detect_disable_telemetry() -> bool:
    val = SESSION.read_dword(
        r"HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection",
        "AllowTelemetry"
    )
    return val == 0
```

Used by the GUI to show live status badges and by the state system for
undo snapshots.

## Skill 6: Plugin Registration

```python
# In turbotweak/tweaks/privacy.py
from turbotweak.tweaks import TweakDef

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="disable-telemetry",
        label="Disable Telemetry",
        category="Privacy",
        apply_fn=apply_disable_telemetry,
        remove_fn=remove_disable_telemetry,
        detect_fn=detect_disable_telemetry,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_TELEMETRY_POLICY, _TELEMETRY_DATA],
    ),
]
```

The plugin loader in `turbotweak/tweaks/__init__.py` collects all
`TWEAKS` lists from every module in the package.

## Skill 7: State Persistence & Undo

```python
from turbotweak.state import TweakState

state = TweakState()
state.save_snapshot("disable-telemetry", before=True, values={...})
state.mark_applied("disable-telemetry")
state.undo_last()  # reverts last action
```

State file: `~/.turbotweak/state.json`

## Skill 8: Running External Commands

```python
import subprocess

# reg.exe
subprocess.run(["reg", "add", key, "/v", name, "/d", val, "/f"],
               check=True, capture_output=True, text=True)

# fsutil
subprocess.run(["fsutil", "behavior", "set", "disablelastaccess", "1"],
               check=True, capture_output=True, text=True)

# PowerShell one-liner
subprocess.run(["powershell", "-NoProfile", "-Command", "..."],
               capture_output=True, text=True, timeout=10)
```

## Skill 9: GUI Threaded Execution

Never run registry operations on the tkinter main thread:

```python
import threading

def _dispatch(self, mode):
    threading.Thread(target=self._run, args=(mode,), daemon=True).start()

def _run(self, mode):
    # ... do work ...
    self._root.after(0, self._update_ui, result)  # schedule UI update
```

## Skill 10: Corp-Safe Classification

| corp_safe | Meaning | Example |
|---|---|---|
| `True` | HKCU-only, no GPO override risk | Mouse acceleration, Recent Folders |
| `False` | Touches HKLM/GPO, could conflict with corp policy | Telemetry, Cortana, Game DVR |

Rule: if the tweak writes to `HKLM\SOFTWARE\Policies\...` or similar
GPO paths → `corp_safe = False`.
