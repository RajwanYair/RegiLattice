# RegiLattice CLI — Command Reference

> Complete reference for all RegiLattice CLI commands, subcommands, and flags.
> Version: v6.24.0 · 40+ commands · Windows 10/11 x64

---

## Quick Start

```powershell
# After MSI install (CLI on PATH):
RegiLatticeCLI.exe --help
RegiLatticeCLI.exe --list

# Portable EXE:
.\RegiLatticeCLI-v6.24.0-win-x64.exe --list

# From source:
dotnet run --project src/RegiLattice.CLI -- --list
```

The CLI supports two equivalent syntaxes — **flag style** and **subcommand style**:

```powershell
# Flag style
RegiLatticeCLI.exe apply priv-disable-telemetry

# Subcommand style (more readable for scripting)
RegiLatticeCLI.exe tweak apply priv-disable-telemetry
```

---

## Global Flags

These flags can be combined with any command:

| Flag | Short | Description |
|------|-------|-------------|
| `--dry-run` | | Preview mode — all operations logged, no registry writes |
| `--force` | | Override CorporateGuard (domain/Azure AD/Intune detection) |
| `--config <path>` | | Custom config file (`%LOCALAPPDATA%\RegiLattice\config.json` by default) |
| `--no-color` | | Disable ANSI colour output (for plain terminals or piped output) |
| `--assume-yes` | `-y` | Skip all confirmation prompts |
| `--portable` | | Store all data in `.\data\` relative to the exe (portable mode) |
| `--silent` | | No console output; communicate only via exit code |
| `--log-file <path>` | | Write JSON result log (most useful with `--silent`) |
| `--help` | `-h` | Show help for the current command |
| `--version` | | Show version information |

---

## Exit Codes

| Code | Meaning |
|------|---------|
| `0` | All operations succeeded (or no-op) |
| `1` | One or more operations failed |
| `2` | Bad arguments, or tweak/profile not found |
| `3` | Administrator privileges required |
| `4` | CorporateGuard blocked the operation (use `--force` to override) |

---

## Tweak Operations

### `apply <id|all>` · `tweak apply <id|all>`

Apply a single tweak by its ID. Writes registry values, configures services, or runs the tweak action. Use `all` to apply every registered tweak.

```powershell
RegiLatticeCLI.exe apply priv-disable-telemetry
RegiLatticeCLI.exe apply gpu-hardware-accelerated-scheduling
RegiLatticeCLI.exe apply perf-disable-animations --dry-run
RegiLatticeCLI.exe apply all --dry-run    # preview applying everything
```

Returns: `Applied`, `Error`, `SkippedCorp`, `SkippedBuild`, or `SkippedHw`.

---

### `remove <id|all>` · `tweak remove <id|all>`

Remove/revert a single tweak — restores the registry to its pre-tweak state. Use `all` to remove every applied tweak.

```powershell
RegiLatticeCLI.exe remove priv-disable-telemetry
RegiLatticeCLI.exe remove all --dry-run
```

---

### `update <id>` · `tweak update <id>`

Update a tweak. For package manager tweaks, runs `UpdateAction`; for others, re-applies the latest version.

```powershell
RegiLatticeCLI.exe update pkg-scoop-install
```

---

### `status <id>` · `tweak status <id>`

Check the current detected status of a tweak (reads from registry/system state).

```powershell
RegiLatticeCLI.exe status priv-disable-telemetry
# Output: [Applied] priv-disable-telemetry — AllowTelemetry = 0
```

Return values: `Applied`, `NotApplied`, `Unknown`, `Error`, `SkippedCorp`, `SkippedBuild`, `SkippedHw`.

---

## Discovery & Search

### `--list` · `tweak list` · `list`

List all registered tweaks with category, scope badge (USER / MACHINE / BOTH), and current status.

```powershell
RegiLatticeCLI.exe --list
RegiLatticeCLI.exe --list --no-color    # pipe-friendly
```

---

### `--search <query>` · `search <query>`

Full-text search across tweak IDs, labels, descriptions, and tags.

```powershell
RegiLatticeCLI.exe --search telemetry
RegiLatticeCLI.exe --search "gaming performance"
RegiLatticeCLI.exe search bluetooth --no-color
```

---

### `--categories`

List all tweak categories with their tweak counts.

```powershell
RegiLatticeCLI.exe --categories
# Output: 122 categories
# Accessibility (63), AI / Copilot (27), Audio (37), ...
```

---

### `--tags`

List all tags used across all registered tweaks.

```powershell
RegiLatticeCLI.exe --tags
```

---

### Filter Flags

Narrow the list or search results with additional criteria:

| Flag | Description |
|------|-------------|
| `--scope <user\|machine\|both>` | Filter by registry scope |
| `--min-build <N>` | Only tweaks requiring Windows build ≥ N |
| `--corp-safe` | Show only corporate-safe tweaks (`CorpSafe = true`) |
| `--needs-admin` | Show only tweaks requiring admin elevation |
| `--filter-status <status>` | Filter by current status (`Applied`, `NotApplied`, `Unknown`) |

```powershell
RegiLatticeCLI.exe --list --scope machine --corp-safe
RegiLatticeCLI.exe --list --filter-status Applied --no-color
RegiLatticeCLI.exe search telemetry --scope user
```

---

## Profiles

### `--profile <name>` · `profile apply <name>`

Apply a named machine profile — applies all tweaks in the profile's categories.

Available profiles: `business`, `gaming`, `privacy`, `minimal`, `server`

```powershell
RegiLatticeCLI.exe --profile gaming
RegiLatticeCLI.exe --profile privacy --dry-run   # preview what will change
RegiLatticeCLI.exe profile apply server --force  # apply on a corporate machine
```

| Profile | Categories | Focus |
|---------|-----------|-------|
| `business` | 39 | Productivity, security, cloud & workflow |
| `gaming` | 31 | GPU, input latency, performance, low-distraction |
| `privacy` | 31 | Telemetry, tracking, cloud data, browser hardening |
| `minimal` | 22 | Fast, clean essentials only |
| `server` | 28 | Hardened, headless, uptime & remote management |

---

### `--list-profiles` · `profile list`

Show all available profiles (built-in and user-created) with descriptions.

```powershell
RegiLatticeCLI.exe --list-profiles
RegiLatticeCLI.exe --list-user-profiles    # only custom profiles
```

---

### `--diff <profile>`

Show which tweaks differ between the current registry state and a named profile.

```powershell
RegiLatticeCLI.exe --diff gaming
```

---

### Profile Management (Custom Profiles)

```powershell
# Create
RegiLatticeCLI.exe --profile-create my-workstation
RegiLatticeCLI.exe profile create my-workstation

