namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class VsCode
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "vscode-disable-vscode-telemetry",
            Label = "Disable VS Code Telemetry",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables VS Code telemetry, crash reporting, and usage data collection via machine-level policy.",
            Tags = ["vscode", "developer", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
        },
        new TweakDef
        {
            Id = "vscode-disable-vscode-update",
            Label = "Disable VS Code Auto-Update",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents VS Code from auto-updating to new versions.",
            Tags = ["vscode", "developer", "update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
        },
        new TweakDef
        {
            Id = "vscode-disable-vscode-ext-update",
            Label = "Disable VS Code Extension Auto-Update",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents VS Code extensions from auto-updating.",
            Tags = ["vscode", "developer", "extensions", "update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
        },
        new TweakDef
        {
            Id = "vscode-disable-vscode-experiments",
            Label = "Disable VS Code A/B Experiments",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables VS Code experiment framework and feature flighting.",
            Tags = ["vscode", "developer", "experiments"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
        },
        new TweakDef
        {
            Id = "vscode-disable-vscode-settings-sync",
            Label = "Disable VS Code Settings Sync",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables VS Code settings sync via machine policy.",
            Tags = ["vscode", "developer", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
        },
        new TweakDef
        {
            Id = "vscode-disable-vscode-startup-editor",
            Label = "Disable VS Code Welcome Tab",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents VS Code from opening the Welcome tab on every launch.",
            Tags = ["vscode", "developer", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
        },
        new TweakDef
        {
            Id = "vscode-disable-vscode-recommendations",
            Label = "Disable VS Code Extension Recommendations",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables extension recommendation popups and banners in VS Code.",
            Tags = ["vscode", "developer", "recommendations"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
        },
        new TweakDef
        {
            Id = "vscode-disable-telemetry-policy",
            Label = "Disable VS Code Telemetry (Policy)",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables VS Code telemetry via the machine-level TelemetryLevel policy value.",
            Tags = ["vscode", "telemetry", "privacy", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
        },
        new TweakDef
        {
            Id = "vscode-disable-experiments",
            Label = "Disable VS Code A/B Experiments",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables VS Code A/B experiments via the machine-level EnableExperiments policy value.",
            Tags = ["vscode", "experiments", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
        },
        new TweakDef
        {
            Id = "vscode-disable-update-notification",
            Label = "Disable VS Code Update Notifications",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables VS Code update notifications via the machine-level UpdateMode policy value.",
            Tags = ["vscode", "update", "notifications", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
        },
        new TweakDef
        {
            Id = "vscode-disable-crash-reporter",
            Label = "Disable VS Code Crash Reporter",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables VS Code crash reporter via the machine-level CrashReporterEnabled policy value.",
            Tags = ["vscode", "crash-reporter", "privacy", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
        },
        new TweakDef
        {
            Id = "vscode-disable-extension-gallery",
            Label = "Restrict VS Code Extension Gallery (Security)",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks access to the VS Code extension marketplace by setting ExtensionGalleryServiceUrl to an empty string via policy.",
            Tags = ["vscode", "extensions", "security", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
        },
        new TweakDef
        {
            Id = "vscode-restrict-workspace-trust",
            Label = "Restrict VS Code Workspace Trust",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables VS Code Workspace Trust prompts via machine policy. All workspaces are treated as trusted. Default: Enabled. Recommended: Disabled for developer machines.",
            Tags = ["vscode", "workspace-trust", "ux", "developer"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
        },
        new TweakDef
        {
            Id = "vscode-disable-gpu-acceleration",
            Label = "Disable VS Code GPU Acceleration",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables GPU/hardware acceleration in VS Code via machine policy. Fixes rendering issues on certain graphics drivers. Default: Enabled. Recommended: Disabled if experiencing display glitches.",
            Tags = ["vscode", "gpu", "performance", "rendering"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
        },
        new TweakDef
        {
            Id = "vscode-disable-telemetry-reporting",
            Label = "Disable VS Code Telemetry (User Policy)",
            Category = "VS Code",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables VS Code telemetry via user-level policy. Default: Enabled. Recommended: Disabled.",
            Tags = ["vscode", "telemetry", "privacy", "user-policy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "EnableTelemetry", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "EnableTelemetry"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "EnableTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-update-check",
            Label = "Disable VS Code Update Check (User Policy)",
            Category = "VS Code",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables VS Code auto-update checking via user-level policy. Default: Enabled. Recommended: Disabled for stable environments.",
            Tags = ["vscode", "update", "auto-update", "user-policy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "UpdateMode", "disabled"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "UpdateMode"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "UpdateMode", "disabled")],
        },
        new TweakDef
        {
            Id = "vscode-vsc-disable-telemetry",
            Label = "Disable VS Code Telemetry (Machine Policy)",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables VS Code telemetry via HKLM machine-level policy. Applies to all users on the machine. Default: Enabled. Recommended: Disabled.",
            Tags = ["vscode", "telemetry", "privacy", "machine-policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "EnableTelemetry", 0),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.telemetryLevel", "off"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "EnableTelemetry"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.telemetryLevel"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "EnableTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "vscode-vsc-disable-update-notif",
            Label = "Disable VS Code Update Notifications (Machine Policy)",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables VS Code update notifications and release notes via machine policy. Default: Enabled. Recommended: Disabled for stable environments.",
            Tags = ["vscode", "update", "notifications", "machine-policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.mode", "none"),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.showReleaseNotes", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.mode"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.showReleaseNotes"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.mode", "none")],
        },
        new TweakDef
        {
            Id = "vscode-vsc-set-gpu-accel",
            Label = "Set VS Code GPU Acceleration (Machine Policy)",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables GPU acceleration for VS Code via machine-level policy. Improves rendering performance. Default: Auto. Recommended: On.",
            Tags = ["vscode", "gpu", "acceleration", "performance", "machine-policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "gpu.acceleration", "on"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "gpu.acceleration"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "gpu.acceleration", "on")],
        },
    ];
}
