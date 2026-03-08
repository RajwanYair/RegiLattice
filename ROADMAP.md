# RegiLattice — Roadmap

> Living document — updated after every sprint.
> Last updated: 2026-03-08 · v1.0.0 · 1 292 tweaks · 69 categories · 17 511 tests

---

## Current State (as of 2026-03-08)

| Metric | Value |
|--------|-------|
| Tweaks | 1 292 across 69 categories |
| Tests | 17 511 (all passing) |
| Python | 3.10 – 3.14 |
| Lint | ruff (E, F, W, I, UP, B, SIM, RUF) |
| Type check | mypy --strict |
| Coverage | ~high (target ≥ 90 %) |
| GUI | tkinter + 4 themes |
| Profiles | 5 (business, gaming, privacy, minimal, server) |
| Platforms | Windows (primary); Linux/WSL (partial) |

---

## Long-Term Vision

Make RegiLattice the **reference Windows registry tweak toolkit**:

- Zero-dependency runtime (stdlib-only at runtime)
- Production-grade packaging distributed via pip, winget, and scoop
- GUI that rivals dedicated Windows tools
- Full corporate-environment safety
- Extensible plugin marketplace
- CI/CD on every commit, cross-platform where possible

---

## Sprints

### Sprint 1 — Foundation & Hygiene ✅ (2026-03-08)

| Theme | Goal |
|-------|------|
| Git hygiene | Fix lint, commit Sprint 8 pending changes |
| Config cleanup | `.gitattributes`, formatter (ruff not black), remove `.flake8` |
| CI | Consolidate duplicate workflows |
| Docs | ROADMAP.md, move PROJECT_SPEC_PROMPT, update stale architecture stats |

### Sprint 2 — Documentation Deep-Dive (planned)

| # | Task |
|---|------|
| 1 | Refresh stale stats in `architecture.md` (now 1 292/69) |
| 2 | Update `README.md` badge for 17 511 tests |
| 3 | Move Sprint 8 in `CHANGELOG.md` from `[Unreleased]` to `[1.0.1]` |
| 4 | Add `docs/DEVELOPMENT.md` — local setup, Windows + WSL guide |
| 5 | Review and update all issue templates in `.github/issue_template/` |
| 6 | Add PR template checklist (tests, lint, docs) |
| 7 | Refresh `CONTRIBUTING.md` with current workflow |

### Sprint 3 — Test Quality & Coverage (planned)

| # | Task |
|---|------|
| 1 | Measure current coverage per-module; identify gaps |
| 2 | Add tests for `gui.py` (widget lifecycle, search, filter) |
| 3 | Add tests for `menu.py` edge cases |
| 4 | Add cross-platform stubs (Linux path handling) |
| 5 | Add property-based tests for registry path splitting |
| 6 | Add mutation-style tests for tweak apply/remove idempotency |
| 7 | Push overall coverage ≥ 90 % on critical paths |

### Sprint 4 — Refactoring & Performance (planned)

| # | Task |
|---|------|
| 1 | Dead-code audit — remove unused functions/imports |
| 2 | Refactor `tweaks/__init__.py` — split by concern (loader, profiles, engine) |
| 3 | Profile startup time; identify and defer heavy imports |
| 4 | Move to `src/` layout for better packaging isolation |
| 5 | Add `__all__` to all public modules |
| 6 | Harden `marketplace.py` — plugin signature check |
| 7 | Audit all `subprocess` calls for command-injection safety |

### Sprint 5 — Production Readiness & Release (planned)

| # | Task |
|---|------|
| 1 | Validate PyPI packaging (`hatch build`, `twine check`) |
| 2 | WSL-compatibility pass (ensure all cross-platform tests pass) |
| 3 | Verify pre-commit hooks work end-to-end |
| 4 | Add GitHub Actions for coverage upload to Codecov/Coveralls |
| 5 | Add `SECURITY.md` with responsible disclosure policy |
| 6 | Tag and release v1.0.1 with all Sprint 1–4 improvements |
| 7 | winget manifest update for v1.0.1 |

---

## Prioritized Backlog

_~50 concrete tasks derived from the roadmap above, ordered by priority._

