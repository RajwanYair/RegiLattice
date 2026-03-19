# Changelog

All notable changes to RegiLattice are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
This project adheres to [Semantic Versioning](https://semver.org/).

## [3.7.0] — 2026-07-17

### Fixed

- **MSI missing from GitHub Releases** — `release.yml` never built `installer/RegiLattice.Installer.wixproj`; publish paths were `publish/release/gui|cli` but installer expects `publish/gui|cli`; both issues now corrected, WiX 6.0.2 installed + MSI build step added + SHA256SUMS updated to include MSI

### Added

- **`NetworkManager.cs`** +2 methods: `PingAsync(host, count, ct)` (shells Windows `ping`, parses stdout into `PingResult` record with loss/avg/min/max); `GetNetworkInterfaceStats()` (per-adapter `NetworkInterfaceStats` with byte + packet counters)
- **`StartupManager.cs`** +2 methods: `AddRegistryEntry(name, command)` (adds HKCU Run entry, validates uniqueness); `ExportEntriesAsync(filePath)` (JSON export of all startup entries)
- **`ServiceManager.cs`** +2 methods: `GetDependentServices(name)` (returns names of dependent services); `ExportToCsvAsync(filePath)` (CSV export with ServiceName, DisplayName, Status, StartType, CanStop)
- **`TweakHistory.cs`** +2 members: `GetSummaryStats()` returns `HistorySummaryStats` record (action counts + top-5 tweaks by frequency); `ExportToJsonAsync(filePath)` exports full history as JSON
- **`Favorites.cs`** +2 methods: `ExportToJsonAsync(filePath)` (sorted JSON array of IDs); `ImportFromJson(filePath)` (merge from JSON array, returns newly-added count)
- **`AppConfig.cs`** +2 properties: `AutoBackupOnApply` (bool, default `true`, triggers registry backup before batch apply); `SnapshotOnProfileChange` (bool, default `true`, auto-snapshots before profile is applied)
- **Tests**: +19 new tests — total 1 879 (all passing)

### Changed

- Version bumped `3.6.0` → `3.7.0`

---

## [3.6.0] — 2026-03-19

### Added

- **v3.6.0 release prep**: bumped version, clean Release build, MSI install package
- **About dialog**: shows `[Debug]` / `[Release]` build configuration; added clickable GitHub Releases link
- **Menu icons**: 7 new 16×16 programmatic icons in `AppIcons` — About, Hardware Info, What's New, Check Updates, Exit, Preferences, Import; all Help and File menu items now have icons
- **Sprint 46 (v3.6.0 release)**: CHANGELOG, Roadmap, README, GitHub metadata updated; copilot-instructions.md updated to v3.6.0; full test suite passing; MSI built

## [Unreleased] — Sprint 45

### Added

- **50 new tweaks** across 5 modules (+10 each):
  - **`Audio.cs`** (+10): `audio-disable-comms-ducking`, `-set-pro-audio-priority`, `-disable-audio-idle-powerdown`, `-set-avrcp-volume-sync`, `-set-audio-latency-mode`, `-enable-audio-log-off`, `-set-endpoint-builder-manual`, `-disable-voice-typing-toast`, `-set-render-clock-rate`, `-set-capture-clock-rate`
  - **`Gaming.cs`** (+10): `game-disable-msmq-service`, `-disable-gameinput-service`, `-set-dxgi-flip-model`, `-enable-game-bar-perf-counter`, `-disable-diagtrack-autologger`, `-set-xgip-service-manual`, `-disable-ndu-adapter`, `-set-games-sfio-priority-high`, `-set-mouse-fix-off`, `-set-games-affinity-all-cpus`
  - **`Security.cs`** (+10): `sec-require-ldap-signing`, `-disable-rdp-clipboard-sync`, `-disable-rdp-drive-mapping`, `-enforce-smb-ntlmv2-auth`, `-disable-printer-spooler-network`, `-enable-run-as-different-user`, `-disable-office-macros-internet`, `-disable-wsh-scripting`, `-restrict-lsass-credential-dump`, `-disable-named-pipe-impersonation`
  - **`WindowsUpdate.cs`** (+10): `wu-disable-automatic-updates`, `-set-schedule-day-saturday`, `-disable-store-app-auto-updates`, `-set-update-service-manual`, `-require-admin-for-updates`, `-disable-metered-update-download`, `-disable-reboot-required-notification`, `-set-feature-update-channel-general`, `-set-orchestrator-service-manual`, `-disable-third-party-preview`
  - **`RemoteDesktop.cs`** (+10): `rdp-set-max-connections-unlimited`, `-set-color-depth-32`, `-disable-smart-card-redirection`, `-set-remote-assistance-off`, `-set-audio-play-on-server`, `-disable-com-port-redirect`, `-enforce-tls-security-layer`, `-limit-single-monitor`, `-set-connection-timeout-8h`, `-disable-lpt-port-redirect`
- **`UpdateCheckService.cs`** — GitHub Releases API checker; returns `UpdateInfo` record (`UpdateAvailable`, `CurrentVersion`, `LatestVersion`, `ReleaseNotes`, `DownloadUrl`)
- **`ComplianceService.cs`** — compare live registry state against a saved snapshot; returns `ComplianceReport` with `Drifted` list and `IsCompliant` flag
- **`GroupPolicyExporter.cs`** — exports `TweakKind.GroupPolicy` tweaks to `.admx` + companion `.adml` file pair
- **`UpdateCheckerDialog.cs`** — GUI dialog for Help → Check for Updates; shows current vs latest version, release notes, download link
- **Help → Check for Updates** menu item wired in `MainForm`
- **CLI `--compliance <snapshot>`** — detects configuration drift against a snapshot file; exits 1 if non-compliant
- **CLI `--export-gpo <path>`** — exports Group Policy tweaks as ADMX/ADML pair
- **StartupManagerDialog +2**: Export CSV button + Open File Location button
- **ServiceManagerDialog +2**: Restart Service button + Set to Automatic button
- **DiskSpaceDialog +2**: Clean TEMP button + live TEMP folder size label
- **WindowsUpdateControlDialog +2**: Update History button (ms-settings deeplink) + Reset WU Components button
- **MemoryCleanerDialog +2**: Auto-clean checkbox + threshold spinner with 30-second polling timer
- **NetworkToolsDialog +2**: Ping tab (multi-host concurrent ping) + Traceroute tab (live `tracert` output)
- **`TweakEngineBuiltinsTests.cs`** — 5 new `[Theory]` methods; 50 new `[InlineData]` entries covering all Sprint 45 tweaks
- Total: **2 946 tweaks** (+50)

---

## [Unreleased] — Sprint 44

### Fixed

- **CRLF line endings** — 152 C# source files normalised from CRLF to LF across `src/` and `tests/`
  to match the `.gitattributes` `*.cs eol=lf` declaration; 0 logic changes

### Added

- **Japanese locale (`ja`)** — 51-key translation added to `Locale.cs` (`BuiltInLocales["ja"]`);
  RegiLattice now ships 6 built-in locales: en, de, fr, es, he, ja
- **50 new tweaks** across 5 modules (+10 each):
  - **`PhoneLink.cs`** (23→33): AllJoyn Router service, WPD service, Link to Windows banner policy,
    Continue on PC delivery, Phone activation policy, Device Association Framework service,
    CDP activation prompt, cross-device roaming trigger consent, Wi-Fi hotspot auth policy,
    Windows Hello companion device
  - **`OneDrive.cs`** (23→33): Known Folder Move opt-in block, KFM silent redirect block,
    delay update ring, SharePoint sync disable, app sync disable, mass-delete threshold (50 files),
    hydration-on-access preview block, auto-update disable, File Explorer side-panel hide,
    external collaboration block
  - **`Notifications.cs`** (25→35): Low disk space alert, Windows Defender notifications (user),
    Windows Update reboot nag, legacy balloon tips, SmartScreen evaluation warnings (user),
    taskbar content suggestions, OEM preinstalled app suggestions, Windows tips & tricks,
    clear recent docs on exit, no-logged-on-user reboot (WU)
  - **`Gaming.cs`** (29→39): SFIO priority High, NDU service disable, SystemResponsiveness=0,
    network throttling index off, GPU priority=8, latency sensitivity High, Background Only=False,
    task CPU priority=6, Xbox Accessory Management service, max user port 65534
  - **`Maintenance.cs`** (28→38): Clear recent docs on logoff, service shutdown timeout (2s),
    app kill timeout (2s), long path support (260+), desktop cleanup wizard, hung-app timeout (2s),
    AutoEndTasks on shutdown, crash-on-audit-fail disable, hide Recent in Quick Access,
    hide Frequent Folders in Quick Access
- **Test coverage** — 50 new `[InlineData]` entries across 5 new `[Theory]` test methods in
  `TweakEngineBuiltinsTests.cs`; Core test count 1344 → 1394

### Stats

- Total tweaks: **2 896** (+50 from Sprint 43)
- All Core tests: **1 394** (1394 Core) — all passing

---

## [Unreleased] — Sprint 43

### Fixed

- **Version display** — `AssemblyInfo.cs` created (`AssemblyVersion`/`AssemblyInformationalVersion = "3.5.0"`);
  `AboutDialog` reads `AssemblyInformationalVersionAttribute` so version shows `3.5.0` instead of `0.0.0.0`
- **`WindowsHealthDialog` crash** — `AppendLog` and `SetBusy` now fully dispose-safe and thread-safe;
  `RunBatchAsync` final status block guarded with `if (!IsDisposed)`
- **Marketplace 404 + corporate proxy** — `PackManager.s_http` uses `HttpClientHandler` with
  `WebRequest.GetSystemWebProxy()` and `UseDefaultCredentials = true`; `FetchIndexAsync` handles
  `HttpStatusCode.NotFound` gracefully returning an empty `PackIndex`

### Added

- **Admin indicator** — firebrick `ToolStripStatusLabel` (`🛡 ADMIN`) in StatusStrip, visible only when
  process is elevated
- **Package Manager top-level menu** — Scoop, pip, PowerShell modules, WinGet, Chocolatey, and Tweak Pack
  Marketplace moved from `Tools` into a dedicated `&Package Manager` top-level menu item
- **Hebrew locale (`he`)** — 51-string translation added to `Locale.cs` (`BuiltInLocales["he"]`)
- **50 new tweaks** across 5 modules (+10 each):
  - **`Debloat.cs`** (29→29): Find My Device, Inking/Typing Personalization, Nearby Sharing, Mixed Reality
    Portal, Steps Recorder, Error Reporting UI, Wireless Display Projection, Post-Update OOBE, Tablet Mode
    Auto-Switch, Spotlight Content in Settings
  - **`BrowserCommon.cs`** (19→29): Cast icon, Sign-in interception, Edge Shopping Assistant, Edge Follow,
    Chrome NTP custom background, Chrome promotional tabs, Chrome NTP spotlight recs, First-run experience,
    Address autofill, Edge pre-launch startup boost
  - **`SystemRestore.cs`** (20→30): Hibernate file, Small crash dump, Dump overwrite, No auto-reboot on
    BSOD, WER 2nd-level data, WER queue limit, WER archive limit, WER throttle bypass, WER response timeout,
    BSOD admin alert
  - **`ScheduledTaskTweaks.cs`** (20→30): Compatibility Appraiser, RAC task, ProgramDataUpdater, WER
    QueueReporting, Device Information, Power Efficiency Diagnostics, SmartScreen AppID, MRT telemetry,
    Defender Cache Maintenance, USB CEIP
  - **`WindowsRecall.cs`** (17→27): Publish user activities (HKCU), Cross-device clipboard, Typing
    insights, Taskbar AI widget content, Cloud search, Voice data collection, Auto map downloads,
    ContentDelivery feature management, Spotlight on settings, CEIP/SQM policy

### Stats

- Total tweaks: **2 846** (+50 from Sprint 42)
- All tests: **1 740** (1344 Core + 154 CLI + 242 GUI) — all passing

---

## [Unreleased] — Sprint 42

### Fixed

- **Version display** — `AssemblyInfo.cs` created (`AssemblyVersion`/`AssemblyInformationalVersion = "3.5.0"`);
  `AboutDialog` reads `AssemblyInformationalVersionAttribute` so version shows `3.5.0` instead of `0.0.0.0`
- **`WindowsHealthDialog` crash** — `AppendLog` and `SetBusy` now fully dispose-safe and thread-safe;
  `RunBatchAsync` final status block guarded with `if (!IsDisposed)`
- **Marketplace 404 + corporate proxy** — `PackManager.s_http` uses `HttpClientHandler` with
  `WebRequest.GetSystemWebProxy()` and `UseDefaultCredentials = true`; `FetchIndexAsync` handles
  `HttpStatusCode.NotFound` gracefully returning an empty `PackIndex`

### Added

- **Admin indicator** — firebrick `ToolStripStatusLabel` (`🛡 ADMIN`) in StatusStrip, visible only when
  process is elevated
- **Package Manager top-level menu** — Scoop, pip, PowerShell modules, WinGet, Chocolatey, and Tweak Pack
  Marketplace moved from `Tools` into a dedicated `&Package Manager` top-level menu item
- **Hebrew locale (`he`)** — 51-string translation added to `Locale.cs` (`BuiltInLocales["he"]`)
- **50 new tweaks** across 5 modules (+10 each):
  - **`Debloat.cs`** (29→29): Find My Device, Inking/Typing Personalization, Nearby Sharing, Mixed Reality
    Portal, Steps Recorder, Error Reporting UI, Wireless Display Projection, Post-Update OOBE, Tablet Mode
    Auto-Switch, Spotlight Content in Settings
  - **`BrowserCommon.cs`** (19→29): Cast icon, Sign-in interception, Edge Shopping Assistant, Edge Follow,
    Chrome NTP custom background, Chrome promotional tabs, Chrome NTP spotlight recs, First-run experience,
    Address autofill, Edge pre-launch startup boost
  - **`SystemRestore.cs`** (20→30): Hibernate file, Small crash dump, Dump overwrite, No auto-reboot on
    BSOD, WER 2nd-level data, WER queue limit, WER archive limit, WER throttle bypass, WER response timeout,
    BSOD admin alert
  - **`ScheduledTaskTweaks.cs`** (20→30): Compatibility Appraiser, RAC task, ProgramDataUpdater, WER
    QueueReporting, Device Information, Power Efficiency Diagnostics, SmartScreen AppID, MRT telemetry,
    Defender Cache Maintenance, USB CEIP
  - **`WindowsRecall.cs`** (17→27): Publish user activities (HKCU), Cross-device clipboard, Typing
    insights, Taskbar AI widget content, Cloud search, Voice data collection, Auto map downloads,
    ContentDelivery feature management, Spotlight on settings, CEIP/SQM policy

### Stats

- Total tweaks: **2 846** (+50 from Sprint 42)
- All tests: **1 740** (1344 Core + 154 CLI + 242 GUI) — all passing

---

## [Unreleased] — Sprint 42

### Added — Hardware & Network Tools

- **`HardwareTemperatureDialog`** — WMI thermal zone polling (`MSAcpi_ThermalZoneTemperature` in `root\WMI`),
  GPU via `Win32_VideoController`; colour-coded bars (green <60°C, amber 60–80°C, red ≥80°C); 3-second
  auto-refresh checkbox; graceful fallback when WMI unavailable; exposes `hwtempmon` in `--tool`
- **`NetworkBandwidthDialog`** — real-time NIC bandwidth monitor via `IPv4Statistics` delta-calc;
  1-second polling timer; per-adapter ↑ send / ↓ recv with B/s, KB/s, MB/s auto-scale; exposes
  `netbandwidth` in `--tool`
- **`MacAddressDialog`** — WMI `Win32_NetworkAdapter` MAC address viewer + randomizer:
  generates locally-administered unicast MAC, writes to registry `NetworkAddress` key under
  `HKLM\SYSTEM\CurrentControlSet\Control\Class\{4D36E972…}`, re-enables adapter via `netsh`;
  Copy-to-clipboard button; admin warning banner; exposes `macaddress` in `--tool`
- **Phase 2 #13** — Automatic memory cleaning on threshold: `AppConfig.AutoCleanMemoryThreshold` (int,
  0=disabled); `OnMonitorTimerTick` purges all process working sets if `memPct >= threshold`
