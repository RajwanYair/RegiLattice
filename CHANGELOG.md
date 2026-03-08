# Changelog

All notable changes to RegiLattice are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
This project adheres to [Semantic Versioning](https://semver.org/).

## [Unreleased] — Sprint 8 refactor (C13–C23, 2026-03)

### Added

- **`RegistrySession.set_expand_string` / `read_expand_string`** — read/write `REG_EXPAND_SZ` values with lru_cache.
- **`RegistrySession.set_multi_sz` / `read_multi_sz`** — read/write `REG_MULTI_SZ` (list-of-strings) values with lru_cache.
- **`corp_guard_reasons()`** — returns a copy of the reason-list that triggered corporate detection; thread-safe.
- **`reset_corp_cache()`** — clears the corporate detection cache for testing and hot-reload scenarios.
- **`detect_battery()`** — `@lru_cache` probe returning `True` when a battery/UPS is detected.
- **`detect_network_type()`** — `@lru_cache` probe returning `"vpn"`, `"wifi"`, `"ethernet"`, or `"unknown"`.
- **`HWProfile.has_battery`** and **`HWProfile.network_type`** fields populated by the new probes.
- **`analytics.record_error_for(tweak_id)`** — track per-tweak error counts.
- **`analytics.error_stats()`** — return `dict[str, int]` of per-tweak error counts.
- **`ratings.average_rating()`** — mean star rating across all rated tweaks (`None` when empty).
- **`ratings.rated_count()`** — number of tweaks that have been rated.
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
- **133 new tests** across 9 test files (C16–C23 additions).

### Changed / Fixed

- **`_invalidate_cache_for` bugfix** — previously only cleared `dword`/`string`/`exists` suffixes; now clears
  all 6: `dword`, `string`, `binary`, `qword`, `expand`, `multi_sz`.
- **`filter_tweaks()` early-exit** — adds `if not pool: return pool` after each filter step, avoiding
  unnecessary work when the result pool drains early.
- **`status_map()` detect-free skip** — tweaks with `detect_fn=None` are assigned `TweakResult.UNKNOWN`
  directly without being submitted to the thread pool.
- **`_split_root()` memoization** — decorated with `@functools.lru_cache(maxsize=256)`; repeated registry
  path splitting is now O(1) on cache hit.
- **`detect_hardware()` workers raised to 6** — runs `detect_battery` and `detect_network_type` in the
  parallel probe pool alongside the existing 4 probes.
- **`_SCOPE_CACHE` / `_SCOPE_LOCK` ordering fix** — moved definitions to before the `_load_plugins()`
  function to eliminate `NameError` at import time.
- **Scope pre-warm** — `_load_plugins()` now pre-populates `_SCOPE_CACHE` for all tweaks at import.

### Infrastructure

- **17 511 tests** across 21 test files after C13–C23 additions (was 17 378 at Sprint 7).
- ruff: all checks pass; mypy `--strict`: 0 issues.

## [Unreleased] — Sprint 7 refactor (C2–C11, 2026-03)

### Added

- **`filter_tweaks()`** — composable multi-criterion filter (`corp_safe`, `needs_admin`,
  `scope`, `category`, `min_build`, `tags`, `query`) in `tweaks/__init__.py`.
- **`tweak_dependencies()`** — DFS-based transitive/one-hop dependency resolver returning
  deps in topological order.
- **`apply_tweaks()` / `remove_tweaks()`** — ID-list batch helpers with auto dep resolution
  (`include_deps=True` by default).
- **`status_map(ids=)`** — partial evaluation: pass an `Iterable[str]` to restrict detection
  to a subset of tweaks (GUI incremental refresh).
- **`RegistrySession.read_binary` / `read_qword`** — read `REG_BINARY` and `REG_QWORD` values.
- **`RegistrySession.set_binary` / `set_qword`** — write `REG_BINARY` and `REG_QWORD` values.
- **`RegistrySession.list_values`** — enumerate `(name, value, type)` triples in a key.
- **`RegistrySession.list_keys`** — enumerate child key names.
- **CLI `--validate`** — non-destructive consistency check across all `TweakDef` entries
  (duplicate IDs, empty labels/categories, missing fn pointers).
- **CLI `--stats`** — rich stats breakdown (total tweaks, categories, scope distribution,
  corp-safe count, admin count, dependency depth).
- **CLI `--output {table,json}`** — switch `--list`, `--search`, and `--categories` to
  machine-readable JSON output.
- **CLI `--list --category <name>`** — filter `--list` output to a single category;
  returns exit code 2 for unknown categories.
- **CLI `--list-categories`** — alias for `--categories` (more intuitive spelling).
- **43 new tests** for registry edge cases (mocked `EnumValue`/`EnumKey`/`SetValueEx`,
  `REG_BINARY`/`REG_QWORD` type constants verified), plus C6 CLI new-flag coverage.
- **18 new CLI tests** covering `--validate`, `--stats`, `--output json`, `--list --category`,
  and `--list-categories`.

### Changed

- **`_VALID_HIVE_PREFIXES`** promoted to module-level `frozenset` (was re-created per call).
- **`_PREFIX_LIST`** pre-sorted longest-first for unambiguous `_split_root()` matching.
- **Thread-safety** — `threading.Lock` guards added to all shared caches in `analytics.py`,
  `config.py`, `corpguard.py`, `locale.py`, `marketplace.py`, and `tweaks/__init__.py`.
- **Caching improvements** — `_split_root` result cache, `_TAG_INDEX` for O(1) tag lookup,
  plugin-prewarm on first import.
- **`__all__`** lists added / completed in 5 core modules
  (`analytics`, `config`, `corpguard`, `locale`, `tweaks/__init__`).
- **`typing.Final`** applied to 12 module-level constants.

### Infrastructure

- **17 378 tests** across 21 test files after C2–C11 additions (was 17 266 at 1.0.0).
- ruff: all checks pass (`E`, `F`, `W`, `I`, `UP`, `B`, `SIM`, `RUF`; line-length 150).
- mypy `--strict`: no issues in 90 source files.

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