# Populate with tweak IDs
RegiLatticeCLI.exe --profile-tweaks "priv-disable-telemetry,perf-disable-animations"

# Set a description
RegiLatticeCLI.exe --profile-desc "My personal workstation hardening"

# Clone an existing profile to customise
RegiLatticeCLI.exe --profile-clone privacy my-privacy-extended

# Rename
RegiLatticeCLI.exe --profile-rename my-workstation workstation-v2

# Delete
RegiLatticeCLI.exe --profile-delete workstation-v2
RegiLatticeCLI.exe profile delete workstation-v2
```

---

## Batch Operations by Category

### `--category <name> apply|remove`

Apply or remove all tweaks in a category.

```powershell
RegiLatticeCLI.exe --category Privacy apply
RegiLatticeCLI.exe --category "Network Optimization" apply --dry-run
RegiLatticeCLI.exe --category Gaming remove
```

---

### `batch apply <file>` · `batch remove <file>`

Apply or remove a set of tweaks listed in a file.

Supported file formats:
- **Plain text** — one tweak ID per line; lines starting with `#` are comments
- **JSON array** — `["id1", "id2", ...]`
- **Snapshot JSON** — any file with `"applied"`, `"tweaks"`, or `"ids"` key

```powershell
# Plain text file
@"
# Privacy tweaks
priv-disable-telemetry
priv-disable-activity-history
priv-disable-advertising-id
"@ | Set-Content tweaks.txt

RegiLatticeCLI.exe batch apply tweaks.txt
RegiLatticeCLI.exe batch apply tweaks.txt --dry-run
RegiLatticeCLI.exe batch remove tweaks.txt
```

---

## Snapshots & Diff

### `--snapshot <file>` · `snapshot save <path>`

Save the current detected state (Applied/NotApplied) of all tweaks to a JSON file.

```powershell
RegiLatticeCLI.exe --snapshot before.json
# ... make changes ...
RegiLatticeCLI.exe snapshot save after.json
```

---

### `--restore <file>` · `snapshot restore <path>`

Restore tweak states from a previously saved snapshot file.

```powershell
RegiLatticeCLI.exe --restore before.json
RegiLatticeCLI.exe --restore before.json --dry-run   # preview the restore
```

---

### `--snapshot-diff <file-a> <file-b>`

Compare two snapshot files and display what changed. Colour-coded output (green = applied, red = removed) in ANSI-capable terminals.

