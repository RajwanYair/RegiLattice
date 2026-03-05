# GUI Components Reference — `regilattice/gui.py`

> Complete class, method, and constant reference with line numbers.
> Last verified: 2025-06-20 (v2.0.0, ~1 209 lines).

---

## Module-Level Constants

### Theme Colors (Catppuccin Mocha Palette)

| Constant | Value | Role | Line |
|----------|-------|------|------|
| `_ACCENT` | `#89B4FA` | Blue — buttons, links, selection highlights | L45 |
| `_BG` | `#1E1E2E` | Base — main window background | L46 |
| `_BG_SURFACE` | `#24273A` | Surface0 — scrollable area background | L47 |
| `_FG` | `#CDD6F4` | Text — default foreground | L48 |
| `_FG_DIM` | `#6C7086` | Overlay0 — dimmed/secondary text | L49 |
| `_CARD_BG` | `#313244` | Surface1 — tweak row card background | L50 |
| `_CARD_HOVER` | `#45475A` | Surface2 — tweak row hover state | L51 |
| `_OK_GREEN` | `#A6E3A1` | Green — applied/success status | L52 |
| `_WARN_YELLOW` | `#F9E2AF` | Yellow — unknown/warning status | L53 |
| `_ERR_RED` | `#F38BA8` | Red — error/corp-blocked status | L54 |
| `_PURPLE` | `#CBA6F7` | Mauve — decorative/accent | L55 |
| `_HEADER_BG` | `#181825` | Crust — category header background | L56 |
| `_BORDER` | `#45475A` | Surface2 — borders and separators | L57 |
| `_DIM_BG` | `#585B70` | Overlay2 — disabled/inactive backgrounds | L58 |
| `_TEAL` | `#94E2D5` | Teal — recommendation badge color | L59 |

### Status Colors

| Constant | Value | Meaning | Line |
|----------|-------|---------|------|
| `_STATUS_APPLIED` | `_OK_GREEN` | Tweak is currently active | L62 |
| `_STATUS_NOT_APPLIED` | `_FG_DIM` | Tweak has been reverted | L63 |
| `_STATUS_UNKNOWN` | `_WARN_YELLOW` | Detection failed or unavailable | L64 |
| `_STATUS_CORP_BLOCKED` | `_ERR_RED` | Blocked by corporate guard | L65 |
| `_STATUS_DEFAULT` | `#89DCEB` (Sky) | Not in registry (Windows default) | L66 |

### Font Definitions

| Constant | Value | Usage | Line |
|----------|-------|-------|------|
| `_FONT` | `("Segoe UI", 10)` | Standard body text | L69 |
| `_FONT_BOLD` | `("Segoe UI Semibold", 10)` | Tweak labels, emphasized text | L70 |
| `_FONT_SM` | `("Segoe UI", 9)` | Secondary labels, descriptions | L71 |
| `_FONT_XS` | `("Segoe UI", 8)` | Tiny annotations | L72 |
| `_FONT_XS_BOLD` | `("Segoe UI", 8, "bold")` | Status badges, tag labels | L73 |
| `_FONT_TITLE` | `("Segoe UI Semibold", 16)` | Window title | L74 |
| `_FONT_CAT` | `("Segoe UI Semibold", 11)` | Category headers | L75 |

---

## Helper Functions

### `_parse_description_metadata(description: str)` — L124

Extracts structured metadata from tweak description text.

- **Returns**: `tuple[str, str, str]` → `(clean_description, default_value, recommended_value)`
- **Parsing**: Looks for `Default:` and `Recommended:` lines in description text
- **Cached**: Decorated with `@functools.lru_cache(maxsize=1024)` for performance
- **Used by**: `_build_tooltip_text()`, `_has_recommendation()`

### `_has_recommendation(td: TweakDef)` — L144

Returns `True` if the tweak's description contains a `Recommended:` metadata line.

- **Returns**: `bool`
- **Used by**: GUI to show teal "REC" badge on tweak rows

