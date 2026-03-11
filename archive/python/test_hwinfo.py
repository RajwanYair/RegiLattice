"""Tests for regilattice.hwinfo — hardware detection and adaptive configuration."""

from __future__ import annotations

from unittest.mock import MagicMock, patch

import pytest

from regilattice.hwinfo import (
    CPUInfo,
    DiskInfo,
    GPUInfo,
    HWProfile,
    MemoryInfo,
    _compute_batch_size,
    _compute_optimal_workers,
    clear_caches,
    detect_battery,
    detect_network_type,
    hardware_summary,
    suggest_profile,
)

# ── Fixtures ─────────────────────────────────────────────────────────────────


@pytest.fixture()
def _clear_hw_caches() -> None:
    """Clear all lru_cache probes so each test runs fresh."""
    clear_caches()


# ── Dataclass defaults ───────────────────────────────────────────────────────


class TestDataclassDefaults:
    def test_cpu_defaults(self) -> None:
        c = CPUInfo()
        assert c.name == "Unknown"
        assert c.cores_physical == 1
        assert c.cores_logical == 1
        assert c.arch == "x86_64"

    def test_gpu_defaults(self) -> None:
        g = GPUInfo()
        assert g.name == "Unknown"
        assert g.adapter_ram_mb == 0

    def test_memory_defaults(self) -> None:
        m = MemoryInfo()
        assert m.total_mb == 0
        assert m.available_mb == 0

    def test_disk_defaults(self) -> None:
        d = DiskInfo()
        assert d.total_gb == 0
        assert d.is_ssd is False

    def test_hwprofile_defaults(self) -> None:
        hw = HWProfile()
        assert hw.optimal_workers == 4
        assert hw.gui_batch_size == 4
        assert hw.gpus == []
        assert hw.has_hyperv is False


# ── _compute_optimal_workers ─────────────────────────────────────────────────