```powershell
RegiLatticeCLI.exe --snapshot-diff before.json after.json
RegiLatticeCLI.exe --snapshot-diff before.json after.json --no-color > diff.txt
```

---

### `--html <path>`

Generate an HTML diff report from two snapshot files (use after `--snapshot-diff`).

```powershell
RegiLatticeCLI.exe --snapshot-diff before.json after.json --html diff-report.html
```

---

## Export

### `export json <path>` · `--export-json <path>`

Export all registered tweaks (metadata only, not current status) as structured JSON.

```powershell
RegiLatticeCLI.exe --export-json all-tweaks.json
RegiLatticeCLI.exe export json all-tweaks.json
```

---

### `export reg <path>` · `--export-reg <path>`

Export currently-applied tweaks as a `.REG` file, importable via `regedit.exe /s`.

```powershell
RegiLatticeCLI.exe --export-reg my-settings.reg
```

---

### `export gpo <path>` · `--export-gpo <path>`

Export all HKLM machine tweaks as an ADMX-compatible Group Policy template.

```powershell
RegiLatticeCLI.exe export gpo "C:\GPO\regilattice-policies.admx"
```

---

### `export intune <path>` · `--export-intune <path>`

Export HKLM tweaks as an Intune OMA-URI JSON payload for MDM deployment.

```powershell
RegiLatticeCLI.exe export intune "C:\MDM\intune-policies.json"
```

---

### `export config <path>` · `--export-config <path>`

Export the current applied tweak selections as a shareable config file.

```powershell
RegiLatticeCLI.exe export config my-config.json
```

---

### `--html-report <path>`

Generate a full HTML report of tweak statuses.

```powershell
RegiLatticeCLI.exe --html-report report.html
```

---

## Import

### `import json <path>` · `--import-json <path>`

Import tweak IDs from a JSON file (combine with `apply` to apply them).

```powershell
RegiLatticeCLI.exe --import-json selected-tweaks.json
```

---

### `import config <path>` · `--import-config <path>`

Import and apply tweaks from a previously exported config file.

```powershell
RegiLatticeCLI.exe import config my-config.json
RegiLatticeCLI.exe import config my-config.json --dry-run
```

---

## Plugin Marketplace

Browse and install community-contributed JSON Tweak Packs.

```powershell
# Browse available packs
RegiLatticeCLI.exe marketplace list
RegiLatticeCLI.exe marketplace search gaming

# Install / uninstall
RegiLatticeCLI.exe marketplace install extra-privacy-pack
RegiLatticeCLI.exe marketplace uninstall extra-privacy-pack

# Manage installed packs
RegiLatticeCLI.exe marketplace installed
RegiLatticeCLI.exe marketplace update extra-privacy-pack
RegiLatticeCLI.exe marketplace updates     # list all packs with available updates
RegiLatticeCLI.exe marketplace info extra-privacy-pack
```

---

## Favorites & History

### Favorites

Mark tweaks you use frequently for quick access:

```powershell
RegiLatticeCLI.exe --favorites              # list all favorited tweaks
RegiLatticeCLI.exe --favorite-add priv-disable-telemetry
RegiLatticeCLI.exe --favorite-remove priv-disable-telemetry
```

---

### Operation History

```powershell
RegiLatticeCLI.exe --history         # last 20 operations
RegiLatticeCLI.exe --history 50      # last 50 operations
```

---

## Compliance & Drift Tracking

### `--compliance <snapshot>`

Run a compliance check — compare the current registry state against a snapshot and report any drift.

```powershell
RegiLatticeCLI.exe --compliance baseline.json
```

---

### `--compliance-report <mode>`

Generate a compliance report using a profile as the baseline. Mode `auto` uses the currently active profile.

```powershell
RegiLatticeCLI.exe --compliance-report auto
```

---

### `--compliance-history`

Show the rolling compliance drift log — timestamps when tweaks were applied, reverted, or changed.

```powershell
RegiLatticeCLI.exe --compliance-history
```

---

## Validation & Diagnostics

### `--validate` · `validate`

Run TweakDef integrity validation across all 7,429 registered tweaks. Checks for:
- Duplicate tweak IDs
- Empty or missing labels/categories
- Broken `DependsOn` references
- Circular dependency chains
- `ImpactScore`/`SafetyRating` out of the valid 1–5 range

```powershell
RegiLatticeCLI.exe --validate
# Output: ✓ 7429 tweaks validated — 0 errors
```

---

### `--stats` · `stats`

