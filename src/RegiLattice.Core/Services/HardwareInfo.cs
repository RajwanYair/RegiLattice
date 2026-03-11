// RegiLattice.Core — Services/HardwareInfo.cs
// Hardware detection using WMI and P/Invoke.

using System.Management;
using System.Runtime.InteropServices;

namespace RegiLattice.Core;

public sealed record CpuInfo(string Name, int PhysicalCores, int LogicalCores, string Architecture);
public sealed record GpuInfo(string Name, string DriverVersion, int AdapterRamMb);
public sealed record MemoryInfo(long TotalMb, long AvailableMb);
public sealed record DiskInfo(string Drive, long TotalGb, long FreeGb, bool IsSsd);

public sealed class HwProfile
{
    public required CpuInfo Cpu { get; init; }
    public required IReadOnlyList<GpuInfo> Gpus { get; init; }
    public required MemoryInfo Memory { get; init; }
    public required DiskInfo Disk { get; init; }
    public bool HasHyperV { get; init; }
    public bool HasWsl { get; init; }
    public bool HasTpm { get; init; }
    public bool HasSecureBoot { get; init; }
    public bool HasBattery { get; init; }
    public int WindowsBuild { get; init; }
    public int OptimalWorkers { get; init; }
    public int GuiBatchSize { get; init; }
}

/// <summary>Hardware detection using WMI and P/Invoke.</summary>
public static class HardwareInfo
{
    private static HwProfile? _cached;

    [StructLayout(LayoutKind.Sequential)]
    private struct MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

    public static HwProfile DetectHardware()
    {
        if (_cached is not null) return _cached;
        var cpu = DetectCpu();
        var gpus = DetectGpus();
        var mem = DetectMemory();
        var disk = DetectDisk();
        var build = TweakEngine.WindowsBuild();

        _cached = new HwProfile
        {
            Cpu = cpu,
            Gpus = gpus,
            Memory = mem,
            Disk = disk,
            HasHyperV = CheckHyperV(),
            HasWsl = CheckWsl(),
            HasTpm = CheckTpm(),
            HasSecureBoot = CheckSecureBoot(),
            HasBattery = CheckBattery(),
            WindowsBuild = build,
            OptimalWorkers = Math.Max(1, cpu.LogicalCores / 2),
            GuiBatchSize = mem.TotalMb > 8192 ? 8 : 4,
        };
        return _cached;
    }

    // ── Quick applicability checks (for TweakDef.IsApplicable) ─────────

    /// <summary>Returns true if an NVIDIA GPU is detected.</summary>
    public static bool HasNvidiaGpu() =>
        DetectHardware().Gpus.Any(g => g.Name.Contains("NVIDIA", StringComparison.OrdinalIgnoreCase));

    /// <summary>Returns true if an AMD/Radeon GPU is detected.</summary>
    public static bool HasAmdGpu() =>
        DetectHardware().Gpus.Any(g =>
            g.Name.Contains("Radeon", StringComparison.OrdinalIgnoreCase) ||
            g.Name.Contains("AMD", StringComparison.OrdinalIgnoreCase));

    /// <summary>Returns true if WSL is installed.</summary>
    public static bool HasWslInstalled() => DetectHardware().HasWsl;

    /// <summary>Returns true if the device has a battery (laptop).</summary>
    public static bool HasBatteryPresent() => DetectHardware().HasBattery;

    /// <summary>Returns true if Hyper-V is available.</summary>
    public static bool HasHyperVAvailable() => DetectHardware().HasHyperV;

    public static CpuInfo DetectCpu()
    {
        var name = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER") ?? "Unknown";
        int physical = 0, logical = Environment.ProcessorCount;
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT Name, NumberOfCores FROM Win32_Processor");
            foreach (var obj in searcher.Get())
            {
                name = obj["Name"]?.ToString()?.Trim() ?? name;
                physical = Convert.ToInt32(obj["NumberOfCores"]);
            }
        }
        catch { physical = logical / 2; }