### P0 — Critical / Immediate

- [x] Auto-fix ruff I001 import-sort issues
- [x] Commit Sprint 8 pending changes
- [x] Fix `.gitattributes` header + add `*.py text eol=lf`
- [x] Fix `.vscode/settings.json` — use ruff-format not black-formatter
- [x] Remove redundant `.flake8`
- [x] Add `[project.urls]` to `pyproject.toml`
- [x] Move `PROJECT_SPEC_PROMPT.md` → `.github/docs/project-spec.md`
- [ ] Consolidate `ci.yml` + `python.yml` into one canonical workflow
- [ ] Update stale stats in `architecture.md` (1 228→1 292, 64→69)
- [ ] Run mypy and fix any strict errors

### P1 — High Value

- [ ] Update `README.md` test-count badge (17 511)
- [ ] Move `CHANGELOG.md [Unreleased]` → `[1.0.1]`
- [ ] Add `docs/DEVELOPMENT.md` (setup, run, test, contribute)
- [ ] Add `__all__` to `registry.py`, `tweaks/__init__.py`, `config.py`
- [ ] Review `pyrightconfig.json` — ensure it matches `pyproject.toml [tool.pyright]`
- [ ] Add `coverage.xml` to `.gitattributes` (binary check — it's xml/text)
- [ ] Measure per-module coverage; document gaps
- [ ] Add `hypothesis` + `pytest-mock` to `[dev]` dependencies

### P2 — Medium Value

- [ ] Add `docs/DEVELOPMENT.md` (Windows + WSL setup guide)
- [ ] Refresh `CONTRIBUTING.md` — current PR/issue workflow
- [ ] Review all `.github/issue_template/` files for accuracy
- [ ] Add `Makefile` or `just` runner for common dev commands
- [ ] Add `CODEOWNERS` rationale comment
- [ ] Audit `gui.py` for thread-safety (tkinter + Background threads)
- [ ] Add `typing.TYPE_CHECKING` guards for expensive imports
- [ ] Profile `all_tweaks()` load time; add benchmark
- [ ] Add `TweakDef.risk_level` field to complement `tweak_risk_level()`
- [ ] Consolidate duplicate `_SCOPE_CACHE` logic into helper

### P3 — Nice to Have

- [ ] `src/` layout migration
- [ ] Pre-commit CI stage (lint + smoke tests on PR)
- [ ] Add Codecov/Coveralls integration
- [ ] Dark mode auto-detection for GUI (follows Windows system theme)
- [ ] CLI completions (bash/zsh/PowerShell via argcomplete)
- [ ] `regilattice export --format json/ps1/reg` unified export
- [ ] Plugin sandbox — limit filesystem/network access for third-party plugins
- [ ] Scoop bucket publish for RegiLattice
- [ ] winget manifest automated update via GitHub Actions
- [ ] Add `logging` module integration (replace ad-hoc SESSION.log)
- [ ] Localization: add 2nd locale (German or Spanish as proof-of-concept)
- [ ] REST API layer (FastAPI) for remote management
- [ ] Web dashboard (read-only) for tweak status visualization
- [ ] Benchmark suite in CI (track performance regressions)
- [ ] Add tweak `changelog` field (last modified date)
- [ ] Add tweak `source_url` field (docs/KB article reference)
- [ ] Auto-generate per-category Markdown docs from `TweakDef` fields
- [ ] Package for Chocolatey
- [ ] `regilattice diff` — compare two snapshots in human-readable format
- [ ] `regilattice doctor` — sanity-check for common misconfigurations
- [ ] Support for custom user-defined registry tweaks (TOML-based)

---

## Sprint Velocity Tracking

| Sprint | Dates | Tasks Planned | Tasks Done | Notes |
|--------|-------|--------------|-----------|-------|
| Sprint 1 | 2026-03-08 | 10 | 8 | Foundation & hygiene |
| Sprint 2 | TBD | 7 | — | Documentation |
| Sprint 3 | TBD | 7 | — | Test coverage |
| Sprint 4 | TBD | 7 | — | Refactoring |
| Sprint 5 | TBD | 7 | — | Production readiness |
