"""Hardware detection for RegiLattice — adaptive configuration per machine.

Detects CPU, GPU, RAM, disk, virtualisation, and WSL capabilities so that
the GUI, thread pools and profile recommendations can auto-tune to the
hardware actually available.  All probes are **read-only** and cached
after the first call via :func:`functools.lru_cache`.

Every function is safe on non-Windows platforms (returns sensible defaults).
"""

from __future__ import annotations

import concurrent.futures
import contextlib
import functools
import os
import platform
import re
import subprocess
import threading
from dataclasses import dataclass, field

# ── Dataclasses ──────────────────────────────────────────────────────────────


@dataclass(slots=True)
class CPUInfo:
    """Detected CPU information."""

    name: str = "Unknown"
    cores_physical: int = 1
    cores_logical: int = 1
    arch: str = "x86_64"


@dataclass(slots=True)
class GPUInfo:
    """Detected GPU information."""

    name: str = "Unknown"
    driver_version: str = ""
    adapter_ram_mb: int = 0


@dataclass(slots=True)
class MemoryInfo:
    """Detected system memory."""

    total_mb: int = 0
    available_mb: int = 0


@dataclass(slots=True)
class DiskInfo:
    """Detected system disk (C: drive)."""

    total_gb: int = 0
    free_gb: int = 0
    is_ssd: bool = False


@dataclass(slots=True)
class HWProfile:
    """Aggregated hardware profile for the current machine."""

    cpu: CPUInfo = field(default_factory=CPUInfo)
    gpus: list[GPUInfo] = field(default_factory=list)
    memory: MemoryInfo = field(default_factory=MemoryInfo)
    disk: DiskInfo = field(default_factory=DiskInfo)
    has_hyperv: bool = False
    has_wsl: bool = False
    has_tpm: bool = False
    has_secure_boot: bool = False
    windows_build: int = 0
    optimal_workers: int = 4
    gui_batch_size: int = 4


# ── Probes ───────────────────────────────────────────────────────────────────


def _run_ps(script: str, timeout: int = 8) -> str:
    """Run a short PowerShell snippet and return stripped stdout."""
    try:
        r = subprocess.run(
            ["powershell", "-NoProfile", "-Command", script],
            capture_output=True,
            text=True,
            timeout=timeout,
        )
        return r.stdout.strip()
    except Exception:
        return ""


# ── Composite CIM query (single PS process for CPU+GPU+RAM+Disk+HyperV) ────

_COMPOSITE_PS = r"""
$cpu = Get-CimInstance Win32_Processor | Select-Object -First 1
$gpus = Get-CimInstance Win32_VideoController
$os = Get-CimInstance Win32_OperatingSystem
$disk = Get-CimInstance Win32_LogicalDisk -Filter "DeviceID='C:'"
$cs = Get-CimInstance Win32_ComputerSystem
$pd = Get-PhysicalDisk | Where-Object { $_.DeviceID -eq '0' } | Select-Object -First 1
"CPU_NAME:" + $cpu.Name
"CPU_CORES:" + $cpu.NumberOfCores
"HYPERV:" + $cs.HypervisorPresent
"MEM_TOTAL:" + $os.TotalVisibleMemorySize
"MEM_FREE:" + $os.FreePhysicalMemory
"DISK_SIZE:" + $disk.Size
"DISK_FREE:" + $disk.FreeSpace
"DISK_MEDIA:" + $pd.MediaType
foreach ($g in $gpus) { "GPU:" + $g.Name + "|" + $g.DriverVersion + "|" + $g.AdapterRAM }
"""

_CIM_CACHE: dict[str, str] | None = None
_CIM_LOCK = threading.Lock()


def _run_composite_cim() -> dict[str, str]:
    """Execute one PowerShell process for all CIM probes and return parsed results.

    Returns a dict like ``{"CPU_NAME": "...", "GPU:0": "NVIDIA...|31...|bytes", ...}``.
    Thread-safe: only one PowerShell process is launched even under concurrency.
    """
    global _CIM_CACHE
    # Fast path without lock (already populated)
    if _CIM_CACHE is not None:
        return _CIM_CACHE
    with _CIM_LOCK:
        # Double-checked locking — re-check inside the lock
        if _CIM_CACHE is not None:
            return _CIM_CACHE

    raw = _run_ps(_COMPOSITE_PS, timeout=12)
    result: dict[str, str] = {}
    gpu_idx = 0
    for line in raw.splitlines():
        line = line.strip()
        if not line:
            continue
        if line.startswith("GPU:"):
            result[f"GPU:{gpu_idx}"] = line[4:]
            gpu_idx += 1
        elif ":" in line:
            key, _, val = line.partition(":")
            result[key.strip()] = val.strip()
    result["GPU_COUNT"] = str(gpu_idx)
    with _CIM_LOCK:
        _CIM_CACHE = result
    return result


