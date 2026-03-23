# Changelog

For the full changelog, see **[docs/CHANGELOG.md](docs/CHANGELOG.md)**.

---

## [4.7.0] — 2026-03-23 (M4 Milestone)

- **M4 milestone release** (Sprints 130–139): 420 new tweaks, plugin signing & sandbox, Chocolatey distribution, virtual registry tests, Stryker.NET mutation testing infrastructure.
- **4 528 tweaks** across **168 categories** (163 module files).
- **2 660 tests** passing — 2 052 Core + 301 CLI + 307 GUI (0 failures).
- Fix: `Ratings.Save()` cross-process retry to eliminate parallel-test `IOException` races.

---

## [4.6.9] — 2026-05-15

- **Sprint 139 (T8.3)**: 50 new tweaks across 5 new modules — `ShellRestrictionsPolicy.cs` (shellrst, 10), `BitsTransferPolicy.cs` (bitspol, 10), `OfflineFilesSyncPolicy.cs` (offsync, 10), `MsiInstallerPolicy.cs` (msipol, 10), `SmbServerPolicy.cs` (smbsrv, 10).
- New categories: Shell Restrictions Policy, BITS Transfer Policy, Offline Files Sync Policy, MSI Installer Policy, SMB Server Policy.

---

## [4.6.8] — 2026-05-15

- **Sprint 138 (T8.2)**: 50 new tweaks across 5 new modules — `SmartScreenPolicy.cs` (smartscr, 10), `CredentialCachingPolicy.cs` (credcache, 10), `WindowsTimeGpoPolicy.cs` (timepol, 10), `FirewallLogPolicy.cs` (fwlog, 10), `LogonGpoPolicy.cs` (logonpol, 10).
- New categories: SmartScreen Policy, Credential Caching Policy, Time Service Policy, Firewall Log Policy, Logon Policy.
- Tweaks: **4,308** | Categories: **141** | Core tests: **2,052** (0 failures).

---

## [4.6.7] — 2026-05-14

- **Sprint 137 (T8.2)**: 50 new tweaks across 5 new modules — `NetworkDiscovery.cs` (netdisc, 10), `CertificatePolicy.cs` (certpol, 10), `PowerShellPolicy.cs` (pspolicy, 10), `DefenderAdvanced.cs` (defadv, 10), `EventLogGpoPolicy.cs` (evtgpo, 10).
- New categories: Network Discovery, Certificate Policy, PowerShell Policy, Defender Advanced, Event Log Policy.
- Total: 4,258 tweaks, 2,052 Core tests (0 failures).

## [4.6.6] — 2026-05-14

- **Sprint 136 (T8.2)**: 50 new tweaks across 5 new modules — `Biometrics.cs` (bio, 10), `WinRmHardening.cs` (winrm, 10), `LocationSensors.cs` (loc, 10), `SettingSyncAdv.cs` (ssync, 10), `AppPrivacyPolicy.cs` (appp, 10).
- New categories: Biometrics, WinRM Hardening, Location & Sensors, Settings Sync, App Privacy Policy.
- Total tweaks: **4,208** (+50). Tests: 2,660 passing.
- Version bump: `4.6.5` → `4.6.6`

## [4.6.5] — 2026-05-14

- **Sprint 135 (T6.6)**: Stryker.NET mutation testing — `stryker-config.json` (15 Core files, break=55%), `dotnet-stryker 4.14.0` in tools manifest, `Run-MutationTests.ps1`, `mutation-testing` CI job (main-branch only).
- Tests: 2,660 passing (unchanged)
- Version bump: `4.6.4` → `4.6.5`

## [4.6.4] — 2026-05-14

- **Sprint 134 (T6.3)**: Virtual registry integration tests — `VirtualRegistryTests.cs` (15 tests, GUID-scoped HKCU isolation, real Apply→Detect→Remove cycle, DryRun guard). No admin required, passes in CI.
- Tests: 2,660 passing (2,052 Core + 301 CLI + 307 GUI)
- Version bump: `4.6.3` → `4.6.4`

## [4.6.3] — 2026-05-14

- **Sprints 132–133 (T7.4)**: Plugin sandbox isolation — `PluginSandbox.cs` (named-pipe child-process execution, 30 s timeout), `SandboxOpDto`/`PluginSandboxRequest`/`PluginSandboxResponse` JSON protocol, `--plugin-host` CLI flag. 17 new tests.
- Tests: 2,037 passing
- Version bump: `4.6.2` → `4.6.3`

## [4.6.2] — 2026-05-13

- **Sprint 131 (T7.3)**: Pack RSA-SHA256 signing — `PackSignatureVerifier.cs`, `PackTrustLevel` enum, `PackDef.SignatureUrl`/`TrustLevel`, `PackIndex.AuthorKeys`/`GetAuthorPublicKey()`. 13 new tests.
- Tests: 2,020 passing
- Version bump: `4.6.1` → `4.6.2`

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
