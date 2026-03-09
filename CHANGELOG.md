# Changelog

All notable changes to RegiLattice are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
This project adheres to [Semantic Versioning](https://semver.org/).

## [Unreleased]

### Added

- **Force Kill button** (`⛔ Force Kill`) in the GUI toolbar — calls `os._exit(0)` for immediate process termination when the normal close path is unavailable.
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
- **ROADMAP.md** — 5-sprint roadmap, 50-item backlog, velocity tracking.
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