        return new CpuInfo(name, Math.Max(1, physical), logical, RuntimeInformation.ProcessArchitecture.ToString());
    }

    public static IReadOnlyList<GpuInfo> DetectGpus()
    {
        var list = new List<GpuInfo>();
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT Name, DriverVersion, AdapterRAM FROM Win32_VideoController");
            foreach (var obj in searcher.Get())
            {
                var ram = Convert.ToInt64(obj["AdapterRAM"] ?? 0);
                list.Add(new GpuInfo(
                    obj["Name"]?.ToString() ?? "Unknown",
                    obj["DriverVersion"]?.ToString() ?? "",
                    (int)(ram / 1024 / 1024)));
            }
        }
        catch { }
        return list.Count > 0 ? list : [new GpuInfo("Unknown", "", 0)];
    }

    public static MemoryInfo DetectMemory()
    {
        var ms = new MEMORYSTATUSEX { dwLength = (uint)Marshal.SizeOf<MEMORYSTATUSEX>() };
        if (GlobalMemoryStatusEx(ref ms))
            return new MemoryInfo((long)(ms.ullTotalPhys / 1024 / 1024), (long)(ms.ullAvailPhys / 1024 / 1024));
        // Fallback via Environment (less precise)
        return new MemoryInfo(0, 0);
    }

    public static DiskInfo DetectDisk()
    {
        try
        {
            var drive = new DriveInfo("C");
            bool isSsd = false;
            try
            {
                using var searcher = new ManagementObjectSearcher(
                    @"\\.\root\Microsoft\Windows\Storage",
                    "SELECT MediaType FROM MSFT_PhysicalDisk");
                // This WMI class is only on Win8+
            }
            catch { }
            return new DiskInfo("C:", (int)(drive.TotalSize / 1024 / 1024 / 1024),
                (int)(drive.AvailableFreeSpace / 1024 / 1024 / 1024), isSsd);
        }
        catch { return new DiskInfo("C:", 0, 0, false); }
    }

    public static string SuggestProfile()
    {
        var hw = DetectHardware();
        var gpus = hw.Gpus;
        bool hasDiscreteGpu = gpus.Any(g =>
            g.Name.Contains("NVIDIA", StringComparison.OrdinalIgnoreCase) ||
            g.Name.Contains("Radeon", StringComparison.OrdinalIgnoreCase) ||
            g.AdapterRamMb > 2048);

        if (CorporateGuard.IsCorporateNetwork()) return "business";
        if (hasDiscreteGpu && hw.Memory.TotalMb >= 16384) return "gaming";
        if (hw.Memory.TotalMb < 4096) return "minimal";
        return "privacy";
    }

    public static string Summary()
    {
        var hw = DetectHardware();
        return $"CPU: {hw.Cpu.Name} ({hw.Cpu.PhysicalCores}C/{hw.Cpu.LogicalCores}T) | " +
               $"RAM: {hw.Memory.TotalMb / 1024}GB | " +
               $"GPU: {string.Join(", ", hw.Gpus.Select(g => g.Name))} | " +
               $"Build: {hw.WindowsBuild}";
    }

    private static bool CheckHyperV()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT HypervisorPresent FROM Win32_ComputerSystem");
            foreach (var obj in searcher.Get())
                return Convert.ToBoolean(obj["HypervisorPresent"]);
        }
        catch { }
        return false;
    }

    private static bool CheckWsl()
    {
        try
        {
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss");
            return key is not null;
        }
        catch { return false; }
    }

    private static bool CheckTpm()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher(
                new ManagementScope(@"\\.\root\CIMv2\Security\MicrosoftTpm"),
                new ObjectQuery("SELECT IsActivated_InitialValue FROM Win32_Tpm"));
            foreach (var obj in searcher.Get())
                return Convert.ToBoolean(obj["IsActivated_InitialValue"]);
        }
        catch { }
        return false;
    }

    private static bool CheckSecureBoot()
    {
        try
        {
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                @"SYSTEM\CurrentControlSet\Control\SecureBoot\State");
            return key?.GetValue("UEFISecureBootEnabled") is int v && v == 1;
        }
        catch { return false; }
    }

    private static bool CheckBattery()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT BatteryStatus FROM Win32_Battery");
            return searcher.Get().Count > 0;
        }
        catch { return false; }
    }
}
