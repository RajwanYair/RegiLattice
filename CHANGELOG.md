# Changelog

All notable changes to RegiLattice are documented in this file.

## [1.0.0] — 2026-03-07

### Added

- **1 228 registry tweaks** across 64 categories covering Windows 11, privacy,
  performance, networking, browsers, developer tools, and more.
- **5 machine-purpose profiles**: Business, Gaming, Privacy, Minimal, Server —
  each pre-selects a curated set of categories.
- **Tkinter GUI** with 4 colour themes (Catppuccin Mocha/Latte, Nord, Dracula),
  deferred loading, threaded execution, collapsible category sections, search bar
  with prefix operators (`tag:`, `cat:`, `scope:`, `admin:`), status/scope filters,
  profile & theme selectors, Export PS1, Import JSON, Scoop Tools Manager, About
  dialog, keyboard shortcuts, right-click context menu, and toggleable log viewer.
- **DPI / scaling awareness** — Per-Monitor v2 DPI with system-level fallback.
- **Window geometry persistence** — size and position saved to
  `~/.regilattice/window.json` across sessions.
- **Per-row status updates** during batch apply/remove operations.
- **Corporate network guard** — detects AD domain, Azure AD/Entra ID, VPN,
  GPO, and SCCM/Intune; blocks non-corp-safe tweaks with `--force` override.
  Detection result cached for the process lifetime.
- **Plugin loader** — auto-discovers tweak modules from `regilattice/tweaks/`;
  errors in individual modules are isolated with `warnings.warn()` instead of
  crashing the entire loader.
- **CLI flags**: `--list`, `--gui`, `--profile`, `--snapshot`, `--restore`,
  `--snapshot-diff`, `--dry-run`, `--force`, `--check-deps`, `--config`,
  `--search`, `--category`, `--export-json`.
- **Interactive console menu** with numbered tweak selection.
- **Registry session** — winreg wrapper with backup/restore, dry-run mode, and
  structured logging.
- **Snapshot system** — save, restore, and diff tweak states as JSON files.
- **Topological sort** — respects `depends_on` ordering during batch operations.
- **Rich hover tooltips** with description, current state, default/recommendation
  hints, tags, and registry keys.
- **Summary stats bar** — Applied / Default / Unknown / Recommended / GPO counts.
- **Scope badges** — USER (green) / MACHINE (blue) / BOTH (yellow) per row.
- **Recommendation badges** — teal "REC" tag for tweaks with recommendations.
- **Category metadata** — risk level, scope, and profile badges on section headers.
- **User config** via `~/.regilattice.toml` (force_corp, max_workers, backup
  directory, auto_backup).
- **Test suite** — ~16 400 tests (pytest) covering tweaks, CLI, GUI themes,
  tooltips, widgets, corpguard, elevation, deps, registry, and more.
- **Lint / type-check** — ruff + mypy --strict, zero warnings.