### `_build_tooltip_text(td: TweakDef, status: str)` — L149

Builds rich multi-line tooltip text for tweak hover.

- **Returns**: `str` — formatted tooltip with sections:
  - Status (with color name)
  - Description (cleaned)
  - Default value (if present)
  - Recommended value (if present)
  - Tags list
  - Registry keys touched
  - Admin requirement
  - Corporate safety status

---

## Classes

### `_Tooltip` — L81

Floating tooltip widget that follows the mouse cursor.

| Method | Line | Description |
|--------|------|-------------|
| `__init__(widget, text)` | L84 | Binds Enter/Leave/Motion events to widget |
| `update_text(text)` | L92 | Dynamically update tooltip content |
| `_show(event)` | L95 | Creates toplevel window with label at cursor position |
| `_move(event)` | L110 | Repositions tooltip to follow cursor |
| `_hide(event)` | L114 | Destroys tooltip toplevel |

### `_TweakRow` — L199

Single tweak entry in the GUI — contains checkbox, label, status badge, tags, and tooltip.

| Method | Line | Description |
|--------|------|-------------|
| `__init__(parent, td, toggle_cb)` | L202 | Creates frame with checkbox, label, status dot, tags, REC badge, tooltip |
| `on_toggle_click()` | L273 | Callback when checkbox is toggled |
| `pack_row()` | L277 | Shows the row in its parent |
| `unpack_row()` | L281 | Hides the row (filtering) |
| `refresh_status()` | L285 | Re-runs `detect_fn()`, updates status dot color and tooltip |

**Key attributes:**
- `td: TweakDef` — the tweak definition
- `var: tk.BooleanVar` — checkbox state
- `frame: ttk.Frame` — container frame
- `status_dot: tk.Label` — colored circle showing current status
- `tooltip: _Tooltip` — hover tooltip instance

### `_CategorySection` — L323

Collapsible category group containing multiple `_TweakRow` instances.

| Method | Line | Description |
|--------|------|-------------|
| `__init__(parent, name, rows)` | L326 | Creates header with expand/collapse, count label, Enable All/Disable All buttons |
| `_rebuild_row_widgets(row)` | L380 | Recreates row widget within the section (used after filtering) |
| `toggle(event)` | L439 | Expand/collapse the section |
| `_show_rows()` | L448 | Pack all visible rows |
| `_hide_rows()` | L453 | Unpack all rows |
| `update_count()` | L458 | Updates "X/Y applied" count label |
| `set_on_batch(callback)` | L464 | Sets callback for Enable All / Disable All buttons |
| `filter_rows(query)` | L469 | Filters rows by search query, returns True if any visible |

**Key attributes:**
- `name: str` — category display name
- `rows: list[_TweakRow]` — tweak rows in this category
- `expanded: bool` — current expand/collapse state
- `header_frame: ttk.Frame` — clickable header
- `body_frame: ttk.Frame` — container for rows (shown/hidden)

### `RegiLatticeGUI` — L498

Main application window — the top-level orchestrator.