- **Phase 2 #14** — System tray tooltip shows live RAM %: `_trayIcon.Text = $"RegiLattice — RAM: {memPct}%"`
- **Phase 2 #17** — Network connectivity status indicator: `_netLabel` in status strip, colour-coded
  green ✓ / red ✗ via `NetworkInterface.GetIsNetworkAvailable()`, refreshed every monitor tick
- **AppIcons** — `ThermometerMenuBitmap`, `BandwidthMenuBitmap`, `MacAddressMenuBitmap` with
  custom `DrawThermometerIcon`, `DrawBandwidthIcon`, `DrawMacAddressIcon` rendering

### Added — 29 New Tweaks

- **`EventLogging.cs`** — 10 new tweaks: limit Application/System/Setup event log sizes, disable
  PowerShell script-block & module logging, disable WER event log entries, disable forwarded events
  (Wecsvc), disable DNS client event tracing, disable NT Kernel Logger ETW session, disable logon
  failure audit
- **`ProxyVpn.cs`** — 10 new tweaks: disable WinHTTP WPAD auto-discovery, disable IE/WinINet proxy
  bypass, disable VPN split tunneling (RAS), disable RAS AutoDial service, disable IPv6 Teredo,
  disable WinINet AutoDetect, disable 6to4 tunneling, disable IP-HTTPS adapter, disable NCSI probing,
  disable TCP timestamps
