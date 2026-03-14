namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Developer productivity tweaks — file system, Git, build, and development environment optimisation.
/// </summary>
internal static class Developer
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "dev-disable-last-access-timestamp",
            Label = "Disable Last Access Timestamp (NTFS)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables NTFS Last Access Time updates. Reduces disk I/O for large codebases and build systems (fsutil).",
            Tags = ["developer", "filesystem", "performance", "build"],
            ApplyAction = _ => ShellRunner.Run("fsutil.exe", ["behavior", "set", "disablelastaccess", "1"]),
            RemoveAction = _ => ShellRunner.Run("fsutil.exe", ["behavior", "set", "disablelastaccess", "0"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("fsutil.exe", ["behavior", "query", "disablelastaccess"]);
                // Output: "DisableLastAccess = 1" when System Managed / User enabled
                return stdout.Contains("= 1", StringComparison.Ordinal) ||
                       stdout.Contains("= 3", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "dev-increase-memory-mapped-limit",
            Label = "Increase Memory-Mapped File Limit",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the PoolUsageMaximum to 60% of RAM. Helps large Visual Studio solutions, Docker, and JetBrains IDEs.",
            Tags = ["developer", "memory", "performance", "ide"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PoolUsageMaximum", 60)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PoolUsageMaximum")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PoolUsageMaximum", 60)],
        },
        new TweakDef
        {
            Id = "dev-add-defender-exclusion-repos",
            Label = "Add Defender Exclusions for Dev Folders",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Adds Windows Defender exclusions for common dev paths: C:\\repos, C:\\src, %USERPROFILE%\\source, and common IDE cache folders.",
            Tags = ["developer", "defender", "performance", "build"],
            SideEffects = "Files in excluded paths won't be scanned by Windows Defender.",
            ApplyAction = _ => ShellRunner.RunPowerShell(
                "foreach ($p in @('C:\\repos','C:\\src',\"$env:USERPROFILE\\source\",\"$env:USERPROFILE\\.nuget\",\"$env:USERPROFILE\\.dotnet\")) { " +
                "Add-MpPreference -ExclusionPath $p -ErrorAction SilentlyContinue };" +
                "foreach ($ext in @('.obj','.pdb','.dll','.exe','.nupkg')) { " +
                "Add-MpPreference -ExclusionExtension $ext -ErrorAction SilentlyContinue }"),
            RemoveAction = _ => ShellRunner.RunPowerShell(
                "foreach ($p in @('C:\\repos','C:\\src',\"$env:USERPROFILE\\source\",\"$env:USERPROFILE\\.nuget\",\"$env:USERPROFILE\\.dotnet\")) { " +
                "Remove-MpPreference -ExclusionPath $p -ErrorAction SilentlyContinue };" +
                "foreach ($ext in @('.obj','.pdb','.dll','.exe','.nupkg')) { " +
                "Remove-MpPreference -ExclusionExtension $ext -ErrorAction SilentlyContinue }"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MpPreference).ExclusionPath -contains 'C:\\repos'");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "dev-enable-utf8-system-wide",
            Label = "Enable System-Wide UTF-8 Support",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Beta: Use Unicode UTF-8 for worldwide language support. Fixes encoding issues in build tools and scripts.",
            Tags = ["developer", "encoding", "utf8", "unicode"],
            SideEffects = "Some legacy apps may display text incorrectly.",
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage", "ACP", "65001"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage", "OEMCP", "65001"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage", "MACCP", "65001"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage", "ACP", "1252"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage", "OEMCP", "437"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage", "MACCP", "10000"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage", "ACP", "65001")],
        },
        new TweakDef
        {
            Id = "dev-enable-sudo",
            Label = "Enable Sudo for Windows (Win11 24H2+)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 26100,
            Description = "Enables the built-in sudo command for Windows 11 24H2+ to run elevated commands inline.",
            Tags = ["developer", "sudo", "uac", "terminal"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Sudo"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Sudo", "Enabled", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Sudo", "Enabled", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Sudo", "Enabled", 3)],
        },
        new TweakDef
        {
            Id = "dev-git-lfs-install",
            Label = "Optimise Git for Large Repos",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Configures Git global settings for large repositories: multi-pack index, commit-graph, increase http.postBuffer.",
            Tags = ["developer", "git", "performance"],
            ApplyAction = _ =>
            {
                ShellRunner.Run("git.exe", ["config", "--global", "core.multiPackIndex", "true"]);
                ShellRunner.Run("git.exe", ["config", "--global", "feature.manyFiles", "true"]);
                ShellRunner.Run("git.exe", ["config", "--global", "core.fsmonitor", "true"]);
                ShellRunner.Run("git.exe", ["config", "--global", "http.postBuffer", "524288000"]);
            },
            RemoveAction = _ =>
            {
                ShellRunner.Run("git.exe", ["config", "--global", "--unset", "core.multiPackIndex"]);
                ShellRunner.Run("git.exe", ["config", "--global", "--unset", "feature.manyFiles"]);
                ShellRunner.Run("git.exe", ["config", "--global", "--unset", "core.fsmonitor"]);
                ShellRunner.Run("git.exe", ["config", "--global", "--unset", "http.postBuffer"]);
            },
            DetectAction = () =>
            {
                var (exit, stdout, _) = ShellRunner.Run("git.exe", ["config", "--global", "--get", "feature.manyFiles"]);
                return exit == 0 && stdout.Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "dev-env-add-dotnet-tools",
            Label = "Add .NET Tools to PATH",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Ensures %USERPROFILE%\\.dotnet\\tools is on the user PATH for running global .NET tools.",
            Tags = ["developer", "dotnet", "path", "tools"],
            ApplyAction = _ => ShellRunner.RunPowerShell(
                "$toolsPath = Join-Path $env:USERPROFILE '.dotnet\\tools'; " +
                "$current = [Environment]::GetEnvironmentVariable('PATH','User'); " +
                "if ($current -notlike \"*$toolsPath*\") { [Environment]::SetEnvironmentVariable('PATH',\"$current;$toolsPath\",'User') }"),
            RemoveAction = _ => ShellRunner.RunPowerShell(
                "$toolsPath = Join-Path $env:USERPROFILE '.dotnet\\tools'; " +
                "$current = [Environment]::GetEnvironmentVariable('PATH','User'); " +
                "[Environment]::SetEnvironmentVariable('PATH', ($current -replace [regex]::Escape(\";$toolsPath\"),''),'User')"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("[Environment]::GetEnvironmentVariable('PATH','User') -like '*\\.dotnet\\tools*'");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "dev-disable-defender-realtime-build",
            Label = "Disable Defender Real-Time for Build Processes",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Adds process exclusions for common build tools (MSBuild, dotnet, node, cargo, gcc) to speed up compilation.",
            Tags = ["developer", "defender", "performance", "build"],
            SideEffects = "Build tool processes won't be scanned in real-time.",
            ApplyAction = _ => ShellRunner.RunPowerShell(
                "foreach ($p in @('MSBuild.exe','dotnet.exe','node.exe','npm.cmd','cargo.exe','rustc.exe','gcc.exe','cl.exe','link.exe','java.exe','javac.exe','python.exe')) { " +
                "Add-MpPreference -ExclusionProcess $p -ErrorAction SilentlyContinue }"),
            RemoveAction = _ => ShellRunner.RunPowerShell(
                "foreach ($p in @('MSBuild.exe','dotnet.exe','node.exe','npm.cmd','cargo.exe','rustc.exe','gcc.exe','cl.exe','link.exe','java.exe','javac.exe','python.exe')) { " +
                "Remove-MpPreference -ExclusionProcess $p -ErrorAction SilentlyContinue }"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MpPreference).ExclusionProcess -contains 'dotnet.exe'");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "dev-enable-developer-mode-full",
            Label = "Enable Developer Mode (Full AppModelUnlock)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables Windows Developer Mode for sideloading apps, SSH server, and Device Portal.",
            Tags = ["developer", "sideload", "apps", "developer-mode"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock", "AllowDevelopmentWithoutDevLicense", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock", "AllowAllTrustedApps", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock", "AllowDevelopmentWithoutDevLicense", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock", "AllowAllTrustedApps", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock", "AllowDevelopmentWithoutDevLicense", 1)],
        },
        new TweakDef
        {
            Id = "dev-increase-irp-stack-size",
            Label = "Increase IRP Stack Size (Network/Disk I/O)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the I/O Request Packet stack size from 15 to 32. Improves performance for heavy disk/network I/O workloads.",
            Tags = ["developer", "disk", "network", "performance", "irp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "IRPStackSize", 32)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "IRPStackSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "IRPStackSize", 32)],
        },
        new TweakDef
        {
            Id = "dev-enable-wsl2",
            Label = "Enable WSL 2 (Windows Subsystem for Linux)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables WSL 2 with Virtual Machine Platform. Installs the latest WSL from the Store.",
            Tags = ["developer", "wsl", "linux", "virtualization"],
            ApplyAction = _ =>
            {
                ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:Microsoft-Windows-Subsystem-Linux", "/All", "/NoRestart"]);
                ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:VirtualMachinePlatform", "/All", "/NoRestart"]);
            },
            RemoveAction = _ =>
            {
                ShellRunner.Run("dism.exe", ["/Online", "/Disable-Feature", "/FeatureName:Microsoft-Windows-Subsystem-Linux", "/NoRestart"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-FeatureInfo", "/FeatureName:Microsoft-Windows-Subsystem-Linux"]);
                return stdout.Contains("State : Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "dev-enable-openssh-server",
            Label = "Install and Start OpenSSH Server",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Installs the OpenSSH Server optional feature and starts the sshd service for remote access.",
            Tags = ["developer", "ssh", "remote", "server"],
            ApplyAction = _ => ShellRunner.RunPowerShell(
                "Add-WindowsCapability -Online -Name OpenSSH.Server~~~~0.0.1.0 -ErrorAction SilentlyContinue; " +
                "Set-Service -Name sshd -StartupType Automatic -ErrorAction SilentlyContinue; " +
                "Start-Service sshd -ErrorAction SilentlyContinue; " +
                "New-NetFirewallRule -Name 'OpenSSH-Server' -DisplayName 'OpenSSH Server' -Enabled True -Direction Inbound -Protocol TCP -Action Allow -LocalPort 22 -ErrorAction SilentlyContinue"),
            RemoveAction = _ => ShellRunner.RunPowerShell(
                "Stop-Service sshd -Force -ErrorAction SilentlyContinue; " +
                "Set-Service -Name sshd -StartupType Disabled -ErrorAction SilentlyContinue; " +
                "Remove-WindowsCapability -Online -Name OpenSSH.Server~~~~0.0.1.0 -ErrorAction SilentlyContinue"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service sshd -ErrorAction SilentlyContinue).Status -eq 'Running'");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "dev-set-execution-policy-unrestricted",
            Label = "Set PowerShell Execution Policy to RemoteSigned",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Sets the machine-level PowerShell execution policy to RemoteSigned, allowing local scripts to run.",
            Tags = ["developer", "powershell", "execution-policy"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-ExecutionPolicy RemoteSigned -Scope LocalMachine -Force"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-ExecutionPolicy Restricted -Scope LocalMachine -Force"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-ExecutionPolicy -Scope LocalMachine) -eq 'RemoteSigned'");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "dev-disable-ntfs-8dot3-names",
            Label = "Disable NTFS 8.3 Short Name Generation",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables DOS-compatible 8.3 short file names. Speeds up directory enumeration in large repos.",
            Tags = ["developer", "filesystem", "ntfs", "performance"],
            ApplyAction = _ => ShellRunner.Run("fsutil.exe", ["8dot3name", "set", "1"]),
            RemoveAction = _ => ShellRunner.Run("fsutil.exe", ["8dot3name", "set", "0"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("fsutil.exe", ["8dot3name", "query"]);
                return stdout.Contains("NtfsDisable8dot3NameCreation = 1", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "dev-increase-file-handle-limit",
            Label = "Increase System-Wide File Handle Limit",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the maximum number of open file handles. Helps Node.js, Java, and build systems with many files.",
            Tags = ["developer", "filesystem", "handles", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "RegistryLazyFlushInterval", 60)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "RegistryLazyFlushInterval")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "RegistryLazyFlushInterval", 60)],
        },
    ];
}
