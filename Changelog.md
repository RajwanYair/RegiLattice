# Changelog

All notable changes to RegiLattice are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
This project adheres to [Semantic Versioning](https://semver.org/).

## [Unreleased]

### Added

- **65 new tweaks across 13 categories** (1 360 → 1 425 total tweaks, 18 708 tests):
  - `telemetry_advanced.py` +5: `telem-disable-sqm-upload`, `telem-disable-mrt-report`,
    `telem-disable-speech-model-update`, `telem-disable-license-telemetry`,
    `telem-disable-ncsi-probing`
  - `remote_desktop.py` +5: `rdp-disable-wallpaper`, `rdp-enable-font-smoothing`,
    `rdp-disable-audio-record`, `rdp-enable-compression`, `rdp-single-session`
  - `windowsupdate.py` +5: `wu-disable-os-upgrade`, `wu-disable-safeguard-hold`,
    `wu-disable-optional-updates`, `wu-exclude-drivers-quality`, `wu-disable-ux-access`
  - `notifications.py` +5: `notif-disable-security-center`, `notif-disable-autoconnect`,
    `notif-disable-account-notif`, `notif-disable-tips-soft-landing`,
    `notif-disable-lock-screen-toasts`
  - `scheduled_tasks.py` +5: `schtask-disable-mrt-update`, `schtask-disable-speech-download`,
    `schtask-disable-power-diagnostics`, `schtask-disable-ngen-log`,
    `schtask-disable-smartscreen`
  - `printing.py` +5: `printing-copy-files-policy`, `printing-emf-despooling`,
    `printing-disable-client-side-map`, `printing-disable-spooler-log`,
    `printing-package-point-server-list`
  - `virtualization.py` +5: `virt-vds-manual`, `virt-disable-rdv-policy`,
    `virt-disable-containers-ext`, `virt-vmms-manual`, `virt-require-platform-security`
  - `widgets_news.py` +5: `widgets-disable-machine-feeds`, `widgets-disable-third-party-suggestions`,
    `widgets-disable-search-highlights`, `widgets-disable-spotlight-features`,
    `widgets-disable-start-personalization`
  - `power.py` +5: `power-no-password-on-resume`, `power-disable-throttling-policy`,
    `power-disable-energy-estimation`, `power-disable-sleep-away`,
    `power-standby-reserve-grace`
  - `performance.py` +5: `perf-win32-priority-sep`, `perf-gpu-hw-scheduling`,
    `perf-large-page-minimum`, `perf-games-io-priority`, `perf-reduce-hung-app-timeout`
  - `defender.py` +5: `sec-disable-wdigest`, `sec-enable-cred-guard-policy`,
    `sec-scan-not-idle-only`, `sec-block-exclusion-local-merge`,
    `sec-enable-behavior-monitoring`
  - `network.py` +5: `net-tcp-keepalive-5min`, `net-smb2-require-signing`,
    `net-block-non-domain-wifi`, `net-tcp-syn-attack-protection`, `net-tcp-timestamps`
  - `gaming.py` +5: `game-system-profile-games`, `game-disable-diagtrack-keyword`,
    `game-force-exclusive-fullscreen`, `game-honor-fse-compat`, `game-irq8-realtime`
- `regilattice/logger.py` — added `__all__` export list.

## [1.0.2] — 2025-07-19

### Added

- **18 new tweaks across 5 modules** to bring the total tweak count from 1 292 to 1 361:
  - `win11.py` +6: `w11-taskbar-never-combine`, `w11-disable-search-highlights`,
    `w11-disable-spotlight-tips`, `w11-disable-copilot-taskbar-btn`,
    `w11-disable-recall-ai`, `w11-show-this-pc-on-desktop`
  - `privacy.py` +3: `priv-disable-contacts-access`, `priv-disable-calendar-access`,
    `priv-disable-radios-access`
  - `defender.py` +3: `sec-enable-spectre-mitigations`, `sec-uac-always-notify`,
    `sec-restrict-ntlmv1`
  - `startup.py` +3: `startup-disable-ink-workspace`, `startup-disable-gamebar-capture`,
    `startup-disable-suggested-app-installs`
  - `wsl.py` +6 (Sprint 12): `wsl-interop-off-policy`, `wsl-disable-binfmt-misc`,
    `wsl-limit-processors`, `wsl-disable-crash-reporting`,
    `wsl-enable-nested-virt-policy`, `wsl-disable-telemetry`
- **`regilattice/logger.py`** — structured logging module (`configure_logging`,
  `get_logger`) with `--log-level` CLI flag integration.
- **`scripts/regilattice-completion.ps1`** — PowerShell tab-completion for all CLI
  flags, modes, profiles, log levels, and dynamic tweak IDs.
- **`docs/Security.md`** — subprocess call inventory and security audit reference
  covering all 7 modules that invoke external processes.
- **`regilattice/elevation.py`** — `_ALLOWED_ELEVATED_EXECUTABLES` allowlist;
  `run_elevated()` now validates executables before spawning privileged processes.
- **`regilattice/registry.py`** — `validate_registry_path()` function; rejects empty
  paths, null bytes, unknown hive prefixes, and missing subkeys.

### Changed

- Version bumped from `1.0.1` → `1.0.2`.

## [1.0.1] — 2025-07-05

### Security

  `askopenfilename` calls in `gui.py` and `gui_dialogs.py` now pass `parent=<root window>`.
  Without an explicit parent HWND the Win32 common dialog returns `E_UNEXPECTED` on Python 3.14 / 64-bit.
  Affected paths: _Export Log_, _Save Snapshot_, _Restore Snapshot_, _Export PowerShell_, _Export JSON_.
  Each call is also wrapped in `except tk.TclError: return` so any remaining edge-case triggers
  (headless CI, minimal Tk installs) are handled gracefully instead of crashing the callback.
- `export_json_selection` and `export_powershell` in `gui_dialogs.py` now accept an optional
  `parent: tk.Misc | None = None` keyword argument forwarded to the file dialog.

### Performance

- **`_wire_section_bindings` O(n²) → O(n)** — the binding loop formerly called `list.index(row)`
  for every row (O(n) per row), scanning all ~1 292 rows per lookup.  Replaced with an `id()`-keyed
  dict built once per call (O(n)), making per-row lookup O(1).  Wiring all 69 sections now takes
  ~20× fewer comparisons total.

### Tooling / DX

- **`pytest-xdist 3.8`** added to `[project.optional-dependencies].dev`.  Run the full test suite
  in parallel with `pytest -n auto --dist=worksteal` — 4–8× faster on machines with ≥ 4 cores.
- **`addopts`** changed from `-v --tb=short` to `-q --tb=short` (quiet mode by default, reducing
  output noise for 17 000+ parametrised smoke tests while keeping tracebacks on failure).
- New VS Code task **"Test (pytest parallel)"** — `pytest -n auto --dist=worksteal --tb=short -q`.
- Python runtime confirmed **64-bit** (CPython 3.14 AMD64); all dev dependencies resolved from
  the 64-bit wheel index.

## [1.0.1] — 2025-07-05

### Security

- **OWASP A03 Injection fix** (`maintenance.py`) — replace `subprocess.run(cmd, shell=True)` with a
  hardened list-based invocation in `create_restore_point()`.
- **PowerShell injection fix** (`gui_dialogs.py`) — all user-supplied and listbox-derived package /
  module names now pass through `_validate_package_name()` (strict `[A-Za-z0-9._-]` allow-list)
  before being interpolated into PowerShell command strings.  Covers `scoop install/remove` and
  `Install-Module / Uninstall-Module / Update-Module` actions.

### Testing

- `test_gui_dialogs.py` coverage **41 % → 89 %** (22 → 53 tests).  New: `TestValidatePackageName`,
  `TestRunPowershellCommand`, `TestOpenScoopManagerWithScoop`, `TestOpenPsModuleManager`.
- `test_gui_widgets.py` coverage **65 % → 95 %** (14 → 40 tests).  New: `TestTweakRowBadges`,
  `TestTweakRowInteraction`, `TestCategorySectionExtra`.

### CI / Tooling

- Add **Codecov** upload step to `ci.yml` (Python 3.12 matrix leg; `fail_ci_if_error: false`).

### Added

- **`TooltipManager` singleton** (`gui_tooltip.py`) — a single shared `tk.Toplevel` for all 1 200+ tweak row tooltips.  Replaces the previous per-row create/destroy strategy with a single deiconify/withdraw cycle per hover, eliminating ~1 200× `Toplevel` churn on busy scrolling.
- **Lazy `CategorySection` widget build** — `CategorySection` only constructs Tk widgets for its rows on first expand.  All sections start collapsed and their row frames are `None` until the user opens them (or a search filter matches them), reducing startup widget creation from ~6 000 to a handful.
- **`set_on_rows_built(callback)` on `CategorySection`** — register a post-build callback fired once after `_build_row_widgets()` completes.  Used by `gui.py` to wire keyboard / context-menu bindings and apply cached statuses to newly built rows.
- **Delta status tracking** (`_prev_statuses`) — `_apply_statuses()` now keeps a `dict[str, TweakResult]` cache of the last-propagated statuses.  Only rows whose status has _changed_ have their widgets reconfigured, reducing redundant Tk IPC on large refresh cycles.
- **`_wire_section_bindings(section)`** new method in `RegiLatticeGUI` — wires shift-click, row-click, and right-click context-menu bindings for a section's rows, then applies cached statuses to newly built widgets.
- **New tests (Sprint 7)**: `TestTooltipManager` (10 tests), `TestCategorySectionLazy` (8 tests), `TestDeltaStatus` (7 tests), `TestWireSectionBindings` (4 tests).

### Changed / Fixed

- `_switch_theme()` now skips `row.apply_theme()` for unbuilt rows (`row.frame is None`), limiting theme-switch cost to the set of _visible_ rows; also clears `_prev_statuses` to force a full repaint on the next status cycle.
- `_finish_loading()` now wires `var.trace_add` for all rows immediately (works on `BooleanVar` regardless of widget build), but defers keyboard and context-menu bindings to `_wire_section_bindings()` called lazily per section.
- `_filter_rows()` triggers lazy widget build for any collapsed section that has matching rows, so search results are always accurate regardless of expand state.
- `gui` fixture in `tests/test_gui.py` now wraps `RegiLatticeGUI()` in a try/except and calls `pytest.skip()` on Tcl/Tk initialisation errors, preventing false failures in environments with partial Tk installations.

### Performance summary (Sprint 7)

| Scenario | Before | After |
|---|---|---|
| Cold startup widget count | ~6 000+ Tk widgets | ~200 (header frames only) |
| Per-hover Toplevel churn | create + destroy × 1 200 | deiconify / withdraw × 1 |
| Status refresh Tk IPC calls | O(all rows) per cycle | O(changed rows) per cycle |
| Theme switch cost | O(all rows) | O(built rows) |

---

- **Instant GUI exit** — `_quit()` now calls `_root.withdraw()` first to hide the window immediately, then persists state (geometry, collapse, preferences, search history) in a background thread before `_root.destroy()`.  Alt+F4 / tray Quit now feel instant.
- **`--doctor` CLI command** — 7-point health check: Python version, winreg availability, admin status, config validity, tweak count/duplicates, corp-guard, log-path write-ability.
- **`TweakDef.source_url`** — optional KB article / documentation URL per tweak (defaults to `""`).
- **Auto system-theme detection on first run** — when no `preferences.json` exists the GUI auto-detects dark/light mode and applies the matching Catppuccin variant.
- **Path-traversal security guard in `marketplace.load_plugin()`** — plugin path must be inside `_PLUGINS_DIR`; loads from `../` are rejected with `RuntimeError`.

### Changed / Fixed

- `_MissingSentinel` now inherits `ModuleType` so `lazy_import()` has a clean return type without `# type: ignore`.
- `TweakRow` widget attributes typed as `... | None` with explicit `None`-guards (`_on_enter`, `_on_leave`, `_refresh_row_bg`, `pack_row`, `unpack_row`, `refresh_status`).
- Pre-commit: added `trailing-whitespace`, `end-of-file-fixer`, `check-yaml/toml/json`, `check-added-large-files`, `mixed-line-ending (lf)` hooks; ruff now uses `--exit-non-zero-on-fix`; mypy uses `--ignore-missing-imports`.

### Infrastructure

- Registry read-cache (`SESSION.read_cache()`) already wraps the full `status_map()` call — all registry reads during a refresh pass are served from an in-memory dict, eliminating redundant `winreg.OpenKey` round-trips.
- Tests: +245 new tests (Sprint 5) including `TestDoctor`, marketplace path-traversal, and `source_url` coverage.

---

## [1.0.1] — 2026-03-08 (Sprint 7 + Sprint 8 + Phase 01-02)

### Added

- **`RegistrySession.set_expand_string` / `read_expand_string`** — read/write `REG_EXPAND_SZ` values with lru_cache.
- **`RegistrySession.set_multi_sz` / `read_multi_sz`** — read/write `REG_MULTI_SZ` (list-of-strings) values with lru_cache.
- **`RegistrySession.read_binary` / `read_qword`** — read `REG_BINARY` and `REG_QWORD` values.
- **`RegistrySession.set_binary` / `set_qword`** — write `REG_BINARY` and `REG_QWORD` values.
- **`RegistrySession.list_values`** — enumerate `(name, value, type)` triples in a key.
- **`RegistrySession.list_keys`** — enumerate child key names.
- **`corp_guard_reasons()`** — returns a copy of the reason-list that triggered corporate detection; thread-safe.
- **`reset_corp_cache()`** — clears the corporate detection cache for testing and hot-reload scenarios.
- **`detect_battery()`** — `@lru_cache` probe returning `True` when a battery/UPS is detected.
- **`detect_network_type()`** — `@lru_cache` probe returning `"vpn"`, `"wifi"`, `"ethernet"`, or `"unknown"`.
- **`HWProfile.has_battery`** and **`HWProfile.network_type`** fields populated by the new probes.
- **`analytics.record_error_for(tweak_id)`** — track per-tweak error counts.
- **`analytics.error_stats()`** — return `dict[str, int]` of per-tweak error counts.
- **`ratings.average_rating()`** — mean star rating across all rated tweaks (`None` when empty).
- **`ratings.rated_count()`** — number of tweaks that have been rated.
- **`filter_tweaks()`** — composable multi-criterion filter (`corp_safe`, `needs_admin`, `scope`, `category`, `min_build`, `tags`, `query`).
- **`tweak_dependencies()`** — DFS-based transitive/one-hop dependency resolver returning deps in topological order.
- **`apply_tweaks()` / `remove_tweaks()`** — ID-list batch helpers with auto dep resolution (`include_deps=True` by default).
- **`status_map(ids=)`** — partial evaluation: restrict detection to a subset of tweaks for GUI incremental refresh.
- **`tweaks_by_scope(scope)`** — return tweaks matching a specific scope string.
- **`tweaks_above_build(build)`** — return tweaks whose `min_build` is `<= build`.
- **`tweak_risk_level(td)`** — classify a tweak as `"low"`, `"medium"`, or `"high"` risk.
- **`tweak_count_by_scope()`** — `dict[str, int]` counts per scope key (`user/machine/both`).
- **`category_counts()`** — `dict[str, int]` mapping category name to tweak count.
- **`AppConfig.theme`** — new field (default `"system"`) loaded from `[general] theme` in TOML.
- **`AppConfig.locale`** — new field (default `"en"`) loaded from `[general] locale` in TOML.
- **CLI `--scope {user,machine,both}`** — filter `--list` / `--search` output by registry scope.
- **CLI `--min-build N`** — filter `--list` / `--search` output by minimum Windows build.
- **CLI `--corp-safe`** — filter `--list` / `--search` to HKCU-only tweaks.
- **CLI `--needs-admin`** — filter `--list` / `--search` to admin-required tweaks.
- **CLI `--validate`** — non-destructive consistency check across all `TweakDef` entries.
- **CLI `--stats`** — rich stats breakdown (total tweaks, categories, scope distribution, corp-safe, admin, dep depth).
- **CLI `--output {table,json}`** — machine-readable JSON output for `--list`, `--search`, `--categories`.
- **CLI `--list --category <name>`** — filter `--list` to a single category; returns exit code 2 for unknown categories.
- **CLI `--list-categories`** — alias for `--categories`.
- **`docs/Roadmap.md`** — 5-sprint roadmap, 50-item backlog, velocity tracking.
- **`.github/docs/project-spec.md`** — relocated from repo root.

### Changed / Fixed

- **`_invalidate_cache_for` bugfix** — previously only cleared `dword`/`string`/`exists` suffixes; now clears all 6: `dword`, `string`, `binary`, `qword`, `expand`, `multi_sz`.
- **`filter_tweaks()` early-exit** — `if not pool: return pool` after each filter step avoids unnecessary work when the result pool drains early.
- **`status_map()` detect-free skip** — tweaks with `detect_fn=None` are assigned `TweakResult.UNKNOWN` directly without being submitted to the thread pool.
- **`_split_root()` memoization** — `@functools.lru_cache(maxsize=256)`; repeated registry path splitting is now O(1) on cache hit.
- **`detect_hardware()` workers raised to 6** — runs `detect_battery` and `detect_network_type` in the parallel probe pool.
- **`_SCOPE_CACHE` / `_SCOPE_LOCK` ordering fix** — moved before `_load_plugins()` to eliminate `NameError` at import time.
- **Scope pre-warm** — `_load_plugins()` now pre-populates `_SCOPE_CACHE` for all tweaks at import.
- **Thread-safety** — `threading.Lock` guards added to all shared caches in `analytics.py`, `config.py`, `corpguard.py`, `locale.py`, `marketplace.py`, and `tweaks/__init__.py`.
- **Caching improvements** — `_split_root` result cache, `_TAG_INDEX` for O(1) tag lookup, plugin-prewarm on first import.
- **`__all__`** lists added/completed in 5 core modules (`analytics`, `config`, `corpguard`, `locale`, `tweaks/__init__`).
- **`typing.Final`** applied to 12 module-level constants.
- **`_VALID_HIVE_PREFIXES`** promoted to module-level `frozenset` (was re-created per call).
- **`_PREFIX_LIST`** pre-sorted longest-first for unambiguous `_split_root()` matching.

### Infrastructure

- **17 511 tests** across 21 test files (was 17 266 at v1.0.0; +245 new tests across Sprints 7–8 and phase-01).
- `.gitattributes` rewritten for Python project (was ExplorerLens C++ copy); `*.py text eol=lf` eliminates CRLF warnings.
- Duplicate CI workflow `python.yml` removed (canonical: `.github/workflows/ci.yml`).
- Redundant `.flake8` removed (ruff is the primary linter).
- `[project.urls]` added to `pyproject.toml` (Homepage, Repository, Bug Tracker, Changelog).
- `pyproject.toml` `[tool.pylint]` extended with documented suppressions.
- ruff: all import-sort (I001) issues fixed; all checks pass.
- mypy `--strict`: 0 issues.

---

## [1.0.0] — 2026-03-07

### Added

- **1 228 registry tweaks** across 64 categories (Accessibility, Adobe, AI/Copilot,
  Audio, Backup, Bluetooth, Boot, Chrome, Clipboard, Cloud Storage, Communication,
  Context Menu, Cortana, Crash & Diagnostics, Developer Tools, Display, DNS,
  Edge, Explorer, File System, Firefox, Fonts, Gaming, GPU, Indexing, Input, Java,
  LibreOffice, Lock Screen, M365 Copilot, Maintenance, Microsoft Store, Multimedia,
  Network, Notifications, Office, OneDrive, Package Management, Performance, Power,
  Printing, Privacy, RealVNC, Remote Desktop, Scheduled Tasks, Scoop Tools,
  Screensaver, Security, Services, Shell, Snap, Startup, Storage, System, Taskbar,
  Telemetry, Terminal, USB, Virtualization, VS Code, Widgets, Windows 11,
  Windows Update, WSL).
- **Zero-wiring plugin loader** — drop a `.py` in `tweaks/` and it auto-discovers.
- **TweakDef dataclass** with `apply_fn`, `remove_fn`, `detect_fn` triplet pattern.
- **5 profiles**: Business, Gaming, Privacy, Minimal, Server.
- **Tkinter GUI** with 4 themes (Catppuccin Mocha/Latte, Nord, Dracula), deferred
  loading, collapsible categories, search/filter, scope badges, tooltips, and
  keyboard navigation.
- **argparse CLI** with `apply`, `remove`, `list`, `gui`, `profile` commands.
- **Interactive console menu** for guided tweak management.
- **RegistrySession** wrapping `winreg` with backup, logging, and dry-run support.
- **Corporate guard** detecting AD/AAD/VPN/GPO/SCCM environments.
- **UAC elevation helpers** (`is_admin`, `request_elevation`, `run_elevated`).
- **AppConfig** via `~/.regilattice.toml` for user preferences.
- **Local analytics** (applies, removes, sessions) and **rating system** (1–5 stars).
- **i18n string table** and **plugin marketplace** (third-party discovery).
- **Export PS1** and **Import JSON** dialogs, **Scoop Manager**, **About** dialog.
- **GitHub Actions CI** (matrix Python 3.10–3.14, ruff, mypy, pytest+coverage).
- **17 266 tests** across 20 test files (smoke, unit, integration, property, benchmarks).

### Infrastructure

- Build: `hatchling` via `pyproject.toml`.
- Lint: `ruff` (E, F, W, I, UP, B, SIM, RUF; 150-char lines; ARG002 ignored).
- Type-check: `mypy --strict` and Pyright/Pylance standard mode.
- Test: `pytest` with `pytest-cov`, `hypothesis` property tests, benchmarks.
- VS Code workspace: settings, tasks, launch configs, recommended extensions.
