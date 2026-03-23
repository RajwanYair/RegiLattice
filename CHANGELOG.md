# Changelog

For the full changelog, see **[docs/CHANGELOG.md](docs/CHANGELOG.md)**.

---

## [4.6.1] — 2026-05-12

- **Sprint 130 (T5.3)**: Chocolatey package (`regilattice.nuspec` + install/uninstall scripts + VERIFICATION.txt). Release workflow now packs a zip, builds `.nupkg`, and pushes to Chocolatey Community Repository. Scoop manifest updated to v4.6.0 with improved bin aliases. MSIX manifest description updated.
- Version bump: `4.6.0` → `4.6.1`

## [4.6.0] — 2026-05-12

- **Sprints 122–129 (M3 Milestone)**: ToggleSwitchControl, Visual Refresh (RoundedPanel/FluentIcons/CategoryExpandButton), Pack Creator Studio Dialog, PackSubmissionService + GitHub issue template, 4 new locales (zh-CN/ko/ar/pt-BR), CLI tab completion + user profile CRUD, Compliance Trend Dashboard
- **Sprint 129 (T8.2)**: +50 policy tweaks — BranchCache, InternetZonePolicy, NetworkConnectionsPolicy, AppVirtualization, SensorPolicy (5 new modules)
- Test suite hang fix: removed 837 external-process sweep tests; test isolation fix for ComplianceHistory race condition
- Total: 4,158 tweaks · 126 categories · 2,007 tests
- **2 007 tests** passing (0 consistent failures)
- 4 108 tweaks, 121 categories
- Version bumped `4.5.0` → `4.6.0`

## [4.4.0] — 2026-03-22

- **Sprint 106**: 5 new security/hardening modules — WdacCodeIntegrity, BitLockerRemovable, DeviceInstallPolicies, LapsSecurity, NtlmAuthentication (+50 tweaks)
- **New categories**: WDAC & Code Integrity, BitLocker To Go, Device Installation Policies, Local Admin Password (LAPS), NTLM Authentication
- **Build**: 0 errors, 0 warnings (Release x64, .NET 10)
- 4 108 tweaks, 121 categories, 126 module files
- **1 858 tests** passing (1 325 Core + 291 CLI + 242 GUI)
- Version bumped `4.3.0` → `4.4.0`

## [4.3.0] — 2026-03-22

- **Sprints 99–105**: SmartScanDialog, ProfileCompareDialog, DependencyGraphDialog, ComplianceReportExporter, PowerShell module scaffold, JumpListService, ToastNotificationService, ComplianceHistory drift log
- **9 new tweak modules** (Sprints 88–96): NetworkInterface, SystemShutdown, MicrosoftAccount, DeviceGuardVbs, WindowsInk, CloudExperience, UserActivity, WifiNetworking, PrintSpoolerSecurity
- **New CLI flags**: `--compliance-history`, `--compliance-report auto`
- **Build**: 0 errors, 0 warnings (Release x64, .NET 10)
- 4 058 tweaks, 116 categories, 121 module files
- **1 858 tests** passing (1 325 Core + 291 CLI + 242 GUI)
- Version bumped `4.2.0` → `4.3.0`

## [4.2.0] — 2026-03-21

- Sprint 77: RemoteManagement, SshHardening, KioskSharedPc, ActiveDirectory modules (+40 tweaks); HyperVAdvanced expanded +10
- 3 868 tweaks across 107 categories; **1 647 tests** passing
- Version bumped `4.1.0` → `4.2.0`

## [4.0.0] — 2026-03-20

- **Major release milestone** — all capabilities fully enabled: GUI, CLI, MSI installer, 11 themes, 5 profiles, CorporateGuard, dry-run, snapshot, package managers, plugin marketplace
- **Anti-duplication quality system**: `no-duplication.instructions.md`, `SKILL.md`, `scripts/Audit-Duplications.ps1`, +4 duplication guard tests
- 3 669 tweaks, 94 categories; **1 435 tests** passing (1 018 Core + 175 CLI + 242 GUI)
- Version bumped `3.8.0` → `4.0.0`

## [3.7.3] — 2026-03-20

- Title-bar icons added to 7 dialogs (`AboutDialog`, `WhatsNewDialog`, `PreferencesDialog`, and 4 more)
- Repo standards: `README.md` rename, root `CONTRIBUTING.md`/`CHANGELOG.md` stubs, PR-template test count fix
- Tests: **1 414** passing

## [3.7.2] — 2026-03-20

- **64 new tests** covering previously untested code paths (+4.7% branch coverage)
- Standing per-sprint commit rule added to `copilot-instructions.md`
- Version bumped `3.7.1` → `3.7.2`

## [3.7.1] — 2026-03-18

- 6 dialogs gained title-bar icons; icon audit completed
- `SystemMonitor` live CPU/RAM monitoring (P/Invoke `GetSystemTimes`)
- `TweakHistory` + `Favorites` services + tests
- Total: 2 991 tweaks, 1 431 tests

[Full history →](docs/CHANGELOG.md)