| Method | Line | Description |
|--------|------|-------------|
| `__init__()` | L501 | Creates root `Tk` window, applies theme, builds UI, starts detection |
| `_apply_win11_dark_titlebar()` | L526 | Uses `ctypes.windll` DWM API to force dark title bar |
| `_setup_styles()` | L542 | Configures ttk styles for all widget types |
| `_bind_shortcuts()` | L576 | Registers keyboard shortcuts (Ctrl+A/D/F/E/R, Esc) |
| `_focus_search()` | L586 | Ctrl+F handler — focuses search entry |
| `_clear_search()` | L590 | Esc handler — clears search or resets filter |
| `_build_ui()` | L596 | **Main UI construction** (~210 lines) — builds all panels, sections, rows |
| `_select_all()` | L806 | Checks all visible tweak checkboxes |
| `_deselect_all()` | L812 | Unchecks all visible tweak checkboxes |
| `_collapse_all()` | L817 | Collapses all category sections |
| `_expand_all()` | L822 | Expands all category sections |
| `_filter_rows()` | L827 | Applies search text + status filter to all sections |
| `_update_selection_count()` | L861 | Updates "X tweaks selected" label |
| `_set_status(text, color)` | L866 | Updates status bar text and color |
| `_selected_tweaks()` | L869 | Returns list of TweakDef for all checked rows |
| `_batch_category(section, mode)` | L874 | Threaded batch apply/remove for a category's Enable/Disable All |
| `_refresh_status_all()` | L891 | **Parallel status refresh** — uses `status_map(parallel=True)` with ThreadPoolExecutor |
| `_toggle_single(row)` | L946 | Apply or remove a single tweak (threaded) |
| `_apply_profile_selection(name)` | L989 | Selects tweaks matching a profile |
| `_export_powershell()` | L1008 | Generates PowerShell `.ps1` script for selected tweaks |
| `_save_snapshot()` | L1054 | Saves current tweak state as JSON snapshot |
| `_restore_snapshot()` | L1068 | Restores tweak state from JSON snapshot |
| `_dispatch(mode)` | L1102 | Dispatches batch apply/remove for selected tweaks |
| `_run_tweaks(items, mode)` | L1127 | Worker thread: runs apply/remove on list of tweaks with progress |
| `_run_restore_point()` | L1163 | Creates Windows System Restore Point before batch operations |
| `run()` | L1181 | Enters mainloop |

### `launch()` — L1186

Module-level function that creates `RegiLatticeGUI` and calls `run()`.

---

## UI Layout Structure

```
RegiLatticeGUI (Tk root)
├── Title Bar (dark via DWM API)
├── Top Bar Frame
│   ├── Title Label ("RegiLattice")
│   ├── Search Entry (Ctrl+F)
│   ├── Status Filter Dropdown (All / Applied / Default / Unknown)
│   ├── Profile Selector Dropdown
│   └── Summary Stats Labels (Applied / Default / Unknown / Recommended)
├── Button Bar Frame
│   ├── Apply Selected (green)
│   ├── Remove Selected (red)
│   ├── Select All / Deselect All
│   ├── Expand All / Collapse All
│   ├── Refresh Status
│   ├── Export PS1
│   ├── Save Snapshot / Restore Snapshot
│   └── Force Checkbox (override corp guard)
├── Scrollable Canvas
│   └── Category Sections (63x)
│       ├── _CategorySection header
│       │   ├── Expand/Collapse arrow
│       │   ├── Category name
│       │   ├── Count label "X/Y applied"
│       │   └── Enable All / Disable All buttons
│       └── _TweakRow items (N per category)
│           ├── Checkbox (BooleanVar)
│           ├── Status dot (colored circle)
│           ├── Label (tweak name)
│           ├── Tag badges
│           ├── REC badge (if applicable, teal)
│           └── _Tooltip (on hover)
└── Status Bar
    └── Status text label
```

## Keyboard Shortcuts

| Shortcut | Action | Method |
|----------|--------|--------|
| `Ctrl+A` | Select all visible tweaks | `_select_all()` |
| `Ctrl+D` | Deselect all visible tweaks | `_deselect_all()` |
| `Ctrl+F` | Focus search bar | `_focus_search()` |
| `Ctrl+E` | Export PowerShell script | `_export_powershell()` |
| `Ctrl+R` | Refresh all statuses | `_refresh_status_all()` |
| `Esc` | Clear search / reset filter | `_clear_search()` |

## Threading Model

- **UI thread**: All tkinter operations happen on the main thread
- **Worker threads**: `_toggle_single()`, `_batch_category()`, `_dispatch()` spawn `threading.Thread(daemon=True)`
- **Parallel detection**: `_refresh_status_all()` calls `status_map(parallel=True)` which uses `concurrent.futures.ThreadPoolExecutor` internally
- **Thread-safe updates**: Workers use `root.after(0, callback)` to schedule UI updates back on the main thread
- **Never blocks**: Long operations (apply/remove/detect) always run off the main thread
