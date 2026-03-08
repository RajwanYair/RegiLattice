# Changelog

All notable changes to RegiLattice are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
This project adheres to [Semantic Versioning](https://semver.org/).

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
