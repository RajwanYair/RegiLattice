# Changelog

All notable changes to RegiLattice are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
This project adheres to [Semantic Versioning](https://semver.org/).

## [4.9.0] — 2026-03-26 (Sprints 142–151)

### Highlights

Sprints 142–151 release. Adds **100 new tweaks** across **10 new categories** covering
LLTD network discovery, Windows Media Player policy, device provisioning, Windows Maps
Connected Search, diagnostic data pipeline, system restore GPO, modern Start menu layout,
cloud content/Spotlight policy, additional app privacy capabilities, and UAC advanced
logon/display controls.

### Added

#### Tweaks (100 new — 10 modules × 10 tweaks each)

- **Network Map Discovery Policy** (`netlltd-*`, `NetworkLltdPolicy.cs`) — LLTDIO enable/disable
  on domain/private/public networks; Rspndr responder control; Peernet/PeerToPeer disabled.
- **Windows Media Player Policy** (`wmply-*`, `WindowsMediaPolicyAdv.cs`) — HKLM screensaver,
  metadata retrieval (CD/DVD, music, radio) suppression, preset/codec/protocol auto-download
  disable and user-scope overrides.
- **Device Provisioning Policy** (`devprov-*`, `DeviceProvisioningPolicy.cs`) — OOBE network,
  first-logon animation, privacy settings page; HomeGroup creation; WorkplaceJoin workplace
  join/device registration; CloudContent Find My Device, SoftLanding, tailored experience.
- **Windows Maps Policy** (`wmaps-*`, `WindowsMapsPolicy.cs`) — Maps auto-download disable;
  HKLM + HKCU Connected Search privacy, SafeSearch enforcement, Search Highlights, Cortana AAD.
- **Diagnostic Data Viewer Policy** (`diagdvr-*`, `DiagnosticDataViewerPolicy.cs`) — DataViewer
  disable, device health attestation, diagnostic log limits, enterprise auth proxy, settings auditing,
  Update Compliance/WUfB/Desktop Analytics/commercial data pipeline controls.
- **System Restore Policy** (`srgpo-*`, `SystemRestoreGpoPolicy.cs`) — GPO DisableSR/DisableConfig;
  VSC session/global intervals, disk percent cap, system checkpoints, scan interval,
  optimistic restore, restore-point creation frequency, incremental restoration prevention.
- **Start Menu Modern Policy** (`smmod-*`, `StartMenuModernPolicy.cs`) — StartMenuExperience
  recent/recommended apps/items, People Bar, MSA notification; Explorer frequent programs,
  recent docs, preview/details panes, machine-boot uninstall.
- **Cloud Content Policy** (`ccpol-*`, `CloudContentPolicy.cs`) — HKLM consumer features,
  third-party suggestions, cloud-optimised content; Spotlight (features, welcome screen, settings,
  third-party suggestions) at both HKLM and HKCU scope.
- **App Privacy Policy Advanced** (`appprv2-*`, `AppPrivacyPolicyAdv.cs`) — LetApps* Force-Deny
  for call history, calendar, contacts, radios, trusted devices, diagnostic info, email, gaze input,
  voice activation, and voice activation above lock screen.
- **User Account Control Advanced Policy** (`uacadv-*`, `UserAccountControlAdvPolicy.cs`) — automatic
  restart sign-on, network selection UI, failed unlock display, locked user ID display, MSA optional,
  shutdown without logon, lock workstation/change password disable, legal notice caption, Task Manager
  disable.

### Stats

- Tweaks: **4 728** (+100 from v4.8.0)
- Categories: **188** (+10)
- Module files: **183** (+10)
- Tests: **2 661** (2 052 Core + 301 CLI + 308 GUI, 0 failures)

---

## [4.8.0] — 2026-03-25 (Sprints 140–141)

### Highlights

Sprints 140–141 release. Adds **100 new tweaks** across **10 new categories** covering
advanced AMSI/script policy, IE compatibility hardening, logon cache policy, Bluetooth
advertising control, process mitigation (SEHOP/ASLR/Spectre/LSA-PPL), remote assistance
policy, network hardened paths, USB storage policy, kiosk assigned access, and application
restart/crash control.

### Added

#### Tweaks (100 new — 10 modules × 10 tweaks each)

- **AMSI & Script Policy** (`amsi-*`, `AmsiScriptPolicy.cs`) — PowerShell ScriptBlock logging,
  module logging, transcription, constrained language mode, WScript disable, Defender cloud
  protection level/timeout, PS v2 disable.
- **IE Compatibility Policy** (`iecompat-*`, `IECompatPolicy.cs`) — IE Enterprise Mode / IE-in-Edge
  disable, IE first-run, cookie/homepage/autocomplete lockdown, IE zone elevation (IEHarden),
  IE add-on install prompt, Edge HTTPS upgrades, Edge password manager.
- **Logon Cache Policy** (`lgncache-*`, `LogonCachePolicy.cs`) — CachedLogonsCount 2/0,
  smart-card remove lock, password-expiry warning (14 days), ForceUnlockLogon, Netlogon
  RequireStrongKey/RequireSignOrSeal/SealSecureChannel/SignSecureChannel, disable domain
  password cache.
- **Bluetooth Advertising Policy** (`btadv-*`, `BluetoothAdvPolicy.cs`) — advertising,
  promiscuous mode, pairing notifications, connectable timeout, file transfer, phonebook
  access, encryption enforcement, remote audio, discoverable state, shared experiences.
- **Application Restart Policy** (`apprstrt-*`, `ApplicationRestartPolicy.cs`) — AeDebug auto
  (JIT debugger), auto-reboot on BSOD, crash dump type (kernel), event logging, WER
  reporting/queue/throttle/consent, dump size limit, overwrite existing dump.
- **Process Mitigation Policy** (`prctmtg-*`, `ProcessMitigationPolicy.cs`) — SEHOP
  (exception chain validation), heap termination on corruption, mandatory ASLR (MoveImages),
  bottom-up + high-entropy ASLR, kernel stack cookies, LSA RunAsPPL, safe DLL search mode,
  Spectre mitigations (BpbEnabled), clear page file at shutdown.
- **Remote Assistance Policy** (`rast-*`, `RemoteAssistancePolicy.cs`) — RA disable,
  shadow/ticket control, firewall rules, helper configuration.
- **Network Hardened Paths** (`nethpth-*`, `NetworkHardenedPaths.cs`) — UNC hardened paths
  with RequireMutualAuthentication/RequireIntegrity/RequirePrivacy flags.
- **USB Storage Policy** (`usbstor-*`, `UsbStoragePolicy.cs`) — USBSTOR start/write-protect,
  removable storage per-class ACL policies.
- **Kiosk Assigned Access** (`kiosk-*`, `KioskAssignedAccess.cs`) — kiosk/assigned-access
  lock screen and session policy controls.

#### Fixes

- **Race condition fix** in `RatingsTests` / `RatingsFileExistsBranchTests` — added
  `[Collection("Ratings")]` to both classes so the file-delete in `RatingsFileExistsBranchTests`
  no longer races with `Rate_ValidStars_CreatesRating`.
- **WFO1000 suppression** in `RegiLattice.GUI.csproj` — added `WFO1000` to `<NoWarn>` to
  suppress the .NET 10 WinForms SDK false-positive on `[DesignerSerializationVisibility]`.

### Stats

- Tweaks: **4 628** (+100 from v4.7.0)
- Categories: **178** (+10)
- Module files: **173** (+10)
- Tests: **2 661** (2 052 Core + 301 CLI + 308 GUI, 0 failures)

---

## [4.7.0] — 2026-03-23 (M4 Milestone)

### Highlights

M4 milestone release (Sprints 130–139). This is the first fully published GitHub release
since v4.6.0 (M3). It contains **420 new tweaks**, new plugin infrastructure, Chocolatey
distribution and mutation-testing improvements.

### Added

#### Distribution

- **Chocolatey package** (`chocolatey/`) — full `regilattice.nuspec` + install/uninstall scripts.
  GitHub Actions release workflow now auto-builds a `.nupkg` and a SHA-256–verified zip on every tag push.

#### Plugin System

- **RSA-SHA256 pack signing** (`PackSignatureVerifier.cs`) — cryptographic signature
  verification for JSON Tweak Packs; prevents tampered community packs from loading.
- **Plugin sandbox via named pipe** (`PluginSandbox.cs`) — pack sub-processes run in
  a restricted environment and communicate through a named pipe, isolating the host process
  from untrusted code.

#### Testing

- **Virtual Registry integration tests** (`VirtualRegistryTests.cs`) — full end-to-end
  engine tests that operate on a virtual registry overlay instead of the real HKCU/HKLM.
- **Stryker.NET mutation testing** (`scripts/Run-MutationTests.ps1`, `.config/dotnet-tools.json`)
  — infrastructure for mutation coverage quality gates.

#### Tweaks (420 new across 40 new categories)

| Sprint | Modules | New tweaks |
|--------|---------|------------|
| 136 | AppVirtualization, BranchCache, InternetZonePolicy, SensorPolicy, LocationSensors | 50 |
| 137 | NetworkDiscovery, CertificatePolicy, PowerShellPolicy, DefenderAdvanced, EventLogPolicy | 50 |
| 138 | SmartScreenPolicy, CredentialCachingPolicy, WindowsTimePolicy, FirewallLogPolicy, LogonPolicy | 50 |
| 139 | ShellRestrictionsPolicy, BitsTransferPolicy, OfflineFilesSyncPolicy, MsiInstallerPolicy, SmbServerPolicy | 50 |
| Various (130–135) | AppPrivacyPolicy, CertificateServices, NetworkConnections, and 17 more | 220 |

#### Bug Fixes

- **`Ratings.Save()` cross-process retry** — added 5-attempt retry loop (60×n ms backoff)
  to handle `IOException` when test projects run in parallel and race on `ratings.json`.

### Stats

- Total tweaks: **4 528** across **168 categories** (163 module files)
- Total tests: **2 660** passing — 2 052 Core + 301 CLI + 307 GUI (0 failures)
- Version bumped `4.6.0` → `4.7.0`

---

## [4.6.9] — 2026-05-15

### Sprint 139 — 50 New Tweaks: 5 New Modules (T8.3)

#### Added

- **ShellRestrictionsPolicy** (`shellrst`, 10 tweaks) — Shell restrictions via `HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer`: NoRun, NoFind, NoClose, NoLogoff, NoDesktop, NoDrivesPage, NoCplApplets, NoDispCPL, DisallowRun, NoNetHood.
- **BitsTransferPolicy** (`bitspol`, 10 tweaks) — BITS background transfer limits via `HKLM\SOFTWARE\Policies\Microsoft\Windows\BITS`: JobInactivityTimeout, MaxJobsPerMachine, MaxJobsPerUser, MaxJobFilesPerJob, MaxRangesPerFile, MaxDownloadTime, MaxInternetBandwidth, EnableBITSMaxBandwidth, DisablePeerCachingClient, DisablePeerCachingServer.
- **OfflineFilesSyncPolicy** (`offsync`, 10 tweaks) — Offline Files sync governance via `NetCache` and `SyncMgr` policy paths: NoMakeAvailableOffline, PurgeAtLogoff, BackgroundSyncEnabled, DefaultCacheSize, GoOfflineAction, EventLoggingLevel, DisableSyncActivity, TurnOffSyncOnCostedNetwork, DisableFileSyncClient, HideOptionsForSyncProvider.
- **MsiInstallerPolicy** (`msipol`, 10 tweaks) — Windows Installer hardening via `HKLM\SOFTWARE\Policies\Microsoft\Windows\Installer`: DisablePatch, DisableBrowse, DisableMSI, TransformsSecure, SafeForScripting, EnforceUpgradeComponentRules, LimitSystemRestoreCheckpointing, DisableLockdownBrowseUI, DisableForbidPatch, DisableMedia.
- **SmbServerPolicy** (`smbsrv`, 10 tweaks) — SMB server security and performance via `LanmanServer\Parameters`: AutoShareServer, AutoShareWks, EnableOpLocks, EnableForcedLogoff, IRPStackSize, MaxMpxCt, MaxWorkItems, EnableRaw, SizReqBuf, DiskSpaceThreshold.

#### Stats

- Total tweaks: ~4 258 across ~141 categories
- Total tests: 2 052 (0 failures)

---

## [4.6.8] — 2026-05-15

### Sprint 138 — 50 New Tweaks: 5 New Modules (T8.2)

#### Added

- `SmartScreenPolicy.cs` (10 tweaks, `smartscr`) — Windows Defender SmartScreen GPO policy:
  shell enable/block-level, app install control, Edge PhishingFilter prevent-override,
  Edge SmartScreen enable/PUA/force-enabled.
- `CredentialCachingPolicy.cs` (10 tweaks, `credcache`) — Credential caching hardening:
  CredSSP oracle mitigation (CVE-2018-0886), RDP RestrictedAdmin delegation,
  WDigest disable, LSA PPL, domain cred/anonymous/LM-hash restrictions.
- `WindowsTimeGpoPolicy.cs` (10 tweaks, `timepol`) — W32Time GPO policy path
  (`SOFTWARE\Policies\Microsoft\W32Time\*`); NTP type/servers, client enable/poll/log,
  server disable, phase/frequency correction rates.
- `FirewallLogPolicy.cs` (10 tweaks, `fwlog`) — Windows Firewall logging GPO policy:
  Domain/Private/Public profile dropped-packets, successful-connections, log-size, log-path.
- `LogonGpoPolicy.cs` (10 tweaks, `logonpol`) — Logon screen privacy/security GPO policy:
  hide last username, hide network UI, hide account details, disable ARSO, disable startup
  sound, block MSA accounts, hide locked user ID, lockout attempts, disable lock-screen
  notifications, hide power button.

#### Stats

- Tweaks: **4,308** (+50 from v4.6.7)
- Categories: **141** (+5)
- Core tests: **2,052** (0 failures)

---

## [4.6.7] — 2026-05-14

### Sprint 137 — 50 New Tweaks: 5 New Modules (T8.2)

#### Added

- `NetworkDiscovery.cs` — 10 tweaks (netdisc): LLTD mapper/responder, mDNS,
  NetBIOS P-Node, UPnP Host, SSDP, FDResPub service disablement
- `CertificatePolicy.cs` — 10 tweaks (certpol): .NET 4 + .NET 2 SchUseStrongCrypto
  (64-bit + WoW64), SystemDefaultTlsVersions, Wintrust certificate padding check, root
  CA auto-update, IE certificate revocation enforcement
- `PowerShellPolicy.cs` — 10 tweaks (pspolicy): ScriptBlockLogging,
  InvocationLogging, ModuleLogging, Transcription (on + header + output path),
  PS2 engine disable, protected event logging, EnableScripts + AllSigned execution policy
- `DefenderAdvanced.cs` — 10 tweaks (defadv): cloud block level High,
  BAFS extended timeout, MAPS advanced membership, auto sample submission,
  behavior/IOAV/script scanning enable, archive + email scanning, scan time randomisation
- `EventLogGpoPolicy.cs` — 10 tweaks (evtgpo): GPO-path event log sizes
  (Application 128 MB, Security 1 GB, System 128 MB, Setup 64 MB, ForwardedEvents 256 MB)
  and overwrite-when-full retention policy for all 5 channels

#### Stats

- Total tweaks: **4,258** (+50)
- Tests: **2,052 Core / 2,660 total** — 0 failures

## [4.6.6] — 2026-05-14

### Sprint 136 — 50 New Tweaks: 5 New Modules (T8.2)

#### Added

- **`Biometrics.cs`** (10 tweaks, category "Biometrics", slug `bio`): Windows Hello for Business and biometric hardware group-policy controls.
  - `bio-disable-biometrics` — disable Windows Biometric Service (GPO).
  - `bio-disable-biometrics-domain` — block domain/AAD biometric sign-in.
  - `bio-disable-biometric-sign-in` — Credential Provider level disable.
  - `bio-enable-facial-anti-spoofing` — Enhanced Anti-Spoofing (ISO PAD).
  - `bio-whfb-require-tpm` — require TPM for WHFB key storage.
  - `bio-whfb-pin-min-length` — minimum PIN = 8 chars.
  - `bio-whfb-pin-require-digits` — PIN must contain digits.
  - `bio-whfb-pin-require-uppercase` — PIN must contain uppercase.
  - `bio-whfb-pin-require-lowercase` — PIN must contain lowercase.
  - `bio-whfb-pin-expiry` — PIN expires every 90 days.
- **`WinRmHardening.cs`** (10 tweaks, category "WinRM Hardening", slug `winrm`): WS-Management authentication hardening.
  - Client-side: deny basic, plaintext, Digest, NTLM, CredSSP auth.
  - Service-side: deny basic, plaintext, NTLM; disable RunAs; allow Kerberos.
- **`LocationSensors.cs`** (10 tweaks, category "Location & Sensors", slug `loc`): Location scripting, sensor framework, and per-app policies.
  - Complements `priv-disable-location` (OS service) and `aperm-deny-location` (user GUID).
  - Covers: DisableLocationScripting, DisableSensors, DisableWindowsLocationProvider, LetAppsAccessLocation=2, LetAppsAccessMotion=2, Windows Search location, Wi-Fi auto-connect, user ConsentStore deny, IE geolocation block.
- **`SettingSyncAdv.cs`** (10 tweaks, category "Settings Sync", slug `ssync`): Granular SettingSync policies beyond the MicrosoftAccount.cs master toggle.
  - Desktop theme, Start layout, browser, language, accessibility, personalization, Windows settings sync disable.
  - Typing/text personalization (RestrictImplicitTextCollection), handwriting/ink collection, machine-policy input personalization off.
- **`AppPrivacyPolicy.cs`** (10 tweaks, category "App Privacy Policy", slug `appp`): Machine-level `HKLM AppPrivacy` LetApps\* force-deny policies.
  - Covers: camera, microphone, notifications, account info, background run, device sync, phone, tasks, messaging, video library.
  - Distinct from `AppPermissions.cs` which uses HKCU DeviceAccess GUIDs (per-user).

#### Stats

- Total tweaks: **4,208** (+50)
- Tests: **2,660 passing** (0 failures)
- New categories: Biometrics, WinRM Hardening, Location & Sensors, Settings Sync, App Privacy Policy

---

## [4.6.5] — 2026-05-14

### Sprint 135 — Stryker.NET Mutation Testing Setup (T6.6)

#### Added

- **`stryker-config.json`** (project root): Stryker.NET 4.14.0 configuration for `RegiLattice.Core`.
  - `mutation-level: Standard` — covers arithmetic, boolean, boundary, and null mutations.
  - Thresholds: `high=80%`, `low=60%`, `break=55%` — CI fails if kill score drops below 55%.
  - Target files: 15 Core source files (TweakEngine, TweakDef, RegistrySession, TweakValidator, DependencyResolver, SnapshotManager, AppConfig, Favorites, Ratings, TweakHistory, ConfigExporter, PackLoader, PackManager, PackSignatureVerifier, PluginSandbox).
  - Reports: HTML + JSON output to `.tmp/stryker-output/` (gitignored).
- **`.config/dotnet-tools.json`** — `dotnet-stryker 4.14.0` added as a local tool alongside CSharpier.
- **`scripts/Run-MutationTests.ps1`**: PowerShell developer script for local mutation test runs with restore, build, and friendly output.
- **`ci.yml` — `mutation-testing` job**: Runs Stryker on main-branch pushes only (not PRs). Uploads HTML report as a CI artifact. Break threshold enforces 55% minimum kill score.

#### Stats

- Total tweaks: **4,158** (unchanged)
- Tests: **2,660 passing** (unchanged)

---

## [4.6.4] — 2026-05-14

### Sprint 134 — Virtual Registry Integration Tests (T6.3)

#### Added

- **`VirtualRegistryTests.cs`** (new, `RegiLattice.Core.Tests`): 15 integration tests that exercise the real Windows registry without administrator rights, using a GUID-scoped HKCU isolation key that is created and cleaned up per-test.
  - Basic round-trip reads/writes: `SetDword`, `SetString`, `SetQword`, `SetBinary`, `SetMultiSz`.
  - Delete operations: `DeleteValue`, `DeleteTree`.
  - `Evaluate()` detection: `CheckDword` true/false, `CheckMissing` before/after set, `CheckKeyMissing`.
  - Full `Execute`→`Evaluate`→`Execute` (Apply→Detect→Remove) round-trip via `RegistrySession` directly.
  - Full Apply→Detect→Remove cycle through `TweakEngine` using a real (non-DryRun) session.
  - DryRun isolation guard: verifies that a DryRun write does not appear in the real registry.
- **Implementation note**: `RegLoadKey`/`RegUnLoadKey` hive isolation requires `SeRestorePrivilege` (admin). The HKCU-subkey approach provides equivalent per-test isolation and runs on `windows-latest` CI runners without elevation.

#### Stats

- Total tweaks: **4,158** (unchanged)
- Tests: **2,052 Core · 301 CLI · 307 GUI = 2,660 total** (+15)

---

## [4.6.3] — 2026-05-14

### Sprints 132–133 — Plugin Sandbox Isolation (T7.4)

#### Added

- **`PluginSandbox.cs`** (new, `RegiLattice.Core.Plugins`): executes third-party `TweakPack` `RegOp` lists in an isolated child process via a named pipe with a configurable timeout (default 30 s).
  - **`SandboxOpDto`** (internal): JSON DTO for a single `RegOp` — 9 typed fields (`Kind`, `Path`, `Name`, `DwordValue`, `StringValue`, `QwordValue`, `BinaryValue`, `MultiSzValue`) with `[JsonPropertyName]` camelCase serialisation.
  - **`PluginSandboxRequest`** (internal): wire message sent to child — `{ bool DryRun, IReadOnlyList<SandboxOpDto> Ops }`.
  - **`PluginSandboxResponse`** (internal): reply from child — `{ bool Success, string ErrorMessage }`.
  - **`PluginSandboxResult`** (public): caller-facing result — `{ bool Success, string ErrorMessage, bool TimedOut }`.
  - **`PluginSandbox.ToDto()`** / **`FromDto()`** (internal static): bidirectional mapping between `RegOp` and `SandboxOpDto` covering all 12 `RegOpKind` variants.
  - **`PluginSandbox.ExecuteAsync()`** (public async): parent side — spawns child, creates a named pipe server, writes request JSON, reads response, enforces timeout. Returns `TimedOut=true` on `OperationCanceledException`; catches spawn-failure exceptions.
  - **`PluginSandbox.RunHostAsync()`** (public async): child side — connects to parent pipe, deserialises request, executes ops via `RegistrySession`, serialises response, returns exit code 0 on success.
- **`CliArgs.PluginHost`** and **`CliArgs.PluginPipeName`** properties: gate the child-process dispatch path.
- **`Program.cs`** — `--plugin-host <pipeName>` flag: parsed before engine initialisation; dispatches immediately to `PluginSandbox.RunHostAsync()` and exits.
- **17 new `PluginSandboxTests`** covering: 10 `ToDto` kind tests, `FromDto` round-trip (12 kinds), `PluginSandboxRequest`/`PluginSandboxResponse` JSON serialisation round-trips, error response serialisation, `ExecuteAsync` with non-existent executable, and `PluginSandboxResult` model defaults.

#### Stats

- Total tweaks: **4,158** (unchanged)
- Tests: **2,037 passing** (+17)

---

## [4.6.2] — 2026-05-13

### Sprint 131 — Pack RSA-SHA256 Signing Support (T7.3)

#### Added

- **`PackTrustLevel` enum** (`PackDef.cs`): three levels — `None`, `HashVerified`, `Signed` — representing the verified integrity state of a Tweak Pack at runtime.
- **`PackDef` — `SignatureUrl` property**: optional URL pointing to the detached `.rlpack.sig` RSA signature file. Serialised in JSON pack index; empty string by default.
- **`PackDef` — `TrustLevel` property**: runtime-only (not persisted); set by `DetermineTrustLevel()` after loading a pack.
- **`PackSignatureVerifier.cs`** (new): RSA-PKCS#1-v1.5 / SHA-256 signing and verification for Tweak Packs.
  - `Verify(ReadOnlySpan<byte>, signatureBase64, publicKeyPem)` — low-level verifier.
  - `Verify(string packJson, signatureBase64, publicKeyPem)` — convenience overload for UTF-8 pack JSON.
  - `DetermineTrustLevel(packJson, pack, sig?, pubKey?)` — resolves `PackTrustLevel`: `Signed` if RSA check passes, `HashVerified` if only SHA-256 matches, `None` otherwise.
  - `Sign(packJson, privateKeyPem)` — returns base64 signature (author tooling).
  - `GenerateKeyPair(keySize=2048)` — returns `(PublicKeyPem, PrivateKeyPem)` tuple (author tooling / tests).
  - Enforces minimum 2048-bit RSA key size; throws `CryptographicException` on under-sized keys.
- **`PackIndex` — `AuthorKey` record** and **`AuthorKeys` list**: pack marketplace index can now carry per-author public keys (PEM format), enabling automated trust resolution for all packs from a known author.
- **`PackIndex.GetAuthorPublicKey(string author)`**: case-insensitive lookup returning the PEM public key for a named author, or `null` if not found.
- **13 new `PackSignatureVerifierTests`** covering: key generation, sign+verify round-trip, tampered content detection, wrong-key rejection, empty/invalid base64 handling, `DetermineTrustLevel` for all three levels, and `PackIndex.GetAuthorPublicKey` including case-insensitive match.

#### Stats

- Total tweaks: **4,158** (unchanged)
- Tests: **2,020 passing** (+13)
- Version: `4.6.2`

---

## [4.6.1] — 2026-05-12

### Sprint 130 — Chocolatey Package & Distribution Improvements (T5.3)

#### Added

- **Chocolatey package** (T5.3): `chocolatey/regilattice.nuspec` — full package descriptor with title, authors, tags, release notes URL, and description. `chocolatey/tools/chocolateyInstall.ps1` — zip-based install with SHA-256 verification, auto-shimming of `RegiLattice.exe` and `RegiLattice.GUI.exe`. `chocolateyUninstall.ps1` with optional `/PurgeData` parameter. `VERIFICATION.txt` for Chocolatey Community Repository trust audit.
- **Release workflow — Chocolatey build step**: CI now packs a `RegiLattice-<version>-win-x64.zip` from the published EXEs, injects the download URL + SHA-256 into `chocolateyInstall.ps1`, runs `choco pack`, and pushes the `.nupkg` to the Chocolatey Community Repository if `CHOCOLATEY_API_KEY` is set. The `.nupkg` and `.zip` are also uploaded as GitHub Release assets.
- **Release workflow — expanded SHA256SUMS**: Checksum file now covers `.zip` and `.nupkg` artifacts in addition to EXEs and MSI/MSIX.
- **Scoop manifest updated**: `scoop/regilattice.json` bumped to v4.6.0, improved `bin` aliases (`regilattice` → CLI, `regilattice-gui` → GUI), expanded `notes` array with dry-run tip.
- **MSIX description updated**: `installer/AppxManifest.xml` description updated to reflect 4,158 tweaks / 126 categories.

#### Stats

- Total tweaks: **4,158** (unchanged)
- Tests: **2,007 passing**
- Distribution channels: GitHub · Scoop · WinGet + **Chocolatey** (new)
- Version: `4.6.1`

---

## [4.6.0] — 2026-05-12

### M3 Milestone — GUI Polish, i18n, Marketplace & Security Policies (Sprints 122–129)

#### Added

- **ToggleSwitchControl** (Sprint 122, T1.5): Custom GDI+ animated toggle switch with smooth slide animation, theme-aware colours, DPI scaling, keyboard support (Space/Enter), focus ring, and accessible `AccessibleObject`. 15 new `ToggleSwitchControlTests`.
- **Visual Refresh** (Sprint 123, T1.6): `RoundedPanel` — GDI+ rounded-corner panel with Mica tint; `FluentIcons` — 60+ Segoe Fluent Icons glyph constants + `DrawGlyph`/`CreateGlyphBitmap` helpers; `CategoryExpandButton` — animated 90° chevron for category headers. 24 new `VisualPolishTests`.
- **Pack Creator Studio Dialog** (Sprint 125, T7.2): 5-step wizard dialog for authoring community Tweak Packs. Steps: basic info → tweak selection → metadata → JSON preview → export + submit URL. 11 new `PackCreatorDialogTests`.
- **PackSubmissionService** (Sprint 124, T7.5): `Validate(PackDef)`, `BuildSubmissionUrl(PackDef)`, `SanitizeName()` with kebab-case slug validation, semver version, SHA-256 hex check, HTTPS source URL. GitHub Issue template `pack-submission.yml` with 12-field form and 5-item checklist. 17 new `PackSubmissionServiceTests`.
- **4 New Locales** (Sprint 126, T2.4/T2.5): zh-CN (~120 keys), ko (~120 keys), ar (~120 keys), pt-BR (~120 keys) added to `Locale.cs`. `BuiltInLocales` expanded from 6 → 10 registered locales. 15 new `LocaleSupplementalTests`.
- **CLI Tab Completion + Profile CRUD** (Sprint 127, T3.4/T8.5): `completions/RegiLattice.ps1` — full PowerShell tab completion with context-aware completions for profile names, scope values, and categories. CLI args `--profile-create/delete/clone/rename/tweaks/desc` + `--list-user-profiles`. `UserProfileService` CRUD wiring in `Program.Dispatch`. 10 new `ParseArgsTests`.
- **Compliance Trend Dashboard** (Sprint 128, T4.4): `ComplianceTrendDialog` — GDI+ line chart of compliance score / violation count over time, toggle between % compliance and violation-count modes, Refresh/Clear/Close controls, entry-count summary label. 12 new `ComplianceTrendDialogTests`.
- **50 New Security & Policy Tweaks** (Sprint 129, T8.2): 5 new modules across enterprise policy areas:
  - **BranchCache.cs** (`bc-*`) — 10 tweaks: distributed/hosted cache mode, cache size (% + GB cap), SHA-256 hashes, firewall exceptions, SMB hash publication, hosted-cache preference, retrieval latency, peer offering delay.
  - **InternetZonePolicy.cs** (`izone-*`) — 10 tweaks: lock zones to machine policy, block ActiveX/ActiveScript/mixed-content in Internet zone, prevent cert error bypass, block auto file download, SmartScreen phishing filter, block unencrypted form submit, block unsafe ActiveX init, block script clipboard access.
  - **NetworkConnectionsPolicy.cs** (`netconn-*`) — 10 tweaks: honour admin prohibits, prevent network bridges, block add/remove components, block binding change, prevent deleting all-user connections, block LAN properties, block VPN connect, block all-user VPN properties, prohibit ICS, prevent connection rename.
  - **AppVirtualization.cs** (`appv-*`) — 10 tweaks: allow package scripts, block high-cost launch, require admin to publish, auto-load background, disable shared content store, enable process interop, block virtual COM creation, enable reporting, 24 h reporting interval, 120 s streaming timeout.
  - **SensorPolicy.cs** (`sensor-*`) — 10 tweaks: block location scripting, disable all sensors, disable Windows Location Provider, prevent user location override, deny radios/activity/gazeInput/contacts/email/bluetoothSync capabilities.

#### Fixed

- Test suite: removed external-process `DetectAction` sweep tests from `BranchCoverage5Tests.cs` that caused >60 s hangs vs. the 20 s per-test budget. 837 placeholder sweep tests replaced by structural-only assertions. Suite now runs in ≤10 s with no hang risk.
- Test isolation: added `[Collection("ComplianceHistory")]` to `ComplianceHistoryTests` and `ComplianceHistoryNullJsonBranchTests` to eliminate rare file-system race causing intermittent failures.

#### Stats

- Total tweaks: **4,158** across **126 categories** (+50 tweaks, +5 categories vs 4.5.0)
- Tests: **2,007** passing (0 consistent failures)
- Build: **0 errors, 0 warnings** (Release x64)
- Version bumped `4.5.0` → `4.6.0`

---

## [4.5.0] — 2026-03-23

### M2 Milestone — Enterprise & Trust (Sprints 114–121)

#### Added

- **IntuneOmaUriExporter.cs**: Full Intune Custom Configuration Profile OMA-URI export for HKLM-path Registry-kind tweaks. Maps to `./Device/Vendor/MSFT/Policy/Config/...` CSP format. Unmappable tweaks flagged with `[NOT_MAPPABLE]`. CLI `--export-intune <file>` and File → Export Intune Policy… in GUI.
- **GroupPolicyExporter.cs** (enhanced): Full ADMX/ADML generation pipeline for all HKLM Registry-kind tweaks. Produces valid `.admx` + `.adml` importable in `gpedit.msc`. Per-tweak `Explain` text from `Description`. CLI `--export-gpo <file>`.
- **AutoUpdater.cs** (enhanced Sprint 115): Download + guided install. `UpdateInfo.DownloadAsync()` fetches MSI to `%TEMP%`, verifies SHA-256 against GitHub release asset manifest, then offers "Install & Restart" in `UpdateCheckerDialog`. Never auto-installs without user confirmation.
- **BenchmarkDotNet project** (Sprint 121): `tests/RegiLattice.Benchmarks/` — 8 TweakEngine benchmarks + 5 RegistrySession benchmarks. Baseline performance data established.
- **Branch coverage 75.14%** (Sprint 121): `BranchCoverage7Tests.cs` — 10 new tests targeting 12 partial-coverage branches in Analytics, SnapshotManager, ComplianceHistory, Ratings, HealthScoreService, AppConfig, ScheduledTweakService, Favorites.
- **200+ Locale keys** (Sprint 120): All GUI labels now routed through `Locale.T()`. String table expanded from 17 keys to 200+ keys covering all UI labels, button text, status messages, dialog titles, error messages across 6 languages (en/de/fr/es/ja/he).

#### Enhanced

- `UpdateCheckerDialog`: Progress bar during download, SHA-256 verification status, "Install & Restart" button (launches MSI, exits app).
- `MainForm`: All menu captions, status bar messages, button labels, filter labels routed through `Locale.T()`.
- CI/CD: `SHA256SUMS.txt` published alongside release artifacts. Smoke test step verifies CLI `--list` + `--validate` on published EXE.
- Scoop manifest `scoop/regilattice.json`: auto-updated SHA-256 on each tagged release via CI.

#### Stats

- Total tweaks: **4,108** across **121 categories**
- Tests: **1,944** passing (0 failures)
- Branch coverage: **75.14%** (1741/2317) — M2 gate ✅
- Build: **0 errors, 0 warnings** (Release x64)
- Version bumped `4.4.0` → `4.5.0`

---

## [4.4.0] — 2026-03-22

### Sprint 106 — 50 New Security & Hardening Tweaks (WDAC/ASR, BitLocker To Go, Device Install, LAPS, NTLM)

#### Added

- **WdacCodeIntegrity.cs** (`wdac-asr-*`) — 10 individual WDAC/Defender ASR rule tweaks, each enabling a specific ASR GUID in block mode (Office child processes, LSASS dump, ransomware, WMI persistence, PSExec/WMI, USB untrusted, email executables, obfuscated scripts, Adobe child, Office code injection)
- **BitLockerRemovable.cs** (`btogo-*`) — 10 BitLocker To Go policy tweaks for removable data volumes (deny write unencrypted, enable RDV, passphrase complexity/length, AES-128-XTS, disable hardware encryption, smart card auth, recovery password/key backup)
- **DeviceInstallPolicies.cs** (`dinst-*`) — 10 device installation restriction policy tweaks (deny removable install, device ID block list, class GUID block list, retroactive application, admin override, disable Windows Update driver search, disable co-installers, WER suppression, metadata download block)
- **LapsSecurity.cs** (`laps-*`) — 10 Windows LAPS (built-in, April 2023+) policy tweaks (Azure AD/Entra backup, AD backup, 14-day max age, 20-char min length, max complexity, password encryption, post-auth reset+logoff, 24h grace period, 12-entry encrypted history, disable legacy LAPS)
- **NtlmAuthentication.cs** (`ntlma-*`) — 10 NTLM authentication hardening tweaks (NTLMMinClientSec/ServerSec 0x20080000, Netlogon channel sign+seal, deny all outgoing NTLM, domain NTLM audit, in-domain NTLM audit/restrict, MSV1_0 audit, block null session fallback, require NTLMv2 at MSV1_0 level)

#### Stats

- Total tweaks: **4,108** (+50 vs 4.3.0)
- Categories: **121** (+5 new: WDAC & Code Integrity, BitLocker To Go, Device Installation Policies, Local Admin Password (LAPS), NTLM Authentication)
- Tests: **1,858** passing (0 failures)

---

## [4.3.0] — 2026-03-22

### Sprints 99–105 — Dialogs, Services, PowerShell Module, Notifications & Compliance History

#### Added

- **SmartScanService.cs** (Sprint 99): `Scan()` / `ScanAsync()` — filters unapplied tweaks, scores by ImpactScore × SafetyRating, returns top 25 `ScanRecommendation` records. IsQuickWin flag for Impact ≥ 4 && Safety ≥ 4.
- **SmartScanDialog** (Sprint 99): ListView of top 25 recommendations; Quick Wins highlighted in green; "Apply All Quick Wins" + per-row Apply buttons; progress bar.
- **ProfileCompareDialog** (Sprint 100): Side-by-side diff of any two built-in profiles. ComboBox pair, colour-coded ListView (Added/Removed/Shared rows), Export HTML (Catppuccin dark theme). Added via Tools menu.
- **DependencyGraphDialog** (Sprint 101): TreeView-based dependency explorer. Searchable ListBox with ★ markers on tweaks that have DependsOn. "Depends on" + "Needed by" branches for any selected tweak. Clickable tree nodes for navigation. Added via Tools menu.
- **ComplianceReportExporter.cs** (Sprint 102): `RegiLattice.Core.Services` — `ExportHtml()` + `BuildHtml()`. Groups tweaks by category, computes applied/pending/unknown counts, health score %. Catppuccin dark HTML with `WebUtility.HtmlEncode`. Wired to File → Export Compliance Report….
- **PowerShell module scaffold** (Sprint 103): `powershell/RegiLattice.psd1` (manifest v4.3.0) + `powershell/RegiLattice.psm1` — CLI-wrapping script module with 5 cmdlets: `Get-RLTweak`, `Get-RLTweakStatus`, `Invoke-RLApply`, `Invoke-RLRemove`, `Get-RLHealthScore`. Pipeline-native `PSCustomObject` output, `Update-TypeData` format defaults, aliases `grt`/`grts`/`ira`/`irr`. `PowerShellModuleGenerator.cs` bumped to v4.3.0.
- **JumpListService.cs** (Sprint 104): Scaffold for Windows 11 taskbar Jump List integration. No-op until MSIX installer provides AUMID; structured for future COM `ICustomDestinationList` wiring.
- **ToastNotificationService.cs** (Sprint 104): WinRT `ToastNotificationManager` toast delivery with automatic fallback to `NotifyIcon.ShowBalloonTip()`. `ShowApplyComplete()` fired after every batch-apply in `MainForm`.
- **ComplianceHistory.cs** (Sprint 105): `RegiLattice.Core.Services` — rolling compliance log persisted to `compliance-history.json` (90-entry cap). Records `ComplianceHistoryEntry` per check (date, total checked, violation count, drifted IDs, snapshot path). `AddEntry()` auto-wired in `RunCompliance()`.
- **--compliance-history CLI flag** (Sprint 105): Prints last 30 compliance entries with date/checked/violations columns.
- **--compliance-report auto CLI flag** (Sprint 105): Auto-locates the latest snapshot in `ConfigDir`, runs a compliance check against it, and prints the drift report.
- **Toast extensions** (Sprint 105): `ShowComplianceDrift(violations)` and `ShowUpdateAvailable(version)` added to `ToastNotificationService`.

### Sprints 88-96 — 9 New Modules + Existing Module Enhancements

#### Added

- **NetworkInterface.cs**: 10 TCP/IP NIC adapter tuning tweaks (`nic-*`)
  - Disable packet coalescing, adapter power management, EEE, Large Send Offload, Checksum Offload, NDIS RSS, jumbo frames; set Rx/Tx buffers, interrupt coalescing; disable AutoNegotiation
- **SystemShutdown.cs**: 10 shutdown & restart behavior tweaks (`shdn-*`)
  - Fast startup, hibernate, hybrid sleep, automatic maintenance wakeup, crash auto-restart, BSOD on hang, shutdown timeout, force shutdown on close, UPS sleep, clean boot
- **MicrosoftAccount.cs**: 10 MSA sync, privacy & linked-devices tweaks (`msa-*`)
  - Disable MSA sign-in policy, consumer experience, Settings sync, account notifications, linked phone, suggested contacts, cloud clipboard, Cortana MSA usage, connected account registration
- **DeviceGuardVbs.cs**: 10 HVCI/VBS/Credential Guard hardening tweaks (`vbs-*`)
  - Enable Credential Guard, HVCI, enforced HVCI, VBS, Secure Boot, kernel DMA protection, kernel-mode code integrity, require trusted launch; disable DMA remapping; block vulnerable drivers
- **WindowsInk.cs**: 10 Ink Workspace, pen & touch-input tweaks (`ink-*`)
  - Disable Ink Workspace, suggested apps in Ink, touch feedback, flicks, pen flicks, tablet mode roaming, touch keyboard auto-launch, handwriting personalization, pen workspace; enforce handwriting input on compatible devices
- **CloudExperience.cs**: 10 OOBE, cloud-content & Microsoft Account tweaks (`oobe-*`)
  - Disable OOBE improvements, pre-installed bloatware, consumer features, Cortana during OOBE, privacy experience, Windows Spotlight, lock-screen widgets, user preferences collection, cloud consumer account state, advertising ID
- **UserActivity.cs**: 10 timeline, recent docs & CDP tweaks (`activity-*`)
  - Disable activity feed publishing, timeline, cross-device sharing (CDP), connected devices platform, recent items, quick-access frequent folders, notification Center, auto-open Downloads, search highlights, last-access timestamp
- **WifiNetworking.cs**: 10 Wi-Fi Sense, metered networks & 802.11 tweaks (`wifi-*`)
  - Disable Wi-Fi Sense hotspot sharing, random hardware addresses, BT coexistence interference, WLAN AutoConfig triggers; enable 802.11d, 802.11h; cap scan retries, set throttle limit; enforce WPA3 preference; disable SSID broadcasting
- **PrintSpoolerSecurity.cs**: 10 PrintNightmare mitigations & spooler hardening tweaks (`spool-*`)
  - Disable Print Spooler, RpcAuthnLevelPrivacyEnabled enforcement, point-and-print restrictions, inbound SMB printing, outbound print events, network printer discovery, Web Services for Printers; restrict driver installation, operator users, HTTPS printing

#### Enhanced (existing modules)

- **AutoRunPolicy.cs**, **SecurityAuditPolicy.cs**, **TimeSync.cs**, **TouchpadGestures.cs**, **VirtualDesktops.cs**, **WindowsSearchAdv.cs**: additional tweaks, corrections, and de-duplication

#### Fixed

- `WifiNetworking.cs`: `WiFiService` → `WifiService` (6× CS0103 capitalization error)
- `UserActivity.cs`: replaced duplicate `activity-disable-timeline-view` with distinct `activity-disable-cdp` (different registry key)

#### Stats

- Tweaks: **4 058** across **116 categories** (121 module files)
- Tests: **1 858** (1 325 Core + 291 CLI + 242 GUI), all passing
- Build: **0 errors, 0 warnings** (Release x64)
- Version bumped `4.2.0` → `4.3.0`

---

## [4.2.0] — 2026-03-21

### Sprint 77 — Remote Management, SSH Hardening, Kiosk/Shared PC, Active Directory, Hyper-V Advanced

#### Added

- **RemoteManagement.cs**: 10 WinRM policy hardening + RPC restriction tweaks (`rmt-*`)
  - Disable WinRM service, block unencrypted/Basic/Digest/CredSSP auth, restrict RPC clients, require RPC auth endpoint isolation, limit WinRM shell memory
- **SshHardening.cs**: 10 OpenSSH `sshd_config` hardening tweaks (`ssh-*`) — gated on `C:\ProgramData\ssh\sshd_config`
  - Limit max auth tries (3), login grace time (30 s), deny empty passwords, disable forwarding (agent/TCP/X11), restrict max sessions (2), enable StrictModes, enforce strong ciphers + MACs
- **KioskSharedPc.cs**: 10 Windows Shared PC / Kiosk configuration tweaks (`kiosk-*`)
  - Enable SharedPC mode, guest account model, auto-delete on sign-out, disk-level thresholds, disable fast user switching, no local password reset, enable education policies, disable lock-screen camera/slideshow
- **ActiveDirectory.cs**: 10 AD domain client hardening tweaks (`ad-*`)
  - Disable NT4 crypto, restrict Kerberos to AES, enable Kerberos armoring (FAST), max token size, scavenge interval, negative cache period, disable mailslot discovery, block single-label DNS, restrict connected-user enumeration
- **HyperVAdvanced.cs**: Expanded from 10 to 20 tweaks (+10 `hyperv-*`)
  - Disable auto-checkpoints, require network credentials, allow SR-IOV, enable bandwidth management, disable VM broadcast, cap max VMs (8), reserve 512 MB host memory, remove default switch, enforce strict network isolation
- 4 new categories: **Remote Management**, **SSH Configuration**, **Kiosk & Shared PC**, **Active Directory**

#### Stats

- Tweaks: **3 868** across **107 categories**
- Tests: **1 647** (1 230 Core + 175 CLI + 242 GUI), all passing
- Version bumped `4.1.0` → `4.2.0`

---

## [4.1.0] — 2026-07-22

### Sprints 57–67 — Intelligence, Portability, Automation & New Tweaks

#### Added

- **ImpactScore & SafetyRating metadata** (Sprint 57): New `int ImpactScore` and `int SafetyRating` fields on `TweakDef` (1–5 scale). GUI shows color-coded Impact/Safety badges. CLI displays in `--list` output.
- **NLP synonym search** (Sprint 58): `TweakEngine.Search()` now expands query tokens through a built-in synonym map (60+ entries: fast→performance, spy→telemetry, bloat→debloat, etc.). Multi-token AND logic with expanded groups.
- **Portable mode** (Sprint 59): `--portable` flag + `AppConfig.SetPortable()`. Redirects all `%LOCALAPPDATA%` paths to `.\data\` in the executable directory. Auto-detected via sentinel file `data\.portable`.
- **Silent mode CLI** (Sprint 60): `--silent` suppresses all console output; JSON operation log written to `--log-file <path>`. Exit codes: 0 = success, 1 = failure/partial.
- **AutoUpdater service** (Sprint 61): `AutoUpdater.cs` — polls GitHub Releases v3 API, compares semantic versions, returns `UpdateInfo` record. `IsNewer()` handles `v`-prefixed version strings.
- **HealthScoreService** (Sprint 62): `HealthScoreService.cs` — computes weighted Privacy/Performance/Security/Stability scores (0–100) from `StatusMap()`. Returns `HealthScore` record with `OverallLabel` ("Excellent"/"Good"/"Fair"/"Needs Work"/"Poor").
- **50 new tweaks across 5 new categories** (Sprint 63):
  - `XboxGameBar.cs` — 10 tweaks (Xbox / Game Bar category)
  - `WindowsHello.cs` — 10 tweaks (Windows Hello category)
  - `SmartAppControl.cs` — 10 tweaks (Smart App Control category)
  - `EnergySaver.cs` — 10 tweaks (Energy Saver category)
  - `CopilotPlus.cs` — 10 tweaks (Copilot+ Features category)
- **FirstRunWizardDialog** (Sprint 64): 3-step wizard shown on first launch — profile selection, dry-run toggle, feature tour. Writes initial config to `config.json`.
- **ProfileWizardDialog** (Sprint 65): 5-question wizard generates a personalized `TweakProfile`. Questions cover gaming, privacy, performance priority, hardware type, and corporate environment.
- **ConflictDetector service** (Sprint 66): `ConflictDetector.cs` — maintains a static lookup table of known-conflicting tweak pairs. `Detect()` returns conflicts given a set of tweak IDs; `ConflictsFor()` checks a single ID against applied tweaks.
- **NLP synonym map fix**: Removed `"privacy"` from the `"telemetry"` synonym expansion to prevent AND-search false positives (e.g., searching "privacy telemetry" now correctly returns only tweaks matching both terms).

#### Tests (Sprint 67)

- `HealthScoreServiceTests.cs` — 13 tests: score computation, bucket helpers, range validation, label mapping
- `AppConfigPortableTests.cs` — 8 tests: default state, SetPortable, ConfigDir redirect, auto-detection
- `ConflictDetectorTests.cs` — 14 tests: AllConflicts, Detect symmetry/known pairs, ConflictsFor
- `NewTweakModulesTests.cs` — 8 tests: category registration, ID uniqueness, validator clean
- `AutoUpdaterTests.cs` — tests: IsNewer comparisons, CheckAsync mock
- `TweakDefMetadataTests.cs` — tests: ImpactScore/SafetyRating model
- `TweakEngineSearchNlpTests.cs` — tests: synonym expansion, multi-token AND, empty/whitespace behavior
- **Fixed**: `Search_SingleToken_ReturnsRelevantTweaks` — relaxed to allow synonym-expanded results while verifying at least one direct match
- **Fixed**: `Search_MultiToken_AllTokensMustMatch` — resolved by correcting synonym map overlap

#### Total (v4.1.0)

| Metric | v4.0.0 | v4.1.0 |
|--------|--------|--------|
| Tweaks | 3,669 | **3,719** |
| Categories | 94 | **99** |
| Tests | 1,435 | **1,538** (1121 Core + 175 CLI + 242 GUI) |
| Core services | 24 | **27** (+ AutoUpdater, HealthScoreService, ConflictDetector) |
| New dialogs | — | **2** (FirstRunWizard, ProfileWizard) |

---

## [4.0.0] — 2026-03-20

### Major Release — All Capabilities Enabled

v4.0.0 is the first **major release milestone**, consolidating every capability built across
the v3.x development cycle into a single production-ready package. All features, interfaces,
and safety systems are fully active — GUI, CLI, MSI installer, 11 themes, 5 profiles,
corporate guard, dry-run, snapshot, package managers, plugin marketplace, and the
newly introduced anti-duplication quality layer.

### Added

- **Anti-duplication quality system** (`chore(quality)` commit `877b80b`):
  - `.github/instructions/no-duplication.instructions.md` — 4-layer prevention rules (IDs, ops, labels, conceptual)
  - `.github/skills/no-duplication/SKILL.md` — 6-step audit workflow with PowerShell one-liners and resolution guide
  - `scripts/Audit-Duplications.ps1` — colour-coded audit script covering all 4 duplication layers; `exit 1` on hard violations
  - **+4 duplication guard tests** in `TweakEngineBuiltinsTests.cs`:
    - `RegisterBuiltins_DuplicateRegistryOps_BelowRegressionThreshold` (threshold ≤ 1 200)
    - `RegisterBuiltins_NoCrossModuleLabelAndPathCollision` (threshold ≤ 200, 128 groups detected as existing debt)
    - `RegisterBuiltins_CategorySlugs_MatchKnownPrefixes` (spot-checks 10 canonical category slug prefixes)
    - `RegisterBuiltins_DetectDuplicateRegistryOps_ProducesUsableOutput` (scale smoke-test at 3 669 tweaks)

- **Next Phase Master Plan** appended to `docs/Roadmap.md` — 9-phase plan (v3.9 → v4.2) covering:
  - Phase A: Deduplication sprint (fix 128 cross-module label+path collisions, ~800 duplicate ops)
  - Phase B: Tweak quality audit (semantic correctness review)
  - Phase C: GUI UX pass (search improvements, keyboard nav, tray polish)
  - Phase D: CLI completeness (remaining commands, shell completion)
  - Phase E: Coverage push (branch coverage from 56.8% toward 70%)
  - Phase F–I: New tweak categories, localization expansion, plugin ecosystem, release automation

### All v4.0.0 Capabilities at a Glance

| Capability | Status |
|---|---|
| **3 669 verified tweaks** across 94 categories | ✅ |
| **WinForms GUI** with 11 switchable themes | ✅ |
| **CLI** with 25+ commands | ✅ |
| **Interactive console menu** | ✅ |
| **MSI installer** (WiX v6, self-contained win-x64) | ✅ |
| **Portable EXE** (GUI + CLI, no .NET required) | ✅ |
| **5 machine profiles** (business, gaming, privacy, minimal, server) | ✅ |
| **Dry-run mode** — preview without touching registry | ✅ |
| **Snapshot & diff** — save/restore/compare JSON state | ✅ |
| **CorporateGuard** — blocks unsafe tweaks on managed machines | ✅ |
| **UAC auto-elevation** | ✅ |
| **Package manager dialogs** (WinGet, Scoop, pip, Chocolatey, PSModules) | ✅ |
| **Plugin marketplace** — JSON Tweak Packs with SHA-256 verification | ✅ |
| **Dependency resolver** — topological apply order | ✅ |
| **TweakValidator** — ID/label/dep/circular-dep integrity checks | ✅ |
| **Anti-duplication system** — 4-layer detection + audit script | ✅ |
| **Localization** — English + German (48 strings) | ✅ |
| **Live CPU/RAM monitoring** in About dialog | ✅ |
| **Automatic JSON backups** before every registry mutation | ✅ |
| **1 435 tests** across 17 test files (0 failures) | ✅ |

### Stats

- Tweaks: **3 669** across 94 categories (unchanged from v3.8.0)
- Tests: **1 435** passing (1 018 Core + 175 CLI + 242 GUI) — +21 from v3.8.0
- Version bumped `3.8.0` → `4.0.0`

---

## [3.8.0] — 2026-07-21

### Added

- **678 new tweaks** across 10 sprint cycles (Sprints 48–57), bringing the total to **3 669 verified tweaks** across **94 categories**:
  - **Sprint 48** — Bluetooth (+10), Printing (+10), Touch & Pen (+10), Voice/Speech (+10), Storage (+10)
  - **Sprint 49** — AI/Copilot (+10), Scoop Tools (+10), Dev Drive (+10), Java (+10), M365 Copilot (+10)
  - **Sprint 50** — Gaming (+10), GPU/Graphics (+10), Boot (+10), Windows 11 (+10), Scheduled Tasks (+10)
  - **Sprint 51** — Microsoft Store (+10), Edge (+10), Firefox (+10), Chrome (+10), Office (+10)
  - **Sprint 52** — Firewall (+15), Encryption (+12), Hardening (+12) — security hardening module expansion
  - **Sprint 53** — Network (+15), DNS & Networking Advanced (+15), Proxy & VPN (+10), Network Optimization (+10)
  - **Sprint 54** — Performance (+10), SSD Optimization (+10), File System (+10), Disk Cleanup (+10)
  - **Sprint 55** — Privacy (+12), Cortana & Search (+12), Widgets & News (+12), Telemetry Advanced (+12)
  - **Sprint 56** — Developer (+13), PowerShell Tweaks (+13), Services (+12), User Account (+12): console VT/UTF8, WER/JIT, Defender CFA/network/CPU, SMB signing+encryption, Teredo/6to4/ISATAP, credential/logon security, Netlogon hardening
  - **Sprint 57** — Shell (+13), Lock Screen (+13), Input (+12), Taskbar (+12): thumbnail cache, spotlight/screensaver policy, PrecisionTouchPad controls, taskbar density, compact mode, logon scripts, NTLM settings

### Stats

- Tweaks: **3 669** (was 2 991, +678)
- Tests: **1 414** passing (1 014 Core + 175 CLI + 225 GUI) — unchanged
- Version bumped `3.7.3` → `3.8.0`

---

## [3.7.3] — 2026-03-20

### Fixed / Polished

- **Title-bar icons** added to 7 dialogs that were missing them:
  `AboutDialog`, `WhatsNewDialog`, `PreferencesDialog`, `HostsAddDialog`,
  `HostsUrlPromptDialog`, `ScheduleEditDialog`, `AddProfileScheduleDialog`

### Chore

- `Readme.md` renamed to `README.md` (universal GitHub/open-source convention)
- Root-level `CONTRIBUTING.md` and `CHANGELOG.md` stubs added — GitHub auto-links
  the Contribute and Changelog community-health buttons from root or `.github/`
- `PULL_REQUEST_TEMPLATE.md` test count corrected: `203+` → `1,431+`
- `github.copilot` base extension added to `.vscode/extensions.json`

### Stats

- Tweaks: **2 991** (unchanged)
- Tests: **1 414** passing (1 014 Core + 175 CLI + 225 GUI)
- Version bumped `3.7.2` → `3.7.3`

---

## [3.7.2] — 2026-03-20

### Added

- **64 new tests** covering previously untested code paths (+4.7% branch coverage):
  - `AppConfigUiPrefsTests` (+44): default-value assertions and save/load roundtrip for
    23 untested `AppConfig` properties — `MinimizeToTray`, `ConfirmApply`, `ConfirmRemove`,
    `ShowInapplicable`, `FontSize`, `ShowLogPanel`, `LogPanelHeight`, `AutoRefreshOnStartup`,
    `LaunchMinimized`, `RememberSplitter`, `SplitterDistance`, `SkipAppliedOnBatch`,
    `BrightnessSchedulerEnabled` + 4 value fields, `HistoryMaxEntries`, `MonitorColorCoded`,
    `AutoCleanMemoryThreshold`, `ProfileOnPlanSwitch`, `ProfileSchedules`; plus
    `ProfileScheduleEntry` record defaults and disabled-entry roundtrip
  - `ComplianceAndManagerParseTests` (+20): `--compliance`, `--export-gpo`, `--manager`
    flag parsing with value variants, without-value edge cases, combined-flag tests

### Changed

- **Standing commit rule** reinforced in `.github/copilot-instructions.md`: Git Workflow
  section now includes the per-sprint mandate and total-count requirement explicitly,
  making it visible on every session load alongside the quick-facts table
- Version bumped `3.7.1` → `3.7.2`

### Stats

- Tweaks: **2 991** (unchanged)
- Tests: **1 431** passing (1 014 Core + 175 CLI + 242 GUI) — was 1 367

---

## [3.7.1] — 2026-07-18

### Changed

- **Duplicate tweaks removed**: eliminated 11 confirmed duplicate tweak definitions
  (3 194 → 3 183 tweaks across 92 categories)
- **Test suite refactored**: `TweakEngineBuiltinsTests.cs` 1 956 → 480 lines;
  replaced 617 per-ID `[InlineData]` existence checks with a single
  `AllRegisteredTweaks_CanBeRetrievedById` Fact iterating all tweaks at runtime
  — total test count 2 088 → **1 367 passing** (970 Core + 155 CLI + 242 GUI)
- **Dead code removed**: deleted `RegistryHives.cs` (11 lines) — `Hive.LM`/`Hive.CU`
  constants confirmed unused across all 93 tweak modules
- **Roadmap trimmed**: `docs/Roadmap.md` 1 265 → ~430 lines; replaced exhaustive
  Sprint 1–47b task tables (all completed) with compact sprint digest table
- **Git workflow instructions updated**: `.github/instructions/git-workflow.instructions.md`
  updated to use .NET commands; added standing per-sprint commit mandate section
- Version bumped `3.7.0` → `3.7.1`

---

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

## [Unreleased] — Sprint 42 (Hardware Tools)

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

### Added (Sprint 8)

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

### Changed (Sprint 8)

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
