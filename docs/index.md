# RegiLattice

> **7718 Windows registry tweaks across 158 categories** — privacy, performance,
> gaming, hardening, and more.

[![CI](https://github.com/RajwanYair/RegiLattice/actions/workflows/ci.yml/badge.svg)](https://github.com/RajwanYair/RegiLattice/actions/workflows/ci.yml)
[![Release](https://img.shields.io/github/v/release/RajwanYair/RegiLattice)](https://github.com/RajwanYair/RegiLattice/releases/latest)

## What is RegiLattice?

RegiLattice is an open-source Windows registry tweak toolkit built in C# 13 / .NET 10.
It provides a curated library of 7718 declarative tweaks you can apply, preview, or
revert at any time — safely, with automatic backups before every change.

## Features at a Glance

| Feature | Details |
|---|---|
| **Tweaks** | 7718 across 158 categories in 195 modules |
| **GUI** | WinForms with 11 colour themes, search, filters, profiles |
| **CLI** | 25+ commands — apply, remove, status, batch, profiles, snapshots |
| **Profiles** | 5 built-in profiles: business, gaming, privacy, minimal, server |
| **Safety** | DryRun mode, automatic JSON backups, CorporateGuard |
| **Tests** | 3304 xUnit tests — 0 failures |

## Quick Start

```powershell
# Download the latest release (single-file EXE, no install needed)
# From: https://github.com/RajwanYair/RegiLattice/releases/latest

# Or build from source:
git clone https://github.com/RajwanYair/RegiLattice.git
cd RegiLattice
dotnet build RegiLattice.sln -c Release

# Launch GUI
dotnet run --project src/RegiLattice.GUI

# Run CLI
dotnet run --project src/RegiLattice.CLI -- --list
```

## Documentation

- [Architecture](Architecture.md) — solution structure, data flow, design decisions
- [CLI Reference](CLI-Reference.md) — all 25+ CLI commands
- [API Reference](Api.md) — `TweakEngine`, `RegistrySession`, models
- [Tweak Categories](TweakCategories.md) — all 158 categories
- [Pack Authoring](PackAuthoring.md) — create custom tweak packs
- [Development](Development.md) — build, test, contribute
- [Roadmap](Roadmap.md) — planned features and priorities
- [Changelog](CHANGELOG.md) — release history

## License

MIT — see [LICENSE](https://github.com/RajwanYair/RegiLattice/blob/main/LICENSE).