- **`PowerShellTweaks.cs`** — 9 new tweaks: set execution policy to RemoteSigned, enable PS Remoting,
  opt out of PowerShell telemetry, enable Constrained Language Mode, disable transcription logging,
  enable Protected Event Logging, disable clipboard history policy, set system-managed page file,
  enable TLS 1.2 for .NET apps

### Tests

- +29 `[InlineData]` entries in `TweakEngineBuiltinsTests.cs` covering all new tweak IDs
- Total: **2 796 tweaks** (+29), **1 740 tests** (+29 passing)

---

## [Unreleased] — Sprint 41

### Fixed

- **`MemoryCleanerDialog`** — resolved 3 build errors: `AppTheme.ApplyToForm` renamed to `AppTheme.Apply`,
  `new SystemMonitor().GetMemoryUsage()` corrected to static call `SystemMonitor.GetMemoryUsage()`,
  2-element tuple deconstruct expanded to 3-element (`(used, _, _)`)

### Added — System Monitor Tool Dialogs

- **`DiskSpaceDialog`** — per-drive disk space overview with colour-coded usage bars (green <70%, amber 70–90%,
  red ≥90%); double-click drive to open in Explorer; async refresh; exposes `diskspace` in `--tool`
- **`PortScannerDialog`** — TCP port/connectivity tester: hostname + CSV port input, 7 preset groups
  (Web, SSH/RDP, FTP, Mail, Database, DNS/DHCP, Top 20), async parallel scan with 2 s timeout,
  ping test, WellKnownService name lookup, colour-coded OPEN/CLOSED results; exposes `portscan` in `--tool`
- **`BatteryHealthDialog`** — WMI `BatteryStaticData` + `BatteryStatus` health monitor: design vs full-charge
  capacity, cycle count, charge/discharge rate bars; **Full Report** runs `powercfg /batteryreport`;
  graceful no-battery fallback; exposes `batteryhealth` in `--tool`
- **`MemoryCleanerDialog`** wired into `Program.cs ResolveManagerArg()` (`memorycleaner`) and
  `Tools → Memory Cleaner` menu item
- Phase 2 items 12 (Memory Cleaner), 16 (Disk Space), 18 (Battery Health) and Phase 4 item 37
  (Port Scanner) completed

### Added — 31 New Tweaks

