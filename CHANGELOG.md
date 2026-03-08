# Changelog

All notable changes to RegiLattice are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
This project adheres to [Semantic Versioning](https://semver.org/).

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
