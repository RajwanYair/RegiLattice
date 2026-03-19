// RegiLattice.Core — Services/SystemMonitor.cs
// Live system monitoring: CPU usage, memory usage, uptime.

using System.Runtime.InteropServices;

namespace RegiLattice.Core;

/// <summary>
/// Provides live system resource monitoring via lightweight P/Invoke calls.
/// CPU usage is computed from <c>GetSystemTimes</c> deltas.
/// Memory usage comes from <c>GlobalMemoryStatusEx</c>.
/// </summary>
public sealed class SystemMonitor
{
    private long _prevIdleTicks;
    private long _prevTotalTicks;

    /// <summary>Get current CPU usage as a percentage (0–100). First call returns 0.</summary>
    public int GetCpuUsagePercent()
    {
        if (!GetSystemTimes(out var idle, out var kernel, out var user))
            return 0;

        long idleTicks = FileTimeToLong(idle);
        long totalTicks = FileTimeToLong(kernel) + FileTimeToLong(user);

        long idleDelta = idleTicks - _prevIdleTicks;
        long totalDelta = totalTicks - _prevTotalTicks;

        _prevIdleTicks = idleTicks;
        _prevTotalTicks = totalTicks;

        if (totalDelta <= 0)
            return 0;

        return (int)(100 - (idleDelta * 100 / totalDelta));
    }

    /// <summary>Get current memory usage: used MB, total MB, and percentage.</summary>
    public static (long UsedMb, long TotalMb, int Percent) GetMemoryUsage()
    {
        var ms = new MEMORYSTATUSEX { dwLength = (uint)Marshal.SizeOf<MEMORYSTATUSEX>() };
        if (!GlobalMemoryStatusEx(ref ms))
            return (0, 0, 0);

        long totalMb = (long)(ms.ullTotalPhys / 1024 / 1024);
        long availMb = (long)(ms.ullAvailPhys / 1024 / 1024);
        long usedMb = totalMb - availMb;
        int percent = totalMb > 0 ? (int)(usedMb * 100 / totalMb) : 0;
        return (usedMb, totalMb, percent);
    }

    /// <summary>Get system uptime since last boot.</summary>
    public static TimeSpan GetUptime() => TimeSpan.FromMilliseconds(Environment.TickCount64);

    // ── P/Invoke ───────────────────────────────────────────────────────────

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetSystemTimes(out FILETIME idleTime, out FILETIME kernelTime, out FILETIME userTime);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

    private static long FileTimeToLong(FILETIME ft) => ((long)ft.dwHighDateTime << 32) | (uint)ft.dwLowDateTime;

    [StructLayout(LayoutKind.Sequential)]
    private struct FILETIME
    {
        public uint dwLowDateTime;
        public uint dwHighDateTime;
    }

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
}