**`DiskCleanup.cs`** (+10, total 25):
`cleanup-disable-recent-docs`, `cleanup-disable-recent-programs`, `cleanup-disable-search-history`,
`cleanup-disable-swap-file`, `cleanup-disable-auto-maintenance`, `cleanup-disable-volume-shadow-copy`,
`cleanup-disable-internet-temp-auto`, `cleanup-disable-wer-queue`, `cleanup-disable-superfetch-write`,
`cleanup-limit-disk-usage-windows-update`

**`UserAccount.cs`** (+11, total 30):
`uac-disable-account-picture`, `uac-disable-guest-account`, `uac-disable-biometrics-policy`,
`uac-disable-smartcard-removal-lock`, `uac-disable-windows-hello-for-business`,
`uac-lock-workstation-on-screensaver`, `uac-disable-microsoft-account-logon`,
`uac-enforce-password-complexity`, `uac-disable-offline-files`, `uac-disable-fast-user-switching`,
`uac-disable-linked-connections`

**`AppCompatibility.cs`** (+10, total 29):
`compat-disable-wer-server-connection`, `compat-disable-compat-telemetry-runner`,
`compat-disable-user-choice-protection`, `compat-disable-vdm-allowed`,
`compat-disable-app-repkg-service`, `compat-disable-install-service`,
`compat-disable-just-in-time-debugging`, `compat-enable-dep-always-on`,
`compat-disable-error-reporting-ui`, `compat-disable-ie-compat-view`

### Tests

- 31 new `[InlineData]` entries in `TweakEngineBuiltinsTests.cs` covering all new tweak IDs
- Total: **1 711 tests** passing (1 315 Core + 154 CLI + 242 GUI, 1 intentional skip)
- Tweaks: **2 767** across 92 categories

## [3.5.0] — 2026-03-18

### Added — Sprint 27: Network Tools

- **`NetworkManager`** Core service: DNS quick-switch (`SetDnsAsync`, `ResetDnsToDhcpAsync`),
  network repair suite (`FlushDnsCacheAsync`, `ResetTcpIpAsync`, `ResetWinsockAsync`), and
  DHCP lease renewal (`RenewDhcpLeaseAsync`, `RepairAllAsync` IAsyncEnumerable)
- **6 built-in DNS presets** — Automatic (DHCP), Cloudflare, Google, Quad9, OpenDNS, NextDNS —
  as `DnsPreset` records with IPv4 + IPv6 addresses
- **`NetworkToolsDialog`** — adapter drop-down, DNS preset quick-switch buttons, repair action
  buttons, async operation log (`RichTextBox`), admin elevation banner
- **Tools → Network Tools** menu item with globe icon

### Added — Sprint 28: Startup Manager

- **`StartupManager`** Core service: reads/writes HKCU Run, HKLM Run, per-user and all-users
  Startup shell folders; exposes `GetAllEntries()`, `SetEnabled()`, `Delete()`
- **`StartupEntry`** record (`Id`, `Name`, `Command`, `Location`, `IsEnabled`) and
  **`StartupLocation`** enum (`RegistryUser`, `RegistryMachine`, `FolderUser`, `FolderAllUsers`)
- **`StartupManagerDialog`** — resizable ListView (Name / Status / Location / Command),
  Enable / Disable / Delete / Refresh buttons, admin elevation banner
- **Tools → Startup Manager** menu item with rocket icon

### Added — Sprint 29: Service Manager

- **`ServiceManager`** Core service: enumerates all Windows services (`GetAllServices()`),
  queries single service (`GetService(name)`), `StartAsync`, `StopAsync`,
  `SetStartTypeAsync` (delegates to `sc.exe config start=`)
- **`ServiceEntry`** record (ServiceName, DisplayName, Description, Status, StartType,
  CanStop, CanPauseAndContinue)
- **`ServiceManagerDialog`** — searchable ListView, description pane, async Start/Stop/
  Enable/Disable/Refresh, admin elevation banner, CancellationToken support
- **Tools → Service Manager** menu item with gear icon
- Added `System.ServiceProcess.ServiceController` v9.0.3 NuGet to `Directory.Packages.props`
  and `RegiLattice.Core.csproj`

### Added — Task 6: BaseDialog Consolidation

- **`BaseDialog : Form`** abstract class with constructor `(string title, Size size, bool resizable)`;
  sets common Form properties (StartPosition, ShowInTaskbar, Icon, MaximizeBox, MinimizeBox)
- Helper factory methods: `CreateSectionHeader()`, `CreateLabel()`, `CreateButtonRow()`, `CreateButton()`
- **Migrated** `NetworkToolsDialog`, `StartupManagerDialog`, `ServiceManagerDialog` to `: BaseDialog`
  — eliminates ~10 lines of identical boilerplate per dialog

### Tests

- **`NetworkManagerTests.cs`** — 8 tests covering `DnsPreset.BuiltIn` structure and
  `NetworkManager.GetActiveAdapterNames()` read-only operations
- **`StartupManagerTests.cs`** — 7 tests covering `StartupManager.GetAllEntries()` return
  contract and `StartupEntry` record semantics
- **`ServiceManagerTests.cs`** — 10 tests covering `ServiceManager.GetAllServices()`,
  `GetService()`, and `ServiceEntry` record semantics

## [3.4.0] — 2026-03-17

### Added — Pre-production Release: Installer, GUI Polish & Repo Cleanup

- **`RegiLattice-3.4.0-win-x64.msi` installer** — self-contained WiX v6 MSI for
  one-click install of GUI + CLI; adds CLI to `PATH`; Start Menu shortcut; upgrade
  and uninstall via Add/Remove Programs
- **Portable executables** — `RegiLattice.GUI.exe` and `RegiLattice.exe` published
  as self-contained win-x64 single-file binaries, available on the Releases page
- **ListView double-click to toggle checkbox** — double-clicking any tweak row in the
  GUI now toggles its checkbox (select/deselect for batch apply)
- **Selected items counter in status bar** — the bottom status bar now shows `☑ N selected`
  when one or more tweaks are checked, updating live as checkboxes change
- **`.gitignore` extended** — added `*.msi`, `*.wixpdb`, `*.wixobj`, `*.cab` to prevent
  WiX installer build outputs from being tracked
- **README overhauled** — updated to reflect 2 610 tweaks, 1 199 tests, 11 themes,
  added Download & Install section with MSI link, added Building the Installer section
  with step-by-step commands, corrected all stale counts and badges

### Fixed

- Indentation of ListView event handler wiring in `MainForm.Designer.cs` (cosmetic)

### Added — Sprint 21: 50 New Tweaks & +10% Coverage Goal Exceeded

- **50 new tweaks** across 5 categories:
  Security (10), Virtualization (10), Bluetooth (10), Accessibility (10), Cortana & Search (10)
