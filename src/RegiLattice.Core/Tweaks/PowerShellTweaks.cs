namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Tweaks executed via PowerShell cmdlets (Set-Service, Get-Service, Enable-WindowsOptionalFeature, etc.).
/// These use ApplyAction/RemoveAction/DetectAction delegates via ShellRunner.RunPowerShell.
/// </summary>
internal static class PowerShellTweaks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Service control via PowerShell ───────────────────────────────
        new TweakDef
        {
            Id = "ps-disable-print-spooler",
            Label = "Disable Print Spooler Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.ServiceControl,
            Description = "Stops and disables the Print Spooler service. Closes the PrintNightmare attack vector on machines without printers.",
            Tags = ["powershell", "service", "security", "print"],
            SideEffects = "Printing will be unavailable.",
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name Spooler -Force -ErrorAction SilentlyContinue; Set-Service -Name Spooler -StartupType Disabled"
                ),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name Spooler -StartupType Automatic; Start-Service -Name Spooler"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name Spooler -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-remote-registry",
            Label = "Disable Remote Registry Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.ServiceControl,
            Description = "Stops and disables the Remote Registry service to prevent remote access to the registry.",
            Tags = ["powershell", "service", "security", "remote"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name RemoteRegistry -Force -ErrorAction SilentlyContinue; Set-Service -Name RemoteRegistry -StartupType Disabled"
                ),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name RemoteRegistry -StartupType Manual"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name RemoteRegistry -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-fax-service",
            Label = "Disable Fax Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables the Fax service — unnecessary on most modern systems.",
            Tags = ["powershell", "service", "cleanup"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell("Stop-Service -Name Fax -Force -ErrorAction SilentlyContinue; Set-Service -Name Fax -StartupType Disabled"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name Fax -StartupType Manual"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name Fax -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-xbox-services",
            Label = "Disable Xbox Live Services",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables Xbox Live Auth, Networking, and Game Save services. Safe if not using Xbox features.",
            Tags = ["powershell", "service", "xbox", "gaming", "cleanup"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($svc in @('XblAuthManager','XblGameSave','XboxNetApiSvc','XboxGipSvc')) { Stop-Service -Name $svc -Force -ErrorAction SilentlyContinue; Set-Service -Name $svc -StartupType Disabled -ErrorAction SilentlyContinue }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($svc in @('XblAuthManager','XblGameSave','XboxNetApiSvc','XboxGipSvc')) { Set-Service -Name $svc -StartupType Manual -ErrorAction SilentlyContinue }"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name XblAuthManager -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        // ── System optimisation via PowerShell ──────────────────────────
        new TweakDef
        {
            Id = "ps-clear-temp-files",
            Label = "Clear Temporary Files",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Removes files from %TEMP%, Windows\\Temp, and prefetch folders to free disk space.",
            Tags = ["powershell", "cleanup", "disk", "maintenance"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Remove-Item -Path \"$env:TEMP\\*\" -Recurse -Force -ErrorAction SilentlyContinue; "
                        + "Remove-Item -Path \"$env:WINDIR\\Temp\\*\" -Recurse -Force -ErrorAction SilentlyContinue; "
                        + "Remove-Item -Path \"$env:WINDIR\\Prefetch\\*\" -Recurse -Force -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => { }, // No undo for cleanup
            DetectAction = () =>
            {
                // Check if temp folder has minimal content
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ChildItem -Path $env:TEMP -ErrorAction SilentlyContinue | Measure-Object).Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count < 10;
            },
        },

        new TweakDef
        {
            Id = "ps-enable-dev-mode",
            Label = "Enable Developer Mode",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Enables Windows Developer Mode for sideloading apps and using developer features.",
            Tags = ["powershell", "developer", "sideload"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock",
                    "AllowDevelopmentWithoutDevLicense",
                    1
                ),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock", "AllowAllTrustedApps", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock",
                    "AllowDevelopmentWithoutDevLicense",
                    0
                ),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock", "AllowAllTrustedApps", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock",
                    "AllowDevelopmentWithoutDevLicense",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "ps-flush-dns-cache",
            Label = "Flush DNS Cache",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Clears the local DNS resolver cache. Useful after changing DNS servers or troubleshooting resolution issues.",
            Tags = ["powershell", "dns", "network", "troubleshooting"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Clear-DnsClientCache"),
            RemoveAction = _ => { }, // No undo for cache flush
            DetectAction = () => false, // Always shows as not applied — it's a one-shot action
        },
        new TweakDef
        {
            Id = "ps-disable-diagnostics-hub",
            Label = "Disable Diagnostics Hub Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables the Diagnostics Hub Standard Collector service (DiagTrack helper). Reduces telemetry overhead.",
            Tags = ["powershell", "service", "telemetry", "privacy", "performance"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name diagnosticshub.standardcollector.service -Force -ErrorAction SilentlyContinue; "
                        + "Set-Service -Name diagnosticshub.standardcollector.service -StartupType Disabled -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-Service -Name diagnosticshub.standardcollector.service -StartupType Manual -ErrorAction SilentlyContinue"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-Service -Name diagnosticshub.standardcollector.service -ErrorAction SilentlyContinue).StartType"
                );
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-wmp-network-sharing",
            Label = "Disable WMP Network Sharing Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables Windows Media Player Network Sharing service. Reduces network exposure.",
            Tags = ["powershell", "service", "media", "network", "cleanup"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name WMPNetworkSvc -Force -ErrorAction SilentlyContinue; "
                        + "Set-Service -Name WMPNetworkSvc -StartupType Disabled -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name WMPNetworkSvc -StartupType Manual -ErrorAction SilentlyContinue"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name WMPNetworkSvc -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-geolocation-service",
            Label = "Disable Geolocation Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables the Geolocation service to prevent apps from tracking your physical location.",
            Tags = ["powershell", "service", "privacy", "location"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name lfsvc -Force -ErrorAction SilentlyContinue; Set-Service -Name lfsvc -StartupType Disabled"
                ),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name lfsvc -StartupType Manual"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name lfsvc -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-connected-user-experience",
            Label = "Disable Connected User Experience (DiagTrack)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables the DiagTrack (Connected User Experiences and Telemetry) service. Major Windows telemetry reducer.",
            Tags = ["powershell", "service", "telemetry", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name DiagTrack -Force -ErrorAction SilentlyContinue; Set-Service -Name DiagTrack -StartupType Disabled"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-Service -Name DiagTrack -StartupType Automatic; Start-Service -Name DiagTrack -ErrorAction SilentlyContinue"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name DiagTrack -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-dmwappush-service",
            Label = "Disable Device Management WAP Push Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables the dmwappushservice used for telemetry data collection routing.",
            Tags = ["powershell", "service", "telemetry", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name dmwappushservice -Force -ErrorAction SilentlyContinue; "
                        + "Set-Service -Name dmwappushservice -StartupType Disabled -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name dmwappushservice -StartupType Automatic -ErrorAction SilentlyContinue"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name dmwappushservice -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-optimize-network-adapter",
            Label = "Optimize Network Adapter Power Settings",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Disables power management on all network adapters to prevent them from sleeping and dropping connections.",
            Tags = ["powershell", "network", "power", "performance", "stability"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter -Physical | ForEach-Object { "
                        + "Set-NetAdapterPowerManagement -Name $_.Name -WakeOnMagicPacket Disabled -WakeOnPattern Disabled -ErrorAction SilentlyContinue }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter -Physical | ForEach-Object { "
                        + "Set-NetAdapterPowerManagement -Name $_.Name -WakeOnMagicPacket Enabled -WakeOnPattern Enabled -ErrorAction SilentlyContinue }"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-disable-execution-policy-restriction",
            Label = "Set PowerShell Execution Policy to RemoteSigned",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Sets the machine-scope execution policy to RemoteSigned, allowing local scripts to run without signed status.",
            Tags = ["powershell", "execution-policy", "scripts", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope LocalMachine -Force"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-ExecutionPolicy -ExecutionPolicy Restricted -Scope LocalMachine -Force"),
            DetectAction = () =>
            {
                var (exit, stdout, _) = ShellRunner.Run(
                    "powershell",
                    ["-NoProfile", "-Command", "(Get-ExecutionPolicy -Scope LocalMachine).ToString()"]
                );
                return exit == 0 && stdout.Trim() == "RemoteSigned";
            },
        },
        new TweakDef
        {
            Id = "ps-enable-remoting",
            Label = "Enable PowerShell Remoting",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Enables PowerShell Remoting (WinRM) for remote session management. Required for remote administration.",
            Tags = ["powershell", "remoting", "winrm", "remote"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Enable-PSRemoting -Force -SkipNetworkProfileCheck"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Disable-PSRemoting -Force"),
            DetectAction = () =>
            {
                var (exit, _, _) = ShellRunner.Run("powershell", ["-NoProfile", "-Command", "(Get-Service WinRM).Status"]);
                return exit == 0;
            },
        },
        new TweakDef
        {
            Id = "ps-disable-telemetry",
            Label = "Disable PowerShell Telemetry",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Sets POWERSHELL_TELEMETRY_OPTOUT=1 for the current user to opt out of PowerShell telemetry submission to Microsoft.",
            Tags = ["powershell", "telemetry", "privacy"],
            ApplyAction = _ => ShellRunner.RunPowerShell("[System.Environment]::SetEnvironmentVariable('POWERSHELL_TELEMETRY_OPTOUT','1','User')"),
            RemoveAction = _ => ShellRunner.RunPowerShell("[System.Environment]::SetEnvironmentVariable('POWERSHELL_TELEMETRY_OPTOUT',$null,'User')"),
            DetectAction = () =>
                System.Environment.GetEnvironmentVariable("POWERSHELL_TELEMETRY_OPTOUT", System.EnvironmentVariableTarget.User) == "1",
        },
        new TweakDef
        {
            Id = "ps-enable-constrained-language-mode",
            Label = "Enable PowerShell Constrained Language Mode",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description =
                "Restricts PowerShell to Constrained Language Mode via environment variable, limiting access to arbitrary .NET types. Hardens against living-off-the-land attacks.",
            Tags = ["powershell", "security", "constrained", "hardening"],
            ApplyAction = _ => ShellRunner.RunPowerShell("[System.Environment]::SetEnvironmentVariable('__PSLockdownPolicy','4','Machine')"),
            RemoveAction = _ => ShellRunner.RunPowerShell("[System.Environment]::SetEnvironmentVariable('__PSLockdownPolicy',$null,'Machine')"),
            DetectAction = () => System.Environment.GetEnvironmentVariable("__PSLockdownPolicy", System.EnvironmentVariableTarget.Machine) == "4",
        },
        new TweakDef
        {
            Id = "ps-set-transcript-logging",
            Label = "Disable PowerShell Transcription Logging",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Disables PowerShell transcript logging which records all session input/output to disk. Reduces privacy exposure.",
            Tags = ["powershell", "transcription", "logging", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-ItemProperty -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\PowerShell\\Transcription' -Name 'EnableTranscripting' -Value 0 -Type DWord -Force"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Remove-ItemProperty -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\PowerShell\\Transcription' -Name 'EnableTranscripting' -ErrorAction SilentlyContinue"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-enable-protected-event-logging",
            Label = "Enable Protected Event Logging",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Enables Protected Event Logging (PEL) which encrypts event log content using a certificate. Prevents credential exposure in logs.",
            Tags = ["powershell", "event-log", "security", "encryption"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-ItemProperty -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\EventLog\\ProtectedEventLogging' -Name 'EnableProtectedEventLogging' -Value 1 -Type DWord -Force"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-ItemProperty -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\EventLog\\ProtectedEventLogging' -Name 'EnableProtectedEventLogging' -Value 0 -Type DWord -Force"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-disable-clipboard-history-via-ps",
            Label = "Disable Clipboard History via Policy",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Disables Win+V clipboard history via group policy registry key, preventing clipboard contents from being saved.",
            Tags = ["powershell", "clipboard", "privacy", "policy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "New-Item -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\System' -Force | New-ItemProperty -Name 'AllowClipboardHistory' -Value 0 -PropertyType DWord -Force"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Remove-ItemProperty -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\System' -Name 'AllowClipboardHistory' -ErrorAction SilentlyContinue"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-optimize-page-file",
            Label = "Set Page File to System-Managed",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Configures the page file to be automatically managed by Windows for optimal memory performance.",
            Tags = ["powershell", "pagefile", "memory", "performance"],
            ApplyAction = _ => ShellRunner.RunPowerShell("$cs = Get-WmiObject Win32_ComputerSystem; $cs.AutomaticManagedPagefile = $true; $cs.Put()"),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell("$cs = Get-WmiObject Win32_ComputerSystem; $cs.AutomaticManagedPagefile = $false; $cs.Put()"),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-enable-tls12",
            Label = "Enable TLS 1.2 for .NET Applications",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Configures the .NET Framework 4.x to use TLS 1.2 by default for all outgoing HTTPS connections.",
            Tags = ["powershell", "tls", "security", "network", "dotnet"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "@('HKLM:\\SOFTWARE\\Microsoft\\.NETFramework\\v4.0.30319','HKLM:\\SOFTWARE\\Wow6432Node\\Microsoft\\.NETFramework\\v4.0.30319') | "
                        + "ForEach-Object { New-Item -Path $_ -Force | New-ItemProperty -Name 'SystemDefaultTlsVersions' -Value 1 -PropertyType DWord -Force | Out-Null; "
                        + "New-ItemProperty -Path $_ -Name 'SchUseStrongCrypto' -Value 1 -PropertyType DWord -Force | Out-Null }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "@('HKLM:\\SOFTWARE\\Microsoft\\.NETFramework\\v4.0.30319','HKLM:\\SOFTWARE\\Wow6432Node\\Microsoft\\.NETFramework\\v4.0.30319') | "
                        + "ForEach-Object { Remove-ItemProperty -Path $_ -Name 'SystemDefaultTlsVersions','SchUseStrongCrypto' -ErrorAction SilentlyContinue }"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-disable-powershell-v2-engine",
            Label = "Disable PowerShell v2 Engine (Attack Surface Reduction)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description =
                "Removes the MicrosoftWindowsPowerShellV2Root optional feature. PowerShell v2 lacks logging, constrained language mode, and ScriptBlock logging — keeping it installed exposes a logging bypass attack vector.",
            Tags = ["powershell", "security", "v2", "dism"],
            ApplyAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Disable-Feature", "/FeatureName:MicrosoftWindowsPowerShellV2Root", "/NoRestart"]),
            RemoveAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:MicrosoftWindowsPowerShellV2Root", "/NoRestart"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-FeatureInfo", "/FeatureName:MicrosoftWindowsPowerShellV2Root"]);
                return stdout.Contains("State : Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-enable-windows-sandbox",
            Label = "Enable Windows Sandbox (Disposable Isolated Environment)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            MinBuild = 18305,
            Description =
                "Enables the Containers-DisposableClientVM optional feature. Provides a lightweight, disposable Windows environment for executing untrusted software safely — no separate licence required.",
            Tags = ["powershell", "sandbox", "isolation", "security"],
            SideEffects = "Requires Hyper-V. Requires a reboot.",
            ApplyAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:Containers-DisposableClientVM", "/All", "/NoRestart"]),
            RemoveAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Disable-Feature", "/FeatureName:Containers-DisposableClientVM", "/NoRestart"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-FeatureInfo", "/FeatureName:Containers-DisposableClientVM"]);
                return stdout.Contains("State : Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-enable-controlled-folder-access",
            Label = "Enable Controlled Folder Access (Ransomware Protection)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Enables Windows Defender Controlled Folder Access via Set-MpPreference. Blocks unauthorised apps from writing to protected user folders (Documents, Desktop, Pictures), providing ransomware protection.",
            Tags = ["powershell", "defender", "ransomware", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -EnableControlledFolderAccess Enabled"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -EnableControlledFolderAccess Disabled"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MpPreference).EnableControlledFolderAccess");
                return stdout.Trim() == "1";
            },
        },
        new TweakDef
        {
            Id = "ps-enable-network-protection",
            Label = "Enable Windows Defender Network Protection",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Enables Defender Network Protection via Set-MpPreference. Blocks connections to known malicious IPs, domains, and URLs using the SmartScreen cloud reputation service.",
            Tags = ["powershell", "defender", "network", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -EnableNetworkProtection Enabled"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -EnableNetworkProtection Disabled"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MpPreference).EnableNetworkProtection");
                return stdout.Trim() == "1";
            },
        },
        new TweakDef
        {
            Id = "ps-set-defender-scan-cpu-limit",
            Label = "Limit Defender Scans to 50% CPU Average",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets ScanAvgCPULoadFactor=50 via Set-MpPreference. Caps Windows Defender background scan CPU usage at 50%, reducing performance impact on developer workloads during scheduled scans.",
            Tags = ["powershell", "defender", "cpu", "performance"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -ScanAvgCPULoadFactor 50"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -ScanAvgCPULoadFactor 80"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MpPreference).ScanAvgCPULoadFactor");
                return stdout.Trim() == "50";
            },
        },
        new TweakDef
        {
            Id = "ps-enable-smb-signing-server",
            Label = "Require SMB Signing on This Server (via PowerShell)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets RequireSecuritySignature=$true via Set-SmbServerConfiguration. Mandates cryptographic signing on all SMB server sessions, preventing man-in-the-middle relay attacks.",
            Tags = ["powershell", "smb", "signing", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-SmbServerConfiguration -RequireSecuritySignature $true -Force"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-SmbServerConfiguration -RequireSecuritySignature $false -Force"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-SmbServerConfiguration).RequireSecuritySignature");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-enable-smb-signing-client",
            Label = "Require SMB Signing on This Client (via PowerShell)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets RequireSecuritySignature=$true via Set-SmbClientConfiguration. Enforces signing on all outbound SMB connections from this machine, blocking NTLM relay attacks.",
            Tags = ["powershell", "smb", "signing", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-SmbClientConfiguration -RequireSecuritySignature $true -Force"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-SmbClientConfiguration -RequireSecuritySignature $false -Force"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-SmbClientConfiguration).RequireSecuritySignature");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-smb-guest-fallback",
            Label = "Disable SMB Insecure Guest Logon Fallback",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets EnableInsecureGuestLogons=$false via Set-SmbClientConfiguration. Prevents Windows from falling back to an unauthenticated guest SMB session when credential negotiation fails.",
            Tags = ["powershell", "smb", "guest", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-SmbClientConfiguration -EnableInsecureGuestLogons $false -Force"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-SmbClientConfiguration -EnableInsecureGuestLogons $true -Force"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-SmbClientConfiguration).EnableInsecureGuestLogons");
                return stdout.Trim().Equals("False", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-enable-smb-encryption-server",
            Label = "Enable SMB Encryption on This Server (via PowerShell)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets EncryptData=$true via Set-SmbServerConfiguration. Encrypts all SMB3 data in transit on this server, protecting file shares on untrusted networks.",
            Tags = ["powershell", "smb", "encryption", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-SmbServerConfiguration -EncryptData $true -Force"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-SmbServerConfiguration -EncryptData $false -Force"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-SmbServerConfiguration).EncryptData");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-teredo",
            Label = "Disable Teredo IPv6 Tunnelling",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description =
                "Disables Teredo via netsh. Teredo is an IPv6-over-UDP-IPv4 tunnelling protocol that can be exploited to bypass firewall rules restricting IPv6 traffic.",
            Tags = ["powershell", "ipv6", "teredo", "network", "security"],
            ApplyAction = _ => ShellRunner.Run("netsh", ["interface", "teredo", "set", "state", "disabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh", ["interface", "teredo", "set", "state", "default"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh", ["interface", "teredo", "show", "state"]);
                return stdout.Contains("disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-6to4",
            Label = "Disable 6to4 IPv6 Transition Protocol",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description =
                "Disables the 6to4 transition mechanism via netsh. 6to4 encapsulates IPv6 packets within IPv4 and can create unexpected outbound routing paths when native IPv6 is absent.",
            Tags = ["powershell", "ipv6", "6to4", "network", "security"],
            ApplyAction = _ => ShellRunner.Run("netsh", ["int", "6to4", "set", "state", "disabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh", ["int", "6to4", "set", "state", "default"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh", ["int", "6to4", "show", "state"]);
                return stdout.Contains("disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-isatap",
            Label = "Disable ISATAP IPv6 Transition Interface",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description =
                "Disables the Intra-Site Automatic Tunnel Addressing Protocol (ISATAP) via netsh. ISATAP is an IPv6-in-IPv4 tunnelling mechanism that creates hidden IPv6 connectivity channels.",
            Tags = ["powershell", "ipv6", "isatap", "network", "security"],
            ApplyAction = _ => ShellRunner.Run("netsh", ["interface", "isatap", "set", "state", "disabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh", ["interface", "isatap", "set", "state", "default"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh", ["interface", "isatap", "show", "state"]);
                return stdout.Contains("disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-enable-defender-realtime",
            Label = "Ensure Windows Defender Realtime Protection is On",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets DisableRealtimeMonitoring=$false via Set-MpPreference. Confirms that Defender real-time scanning is active — useful as a remediation step when Group Policy or another tool has disabled it.",
            Tags = ["powershell", "defender", "realtime", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -DisableRealtimeMonitoring $false"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -DisableRealtimeMonitoring $true"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MpPreference).DisableRealtimeMonitoring");
                return stdout.Trim().Equals("False", StringComparison.OrdinalIgnoreCase);
            },
        },
    ];
}

// ── Merged from CommandLineTweaks.cs ──────────────────────────────────────────────────

internal static class CommandLineTweaks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── bcdedit tweaks ──────────────────────────────────────────────
        new TweakDef
        {
            Id = "cmd-disable-hyper-v-hypervisor",
            Label = "Disable Hyper-V Hypervisor (bcdedit)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Uses bcdedit to set hypervisorlaunchtype off. Reduces overhead for non-Hyper-V workloads. Requires reboot.",
            Tags = ["bcdedit", "hypervisor", "performance", "gaming"],
            SideEffects = "Disables Hyper-V, WSL 2, Windows Sandbox, and Credential Guard.",
            ApplyAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "hypervisorlaunchtype", "off"]),
            RemoveAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "hypervisorlaunchtype", "auto"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit.exe", ["/enum", "{current}"]);
                return stdout.Contains("hypervisorlaunchtype    Off", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-boot-log",
            Label = "Enable Boot Log (bcdedit)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables boot logging to %SystemRoot%\\ntbtlog.txt for troubleshooting driver load order.",
            Tags = ["bcdedit", "boot", "diagnostics"],
            ApplyAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "{current}", "bootlog", "yes"]),
            RemoveAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "{current}", "bootlog", "no"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit.exe", ["/enum", "{current}"]);
                return stdout.Contains("bootlog                 Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-increase-tscsyncpolicy",
            Label = "Set TSC Sync Policy to Enhanced (bcdedit)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Sets TSC synchronisation policy to Enhanced for more accurate timers in gaming and real-time workloads.",
            Tags = ["bcdedit", "performance", "gaming", "timer"],
            ApplyAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "tscsyncpolicy", "enhanced"]),
            RemoveAction = _ => ShellRunner.Run("bcdedit.exe", ["/deletevalue", "tscsyncpolicy"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit.exe", ["/enum", "{current}"]);
                return stdout.Contains("tscsyncpolicy           Enhanced", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-disable-dynamic-tick",
            Label = "Disable Dynamic Tick (bcdedit)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables dynamic tick to ensure consistent timer resolution. Beneficial for low-latency audio/gaming.",
            Tags = ["bcdedit", "performance", "gaming", "latency"],
            SideEffects = "May slightly increase power consumption.",
            ApplyAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "disabledynamictick", "yes"]),
            RemoveAction = _ => ShellRunner.Run("bcdedit.exe", ["/deletevalue", "disabledynamictick"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit.exe", ["/enum", "{current}"]);
                return stdout.Contains("disabledynamictick      Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-set-platform-tick-high",
            Label = "Force Platform Clock to High Resolution (bcdedit)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Forces the platform clock to use the highest resolution available. Reduces timer jitter.",
            Tags = ["bcdedit", "performance", "latency", "timer"],
            ApplyAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "useplatformtick", "yes"]),
            RemoveAction = _ => ShellRunner.Run("bcdedit.exe", ["/deletevalue", "useplatformtick"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit.exe", ["/enum", "{current}"]);
                return stdout.Contains("useplatformtick         Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        // ── netsh tweaks ────────────────────────────────────────────────
        new TweakDef
        {
            Id = "cmd-disable-netbios-over-tcpip",
            Label = "Disable NetBIOS over TCP/IP (netsh)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables the NetBIOS name resolution protocol via Windows Firewall inbound rule. Reduces attack surface.",
            Tags = ["netsh", "security", "network"],
            ApplyAction = _ =>
                ShellRunner.Run(
                    "netsh.exe",
                    ["advfirewall", "firewall", "add", "rule", "name=Block NetBIOS", "dir=in", "action=block", "protocol=TCP", "localport=137-139"]
                ),
            RemoveAction = _ => ShellRunner.Run("netsh.exe", ["advfirewall", "firewall", "delete", "rule", "name=Block NetBIOS"]),
            DetectAction = () =>
            {
                var (exit, _, _) = ShellRunner.Run("netsh.exe", ["advfirewall", "firewall", "show", "rule", "name=Block NetBIOS"]);
                return exit == 0;
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-tcp-autotuning",
            Label = "Set TCP Auto-Tuning to Normal (netsh)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Sets TCP receive window auto-tuning level to normal for maximum throughput.",
            Tags = ["netsh", "network", "performance", "tcp"],
            ApplyAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "autotuninglevel=normal"]),
            RemoveAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "autotuninglevel=default"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh.exe", ["int", "tcp", "show", "global"]);
                return stdout.Contains("Receive Window Auto-Tuning Level", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("normal", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-rss",
            Label = "Enable Receive Side Scaling (netsh)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables RSS to distribute network processing across multiple CPU cores.",
            Tags = ["netsh", "network", "performance", "rss"],
            ApplyAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "rss=enabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "rss=disabled"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh.exe", ["int", "tcp", "show", "global"]);
                return stdout.Contains("Receive-Side Scaling State", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-disable-tcp-timestamps",
            Label = "Disable TCP Timestamps (netsh)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables TCP timestamps to reduce packet overhead and prevent OS fingerprinting.",
            Tags = ["netsh", "security", "network", "privacy"],
            ApplyAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "timestamps=disabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "timestamps=enabled"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh.exe", ["int", "tcp", "show", "global"]);
                return stdout.Contains("Timestamps", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-ecn",
            Label = "Enable ECN Capability (netsh)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables Explicit Congestion Notification for better network congestion handling.",
            Tags = ["netsh", "network", "performance"],
            ApplyAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "ecncapability=enabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "ecncapability=disabled"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh.exe", ["int", "tcp", "show", "global"]);
                return stdout.Contains("ECN Capability", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        // ── powercfg tweaks ─────────────────────────────────────────────
        new TweakDef
        {
            Id = "cmd-set-ultimate-perf-plan",
            Label = "Activate Ultimate Performance Power Plan (powercfg)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Unhides and activates the Ultimate Performance power plan for maximum CPU/GPU performance.",
            Tags = ["powercfg", "power", "performance", "gaming"],
            ApplyAction = _ =>
            {
                // Enable the hidden plan, then set it active
                ShellRunner.Run("powercfg.exe", ["/duplicatescheme", "e9a42b02-d5df-448d-aa00-03f14749eb61"]);
                ShellRunner.Run("powercfg.exe", ["/setactive", "e9a42b02-d5df-448d-aa00-03f14749eb61"]);
            },
            RemoveAction = _ =>
            {
                // Switch back to Balanced
                ShellRunner.Run("powercfg.exe", ["/setactive", "381b4222-f694-41f0-9685-ff5bb260df2e"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("powercfg.exe", ["/getactivescheme"]);
                return stdout.Contains("e9a42b02-d5df-448d-aa00-03f14749eb61", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-disable-usb-selective-suspend",
            Label = "Disable USB Selective Suspend (powercfg)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables USB selective suspend to prevent USB devices from disconnecting during idle.",
            Tags = ["powercfg", "usb", "power", "stability"],
            ApplyAction = _ =>
            {
                ShellRunner.Run(
                    "powercfg.exe",
                    ["/setacvalueindex", "SCHEME_CURRENT", "2a737441-1930-4402-8d77-b2bebba308a3", "48e6b7a6-50f5-4782-a5d4-53bb8f07e226", "0"]
                );
                ShellRunner.Run(
                    "powercfg.exe",
                    ["/setdcvalueindex", "SCHEME_CURRENT", "2a737441-1930-4402-8d77-b2bebba308a3", "48e6b7a6-50f5-4782-a5d4-53bb8f07e226", "0"]
                );
                ShellRunner.Run("powercfg.exe", ["/setactive", "SCHEME_CURRENT"]);
            },
            RemoveAction = _ =>
            {
                ShellRunner.Run(
                    "powercfg.exe",
                    ["/setacvalueindex", "SCHEME_CURRENT", "2a737441-1930-4402-8d77-b2bebba308a3", "48e6b7a6-50f5-4782-a5d4-53bb8f07e226", "1"]
                );
                ShellRunner.Run(
                    "powercfg.exe",
                    ["/setdcvalueindex", "SCHEME_CURRENT", "2a737441-1930-4402-8d77-b2bebba308a3", "48e6b7a6-50f5-4782-a5d4-53bb8f07e226", "1"]
                );
                ShellRunner.Run("powercfg.exe", ["/setactive", "SCHEME_CURRENT"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(powercfg /query SCHEME_CURRENT 2a737441-1930-4402-8d77-b2bebba308a3 48e6b7a6-50f5-4782-a5d4-53bb8f07e226) -match '0x00000000'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        // ── DISM tweaks ─────────────────────────────────────────────────
        new TweakDef
        {
            Id = "cmd-disable-ie-feature",
            Label = "Disable Internet Explorer (DISM)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables the Internet Explorer optional feature via DISM. Reduces attack surface.",
            Tags = ["dism", "security", "ie", "legacy"],
            ApplyAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Disable-Feature", "/FeatureName:Internet-Explorer-Optional-amd64", "/NoRestart"]),
            RemoveAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:Internet-Explorer-Optional-amd64", "/NoRestart"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-FeatureInfo", "/FeatureName:Internet-Explorer-Optional-amd64"]);
                return stdout.Contains("State : Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-sandbox",
            Label = "Enable Windows Sandbox (DISM)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables the Windows Sandbox feature for isolated testing environments. Requires Hyper-V support.",
            Tags = ["dism", "security", "sandbox", "virtualization"],
            SideEffects = "Requires reboot after enabling.",
            ApplyAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:Containers-DisposableClientVM", "/All", "/NoRestart"]),
            RemoveAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Disable-Feature", "/FeatureName:Containers-DisposableClientVM", "/NoRestart"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-FeatureInfo", "/FeatureName:Containers-DisposableClientVM"]);
                return stdout.Contains("State : Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-net35",
            Label = "Enable .NET Framework 3.5",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables the .NET Framework 3.5 feature (includes .NET 2.0 and 3.0) for legacy application support.",
            Tags = ["dism", "dotnet", "framework", "legacy"],
            SideEffects = "Downloads components from Windows Update if not cached.",
            ApplyAction = _ => ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:NetFx3", "/All", "/NoRestart"]),
            RemoveAction = _ => ShellRunner.Run("dism.exe", ["/Online", "/Disable-Feature", "/FeatureName:NetFx3", "/NoRestart"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-FeatureInfo", "/FeatureName:NetFx3"]);
                return stdout.Contains("State : Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-disable-ipv6-tunnel-adapters",
            Label = "Disable IPv6 Tunnel Adapters (6to4, ISATAP, Teredo)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables IPv6 transition technologies (6to4, ISATAP, Teredo) to reduce attack surface.",
            Tags = ["netsh", "ipv6", "security", "network"],
            ApplyAction = _ =>
            {
                ShellRunner.Run("netsh.exe", ["interface", "6to4", "set", "state", "disabled"]);
                ShellRunner.Run("netsh.exe", ["interface", "isatap", "set", "state", "disabled"]);
                ShellRunner.Run("netsh.exe", ["interface", "teredo", "set", "state", "disabled"]);
            },
            RemoveAction = _ =>
            {
                ShellRunner.Run("netsh.exe", ["interface", "6to4", "set", "state", "default"]);
                ShellRunner.Run("netsh.exe", ["interface", "isatap", "set", "state", "default"]);
                ShellRunner.Run("netsh.exe", ["interface", "teredo", "set", "state", "default"]);
            },
            DetectAction = () => false,
        },

        new TweakDef
        {
            Id = "cmd-enable-ntp-high-freq",
            Label = "Set NTP Polling to High Frequency",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Configures the Windows Time service to poll NTP servers more frequently (every 256s instead of 3600s).",
            Tags = ["time", "ntp", "synchronisation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config", "MinPollInterval", 6),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config", "MaxPollInterval", 8),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config", "MinPollInterval", 10),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config", "MaxPollInterval", 15),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config", "MinPollInterval", 6)],
        },
        new TweakDef
        {
            Id = "cmd-set-multi-plane-overlay",
            Label = "Enable Multi-Plane Overlay (MPO)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Ensures Multi-Plane Overlay is enabled for GPU composition offloading, reducing CPU usage.",
            Tags = ["gpu", "display", "performance", "mpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode", 5)],
        },
        new TweakDef
        {
            Id = "cmd-disable-game-dvr-background",
            Label = "Disable Background Game Recording (Game DVR)",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Xbox Game DVR background recording to free up GPU and disk resources.",
            Tags = ["gaming", "game-dvr", "xbox", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 1),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0)],
        },
    ];
}