@functools.lru_cache(maxsize=1)
def detect_cpu() -> CPUInfo:
    """Detect CPU model, physical/logical cores, and architecture."""
    logical = os.cpu_count() or 1
    arch = platform.machine() or "x86_64"
    name = "Unknown"
    physical = logical

    if os.name == "nt":
        # CPU name from environment (fast, no subprocess)
        name = os.environ.get("PROCESSOR_IDENTIFIER", "Unknown")
        # Try NUMBER_OF_PROCESSORS for logical count (fast)
        with contextlib.suppress(ValueError):
            logical = int(os.environ.get("NUMBER_OF_PROCESSORS", str(logical)))
        # Use composite CIM query (single PS process)
        cim = _run_composite_cim()
        raw_cores = cim.get("CPU_CORES", "")
        if raw_cores.isdigit():
            physical = int(raw_cores)
        cim_name = cim.get("CPU_NAME", "")
        if cim_name:
            name = cim_name

    return CPUInfo(name=name, cores_physical=physical, cores_logical=logical, arch=arch)


@functools.lru_cache(maxsize=1)
def detect_gpus() -> list[GPUInfo]:
    """Detect GPU adapters — returns one entry per adapter."""
    if os.name != "nt":
        return []
    cim = _run_composite_cim()
    gpu_count = int(cim.get("GPU_COUNT", "0"))
    gpus: list[GPUInfo] = []
    for i in range(gpu_count):
        raw_line = cim.get(f"GPU:{i}", "")
        parts = raw_line.split("|")
        if not parts or not parts[0].strip():
            continue
        name = parts[0].strip()
        driver = parts[1].strip() if len(parts) > 1 else ""
        ram_bytes = 0
        if len(parts) > 2:
            with contextlib.suppress(ValueError):
                ram_bytes = int(parts[2].strip())
        gpus.append(GPUInfo(name=name, driver_version=driver, adapter_ram_mb=ram_bytes // (1024 * 1024)))
    return gpus


@functools.lru_cache(maxsize=1)
def detect_memory() -> MemoryInfo:
    """Detect total and available system RAM in MB."""
    if os.name != "nt":
        return MemoryInfo()
    cim = _run_composite_cim()
    total_kb = 0
    avail_kb = 0
    with contextlib.suppress(ValueError):
        total_kb = int(cim.get("MEM_TOTAL", "0"))
    with contextlib.suppress(ValueError):
        avail_kb = int(cim.get("MEM_FREE", "0"))
    return MemoryInfo(total_mb=total_kb // 1024, available_mb=avail_kb // 1024)


@functools.lru_cache(maxsize=1)
def detect_disk() -> DiskInfo:
    """Detect C: drive size, free space, and SSD status."""
    if os.name != "nt":
        return DiskInfo()
    cim = _run_composite_cim()
    total_bytes = 0
    free_bytes = 0
    with contextlib.suppress(ValueError):
        total_bytes = int(cim.get("DISK_SIZE", "0"))
    with contextlib.suppress(ValueError):
        free_bytes = int(cim.get("DISK_FREE", "0"))

    is_ssd = "SSD" in cim.get("DISK_MEDIA", "").upper()

    return DiskInfo(
        total_gb=total_bytes // (1024**3),
        free_gb=free_bytes // (1024**3),
        is_ssd=is_ssd,
    )


@functools.lru_cache(maxsize=1)
def detect_hyperv() -> bool:
    """Return True if Hyper-V is available / enabled."""
    if os.name != "nt":
        return False
    cim = _run_composite_cim()
    return cim.get("HYPERV", "").strip().lower() == "true"


@functools.lru_cache(maxsize=1)
def detect_wsl() -> bool:
    """Return True if WSL appears installed."""
    if os.name != "nt":
        return False
    try:
        r = subprocess.run(
            ["wsl", "--status"],
            capture_output=True,
            text=True,
            timeout=5,
        )
        return r.returncode == 0
    except Exception:
        return False


@functools.lru_cache(maxsize=1)
def detect_tpm() -> bool:
    """Return True if a TPM chip is present."""
    if os.name != "nt":
        return False
    raw = _run_ps("(Get-Tpm -ErrorAction SilentlyContinue).TpmPresent")
    return raw.strip().lower() == "true"


@functools.lru_cache(maxsize=1)
def detect_secure_boot() -> bool:
    """Return True if Secure Boot is active."""
    if os.name != "nt":
        return False
    raw = _run_ps("Confirm-SecureBootUEFI -ErrorAction SilentlyContinue")
    return raw.strip().lower() == "true"


# ── Composite profile ───────────────────────────────────────────────────────


def _compute_optimal_workers(cpu: CPUInfo, mem: MemoryInfo) -> int:
    """Derive the best thread-pool size for status detection.

    Heuristic: use logical cores capped at 12, but reduce if RAM is tight.
    """
    base = min(cpu.cores_logical, 12)
    if mem.total_mb > 0 and mem.total_mb < 4096:
        base = max(2, base // 2)
    return max(2, base)


def _compute_batch_size(cpu: CPUInfo, mem: MemoryInfo) -> int:
    """Derive the GUI category batch size — larger batches on fast machines."""
    if cpu.cores_logical >= 8 and mem.total_mb >= 16384:
        return 8
    if cpu.cores_logical >= 4 and mem.total_mb >= 8192:
        return 6
    return 4


@functools.lru_cache(maxsize=1)
def detect_hardware() -> HWProfile:
    """Run all probes and return a composite :class:`HWProfile`.

    The composite CIM query runs once for CPU/GPU/RAM/Disk/HyperV.
    Independent probes (WSL, TPM, SecureBoot) run in parallel threads.
    """
    # Trigger the composite CIM query once (populates _CIM_CACHE)
    cpu = detect_cpu()
    gpus = detect_gpus()
    mem = detect_memory()
    disk = detect_disk()

    # Run independent probes in parallel (each is a separate subprocess)
    with concurrent.futures.ThreadPoolExecutor(max_workers=4) as pool:
        f_hyperv = pool.submit(detect_hyperv)
        f_wsl = pool.submit(detect_wsl)
        f_tpm = pool.submit(detect_tpm)
        f_secboot = pool.submit(detect_secure_boot)

        has_hyperv = f_hyperv.result(timeout=15)
        has_wsl = f_wsl.result(timeout=15)
        has_tpm = f_tpm.result(timeout=15)
        has_secboot = f_secboot.result(timeout=15)

    build = 0
    with contextlib.suppress(ValueError, AttributeError):
        build = int(platform.version().split(".")[-1])

    return HWProfile(
        cpu=cpu,
        gpus=gpus,
        memory=mem,
        disk=disk,
        has_hyperv=has_hyperv,
        has_wsl=has_wsl,
        has_tpm=has_tpm,
        has_secure_boot=has_secboot,
        windows_build=build,
        optimal_workers=_compute_optimal_workers(cpu, mem),
        gui_batch_size=_compute_batch_size(cpu, mem),
    )


# ── Profile suggestion ───────────────────────────────────────────────────────


def clear_caches() -> None:
    """Reset all hardware detection caches — useful for testing."""
    global _CIM_CACHE
    with _CIM_LOCK:
        _CIM_CACHE = None
    for fn in (detect_cpu, detect_gpus, detect_memory, detect_disk, detect_hyperv, detect_wsl, detect_tpm, detect_secure_boot, detect_hardware):
        fn.cache_clear()


def suggest_profile(hw: HWProfile | None = None) -> str:
    """Suggest the most appropriate profile based on detected hardware.

    Returns one of: ``business``, ``gaming``, ``privacy``, ``minimal``, ``server``.
    """
    if hw is None:
        hw = detect_hardware()

    has_dgpu = any(
        g.adapter_ram_mb >= 1024 and not re.search(r"Microsoft|Basic|Remote|Virtual|Intel.*UHD|Intel.*HD", g.name, re.IGNORECASE) for g in hw.gpus
    )
    low_ram = hw.memory.total_mb > 0 and hw.memory.total_mb < 8192
    is_server_build = hw.windows_build > 0 and (hw.has_hyperv and hw.cpu.cores_logical >= 8 and not has_dgpu)

    if is_server_build:
        return "server"
    if has_dgpu and hw.memory.total_mb >= 16384:
        return "gaming"
    if low_ram:
        return "minimal"
    return "business"


# ── Human-readable summary ───────────────────────────────────────────────────


def hardware_summary(hw: HWProfile | None = None) -> str:
    """Return a compact multi-line description for the About dialog / CLI."""
    if hw is None:
        hw = detect_hardware()

    lines = [
        f"CPU: {hw.cpu.name}",
        f"     {hw.cpu.cores_physical}P / {hw.cpu.cores_logical}L cores  ({hw.cpu.arch})",
    ]
    if hw.gpus:
        for i, g in enumerate(hw.gpus):
            ram = f" ({g.adapter_ram_mb} MB)" if g.adapter_ram_mb else ""
            lines.append(f"GPU{i}: {g.name}{ram}")
    else:
        lines.append("GPU: not detected")
    if hw.memory.total_mb:
        lines.append(f"RAM: {hw.memory.total_mb // 1024} GB total, {hw.memory.available_mb // 1024} GB free")
    if hw.disk.total_gb:
        ssd = " (SSD)" if hw.disk.is_ssd else ""
        lines.append(f"Disk C: {hw.disk.total_gb} GB total, {hw.disk.free_gb} GB free{ssd}")

    caps: list[str] = []
    if hw.has_hyperv:
        caps.append("Hyper-V")
    if hw.has_wsl:
        caps.append("WSL")
    if hw.has_tpm:
        caps.append("TPM")
    if hw.has_secure_boot:
        caps.append("SecureBoot")
    if caps:
        lines.append(f"Caps: {', '.join(caps)}")

    lines.append(f"Tuning: {hw.optimal_workers} workers, batch {hw.gui_batch_size}")
    return "\n".join(lines)