- **93 new Core tests** added across 4 files:
  `TweakDefTests.cs`, `TweakEngineTests.cs`, `RegistrySessionTests.cs`, `ServicesTests.cs`
- **Coverage boost (Core, Cobertura line rate):**
  - TweakEngine.cs: **82.3% → 90.18%** (+7.88)
  - TweakDef.cs: **67.9% → 100%** (+32.1)
  - RegistrySession.cs: **38.9% → 90.25%** (+51.35)
  - Analytics.cs: **54.1% → 100%** (+45.9)
  - Locale.cs: **84.6% → 100%** (+15.4)
  - Ratings.cs: **88.9% → 100%** (+11.1)
  - Overall Core line coverage: **95.3%**
- **Full validation:** all tests pass on solution run (**1,199/1,199**)
- Total: **2,610 tweaks**, **1,199 tests passing** (888 Core + 116 CLI + 195 GUI)

### Added — Sprint 20: GUI Enhancements, Coverage Boost & 50 New Tweaks

- **Search clear button**: ToolStripButton "✕" next to search box, auto-shown/hidden
  when search text is non-empty, clears search on click
- **ListView checkboxes**: custom owner-drawn 14px checkboxes with accent-colour fill
  and white checkmark on checked items, reflecting selection state visually
- **Pending status filter**: new "Pending" option in status filter combo to show only
  tweaks with a pending reboot requirement (from `_pendingRebootIds`)
- **Reboot warning on exit**: when exiting with pending tweaks, a MessageBox warns that
  changes will only take effect after a reboot
- **Test coverage improvement**: TweakEngine.cs 61% → 82.26% (+21%);
  14 new Core tests, ~30 new GUI tests, 6 new CLI tests
- **Memory Optimization** (10 new): pool usage max, session pool size, conservative swap,
  crash dump disable, auto-reboot BSOD disable, dirty page threshold, heap decommit,
  PAE enable, write watch disable, paged pool quota
- **Storage** (10 new): disk quotas disable, volume shadow schedule disable,
  low disk space warning disable, write cache flush enable, thumbnail cache cleanup disable,
  remote diff compression disable, recycle bin 5% max, WER dump disable,
  search index backoff disable, offline files cache disable
- **Startup** (10 new): tablet mode prompt disable, sign-in info reopen disable,
  boot logo disable, auto maintenance disable, narrator at login disable,
  fast user switching disable, logon provider ads disable, Edge prelaunch disable,
  prefetch on SSD disable, compatibility assistant disable
- **SSD Optimization** (10 new): AHCI link power management disable, DIPM disable,
  idle power timeout disable, MFT zone increase, log file flush disable,
  pagefile encryption disable, power scheme optimize, directory timestamp disable,
  volatile write cache enable, global content indexing disable
- **File System** (10 new): critical worker threads increase, delayed worker threads increase,
  change notifications disable, path cache increase, opportunistic locking enable,
  NTFS tunneling disable, I/O queue depth increase, long paths via policy,
  TxF rollback disable, file handle limit increase
- Total: **2,560 tweaks**, **~1,090 tests passing** (50 new tests added)

### Added — Sprint 19: System Monitoring, Live CPU/RAM & 50 New Tweaks

- **SystemMonitor service** (`SystemMonitor.cs`): live system resource monitoring
  via `GetSystemTimes` and `GlobalMemoryStatusEx` P/Invoke — CPU usage (delta-based),
  memory usage (used/total MB + percent), system uptime
- **Live CPU/RAM status bar**: 2-second polling timer displays `CPU: X%` and
  `RAM: X.X / Y.Y GB (Z%)` in the MainForm status strip with accent-coloured labels
- **System uptime in About dialog**: shows `Uptime: Xd Yh Zm` in the hardware info panel
- **Display** (10 new): Windows Ink Workspace disable, force disable HDR, high contrast mode,
  color depth 32-bit, auto-rotation disable, caption button height, mouse hover select,
  full-screen optimization, menu animation fade, peek desktop disable
- **Fonts** (10 new): DPI-aware font scaling, font substitution policy, Cascadia Code icon title font,
  TrueType rendering, font hinting disable, system font size default, DirectWrite enable,
  font providers disable, caption font weight, message font default
- **Input** (10 new): Feedback Hub disable, wheel scroll chars, pen workspace disable,
  handwriting panel disable, mouse hover width, touch visualizations, gesture visualizations,
  input personalization disable, writing insights disable, mouse pointer speed
- **Audio** (10 new): recording quality limit, stereo mix enable, MMCSS scheduling,
  network throttling (multimedia), audio graph isolation, device priority high,
  DPC latency low, beep sounds disable, critical battery sound disable, headphone auto-detect
- **Taskbar** (10 new): taskbar transparency disable, full path title bars, Cortana taskbar button,
  taskbar animations disable, Ink Workspace button hide, news feed taskbar, multi-display show all,
  thumbnail preview disable, thumbnail preview size, peek live preview disable
- **7 new xUnit tests**: SystemMonitor CPU usage, memory usage, uptime, consistency, multi-instance
- Total: **2,510 tweaks**, **1,305 tests passing** (784 Core + 111 CLI + 410 GUI)

### Added — Sprint 18: GUI Visual Overhaul, 7 New Themes & 50 New Tweaks

- **7 new colour themes**: Tokyo Night, Gruvbox Dark, Solarized Dark, One Dark Pro,
  Rosé Pine, Everforest, Cyberpunk — 11 themes total, all switchable at runtime
- **AppIcons overhaul**: all 9 existing icons upgraded to vibrant gradient fills
  (LinearGradientBrush), rounded-rect/circle shapes with GDI+ anti-aliasing
- **7 new menu icon bitmaps**: File, View, Help, Apply (green checkmark),
  Remove (red X), Refresh (blue circular arrow), Export (cyan arrow-out)
- **ToolStrip buttons**: Apply/Remove/Refresh now use colourful ImageAndText style
  instead of plain Unicode text
- **Top-level menus**: File, View, Help now have colourful icon bitmaps
- **Export menu items**: all three export formats (PS1, JSON, REG) display Export icon
- **MainForm visual polish**: gradient header backgrounds (surface→overlay),
  gradient selection highlight (accent tint→overlay) for selected rows
- **DNS & Networking Advanced** (10 new): LLMNR disable, IPv4 DNS priority,
  smart multi-homed disable, DNS client diagnostics, WINS disable,
  negative cache TTL, devolution fallback, FQDN-only, ETW query logging,
  parallel adapter query disable
- **Encryption** (10 new): DES cipher disable, strong key enforcement,
  TLS session ticket lifetime, export-grade cipher disable, OCSP stapling,
  MD5 disable, cert padding enforcement, legacy renegotiation disable,
  extended master secret, Triple DES disable
