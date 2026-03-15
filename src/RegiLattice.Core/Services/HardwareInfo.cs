// RegiLattice.Core — Services/HardwareInfo.cs
// Hardware detection using WMI and P/Invoke.

using System.Collections.Concurrent;
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
        if (_cached is not null)
            return _cached;

        // Parallelize WMI queries for faster startup
        CpuInfo cpu = null!;
        IReadOnlyList<GpuInfo> gpus = null!;
        DiskInfo disk = null!;
        bool hasHyperV = false,
            hasWsl = false,
            hasTpm = false,
            hasSecureBoot = false,
            hasBattery = false;

        Task.WaitAll(
            Task.Run(() => cpu = DetectCpu()),
            Task.Run(() => gpus = DetectGpus()),
            Task.Run(() => disk = DetectDisk()),
            Task.Run(() => hasHyperV = CheckHyperV()),
            Task.Run(() => hasWsl = CheckWsl()),
            Task.Run(() => hasTpm = CheckTpm()),
            Task.Run(() => hasSecureBoot = CheckSecureBoot()),
            Task.Run(() => hasBattery = CheckBattery())
        );

        var mem = DetectMemory();
        var build = TweakEngine.WindowsBuild();

        _cached = new HwProfile
        {
            Cpu = cpu,
            Gpus = gpus,
            Memory = mem,
            Disk = disk,
            HasHyperV = hasHyperV,
            HasWsl = hasWsl,
            HasTpm = hasTpm,
            HasSecureBoot = hasSecureBoot,
            HasBattery = hasBattery,
            WindowsBuild = build,
            OptimalWorkers = Math.Max(1, cpu.LogicalCores / 2),
            GuiBatchSize = mem.TotalMb > 8192 ? 8 : 4,
        };
        return _cached;
    }

    // ── Quick applicability checks (for TweakDef.IsApplicable) ─────────

    /// <summary>Returns true if an NVIDIA GPU is detected.</summary>
    public static bool HasNvidiaGpu() => DetectHardware().Gpus.Any(g => g.Name.Contains("NVIDIA", StringComparison.OrdinalIgnoreCase));

    /// <summary>Returns true if an AMD/Radeon GPU is detected.</summary>
    public static bool HasAmdGpu() =>
        DetectHardware()
            .Gpus.Any(g =>
                g.Name.Contains("Radeon", StringComparison.OrdinalIgnoreCase) || g.Name.Contains("AMD", StringComparison.OrdinalIgnoreCase)
            );

    /// <summary>Returns true if WSL is installed.</summary>
    public static bool HasWslInstalled() => DetectHardware().HasWsl;

    /// <summary>Returns true if the device has a battery (laptop).</summary>
    public static bool HasBatteryPresent() => DetectHardware().HasBattery;

    /// <summary>Returns true if Hyper-V is available.</summary>
    public static bool HasHyperVAvailable() => DetectHardware().HasHyperV;

    // ── Software detection (cached per session) ─────────────────────────

    private static readonly ConcurrentDictionary<string, bool> _softwareCache = new(StringComparer.OrdinalIgnoreCase);

    private static bool CachedCheck(string key, Func<bool> detect) => _softwareCache.GetOrAdd(key, _ => detect());

    /// <summary>Returns true if Google Chrome is installed.</summary>
    public static bool IsChromeInstalled() =>
        CachedCheck(
            "chrome",
            () =>
                Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe", null, null)
                    is not null
        );

    /// <summary>Returns true if Mozilla Firefox is installed.</summary>
    public static bool IsFirefoxInstalled() =>
        CachedCheck(
            "firefox",
            () =>
                Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\firefox.exe", null, null)
                    is not null
        );

    /// <summary>Returns true if Microsoft Edge is installed.</summary>
    public static bool IsEdgeInstalled() =>
        CachedCheck(
            "edge",
            () =>
                Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\msedge.exe", null, null)
                    is not null
        );

    /// <summary>Returns true if Java (JRE/JDK) is installed.</summary>
    public static bool IsJavaInstalled() =>
        CachedCheck(
            "java",
            () =>
                Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment", "CurrentVersion", null)
                    is not null
                || Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\JDK", "CurrentVersion", null) is not null
        );

    /// <summary>Returns true if Docker Desktop is installed.</summary>
    public static bool IsDockerInstalled() =>
        CachedCheck(
            "docker",
            () =>
                File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Docker", "Docker", "Docker Desktop.exe"))
        );

    /// <summary>Returns true if Adobe Creative Cloud is installed.</summary>
    public static bool IsAdobeInstalled() =>
        CachedCheck(
            "adobe",
            () =>
                Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Adobe\Adobe Creative Cloud", null, null) is not null
                || Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Adobe"))
        );

    /// <summary>Returns true if LibreOffice is installed.</summary>
    public static bool IsLibreOfficeInstalled() =>
        CachedCheck("libreoffice", () => Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\LibreOffice", null, null) is not null);

    /// <summary>Returns true if Microsoft Office is installed.</summary>
    public static bool IsOfficeInstalled() =>
        CachedCheck(
            "office",
            () => Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\ClickToRun", "InstallPath", null) is not null
        );

    /// <summary>Returns true if RealVNC is installed.</summary>
    public static bool IsRealVncInstalled() =>
        CachedCheck("realvnc", () => Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC", null, null) is not null);

    /// <summary>Returns true if VS Code is installed.</summary>
    public static bool IsVsCodeInstalled() =>
        CachedCheck(
            "vscode",
            () =>
                Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\code.cmd", null, null)
                    is not null
                || File.Exists(
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Programs",
                        "Microsoft VS Code",
                        "Code.exe"
                    )
                )
        );

    /// <summary>Returns true if Scoop package manager is installed.</summary>
    public static bool IsScoopInstalled() =>
        CachedCheck(
            "scoop",
            () => Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "shims"))
        );

    public static CpuInfo DetectCpu()
    {
        var name = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER") ?? "Unknown";
        int physical = 0,
            logical = Environment.ProcessorCount;
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT Name, NumberOfCores FROM Win32_Processor");
            foreach (var obj in searcher.Get())
            {
                name = obj["Name"]?.ToString()?.Trim() ?? name;
                physical = Convert.ToInt32(obj["NumberOfCores"]);
            }
        }
        catch
        {
            physical = logical / 2;
        }

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
                list.Add(new GpuInfo(obj["Name"]?.ToString() ?? "Unknown", obj["DriverVersion"]?.ToString() ?? "", (int)(ram / 1024 / 1024)));
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
                using var searcher = new ManagementObjectSearcher(@"\\.\root\Microsoft\Windows\Storage", "SELECT MediaType FROM MSFT_PhysicalDisk");
                // This WMI class is only on Win8+
            }
            catch { }
            return new DiskInfo("C:", (int)(drive.TotalSize / 1024 / 1024 / 1024), (int)(drive.AvailableFreeSpace / 1024 / 1024 / 1024), isSsd);
        }
        catch
        {
            return new DiskInfo("C:", 0, 0, false);
        }
    }

    public static string SuggestProfile()
    {
        var hw = DetectHardware();
        var gpus = hw.Gpus;
        bool hasDiscreteGpu = gpus.Any(g =>
            g.Name.Contains("NVIDIA", StringComparison.OrdinalIgnoreCase)
            || g.Name.Contains("Radeon", StringComparison.OrdinalIgnoreCase)
            || g.AdapterRamMb > 2048
        );

        if (CorporateGuard.IsCorporateNetwork())
            return "business";
        if (hasDiscreteGpu && hw.Memory.TotalMb >= 16384)
            return "gaming";
        if (hw.Memory.TotalMb < 4096)
            return "minimal";
        return "privacy";
    }

    public static string Summary()
    {
        var hw = DetectHardware();
        return $"CPU: {hw.Cpu.Name} ({hw.Cpu.PhysicalCores}C/{hw.Cpu.LogicalCores}T) | "
            + $"RAM: {hw.Memory.TotalMb / 1024}GB | "
            + $"GPU: {string.Join(", ", hw.Gpus.Select(g => g.Name))} | "
            + $"Build: {hw.WindowsBuild}";
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
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss");
            return key is not null;
        }
        catch
        {
            return false;
        }
    }

    private static bool CheckTpm()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher(
                new ManagementScope(@"\\.\root\CIMv2\Security\MicrosoftTpm"),
                new ObjectQuery("SELECT IsActivated_InitialValue FROM Win32_Tpm")
            );
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
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\SecureBoot\State");
            return key?.GetValue("UEFISecureBootEnabled") is int v && v == 1;
        }
        catch
        {
            return false;
        }
    }

    private static bool CheckBattery()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT BatteryStatus FROM Win32_Battery");
            return searcher.Get().Count > 0;
        }
        catch
        {
            return false;
        }
    }
}
