# Security — P/Invoke & External Process Inventory

This document enumerates every P/Invoke call and external process invocation in the
RegiLattice C# codebase, the source of each argument, and the security rationale.

---

## P/Invoke Calls (4 total)

The codebase uses only 4 P/Invoke calls. All registry access is via `Microsoft.Win32.Registry`.

### `CorporateGuard.cs`

| Call site | DLL | Function | Purpose |
|-----------|-----|----------|---------|
| `IsCorporateNetwork()` | `kernel32.dll` | `GetComputerNameExW` | Detect AD domain membership |

- **Arguments**: Hardcoded enum value `ComputerNameDnsDomain` + pre-allocated buffer
- **Security**: Read-only operation, no user input, buffer size validated

### `HardwareInfo.cs`

| Call site | DLL | Function | Purpose |
|-----------|-----|----------|---------|
| `Detect()` | `kernel32.dll` | `GlobalMemoryStatusEx` | Detect system RAM size |

- **Arguments**: Pre-allocated `MEMORYSTATUSEX` struct with hardcoded `dwLength`
- **Security**: Read-only operation, no user input

### `SystemMonitor.cs`

| Call site | DLL | Function | Purpose |
|-----------|-----|----------|---------|
| `GetCpuUsagePercent()` | `kernel32.dll` | `GetSystemTimes` | Measure CPU idle/kernel/user time |
| `GetMemoryUsage()` | `kernel32.dll` | `GlobalMemoryStatusEx` | Live RAM usage monitoring |

- **Arguments**: Pre-allocated structs with hardcoded sizes
- **Security**: Read-only operations, no user input, only queried on a timer

---

## WMI Queries (via System.Management)

### `CorporateGuard.cs`

| Query | Purpose | Notes |
|-------|---------|-------|
| `SELECT * FROM SMS_Client` | SCCM/Intune enrollment detection | Read-only, hardcoded query |

### `HardwareInfo.cs`

| Query | Purpose | Notes |
|-------|---------|-------|
| `SELECT * FROM Win32_Processor` | CPU detection | Read-only |
| `SELECT * FROM Win32_VideoController` | GPU detection | Read-only |
| `SELECT * FROM Win32_DiskDrive` | Disk detection | Read-only |

---

## Registry Access

All registry operations use `Microsoft.Win32.Registry` (managed API).

- **Write operations**: Only performed by `RegistrySession.Execute()` when applying tweaks
- **Read operations**: Used by `RegistrySession.Evaluate()` for detection and `CorporateGuard` for GPO checks
- **Path validation**: All registry paths validated against known hive prefixes (`HKEY_LOCAL_MACHINE`, `HKEY_CURRENT_USER`, etc.)
- **Backup**: JSON backup created before any write operation via `RegistrySession.Backup()`
- **DryRun**: `RegistrySession.DryRun = true` prevents all writes (used in tests)

---

## External Process Invocations

`ShellRunner.cs` (Core) provides a safe process runner used by command-based tweaks
(`SystemCommand`, `ServiceControl`, `ScheduledTask`, `PowerShell`, and `PackageManager` kinds).

| Class | File | Method | Security notes |
|-------|------|--------|---------------|
| `ShellRunner` | `Services/ShellRunner.cs` | `RunAsync(fileName, args)` | Uses `ArgumentList` (no shell injection); never passes user input directly |
| `ShellRunner` | `Services/ShellRunner.cs` | `RunPowerShellAsync(script)` | Wraps `RunAsync` with `powershell.exe -NoProfile -NonInteractive -Command`; scripts are hardcoded in `TweakDef.ApplyAction` delegates |

**Security properties of ShellRunner**:
- `ArgumentList` (not `Arguments`) — arguments are parameterized, never shell-interpolated
- `UseShellExecute = false` — no implicit cmd.exe wrapper
- Scripts and executable names are hardcoded constants in tweak modules; no user input reaches `RunAsync`
- `CreateNoWindow = true` — no visible terminal spawned

All other functionality is implemented via:

1. `Microsoft.Win32.Registry` — for registry read/write
2. `System.Management` — for WMI queries
3. P/Invoke — for the 4 kernel32.dll calls listed above

---

## Security Properties

| Property | Status |
|----------|--------|
| No shell execution | ✅ No `Process.Start` with shell commands |
| No user input in P/Invoke | ✅ All arguments are hardcoded constants |
| No outbound network calls | ✅ No HTTP clients at runtime |
| Registry path validation | ✅ Hive prefix validation on all paths |
| Backup before writes | ✅ JSON backup via `RegistrySession.Backup()` |
| DryRun mode available | ✅ `RegistrySession.DryRun` prevents all writes |
| Managed registry access | ✅ `Microsoft.Win32.Registry` (no `reg.exe`) |
| Minimal P/Invoke surface | ✅ Only 2 calls, both read-only |