- **Firewall** (10 new): multicast/broadcast response disable, domain/private profile logging,
  public outbound default-block, NetBIOS/SMB/RPC inbound blocking,
  notification suppression, log max size increase, domain outbound block
- **Hardening** (10 new): WPAD disable, LM auth disable (NTLMv2 only),
  remote registry disable, SEHOP, mandatory ASLR, anonymous SAM restrict,
  CFG enforcement, autoplay all-drives, named pipe restrict, NTLM outgoing block
- **Recovery** (10 new): auto-restart disable, boot logging, minidump type,
  auto-repair disable, crash upload disable, dump folder path, dump count increase,
  startup repair prompt, system failure popup, overwrite existing dump
- **28 new xUnit tests**: 21 new theme verification Theory tests (7 dark themes × 3),
  7 new AppIcons bitmap validity tests

### Added — Sprint 17: Core Services, CLI Commands & 50 New Tweaks

- **ConfigExporter** service — export/import portable tweak selection configs as JSON;
  supports 3 import formats (full, array, object), Validate() for ID hygiene
- **Favorites** service — persist user's favorite tweak IDs with thread-safe static API;
  case-insensitive HashSet, Add/Remove/Toggle/IsFavorite/All/Flush/Clear
- **TweakHistory** service — rolling 500-entry history of tweak operations (apply/remove/update);
  HistoryEntry model with ISO 8601 timestamps, Recent/ForTweak/Flush
- **7 new CLI commands**: `--export-config`, `--import-config`, `--favorites`,
  `--favorite-add`, `--favorite-remove`, `--history`, `--history <count>`
- **Display** (10 new): icon spacing, scrollbar dimensions, border width, window shake,
  menu show delay, text cursor indicator, tooltip delay, dark mode system
- **Startup** (10 new): welcome experience, tips & suggestions, boot timeout,
  first-logon animation, pre-launch apps, background apps policy, autoplay
- **Network Optimization** (10 new): ARP cache size, max connections, NetBIOS over TCP/IP,
  LMHosts lookup, DNS cache TTL, WPAD, RSS, SMB throttling, max user port, TCP timestamps
- **Power Management** (10 new): processor boost, throttle states, energy saver,
  away mode, min processor state, unattended timeout, dimmed display, hybrid sleep,
  lid close action, and more
- **Privacy** (10 new): error reporting, web search in Start, search highlights,
  cloud content search, app launch tracking, handwriting error reports,
  customer experience program, inventory collector
- **40 new tests**: FavoritesTests (11), TweakHistoryTests (11), ConfigExporterTests (10),
  FavoritesAndHistoryParseTests (8)
- Total: **2,410 tweaks** across 89 categories, **1,001 tests passing**

### Added — Sprint 15: 50 New Tweaks

- **Power Management** (10 new): adaptive brightness, power throttling, hard disk timeout,
  core parking, PCI Express max performance, display scaling, processor idle demote,
  energy estimation, high precision timer, turbo boost control
- **Command Line** (7 new): .NET 3.5, IPv6 tunnel adapters, processor scheduling,
  NetBIOS broadcast, NTP high frequency, MPO, Game DVR
- **Developer** (10 new): .NET CLI telemetry, symlink without admin, Python UTF-8 mode,
  Git credential manager, Git default branch, Git autocrlf, Cargo PATH, WER disable,
  environment variable size, Windows Containers
- **Hardening** (10 new): AutoRun disable, remote SAM restriction, remote assistance,
  SMB signing (client + server), LLMNR disable, SMB encryption, cached logons limit,
  admin shares disable
- **Network Optimization** (10 new): TCP Fast Open, TCP slow start, ARP cache, RSC,
  direct cache access, TCP max connections, TCP keepalive, NIC flow control,
  NIC power management, DNS IPv4 priority
- Total: **2,363 tweaks** across 89 categories

### Added — Sprint 16: Security Audit & Validation Enhancement

- **TweakValidator.DetectDuplicateRegistryOps()** — new public method that warns when
  multiple tweaks write to the same registry `Path\Name` target. Case-insensitive.
  Skips check-only ops (CheckValue, CheckMissing, CheckKeyMissing).
- **TweakEngine.DetectDuplicateRegistryOps()** — convenience method delegating to
  TweakValidator for engine consumers
- **CLI `--validate`** now shows duplicate registry warnings separately from errors;
  exit code 1 only on errors (warnings are informational)
- **6 new validator tests**: DuplicateTarget, SamePathDiffNames, CaseInsensitive,
  SameTweakMultiOps, DeleteTreeDuplicate, NoOverlap
- **100-item future roadmap** added to docs/Roadmap.md spanning 10 phases
- **Competitive analysis** of 13 Win11 tweak tools (Winaero, ExplorerPatcher, OFGB, etc.)

### Removed — Security Hardening

- Removed 3 insecure tweaks from CommandLineTweaks.cs:
  - `cmd-enable-telnet-client` — enables unencrypted Telnet protocol
  - `cmd-enable-tftp-client` — enables unencrypted TFTP protocol
  - `cmd-enable-fsutil-disable-encrypt` — disables EFS disk encryption

### Changed

- GUI test suite optimised: 49% faster xUnit time (ToolVersionChecker timeout reduced,
  assembly-level parallelism enabled)

## [3.3.0] — 2026-03-16

### Refactored — Codebase Architecture Improvements

#### Core Engine Decomposition

- **SnapshotManager** — extracted Save/Load/Restore snapshot logic from TweakEngine
  into dedicated `SnapshotManager.cs` (single responsibility, backward-compatible delegation)
- **TweakValidator** — extracted `ValidateTweaks()` + circular dependency detection
  into static `TweakValidator.cs` with pure-function API
- **DependencyResolver** — extracted `ResolveDependencies()`, topological sort, and
  reverse lookup (`Dependents()`) into static `DependencyResolver.cs`
- TweakEngine public API unchanged — all existing tests pass without modification

#### CLI Extraction

- **CliArgs** — extracted nested `Program.CliArgs` class to standalone `CliArgs.cs`
- **ConsoleColorizer** — extracted 5 ANSI color helper methods from Program.cs into
  `ConsoleColorizer.cs` with `NoColor` toggle property

#### Package Manager DRY Elimination

- **PackageNameValidator** — consolidated 5 identical `SafeNameRegex` patterns and
  `ValidateName()` methods from Scoop/Pip/WinGet/Chocolatey/PSModule managers into
  single shared utility with `Validate()` and `ExtractNames()` methods

#### Tests (Sprint 12)

- 27 new tests (700 → 727): 10 ConsoleColorizer tests, 8 PackageNameValidator tests,
  9 additional CLI parsing tests

