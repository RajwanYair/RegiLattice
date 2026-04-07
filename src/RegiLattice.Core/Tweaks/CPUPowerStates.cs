#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

internal static class CPUPowerStates
{
    // Processor performance subgroup GUID: 54533251-82be-4824-96c1-47b60b740d00
    private const string ProcSubgroupKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00";
    private const string MinProcStateKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\893dee8e-2bef-41e0-89c6-b55d0929964c";
    private const string MaxProcStateKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\bc5038f7-23e0-4960-96da-33abaf5935ec";
    private const string ProcBoostKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\be337238-0d82-4146-a960-4f3749d470c7";
    private const string ProcEppKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\36687f9e-e3a5-4dbf-b1dc-15eb381c6863";
    private const string CoreParkingKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583";
    private const string ThrottleStatesKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\aa006c4e-1c58-4f95-a560-bfa9d6f7e2f2";
    private const string IdlePromoteKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\7b224883-b3cc-4d79-819f-8374152cbe7c";
    private const string PerfIncreaseKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\465e1f50-b610-473a-ab58-00d1077dc418";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "cpupwr-expose-min-proc-state",
            Label = "Expose Minimum Processor State Setting in Power Options UI",
            Category = "CPU Power States",
            Description =
                "Unhides the Minimum Processor State (C-state floor) power setting in the advanced Power Options dialog. Allows per-plan configuration of the minimum CPU frequency percentage (0–100%), balancing idle power savings against application responsiveness.",
            Tags = ["cpu", "power", "processor", "frequency", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Exposes minimum CPU frequency control in Power Options UI; reduceable to 5% for power saving.",
            ApplyOps = [RegOp.SetDword(MinProcStateKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(MinProcStateKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(MinProcStateKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "cpupwr-expose-max-proc-state",
            Label = "Expose Maximum Processor State Setting in Power Options UI",
            Category = "CPU Power States",
            Description =
                "Unhides the Maximum Processor State setting in the Power Options advanced dialog. Enables per-plan frequency capping — setting to 99% can prevent CPU Turbo Boost, reducing heat and power consumption on battery.",
            Tags = ["cpu", "power", "processor", "frequency", "turbo", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enables CPU cap in Power Options UI; set to 99% on DC plan to prevent turbo boost.",
            ApplyOps = [RegOp.SetDword(MaxProcStateKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(MaxProcStateKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(MaxProcStateKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "cpupwr-expose-processor-boost-mode",
            Label = "Expose Processor Performance Boost Mode in Power Options UI",
            Category = "CPU Power States",
            Description =
                "Unhides the Processor Performance Boost Mode setting in Power Options, allowing per-plan control of CPU Turbo Boost behaviour: Disabled, Enabled, Aggressive, or Efficient Aggressive.",
            Tags = ["cpu", "power", "processor", "boost", "turbo", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Exposes Turbo Boost control per-plan in Power Options UI.",
            ApplyOps = [RegOp.SetDword(ProcBoostKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(ProcBoostKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(ProcBoostKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "cpupwr-expose-epp-setting",
            Label = "Expose Energy Performance Preference (EPP) in Power Options UI",
            Category = "CPU Power States",
            Description =
                "Unhides the Energy Performance Preference (EPP) slider setting in Power Options. EPP is the CPPC2 hint sent to the CPU on modern Intel/AMD platforms to bias the hardware scheduler between maximum performance (0) and maximum power saving (255).",
            Tags = ["cpu", "power", "processor", "epp", "amd", "intel", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Exposes the hardware-level performance/power balance hint for modern CPUs in Power Options.",
            ApplyOps = [RegOp.SetDword(ProcEppKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(ProcEppKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(ProcEppKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "cpupwr-expose-core-parking",
            Label = "Expose Processor Core Parking Setting in Power Options UI",
            Category = "CPU Power States",
            Description =
                "Unhides the Processor Core Parking Minimum Cores setting in Power Options. Core parking allows the OS to power-down CPU cores that aren't needed, reducing power consumption on multi-core systems at the cost of some scheduling latency.",
            Tags = ["cpu", "power", "processor", "core-parking", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Exposes core parking percentage control in Power Options UI.",
            ApplyOps = [RegOp.SetDword(CoreParkingKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(CoreParkingKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(CoreParkingKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "cpupwr-expose-throttle-states",
            Label = "Expose Allow Throttle States Setting in Power Options UI",
            Category = "CPU Power States",
            Description =
                "Unhides the 'Allow Throttle States' setting in Power Options, which controls whether the OS permits CPU thermal/power throttling. Exposing this setting allows power users to prevent throttling in sustained workloads.",
            Tags = ["cpu", "power", "processor", "throttle", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Exposes throttle state allowance per-plan in the Power Options advanced dialog.",
            ApplyOps = [RegOp.SetDword(ThrottleStatesKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(ThrottleStatesKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(ThrottleStatesKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "cpupwr-proc-subgroup-visible",
            Label = "Ensure Processor Power Management Subgroup is Visible",
            Category = "CPU Power States",
            Description =
                "Sets Attributes=0 on the Processor Power Management subgroup to ensure it is always visible in the Power Options advanced settings dialog, even on plans where it is normally hidden (e.g., Balanced plan).",
            Tags = ["cpu", "power", "processor", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Restores Processor Power Management group visibility in advanced Power Options.",
            ApplyOps = [RegOp.SetDword(ProcSubgroupKey, "Attributes", 0)],
            RemoveOps = [RegOp.SetDword(ProcSubgroupKey, "Attributes", 0)],
            DetectOps = [RegOp.CheckDword(ProcSubgroupKey, "Attributes", 0)],
        },
        new TweakDef
        {
            Id = "cpupwr-expose-idle-promote",
            Label = "Expose Processor Idle Promote Threshold in Power Options UI",
            Category = "CPU Power States",
            Description =
                "Unhides the Processor Idle Promote Threshold setting in Power Options. This value controls the CPU utilisation percentage below which the OS promotes deeper C-states (lower power idle states). Lower values = more aggressive power saving.",
            Tags = ["cpu", "power", "processor", "idle", "c-states", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Exposes C-state promotion threshold control in Power Options UI.",
            ApplyOps = [RegOp.SetDword(IdlePromoteKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(IdlePromoteKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(IdlePromoteKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "cpupwr-expose-perf-increase-policy",
            Label = "Expose Processor Performance Increase Policy in Power Options",
            Category = "CPU Power States",
            Description =
                "Unhides the Processor Performance Increase Policy setting in Power Options. This controls how aggressively the OS ramps up CPU frequency in response to load: 0=None, 1=Ideal, 2=Single (quick burst only), 3=Rocket (aggressive).",
            Tags = ["cpu", "power", "processor", "frequency", "boost", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Exposes CPU frequency ramp-up policy in Power Options UI.",
            ApplyOps = [RegOp.SetDword(PerfIncreaseKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(PerfIncreaseKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(PerfIncreaseKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "cpupwr-epp-balanced-dc",
            Label = "Set EPP to Balanced (128) on Battery Power",
            Category = "CPU Power States",
            Description =
                "Sets the Energy Performance Preference to 128 (balanced midpoint) for DC (battery) power. Configures the CPU hardware scheduler hint to balance performance and power consumption symmetrically when running on battery.",
            Tags = ["cpu", "power", "processor", "epp", "battery"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Balances CPU performance and battery life when on DC power.",
            ApplyOps = [RegOp.SetDword(ProcEppKey, "DCSettingIndex", 128)],
            RemoveOps = [RegOp.DeleteValue(ProcEppKey, "DCSettingIndex")],
            DetectOps = [RegOp.CheckDword(ProcEppKey, "DCSettingIndex", 128)],
        },
    ];
}
