namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class VsCode
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
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
        new TweakDef
        {
            Id = "vscode-disable-telemetry",
            Label = "Disable VS Code Telemetry via Policy",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables VS Code telemetry collection via machine-wide policy. Default: enabled.",
            Tags = ["vscode", "telemetry", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.telemetryLevel", "off")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.telemetryLevel")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.telemetryLevel", "off")],
        },
        new TweakDef
        {
            Id = "vscode-disable-natural-language-search",
            Label = "Disable VS Code Natural Language Search",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the natural language search feature in VS Code settings (prevents Bing queries). Default: enabled.",
            Tags = ["vscode", "search", "bing", "natural-language"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.settings.enableNaturalLanguageSearch", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.settings.enableNaturalLanguageSearch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.settings.enableNaturalLanguageSearch", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-extension-recommendations",
            Label = "Disable VS Code Extension Recommendations",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables extension recommendations in VS Code via machine policy. Default: enabled.",
            Tags = ["vscode", "extensions", "recommendations", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.showRecommendationsOnlyOnDemand", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.showRecommendationsOnlyOnDemand")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.showRecommendationsOnlyOnDemand", 1)],
        },
        new TweakDef
        {
            Id = "vscode-disable-crash-reporter",
            Label = "Disable VS Code Crash Reporter",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables VS Code crash reporter via machine policy. Stops sending crash dumps to Microsoft. Default: enabled.",
            Tags = ["vscode", "crash", "reporter", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "enable-crash-reporter", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "enable-crash-reporter")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "enable-crash-reporter", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-experiments",
            Label = "Disable VS Code Experiments",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables A/B experiments in VS Code that can change features. Default: enabled.",
            Tags = ["vscode", "experiments", "ab-testing", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.enableExperiments", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.enableExperiments")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.enableExperiments", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-remote-telemetry",
            Label = "Disable VS Code Remote Extension Telemetry",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables telemetry for VS Code Remote extensions (SSH, WSL, Dev Containers). Default: enabled.",
            Tags = ["vscode", "remote", "telemetry", "ssh", "wsl"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "remote.telemetryLevel", "off")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "remote.telemetryLevel")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "remote.telemetryLevel", "off")],
        },
        new TweakDef
        {
            Id = "vscode-disable-edit-sessions",
            Label = "Disable VS Code Edit Sessions Cloud",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables VS Code Edit Sessions that sync uncommitted changes to the cloud. Keeps changes local. Default: enabled.",
            Tags = ["vscode", "edit-sessions", "cloud", "sync", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.editSessions.autoResume", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.editSessions.autoResume")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.editSessions.autoResume", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-online-services",
            Label = "Disable VS Code Online Services",
            Category = "VS Code",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables VS Code online service features including settings sync, marketplace, etc. Default: enabled.",
            Tags = ["vscode", "online", "services", "offline", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.autoCheckUpdates", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.autoCheckUpdates")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.autoCheckUpdates", 0)],
        },
    ];
}