### Added — Test Coverage Expansion (Sprint 13)

- **SnapshotManagerTests.cs** — 12 direct tests (Save, Load, Restore, round-trip, edge cases)
- **TweakValidatorTests.cs** — 19 direct tests (valid tweaks, empty fields, duplicates,
  circular deps, broken deps)
- **DependencyResolverTests.cs** — 15 direct tests (Resolve topological sort, Dependents
  reverse lookup, circular detection)
- TweakEngine edge case tests (+13): TweaksByScope, Filter, IsApplicableOnHardware,
  DetectStatus, StatusMap subset, Search multi-token
- RegistrySession edge case tests (+17): Execute DryRun, Evaluate CheckMissing/CheckKeyMissing,
  Backup, WriteLog, Read ops, ParsePath
- Total after Sprint 13: **799 tests** (643 Core + 72 CLI + 84 GUI)

### Added — Deep Test Expansion (Sprint 14)

- **TweakEngineBuiltinsTests.cs** — new integration test file with shared `BuiltinsFixture`;
  63 test methods covering RegisterBuiltins validation, global ID uniqueness, required fields,
  profile coverage (all 5 profiles), category counts, search/filter integration, scope
  distribution, TweakKind distribution, dependency resolution on real data
- Expanded TweakDefTests (+57 methods): RegOp factory coverage, TweakScope computation,
  HasOperations gate, KindHint override, ExpectedResult generation
- Expanded ServicesTests (+70 methods): Analytics, AppConfig, CorporateGuard, Elevation,
  HardwareInfo, Locale, Ratings comprehensive coverage
- Expanded CLI ParseArgsTests (+31): additional flag/option combos, edge cases, scope parsing
- Expanded GUI ThemeTests (+48): all 4 themes colour attribute validation, system detection
- Expanded GUI AppIconsTests (+13): bitmap/icon validity, cache invalidation safety
- Total: **2,316 tweaks**, **972 tests** (738 Core + 103 CLI + 131 GUI), **13 test files**

## [3.2.1] — 2026-03-15

### Sprint 11 — GUI Polish, Package Manager Fixes & Documentation Refresh

#### Changed

- **ShellRunner default timeout** increased from 10 s → 30 s — fixes package managers
  that failed to list installed packages due to timeout (winget list, pip list, scoop list)
- **Explicit longer timeouts** for slow list commands: winget list/upgrade (60 s),
  scoop list/status (30 s), pip list (30 s), PowerShell Get-Module (30 s)
- **Scoop detection** now falls back to PATH check when `~/scoop/shims/scoop.ps1`
  doesn't exist (non-default Scoop installations)
- **GUI log panel** now visible by default (was hidden)

#### Added

- **Menu item icons** — all 8 Tools menu entries now show 16×16 tool-specific icons
  (Scoop, PowerShell, pip, WinGet, Chocolatey, Tool Versions, Windows Health, Marketplace)
- **MarketplaceIcon** — new purple shopping-bag icon for the Tweak Pack Marketplace dialog
- **AppIcons.MenuBitmap()** — generates 16×16 bitmaps for ToolStripMenuItem images

#### Documentation

- Updated `copilot-instructions.md`: full TweakDef model (all fields), TweakKind table
  (8 variants with fields used), TweakResult table (7 outcomes), GUI details
- Updated `workspace.instructions.md`: TweakKind table with fields used per kind
- Updated `lessons-learned.instructions.md`: HasOperations gate, coverage patterns,
  Assert.Contains ambiguity lessons
- Updated `testing.instructions.md`: TweakKind coverage by kind, actual coverage data
  (94.9% line), intentionally untested components
- Updated `Roadmap.md`: Sprint 11 entry, marked completed backlog items (self-contained
  publish, parallel StatusMap, winget manifest v3.2.0, GitHub Releases automation)
- Total: **2,316 tweaks**, **700 tests** (571 Core + 58 CLI + 71 GUI)

## [3.2.0] — 2025-07-22

### Added

- **Windows Health & Maintenance manager** — 19 system health commands
  (DISM, SFC, disk cleanup, network reset, chkdsk, power reports) with
  full dialog UI, admin badge, progress bar, and per-command log
- **320 new tweaks** across expanded modules, bringing total to **2 301 tweaks**
  across **89 categories**:
  - 8 modules expanded to ~20 tweaks each: EventLogging, SsdOptimization,
    AppCompatibility, BrowserCommon, Security, UserAccount, SystemRestore,
    ScheduledTaskTweaks
  - 6 first-wave modules (57 tweaks): CommandLine, PowerShell, Hardening,
    Developer, MemoryOptimization, ScheduledTaskTweaks
  - 4 second-wave modules + expanded Developer/Hardening (+71 tweaks)
  - Wave 3: SsdOptimization, AppCompatibility, UserAccount, BrowserCommon
  - Wave 4: WindowsRecall, ProxyVpn, EventLogging, SystemRestore
  - MemoryOptimization 6→15, PowerShellTweaks 9→15
- **RegistryHives.cs** constant strings for common registry paths
- **77 new expansion tests** — total now **556 tests** (435 Core + 52 CLI + 69 GUI)
- **AppIcons.WindowsHealthIcon** (green shield with white cross)
- **Windows Health** menu item in Tools menu

### Fixed

- **Duplicate tweak ID crash** — `evtlog-enable-powershell-module-logging`
  appeared at lines 80 and 302 in EventLogging.cs; renamed second to
  `evtlog-enable-powershell-transcription`
- **3 duplicate sec- IDs** between Security.cs and Defender.cs: renamed to
  `sec-enforce-lsa-ppl`, `sec-block-wdigest-caching`, `sec-enforce-sehop`
- **Test hangs** — added `tests/.runsettings` with `MaxCpuCount=1`,
  capped `maxParallelThreads` to 4 in all xunit.runner.json

### Changed

- Performance optimizations: tag index, search, HardwareInfo parallelization,
  Analytics caching, MainForm filter dedup, UpdateCounters single-pass
- Updated all documentation with current statistics (2 301 tweaks, 89 categories,
  556 tests)

### Sprint 10 — Test Deepening & Engine Coverage

- 36 new tests across TweakEngine, RegistrySession, and Services
- TweakEngine: snapshot round-trip (SaveSnapshot, LoadSnapshot, RestoreSnapshot),
  ExportJson validation, TweaksByTag, TweaksByScope, GetScope, Freeze/CategoryCounts/ScopeCounts,
  TweaksForProfile, WindowsBuild
- RegistrySession: ReadValue, ReadString, KeyExists, ValueExists, ListSubKeys,
  ListValueNames, ParsePath abbreviated/edge cases
