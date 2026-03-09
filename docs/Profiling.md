# RegiLattice — GUI Performance & Profiling Guide

> **Sprint 8 deliverable** — systematic approach to diagnosing and fixing Python
> GUI slowness. All code examples are production-ready and adapted to this codebase.
> Last updated: 2026-03-09

---

## Table of Contents

1. [Performance Overview](#1-performance-overview)
2. [Bottleneck Detection Strategy](#2-bottleneck-detection-strategy)
3. [Profiling with cProfile — Step-by-Step](#3-profiling-with-cprofile--step-by-step)
4. [Visualising Results with snakeviz](#4-visualising-results-with-snakeviz)
5. [Additional Profiling Tools](#5-additional-profiling-tools)
6. [Built-in Profiling Helpers](#6-built-in-profiling-helpers-regilatticeprofilerpy)
7. [Interpreting Results — Common Culprits](#7-interpreting-results--common-culprits)
8. [Prioritised Refactor Plan](#8-prioritised-refactor-plan)
9. [Architecture Reference](#9-architecture-reference)

---

## 1. Performance Overview

A Python GUI can feel slow for three distinct reasons. Misdiagnosing them wastes effort:

| Root Cause | Symptoms | Tool to diagnose |
|---|---|---|
| **CPU-bound** | CPU % spikes, UI freezes during computation | `cProfile`, `pyinstrument` |
| **I/O-bound** | Freeze waiting for disk/registry/network | `cProfile` `tottime` in I/O calls, `strace` / ProcMon |
| **Rendering / UI-thread stall** | UI feels heavy even for "nothing" | `time.perf_counter()` around Tk calls, `Stopwatch` |
| **Widget overload** | Startup or theme-switch takes seconds | Widget count audit, lazy build (Sprint 7 ✅) |

> **Golden rule** — The main (UI) thread must never block for more than ~16 ms
> (one frame at 60 fps). Any work longer than 50 ms should run on a background thread.

---

## 2. Bottleneck Detection Strategy

### 2.1 Distinguishing bottleneck types

```python
# Is it CPU or I/O? Add two timing points and watch CPU in Task Manager:
from regilattice.profiler import Stopwatch

with Stopwatch("status_map_total") as sw:
    statuses = status_map(parallel=True)
# If elapsed_ms is high and CPU was also high → CPU-bound
# If elapsed_ms is high and CPU was near 0 → I/O-bound (registry reads waiting)
```

### 2.2 Detecting UI-thread stalls

Tkinter does all rendering on the main thread. To find where the thread stalls:

```python
import time, tkinter as tk

_LAST_FRAME = time.perf_counter()

def _frame_watchdog(root: tk.Tk) -> None:
    global _LAST_FRAME
    now = time.perf_counter()
    gap = (now - _LAST_FRAME) * 1000
    if gap > 50:
        print(f"[watchdog] UI thread stalled for {gap:.0f} ms")
    _LAST_FRAME = now
    root.after(16, _frame_watchdog, root)   # call every 16 ms (60 fps)

# Start it after mainloop begins:
root.after(16, _frame_watchdog, root)
```

### 2.3 Identifying rendering inefficiency

- **Check widget count**: `len(root.winfo_children())` recursively. Above ~500 tk widgets
  causes slow redraws. Sprint 7 reduced this from ~6 000 to ~200 via lazy build.
- **Check redraws**: Wrap any code that calls `configure()` on widgets in a Stopwatch.
- **Theme switch cost**: If `_switch_theme` is slow, check how many rows have
  `row.frame is not None` (unbuilt rows should be skipped — Sprint 7 ✅).

### 2.4 Data/model inefficiency

- **Repeated parsing**: Run `cProfile` on startup. If `json.loads`, `Path.read_text`,
  or registry reads appear high in `ncalls`, add caching (`functools.lru_cache`).
- **Repeated `all_tweaks()` calls**: `all_tweaks()` is cached in `tweaks/__init__.py`.
  Any code calling it more than once per session is redundant.
- **`status_map()` on every keypress**: The search bar debounces at 300 ms and never
  triggers `status_map()`. But if a callback accidentally triggers it, it will freeze.

---

## 3. Profiling with cProfile — Step-by-Step

### 3.1 Whole-application profile

```powershell
# Profile the full GUI startup + a few interactions (close window when done):
C:\Users\ryair\AppData\Local\Python\bin\python.exe `
    -m cProfile -o docs/profile_out/startup.prof `
    -m regilattice --gui

# Inspect immediately in the terminal (top 20 functions by cumulative time):
C:\Users\ryair\AppData\Local\Python\bin\python.exe -c "
import pstats
p = pstats.Stats('docs/profile_out/startup.prof')
p.sort_stats('cumulative')
p.print_stats(20)
"
```

### 3.2 Profile a specific CLI operation

```powershell
# Profile --list (useful to see tweak-loader cost):
C:\Users\ryair\AppData\Local\Python\bin\python.exe `
    -m cProfile -o docs/profile_out/list.prof `
    -m regilattice --list
```

### 3.3 Wrap a GUI callback in-code

Use `profile_fn` from `regilattice.profiler` to profile a single action without
modifying the function's signature:

```python
from regilattice.profiler import profile_fn
from regilattice.tweaks import status_map

# Profile one status refresh and save .prof for snakeviz:
result = profile_fn(
    status_map,
    kwargs={"parallel": True},
    output="docs/profile_out/status_map.prof",
    top_n=20,
    sort_by="cumulative",
)
```

Or wrap a method at class definition time:

```python
# In gui.py (temporary — remove after profiling session):
from regilattice.profiler import timed

class RegiLatticeGUI:
    @timed("gui._on_apply_all_clicked", threshold_ms=10)
    def _on_apply_all_clicked(self) -> None:
        ...
```

### 3.4 Profile a specific action triggered from the GUI menu / button

```python
# In gui.py — patch _on_apply_all_clicked to run under cProfile:
import cProfile

def _profiled_apply_all(self) -> None:
    cProfile.runctx(
        "self._do_apply_all()",
        globals=globals(),
        locals={"self": self},
        filename="apply_all.prof",
    )
```

### 3.5 Reading pstats output — key columns

```
   ncalls  tottime  percall  cumtime  percall filename:lineno(function)
     5000    0.120    0.000    1.500    0.000 registry.py:88(read_dword)
```

| Column | Meaning | What to look for |
|---|---|---|
| `ncalls` | How many times the function was called | Unexpectedly high count = redundant calls |
| `tottime` | Time *only* inside this function (excludes callees) | High = hot loop itself |
| `cumtime` | Total time including all callees | High = expensive subtree |
| `percall` (tottime) | `tottime / ncalls` | High even with few calls = slow inner logic |
| `percall` (cumtime) | `cumtime / ncalls` | High = each call spawns expensive work |

**Decision rules:**
- `ncalls` very high + `tottime` medium → reduce call frequency (debounce, cache)
- `tottime` high → optimize the function body itself
- `cumtime` high, `tottime` low → the real cost is in callees (drill deeper)

---

## 4. Visualising Results with snakeviz

`snakeviz` renders a `.prof` file as an interactive icicle chart in the browser.

### Install

```powershell
C:\Users\ryair\AppData\Local\Python\bin\python.exe -m pip install snakeviz
```

### Run

```powershell
# Serve the icicle chart on http://127.0.0.1:8080:
C:\Users\ryair\AppData\Local\Python\bin\python.exe -m snakeviz docs/profile_out/startup.prof
```

### Reading the chart

- **Icicle chart**: outer (top) = root caller, inner (bottom) = callee. Wider = more cumtime.
- **Click a bar** to zoom into that subtree.
- **Table below** the chart: sortable columns, filters by function name.
- **`[self]` entry** in a bar = time inside that function excluding callees = `tottime`.

---

## 5. Additional Profiling Tools

### pyinstrument — Statistical profiler (low overhead)

Better for long-running sessions because it samples the call stack rather than
intercepting every function call (10–50× lower overhead than cProfile).

```powershell
# Install:
C:\Users\ryair\AppData\Local\Python\bin\python.exe -m pip install pyinstrument

# Profile startup:
C:\Users\ryair\AppData\Local\Python\bin\python.exe `
    -m pyinstrument -m regilattice --list

# Profile GUI (close window when done):
C:\Users\ryair\AppData\Local\Python\bin\python.exe `
    -m pyinstrument --renderer html -o docs/profile_out/startup_pyinstrument.html `
    -m regilattice --gui
```

Pyinstrument shows a **flame tree** grouped by call stack — excellent for finding
the deepest slow path without manually tracing cumtime chains in pstats.

### line_profiler — Line-by-line timing of a single function

```powershell
C:\Users\ryair\AppData\Local\Python\bin\python.exe -m pip install line-profiler
```

```python
# Decorate the target function (temporary):
from line_profiler import profile

@profile
def _apply_statuses(self, statuses):
    ...
```

```powershell
# Run with kernprof:
kernprof -l -v -m regilattice --list
```

Use this only when `cProfile` has already identified `_apply_statuses` (or similar)
as the hot function and you need to know exactly which line is expensive.

### tracemalloc — Memory growth over time

Useful if the GUI becomes sluggish after being open for several minutes (memory
pressure causing GC pauses or Python's allocator slowing down).

```python
import tracemalloc

tracemalloc.start()
# ... do the slow operation ...
snapshot = tracemalloc.take_snapshot()
for stat in snapshot.statistics("lineno")[:10]:
    print(stat)
```

---

## 6. Built-in Profiling Helpers (`regilattice/profiler.py`)

This module ships three utilities with zero runtime dependencies.

### `@timed` decorator

Logs elapsed time at DEBUG (green) or WARNING (yellow) automatically:

```python
from regilattice.profiler import timed

@timed("status_refresh", threshold_ms=50)
def _apply_statuses(self, statuses: dict) -> None:
    ...
# Logged: [profiler] status_refresh                  41.3 ms   (DEBUG)
# Logged: [profiler] status_refresh                 312.7 ms   (WARNING — exceeds 50 ms)
```

Already applied to these hot paths in `gui.py`:
- `_finish_loading` (wires 1 200+ row bindings)
- `_filter_rows` (search bar debounce callback)
- `_apply_statuses` (delta status refresh)
- `_switch_theme` (theme propagation)

### `Stopwatch` context manager

```python
from regilattice.profiler import Stopwatch

with Stopwatch("populate_category_sections") as sw:
    self._grouped_items = ...  # expensive grouping
print(f"Grouping took {sw.elapsed_ms:.0f} ms")
```

### `profile_fn` — one-shot cProfile wrapper

```python
from regilattice.profiler import profile_fn

profile_fn(
    status_map,
    kwargs={"parallel": True},
    output="docs/profile_out/status_map.prof",
    top_n=20,
)
```

### Enable logging output

Profiler messages go to the `regilattice.profiler` logger at DEBUG/WARNING.
To see them in the console during development:

```python
import logging
logging.basicConfig(level=logging.DEBUG)
# or for profiler only:
logging.getLogger("regilattice.profiler").setLevel(logging.DEBUG)
```

---

## 7. Interpreting Results — Common Culprits

### Pattern A — High `ncalls` on registry reads

If `read_dword` / `key_exists` appears in top-20 with ncalls > 1 000:
- `status_map(parallel=True)` uses a thread pool — this is expected
- If seen in sequential mode, check whether detect functions are called outside `status_map`

### Pattern B — High `cumtime` on `all_tweaks()`

`all_tweaks()` auto-discovers and imports 69 modules. First call takes ~150–200 ms on
an average disk. It is cached after that. If it appears multiple times in a profile:
- Search for call sites outside of `RegiLatticeGUI.__init__`

### Pattern C — High `tottime` in `tk.Widget.configure`

Indicates that too many widgets are being reconfigured. Sprint 7 added delta tracking
for `_apply_statuses` and lazy build for `_switch_theme`. If `configure` still appears:
- Check for any loop over `self._tweak_rows` that calls `configure` without a `if row.frame is not None` guard.

### Pattern D — High `tottime` in `json.loads` / `Path.read_text`

Theme file or config file being read repeatedly. Fix: load once in `__init__`, cache.

### Pattern E — Slow search bar

`_filter_rows` should complete in < 50 ms for 1 292 tweaks (no regex, pure string
membership check). If it's slow, check that `_build_row_widgets()` is only called
once per section (guarded by `_widgets_built`).

---

## 8. Prioritised Refactor Plan

### Phase 1 — Quick wins (< 1 hour each) ✅

| Done | Change | Impact |
|---|---|---|
| ✅ | Lazy `CategorySection._build_row_widgets` | Startup: ~6 000 → ~200 widgets |
| ✅ | `TooltipManager` singleton | 1 shared Toplevel per session |
| ✅ | `_apply_statuses` delta tracking | Status refresh: O(all) → O(changed) |
| ✅ | `_switch_theme` skip unbuilt rows | Theme switch: O(N) → O(built) |

### Phase 2 — Medium effort (1–4 hours each)

| Priority | Change | Impact |
|---|---|---|
| P1 | Move `status_map()` to a dedicated background thread with a 2 s interval | Eliminate all UI-thread blocking during detection |
| P1 | Add `@timed` + logging to all `_on_*` callbacks to find hidden slow paths | Systematic visibility |
| P2 | Virtual scrolling for the category list (render only visible rows) | Memory: further reduce from ~200 to ~30 at any time |
| P2 | Profile startup with `cProfile -o startup.prof` and snakeviz | Identify module-import bottleneck |

### Phase 3 — Architectural improvements (4–16 hours each)

| Priority | Change | Impact |
|---|---|---|
| P1 | MVC separation: extract `TweakViewModel` from `RegiLatticeGUI` | Testability, reusability |
| P2 | Replace status polling with event-driven registry watchers (`RegNotifyChangeKeyValue`) | Near-zero CPU on idle |
| P3 | Migrate to `tkinter` + `ttk` virtual tree for the main list (`ttk.Treeview`) | Proven O(1) scroll for large item sets |

### Phase 4 — Future exploration

- Evaluate **Textual** (terminal UI, cross-platform, async-native) for a TUI alternative
- Evaluate **wxPython** or **PyQt6** for native Win11 look without DWM hacks
- Add `asyncio` event loop bridged to Tkinter for non-blocking I/O

---

## 9. Architecture Reference

### Current hot path — status refresh

```
_status_refresh_worker() [background thread]
    └── status_map(parallel=True)          ~300–800 ms (69 × detect_fn)
         └── ThreadPoolExecutor             parallel registry reads
                                            ↓
_apply_statuses() [UI thread, via after()]
    └── for row in _tweak_rows             1 292 iterations
         └── if status unchanged → skip    Delta tracking (Sprint 7 ✅)
         └── else → configure widgets      ~0.1 ms per row
```

### Startup sequence

```
python -m regilattice --gui
    └── cli.main()
    └── gui.launch()
         └── RegiLatticeGUI.__init__()
              ├── _build_ui()              ~20 ms (header, search, scrollframe)
              ├── TooltipManager.init()    ~1 ms
              └── root.after(100, _deferred_init)
                   ├── hw_detect_worker() [background]
                   ├── corp_check_worker() [background]
                   └── _populate_batch() × N batches (4 sections/batch × 1 ms)
                        └── CategorySection(expanded=False)  zero widget cost
                   └── _finish_loading()
                        └── _refresh_statuses() [background thread]
```

### Key performance numbers (Sprint 7 baseline)

| Metric | Before Sprint 7 | After Sprint 7 |
|---|---|---|
| Startup widget count | ~6 000 | ~200 |
| Time to visible window | ~3–8 s | < 1 s |
| Theme switch time | ~1–3 s | < 100 ms |
| Status refresh (delta, no change) | ~500 ms | < 10 ms |
| Tooltip object count | 1 200 Toplevels | 1 Toplevel |
