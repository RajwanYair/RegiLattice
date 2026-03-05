# RegiLattice -- Code Patterns

For API reference, architecture, and project conventions see
[copilot-instructions.md](copilot-instructions.md).

---

## 1 -- Scoop package helpers

Factory for standardised install/remove/detect scoop-package tweaks.

```python
from regilattice.tweaks.scoop_tools import list_installed_scoop_apps, search_scoop_apps

# List currently installed scoop apps
installed = list_installed_scoop_apps()  # -> list[str] sorted app names

# Search for available scoop apps
results = search_scoop_apps("git")       # -> list[str] matching app names

# The module also exposes a factory:
# _make_scoop_tweak(cmd, pkg, label, desc, tags) -> TweakDef
```

## 2 -- GUI tooltip building

Rich tooltip with description, status, and metadata hints.

```python
# gui.py
def _build_tooltip_text(td: TweakDef, status: str) -> str:
    """Build rich tooltip including:
    - Main description text
    - Separator line
    - Current status (APPLIED / DEFAULT / UNKNOWN)
    - Default behaviour hint (if present)
    - Recommendation hint (if present)
    - Admin / corp-safe indicators
    - Tags list
    - Registry key paths
    """
```

Tooltips follow the mouse via the `_Tooltip` class and update on `refresh_status()`.

## 3 -- Snapshot save / restore

Persist and restore full tweak state as JSON.

```python
from pathlib import Path
from regilattice.tweaks import save_snapshot, load_snapshot, restore_snapshot

save_snapshot(Path("state.json"))

data = load_snapshot(Path("state.json"))  # -> dict[str, str]

results = restore_snapshot(Path("state.json"), force_corp=False, require_admin=True)
# results: {tweak_id: "applied" | "removed" | "unchanged" | "skipped (corp)" | "error"}
```

## 4 -- Description metadata parsing

Extracts `Default:` and `Recommended:` hints from tweak descriptions for GUI tooltips.

```python
# gui.py
@functools.lru_cache(maxsize=1024)
def _parse_description_metadata(description: str) -> tuple[str, str, str]:
    """Returns (main_text, default_hint, recommendation_hint)."""
    main_parts: list[str] = []
    default_hint = ""
    rec_hint = ""
    for sentence in description.replace(". ", ".\n").splitlines():
        s = sentence.strip()
        low = s.lower()
        if low.startswith("default:"):
            default_hint = s
        elif low.startswith("recommended:"):
            rec_hint = s
        else:
            main_parts.append(s)
    return " ".join(main_parts).strip(), default_hint, rec_hint

def _has_recommendation(td: TweakDef) -> bool:
    return "recommended:" in td.description.lower()
```

**Convention:** Include `Default: <value>.` and optionally `Recommended: <advice>.`
as separate sentences in descriptions. The GUI shows a teal "REC" badge for tweaks
with recommendations.

## 5 -- Parallel status detection

Thread-pool detection for fast GUI refresh.

```python
from concurrent.futures import ThreadPoolExecutor, as_completed
from regilattice.tweaks import status_map

# Single-call API
statuses = status_map(parallel=True, max_workers=8)
# -> {tweak_id: "applied" | "not applied" | "unknown"}

# GUI refresh pattern (gui.py)
def _refresh_status_all(self) -> None:
    statuses = status_map(parallel=True, max_workers=8)
    for row in self._tweak_rows:
        st = statuses.get(row.td.id, "unknown")
        # update row UI from cached status ...
```

## 6 -- Export PS1 pattern

Generates a PowerShell script from selected tweaks.

```python
# gui.py
def _export_powershell(self) -> None:
    selected = self._selected_tweaks()
    lines = [
        "# RegiLattice -- Exported Tweaks",
        f"# Generated from RegiLattice v{__version__}",
        f"# Tweaks: {len(selected)}",
        '#Requires -RunAsAdministrator',
        "",
    ]
    for td in selected:
        lines.append(f"# -- {td.label} ({'admin' if td.needs_admin else 'user'}) --")
        if td.description:
            lines.append(f"# {td.description}")
        for key in td.registry_keys:
            ps_key = key.replace("HKEY_LOCAL_MACHINE", "HKLM:")
            ps_key = ps_key.replace("HKEY_CURRENT_USER", "HKCU:")
            lines.append(f"# Registry key: {ps_key}")
        lines.append(f"Write-Host 'Applying: {td.label}...'")
        lines.append("")
    # Write with BOM for PowerShell compatibility
    with open(path, "w", encoding="utf-8-sig") as f:
        f.write("\n".join(lines))
```

## 7 -- Scope badge pattern

Classifies tweaks as USER / MACHINE / BOTH from their registry key paths.

```python
# tweaks/__init__.py
from regilattice.tweaks import tweak_scope

scope = tweak_scope(td)  # -> "user" | "machine" | "both"

# GUI usage (gui.py) -- colour-coded scope badges per row
_scope_cfg = {
    "user":    ("USER",    _OK_GREEN),      # green
    "machine": ("MACHINE", _ACCENT),        # blue
    "both":    ("BOTH",    _WARN_YELLOW),   # yellow
}
_s_text, _s_color = _scope_cfg.get(scope, ("?", _FG_DIM))
tk.Label(frame, text=_s_text, fg=_s_color, bg=_CARD_BG,
         font=("Segoe UI", 7, "bold")).pack(side="right", padx=(0, 4))
```

Logic: checks `HKCU`/`HKLM` prefixes in `td.registry_keys`; falls back to
`needs_admin` when no keys are listed.