- Services: HardwareInfo (DetectHardware, Summary, SuggestProfile, IsEdgeInstalled),
  CorporateGuard (IsCorporateNetwork, Status, IsGpoManaged, ClearCache)
- Total: **2,316 tweaks**, **700 tests** (571 Core + 58 CLI + 71 GUI)

### Sprint 9 — Test Coverage & Analytics Integration

- CLI: `update <id>` command — runs UpdateAction or falls back to Apply
- CLI: Analytics integration — `RecordSession()` on startup, `Flush()` on exit,
  `RecordApply/Remove/Error` in all action methods (RunAction, RunApplyProfile,
  RunCategoryAction, RunImportJson, RunUpdate)
- 17 new tests: Filter multi-criteria (4), Update method (3), complex dependency
  graphs (3), Analytics persistence (5), CLI update parsing (2)
- Total: **2,316 tweaks**, **658 tests** (529 Core + 58 CLI + 71 GUI)

### Added

- **System theme auto-detection** — GUI follows Windows dark/light mode on startup,
  `Theme.DetectSystemTheme()` reads `AppsUseLightTheme` registry value
- **Percentage progress bar** — batch apply/remove shows percentage instead of
  indeterminate marquee; `SetBusy(bool, string?, int)` + `SetProgress(int)`
- **Tray icon** — minimize to system tray with context menu (Show/Exit);
  `NotifyIcon` with app icon, restore on double-click
- **FrozenDictionary performance** — `TweakEngine.Freeze()` builds `FrozenDictionary`
  for O(1) ID lookups, caches sorted categories, category counts, scope counts;
  called automatically at end of `RegisterBuiltins()`
- **62 plugin system tests** — `PluginTests.cs` covering PackLoader (load, validate,
  SHA-256, all 12 RegOp kinds, validation failures), PackManager (install/uninstall
  lifecycle, version comparison), PackIndex (round-trip), TweakEngine `RegisterPack`
  integration, and Locale (German translations, format args, file loading)
- **Built-in German locale** — 48 translated UI strings in `Locale.cs`,
  `AvailableLocales` property, `SetLocale()` uses built-in locale as base
- **2 system theme tests** in ThemeTests.cs: `DetectSystemTheme_ReturnsValidThemeKey`,
  `DetectSystemTheme_ThemeKeyExistsInAvailable`

### Changed

- **Test parallelism** — `.runsettings` `MaxCpuCount` 1→4 (4 assemblies parallel),
  `TestSessionTimeout` 300s→60s; all `xunit.runner.json` now include
  `longRunningTestSeconds: 5`
- **ShellRunner.DefaultTimeout** reduced from 30s to 10s; `ToolVersionChecker`
  per-tool timeout 5s, per-probe timeout 2s
- **`ScopeCounts()`** now uses `_tweaksByScope` dictionary (O(3)) instead of
  O(n=2301) GroupBy scan
- **`Categories()`** returns cached sorted array instead of re-sorting on every call
- **PackageManagerValidationTests** — removed `OperationCanceledException` swallow
  that silently passed when tool checks timed out
- `.gitignore` pattern changed from `RegiLattice.log` to `*.log`
- Updated Roadmap with Sprint 1–5 completion status
- Total tests: **641** (514 Core + 56 CLI + 71 GUI), all passing

### Sprint 7 — Engine Optimization & Tweak Expansion

- Clean up stale tracking files (current-ids.txt regenerated, missing/removed deleted)
- Profile RegisterBuiltins() performance: 37ms for 2,301 tweaks (budget 500ms)
- Add 4 perf benchmark tests (startup, search, freeze, caching)
- Add 15 new tweaks: 5 Windows Recall, 5 Debloat, 5 Proxy & VPN
- Total: 2,316 tweaks, 89 categories

### Sprint 8 — Consolidation, Validation & CLI Enhancements

- Untrack archive/ (151 files, 84,575 line deletions) + current-ids.txt from git
- Delete `.mypy_cache` (16 MB) and `__pycache__` (3 MB) from disk
- Core: `ValidateTweaks()` — checks empty IDs/Labels/Categories, broken DependsOn, circular deps
- Core: `ResolveDependencies(id)` — topological-sort dependency resolution
- Core: `Dependents(id)` — reverse dependency lookup
- Core: `ApplyBatch`/`RemoveBatch` progress overloads with `Action<int,int,string,TweakResult>` callback
- CLI: ANSI colour output for status display (Green/Red/Yellow/Dim)
- CLI: `--depends-on <id>` command showing deps, reverse deps, and resolved chain
- CLI: `--no-color` flag + auto-detect `Console.IsOutputRedirected`
- CLI: version bump 3.0.0 → 3.2.0, `RunValidate` delegates to engine
- 15 new tests (11 Core + 4 CLI): validation, dep resolution, batch progress, CLI flags
- Total: **2,316 tweaks**, **641 tests** (514 Core + 56 CLI + 71 GUI)

## [3.1.5] — 2025-07-20

### Added

- **49 DetectOps additions** across 18 tweak modules — every registry-based tweak now has
  detection logic (CheckDword, CheckString, or CheckMissing) so `StatusMap()` and the GUI
  status column report accurate applied/not-applied state
  - Backup (1), Boot (3), ContextMenu (4), Defender (2), DevDrive (1), Explorer (1),
    GPU (1), IndexingSearch (2), LockScreen (4), MsStore (1), Network (2), NightLight (2),
    Office (2), OneDrive (1), Performance (1), RealVnc (2), Screensaver (1),
    CloudStorage (14), Startup (3)

### Fixed

- **16 broken TweakDef headers** restored after multi-edit consumed `new TweakDef` openers
  — all 1 981 tweaks register correctly again
- **Build clean** — 0 warnings, 0 errors with `-warnaserror`

### Changed

- Version bump to 3.1.5 across csproj, winget manifests, and WiX installer

## [3.0.0] — 2025-07-20

### ⚠️ BREAKING: Quality audit — removed 468 non-functional tweak stubs

v3.0.0 is the first verified-clean release of the C# codebase. Every remaining tweak
has functional apply/remove/detect operations. Non-functional metadata-only stubs
that silently returned "Applied" without performing any action have been removed.

### Removed

- **468 non-functional tweak stubs** across 66 modules — these had metadata (Id, Label,
  Category, Tags) but no ApplyOps, RemoveOps, DetectOps, or Action delegates. The engine
  silently returned `TweakResult.Applied` for these without performing any registry changes.
- Tweak count reduced from ~1 828 to **1 360 verified functional tweaks**; subsequently
  expanded back to **1 981** through multiple tweak addition campaigns

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
- **129 xUnit tests** (93 Core + 36 GUI) at initial 2.0.0 release;
  expanded to **203 tests** (112 Core + 52 CLI + 39 GUI) by v3.1.5
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
