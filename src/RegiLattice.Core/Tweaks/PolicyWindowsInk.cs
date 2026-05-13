namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicyWindowsInk
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "winks-disable-ink-touch-keyboard-autoinvoke",
            Label = "Disable Touch Keyboard Auto-Invoke in Ink",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the touch keyboard from automatically appearing when a text field is focused in Windows Ink apps. Reduces accidental keyboard pop-ups on pen-only tablet workflows.",
            Tags = ["ink", "touch-keyboard", "tablet", "policy", "windows-ink"],
            RegistryKeys = [Key],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Touch keyboard no longer auto-invokes in ink context; pen workflow uninterrupted.",
            ApplyOps = [RegOp.SetDword(Key, "TouchKeyboardAutoInvokeEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "TouchKeyboardAutoInvokeEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "TouchKeyboardAutoInvokeEnabled", 0)],
        },
        new TweakDef
        {
            Id = "winks-disable-ink-workspace-telemetry",
            Label = "Disable Windows Ink Workspace Telemetry",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks telemetry data collection from Windows Ink Workspace usage patterns. Prevents Microsoft from receiving information about which ink tools are used and how frequently.",
            Tags = ["ink", "telemetry", "privacy", "policy", "windows-ink", "data-collection"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ink workspace usage statistics not reported to Microsoft.",
            ApplyOps = [RegOp.SetDword(Key, "AllowWindowsInkWorkspaceTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowWindowsInkWorkspaceTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "AllowWindowsInkWorkspaceTelemetry", 0)],
        },
    ];
}