class TestComputeOptimalWorkers:
    def test_caps_at_12(self) -> None:
        cpu = CPUInfo(cores_logical=32)
        mem = MemoryInfo(total_mb=32768)
        assert _compute_optimal_workers(cpu, mem) == 12

    def test_low_ram_halves(self) -> None:
        cpu = CPUInfo(cores_logical=8)
        mem = MemoryInfo(total_mb=2048)
        assert _compute_optimal_workers(cpu, mem) == max(2, 8 // 2)

    def test_minimum_is_two(self) -> None:
        cpu = CPUInfo(cores_logical=1)
        mem = MemoryInfo(total_mb=1024)
        assert _compute_optimal_workers(cpu, mem) >= 2

    def test_normal_machine(self) -> None:
        cpu = CPUInfo(cores_logical=8)
        mem = MemoryInfo(total_mb=16384)
        assert _compute_optimal_workers(cpu, mem) == 8

    def test_zero_ram_no_halving(self) -> None:
        cpu = CPUInfo(cores_logical=6)
        mem = MemoryInfo(total_mb=0)
        assert _compute_optimal_workers(cpu, mem) == 6


# ── _compute_batch_size ──────────────────────────────────────────────────────


class TestComputeBatchSize:
    def test_high_end(self) -> None:
        cpu = CPUInfo(cores_logical=16)
        mem = MemoryInfo(total_mb=32768)
        assert _compute_batch_size(cpu, mem) == 8

    def test_mid_range(self) -> None:
        cpu = CPUInfo(cores_logical=4)
        mem = MemoryInfo(total_mb=8192)
        assert _compute_batch_size(cpu, mem) == 6

    def test_low_end(self) -> None:
        cpu = CPUInfo(cores_logical=2)
        mem = MemoryInfo(total_mb=4096)
        assert _compute_batch_size(cpu, mem) == 4

    def test_boundary_8cores_16gb(self) -> None:
        cpu = CPUInfo(cores_logical=8)
        mem = MemoryInfo(total_mb=16384)
        assert _compute_batch_size(cpu, mem) == 8


# ── suggest_profile ──────────────────────────────────────────────────────────


class TestSuggestProfile:
    def test_gaming_profile(self) -> None:
        hw = HWProfile(
            gpus=[GPUInfo(name="NVIDIA RTX 4090", adapter_ram_mb=24576)],
            memory=MemoryInfo(total_mb=32768),
        )
        assert suggest_profile(hw) == "gaming"

    def test_server_profile(self) -> None:
        hw = HWProfile(
            cpu=CPUInfo(cores_logical=16),
            has_hyperv=True,
            windows_build=20000,
        )
        assert suggest_profile(hw) == "server"

    def test_minimal_profile_low_ram(self) -> None:
        hw = HWProfile(memory=MemoryInfo(total_mb=4096))
        assert suggest_profile(hw) == "minimal"

    def test_business_default(self) -> None:
        hw = HWProfile(memory=MemoryInfo(total_mb=16384))
        assert suggest_profile(hw) == "business"

    def test_igpu_not_dgpu(self) -> None:
        """Intel UHD should NOT be counted as a discrete GPU."""
        hw = HWProfile(
            gpus=[GPUInfo(name="Intel UHD Graphics 770", adapter_ram_mb=2048)],
            memory=MemoryInfo(total_mb=32768),
        )
        assert suggest_profile(hw) != "gaming"

    def test_virtual_gpu_not_dgpu(self) -> None:
        hw = HWProfile(
            gpus=[GPUInfo(name="Microsoft Basic Display Adapter", adapter_ram_mb=2048)],
            memory=MemoryInfo(total_mb=32768),
        )
        assert suggest_profile(hw) != "gaming"

    def test_none_hw_calls_detect(self) -> None:
        """Passing None should still return a valid string."""
        result = suggest_profile(None)
        assert result in {"business", "gaming", "privacy", "minimal", "server"}


# ── hardware_summary ─────────────────────────────────────────────────────────


class TestHardwareSummary:
    def test_basic_format(self) -> None:
        hw = HWProfile(
            cpu=CPUInfo(name="Test CPU", cores_physical=4, cores_logical=8, arch="AMD64"),
            memory=MemoryInfo(total_mb=16384, available_mb=8192),
        )
        text = hardware_summary(hw)
        assert "Test CPU" in text
        assert "4P / 8L" in text
        assert "AMD64" in text
        assert "16 GB total" in text

    def test_gpu_listed(self) -> None:
        hw = HWProfile(
            gpus=[
                GPUInfo(name="NVIDIA RTX 4090", adapter_ram_mb=24576),
                GPUInfo(name="Intel UHD 770", adapter_ram_mb=0),
            ],
        )
        text = hardware_summary(hw)
        assert "GPU0: NVIDIA RTX 4090 (24576 MB)" in text
        assert "GPU1: Intel UHD 770" in text

    def test_no_gpu(self) -> None:
        hw = HWProfile()
        text = hardware_summary(hw)
        assert "not detected" in text

    def test_disk_ssd(self) -> None:
        hw = HWProfile(disk=DiskInfo(total_gb=500, free_gb=200, is_ssd=True))
        text = hardware_summary(hw)
        assert "(SSD)" in text
        assert "500 GB total" in text

    def test_capabilities_listed(self) -> None:
        hw = HWProfile(has_hyperv=True, has_wsl=True, has_tpm=True, has_secure_boot=True)
        text = hardware_summary(hw)
        assert "Hyper-V" in text
        assert "WSL" in text
        assert "TPM" in text
        assert "SecureBoot" in text

    def test_tuning_line(self) -> None:
        hw = HWProfile(optimal_workers=8, gui_batch_size=6)
        text = hardware_summary(hw)
        assert "8 workers" in text
        assert "batch 6" in text

    def test_none_hw_returns_string(self) -> None:
        result = hardware_summary(None)
        assert isinstance(result, str)
        assert "CPU:" in result


# ── Probe functions with mocking ─────────────────────────────────────────────


@pytest.mark.usefixtures("_clear_hw_caches")
class TestDetectCPU:
    @patch("regilattice.hwinfo.os.name", "posix")
    def test_non_windows_returns_defaults(self) -> None:
        from regilattice.hwinfo import detect_cpu

        cpu = detect_cpu()
        assert cpu.cores_logical >= 1
        # Name stays "Unknown" on non-NT
        assert cpu.name == "Unknown"

    @patch("regilattice.hwinfo.os.name", "nt")
    @patch("regilattice.hwinfo._run_composite_cim", return_value={})
    @patch.dict("os.environ", {"PROCESSOR_IDENTIFIER": "TestCPU", "NUMBER_OF_PROCESSORS": "16"})
    def test_env_vars_used(self, _mock_cim: MagicMock) -> None:
        from regilattice.hwinfo import detect_cpu

        cpu = detect_cpu()
        assert cpu.name == "TestCPU"
        assert cpu.cores_logical == 16

    @patch("regilattice.hwinfo.os.name", "nt")
    @patch("regilattice.hwinfo._run_composite_cim", return_value={"CPU_NAME": "Intel Core i9-14900K", "CPU_CORES": "24"})
    @patch.dict("os.environ", {"PROCESSOR_IDENTIFIER": "EnvCPU", "NUMBER_OF_PROCESSORS": "8"})
    def test_cim_name_overrides_env(self, _mock_cim: MagicMock) -> None:
        from regilattice.hwinfo import detect_cpu

        cpu = detect_cpu()
        assert cpu.name == "Intel Core i9-14900K"
        assert cpu.cores_physical == 24


@pytest.mark.usefixtures("_clear_hw_caches")
class TestDetectGPUs:
    @patch("regilattice.hwinfo.os.name", "posix")
    def test_non_windows_returns_empty(self) -> None:
        from regilattice.hwinfo import detect_gpus

        assert detect_gpus() == []

    @patch("regilattice.hwinfo.os.name", "nt")
    @patch(
        "regilattice.hwinfo._run_composite_cim",
        return_value={
            "GPU:0": "NVIDIA RTX 4090|31.0.15.4601|25769803776",
            "GPU_COUNT": "1",
        },
    )
    def test_single_gpu_parsed(self, _mock_cim: MagicMock) -> None:
        from regilattice.hwinfo import detect_gpus

        gpus = detect_gpus()
        assert len(gpus) == 1
        assert gpus[0].name == "NVIDIA RTX 4090"
        assert gpus[0].driver_version == "31.0.15.4601"
        assert gpus[0].adapter_ram_mb == 25769803776 // (1024 * 1024)

    @patch("regilattice.hwinfo.os.name", "nt")
    @patch(
        "regilattice.hwinfo._run_composite_cim",
        return_value={
            "GPU:0": "GPU1|1.0|1073741824",
            "GPU:1": "GPU2|2.0|2147483648",
            "GPU_COUNT": "2",
        },
    )
    def test_multiple_gpus(self, _mock_cim: MagicMock) -> None:
        from regilattice.hwinfo import detect_gpus

        gpus = detect_gpus()
        assert len(gpus) == 2
        assert gpus[0].name == "GPU1"
        assert gpus[1].name == "GPU2"

    @patch("regilattice.hwinfo.os.name", "nt")
    @patch(
        "regilattice.hwinfo._run_composite_cim",
        return_value={
            "GPU:0": "MyGPU|1.0|notanumber",
            "GPU_COUNT": "1",
        },
    )
    def test_bad_ram_value_defaults_zero(self, _mock_cim: MagicMock) -> None:
        from regilattice.hwinfo import detect_gpus

        gpus = detect_gpus()
        assert len(gpus) == 1
        assert gpus[0].adapter_ram_mb == 0


@pytest.mark.usefixtures("_clear_hw_caches")
class TestDetectMemory:
    @patch("regilattice.hwinfo.os.name", "posix")
    def test_non_windows_returns_defaults(self) -> None:
        from regilattice.hwinfo import detect_memory

        mem = detect_memory()
        assert mem.total_mb == 0

    @patch("regilattice.hwinfo.os.name", "nt")
    @patch("regilattice.hwinfo._ctypes_memory_mb", return_value=None)
    @patch(
        "regilattice.hwinfo._run_composite_cim",
        return_value={
            "MEM_TOTAL": "16777216",
            "MEM_FREE": "8388608",
        },
    )
    def test_memory_parsed(self, _mock_cim: MagicMock, _mock_ctypes: MagicMock) -> None:
        from regilattice.hwinfo import detect_memory

        mem = detect_memory()
        assert mem.total_mb == 16777216 // 1024
        assert mem.available_mb == 8388608 // 1024


@pytest.mark.usefixtures("_clear_hw_caches")
class TestDetectDisk:
    @patch("regilattice.hwinfo.os.name", "posix")
    def test_non_windows_returns_defaults(self) -> None:
        from regilattice.hwinfo import detect_disk

        d = detect_disk()
        assert d.total_gb == 0
        assert d.is_ssd is False

    @patch("regilattice.hwinfo.os.name", "nt")
    @patch(
        "regilattice.hwinfo._run_composite_cim",
        return_value={
            "DISK_SIZE": "536870912000",
            "DISK_FREE": "268435456000",
            "DISK_MEDIA": "SSD",
        },
    )
    def test_disk_with_ssd(self, _mock_cim: MagicMock) -> None:
        from regilattice.hwinfo import detect_disk

        d = detect_disk()
        assert d.total_gb == 536870912000 // (1024**3)
        assert d.is_ssd is True


@pytest.mark.usefixtures("_clear_hw_caches")
class TestDetectBooleans:
    @patch("regilattice.hwinfo.os.name", "posix")
    def test_non_windows_all_false(self) -> None:
        from regilattice.hwinfo import detect_hyperv, detect_secure_boot, detect_tpm, detect_wsl

        assert detect_hyperv() is False
        assert detect_wsl() is False
        assert detect_tpm() is False
        assert detect_secure_boot() is False

    @patch("regilattice.hwinfo.os.name", "nt")
    @patch("regilattice.hwinfo._run_composite_cim", return_value={"HYPERV": "True"})
    def test_hyperv_true(self, _mock_cim: MagicMock) -> None:
        from regilattice.hwinfo import detect_hyperv

        assert detect_hyperv() is True

    @patch("regilattice.hwinfo.os.name", "nt")
    @patch("regilattice.hwinfo._run_ps", return_value="True")
    def test_tpm_true(self, _mock_ps: MagicMock) -> None:
        from regilattice.hwinfo import detect_tpm

        assert detect_tpm() is True

    @patch("regilattice.hwinfo.os.name", "nt")
    @patch("regilattice.hwinfo._run_ps", return_value="True")
    def test_secure_boot_true(self, _mock_ps: MagicMock) -> None:
        from regilattice.hwinfo import detect_secure_boot

        assert detect_secure_boot() is True


# ── detect_hardware composite ────────────────────────────────────────────────


@pytest.mark.usefixtures("_clear_hw_caches")
class TestDetectHardware:
    @patch("regilattice.hwinfo.detect_secure_boot", return_value=False)
    @patch("regilattice.hwinfo.detect_tpm", return_value=True)
    @patch("regilattice.hwinfo.detect_wsl", return_value=True)
    @patch("regilattice.hwinfo.detect_hyperv", return_value=False)
    @patch("regilattice.hwinfo.detect_disk", return_value=DiskInfo(total_gb=500, free_gb=200, is_ssd=True))
    @patch("regilattice.hwinfo.detect_memory", return_value=MemoryInfo(total_mb=16384, available_mb=8000))
    @patch("regilattice.hwinfo.detect_gpus", return_value=[GPUInfo(name="TestGPU", adapter_ram_mb=4096)])
    @patch("regilattice.hwinfo.detect_cpu", return_value=CPUInfo(name="TestCPU", cores_physical=8, cores_logical=16, arch="AMD64"))
    def test_composite_profile(self, *_mocks: MagicMock) -> None:
        from regilattice.hwinfo import detect_hardware

        hw = detect_hardware()
        assert hw.cpu.name == "TestCPU"
        assert hw.cpu.cores_logical == 16
        assert len(hw.gpus) == 1
        assert hw.memory.total_mb == 16384
        assert hw.disk.is_ssd is True
        assert hw.has_wsl is True
        assert hw.has_tpm is True
        assert hw.optimal_workers <= 12
        assert hw.gui_batch_size >= 4


# ── Composite CIM query ─────────────────────────────────────────────────────


class TestCompositeCIM:
    def test_clear_caches_resets_cim(self) -> None:
        import regilattice.hwinfo as hwmod

        hwmod._CIM_CACHE = {"FOO": "bar"}
        clear_caches()
        assert hwmod._CIM_CACHE is None

    @patch(
        "regilattice.hwinfo._run_ps",
        return_value=(
            "CPU_NAME:Test CPU\nCPU_CORES:8\nHYPERV:True\n"
            "MEM_TOTAL:16000000\nMEM_FREE:8000000\n"
            "DISK_SIZE:500000000000\nDISK_FREE:250000000000\nDISK_MEDIA:SSD\n"
            "GPU:NVIDIA Test|1.0|4294967296\n"
        ),
    )
    def test_composite_parse(self, _mock_ps: MagicMock) -> None:
        from regilattice.hwinfo import _run_composite_cim

        clear_caches()
        result = _run_composite_cim()
        assert result["CPU_NAME"] == "Test CPU"
        assert result["CPU_CORES"] == "8"
        assert result["HYPERV"] == "True"
        assert result["GPU:0"] == "NVIDIA Test|1.0|4294967296"
        assert result["GPU_COUNT"] == "1"

    @patch("regilattice.hwinfo._run_ps", return_value="")
    def test_composite_empty_response(self, _mock_ps: MagicMock) -> None:
        from regilattice.hwinfo import _run_composite_cim

        clear_caches()
        result = _run_composite_cim()
        assert result["GPU_COUNT"] == "0"


# ── detect_battery tests ─────────────────────────────────────────────────────


class TestDetectBattery:
    """Tests for detect_battery()."""

    def test_returns_bool(self) -> None:
        clear_caches()
        with patch("regilattice.hwinfo._run_ps", return_value="0"):
            result = detect_battery()
        assert isinstance(result, bool)

    def test_no_battery_when_count_zero(self) -> None:
        clear_caches()
        with patch("regilattice.hwinfo._run_ps", return_value="0"):
            assert detect_battery() is False

    def test_battery_present_when_count_one(self) -> None:
        clear_caches()
        with patch("regilattice.hwinfo._run_ps", return_value="1"):
            assert detect_battery() is True

    def test_battery_present_when_count_two(self) -> None:
        """Dual-battery machines (e.g. ThinkPad) should still return True."""
        clear_caches()
        with patch("regilattice.hwinfo._run_ps", return_value="2"):
            assert detect_battery() is True

    def test_non_integer_response_returns_false(self) -> None:
        clear_caches()
        with patch("regilattice.hwinfo._run_ps", return_value="not_a_number"):
            assert detect_battery() is False

    def test_empty_response_returns_false(self) -> None:
        clear_caches()
        with patch("regilattice.hwinfo._run_ps", return_value=""):
            assert detect_battery() is False

    def test_result_is_cached(self) -> None:
        clear_caches()
        with patch("regilattice.hwinfo._run_ps", return_value="1") as mock_ps:
            detect_battery()
            detect_battery()
        assert mock_ps.call_count == 1


# ── detect_network_type tests ────────────────────────────────────────────────


class TestDetectNetworkType:
    """Tests for detect_network_type()."""

    def test_returns_str(self) -> None:
        clear_caches()
        with patch("regilattice.hwinfo._run_ps", return_value=""):
            result = detect_network_type()
        assert isinstance(result, str)

    def test_empty_adapter_list_returns_unknown(self) -> None:
        clear_caches()
        with patch("regilattice.hwinfo._run_ps", return_value=""):
            assert detect_network_type() == "unknown"

    def test_wifi_adapter_detected(self) -> None:
        clear_caches()
        with patch("regilattice.hwinfo._run_ps", return_value="Intel Wireless-AC 9560"):
            assert detect_network_type() == "wifi"

    def test_802_11_suffix_detected_as_wifi(self) -> None:
        clear_caches()
        with patch("regilattice.hwinfo._run_ps", return_value="Broadcom 802.11ac Network Adapter"):
            assert detect_network_type() == "wifi"

    def test_ethernet_adapter_detected(self) -> None:
        clear_caches()
        with patch("regilattice.hwinfo._run_ps", return_value="Intel Ethernet Connection I219-V"):
            assert detect_network_type() == "ethernet"

    def test_vpn_takes_priority_over_wifi(self) -> None:
        """When both VPN and Wi-Fi adapters present, VPN should win."""
        clear_caches()
        adapters = "Cisco AnyConnect Virtual Miniport Adapter\nIntel Wireless-AC 9560"
        with patch("regilattice.hwinfo._run_ps", return_value=adapters):
            assert detect_network_type() == "vpn"

    def test_vpn_cisco_detected(self) -> None:
        clear_caches()
        with patch("regilattice.hwinfo._run_ps", return_value="Cisco VPN Adapter"):
            assert detect_network_type() == "vpn"

    def test_vpn_wireguard_detected(self) -> None:
        clear_caches()
        with patch("regilattice.hwinfo._run_ps", return_value="WireGuard Tunnel"):
            assert detect_network_type() == "vpn"

    def test_result_is_cached(self) -> None:
        clear_caches()
        with patch("regilattice.hwinfo._run_ps", return_value="Intel Ethernet Connection") as mock_ps:
            detect_network_type()
            detect_network_type()
        assert mock_ps.call_count == 1

    def test_valid_scope_values(self) -> None:
        clear_caches()
        valid = {"vpn", "wifi", "ethernet", "unknown"}
        for resp in ("", "Cisco AnyConnect", "Intel Wireless-AC 9560 WiFi", "Realtek Ethernet"):
            clear_caches()
            with patch("regilattice.hwinfo._run_ps", return_value=resp):
                assert detect_network_type() in valid


# ── HWProfile new fields ─────────────────────────────────────────────────────


class TestHWProfileNewFields:
    """Tests for has_battery and network_type fields on HWProfile."""

    def test_has_battery_default_false(self) -> None:
        hw = HWProfile()
        assert hw.has_battery is False

    def test_network_type_default_unknown(self) -> None:
        hw = HWProfile()
        assert hw.network_type == "unknown"

    def test_has_battery_set_true(self) -> None:
        hw = HWProfile(has_battery=True)
        assert hw.has_battery is True

    def test_network_type_wifi(self) -> None:
        hw = HWProfile(network_type="wifi")
        assert hw.network_type == "wifi"

    def test_hardware_summary_includes_network_type(self) -> None:
        hw = HWProfile(network_type="ethernet")
        summary = hardware_summary(hw)
        assert "ethernet" in summary

    def test_hardware_summary_includes_battery_when_present(self) -> None:
        hw = HWProfile(has_battery=True, network_type="wifi")
        summary = hardware_summary(hw)
        assert "battery" in summary.lower() or "laptop" in summary.lower()

    def test_hardware_summary_no_battery_line_when_absent(self) -> None:
        hw = HWProfile(has_battery=False, network_type="ethernet")
        summary = hardware_summary(hw)
        # The word "battery" should not appear when has_battery=False
        assert "battery" not in summary.lower()
