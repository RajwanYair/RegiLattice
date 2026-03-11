# Changelog

All notable changes to RegiLattice are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
This project adheres to [Semantic Versioning](https://semver.org/).

## [Unreleased]

## [3.0.0] — 2025-07-20

### ⚠️ BREAKING: Quality audit — removed 468 non-functional tweak stubs

v3.0.0 is the first verified-clean release of the C# codebase. Every remaining tweak
has functional apply/remove/detect operations. Non-functional metadata-only stubs
that silently returned "Applied" without performing any action have been removed.

### Removed

- **468 non-functional tweak stubs** across 66 modules — these had metadata (Id, Label,
  Category, Tags) but no ApplyOps, RemoveOps, DetectOps, or Action delegates. The engine
  silently returned `TweakResult.Applied` for these without performing any registry changes.
- Tweak count reduced from ~1 828 to **1 360 verified functional tweaks**

### Added

- **TweakKind enum** — `Registry`, `Command`, `FileConfig` — classifies how each tweak operates
- **CategoryIcon enum** — 24 icon categories (Shield, Globe, Monitor, Gear, Lock, etc.)
- **CategoryIcons helper class** — maps all 72 category names to icons with Unicode symbols
  for CLI display
- **TweakDef.HasOperations** — computed property that returns true only if a tweak has
  ApplyOps or ApplyAction defined
- **TweakDef.Kind** — computed property returning the TweakKind classification
- **TweakEngine no-op guard** — `Register()` now silently skips tweaks where `HasOperations`
  is false, preventing non-functional tweaks from entering the engine

### Fixed

- **CS0067 warning** — `RegistrySession.LogWritten` event was declared but never invoked;
  now fires in `WriteLog()` method
- **Build warnings** — Release build now produces 0 errors, 0 warnings

## [2.0.0] — 2025-07-20

### ⚠️ BREAKING: Complete rewrite from Python to C#/.NET

The entire project has been rewritten from Python (tkinter, argparse, winreg) to
C#/.NET 10 (WinForms, Microsoft.Win32.Registry). This is a clean-break major version.

### Architecture

- **RegiLattice.Core** — class library with TweakEngine, TweakDef model, RegOp declarative
  pattern, RegistrySession, CorporateGuard, ProfileDefinitions, and 7 service classes
- **RegiLattice.GUI** — WinForms application with 4 switchable themes, package manager
  dialogs, collapsible categories, and scope badges
- **RegiLattice.CLI** — console application with 25+ commands (apply, remove, status,
  list, search, profile, snapshot, diff, doctor, hwinfo, export, import, etc.)
- **71 tweak category modules** — each exports `static List<TweakDef> Tweaks`
- **Declarative RegOp pattern** — ~95% of tweaks defined as data (ApplyOps/RemoveOps/DetectOps)
  instead of imperative code; remaining ~5% use Action/Func delegates

### Added

- **~1 828 tweaks** across 72 categories (migrated from Python with all registry logic preserved)
- **WinForms GUI** replacing tkinter — native Windows look, double-buffered rendering,
  4 themes (Catppuccin Mocha/Latte, Nord, Dracula) with runtime switching and persistence
- **TweakDef.RegOp** — 12 factory methods (SetDword, SetString, SetExpandString, SetQword,
  SetBinary, SetMultiSz, DeleteValue, DeleteTree, CheckDword, CheckString, CheckMissing, CheckKeyMissing)
- **TweakScope enum** — User, Machine, Both (auto-computed from registry key paths)
- **TweakResult enum** — Applied, NotApplied, Unknown, Error, SkippedCorp, SkippedBuild
- **TweakEngine** — comprehensive API: Register, AllTweaks, GetTweak, Categories, Search,
  Filter, StatusMap (parallel), Apply/Remove/ApplyBatch/RemoveBatch, Profiles,
  SaveSnapshot/LoadSnapshot/RestoreSnapshot, CategoryCounts, ScopeCounts, ExportJson
- **RegistrySession** — full registry wrapper with SetDword/SetString/SetExpandString/
  SetQword/SetBinary/SetMultiSz, DeleteValue/DeleteTree, ReadDword/ReadString/etc.,
  KeyExists/ValueExists, ListSubKeys/ListValueNames, Execute(RegOps), Evaluate(DetectOps),
  Backup/Restore, DryRun mode, structured logging
- **5 profiles** — business (39 cats), gaming (31 cats), privacy (31 cats), minimal (22 cats),
  server (28 cats)
- **CorporateGuard** — domain membership, Azure AD detection, GPO checks, SCCM/Intune detection
  with caching and detailed status reporting
- **HardwareInfo** — CPU/GPU/RAM/disk detection, profile suggestion, hardware summary
- **Package manager dialogs** — Scoop, pip, PowerShell module managers in GUI
- **About dialog** — system info, hardware detection, shortcut reference
- **129 xUnit tests** (93 Core + 36 GUI) — TweakDef, TweakEngine, RegistrySession,
  Services, Theme, PackageManager validation
- **winget manifests** — installer package for Windows Package Manager

### Changed

- **Language**: Python 3.10+ → C# 13 / .NET 10
- **GUI framework**: tkinter → Windows Forms
- **Registry access**: winreg (Python) → Microsoft.Win32.Registry (C#)
- **Test framework**: pytest → xUnit 2.9.2
- **Build system**: hatchling/pip → dotnet build/MSBuild
- **Tweak pattern**: Function triplet (_apply/_remove/_detect) → Declarative RegOp lists
- **Plugin loading**: Dynamic file discovery → Static RegisterBuiltins() method

### Removed

- Python codebase (archived in `archive/python/`)
- pyproject.toml, requirements.txt, ruff/mypy/pytest configuration
- tkinter GUI, argparse CLI, interactive console menu (Python versions)
- All Python-specific dependencies (hypothesis, pytest-mock, pytest-xdist, etc.)

---

## [1.0.2] — 2025-07-19 (Python — archived)

Final Python release before C# migration. See `archive/python/` for full history.