Print statistics: tweaks by scope (User/Machine/Both), admin requirement breakdown, corp-safe counts, category distribution, and profile coverage.

```powershell
RegiLatticeCLI.exe --stats
```

---

### `--check` · `check`

Audit mode — show every tweak's current status (Applied / NotApplied / Unknown / Skipped).

```powershell
RegiLatticeCLI.exe --check
RegiLatticeCLI.exe --check --filter-status Applied --no-color
```

---

### `--doctor` · `doctor`

System health check. Verifies:
- Administrator privilege status
- Windows build version
- CorporateGuard status (domain / Azure AD / Intune detection)
- Registry read/write access
- .NET runtime version

```powershell
RegiLatticeCLI.exe --doctor
```

---

### `--hwinfo`

Display hardware information: CPU, RAM, GPU, Windows edition, build number, and a recommended profile.

```powershell
RegiLatticeCLI.exe --hwinfo
# Output: Intel Core i7-1365U · 32 GB RAM · Intel UHD Graphics
#         Windows 11 Pro · Build 26100
#         Recommended profile: business
```

---

### `--report`

Full diagnostic report: hardware info, health check, category counts, all tweak statuses.

```powershell
RegiLatticeCLI.exe --report
RegiLatticeCLI.exe --report --no-color > report.txt
```

---

## Dependency Inspection

### `--depends-on <id>`

Show the full dependency chain for a tweak in topological order.

```powershell
RegiLatticeCLI.exe --depends-on sec-enable-secure-boot
# Output: sec-enable-uefi → sec-disable-legacy-boot → sec-enable-secure-boot
```

---

## Interactive Modes

### `--gui`

Launch the WinForms GUI from the CLI.

```powershell
RegiLatticeCLI.exe --gui
```

---

### `--menu`

Launch the interactive console menu — browse categories, select tweaks, and apply/remove without memorizing IDs.

```powershell
RegiLatticeCLI.exe --menu
```

---

## Scripting & Automation

### Piping & Silent Mode

```powershell
# Apply a profile silently — exit code tells you if it worked
RegiLatticeCLI.exe --profile privacy --silent
if ($LASTEXITCODE -ne 0) { throw "Privacy profile failed (exit $LASTEXITCODE)" }

# Write a JSON result log without console noise
RegiLatticeCLI.exe --profile privacy --silent --log-file C:\logs\regilattice.json

# Skip confirmation prompts in automated flows
RegiLatticeCLI.exe --category Security apply -y
```

---

### Environment Variables

| Variable | Equivalent flag |
|----------|-----------------|
| `REGILATTICE_FORCE=1` | `--force` |
| `REGILATTICE_DRY_RUN=1` | `--dry-run` |
| `REGILATTICE_NO_COLOR=1` | `--no-color` |
| `REGILATTICE_CONFIG=<path>` | `--config <path>` |

---

### Intune / MDM Deployment

```powershell
# Generate Intune OMA-URI JSON for machine-scope tweaks
RegiLatticeCLI.exe export intune C:\MDM\policies.json

# Validate the output silently in CI
RegiLatticeCLI.exe --validate --silent
if ($LASTEXITCODE -ne 0) { exit 1 }
```

---

### Common One-Liners

```powershell
# Preview applying the gaming profile
RegiLatticeCLI.exe --profile gaming --dry-run

# Apply all privacy tweaks, skip confirmations
RegiLatticeCLI.exe --category Privacy apply -y

# Snapshot → apply minimal profile → diff
RegiLatticeCLI.exe --snapshot before.json
RegiLatticeCLI.exe --profile minimal -y
RegiLatticeCLI.exe --snapshot after.json
RegiLatticeCLI.exe --snapshot-diff before.json after.json

# Export everything for auditing
RegiLatticeCLI.exe --export-json tweaks.json --no-color
RegiLatticeCLI.exe --html-report report.html

# Apply a batch file (plain text)
RegiLatticeCLI.exe batch apply my-tweaks.txt --dry-run
RegiLatticeCLI.exe batch apply my-tweaks.txt -y

# Validate all tweak definitions, show compliance, check hardware
RegiLatticeCLI.exe --validate
RegiLatticeCLI.exe --compliance-report auto
RegiLatticeCLI.exe --hwinfo
```

---

## See Also

- [Architecture](Architecture.md) — engine design, data flow diagrams
- [API Reference](Api.md) — C# public API (`TweakEngine`, `RegOp`, `RegistrySession`)
- [Troubleshooting](Troubleshooting.md) — common errors and fixes
- [Development](Development.md) — build from source, add tweaks, testing
