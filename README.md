<!-- SEO / GitHub search keywords
     windows registry tweaks windows 11 debloat privacy hardening performance optimizer
     disable telemetry windows optimizer system hardening group policy alternative
     registry editor winforms gui cli dotnet csharp tweak engine
     shutup10 alternative w10privacy alternative O&O ShutUp10 winutil win11debloat
     windows 11 tweaks windows 10 tweaks gaming optimization security hardening
     registry backup corporate IT sysadmin gpo intune-compatible compliance audit
     7505 tweaks 464 categories declarative regop engine dry-run snapshot diff
     RegiLattice windows-optimizer tweak-manager registry-automation open-source
-->

# ⚡ RegiLattice

<p align="center">
  <img src="docs/assets/banner.svg" alt="RegiLattice — Windows Registry Tweaks Toolkit" width="100%"/>
</p>

> **Windows 10 / Windows 11 registry tweaks toolkit** — privacy hardening · performance optimizer · debloater · security hardening · group policy alternative · .NET 10 C# · WinForms GUI · CLI

[![CI](https://github.com/RajwanYair/RegiLattice/actions/workflows/ci.yml/badge.svg)](https://github.com/RajwanYair/RegiLattice/actions/workflows/ci.yml)
![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-13-239120?logo=csharp&logoColor=white)
![Tests](https://img.shields.io/badge/tests-2742%20passing-brightgreen)
![Platform](https://img.shields.io/badge/platform-Windows%20x64-0078D6?logo=windows&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-green)
![Version](https://img.shields.io/badge/version-5.54.0-blue)

A comprehensive **Windows 10 / Windows 11 registry tweak toolkit** and system optimizer — debloater · privacy hardening tool · performance optimizer · security hardening · group policy alternative — with **7505 verified tweaks** across **464 categories**, a **declarative RegOp engine**, a **full CLI** with 25+ commands, an **interactive console menu**, and a **WinForms GUI** with **11 switchable themes**. Built on **.NET 10 (C# 13)** for native performance on Windows 10/11 x64.

## Download & Install

**Pre-built installer (recommended):**

👉 **[Download RegiLattice v5.54.0](https://github.com/RajwanYair/RegiLattice/releases/latest)** (MSI installer + portable EXE) from the [Releases page](https://github.com/RajwanYair/RegiLattice/releases)

The MSI installer:
- Installs **GUI** (`RegiLattice.GUI.exe`) under `Program Files\RegiLattice\GUI\`
- Installs **CLI** (`RegiLattice.exe`) under `Program Files\RegiLattice\CLI\` and adds it to `PATH`
- Creates a **Start Menu** shortcut
- Supports **upgrade** and **uninstall** via Add/Remove Programs
- Requires no separate .NET runtime (self-contained, win-x64)

**Portable executables (no install required):**

Download `RegiLattice.GUI.exe` or `RegiLattice.exe` directly from the [Releases page](https://github.com/RajwanYair/RegiLattice/releases), place them anywhere, and run.

## Highlights

<p align="center">
  <img src="docs/assets/stats.svg" alt="RegiLattice Stats" width="100%"/>
</p>

- **7505 verified tweaks** across 464 categories — each fully reversible with apply + remove
- **Declarative RegOp pattern** — most tweaks defined as data (`ApplyOps`/`RemoveOps`/`DetectOps`), not code
- **3 interfaces** — WinForms GUI, CLI with 25+ commands, interactive console menu
- **WinForms GUI** — 11 switchable themes (Catppuccin Mocha/Latte, Nord, Dracula, Tokyo Night, Gruvbox Dark, Solarized Dark, One Dark Pro, Rosé Pine, Everforest, Cyberpunk), collapsible categories, scope badges (USER/MACHINE/BOTH), live search, checkbox selection, status filters, profile selector
- **5 machine profiles** — business, gaming, privacy, minimal, server
- **Dry-run mode** — preview changes without touching the registry (`--dry-run`)
- **Snapshot & diff** — save/restore tweak state (JSON), compare snapshots (`--snapshot-diff`)
- **Compliance history** — rolling drift log; `--compliance-history` + `--compliance-report auto` CLI flags
- **Validation & stats** — `--validate` checks all TweakDef integrity; `--stats` shows scope/admin/corp breakdown
- **JSON export** — `--export-json` for scripting; `--export-reg` for .REG file generation
- **Composable filters** — `Filter()` engine API supports scope, category, min-build, tags, corp-safe, and free-text query
- **Dependency resolver** — topological ordering; `ApplyBatch()` auto-resolves deps
- **Parallel detection** — `StatusMap(parallel: true)` for fast batch status checks
- **UAC elevation** — automatic admin re-launch
- **Corporate network safety** — blocks tweaks on domain-joined, Azure AD, VPN, and managed machines
- **Automatic backups** — every registry mutation is backed up to JSON before changes
- **Package managers** — built-in Scoop, pip, Chocolatey, WinGet, and PowerShell module manager dialogs
- **2742 tests** across 17+ test files — full engine, model, service, plugin, and GUI coverage (xUnit)
- **Dependency resolution** — `ResolveDependencies()` topological sort; `Dependents()` reverse lookup
- **Validation engine** — `ValidateTweaks()` checks IDs, labels, categories, broken DependsOn, circular deps
- **Plugin system** — JSON Tweak Packs with marketplace, SHA-256 verification
- **Localization** — built-in English + German locale (48 strings)

<p align="center">
  <img src="docs/assets/features.svg" alt="RegiLattice Feature Categories" width="100%"/>
</p>

## Architecture

> Full architecture reference — Mermaid diagrams for data flow, class model, CI pipeline, and more: **[docs/Architecture.md](docs/Architecture.md)**

<p align="center">
  <img src="docs/assets/architecture.svg" alt="RegiLattice Architecture — Core, GUI, CLI, Tweaks, Registry" width="100%"/>
</p>

```mermaid
graph LR
    subgraph Interfaces
        CLI[RegiLattice.CLI<br/>25+ commands]
        GUI[RegiLattice.GUI<br/>11 themes · WinForms]
    end

    subgraph Core["RegiLattice.Core (library)"]
        TE[TweakEngine<br/>Register · Apply · Search · Filter]
        RS[RegistrySession<br/>Read · Write · Backup · DryRun]
        SM[SnapshotManager]
        TV[TweakValidator]
        DR[DependencyResolver]
        CG[CorporateGuard]
        SV[Services]
        PM[Plugins / Packs]
    end

    subgraph Tweaks["464 Categories · 7505 tweaks"]
        T1[Performance]
        T2[Privacy]
        T3[Security]
        T4[Gaming]
        TN[... 460 more]
    end

    CLI --> TE
    GUI --> TE
    TE --> RS
    TE --> SM
    TE --> TV
    TE --> DR
    TE --> CG
    TE --> SV
    TE --> PM
    T1 & T2 & T3 & T4 & TN --> TE
    RS -->|Microsoft.Win32.Registry| WR[(Windows Registry)]
```

## How It Works

<p align="center">
  <img src="docs/assets/how-it-works.svg" alt="How RegiLattice Works — Browse, Preview, Apply, Verify, Revert" width="100%"/>
</p>

## Use Cases

| Who | What RegiLattice solves |
|-----|------------------------|
| **Privacy-conscious users** | Disable telemetry, activity tracking, Cortana, OneDrive sync, diagnostic data, and advertising IDs across 31 privacy categories in one pass |
| **Gamers** | Reduce input lag, disable background services, optimize GPU scheduling, power plan, TCP stack, and HPET across 31 gaming categories |
| **IT admins / sysadmins** | Batch-apply GPO-equivalent registry hardening (SEHOP, LSA-PPL, ASLR, SMB signing, UAC, BitLocker) with full rollback and dry-run mode |
| **Corporate IT / MDM** | CorporateGuard blocks unsafe tweaks on domain-joined, Azure AD, and Intune-managed machines; `--force` override when authorized |
| **Developers** | Declarative `TweakDef` + `RegOp` API, extensible plugin system, xUnit-tested, CLI for scripting and CI pipelines |
| **Power users** | Apply a machine profile (business/gaming/privacy/minimal/server) in a single command; snapshot before/after, diff, restore |

## Theme Gallery

<p align="center">
  <img src="docs/assets/themes-preview.svg" alt="11 Switchable Colour Themes" width="100%"/>
</p>

## Tweak Categories (464)

464 categories spanning privacy, performance, security, accessibility, gaming, networking, browser hardening, developer tools, identity, biometrics, compliance, voice access, app consent, and more. Each tweak is fully reversible with apply/remove/detect operations.

See `--show-categories` for the full list with tweak counts, or use `--stats` for a complete breakdown.

## Requirements

- **Windows 10/11** (build 19041+)
- **.NET 10 Runtime** (or build from source with .NET 10 SDK)
- Administrator privileges for HKLM tweaks (auto-elevates via UAC prompt)

## Quick Start

### Build from Source

```powershell
# Clone and build
git clone https://github.com/RajwanYair/RegiLattice.git
cd RegiLattice
dotnet build RegiLattice.sln -c Release

# Run tests (2742 tests)
dotnet test RegiLattice.sln

# Publish self-contained executables
dotnet publish src/RegiLattice.GUI/RegiLattice.GUI.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish/gui
dotnet publish src/RegiLattice.CLI/RegiLattice.CLI.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish/cli
```

### GUI (Recommended)

```powershell
dotnet run --project src/RegiLattice.GUI
# or run the published self-contained executable:
.\publish\gui\RegiLattice.GUI.exe
# or install via MSI and launch from Start Menu
```

WinForms window with **11 themes** (Catppuccin Mocha default), collapsible categories, scope badges (USER/MACHINE/BOTH), live search bar, checkbox selection (double-click to toggle), status filters, profile selector, and package manager dialogs (Scoop, pip, Chocolatey, WinGet).

### CLI

```powershell
dotnet run --project src/RegiLattice.CLI -- --list
dotnet run --project src/RegiLattice.CLI -- apply disable-telemetry
dotnet run --project src/RegiLattice.CLI -- remove disable-telemetry
dotnet run --project src/RegiLattice.CLI -- status disable-telemetry
dotnet run --project src/RegiLattice.CLI -- --profile gaming
dotnet run --project src/RegiLattice.CLI -- --gui
dotnet run --project src/RegiLattice.CLI -- --menu
dotnet run --project src/RegiLattice.CLI -- --dry-run --list
dotnet run --project src/RegiLattice.CLI -- --snapshot state.json
dotnet run --project src/RegiLattice.CLI -- --restore state.json
dotnet run --project src/RegiLattice.CLI -- --snapshot-diff before.json after.json
dotnet run --project src/RegiLattice.CLI -- --export-json tweaks.json
dotnet run --project src/RegiLattice.CLI -- --export-reg tweaks.reg
dotnet run --project src/RegiLattice.CLI -- --doctor
dotnet run --project src/RegiLattice.CLI -- --hwinfo
```

### Machine Profiles

```powershell
dotnet run --project src/RegiLattice.CLI -- --profile business   # 39 categories — productivity & security
dotnet run --project src/RegiLattice.CLI -- --profile gaming     # 31 categories — GPU & low-latency
dotnet run --project src/RegiLattice.CLI -- --profile privacy    # 31 categories — telemetry & tracking off
dotnet run --project src/RegiLattice.CLI -- --profile minimal    # 22 categories — fast, clean essentials
dotnet run --project src/RegiLattice.CLI -- --profile server     # 28 categories — hardened & headless
```

### PowerShell Launcher

```powershell
.\Launch-RegiLattice.ps1              # launch with defaults
.\Launch-RegiLattice.ps1 --gui        # launch GUI directly
```

## Screenshots

> Place screenshot images in `docs/screenshots/` and reference them here.

| View | Description |
|------|-------------|
| **GUI — Catppuccin Mocha** | Main window with collapsible categories, scope badges, and search bar |
| **GUI — Nord Theme** | Same layout with the Nord colour palette |
| **CLI — --list** | Terminal output with categories, status badges, and descriptions |
| **Snapshot Diff** | Coloured terminal or HTML diff comparing two snapshot files |
| **Profile Selector** | GUI dropdown showing Business / Gaming / Privacy / Minimal / Server profiles |
| **About Dialog** | System info, hardware detection, and version details |

## Corporate Network Safety

Automatically detects corporate environments and **blocks non-safe tweaks** to prevent policy violations:

- **Active Directory** domain membership (P/Invoke `GetComputerNameExW`)
- **Azure AD / Entra ID** join status (`dsregcmd /status`)
- **VPN adapters** — Cisco AnyConnect, GlobalProtect, Zscaler, WireGuard, etc.
- **Group Policy** registry indicators
- **SCCM / Intune** management agents

Override with `--force` (CLI) or the "Force" checkbox (GUI) at your own risk.

## Project Structure

```mermaid
graph TD
    subgraph SRC["📁 src/"]
        CORE["📦 RegiLattice.Core<br/>TweakEngine · SnapshotManager · TweakValidator<br/>DependencyResolver · RegistrySession · CorporateGuard<br/>14 Services · Plugins<br/>461 Modules · 7505 tweaks · 464 categories"]
        GUI["🖥️ RegiLattice.GUI<br/>WinForms · 11 Themes<br/>MainForm · 9 Dialogs · 5 Package Managers"]
        CLI["⌨️ RegiLattice.CLI<br/>25+ CLI Commands · CliArgs · ConsoleColorizer"]
    end

    subgraph TST["🧪 tests/  ·  2742 xUnit tests"]
        CT["Core.Tests<br/>2102 tests · 12 files"]
        CLT["CLI.Tests<br/>301 tests · 1 file"]
        GT["GUI.Tests<br/>339 tests · 3 files"]
    end

    subgraph INFRA["🔧 Infrastructure & Docs"]
        INST["📦 installer/ — WiX · MSI"]
        DOCS["📄 docs/ — Api · Changelog · Security"]
        GH["⚙️ .github/ — CI · Release · Skills"]
    end

    CLI -->|project ref| CORE
    GUI -->|project ref| CORE
    CT -.->|tests| CORE
    CLT -.->|tests| CLI
    GT -.->|tests| GUI
```

## Adding a Custom Tweak

Create a new `.cs` file in `src/RegiLattice.Core/Tweaks/` and register it in `TweakEngine.RegisterBuiltins()`.

**Example — declarative RegOp pattern** (preferred for simple registry tweaks):

```csharp
// src/RegiLattice.Core/Tweaks/MyCategory.cs
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

public static class MyCategory
{
    private const string Key = @"HKEY_CURRENT_USER\Software\MyApp";

    public static List<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "myapp-fancy-mode",
            Label = "Enable Fancy Mode",
            Category = "My App",
            NeedsAdmin = false,
            CorpSafe = true,
            RegistryKeys = [Key],
            Description = "Enables Fancy Mode in MyApp.",
            Tags = ["myapp", "fancy", "ui"],
            ApplyOps = [RegOp.SetDword(Key, "FancyMode", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "FancyMode")],
            DetectOps = [RegOp.CheckDword(Key, "FancyMode", 1)],
        },
    ];
}
```

For complex tweaks that need custom logic, use `ApplyAction`/`RemoveAction`/`DetectAction` delegates instead:

```csharp
new TweakDef
{
    Id = "myapp-complex-tweak",
    Label = "Complex Custom Logic",
    Category = "My App",
    RegistryKeys = [Key],
    ApplyAction = (requireAdmin) => { /* custom apply logic */ },
    RemoveAction = (requireAdmin) => { /* custom remove logic */ },
    DetectAction = () => { /* return true if applied */ return false; },
}
```

See [CONTRIBUTING.md](docs/CONTRIBUTING.md) for the full guide.

## Building the Installer

The MSI installer is built with [WiX Toolset v6](https://wixtoolset.org/). It requires the self-contained publish outputs to exist first:

```powershell
# 1. Publish self-contained executables
dotnet publish src/RegiLattice.GUI/RegiLattice.GUI.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish/gui
dotnet publish src/RegiLattice.CLI/RegiLattice.CLI.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish/cli

# 2. Build the MSI
dotnet build installer/RegiLattice.Installer.wixproj -c Release

# Output: installer/bin/Release/RegiLattice.msi
```

Install WiX toolset (if not already installed):

```powershell
dotnet tool install --global wix
# or update:
dotnet tool update --global wix
```

## License

MIT — see [LICENSE](LICENSE) for details.

---

## Keywords & GitHub Topics

### Repository Description *(GitHub UI → "About" gear → Description)*

Paste this into the repository **Description** field for maximum GitHub search visibility:

> Windows 10/11 registry tweaks toolkit — 7,505 tweaks, debloater, privacy hardening, performance optimizer, security hardening, group policy alternative. WinForms GUI + CLI. .NET 10, C# 13.

### Recommended GitHub Topics

Set the following topics on the repository (GitHub UI → "About" gear → Topics):

`windows-registry` `registry-editor` `windows-tweaks` `windows-optimizer` `windows-11` `windows-10` `debloat` `privacy` `performance-optimization` `system-optimization` `gaming-optimization` `windows-hardening` `security-hardening` `registry-backup` `dotnet` `csharp` `winforms` `cli-tool` `open-source` `tweak-manager` `group-policy` `privacy-tools` `windows-debloat` `registry-automation` `tweak-engine` `gpo-alternative` `windows-11-tweaks` `windows-11-debloat` `windows-11-privacy` `windows-11-gaming` `windows-11-hardening` `disable-telemetry` `windows-decrapifier` `sysadmin-tools` `corporate-it` `compliance-audit` `drift-detection` `snapshot-diff` `dry-run` `declarative-config` `registry-policy` `winget` `chocolatey` `scoop`

### Repository Description *(paste into GitHub → About gear → Description)*

> Windows 10/11 registry tweaks toolkit — 7,505 tweaks, debloater, privacy hardening, performance optimizer, security hardening, group policy alternative, compliance audit. WinForms GUI + CLI. .NET 10, C# 13. Open source.

### Search Keywords

<!-- GitHub indexes this content for code/repository search -->

**Registry & System:**
`windows-registry` · `registry-editor` · `registry-optimizer` · `registry-backup` · `registry-cleaner` · `registry-automation` · `registry-scripting` · `registry-policy` · `group-policy-alternative` · `windows-settings-manager` · `windows-configuration`

**Privacy & Security:**
`disable-telemetry` · `anti-telemetry` · `privacy-tools` · `privacy-tweaks` · `disable-tracking` · `windows-privacy` · `block-microsoft-telemetry` · `disable-cortana` · `disable-windows-search` · `disable-activity-tracking` · `windows-hardening` · `security-hardening` · `uac-management` · `lsa-protection` · `aslr-sehop` · `windows-firewall-rules` · `windows-defender-policy` · `bitlocker-policy` · `corporate-hardening` · `cis-benchmarks`

**Performance & Gaming:**
`windows-performance` · `performance-tweaks` · `gaming-optimization` · `latency-reduction` · `gpu-tweaks` · `cpu-optimization` · `ram-optimization` · `ssd-optimization` · `windows-gaming` · `game-mode` · `disable-animations` · `windows-startup-optimizer` · `power-plan` · `reduce-ram-usage` · `network-optimization` · `tcp-optimization` · `boot-optimization` · `responsiveness` · `input-lag` · `turbo-boost`

**Debloat & Cleanup:**
`debloat` · `debloat-windows` · `remove-bloatware` · `uninstall-apps` · `disable-bloatware` · `windows-cleanup` · `disk-cleanup` · `cortana-removal` · `onedrive-removal` · `telemetry-removal` · `windows-decrapifier` · `trim-windows` · `minimal-windows`

**Windows Versions:**
`windows-11-tweaks` · `windows-10-tweaks` · `win11` · `win10` · `windows-11-optimization` · `windows-11-privacy` · `windows-11-gaming` · `windows-11-debloat` · `windows-11-hardening`

**Developer & IT:**
`dotnet` · `dotnet-10` · `csharp-13` · `winforms` · `windows-forms` · `cli-tool` · `powershell-tool` · `windows-automation` · `sysadmin-tools` · `it-management` · `group-policy` · `gpo` · `intune-compatible` · `corporate-it` · `windows-deployment` · `scripting-windows` · `winget` · `chocolatey` · `scoop`

**Package & Tool Management:**
`package-manager` · `winget-integration` · `chocolatey-integration` · `scoop-integration` · `pip-integration` · `powershell-modules` · `tool-management` · `software-management`

**Compliance & Audit:**
`compliance-audit` · `drift-detection` · `policy-compliance` · `configuration-audit` · `baseline-hardening` · `cis-benchmarks` · `security-baseline` · `windows-compliance` · `it-policy-enforcement` · `change-tracking`

**Similar / Related Tools:**
RegiLattice is a programmatic, fully reversible, and enterprise-safe alternative to tools in the same space as O\&O ShutUp10, W10Privacy, Winaero Tweaker, Chris Titus WinUtil, BloatyNosy, sophia-script, and Windows10Debloater — built on .NET 10 with a declarative engine, CLI, WinForms GUI, xUnit test suite, and corporate network guard.

`shutup10-alternative` · `w10privacy-alternative` · `winaero-alternative` · `chris-titus-winutil-alternative` · `windows-decrapifier` · `debloat-windows-11` · `debloat-windows-10`
