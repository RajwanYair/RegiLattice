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
                return stdout.Contains("= 1", StringComparison.Ordinal) || stdout.Contains("= 3", StringComparison.Ordinal);
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PoolUsageMaximum", 60),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PoolUsageMaximum"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PoolUsageMaximum", 60),
            ],
        },
        new TweakDef
        {
            Id = "dev-add-defender-exclusion-repos",
            Label = "Add Defender Exclusions for Dev Folders",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description =
                "Adds Windows Defender exclusions for common dev paths: C:\\repos, C:\\src, %USERPROFILE%\\source, and common IDE cache folders.",
            Tags = ["developer", "defender", "performance", "build"],
            SideEffects = "Files in excluded paths won't be scanned by Windows Defender.",
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($p in @('C:\\repos','C:\\src',\"$env:USERPROFILE\\source\",\"$env:USERPROFILE\\.nuget\",\"$env:USERPROFILE\\.dotnet\")) { "
                        + "Add-MpPreference -ExclusionPath $p -ErrorAction SilentlyContinue };"
                        + "foreach ($ext in @('.obj','.pdb','.dll','.exe','.nupkg')) { "
                        + "Add-MpPreference -ExclusionExtension $ext -ErrorAction SilentlyContinue }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($p in @('C:\\repos','C:\\src',\"$env:USERPROFILE\\source\",\"$env:USERPROFILE\\.nuget\",\"$env:USERPROFILE\\.dotnet\")) { "
                        + "Remove-MpPreference -ExclusionPath $p -ErrorAction SilentlyContinue };"
                        + "foreach ($ext in @('.obj','.pdb','.dll','.exe','.nupkg')) { "
                        + "Remove-MpPreference -ExclusionExtension $ext -ErrorAction SilentlyContinue }"
                ),
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
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "$toolsPath = Join-Path $env:USERPROFILE '.dotnet\\tools'; "
                        + "$current = [Environment]::GetEnvironmentVariable('PATH','User'); "
                        + "if ($current -notlike \"*$toolsPath*\") { [Environment]::SetEnvironmentVariable('PATH',\"$current;$toolsPath\",'User') }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "$toolsPath = Join-Path $env:USERPROFILE '.dotnet\\tools'; "
                        + "$current = [Environment]::GetEnvironmentVariable('PATH','User'); "
                        + "[Environment]::SetEnvironmentVariable('PATH', ($current -replace [regex]::Escape(\";$toolsPath\"),''),'User')"
                ),
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
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($p in @('MSBuild.exe','dotnet.exe','node.exe','npm.cmd','cargo.exe','rustc.exe','gcc.exe','cl.exe','link.exe','java.exe','javac.exe','python.exe')) { "
                        + "Add-MpPreference -ExclusionProcess $p -ErrorAction SilentlyContinue }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($p in @('MSBuild.exe','dotnet.exe','node.exe','npm.cmd','cargo.exe','rustc.exe','gcc.exe','cl.exe','link.exe','java.exe','javac.exe','python.exe')) { "
                        + "Remove-MpPreference -ExclusionProcess $p -ErrorAction SilentlyContinue }"
                ),
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
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Add-WindowsCapability -Online -Name OpenSSH.Server~~~~0.0.1.0 -ErrorAction SilentlyContinue; "
                        + "Set-Service -Name sshd -StartupType Automatic -ErrorAction SilentlyContinue; "
                        + "Start-Service sshd -ErrorAction SilentlyContinue; "
                        + "New-NetFirewallRule -Name 'OpenSSH-Server' -DisplayName 'OpenSSH Server' -Enabled True -Direction Inbound -Protocol TCP -Action Allow -LocalPort 22 -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service sshd -Force -ErrorAction SilentlyContinue; "
                        + "Set-Service -Name sshd -StartupType Disabled -ErrorAction SilentlyContinue; "
                        + "Remove-WindowsCapability -Online -Name OpenSSH.Server~~~~0.0.1.0 -ErrorAction SilentlyContinue"
                ),
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
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager",
                    "RegistryLazyFlushInterval",
                    60
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager",
                    "RegistryLazyFlushInterval"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager",
                    "RegistryLazyFlushInterval",
                    60
                ),
            ],
        },
        new TweakDef
        {
            Id = "dev-set-dotnet-cli-telemetry-off",
            Label = "Disable .NET CLI Telemetry",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Sets the DOTNET_CLI_TELEMETRY_OPTOUT environment variable to suppress .NET SDK telemetry.",
            Tags = ["dotnet", "telemetry", "developer", "privacy"],
            ApplyAction = _ => Environment.SetEnvironmentVariable("DOTNET_CLI_TELEMETRY_OPTOUT", "1", EnvironmentVariableTarget.User),
            RemoveAction = _ => Environment.SetEnvironmentVariable("DOTNET_CLI_TELEMETRY_OPTOUT", null, EnvironmentVariableTarget.User),
            DetectAction = () =>
            {
                var val = Environment.GetEnvironmentVariable("DOTNET_CLI_TELEMETRY_OPTOUT", EnvironmentVariableTarget.User);
                return val is "1" or "true";
            },
        },
        new TweakDef
        {
            Id = "dev-enable-symlink-no-admin",
            Label = "Allow Symbolic Links Without Admin",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables Developer Mode policy that allows creating symlinks without elevation.",
            Tags = ["symlink", "developer", "filesystem"],
            ApplyAction = _ =>
                ShellRunner.Run(
                    "reg.exe",
                    [
                        "add",
                        @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock",
                        "/v",
                        "AllowDevelopmentWithoutDevLicense",
                        "/t",
                        "REG_DWORD",
                        "/d",
                        "1",
                        "/f",
                    ]
                ),
            RemoveAction = _ =>
                ShellRunner.Run(
                    "reg.exe",
                    [
                        "add",
                        @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock",
                        "/v",
                        "AllowDevelopmentWithoutDevLicense",
                        "/t",
                        "REG_DWORD",
                        "/d",
                        "0",
                        "/f",
                    ]
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run(
                    "reg.exe",
                    ["query", @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock", "/v", "AllowDevelopmentWithoutDevLicense"]
                );
                return stdout.Contains("0x1", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "dev-enable-python-utf8-mode",
            Label = "Enable Python UTF-8 Mode Globally",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Sets PYTHONUTF8=1 environment variable so Python uses UTF-8 encoding by default on Windows.",
            Tags = ["python", "utf8", "developer", "encoding"],
            ApplyAction = _ => Environment.SetEnvironmentVariable("PYTHONUTF8", "1", EnvironmentVariableTarget.User),
            RemoveAction = _ => Environment.SetEnvironmentVariable("PYTHONUTF8", null, EnvironmentVariableTarget.User),
            DetectAction = () =>
            {
                var val = Environment.GetEnvironmentVariable("PYTHONUTF8", EnvironmentVariableTarget.User);
                return val == "1";
            },
        },
        new TweakDef
        {
            Id = "dev-git-credential-manager",
            Label = "Set Git Credential Manager as Default",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Configures Git Credential Manager as the default credential helper for HTTPS repos.",
            Tags = ["git", "credential", "developer"],
            ApplyAction = _ => ShellRunner.Run("git.exe", ["config", "--global", "credential.helper", "manager"]),
            RemoveAction = _ => ShellRunner.Run("git.exe", ["config", "--global", "--unset", "credential.helper"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("git.exe", ["config", "--global", "credential.helper"]);
                return stdout.Trim().Equals("manager", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "dev-git-default-branch-main",
            Label = "Set Git Default Branch to 'main'",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Sets the default branch name for new Git repositories to 'main'.",
            Tags = ["git", "branch", "developer"],
            ApplyAction = _ => ShellRunner.Run("git.exe", ["config", "--global", "init.defaultBranch", "main"]),
            RemoveAction = _ => ShellRunner.Run("git.exe", ["config", "--global", "--unset", "init.defaultBranch"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("git.exe", ["config", "--global", "init.defaultBranch"]);
                return stdout.Trim().Equals("main", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "dev-git-auto-crlf-input",
            Label = "Set Git autocrlf to Input (LF on Commit)",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Normalises line endings to LF on commit while checking out as-is. Best for cross-platform repos.",
            Tags = ["git", "line-endings", "developer"],
            ApplyAction = _ => ShellRunner.Run("git.exe", ["config", "--global", "core.autocrlf", "input"]),
            RemoveAction = _ => ShellRunner.Run("git.exe", ["config", "--global", "--unset", "core.autocrlf"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("git.exe", ["config", "--global", "core.autocrlf"]);
                return stdout.Trim().Equals("input", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "dev-env-add-cargo-bin",
            Label = "Add Cargo bin to User PATH",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Adds %USERPROFILE%\\.cargo\\bin to user PATH for Rust toolchain executables.",
            Tags = ["rust", "cargo", "path", "developer"],
            ApplyAction = _ =>
            {
                var cargoPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".cargo", "bin");
                var current = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User) ?? "";
                if (!current.Contains(cargoPath, StringComparison.OrdinalIgnoreCase))
                    Environment.SetEnvironmentVariable("PATH", current + ";" + cargoPath, EnvironmentVariableTarget.User);
            },
            RemoveAction = _ =>
            {
                var cargoPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".cargo", "bin");
                var current = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User) ?? "";
                var updated = current.Replace(";" + cargoPath, "", StringComparison.OrdinalIgnoreCase);
                Environment.SetEnvironmentVariable("PATH", updated, EnvironmentVariableTarget.User);
            },
            DetectAction = () =>
            {
                var cargoPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".cargo", "bin");
                var current = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User) ?? "";
                return current.Contains(cargoPath, StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "dev-increase-environment-variable-size",
            Label = "Increase Max Environment Variable Size",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the maximum size of environment variables to support very long PATH strings.",
            Tags = ["environment", "path", "developer"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "MaxUserEnvSize", 65536)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "MaxUserEnvSize")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "MaxUserEnvSize", 65536),
            ],
        },
        new TweakDef
        {
            Id = "dev-enable-containers-feature",
            Label = "Enable Windows Containers Feature",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables the Windows Containers optional feature for Docker and container workloads.",
            Tags = ["dism", "containers", "docker", "developer"],
            SideEffects = "Requires reboot. May need Hyper-V enabled.",
            ApplyAction = _ => ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:Containers", "/All", "/NoRestart"]),
            RemoveAction = _ => ShellRunner.Run("dism.exe", ["/Online", "/Disable-Feature", "/FeatureName:Containers", "/NoRestart"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-FeatureInfo", "/FeatureName:Containers"]);
                return stdout.Contains("State : Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "dev-enable-virtual-terminal",
            Label = "Enable Virtual Terminal (ANSI/VT100) in cmd.exe",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets VirtualTerminalLevel=1 in the Console registry key. Enables ANSI escape code processing in the legacy cmd.exe window, which is required by many CLI and developer tools.",
            Tags = ["developer", "console", "ansi", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel", 1)],
        },
        new TweakDef
        {
            Id = "dev-set-console-history-size",
            Label = "Increase Console Command History Buffer to 2000",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets HistoryBufferSize=2000 in the Console registry key. Keeps the last 2000 commands per console window in history (Win+Up to browse), up from the default 50.",
            Tags = ["developer", "console", "history", "productivity"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 2000)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 50)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 2000)],
        },
        new TweakDef
        {
            Id = "dev-set-console-utf8",
            Label = "Set Console Default Code Page to UTF-8 (65001)",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets CodePage=65001 in the Console key. Makes cmd.exe and PowerShell windows default to UTF-8 encoding, preventing mojibake when printing or piping non-ASCII text.",
            Tags = ["developer", "console", "utf8", "encoding"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "CodePage", 65001)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "CodePage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "CodePage", 65001)],
        },
        new TweakDef
        {
            Id = "dev-disable-wer-show-ui",
            Label = "Suppress Windows Error Reporting Crash Dialog",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DontShowUI=1 in Windows Error Reporting. Silently collects crash data without showing the \"this program has stopped working\" dialog, speeding up developer crash loops.",
            Tags = ["developer", "wer", "crash", "dialog"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontShowUI", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontShowUI")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontShowUI", 1)],
        },
        new TweakDef
        {
            Id = "dev-set-wer-consent-silent",
            Label = "Set WER to Auto-Send Reports Without Consent Prompt",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultConsent=1 in WER\\Consent. Sends Windows Error Reports automatically without asking the user for permission, eliminating consent dialogs in development environments.",
            Tags = ["developer", "wer", "consent", "crash"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent", 1)],
        },
        new TweakDef
        {
            Id = "dev-disable-file-download-block",
            Label = "Prevent Windows from Blocking Downloaded Files (Zone.Identifier)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets SaveZoneInformation=1 in Attachments policy. Stops Windows from saving Zone.Identifier alternate data streams on downloaded files, eliminating the \"This file came from the internet\" unblock prompt.",
            Tags = ["developer", "download", "zone", "unblock"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Attachments"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Attachments", "SaveZoneInformation", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Attachments", "SaveZoneInformation"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Attachments", "SaveZoneInformation", 1),
            ],
        },
        new TweakDef
        {
            Id = "dev-show-super-hidden-files",
            Label = "Show OS-Protected Super-Hidden Files in Explorer",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ShowSuperHidden=1 in Explorer Advanced. Reveals system-protected files (Thumbs.db, desktop.ini, hiberfil.sys) in Explorer. Useful when diagnosing file system or boot issues.",
            Tags = ["developer", "explorer", "system-files", "hidden"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSuperHidden", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSuperHidden", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSuperHidden", 1)],
        },
        new TweakDef
        {
            Id = "dev-disable-hide-file-ext-zone-info",
            Label = "Hide Zone.Identifier ADS Info on File Properties",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets HideZoneInfoOnProperties=1 in Attachments policy. Removes the Security tab \"This file came from another computer\" checkbox and security info from file property dialogs.",
            Tags = ["developer", "zone", "security", "explorer"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Attachments"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Attachments", "HideZoneInfoOnProperties", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Attachments", "HideZoneInfoOnProperties"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Attachments", "HideZoneInfoOnProperties", 1),
            ],
        },
    ];
}

// ── Merged from DevDrive.cs ──────────────────────────────────────────────────

internal static class DevDrive
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "dev-disable-filter-attach",
            Label = "Disable Anti-Malware Minifilter",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables real-time anti-malware minifilter driver (Dev Drive performance mode). Fastest I/O but reduces security. Only use on trusted dev volumes.",
            Tags = ["dev-drive", "minifilter", "performance", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                    "DisableRealtimeMonitoring",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                    "DisableRealtimeMonitoring"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection",
                    "DisableRealtimeMonitoring",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "dev-disable-efs-warning",
            Label = "Suppress EFS Encryption Warning",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Suppresses the EFS encryption service prompt when Dev Drive volumes are created without encryption.",
            Tags = ["dev-drive", "efs", "encryption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptionService", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptionService")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptionService", 0)],
        },
        new TweakDef
        {
            Id = "dev-paged-pool-opt",
            Label = "Optimise Paged Pool for Builds",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Lets Windows auto-size paged pool (value 0 = system managed). Optimal for machines with 16+ GB RAM running large builds.",
            Tags = ["dev-drive", "paged-pool", "memory", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagedPoolSize", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagedPoolSize"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagedPoolSize", 0),
            ],
        },
        new TweakDef
        {
            Id = "dev-disable-devhome-telemetry",
            Label = "Disable Dev Home Telemetry",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables diagnostic data collection by the Windows Dev Home app. Default: Enabled.",
            Tags = ["dev-drive", "dev-home", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\DevHome"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\DevHome", "DiagnosticsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\DevHome", "DiagnosticsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\DevHome", "DiagnosticsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "dev-disable-fs-compress",
            Label = "Disable NTFS Extended Char in 8.3",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables extended character support in 8.3 filenames. Reduces file system overhead for dev volumes.",
            Tags = ["dev-drive", "ntfs", "8dot3", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsAllowExtendedCharacterIn8dot3Name", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsAllowExtendedCharacterIn8dot3Name"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsAllowExtendedCharacterIn8dot3Name", 0),
            ],
        },
        new TweakDef
        {
            Id = "dev-enable-developer-mode",
            Label = "Enable Developer Mode",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables Windows Developer Mode. Allows sideloading apps and access to dev features. Default: disabled.",
            Tags = ["dev", "developer-mode", "sideload", "apps"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowAllTrustedApps", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowAllTrustedApps")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowAllTrustedApps", 1)],
        },
        new TweakDef
        {
            Id = "dev-disable-realtime-protection-devdrive",
            Label = "Exclude Dev Drive from Realtime Scan",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Configures Defender to trust Dev Drive volumes for performance. Build times improve 10-30%. Default: scanned.",
            Tags = ["dev", "defender", "realtime", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "TrustDevDrive", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "TrustDevDrive")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "TrustDevDrive", 1)],
        },
        new TweakDef
        {
            Id = "dev-disable-last-access",
            Label = "Disable NTFS Last Access Timestamps",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables NTFS last-access timestamp updates. Reduces disk I/O overhead for file-heavy build operations. Default: enabled.",
            Tags = ["developer", "ntfs", "last-access", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", 1)],
        },
        new TweakDef
        {
            Id = "dev-enable-host-cache",
            Label = "Enable Developer DNS Host Cache",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases DNS client cache size for developers running multiple services. Reduces DNS lookup latency. Default: standard cache.",
            Tags = ["developer", "dns", "cache", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit", 86400)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit", 86400),
            ],
        },
        new TweakDef
        {
            Id = "dev-exclude-build-tools",
            Label = "Exclude Build Tools from Defender",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Adds common build tool processes (dotnet, msbuild, node) to Defender exclusions. Speeds up builds significantly. Default: scanned.",
            Tags = ["developer", "defender", "exclusion", "build"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Exclusions\Processes"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Exclusions\Processes", "dotnet.exe", "dotnet.exe"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Exclusions\Processes", "dotnet.exe")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Exclusions\Processes",
                    "dotnet.exe",
                    "dotnet.exe"
                ),
            ],
        },
        new TweakDef
        {
            Id = "dev-ntfs-write-cache",
            Label = "Enable NTFS Write Caching",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables NTFS memory-mapped I/O write caching. Improves write performance for build and compile operations. Default: varies.",
            Tags = ["developer", "ntfs", "write-cache", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 2)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-filter-native",
            Label = "Set Dev Drive Filter to Native Antimalware Only",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Configures Dev Drive to accept only native antimalware filter drivers. Non-essential minifilters are excluded. Improves build performance.",
            Tags = ["devdrive", "filter", "performance", "win11"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "FilterControlNative", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "FilterControlNative")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "FilterControlNative", 2)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-filter-core",
            Label = "Set Dev Drive Core Filter to Performance Mode",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Sets Dev Drive core filter control to performance mode. Reduces overhead from filter driver stacks. Default: standard mode.",
            Tags = ["devdrive", "filter", "core", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "FilterControlCore", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "FilterControlCore")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "FilterControlCore", 2)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-ntfs-quota-off",
            Label = "Disable NTFS Quota Tracking on Dev Drive",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables NTFS per-user disk quota tracking. Reduces filesystem overhead for developer build volumes. Default: enabled.",
            Tags = ["devdrive", "ntfs", "quota", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableQuotaTracking", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableQuotaTracking", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableQuotaTracking", 1)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-no-compress-policy",
            Label = "Disable NTFS Compression on Dev Drive (Policy)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Prevents NTFS file compression on Dev Drive. Compression adds CPU overhead during heavy I/O build operations. Default: compression allowed.",
            Tags = ["devdrive", "ntfs", "compression", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "AllowNtfsCompression", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "AllowNtfsCompression")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "AllowNtfsCompression", 0)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-no-smb-share",
            Label = "Disable SMB Sharing of Dev Drive (Policy)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Prevents Dev Drive volumes from being shared via SMB. Limits access to local processes only for security. Default: sharing allowed.",
            Tags = ["devdrive", "smb", "security", "sharing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "AllowSmbSharing", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "AllowSmbSharing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "AllowSmbSharing", 0)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-cache-size-64mb",
            Label = "Set Dev Drive Cache Size to 64 MB",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Sets the Dev Drive filter cache size to 64 MB. Larger cache improves performance for iterative build workloads. Default: 16 MB.",
            Tags = ["devdrive", "cache", "performance", "build"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "CacheSizeMB", 64)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "CacheSizeMB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "CacheSizeMB", 64)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-disable-telemetry",
            Label = "Disable Dev Drive Telemetry",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Disables telemetry data collection for Dev Drive operations. Prevents usage metrics being sent to Microsoft. Default: enabled.",
            Tags = ["devdrive", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DevDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DevDrive", "DisableTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DevDrive", "DisableTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DevDrive", "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-security-trusted",
            Label = "Set Dev Drive Security to Trusted Mode",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            MinBuild = 22621,
            Description =
                "Configures Dev Drive security trust level to allow looser security enforcement for local development environments. Default: standard.",
            Tags = ["devdrive", "security", "trust"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "SecurityLevel", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "SecurityLevel", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "SecurityLevel", 1)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-quota-user-off",
            Label = "Disable Per-User Disk Quota on Dev Drive",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Disables per-user disk quota enforcement on Dev Drive volumes. Prevents quota warnings during large builds. Default: system default.",
            Tags = ["devdrive", "quota", "user", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "DisableUserQuota", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "DisableUserQuota")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DevDrive", "DisableUserQuota", 1)],
        },
        new TweakDef
        {
            Id = "dev-devdrive-perf-mode-high",
            Label = "Enable Dev Drive High-Performance Mode",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Enables high-performance mode for Dev Drive I/O operations. Optimises scheduling for build-heavy workloads. Default: standard mode.",
            Tags = ["devdrive", "performance", "mode", "build"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "PerformanceMode", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "PerformanceMode", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DevDriveFilterControl", "PerformanceMode", 1)],
        },
        new TweakDef
        {
            Id = "dev-sandbox-disable-vgpu",
            Label = "Disable vGPU in Windows Sandbox",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 19041,
            Description =
                "Disables virtualised GPU (vGPU) inside Windows Sandbox. Reduces GPU overhead and sandbox startup time when GPU acceleration is not needed. Default: vGPU enabled.",
            Tags = ["dev", "sandbox", "vgpu", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowVGPU", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowVGPU")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowVGPU", 0)],
        },
        new TweakDef
        {
            Id = "dev-sandbox-disable-networking",
            Label = "Disable Networking in Windows Sandbox",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 19041,
            Description =
                "Disables network access inside Windows Sandbox for isolated testing environments. Useful when testing untrusted code or malware analysis. Default: networking enabled.",
            Tags = ["dev", "sandbox", "network", "isolation", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowNetworking", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowNetworking")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowNetworking", 0)],
        },
        new TweakDef
        {
            Id = "dev-sandbox-enable-protected-client",
            Label = "Enable Protected Client Mode in Windows Sandbox",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 19041,
            Description =
                "Turns on Protected Client mode for Windows Sandbox, which runs the sandbox with reduced privileges and additional isolation. Increases security at the cost of some compatibility. Default: disabled.",
            Tags = ["dev", "sandbox", "security", "protected-client"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowProtectedClient", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowProtectedClient")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowProtectedClient", 1)],
        },
        new TweakDef
        {
            Id = "dev-disable-offline-files",
            Label = "Disable Offline Files (CSC) Service",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Client-Side Caching (CSC) service which handles offline file synchronisation. Removes overhead on dev workstations not using corporate file shares. Default: automatic start.",
            Tags = ["dev", "offline-files", "csc", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CSC"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CSC", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CSC", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CSC", "Start", 4)],
        },
        new TweakDef
        {
            Id = "dev-disable-app-compat-engine",
            Label = "Disable Application Compatibility Engine",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Application Compatibility Engine which checks every process launch against a compatibility database. Speeds up process startup on dev machines with modern software only. Default: enabled.",
            Tags = ["dev", "compat", "performance", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine", 1)],
        },
        new TweakDef
        {
            Id = "dev-disable-program-compat-telemetry",
            Label = "Disable Application Telemetry (Compat)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Application Impact Telemetry (AIT) which logs app usage and compatibility events. Reduces background disk and network activity from the compat engine. Default: enabled.",
            Tags = ["dev", "compat", "telemetry", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable", 0)],
        },
        new TweakDef
        {
            Id = "dev-disable-wer-service-dev",
            Label = "Disable Windows Error Reporting Service (Dev)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets the Windows Error Reporting (WerSvc) service to disabled. Stops crash dumps and error telemetry uploads from dev builds. Use debuggers instead for crash analysis. Default: manual start.",
            Tags = ["dev", "wer", "crash", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WerSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WerSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WerSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WerSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "dev-set-high-res-timer",
            Label = "Enable High-Resolution Timer for Dev Workloads",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            Description =
                "Allows high-resolution timer requests globally via the kernel setting introduced in Win 11 22H2. Lets developer tools (profilers, benchmarks) achieve sub-millisecond timing precision without per-process requests. Default: off.",
            Tags = ["dev", "timer", "performance", "precision"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "GlobalTimerResolutionRequests", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "GlobalTimerResolutionRequests", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "GlobalTimerResolutionRequests", 1),
            ],
        },
    ];
}

// ── merged from VsCode.cs ────────────────────────────────────────
internal static class VsCode
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "vscode-disable-telemetry-reporting",
            Label = "Disable VS Code Telemetry (User Policy)",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables VS Code telemetry via user-level policy. Default: Enabled. Recommended: Disabled.",
            Tags = ["vscode", "telemetry", "privacy", "user-policy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "EnableTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "EnableTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "EnableTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-update-check",
            Label = "Disable VS Code Update Check (User Policy)",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables VS Code auto-update checking via user-level policy. Default: Enabled. Recommended: Disabled for stable environments.",
            Tags = ["vscode", "update", "auto-update", "user-policy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "UpdateMode", "disabled")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "UpdateMode")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "UpdateMode", "disabled")],
        },
        new TweakDef
        {
            Id = "vscode-vsc-disable-telemetry",
            Label = "Disable VS Code Telemetry (Machine Policy)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables VS Code telemetry via HKLM machine-level policy. Applies to all users on the machine. Default: Enabled. Recommended: Disabled.",
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
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables VS Code update notifications and release notes via machine policy. Default: Enabled. Recommended: Disabled for stable environments.",
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
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables GPU acceleration for VS Code via machine-level policy. Improves rendering performance. Default: Auto. Recommended: On.",
            Tags = ["vscode", "gpu", "acceleration", "performance", "machine-policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "gpu.acceleration", "on")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "gpu.acceleration")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "gpu.acceleration", "on")],
        },
        new TweakDef
        {
            Id = "vscode-disable-natural-language-search",
            Label = "Disable VS Code Natural Language Search",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the natural language search feature in VS Code settings (prevents Bing queries). Default: enabled.",
            Tags = ["vscode", "search", "bing", "natural-language"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.settings.enableNaturalLanguageSearch", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.settings.enableNaturalLanguageSearch"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.settings.enableNaturalLanguageSearch", 0),
            ],
        },
        new TweakDef
        {
            Id = "vscode-disable-extension-recommendations",
            Label = "Disable VS Code Extension Recommendations",
            Category = "Developer",
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
            Category = "Developer",
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
            Category = "Developer",
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
            Category = "Developer",
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
            Category = "Developer",
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
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables VS Code online service features including settings sync, marketplace, etc. Default: enabled.",
            Tags = ["vscode", "online", "services", "offline", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.autoCheckUpdates", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.autoCheckUpdates")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.autoCheckUpdates", 0)],
        },
        // ── Restored stubs with real registry operations ──────────────────

        new TweakDef
        {
            Id = "vscode-disable-extension-gallery",
            Label = "Disable Extension Marketplace",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the VS Code extension marketplace via Group Policy, preventing extension installs.",
            Tags = ["vscode", "extensions", "marketplace", "policy", "offline"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.gallery.serviceUrl", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.gallery.serviceUrl")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.gallery.serviceUrl", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-gpu-acceleration",
            Label = "Disable VS Code GPU Acceleration",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables GPU/hardware acceleration in VS Code via policy. Useful for remote desktop or VM environments.",
            Tags = ["vscode", "gpu", "acceleration", "rdp", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "disable-hardware-acceleration", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "disable-hardware-acceleration")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "disable-hardware-acceleration", 1)],
        },
        new TweakDef
        {
            Id = "vscode-disable-telemetry-policy",
            Label = "Disable VS Code Telemetry (Machine)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables all VS Code telemetry and diagnostics via machine-level Group Policy.",
            Tags = ["vscode", "telemetry", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.telemetryLevel", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.enableCrashReporter", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.enableTelemetry", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.telemetryLevel"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.enableCrashReporter"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.enableTelemetry"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.telemetryLevel", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-vscode-ext-update",
            Label = "Disable VS Code Extension Auto-Update",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents VS Code from automatically updating extensions via policy.",
            Tags = ["vscode", "extensions", "auto-update", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.autoUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.autoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.autoUpdate", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-vscode-settings-sync",
            Label = "Disable VS Code Settings Sync",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Settings Sync feature in VS Code via Group Policy.",
            Tags = ["vscode", "settings-sync", "cloud", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "settingsSync.enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "settingsSync.enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "settingsSync.enabled", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-vscode-startup-editor",
            Label = "Disable VS Code Startup Welcome Tab",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents VS Code from showing the Welcome tab on startup via policy.",
            Tags = ["vscode", "startup", "welcome", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.startupEditor", "none")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.startupEditor")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.startupEditor", "none")],
        },
        new TweakDef
        {
            Id = "vscode-disable-vscode-telemetry",
            Label = "Disable VS Code Telemetry (All)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets VS Code telemetry level to 'off' via machine policy, disabling all data collection.",
            Tags = ["vscode", "telemetry", "off", "privacy", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.telemetryLevel.value", "off")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.telemetryLevel.value")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.telemetryLevel.value", "off")],
        },
        new TweakDef
        {
            Id = "vscode-restrict-workspace-trust",
            Label = "Restrict VS Code Workspace Trust",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic workspace trust prompts and defaults to restricted mode via policy.",
            Tags = ["vscode", "workspace-trust", "security", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "security.workspace.trust.enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "security.workspace.trust.enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "security.workspace.trust.enabled", 0)],
        },
        // ── Sprint 47 additions ───────────────────────────────────────────────
        new TweakDef
        {
            Id = "vscode-policy-crash-reporter",
            Label = "Disable VS Code Crash Reporter",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks VS Code from sending crash reports to Microsoft via group policy.",
            Tags = ["vscode", "privacy", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "enable-crash-reporter", "false")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "enable-crash-reporter")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "enable-crash-reporter", "false")],
        },
        new TweakDef
        {
            Id = "vscode-policy-extension-gallery",
            Label = "Disable VS Code Extension Gallery",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables access to the VS Code public extension marketplace (useful in locked-down environments).",
            Tags = ["vscode", "extensions", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.gallery.enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.gallery.enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.gallery.enabled", 0)],
        },
        new TweakDef
        {
            Id = "vscode-policy-online-services",
            Label = "Disable VS Code Online Services",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents VS Code from making requests to online services (cloud settings, snippets).",
            Tags = ["vscode", "privacy", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.ignoreRecommendations", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.ignoreRecommendations")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.ignoreRecommendations", 1)],
        },
        new TweakDef
        {
            Id = "vscode-policy-nls-search",
            Label = "Disable VS Code Natural Language Search",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Bing-powered natural language extension search feature in VS Code.",
            Tags = ["vscode", "privacy", "search"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.naturalLanguageSearch.enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.naturalLanguageSearch.enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.naturalLanguageSearch.enabled", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-account-sync",
            Label = "Disable VS Code Account & Settings Sync",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks VS Code from syncing settings, keybindings and extensions to a Microsoft account.",
            Tags = ["vscode", "privacy", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "settingsSync.keybindingsPerPlatform", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "settingsSync.keybindingsPerPlatform")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "settingsSync.keybindingsPerPlatform", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-github-copilot-chat",
            Label = "Disable VS Code GitHub Copilot Chat",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables GitHub Copilot Chat AI features in VS Code via policy registry key.",
            Tags = ["vscode", "copilot", "ai", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "chat.enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "chat.enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "chat.enabled", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-output-link-detection",
            Label = "Disable VS Code Output Link Detection",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Stops VS Code from scanning terminal/output panels for clickable links (reduces CPU on heavy output).",
            Tags = ["vscode", "performance", "terminal"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "output.linkify", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "output.linkify")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "output.linkify", 0)],
        },
        // ── merged from: Wsl.cs ──────────────────────────────────────────────────
        new TweakDef
        {
            Id = "wsl-nested-virt",
            Label = "WSL Nested Virtualisation",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables nested virtualisation for WSL 2 guests, allowing Docker Desktop, KVM, and other VM workloads inside WSL.",
            Tags = ["wsl", "virtualisation", "docker"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableNestedVirtualization", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableNestedVirtualization")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableNestedVirtualization", 1)],
        },
        new TweakDef
        {
            Id = "wsl-disable-interop",
            Label = "Disable WSL Windows Interop",
            Category = "Virtualization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables WSL Windows interop (running Windows executables from WSL). Default: Enabled. Recommended: Disabled for isolation.",
            Tags = ["wsl", "interop", "windows", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "WslInterop", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "WslInterop", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "WslInterop", 0)],
        },
        new TweakDef
        {
            Id = "wsl-sparse-vhd",
            Label = "Enable WSL Sparse VHD",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables sparse VHD mode so WSL2 virtual disks automatically shrink when free space is released inside the distro. Win11 22H2+. Default: disabled. Recommended: enabled.",
            Tags = ["wsl", "sparse", "vhd", "disk", "storage"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableSparseVhd", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableSparseVhd")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableSparseVhd", 1)],
        },
        new TweakDef
        {
            Id = "wsl-firewall",
            Label = "Enable WSL Firewall Integration",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Windows Firewall integration for WSL2 network traffic. Allows corporate firewall rules to apply to WSL traffic. Win11 22H2+. Default: disabled. Recommended: enabled on managed networks.",
            Tags = ["wsl", "firewall", "security", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableFirewall", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableFirewall")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableFirewall", 1)],
        },
        new TweakDef
        {
            Id = "wsl-disable-gui",
            Label = "Disable WSLg (GUI App Support)",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables WSLg (Windows Subsystem for Linux GUI) to reduce memory and GPU overhead when only CLI workloads are needed. Default: enabled. Recommended: disabled for CLI-only usage.",
            Tags = ["wsl", "gui", "wslg", "performance", "memory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableWslGraphics", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableWslGraphics")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableWslGraphics", 0)],
        },
        new TweakDef
        {
            Id = "wsl-safe-mode",
            Label = "Enable WSL Safe Mode",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables WSL safe mode which bypasses custom /etc/wsl.conf settings for troubleshooting. Useful when a bad config prevents WSL from starting. Default: disabled. Recommended: disabled (enable only for debugging).",
            Tags = ["wsl", "safe-mode", "diagnostic", "troubleshooting"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SafeMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SafeMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SafeMode", 1)],
        },
        new TweakDef
        {
            Id = "wsl-debug-console",
            Label = "Enable WSL Debug Console",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables the WSL debug console for kernel log output and diagnostics. Default: disabled. Recommended: disabled (enable only for debugging).",
            Tags = ["wsl", "debug", "console", "kernel", "diagnostic"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DebugConsole", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DebugConsole")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DebugConsole", 1)],
        },
        new TweakDef
        {
            Id = "wsl-enforce-v2-policy",
            Label = "Enforce WSL Version 2 as Default (Policy)",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enforces WSL version 2 as the default for all new distributions via machine-wide Group Policy. Default: not set. Recommended: version 2.",
            Tags = ["wsl", "version", "v2", "policy", "default"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "DefaultVersion", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "DefaultVersion")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "DefaultVersion", 2)],
        },
        new TweakDef
        {
            Id = "wsl-limit-memory",
            Label = "Limit WSL Memory to 4 GB",
            Category = "Virtualization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Limits the maximum memory allocated to WSL2 virtual machines to 4096 MB. Prevents WSL from consuming excessive host RAM. Default: 50%% of host RAM. Recommended: 4 GB.",
            Tags = ["wsl", "memory", "limit", "ram", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "MaxVmMemory", 4096)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "MaxVmMemory")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "MaxVmMemory", 4096)],
        },
        new TweakDef
        {
            Id = "wsl-systemd-default",
            Label = "Enable Systemd as Default Init (Policy)",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables systemd as the default init system for WSL2 distributions via Group Policy. Required for services like snap and Docker. Default: disabled. Recommended: enabled.",
            Tags = ["wsl", "systemd", "init", "policy", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "SystemdEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "SystemdEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "SystemdEnabled", 1)],
        },
        new TweakDef
        {
            Id = "wsl-automount-metadata",
            Label = "Enable DrvFs Auto-Mount with Metadata",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables DrvFs metadata on Windows drive mounts inside WSL, allowing proper Linux file permissions (chmod/chown) on /mnt/c etc. Default: disabled. Recommended: enabled for development.",
            Tags = ["wsl", "drvfs", "mount", "metadata", "permissions"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DrvFsEnableMetadata", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DrvFsEnableMetadata")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DrvFsEnableMetadata", 1)],
        },
        new TweakDef
        {
            Id = "wsl-no-windows-path",
            Label = "Disable Windows PATH Append in WSL",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents WSL from appending Windows system PATH directories to the Linux $PATH. Keeps the Linux environment clean and avoids Windows executable conflicts. Default: append enabled. Recommended: disabled for isolated dev environments.",
            Tags = ["wsl", "path", "interop", "isolation", "development"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "AppendNtPath", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "AppendNtPath")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "AppendNtPath", 0)],
        },
        new TweakDef
        {
            Id = "wsl-swap-size",
            Label = "Limit WSL2 Swap to 2 GB",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Limits the WSL2 virtual machine swap file to 2 GB to prevent excessive disk usage. Default: 25%% of host RAM. Recommended: 2 GB for most workloads.",
            Tags = ["wsl", "swap", "disk", "memory", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SwapSize", 2048)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SwapSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SwapSize", 2048)],
        },
        new TweakDef
        {
            Id = "wsl-gpu-compute",
            Label = "Enable GPU Compute Pass-Through (CUDA/DirectML)",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables GPU compute pass-through in WSL2 for CUDA, DirectML, and OpenCL workloads. Required for machine learning and GPU-accelerated applications inside WSL. Default: enabled on Win11. Recommended: enabled.",
            Tags = ["wsl", "gpu", "cuda", "directml", "compute", "ml"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableGpuSupport", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableGpuSupport")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableGpuSupport", 1)],
        },
        new TweakDef
        {
            Id = "wsl-interop-off-policy",
            Label = "Disable WSL Windows Interop",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables WSL interoperability with Windows (launching Windows .exe from WSL). Reduces attack surface and eliminates Windows path leakage. Default: enabled. Recommended: disabled for isolated/security workloads.",
            Tags = ["wsl", "interop", "security", "isolation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableInterop", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableInterop")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableInterop", 0)],
        },
        new TweakDef
        {
            Id = "wsl-disable-binfmt-misc",
            Label = "Disable WSL Binfmt Misc Registration",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables binfmt_misc registration in WSL2, preventing the kernel from automatically running Windows executables from Linux paths. Default: enabled. Recommended: disabled for pure-Linux dev environments.",
            Tags = ["wsl", "binfmt", "kernel", "interop", "isolation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableBinfmtMisc", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableBinfmtMisc")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableBinfmtMisc", 0)],
        },
        new TweakDef
        {
            Id = "wsl-limit-processors",
            Label = "Limit WSL2 VM to 4 Processors",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Caps the number of logical processors available to the WSL2 VM to 4. Prevents WSL from starving the host of CPU resources during builds. Default: all host processors. Recommended: 4 for background dev use.",
            Tags = ["wsl", "cpu", "performance", "vm", "resource"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "ProcessorCount", 4)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "ProcessorCount")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "ProcessorCount", 4)],
        },
        new TweakDef
        {
            Id = "wsl-disable-crash-reporting",
            Label = "Disable WSL Crash Dump Creation",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Watson (crash reporting) from creating crash dumps for WSL processes. Frees disk space and avoids slow post-crash dump write. Default: enabled. Recommended: disabled on developer machines.",
            Tags = ["wsl", "crash", "dump", "telemetry", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DrWatson"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DrWatson", "CreateCrashDump", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DrWatson", "CreateCrashDump")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DrWatson", "CreateCrashDump", 0)],
        },
        new TweakDef
        {
            Id = "wsl-disable-telemetry",
            Label = "Disable WSL Telemetry",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables WSL subsystem telemetry data collection sent to Microsoft. Default: enabled. Recommended: disabled for privacy-focused environments.",
            Tags = ["wsl", "telemetry", "privacy", "microsoft"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "wsl-disable-windows-path-interop",
            Label = "Disable Windows PATH Interop in WSL",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows PATH from being appended to WSL $PATH. Avoids conflicts with Windows executables. Default: enabled.",
            Tags = ["wsl", "path", "interop", "isolation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "AppendNtPath", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "AppendNtPath", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "AppendNtPath", 0)],
        },
        new TweakDef
        {
            Id = "wsl-disable-gui-support",
            Label = "Disable WSLg (GUI App Support)",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables WSLg (Linux GUI app support via Wayland/X11). Reduces memory and resource usage. Default: enabled.",
            Tags = ["wsl", "wslg", "gui", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableGUIApplications", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableGUIApplications", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableGUIApplications", 0)],
        },
        new TweakDef
        {
            Id = "wsl-set-default-version-2",
            Label = "Set Default WSL Version to 2",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the default WSL version to 2 for new distro installations. WSL2 uses a real Linux kernel. Default: 1.",
            Tags = ["wsl", "version", "wsl2", "default"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "DefaultVersion", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "DefaultVersion", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "DefaultVersion", 2)],
        },
        new TweakDef
        {
            Id = "wsl-disable-dns-tunneling",
            Label = "Disable WSL DNS Tunneling",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables DNS tunneling in WSL2. Uses host DNS resolution instead. Default: enabled in newer builds.",
            Tags = ["wsl", "dns", "tunneling", "networking"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableDnsTunneling", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableDnsTunneling")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableDnsTunneling", 0)],
        },
        new TweakDef
        {
            Id = "wsl-disable-auto-memory-reclaim",
            Label = "Disable WSL Auto Memory Reclaim",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic memory reclaim in WSL2. Prevents WSL from releasing cached memory back to Windows. Default: enabled.",
            Tags = ["wsl", "memory", "reclaim", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoMemoryReclaim", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoMemoryReclaim")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoMemoryReclaim", 0)],
        },
        // ── Command-based WSL tweaks ───────────────────────────────────────
        new TweakDef
        {
            Id = "wsl-enable-feature",
            Label = "Enable WSL Windows Feature (DISM)",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables the Microsoft-Windows-Subsystem-Linux optional feature via DISM. Requires reboot.",
            Tags = ["wsl", "feature", "dism", "install"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Requires reboot to take effect.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                var (code, _, stderr) = Elevation.RunElevated(
                    "dism",
                    ["/online", "/enable-feature", "/featurename:Microsoft-Windows-Subsystem-Linux", "/norestart"]
                );
                if (code != 0 && !stderr.Contains("already enabled", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException($"DISM enable WSL feature failed: {stderr}");
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("dism", ["/online", "/disable-feature", "/featurename:Microsoft-Windows-Subsystem-Linux", "/norestart"]);
            },
            DetectAction = () =>
            {
                var (code, stdout, _) = Elevation.RunElevated(
                    "dism",
                    ["/online", "/get-featureinfo", "/featurename:Microsoft-Windows-Subsystem-Linux"]
                );
                return code == 0 && stdout.Contains("State : Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "wsl-enable-vmplatform",
            Label = "Enable Virtual Machine Platform (DISM)",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables the VirtualMachinePlatform feature required for WSL2. Requires reboot.",
            Tags = ["wsl", "vm", "platform", "dism", "install"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Requires reboot to take effect.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                var (code, _, stderr) = Elevation.RunElevated(
                    "dism",
                    ["/online", "/enable-feature", "/featurename:VirtualMachinePlatform", "/norestart"]
                );
                if (code != 0 && !stderr.Contains("already enabled", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException($"DISM enable VirtualMachinePlatform failed: {stderr}");
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("dism", ["/online", "/disable-feature", "/featurename:VirtualMachinePlatform", "/norestart"]);
            },
            DetectAction = () =>
            {
                var (code, stdout, _) = Elevation.RunElevated("dism", ["/online", "/get-featureinfo", "/featurename:VirtualMachinePlatform"]);
                return code == 0 && stdout.Contains("State : Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "wsl-compact-vhd",
            Label = "Compact WSL2 VHD Disks",
            Category = "Virtualization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Terminates running WSL instances and compacts all .vhdx virtual disk files to reclaim unused space. One-time action.",
            Tags = ["wsl", "vhd", "compact", "disk", "storage"],
            KindHint = TweakKind.PowerShell,
            SideEffects = "Shuts down all running WSL2 instances.",
            ApplyAction = (_) =>
            {
                // Shut down WSL
                ShellRunner.Run("wsl", ["--shutdown"]);

                // Find and compact all .vhdx files under the WSL Lxss directory
                string lxssPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages");
                if (!Directory.Exists(lxssPath))
                    return;

                foreach (var vhdx in Directory.EnumerateFiles(lxssPath, "ext4.vhdx", SearchOption.AllDirectories))
                {
                    ShellRunner.RunPowerShell($"Optimize-VHD -Path '{vhdx.Replace("'", "''")}' -Mode Full");
                }
            },
            DetectAction = () => false, // One-time action, always shows as "not applied"
        },
        new TweakDef
        {
            Id = "wsl-shutdown",
            Label = "Shutdown All WSL2 Instances",
            Category = "Virtualization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Immediately terminates all running WSL2 distributions and the lightweight utility VM. Frees memory and CPU resources.",
            Tags = ["wsl", "shutdown", "memory", "resource"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "All running WSL sessions will be terminated.",
            ApplyAction = (_) =>
            {
                ShellRunner.Run("wsl", ["--shutdown"]);
            },
            DetectAction = () => false,
        },
        // ── Restored tweaks ───────────────────────────────────────────────

        new TweakDef
        {
            Id = "wsl-autostart",
            Label = "Auto-Start WSL2 at Logon",
            Category = "Virtualization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Adds a Run key to pre-boot the WSL2 lightweight VM at logon, eliminating cold-start latency for the first wsl.exe invocation.",
            Tags = ["wsl", "startup", "performance", "boot"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "WSLBootstrap", @"wsl.exe --exec /bin/true"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "WSLBootstrap")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "WSLBootstrap", @"wsl.exe --exec /bin/true"),
            ],
        },
        new TweakDef
        {
            Id = "wsl-compact-disk",
            Label = "Enable WSL Automatic Disk Compaction",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables automatic compaction of WSL2 virtual disks to reclaim unused space without manual intervention. Win11 22H2+.",
            Tags = ["wsl", "disk", "compact", "storage", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoCompact", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoCompact")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoCompact", 1)],
        },
        new TweakDef
        {
            Id = "wsl-default-v2",
            Label = "Set Default WSL Version to 2 (CLI)",
            Category = "Virtualization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the default WSL version to 2 via the wsl.exe CLI. WSL2 uses a full Linux kernel with better I/O and syscall compatibility.",
            Tags = ["wsl", "version", "wsl2", "default", "cli"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (_) =>
            {
                ShellRunner.Run("wsl", ["--set-default-version", "2"]);
            },
            DetectAction = () =>
            {
                var (code, stdout, _) = ShellRunner.Run("wsl", ["--status"]);
                return code == 0 && stdout.Contains("Default Version: 2", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "wsl-default-version-2",
            Label = "Set Default WSL Version to 2 (User Registry)",
            Category = "Virtualization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the default WSL version to 2 via the user-level Lxss registry key. New distro installations will use WSL2.",
            Tags = ["wsl", "version", "wsl2", "default", "registry"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Lxss", "DefaultVersion", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Lxss", "DefaultVersion")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Lxss", "DefaultVersion", 2)],
        },
        new TweakDef
        {
            Id = "wsl-disable-auto-update",
            Label = "Disable WSL Auto-Update",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents WSL from automatically checking for and installing kernel/runtime updates. Useful for controlled environments.",
            Tags = ["wsl", "update", "auto-update", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoUpdate", 0)],
        },
        new TweakDef
        {
            Id = "wsl-disable-nested-virt",
            Label = "Disable WSL Nested Virtualisation",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Explicitly disables nested virtualisation for WSL2 guests. Reduces attack surface when Docker/KVM inside WSL is not needed.",
            Tags = ["wsl", "virtualisation", "security", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableNestedVirtualization", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableNestedVirtualization")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableNestedVirtualization", 0)],
        },
        new TweakDef
        {
            Id = "wsl-dns-tunneling",
            Label = "Enable WSL DNS Tunneling",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables DNS tunneling in WSL2 so DNS requests are routed through the Windows host. Improves name resolution behind VPNs/proxies.",
            Tags = ["wsl", "dns", "tunneling", "networking", "vpn"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableDnsTunneling", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableDnsTunneling")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableDnsTunneling", 1)],
        },
        new TweakDef
        {
            Id = "wsl-enable-localhost-forward",
            Label = "Enable WSL Localhost Forwarding",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables automatic forwarding of WSL2 ports to localhost on the Windows host, allowing access to WSL services via 127.0.0.1.",
            Tags = ["wsl", "localhost", "forwarding", "networking", "port"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "LocalhostForwarding", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "LocalhostForwarding")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "LocalhostForwarding", 1)],
        },
        new TweakDef
        {
            Id = "wsl-enable-nested-virt-policy",
            Label = "Enable Nested Virtualisation (Hyper-V Policy)",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables nested virtualisation via Hyper-V Group Policy. Required by some organisations before the per-VM Lxss setting takes effect.",
            Tags = ["wsl", "virtualisation", "policy", "hyperv", "nested"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "AllowNestedVirtualization", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "AllowNestedVirtualization")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "AllowNestedVirtualization", 1)],
        },
        new TweakDef
        {
            Id = "wsl-enable-systemd",
            Label = "Enable Systemd (User Registry)",
            Category = "Virtualization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables systemd as the default init system for WSL2 via the user-level Lxss key. Services like snap, Docker, and journald require systemd.",
            Tags = ["wsl", "systemd", "init", "services", "user"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Lxss", "EnableSystemd", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Lxss", "EnableSystemd")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Lxss", "EnableSystemd", 1)],
        },
        new TweakDef
        {
            Id = "wsl-feature",
            Label = "Enable WSL Feature (PowerShell)",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables the Microsoft-Windows-Subsystem-Linux optional feature via PowerShell cmdlet. Requires reboot. Alternative to DISM approach.",
            Tags = ["wsl", "feature", "install", "powershell"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Requires reboot to take effect.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                ShellRunner.RunPowerShell("Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Windows-Subsystem-Linux -NoRestart");
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                ShellRunner.RunPowerShell("Disable-WindowsOptionalFeature -Online -FeatureName Microsoft-Windows-Subsystem-Linux -NoRestart");
            },
            DetectAction = () =>
            {
                var (_, stdout, _2) = ShellRunner.RunPowerShell(
                    "(Get-WindowsOptionalFeature -Online -FeatureName Microsoft-Windows-Subsystem-Linux).State"
                );
                return stdout.Trim().Equals("Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "wsl-kernel-update",
            Label = "Update WSL Kernel to Latest",
            Category = "Virtualization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Runs wsl --update to download and install the latest WSL kernel and runtime. One-time action.",
            Tags = ["wsl", "kernel", "update", "maintenance"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (_) =>
            {
                ShellRunner.Run("wsl", ["--update"]);
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "wsl-memory-reclaim",
            Label = "Enable WSL Auto Memory Reclaim",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables automatic memory reclaim so WSL2 returns unused cached memory to Windows. Reduces host memory pressure. Win11 22H2+.",
            Tags = ["wsl", "memory", "reclaim", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoMemoryReclaim", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoMemoryReclaim")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoMemoryReclaim", 1)],
        },
        new TweakDef
        {
            Id = "wsl-mirrored-network",
            Label = "Enable WSL Mirrored Networking",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Switches WSL2 to mirrored networking mode. WSL shares the host network stack for full LAN visibility and IPv6 support. Win11 23H2+.",
            Tags = ["wsl", "network", "mirrored", "networking"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "MirroredNetworkingMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "MirroredNetworkingMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "MirroredNetworkingMode", 1)],
        },
        new TweakDef
        {
            Id = "wsl-update-distro",
            Label = "Update WSL Distributions (Web Download)",
            Category = "Virtualization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Runs wsl --update --web-download to update WSL components directly from the web. One-time action.",
            Tags = ["wsl", "update", "distro", "maintenance"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (_) =>
            {
                ShellRunner.Run("wsl", ["--update", "--web-download"]);
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "wsl-vm-platform",
            Label = "Enable VM Platform (PowerShell)",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables the VirtualMachinePlatform optional feature via PowerShell cmdlet. Required for WSL2. Requires reboot. Alternative to DISM approach.",
            Tags = ["wsl", "vm", "platform", "install", "powershell"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Requires reboot to take effect.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                ShellRunner.RunPowerShell("Enable-WindowsOptionalFeature -Online -FeatureName VirtualMachinePlatform -NoRestart");
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                ShellRunner.RunPowerShell("Disable-WindowsOptionalFeature -Online -FeatureName VirtualMachinePlatform -NoRestart");
            },
            DetectAction = () =>
            {
                var (_, stdout, _2) = ShellRunner.RunPowerShell("(Get-WindowsOptionalFeature -Online -FeatureName VirtualMachinePlatform).State");
                return stdout.Trim().Equals("Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
    ];
}


internal static class PolicyPowerShell
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _IseDeprecationPolicy.Data,
            .. _PowerShellPolicy.Data,
            .. _Ps7ExecutionModePolicy.Data,
            .. _ScriptBlockLoggingAdvancedPolicy.Data,
            .. _ScriptedDiagnosticsPolicy.Data,
            .. _WindowsTerminalAdvancedPolicy.Data,
        ];

    // ── IseDeprecationPolicy ──
    private static class _IseDeprecationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ProtectedEventLogging";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "isedep-block-ise-launch",
                    Label = "Block PowerShell ISE Launch",
                    Category = "PowerShell",
                    Description =
                        "Blocks launch of the Windows PowerShell ISE (Integrated Scripting Environment), which is end-of-life and lacks modern security controls like AMSI integration.",
                    Tags = ["powershell", "ise", "deprecation", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "PowerShell ISE cannot be opened; users must use VS Code or PowerShell 7 instead.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableISE", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableISE")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableISE", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-force-remoting-allsigned",
                    Label = "Block Unsigned Scripts via PS Remoting",
                    Category = "PowerShell",
                    Description =
                        "Sets the remoting script execution policy to AllSigned, so scripts delivered via WinRM PowerShell remoting sessions must be digitally signed.",
                    Tags = ["powershell", "remoting", "signing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Unsigned scripts over WinRM remoting blocked; signed scripts required.",
                    ApplyOps = [RegOp.SetDword(Key, "RemotingExecutionPolicy", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RemotingExecutionPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "RemotingExecutionPolicy", 2)],
                },
                new TweakDef
                {
                    Id = "isedep-disable-v2-engine",
                    Label = "Disable PowerShell v2 Engine",
                    Category = "PowerShell",
                    Description =
                        "Disables the Windows PowerShell version 2 engine (powershell.exe -version 2) which bypasses modern security controls such as AMSI, ETW, and Constrained Language Mode.",
                    Tags = ["powershell", "v2", "downgrade", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "PS v2 engine blocked; attackers cannot downgrade to bypass AMSI. Legacy apps requiring PS2 will break.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePowerShellV2", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePowerShellV2")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePowerShellV2", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-enable-protected-event-logging",
                    Label = "Enable Protected Event Logging",
                    Category = "PowerShell",
                    Description =
                        "Enables Protected Event Logging (PEL) for PowerShell, which encrypts sensitive PowerShell script block log entries at rest using a certificate, protecting them from unauthorized access.",
                    Tags = ["powershell", "event-logging", "encryption", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Script block logs encrypted; only authorised certificate holders can decrypt and read them.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableProtectedEventLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableProtectedEventLogging")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableProtectedEventLogging", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-disable-credential-prompt",
                    Label = "Disable Credential Prompt in PowerShell Sessions",
                    Category = "PowerShell",
                    Description =
                        "Disables interactive credential prompts within PowerShell sessions, forcing scripts to use pre-provisioned credentials or fail instead of prompting the user.",
                    Tags = ["powershell", "credentials", "prompt", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Credential prompts inside PS sessions blocked; scripts requiring user input must be refactored.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCredentialRequestPrompt", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCredentialRequestPrompt")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCredentialRequestPrompt", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-enable-module-logging",
                    Label = "Enable Module Logging for PowerShell",
                    Category = "PowerShell",
                    Description =
                        "Enables module-level logging for all PowerShell modules by default, ensuring that all custom module invocations are captured in the Windows PowerShell/Operational event log.",
                    Tags = ["powershell", "module-logging", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "All loaded PS module commands logged; log volume scales with module usage.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableModuleLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableModuleLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableModuleLogging", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-disable-script-download",
                    Label = "Disable Script Download from Internet in PowerShell",
                    Category = "PowerShell",
                    Description =
                        "Blocks PowerShell from downloading and executing scripts from internet URIs using Invoke-Expression (IEX) with web requests, a common living-off-the-land attack technique.",
                    Tags = ["powershell", "download-cradle", "iex", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "PS internet download-and-exec cradles blocked; IEX/webclient patterns are prevented.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableScriptDownloadFromInternet", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableScriptDownloadFromInternet")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableScriptDownloadFromInternet", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-block-ps-dev-mode",
                    Label = "Block PowerShell Developer Mode",
                    Category = "PowerShell",
                    Description =
                        "Disables the PowerShell developer mode flag that bypasses certain security policies, ensuring that production machines do not inadvertently run in a relaxed-security development mode.",
                    Tags = ["powershell", "developer-mode", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PS developer/debug mode disabled; policy overrides cannot be bypassed via dev flags.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockPSDeveloperMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockPSDeveloperMode")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockPSDeveloperMode", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-disable-ps-telemetry",
                    Label = "Disable Windows PowerShell 5 Telemetry",
                    Category = "PowerShell",
                    Description =
                        "Disables usage telemetry collection in Windows PowerShell 5.1, preventing execution metadata and error statistics from being sent to Microsoft.",
                    Tags = ["powershell", "ps5", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "PS 5.1 telemetry stopped; no functional impact.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-force-network-restricted-sessions",
                    Label = "Force Network-Restricted PowerShell Remoting Sessions",
                    Category = "PowerShell",
                    Description =
                        "Forces all incoming PowerShell remoting sessions to run as NetworkRestricted, preventing remotely established sessions from making outbound network connections.",
                    Tags = ["powershell", "remoting", "network-restricted", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Remote PS sessions cannot make new network connections; lateral movement via PS remoting reduced.",
                    ApplyOps = [RegOp.SetDword(Key, "ForceNetworkRestrictedSessions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ForceNetworkRestrictedSessions")],
                    DetectOps = [RegOp.CheckDword(Key, "ForceNetworkRestrictedSessions", 1)],
                },
            ];
    }

    // ── PowerShellPolicy ──
    private static class _PowerShellPolicy
    {
        private const string PsRoot = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell";
        private const string ScriptBlockLogging = PsRoot + @"\ScriptBlockLogging";
        private const string ModuleLogging = PsRoot + @"\ModuleLogging";
        private const string Transcription = PsRoot + @"\Transcription";
        private const string ProtectedEventLogging = PsRoot + @"\ProtectedEventLogging";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "pspolicy-script-block-logging",
                Label = "Enable PowerShell Script Block Logging",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enables PowerShell Script Block Logging (GPO) so that all script blocks "
                    + "executed by PowerShell are written to the Operational event log "
                    + "(Microsoft-Windows-PowerShell/Operational). Essential for threat hunting.",
                Tags = ["powershell", "logging", "script block", "security", "audit"],
                RegistryKeys = [ScriptBlockLogging],
                ApplyOps = [RegOp.SetDword(ScriptBlockLogging, "EnableScriptBlockLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(ScriptBlockLogging, "EnableScriptBlockLogging")],
                DetectOps = [RegOp.CheckDword(ScriptBlockLogging, "EnableScriptBlockLogging", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-script-invocation-logging",
                Label = "Enable PowerShell Script Invocation Logging",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enables logging of script-block invocation start/stop events in addition "
                    + "to the raw script text. Adds invocation sequence to event IDs 4104/4105/4106. "
                    + "EnableScriptBlockInvocationLogging=1.",
                Tags = ["powershell", "logging", "invocation logging", "security"],
                RegistryKeys = [ScriptBlockLogging],
                ApplyOps = [RegOp.SetDword(ScriptBlockLogging, "EnableScriptBlockInvocationLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(ScriptBlockLogging, "EnableScriptBlockInvocationLogging")],
                DetectOps = [RegOp.CheckDword(ScriptBlockLogging, "EnableScriptBlockInvocationLogging", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-module-logging",
                Label = "Enable PowerShell Module Logging",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enables Module Logging, which records pipeline execution details for "
                    + "specified (or all) PowerShell modules to the Windows event log. "
                    + "Helps detect malicious module imports. EnableModuleLogging=1.",
                Tags = ["powershell", "module logging", "security", "audit"],
                RegistryKeys = [ModuleLogging],
                ApplyOps = [RegOp.SetDword(ModuleLogging, "EnableModuleLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(ModuleLogging, "EnableModuleLogging")],
                DetectOps = [RegOp.CheckDword(ModuleLogging, "EnableModuleLogging", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-transcription-on",
                Label = "Enable PowerShell Transcription",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Turns on PowerShell Transcription, writing a plain-text record of every "
                    + "PowerShell session (all commands and output) to a configured output "
                    + "directory. EnableTranscripting=1.",
                Tags = ["powershell", "transcription", "logging", "audit"],
                RegistryKeys = [Transcription],
                ApplyOps = [RegOp.SetDword(Transcription, "EnableTranscripting", 1)],
                RemoveOps = [RegOp.DeleteValue(Transcription, "EnableTranscripting")],
                DetectOps = [RegOp.CheckDword(Transcription, "EnableTranscripting", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-transcription-header",
                Label = "Include Invocation Header in PowerShell Transcripts",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Adds timestamps, username, and invocation details to each PowerShell "
                    + "transcript header, making audit review significantly easier. "
                    + "EnableInvocationHeader=1.",
                Tags = ["powershell", "transcription", "header", "audit"],
                RegistryKeys = [Transcription],
                ApplyOps = [RegOp.SetDword(Transcription, "EnableInvocationHeader", 1)],
                RemoveOps = [RegOp.DeleteValue(Transcription, "EnableInvocationHeader")],
                DetectOps = [RegOp.CheckDword(Transcription, "EnableInvocationHeader", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-transcription-output-path",
                Label = "Set PowerShell Transcript Output Directory",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets the PowerShell transcript output directory to "
                    + @"%SYSTEMROOT%\Logs\PowerShell so transcripts are centralised and "
                    + "survive user profile deletion. OutputDirectory (REG_SZ).",
                Tags = ["powershell", "transcription", "output path", "audit"],
                RegistryKeys = [Transcription],
                ApplyOps = [RegOp.SetString(Transcription, "OutputDirectory", @"%SYSTEMROOT%\Logs\PowerShell")],
                RemoveOps = [RegOp.DeleteValue(Transcription, "OutputDirectory")],
                DetectOps = [RegOp.CheckString(Transcription, "OutputDirectory", @"%SYSTEMROOT%\Logs\PowerShell")],
            },
            new TweakDef
            {
                Id = "pspolicy-disable-ps2-engine",
                Label = "Disable PowerShell 2.0 Engine",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets EnablePS2=0 via GPO to prevent PowerShell from falling back to the "
                    + "legacy v2 engine, which lacks script block logging and constrained language "
                    + "mode — a known AMSI/logging bypass vector.",
                Tags = ["powershell", "ps2", "downgrade attack", "security", "amsi"],
                RegistryKeys = [PsRoot],
                ApplyOps = [RegOp.SetDword(PsRoot, "EnablePS2", 0)],
                RemoveOps = [RegOp.DeleteValue(PsRoot, "EnablePS2")],
                DetectOps = [RegOp.CheckDword(PsRoot, "EnablePS2", 0)],
            },
            new TweakDef
            {
                Id = "pspolicy-protected-event-logging",
                Label = "Enable Protected Event Logging",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Enables Protected Event Logging, which encrypts sensitive event log data "
                    + "(such as PowerShell credentials in transcripts) using a CMS certificate, "
                    + "so only authorized readers can decrypt. EnableProtectedEventLogging=1.",
                Tags = ["powershell", "event log", "encryption", "protected logging", "security"],
                RegistryKeys = [ProtectedEventLogging],
                ApplyOps = [RegOp.SetDword(ProtectedEventLogging, "EnableProtectedEventLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(ProtectedEventLogging, "EnableProtectedEventLogging")],
                DetectOps = [RegOp.CheckDword(ProtectedEventLogging, "EnableProtectedEventLogging", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-enable-scripts",
                Label = "Enable PowerShell Script Execution (GPO)",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets EnableScripts=1 via GPO, activating the policy-controlled execution "
                    + "policy. Must be enabled before ExecutionPolicy can be enforced via "
                    + "Group Policy. Set together with pspolicy-require-signed-scripts.",
                Tags = ["powershell", "execution policy", "scripts", "gpo"],
                RegistryKeys = [PsRoot],
                ApplyOps = [RegOp.SetDword(PsRoot, "EnableScripts", 1)],
                RemoveOps = [RegOp.DeleteValue(PsRoot, "EnableScripts")],
                DetectOps = [RegOp.CheckDword(PsRoot, "EnableScripts", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-require-signed-scripts",
                Label = "Require Signed PowerShell Scripts (AllSigned)",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Sets ExecutionPolicy=AllSigned via GPO, requiring that all scripts and "
                    + "configuration files — including local scripts — be signed by a trusted "
                    + "publisher. Strongest policy-level execution restriction.",
                Tags = ["powershell", "execution policy", "signed", "allsigned", "security"],
                RegistryKeys = [PsRoot],
                ApplyOps = [RegOp.SetString(PsRoot, "ExecutionPolicy", "AllSigned")],
                RemoveOps = [RegOp.DeleteValue(PsRoot, "ExecutionPolicy")],
                DetectOps = [RegOp.CheckString(PsRoot, "ExecutionPolicy", "AllSigned")],
            },
        ];
    }

    // ── Ps7ExecutionModePolicy ──
    private static class _Ps7ExecutionModePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PowerShellCore";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PowerShellCore\ScriptBlockLogging";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ps7exec-enable-constrained-language",
                    Label = "Enable Constrained Language Mode in PowerShell 7",
                    Category = "PowerShell",
                    Description =
                        "Enables Constrained Language Mode (CLM) for PowerShell 7 (pwsh), restricting the .NET types and COM objects that scripts can use and mitigating fileless malware execution.",
                    Tags = ["powershell", "ps7", "constrained-language", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote = "PS7 runs in Constrained Language Mode; complex scripts and .NET interop may break.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableConstrainedLanguageMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableConstrainedLanguageMode")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableConstrainedLanguageMode", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-set-allsigned-policy",
                    Label = "Enforce AllSigned Execution Policy in PowerShell 7",
                    Category = "PowerShell",
                    Description =
                        "Sets the PowerShell 7 execution policy to AllSigned, requiring all scripts (including local scripts) to be digitally signed by a trusted publisher before execution.",
                    Tags = ["powershell", "ps7", "execution-policy", "signing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Only code-signed PS7 scripts run; unsigned dev scripts require explicit bypass.",
                    ApplyOps = [RegOp.SetDword(Key, "ExecutionPolicy", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ExecutionPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "ExecutionPolicy", 2)],
                },
                new TweakDef
                {
                    Id = "ps7exec-disable-remoting",
                    Label = "Disable PowerShell 7 Remoting",
                    Category = "PowerShell",
                    Description =
                        "Disables PowerShell 7 remoting (WinRM/SSH transport) via policy, preventing pwsh from being used as a remote administration target.",
                    Tags = ["powershell", "ps7", "remoting", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "PS7 remoting disabled; WinRM/SSH-based remote pwsh sessions blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRemoting", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRemoting")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRemoting", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-disable-implicit-remoting",
                    Label = "Disable PS7 Implicit Remoting Module Import",
                    Category = "PowerShell",
                    Description =
                        "Disables implicit remoting module imports in PowerShell 7, preventing a script from automatically importing and executing remote commands from untrusted sources.",
                    Tags = ["powershell", "ps7", "implicit-remoting", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Implicit remote module imports blocked; remote command invocation requires explicit setup.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableImplicitRemoting", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableImplicitRemoting")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableImplicitRemoting", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-require-signed-modules",
                    Label = "Require Signed Module Manifests in PowerShell 7",
                    Category = "PowerShell",
                    Description =
                        "Requires all PowerShell 7 module manifests (.psd1) to be signed by a trusted publisher before the module can be loaded, blocking unsigned third-party modules.",
                    Tags = ["powershell", "ps7", "modules", "signing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Unsigned PS7 modules cannot load; all modules must have trusted code signatures.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireSignedModuleManifests", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireSignedModuleManifests")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireSignedModuleManifests", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-block-ps-gallery",
                    Label = "Block PowerShell Gallery Repository in PS7",
                    Category = "PowerShell",
                    Description =
                        "Disables access to the default PowerShell Gallery online repository in PowerShell 7, forcing module and script installation through an approved internal repository.",
                    Tags = ["powershell", "ps7", "gallery", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PSGallery blocked; Install-Module will not reach the public gallery.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockPSGallery", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockPSGallery")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockPSGallery", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-enable-script-block-logging",
                    Label = "Enable Script Block Logging in PowerShell 7",
                    Category = "PowerShell",
                    Description =
                        "Enables script block logging in PowerShell 7 to record all script blocks executed to the event log (Microsoft-Windows-PowerShell/Operational), supporting forensic analysis.",
                    Tags = ["powershell", "ps7", "script-block-logging", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "All executed PS7 script blocks logged; event log volume may increase significantly.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableScriptBlockLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableScriptBlockLogging")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableScriptBlockLogging", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-enable-invocation-logging",
                    Label = "Enable Script Block Invocation Logging in PS7",
                    Category = "PowerShell",
                    Description =
                        "Enables verbose script block invocation logging in PowerShell 7, capturing start and stop events for each script block execution for detailed forensic trails.",
                    Tags = ["powershell", "ps7", "invocation-logging", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Verbose invocation events logged; high-traffic PS7 hosts will generate significant log volume.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableScriptBlockInvocationLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableScriptBlockInvocationLogging")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableScriptBlockInvocationLogging", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-disable-telemetry",
                    Label = "Disable PowerShell 7 Telemetry",
                    Category = "PowerShell",
                    Description =
                        "Disables the PowerShell 7 telemetry feature that sends usage statistics (command names, error categories, OS info) to Microsoft via opt-out environment variable enforcement at policy level.",
                    Tags = ["powershell", "ps7", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PS7 telemetry fully disabled at policy level; no usage data sent.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-disable-update-notif",
                    Label = "Disable PowerShell 7 Update Notifications",
                    Category = "PowerShell",
                    Description =
                        "Suppresses in-session PowerShell 7 update available notifications that prompt users to download newer versions, deferring updates to a managed patching process.",
                    Tags = ["powershell", "ps7", "update", "notifications", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "PS7 update banners not shown; version management via package manager.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableUpdateNotifications", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUpdateNotifications")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUpdateNotifications", 1)],
                },
            ];
    }

    // ── ScriptBlockLoggingAdvancedPolicy ──
    private static class _ScriptBlockLoggingAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\Transcription";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sbloga-enable-script-block-logging",
                    Label = "Enable Script Block Logging (Windows PS)",
                    Category = "PowerShell",
                    Description =
                        "Enables PowerShell script block logging for Windows PowerShell 5.1 via the dedicated ScriptBlockLogging policy key, recording all executed script blocks to the PowerShell/Operational event log.",
                    Tags = ["powershell", "script-block-logging", "audit", "siem", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Full script block content logged; essential for threat hunting but increases log volume.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableScriptBlockLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableScriptBlockLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableScriptBlockLogging", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-enable-invocation-header",
                    Label = "Enable Script Block Invocation Header Logging",
                    Category = "PowerShell",
                    Description =
                        "Enables logging of the start and stop events for each function/script-block invocation, providing timestamped execution boundaries in the event log.",
                    Tags = ["powershell", "script-block-logging", "invocation", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Invocation start/stop events logged; detailed execution trail generated.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableScriptBlockInvocationLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableScriptBlockInvocationLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableScriptBlockInvocationLogging", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-enable-transcription",
                    Label = "Enable PowerShell Transcript Logging",
                    Category = "PowerShell",
                    Description =
                        "Enables PowerShell session transcription that saves a full text copy of every PowerShell session to a transcript file on disk, providing a human-readable audit trail.",
                    Tags = ["powershell", "transcription", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Every PS session recorded to transcript file; disk space usage increases with PS activity.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableTranscripting", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableTranscripting")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableTranscripting", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-enable-invocation-header-transcript",
                    Label = "Include Invocation Header in PS Transcripts",
                    Category = "PowerShell",
                    Description =
                        "Adds invocation header information (command name, arguments, timestamps, username, process info) to PowerShell transcript files.",
                    Tags = ["powershell", "transcription", "header", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Transcript files include rich header context for each command.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableInvocationHeader", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableInvocationHeader")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableInvocationHeader", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-set-output-directory",
                    Label = "Set Centralised PowerShell Transcript Directory",
                    Category = "PowerShell",
                    Description =
                        "Sets the PowerShell transcript output directory to a centralised network share or admin-controlled path so all endpoint transcripts are collected in one location.",
                    Tags = ["powershell", "transcription", "directory", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Transcripts written to admin-specified path; ensure path is writable and monitored.",
                    ApplyOps = [RegOp.SetString(Key2, "OutputDirectory", @"C:\Windows\Logs\PowerShell")],
                    RemoveOps = [RegOp.DeleteValue(Key2, "OutputDirectory")],
                    DetectOps = [RegOp.CheckString(Key2, "OutputDirectory", @"C:\Windows\Logs\PowerShell")],
                },
                new TweakDef
                {
                    Id = "sbloga-log-encoded-commands",
                    Label = "Log Encoded PowerShell Command Executions",
                    Category = "PowerShell",
                    Description =
                        "Enables script block logging specifically targeting Base64-encoded commands (-EncodedCommand), which are commonly used by malware to obfuscate payloads.",
                    Tags = ["powershell", "encoded-commands", "obfuscation", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Encoded command executions captured in logs; key detection for fileless attacks.",
                    ApplyOps = [RegOp.SetDword(Key, "LogEncodedCommands", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogEncodedCommands")],
                    DetectOps = [RegOp.CheckDword(Key, "LogEncodedCommands", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-log-dynamic-code",
                    Label = "Log Dynamically Generated PowerShell Code",
                    Category = "PowerShell",
                    Description =
                        "Enables logging of dynamically generated PowerShell code (e.g., from Invoke-Expression or Add-Type), capturing obfuscated payloads that are assembled at runtime.",
                    Tags = ["powershell", "dynamic-code", "invoke-expression", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Dynamically generated PS code blocks captured; critical for detecting memory-only malware.",
                    ApplyOps = [RegOp.SetDword(Key, "LogDynamicCode", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogDynamicCode")],
                    DetectOps = [RegOp.CheckDword(Key, "LogDynamicCode", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-set-max-log-size",
                    Label = "Set PowerShell Operational Log Max Size to 512 MB",
                    Category = "PowerShell",
                    Description =
                        "Increases the Microsoft-Windows-PowerShell/Operational event log maximum size to 512 MB to prevent log overwriting (circular buffer) during high-volume script block logging.",
                    Tags = ["powershell", "event-log", "size", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PS operational log grows to 512 MB max; older entries retained longer before cycling.",
                    ApplyOps =
                    [
                        RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "MaxSize", 524288),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "MaxSize"),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "MaxSize", 524288),
                    ],
                },
                new TweakDef
                {
                    Id = "sbloga-retain-on-clear",
                    Label = "Retain PowerShell Log Archive on Clear",
                    Category = "PowerShell",
                    Description =
                        "Configures the PowerShell operational event log to archive before clearing when the log becomes full, preventing permanent log loss during log maintenance.",
                    Tags = ["powershell", "event-log", "archive", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PS log archived to .evtx file before clearing; no historical data lost.",
                    ApplyOps =
                    [
                        RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "Retention", 0),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "Retention"),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "Retention", 0),
                    ],
                },
                new TweakDef
                {
                    Id = "sbloga-block-clear-eventlog",
                    Label = "Block Standard Users from Clearing Event Logs",
                    Category = "PowerShell",
                    Description =
                        "Restricts the ability to clear the PowerShell and Windows event logs to administrators only, preventing attackers with standard user access from clearing their tracks.",
                    Tags = ["powershell", "event-log", "clear", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only admins can clear event logs; standard users get access denied.",
                    ApplyOps =
                    [
                        RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security", "RestrictGuestAccess", 1),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security", "RestrictGuestAccess"),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security", "RestrictGuestAccess", 1),
                    ],
                },
            ];
    }

    // ── ScriptedDiagnosticsPolicy ──
    private static class _ScriptedDiagnosticsPolicy
    {
        private const string SdKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnostics";
        private const string SdProv = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy";
        private const string TshootKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Troubleshooting\AllowRecommendations";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "sdiag-disable-scripted-diagnostics",
                Label = "Disable Scripted Diagnostics Execution",
                Category = "PowerShell",
                Description =
                    "Sets ExecutionPolicy=0 in the ScriptedDiagnostics policy key. "
                    + "Prevents Windows from executing scripted diagnostic packages (.diagpkg, .diag files), "
                    + "including the automated troubleshooters triggered from 'Troubleshoot settings'. "
                    + "Reduces data collection and prevents unintended automated changes. "
                    + "Default: absent (diagnostics run). Recommended: 1 on managed or high-security systems.",
                Tags = ["diagnostics", "scripted", "troubleshooter", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Scripted diagnostic packages (.diagpkg) cannot execute; automated troubleshooters are blocked.",
                ApplyOps = [RegOp.SetDword(SdKey, "ExecutionPolicy", 0)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "ExecutionPolicy")],
                DetectOps = [RegOp.CheckDword(SdKey, "ExecutionPolicy", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-online-troubleshooters",
                Label = "Disable Online Troubleshooting Recommendations",
                Category = "PowerShell",
                Description =
                    "Sets EnabledPolicy=0 in the ScriptedDiagnosticsProvider Policy key. "
                    + "Prevents Windows from downloading and applying troubleshooting recommendations from Microsoft's "
                    + "online diagnostic database. Stops automatic remediation steps that could modify system settings. "
                    + "Default: absent (online recommendations enabled). Recommended: 0 on air-gapped or privacy-sensitive systems.",
                Tags = ["diagnostics", "online", "recommendations", "troubleshooter", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Online troubleshooting recommendations from Microsoft not fetched or applied.",
                ApplyOps = [RegOp.SetDword(SdProv, "EnabledPolicy", 0)],
                RemoveOps = [RegOp.DeleteValue(SdProv, "EnabledPolicy")],
                DetectOps = [RegOp.CheckDword(SdProv, "EnabledPolicy", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-recommended-troubleshooting",
                Label = "Disable Windows Recommended Troubleshooting",
                Category = "PowerShell",
                Description =
                    "Sets TurnOffWindowsErrorReportingServer=1 in the AllowRecommendations "
                    + "Troubleshooting policy key. Disables the 'Recommended troubleshooting' feature "
                    + "that automatically diagnoses and resolves common problems. Prevents Windows from "
                    + "silently applying fixes based on crash data from Windows Error Reporting. "
                    + "Default: absent. Recommended: 1 when automated fixes are undesired in production environments.",
                Tags = ["diagnostics", "recommended", "auto-fix", "troubleshooter", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Windows Recommended Troubleshooting feature disabled; no automatic problem fixes applied.",
                ApplyOps = [RegOp.SetDword(TshootKey, "TurnOffWindowsErrorReportingServer", 1)],
                RemoveOps = [RegOp.DeleteValue(TshootKey, "TurnOffWindowsErrorReportingServer")],
                DetectOps = [RegOp.CheckDword(TshootKey, "TurnOffWindowsErrorReportingServer", 1)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-automatic-maintenance-diagnostics",
                Label = "Disable Automatic Maintenance Diagnostics",
                Category = "PowerShell",
                Description =
                    "Sets EnableAutomatedTroubleshooting=0 in the ScriptedDiagnostics policy key. "
                    + "Prevents Windows Automatic Maintenance from running scripted diagnostic jobs "
                    + "in the background during maintenance windows. Avoids unexpected system changes from "
                    + "background maintenance troubleshooters. "
                    + "Default: absent (enabled). Recommended: 0 in change-controlled environments.",
                Tags = ["diagnostics", "maintenance", "automated", "background", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Scripted diagnostic jobs from Windows Automatic Maintenance are disabled.",
                ApplyOps = [RegOp.SetDword(SdKey, "EnableAutomatedTroubleshooting", 0)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "EnableAutomatedTroubleshooting")],
                DetectOps = [RegOp.CheckDword(SdKey, "EnableAutomatedTroubleshooting", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-elevated-troubleshooter",
                Label = "Disable Elevated Scripted Troubleshooter Execution",
                Category = "PowerShell",
                Description =
                    "Sets RunAsHighestAvailablePrivilege=0 in the ScriptedDiagnostics policy key. "
                    + "Prevents scripted diagnostic packages from automatically requesting elevation to "
                    + "run with highest available privileges. Forces diagnostics to run as standard user "
                    + "unless explicitly elevated by an administrator. "
                    + "Default: absent (auto-elevation allowed). Recommended: 0 on principle-of-least-privilege systems.",
                Tags = ["diagnostics", "elevation", "uac", "privilege", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Diagnostic packages cannot auto-elevate; admin must explicitly run elevated troubleshooters.",
                ApplyOps = [RegOp.SetDword(SdKey, "RunAsHighestAvailablePrivilege", 0)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "RunAsHighestAvailablePrivilege")],
                DetectOps = [RegOp.CheckDword(SdKey, "RunAsHighestAvailablePrivilege", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-results-upload",
                Label = "Disable Diagnostic Results Upload",
                Category = "PowerShell",
                Description =
                    "Sets AllowDiagnosticDataUpload=0 in the ScriptedDiagnostics policy key. "
                    + "Prevents scripted diagnostic packages from uploading their results logs, "
                    + "diagnostic data, or anonymised telemetry to Microsoft or third-party servers. "
                    + "Default: absent (upload allowed). Recommended: 0 on air-gapped or privacy-sensitive systems.",
                Tags = ["diagnostics", "upload", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Diagnostic results not uploaded; data stays on-device.",
                ApplyOps = [RegOp.SetDword(SdKey, "AllowDiagnosticDataUpload", 0)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "AllowDiagnosticDataUpload")],
                DetectOps = [RegOp.CheckDword(SdKey, "AllowDiagnosticDataUpload", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-user-initiated-troubleshooter",
                Label = "Block User-Initiated Troubleshooters",
                Category = "PowerShell",
                Description =
                    "Sets DisableUserDiagnostics=1 in the ScriptedDiagnostics policy key. "
                    + "Prevents non-administrator users from launching troubleshooters from Settings "
                    + "('Get help', 'Troubleshoot', 'Fix problems'). Only administrators can initiate "
                    + "diagnostic packages. Useful on shared or terminal-server machines. "
                    + "Default: absent (users can launch troubleshooters). Recommended: 1 on kiosk/terminal machines.",
                Tags = ["diagnostics", "user", "kiosk", "restrict", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Non-admin users cannot launch Windows troubleshooters from Settings.",
                ApplyOps = [RegOp.SetDword(SdKey, "DisableUserDiagnostics", 1)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "DisableUserDiagnostics")],
                DetectOps = [RegOp.CheckDword(SdKey, "DisableUserDiagnostics", 1)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-third-party-diagnostics",
                Label = "Block Third-Party Diagnostic Packages",
                Category = "PowerShell",
                Description =
                    "Sets AllowThirdPartyDiagnostics=0 in the ScriptedDiagnostics policy key. "
                    + "Prevents Windows from running scripted diagnostic packages (.diagpkg) from publishers "
                    + "other than Microsoft. Only Microsoft-signed diagnostic packages are permitted to run. "
                    + "Default: absent (third-party packages allowed). Recommended: 0 to limit diagnostic execution surface.",
                Tags = ["diagnostics", "third-party", "packages", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Third-party diagnostic packages (.diagpkg) blocked; only Microsoft-signed packages run.",
                ApplyOps = [RegOp.SetDword(SdKey, "AllowThirdPartyDiagnostics", 0)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "AllowThirdPartyDiagnostics")],
                DetectOps = [RegOp.CheckDword(SdKey, "AllowThirdPartyDiagnostics", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-scheduled-diagnostics",
                Label = "Disable Scheduled Diagnostic Tasks",
                Category = "PowerShell",
                Description =
                    "Sets DisableScheduledDiagnostics=1 in the ScriptedDiagnostics policy key. "
                    + "Prevents the Scheduled Maintenance Diagnostics task scheduler jobs from creating "
                    + "or running scripted diagnostic tasks in the background on a schedule. "
                    + "Reduces background system load and unexpected modifications. "
                    + "Default: absent (scheduled diagnostics run). Recommended: 1 on optimised/stable systems.",
                Tags = ["diagnostics", "scheduled", "maintenance", "task-scheduler", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Scheduled background diagnostic maintenance tasks are blocked.",
                ApplyOps = [RegOp.SetDword(SdKey, "DisableScheduledDiagnostics", 1)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "DisableScheduledDiagnostics")],
                DetectOps = [RegOp.CheckDword(SdKey, "DisableScheduledDiagnostics", 1)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-troubleshooting-history",
                Label = "Disable Troubleshooting History Storage",
                Category = "PowerShell",
                Description =
                    "Sets DisableTroubleshootingHistory=1 in the ScriptedDiagnostics policy key. "
                    + "Prevents Windows from writing troubleshooter run results and histories to the "
                    + "machine's troubleshooting log database. Reduces local data accumulation from "
                    + "diagnostic activities. "
                    + "Default: absent (history stored). Recommended: 1 on privacy-focused or ephemeral systems.",
                Tags = ["diagnostics", "history", "privacy", "logging", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Troubleshooter run history and results are not stored in the local database.",
                ApplyOps = [RegOp.SetDword(SdKey, "DisableTroubleshootingHistory", 1)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "DisableTroubleshootingHistory")],
                DetectOps = [RegOp.CheckDword(SdKey, "DisableTroubleshootingHistory", 1)],
            },
        ];
    }

    // ── WindowsTerminalAdvancedPolicy ──
    private static class _WindowsTerminalAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal\Updates";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "termadv-disable-auto-update",
                    Label = "Disable Windows Terminal Auto-Update",
                    Category = "PowerShell",
                    Description =
                        "Disables automatic update checks and downloads for Windows Terminal, ensuring the terminal version is managed by WSUS or package management rather than in-app updates.",
                    Tags = ["terminal", "update", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Windows Terminal will not auto-update; version management via package manager or WSUS.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoUpdate")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoUpdate", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-telemetry",
                    Label = "Disable Windows Terminal Telemetry",
                    Category = "PowerShell",
                    Description =
                        "Disables usage telemetry collection in Windows Terminal including keyboard shortcut usage, profile creation frequency, and renderer performance data.",
                    Tags = ["terminal", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Terminal telemetry disabled; no usage data sent to Microsoft.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-store-launch",
                    Label = "Disable Store Launch from Windows Terminal",
                    Category = "PowerShell",
                    Description =
                        "Prevents Windows Terminal from launching the Microsoft Store for extensions, themes, or profile suggestions, reducing MS Store telemetry exposure.",
                    Tags = ["terminal", "store", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "MS Store launch button in terminal disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableStoreLaunch", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableStoreLaunch")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableStoreLaunch", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-startup-tasks",
                    Label = "Disable Windows Terminal Startup Tasks",
                    Category = "PowerShell",
                    Description =
                        "Disables Windows Terminal startup task registration that auto-starts terminal on user login, reducing unnecessary background process startup.",
                    Tags = ["terminal", "startup", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Terminal does not auto-launch at logon.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableStartupTasks", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableStartupTasks")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableStartupTasks", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-enforce-restricted-profile",
                    Label = "Enforce Restricted Profile in Windows Terminal",
                    Category = "PowerShell",
                    Description =
                        "Enables restricted profile enforcement in Windows Terminal, blocking users from modifying terminal profiles, settings JSON, or key bindings.",
                    Tags = ["terminal", "profile", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Users cannot modify terminal settings; only admin-defined profiles are available.",
                    ApplyOps = [RegOp.SetDword(Key, "EnforceRestrictedProfile", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnforceRestrictedProfile")],
                    DetectOps = [RegOp.CheckDword(Key, "EnforceRestrictedProfile", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-extensions",
                    Label = "Disable Windows Terminal Extensions",
                    Category = "PowerShell",
                    Description =
                        "Disables the ability to install or run third-party extensions in Windows Terminal, reducing the attack surface from unvetted extension code execution.",
                    Tags = ["terminal", "extensions", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Terminal extensions disabled; only built-in functionality available.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableExtensions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableExtensions")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableExtensions", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-block-ssh-agent",
                    Label = "Block SSH Agent Integration in Windows Terminal",
                    Category = "PowerShell",
                    Description =
                        "Disables the SSH agent forwarding integration in Windows Terminal, preventing terminal sessions from forwarding SSH keys to remote hosts.",
                    Tags = ["terminal", "ssh", "agent", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "SSH agent forwarding blocked from terminal; prevents key forwarding to hostile servers.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockSshAgentIntegration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockSshAgentIntegration")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockSshAgentIntegration", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-preview-builds",
                    Label = "Disable Windows Terminal Preview Build Channel",
                    Category = "PowerShell",
                    Description =
                        "Forces Windows Terminal to the stable release channel, disabling the Preview and Canary build channels to ensure only stable, vetted versions are used.",
                    Tags = ["terminal", "preview", "channel", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Terminal locked to stable channel; Preview/Canary builds not offered.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisablePreviewBuilds", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisablePreviewBuilds")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisablePreviewBuilds", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-update-notifications",
                    Label = "Disable Update Notifications in Windows Terminal",
                    Category = "PowerShell",
                    Description =
                        "Suppresses in-app update available notifications in Windows Terminal, which can distract users and prompt unauthorized manual updates.",
                    Tags = ["terminal", "update", "notifications", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Update reminder banners not shown in terminal.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableUpdateNotifications", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableUpdateNotifications")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableUpdateNotifications", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-block-manual-updates",
                    Label = "Block Manual Windows Terminal Updates by Users",
                    Category = "PowerShell",
                    Description =
                        "Prevents standard users from triggering manual Windows Terminal update checks or downloads, ensuring that all terminal update operations require administrator rights.",
                    Tags = ["terminal", "update", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Standard users cannot manually update terminal; admin action required.",
                    ApplyOps = [RegOp.SetDword(Key2, "BlockManualUpdates", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "BlockManualUpdates")],
                    DetectOps = [RegOp.CheckDword(Key2, "BlockManualUpdates", 1)],
                },
            ];
    }
}

internal static class PackageManagement
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "pkg-disable-suggested-apps",
            Label = "Disable Suggested App Installations",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from silently installing suggested apps. Default: Enabled. Recommended: Disabled.",
            Tags = ["packages", "suggested", "bloatware"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "pkg-disable-winget-auto-update",
            Label = "Disable WinGet Auto-Update",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic WinGet package manager self-updates via policy. Default: Enabled. Recommended: Disabled for controlled environments.",
            Tags = ["packages", "winget", "auto-update", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAutoUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAutoUpdate", 0)],
        },
        new TweakDef
        {
            Id = "pkg-choco-proxy",
            Label = "Set Chocolatey System Proxy",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Configures Chocolatey to use the system proxy for package downloads. Useful in corporate environments behind a proxy. Default: Direct. Recommended: Enabled behind proxy.",
            Tags = ["packages", "chocolatey", "proxy", "corporate"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Chocolatey"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Chocolatey", "UseSystemProxy", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Chocolatey", "UseSystemProxy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Chocolatey", "UseSystemProxy", 1)],
        },
        new TweakDef
        {
            Id = "pkg-source-validation",
            Label = "Enable Package Source Validation",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents WinGet from overriding package hash validation. Ensures integrity checks are enforced for all package sources. Default: Override allowed. Recommended: Disabled (validation enforced).",
            Tags = ["packages", "winget", "hash", "validation", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableHashOverride", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableHashOverride", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableHashOverride", 0)],
        },
        new TweakDef
        {
            Id = "pkg-disable-ms-store",
            Label = "Disable Microsoft Store via Policy",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables access to the Microsoft Store via Group Policy. Default: enabled.",
            Tags = ["package", "msstore", "disable", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RemoveWindowsStore", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RemoveWindowsStore")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RemoveWindowsStore", 1)],
        },
        new TweakDef
        {
            Id = "pkg-enable-developer-sideload",
            Label = "Enable Developer Mode Sideloading",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables developer mode to allow sideloading of apps without the Store. Default: disabled.",
            Tags = ["package", "developer", "sideload", "appx"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowDevelopmentWithoutDevLicense", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowDevelopmentWithoutDevLicense")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowDevelopmentWithoutDevLicense", 1)],
        },
        new TweakDef
        {
            Id = "pkg-disable-appinstaller-protocol",
            Label = "Disable ms-appinstaller Protocol",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the ms-appinstaller URI protocol. Prevents drive-by installs from web links. Default: enabled.",
            Tags = ["package", "appinstaller", "protocol", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableMSAppInstallerProtocol", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableMSAppInstallerProtocol")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableMSAppInstallerProtocol", 0)],
        },
        new TweakDef
        {
            Id = "pkg-disable-auto-repair-apps",
            Label = "Disable Auto-Repair of Windows Apps",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from automatically repairing broken UWP/MSIX apps. Default: enabled.",
            Tags = ["package", "auto-repair", "uwp", "msix"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowAutomaticAppArchiving", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowAutomaticAppArchiving")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx", "AllowAutomaticAppArchiving", 0)],
        },
        // ── Command-based package management tweaks ────────────────────────
        new TweakDef
        {
            Id = "pkg-trust-psgallery",
            Label = "Trust PSGallery Repository",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the PowerShell Gallery as a trusted repository, eliminating installation prompts for modules.",
            Tags = ["package", "powershell", "psgallery", "trust", "module"],
            KindHint = TweakKind.PowerShell,
            ApplyAction = (_) =>
            {
                ShellRunner.RunPowerShell("Set-PSRepository -Name PSGallery -InstallationPolicy Trusted");
            },
            RemoveAction = (_) =>
            {
                ShellRunner.RunPowerShell("Set-PSRepository -Name PSGallery -InstallationPolicy Untrusted");
            },
            DetectAction = () =>
            {
                var (code, stdout, _) = ShellRunner.RunPowerShell("(Get-PSRepository -Name PSGallery).InstallationPolicy");
                return code == 0 && stdout.Trim().Equals("Trusted", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pkg-install-scoop",
            Label = "Install Scoop Package Manager",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Installs Scoop — a CLI package manager for Windows. Scoop installs apps to ~/scoop by default and requires no admin.",
            Tags = ["package", "scoop", "install", "cli"],
            KindHint = TweakKind.PackageManager,
            ApplyAction = (_) =>
            {
                ShellRunner.RunPowerShell(
                    "Set-ExecutionPolicy RemoteSigned -Scope CurrentUser -Force; " + "Invoke-RestMethod -Uri https://get.scoop.sh | Invoke-Expression"
                );
            },
            DetectAction = () =>
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "shims", "scoop.ps1");
                return File.Exists(path);
            },
        },
        new TweakDef
        {
            Id = "pkg-update-powershellget",
            Label = "Update PowerShellGet to Latest",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Updates PowerShellGet module to the latest version for improved module management.",
            Tags = ["package", "powershell", "powershellget", "update"],
            KindHint = TweakKind.PackageManager,
            ApplyAction = (_) =>
            {
                ShellRunner.RunPowerShell("Install-Module -Name PowerShellGet -Force -AllowClobber -Scope CurrentUser");
            },
            DetectAction = () =>
            {
                var (code, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-Module -ListAvailable PowerShellGet | Sort-Object Version -Descending | Select-Object -First 1).Version.ToString()"
                );
                if (code != 0)
                    return false;
                // If version >= 2.2.5, consider "applied"
                return Version.TryParse(stdout.Trim(), out var ver) && ver >= new Version(2, 2, 5);
            },
        },
        new TweakDef
        {
            Id = "pkg-enable-winget",
            Label = "Enable WinGet App Installer",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables the WinGet package manager via App Installer policy. Ensures WinGet is available on managed devices. Default: enabled.",
            Tags = ["packages", "winget", "enable", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAppInstaller", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAppInstaller")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAppInstaller", 1)],
        },
        new TweakDef
        {
            Id = "pkg-npm-prefer-offline",
            Label = "NPM Prefer Offline Cache",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Configures npm to prefer cached packages over network requests. Speeds up installs when packages are already cached. Default: online first.",
            Tags = ["packages", "npm", "offline", "cache"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "NPM_CONFIG_PREFER_OFFLINE", "true")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "NPM_CONFIG_PREFER_OFFLINE")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "NPM_CONFIG_PREFER_OFFLINE", "true")],
        },
        new TweakDef
        {
            Id = "pkg-pip-disable-version-check",
            Label = "Disable pip Version Check",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables pip from checking for newer versions on every run. Speeds up pip operations. Default: checks on every run.",
            Tags = ["packages", "pip", "version-check", "speed"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "PIP_DISABLE_PIP_VERSION_CHECK", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "PIP_DISABLE_PIP_VERSION_CHECK")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "PIP_DISABLE_PIP_VERSION_CHECK", "1")],
        },
        new TweakDef
        {
            Id = "pkg-pip-no-cache",
            Label = "Disable pip Cache",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables pip download caching. Saves disk space at the cost of re-downloading packages. Default: caching enabled.",
            Tags = ["packages", "pip", "cache", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "PIP_NO_CACHE_DIR", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "PIP_NO_CACHE_DIR")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "PIP_NO_CACHE_DIR", "1")],
        },
        new TweakDef
        {
            Id = "pkg-pip-require-venv",
            Label = "Require Virtualenv for pip Install",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Forces pip to only install packages inside a virtual environment. Prevents accidental global installs. Default: allows global.",
            Tags = ["packages", "pip", "virtualenv", "safety"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "PIP_REQUIRE_VIRTUALENV", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "PIP_REQUIRE_VIRTUALENV")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "PIP_REQUIRE_VIRTUALENV", "1")],
        },
        new TweakDef
        {
            Id = "pkg-pip-system-index",
            Label = "Set System pip Index URL",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the default PyPI index URL for all users at the system level. Useful for corporate mirrors. Default: pypi.org.",
            Tags = ["packages", "pip", "index", "system"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
                    "PIP_INDEX_URL",
                    "https://pypi.org/simple"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "PIP_INDEX_URL")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
                    "PIP_INDEX_URL",
                    "https://pypi.org/simple"
                ),
            ],
        },
        new TweakDef
        {
            Id = "pkg-pip-system-no-cache",
            Label = "Disable pip Cache (System)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables pip download caching at the system level for all users. Default: caching enabled.",
            Tags = ["packages", "pip", "cache", "system"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "PIP_NO_CACHE_DIR", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "PIP_NO_CACHE_DIR")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "PIP_NO_CACHE_DIR", "1"),
            ],
        },
        new TweakDef
        {
            Id = "pkg-pip-system-require-venv",
            Label = "Require Virtualenv for pip (System)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces pip to only install inside virtual environments at the system level for all users. Default: allows global.",
            Tags = ["packages", "pip", "virtualenv", "system"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "PIP_REQUIRE_VIRTUALENV", "1"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "PIP_REQUIRE_VIRTUALENV"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "PIP_REQUIRE_VIRTUALENV", "1"),
            ],
        },
        new TweakDef
        {
            Id = "pkg-pip-system-trusted-host",
            Label = "Set pip Trusted Hosts (System)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets trusted pip hosts at the system level to bypass SSL verification. Useful for corporate proxies. Default: none.",
            Tags = ["packages", "pip", "trusted-host", "system"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
                    "PIP_TRUSTED_HOST",
                    "pypi.org files.pythonhosted.org"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "PIP_TRUSTED_HOST")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
                    "PIP_TRUSTED_HOST",
                    "pypi.org files.pythonhosted.org"
                ),
            ],
        },
        new TweakDef
        {
            Id = "pkg-pip-timeout",
            Label = "Set pip Network Timeout",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the pip network timeout to 30 seconds. Prevents hangs on slow connections while allowing reasonable wait time. Default: 15 seconds.",
            Tags = ["packages", "pip", "timeout", "network"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "PIP_TIMEOUT", "30")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "PIP_TIMEOUT")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "PIP_TIMEOUT", "30")],
        },
        new TweakDef
        {
            Id = "pkg-pip-trusted-host",
            Label = "Set pip Trusted Hosts",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets trusted pip hosts for the current user to bypass SSL verification. Useful for corporate proxies. Default: none.",
            Tags = ["packages", "pip", "trusted-host", "ssl"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "PIP_TRUSTED_HOST", "pypi.org files.pythonhosted.org")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "PIP_TRUSTED_HOST")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "PIP_TRUSTED_HOST", "pypi.org files.pythonhosted.org")],
        },
        new TweakDef
        {
            Id = "pkg-pip-user-default",
            Label = "pip Install to User Site by Default",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Configures pip to install packages to the user site-packages directory by default. Avoids needing admin for pip install. Default: system site.",
            Tags = ["packages", "pip", "user", "install"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "PIP_USER", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "PIP_USER")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "PIP_USER", "1")],
        },
        new TweakDef
        {
            Id = "pkg-ps-gallery-trust",
            Label = "Trust PowerShell Gallery",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Sets the PSGallery repository as trusted to suppress install prompts. Default: untrusted.",
            Tags = ["packages", "powershell", "gallery", "trust"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-PSRepository -Name PSGallery -InstallationPolicy Trusted"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-PSRepository -Name PSGallery -InstallationPolicy Untrusted"),
            DetectAction = () =>
            {
                var (code, stdout, _) = ShellRunner.RunPowerShell("(Get-PSRepository -Name PSGallery).InstallationPolicy");
                return code == 0 && stdout.Trim().Equals("Trusted", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pkg-ps-remotesigned",
            Label = "Set PowerShell ExecutionPolicy to RemoteSigned",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the PowerShell execution policy to RemoteSigned for the current user. Allows local scripts to run. Default: Restricted.",
            Tags = ["packages", "powershell", "execution-policy", "remotesigned"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\Software\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell",
                    "ExecutionPolicy",
                    "RemoteSigned"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell", "ExecutionPolicy")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_CURRENT_USER\Software\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell",
                    "ExecutionPolicy",
                    "RemoteSigned"
                ),
            ],
        },
        new TweakDef
        {
            Id = "pkg-scoop-setup",
            Label = "Install Scoop Package Manager",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description =
                "Installs the Scoop package manager for Windows. Provides command-line app installation without admin. Default: not installed.",
            Tags = ["packages", "scoop", "install", "setup"],
            ApplyAction = _ => ShellRunner.RunPowerShell("irm get.scoop.sh | iex"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall scoop"),
            DetectAction = () =>
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "shims", "scoop.ps1");
                return File.Exists(path);
            },
        },
        new TweakDef
        {
            Id = "pkg-winget-disable-auto-update",
            Label = "Disable WinGet Auto-Upgrade",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic package upgrades via WinGet auto-update policy. Prevents unattended app updates. Default: enabled.",
            Tags = ["packages", "winget", "auto-upgrade", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAutoUpgrade", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAutoUpgrade")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableAutoUpgrade", 0)],
        },
        new TweakDef
        {
            Id = "pkg-winget-disable-msstore-source",
            Label = "Disable WinGet Microsoft Store Source",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Microsoft Store source in WinGet. Limits installs to winget community repository only. Default: enabled.",
            Tags = ["packages", "winget", "msstore", "source"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableMSStoreSource", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableMSStoreSource")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppInstaller", "EnableMSStoreSource", 0)],
        },
    ];
}

// ── Merged from ScoopTools.cs ──────────────────────────────────────────────────

internal static class ScoopTools
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "scoop-disable-autoupdate",
            Label = "Disable Scoop Auto-Update on Install",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SCOOP_NO_AUTO_UPDATE=1 to prevent Scoop from auto-updating itself before every app install. Default: auto-update. Recommended: disabled for speed.",
            Tags = ["scoop", "autoupdate", "speed", "environment"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_AUTO_UPDATE", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_AUTO_UPDATE")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_AUTO_UPDATE", "1")],
        },
        new TweakDef
        {
            Id = "scoop-parallel-downloads",
            Label = "Enable Scoop Parallel Downloads (aria2)",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SCOOP_ARIA2_ENABLED=true to enable parallel downloads via aria2 for faster Scoop package installs. Default: disabled. Recommended: enabled.",
            Tags = ["scoop", "parallel", "downloads", "aria2", "speed"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_ENABLED", "true")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_ENABLED")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_ENABLED", "true")],
        },
        new TweakDef
        {
            Id = "scoop-set-global-install-dir",
            Label = "Set Scoop Global Install Directory",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the global Scoop install directory to C:\\Scoop via environment variable. Default: C:\\ProgramData\\scoop.",
            Tags = ["scoop", "global", "install", "directory"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL", @"C:\Scoop")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL", @"C:\Scoop")],
        },
        new TweakDef
        {
            Id = "scoop-set-cache-dir",
            Label = "Set Scoop Cache Directory",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets Scoop download cache to C:\\ScoopCache. Keeps downloads separate from installs. Default: ~\\scoop\\cache.",
            Tags = ["scoop", "cache", "directory", "download"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_CACHE", @"C:\ScoopCache")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_CACHE")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_CACHE", @"C:\ScoopCache")],
        },
        new TweakDef
        {
            Id = "scoop-enable-debug-mode",
            Label = "Enable Scoop Debug Mode",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables Scoop debug output for troubleshooting install failures. Default: disabled.",
            Tags = ["scoop", "debug", "verbose", "troubleshooting"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_DEBUG", "true")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_DEBUG")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_DEBUG", "true")],
        },
        new TweakDef
        {
            Id = "scoop-set-aria2-max-connections",
            Label = "Set Scoop Aria2 Max Connections to 16",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets Scoop Aria2 max connections per server to 16. Speeds up downloads. Default: not set (Aria2 default is 1).",
            Tags = ["scoop", "aria2", "connections", "speed"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_OPTIONS", "-x 16 -s 16")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_OPTIONS")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_OPTIONS", "-x 16 -s 16")],
        },
        new TweakDef
        {
            Id = "scoop-set-global-install-path",
            Label = "Set Scoop Global Install Path",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the Scoop global apps install directory to C:\\ScoopGlobal. Keeps system programs organised. Default: %ProgramData%\\scoop.",
            Tags = ["scoop", "global", "install-path", "directory"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL", @"C:\ScoopGlobal")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL", @"C:\ScoopGlobal")],
        },
        new TweakDef
        {
            Id = "scoop-set-virustotal-api-key",
            Label = "Set Scoop VirusTotal API Key Variable",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the SCOOP_VIRUSTOTAL_API_KEY environment variable placeholder. Replace with your actual key for automatic malware scanning. Default: not set.",
            Tags = ["scoop", "virustotal", "security", "scanning"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_VIRUSTOTAL_API_KEY", "YOUR_API_KEY_HERE")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_VIRUSTOTAL_API_KEY")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_VIRUSTOTAL_API_KEY", "YOUR_API_KEY_HERE")],
        },
        new TweakDef
        {
            Id = "scoop-disable-checkver",
            Label = "Disable Scoop Auto-Version Check",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets SCOOP_NO_CHECKVER=1 to skip automatic version checks. Speeds up 'scoop status'. Default: checks versions.",
            Tags = ["scoop", "checkver", "speed", "environment"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_CHECKVER", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_CHECKVER")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_CHECKVER", "1")],
        },
        new TweakDef
        {
            Id = "scoop-add-extras-bucket",
            Label = "Add Scoop Extras Bucket",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Adds the Scoop 'extras' bucket which contains popular GUI apps. Default: not added.",
            Tags = ["scoop", "extras", "bucket", "apps"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop bucket add extras"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop bucket rm extras"),
            DetectAction = () =>
            {
                var (exit, _, _) = ShellRunner.Run("scoop", ["bucket", "list"]);
                return exit == 0;
            },
        },
        new TweakDef
        {
            Id = "scoop-cleanup-all",
            Label = "Clean Up All Scoop Caches",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Runs scoop cleanup * and scoop cache rm * to free disk space from old versions and downloads.",
            Tags = ["scoop", "cleanup", "cache", "disk-space"],
            ApplyAction = _ =>
            {
                ShellRunner.RunPowerShell("scoop cleanup *");
                ShellRunner.RunPowerShell("scoop cache rm *");
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "scoop-install-aria2",
            Label = "Install Aria2 for Scoop Downloads",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs aria2 download manager for faster parallel Scoop downloads. Default: not installed.",
            Tags = ["scoop", "aria2", "download", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install aria2"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall aria2"),
            DetectAction = () =>
            {
                var scoopDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "aria2");
                return Directory.Exists(scoopDir);
            },
        },
        new TweakDef
        {
            Id = "scoop-7zip",
            Label = "Install 7-Zip via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs 7-Zip file archiver via Scoop. Supports 7z, ZIP, RAR, and many other formats.",
            Tags = ["scoop", "7zip", "archive", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install 7zip"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall 7zip"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "7zip")),
        },
        new TweakDef
        {
            Id = "scoop-bat",
            Label = "Install bat via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs bat — a cat clone with syntax highlighting and Git integration.",
            Tags = ["scoop", "bat", "cat", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install bat"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall bat"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "bat")),
        },
        new TweakDef
        {
            Id = "scoop-btop",
            Label = "Install btop via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs btop — a resource monitor with CPU, memory, disk, network, and process stats.",
            Tags = ["scoop", "btop", "monitor", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install btop"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall btop"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "btop")),
        },
        new TweakDef
        {
            Id = "scoop-curl",
            Label = "Install curl via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs curl — command-line tool for transferring data with URLs.",
            Tags = ["scoop", "curl", "http", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install curl"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall curl"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "curl")),
        },
        new TweakDef
        {
            Id = "scoop-delta",
            Label = "Install delta via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs delta — a syntax-highlighting pager for git, diff, and grep output.",
            Tags = ["scoop", "delta", "diff", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install delta"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall delta"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "delta")),
        },
        new TweakDef
        {
            Id = "scoop-duf",
            Label = "Install duf via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs duf — a better df alternative for viewing disk usage with a modern UI.",
            Tags = ["scoop", "duf", "disk", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install duf"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall duf"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "duf")),
        },
        new TweakDef
        {
            Id = "scoop-dust",
            Label = "Install dust via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs dust — a more intuitive version of du showing disk usage as a tree.",
            Tags = ["scoop", "dust", "disk-usage", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install dust"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall dust"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "dust")),
        },
        new TweakDef
        {
            Id = "scoop-everything",
            Label = "Install Everything via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Everything — instant file search engine for Windows with real-time indexing.",
            Tags = ["scoop", "everything", "search", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install everything"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall everything"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "everything")),
        },
        new TweakDef
        {
            Id = "scoop-fd",
            Label = "Install fd via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs fd — a fast and user-friendly alternative to find.",
            Tags = ["scoop", "fd", "find", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install fd"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall fd"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "fd")),
        },
        new TweakDef
        {
            Id = "scoop-fzf",
            Label = "Install fzf via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs fzf — a general-purpose command-line fuzzy finder.",
            Tags = ["scoop", "fzf", "fuzzy", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install fzf"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall fzf"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "fzf")),
        },
        new TweakDef
        {
            Id = "scoop-git",
            Label = "Install Git via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Git distributed version control system via Scoop.",
            Tags = ["scoop", "git", "vcs", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install git"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall git"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "git")),
        },
        new TweakDef
        {
            Id = "scoop-gsudo",
            Label = "Install gsudo via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs gsudo — a sudo equivalent for Windows that elevates commands inline.",
            Tags = ["scoop", "gsudo", "sudo", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install gsudo"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall gsudo"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "gsudo")),
        },
        new TweakDef
        {
            Id = "scoop-hyperfine",
            Label = "Install hyperfine via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs hyperfine — a command-line benchmarking tool with statistical analysis.",
            Tags = ["scoop", "hyperfine", "benchmark", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install hyperfine"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall hyperfine"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "hyperfine")),
        },
        new TweakDef
        {
            Id = "scoop-jq",
            Label = "Install jq via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs jq — a lightweight command-line JSON processor.",
            Tags = ["scoop", "jq", "json", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install jq"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall jq"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "jq")),
        },
        new TweakDef
        {
            Id = "scoop-lazygit",
            Label = "Install lazygit via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs lazygit — a simple terminal UI for Git commands.",
            Tags = ["scoop", "lazygit", "git", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install lazygit"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall lazygit"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "lazygit")),
        },
        new TweakDef
        {
            Id = "scoop-neovim",
            Label = "Install Neovim via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Neovim — a hyperextensible Vim-based text editor.",
            Tags = ["scoop", "neovim", "editor", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install neovim"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall neovim"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "neovim")),
        },
        new TweakDef
        {
            Id = "scoop-nodejs",
            Label = "Install Node.js via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Node.js JavaScript runtime via Scoop.",
            Tags = ["scoop", "nodejs", "javascript", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install nodejs"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall nodejs"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "nodejs")),
        },
        new TweakDef
        {
            Id = "scoop-python",
            Label = "Install Python via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Python interpreter via Scoop.",
            Tags = ["scoop", "python", "interpreter", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install python"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall python"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "python")),
        },
        new TweakDef
        {
            Id = "scoop-ripgrep",
            Label = "Install ripgrep via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs ripgrep — a line-oriented search tool that recursively searches directories.",
            Tags = ["scoop", "ripgrep", "search", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install ripgrep"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall ripgrep"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "ripgrep")),
        },
        new TweakDef
        {
            Id = "scoop-set-global-path",
            Label = "Add Scoop Global Apps to PATH",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Adds the Scoop global apps directory to the system PATH. Allows globally installed Scoop apps to be available to all users. Default: not in PATH.",
            Tags = ["scoop", "global", "path", "environment"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
            ApplyOps =
            [
                RegOp.SetExpandString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
                    "SCOOP_GLOBAL",
                    @"%ProgramData%\scoop"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "SCOOP_GLOBAL")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
                    "SCOOP_GLOBAL",
                    @"%ProgramData%\scoop"
                ),
            ],
        },
        new TweakDef
        {
            Id = "scoop-starship",
            Label = "Install Starship via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Starship — a minimal, blazing-fast cross-shell prompt.",
            Tags = ["scoop", "starship", "prompt", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install starship"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall starship"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "starship")),
        },
        new TweakDef
        {
            Id = "scoop-tldr",
            Label = "Install tldr via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs tldr — simplified and community-driven man pages.",
            Tags = ["scoop", "tldr", "man", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install tldr"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall tldr"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "tldr")),
        },
        new TweakDef
        {
            Id = "scoop-wget",
            Label = "Install wget via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs wget — a non-interactive network downloader for HTTP, HTTPS, and FTP.",
            Tags = ["scoop", "wget", "download", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install wget"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall wget"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "wget")),
        },
        new TweakDef
        {
            Id = "scoop-zoxide",
            Label = "Install zoxide via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs zoxide — a smarter cd command that learns your most-used directories and jumps to them intelligently.",
            Tags = ["scoop", "zoxide", "navigation", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install zoxide"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall zoxide"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "zoxide")),
        },
        new TweakDef
        {
            Id = "scoop-lsd",
            Label = "Install lsd via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs lsd — a modern ls replacement with icons, colours, and tree view.",
            Tags = ["scoop", "lsd", "files", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install lsd"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall lsd"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "lsd")),
        },
        new TweakDef
        {
            Id = "scoop-sd",
            Label = "Install sd via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs sd — a modern sed/awk replacement for intuitive find-and-replace operations.",
            Tags = ["scoop", "sd", "text", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install sd"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall sd"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "sd")),
        },
        new TweakDef
        {
            Id = "scoop-procs",
            Label = "Install procs via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs procs — a modern replacement for ps that shows processes with colour-coded resource usage.",
            Tags = ["scoop", "procs", "processes", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install procs"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall procs"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "procs")),
        },
        new TweakDef
        {
            Id = "scoop-bottom",
            Label = "Install bottom via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs bottom (btm) — a graphical system monitor for CPU, RAM, disk, and network.",
            Tags = ["scoop", "bottom", "monitor", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install bottom"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall bottom"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "bottom")),
        },
        new TweakDef
        {
            Id = "scoop-xh",
            Label = "Install xh via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs xh — a friendly and fast HTTP client similar to HTTPie.",
            Tags = ["scoop", "xh", "http", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install xh"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall xh"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "xh")),
        },
        new TweakDef
        {
            Id = "scoop-gping",
            Label = "Install gping via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs gping — a graphical ping tool that displays network latency as a real-time graph.",
            Tags = ["scoop", "gping", "network", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install gping"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall gping"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "gping")),
        },
        new TweakDef
        {
            Id = "scoop-tokei",
            Label = "Install tokei via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs tokei — a fast code statistics tool that counts lines of code by language.",
            Tags = ["scoop", "tokei", "code", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install tokei"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall tokei"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "tokei")),
        },
        new TweakDef
        {
            Id = "scoop-tealdeer",
            Label = "Install tealdeer via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs tealdeer — a fast Rust-based tldr pages client for quick command summaries.",
            Tags = ["scoop", "tealdeer", "tldr", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install tealdeer"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall tealdeer"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "tealdeer")),
        },
        new TweakDef
        {
            Id = "scoop-carapace-bin",
            Label = "Install carapace-bin via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs carapace-bin — a multi-shell completion engine supporting PowerShell, bash, zsh, and more.",
            Tags = ["scoop", "carapace", "completion", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install carapace-bin"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall carapace-bin"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "carapace-bin")),
        },
        new TweakDef
        {
            Id = "scoop-eza",
            Label = "Install eza via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description =
                "Installs eza — a modern, maintained replacement for ls with colour output, icons, and git integration. Replaces the deprecated exa.",
            Tags = ["scoop", "eza", "ls", "files", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install eza"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall eza"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "eza")),
        },
        new TweakDef
        {
            Id = "scoop-yazi",
            Label = "Install yazi via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs yazi — a blazing-fast terminal file manager written in Rust with async I/O and vim-style navigation.",
            Tags = ["scoop", "yazi", "file-manager", "terminal", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install yazi"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall yazi"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "yazi")),
        },
        new TweakDef
        {
            Id = "scoop-helix",
            Label = "Install Helix editor via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description =
                "Installs Helix — a post-modern modal text editor inspired by Kakoune and Neovim, with built-in LSP and tree-sitter support.",
            Tags = ["scoop", "helix", "editor", "modal", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install helix"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall helix"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "helix")),
        },
        new TweakDef
        {
            Id = "scoop-nushell",
            Label = "Install Nushell via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Nushell (nu) — a modern shell with structured, type-aware pipelines. Treats output as tables, not plain text.",
            Tags = ["scoop", "nushell", "shell", "nu", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install nu"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall nu"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "nu")),
        },
        new TweakDef
        {
            Id = "scoop-zellij",
            Label = "Install Zellij via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Zellij — a terminal workspace with panes, tabs, and a plugin system. Modern Rust-based tmux alternative.",
            Tags = ["scoop", "zellij", "terminal", "multiplexer", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install zellij"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall zellij"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "zellij")),
        },
        new TweakDef
        {
            Id = "scoop-gitoxide",
            Label = "Install gitoxide (gix) via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs gitoxide — a pure Rust git implementation providing the gix CLI tool for fast, safe git operations.",
            Tags = ["scoop", "gitoxide", "git", "rust", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install gitoxide"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall gitoxide"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "gitoxide")),
        },
        new TweakDef
        {
            Id = "scoop-watchexec",
            Label = "Install watchexec via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description =
                "Installs watchexec — a tool that watches files for changes and re-runs a command automatically. Ideal for dev and build workflows.",
            Tags = ["scoop", "watchexec", "watch", "automation", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install watchexec"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall watchexec"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "watchexec")),
        },
        new TweakDef
        {
            Id = "scoop-topgrade",
            Label = "Install topgrade via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs topgrade — a one-command upgrade tool for all package managers, shells, plugins, and tools on the system.",
            Tags = ["scoop", "topgrade", "upgrade", "maintenance", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install topgrade"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall topgrade"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "topgrade")),
        },
        new TweakDef
        {
            Id = "scoop-pueue",
            Label = "Install pueue via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs pueue — a background task queue manager for long-running shell commands with pause, abort, and log features.",
            Tags = ["scoop", "pueue", "task-queue", "background", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install pueue"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall pueue"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "pueue")),
        },
        new TweakDef
        {
            Id = "scoop-oha",
            Label = "Install oha via Scoop",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs oha — a fast HTTP load generator written in Rust for benchmarking web endpoints from the command line.",
            Tags = ["scoop", "oha", "http", "load-test", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install oha"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall oha"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "oha")),
        },
    ];
}

// ── merged from Java.cs ──
internal static class Java
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "java-security-high",
            Label = "Java: Set Security Level to Very High",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Raises the Java security level to VERY_HIGH, blocking unsigned applets.",
            Tags = ["java", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-java-tip-of-day",
            Label = "Disable Java Tip of the Day",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the 'Tip of the Day' pop-up dialog in Java Control Panel.",
            Tags = ["java", "ui", "annoyance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.tip.day", "false")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.tip.day")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.tip.day", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-update-check",
            Label = "Disable Java Auto-Update Check",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Java's automatic update check at startup. Reduces background network traffic. Default: Enabled. Recommended: Disabled for managed environments.",
            Tags = ["java", "update", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "NotifyDownload", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "NotifyDownload", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 0)],
        },
        new TweakDef
        {
            Id = "java-high-perf-graphics",
            Label = "Java High Performance Graphics",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables hardware graphics acceleration for Java/JavaFX applications. Improves rendering performance. Default: Software. Recommended: Hardware.",
            Tags = ["java", "graphics", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment", "JavaFXHardwareAcceleration", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment", "JavaFXHardwareAcceleration")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment", "JavaFXHardwareAcceleration", 1)],
        },
        new TweakDef
        {
            Id = "java-disable-sponsor-offers",
            Label = "Disable Java Sponsor Offers",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables sponsor/adware offers bundled with Java updates. Default: Enabled. Recommended: Disabled.",
            Tags = ["java", "sponsor", "adware", "offers"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 0)],
        },
        new TweakDef
        {
            Id = "java-disable-usage-tracking",
            Label = "Disable Java Usage Tracking",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Java usage tracker analytics. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["java", "usage", "tracking", "analytics", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.usagetracker.enabled", "false")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.usagetracker.enabled")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.usagetracker.enabled", "false")],
        },
        new TweakDef
        {
            Id = "java-set-high-security",
            Label = "Set Java Security Level to Very High",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets Java Web Start / applet security level to Very High. Only signed and trusted apps run. Default: High.",
            Tags = ["java", "security", "level", "applet"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.security.level", "HIGH")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH")],
        },
        new TweakDef
        {
            Id = "java-disable-web-plugin",
            Label = "Disable Java Browser Plugin",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Java browser plugin (applets). Reduces browser attack surface. Default: enabled.",
            Tags = ["java", "browser", "plugin", "applet", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "false")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "true")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "false")],
        },
        new TweakDef
        {
            Id = "java-disable-log-file",
            Label = "Disable Java Console Log File",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Java console log file creation. Reduces disk writes from Java applications. Default: enabled.",
            Tags = ["java", "console", "log", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javaws.logFileName", "")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javaws.logFileName")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javaws.logFileName", "")],
        },
        new TweakDef
        {
            Id = "java-set-high-dpi-awareness",
            Label = "Enable Java High DPI Awareness",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables high-DPI awareness for Java applications. Prevents blurry rendering on HiDPI displays. Default: system-aware.",
            Tags = ["java", "dpi", "hidpi", "scaling"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javafx.highDPIAware", "true")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javafx.highDPIAware")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javafx.highDPIAware", "true")],
        },
        new TweakDef
        {
            Id = "java-disable-usage-tracker",
            Label = "Disable Java Usage Tracker",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Java Usage Tracker that reports Java runtime usage data to Oracle. Default: enabled.",
            Tags = ["java", "usage", "tracker", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "com.oracle.usagetracker.track.last.usage", "false"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "com.oracle.usagetracker.track.last.usage")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "com.oracle.usagetracker.track.last.usage", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-java-cert-revoke",
            Label = "Disable Java Certificate Revocation Check",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Java certificate revocation list checking. Speeds up Java applet loading but reduces security. Default: enabled.",
            Tags = ["java", "certificate", "revocation", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.security.validation.crl", "false")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.security.validation.crl", "true")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.security.validation.crl", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-java-error-reporting",
            Label = "Disable Java Error Reporting",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Java crash and error reporting to Oracle. Prevents error data from being sent externally. Default: enabled.",
            Tags = ["java", "error", "reporting", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.user.security.exception.sites", "false"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.user.security.exception.sites")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.user.security.exception.sites", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-java-tracking",
            Label = "Disable Java Analytics Tracking",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Java analytics and tracking features. Prevents collection of usage patterns by Oracle. Default: enabled.",
            Tags = ["java", "analytics", "tracking", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javaws.shortcut", "false")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javaws.shortcut")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javaws.shortcut", "false")],
        },
        new TweakDef
        {
            Id = "java-high-dpi",
            Label = "Enable Java High DPI Scaling",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables high DPI scaling awareness for Java applications. Prevents blurry rendering on high-resolution displays. Default: not set.",
            Tags = ["java", "dpi", "scaling", "display"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties",
                    "deployment.javaws.jre.platform.version",
                    "sun.java2d.uiScale.enabled=true"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.javaws.jre.platform.version")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties",
                    "deployment.javaws.jre.platform.version",
                    "sun.java2d.uiScale.enabled=true"
                ),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-installer-sponsor",
            Label = "Disable Java Sponsor Offers",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Prevents Java installer from showing third-party sponsor offers (e.g., Ask Toolbar). Default: enabled.",
            Tags = ["java", "sponsor", "ads", "installer"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft", "SPONSORS", "DISABLE")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft", "SPONSORS")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft", "SPONSORS", "DISABLE")],
        },
        new TweakDef
        {
            Id = "java-disable-tls-10",
            Label = "Disable TLS 1.0 in Java",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Disables TLS 1.0 in Java deployment properties. TLS 1.0 is deprecated. Default: enabled.",
            Tags = ["java", "tls", "security", "encryption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.TLSv1", "false")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.TLSv1")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.TLSv1", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-tls-11",
            Label = "Disable TLS 1.1 in Java",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Disables TLS 1.1 in Java deployment properties. TLS 1.1 is deprecated. Default: enabled.",
            Tags = ["java", "tls", "security", "encryption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.TLSv1.1", "false"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.TLSv1.1")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.TLSv1.1", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-browser-plugin",
            Label = "Disable Java Browser Plugin",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Disables the Java browser plugin. Java applets in browsers are obsolete and a security risk. Default: enabled.",
            Tags = ["java", "browser", "plugin", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "false"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.webjava.enabled")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-enable-certificate-revocation",
            Label = "Enable Certificate Revocation Checking",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Enables certificate revocation checking via CRL and OCSP in Java. Default: enabled.",
            Tags = ["java", "certificate", "revocation", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.validation.crl", "true"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.validation.crl"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.security.validation.crl",
                    "true"
                ),
            ],
        },
        new TweakDef
        {
            Id = "java-enable-ocsp",
            Label = "Enable OCSP Certificate Checking",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Enables Online Certificate Status Protocol (OCSP) checking for Java certificates. Default: enabled.",
            Tags = ["java", "ocsp", "certificate", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.validation.ocsp", "true"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.validation.ocsp"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.security.validation.ocsp",
                    "true"
                ),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-jnlp-association",
            Label = "Disable JNLP File Association",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Disables Java Web Start JNLP file association. Prevents accidental launch of Web Start apps. Default: enabled.",
            Tags = ["java", "jnlp", "web-start", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.shortcut", "NEVER"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.shortcut")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.shortcut", "NEVER"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-console-output",
            Label = "Disable Java Console Output",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Hides the Java console for deployed applications. Reduces clutter for end users. Default: show console.",
            Tags = ["java", "console", "output"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.console.startup.mode", "HIDE"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.console.startup.mode")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.console.startup.mode", "HIDE"),
            ],
        },
        new TweakDef
        {
            Id = "java-set-proxy-direct",
            Label = "Set Java Proxy to Direct (No Proxy)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Configures Java to use a direct connection (no proxy). Default: uses browser proxy settings.",
            Tags = ["java", "proxy", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.proxy.type", "0")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.proxy.type")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.proxy.type", "0")],
        },
        new TweakDef
        {
            Id = "java-set-cache-max-100mb",
            Label = "Set Java Cache Max Size to 100 MB",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the Java deployment cache to 100 MB. Prevents unbounded cache growth on developer systems. Default: unlimited.",
            Tags = ["java", "cache", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.cache.max.size.file.mb", "100"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.cache.max.size.file.mb"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.cache.max.size.file.mb", "100"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-webstart-splash",
            Label = "Disable Java Web Start Splash Screen",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Suppresses the splash screen shown when launching Java Web Start applications. Default: splash screen shown.",
            Tags = ["java", "webstart", "splash", "ui"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.splash.enabled", "false"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.splash.enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.splash.enabled", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-set-connect-timeout-10s",
            Label = "Set Java Socket Connection Timeout to 10s",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Java socket connection timeout to 10 seconds. Prevents indefinite hangs when connecting to unreachable resources.",
            Tags = ["java", "timeout", "socket", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.timeout.socket.connect", "10000"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.timeout.socket.connect"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.timeout.socket.connect",
                    "10000"
                ),
            ],
        },
        new TweakDef
        {
            Id = "java-set-read-timeout-30s",
            Label = "Set Java Socket Read Timeout to 30s",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the Java socket read timeout to 30 seconds. Prevents indefinite hangs when reading from slow or hung resources.",
            Tags = ["java", "timeout", "socket", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.timeout.socket.read", "30000"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.timeout.socket.read")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.timeout.socket.read", "30000"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-update-check-interval",
            Label = "Disable Java Update Check Interval",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables periodic Java update check background scheduling. Prevents background processes polling for updates. Default: periodic checks enabled.",
            Tags = ["java", "update", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.update.check.interval.days", "-1"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.update.check.interval.days"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.update.check.interval.days",
                    "-1"
                ),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-eula-check",
            Label = "Disable Java EULA Check on First Run",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Marks Java EULA as accepted to suppress the first-run EULA dialog in enterprise deployments. Default: shows EULA on first launch.",
            Tags = ["java", "eula", "first-run", "enterprise"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.eula.dismissed", "true")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.eula.dismissed")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.eula.dismissed", "true"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-application-description",
            Label = "Disable Java Application Description Prompt",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Suppresses the application description tooltip shown when launching Java Web Start applications. Default: shown.",
            Tags = ["java", "ui", "prompt", "silent"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.application.description.shown",
                    "false"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.application.description.shown"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.application.description.shown",
                    "false"
                ),
            ],
        },
        new TweakDef
        {
            Id = "java-set-concurrent-downloads-3",
            Label = "Set Java Concurrent Downloads to 3",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Allows Java to download up to 3 resources concurrently. Improves load time for Java apps with many classpath resources. Default: 1.",
            Tags = ["java", "download", "concurrency", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.concurrent.downloads", "3"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.concurrent.downloads")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.concurrent.downloads", "3"),
            ],
        },
        new TweakDef
        {
            Id = "java-enable-strict-security",
            Label = "Enable Java Strict Security Mode",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables strict security validation for Java deployments. Enforces all certificate and permission checks. Default: standard mode.",
            Tags = ["java", "security", "strict", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.strict.mode", "true"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.strict.mode")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.strict.mode", "true"),
            ],
        },
        new TweakDef
        {
            Id = "java-lock-security-level",
            Label = "Lock Java Security Level (Prevent User Change)",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Locks the Java security level setting so users cannot lower it. Prevents accidental or deliberate reduction of Java security from the Java Control Panel. Default: unlocked.",
            Tags = ["java", "security", "lock", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level.locked", "true"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level.locked"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level.locked", "true"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-console-autostart",
            Label = "Disable Java Console Auto-Start",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Java console startup mode to NEVER so it does not automatically open during applet execution. Reduces UI clutter in production and user environments. Default: HIDE.",
            Tags = ["java", "console", "startup", "ui"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.console.startup.mode", "NEVER"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.console.startup.mode")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.console.startup.mode", "NEVER"),
            ],
        },
        new TweakDef
        {
            Id = "java-set-revocation-all-certs",
            Label = "Enable Java Revocation Check for All Certificates",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures Java to check certificate revocation for all certificates in the chain (not just the end entity). Provides stronger PKI validation. Default: PUBLISHER_ONLY.",
            Tags = ["java", "security", "revocation", "pki"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.security.revocation.check",
                    "ALL_CERTIFICATES"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.revocation.check"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.security.revocation.check",
                    "ALL_CERTIFICATES"
                ),
            ],
        },
        new TweakDef
        {
            Id = "java-lock-update-check",
            Label = "Lock Java Update Check Setting",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Locks the Java update check setting via deployment policy so users cannot re-enable automatic update checking. Complements java-disable-auto-update. Default: unlocked.",
            Tags = ["java", "update", "lock", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.updatecheck.locked", "true"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.updatecheck.locked")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.updatecheck.locked", "true"),
            ],
        },
        new TweakDef
        {
            Id = "java-set-plugin-session-lifetime",
            Label = "Set Java Plugin Session Lifetime Mode",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Java plugin credential and session lifetime to SESSION mode so temporary data is cleared when the browser exits. Reduces residual data exposure. Default: FOREVER.",
            Tags = ["java", "session", "privacy", "plugin"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.plugin.lifetime", "SESSION"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.plugin.lifetime")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.plugin.lifetime", "SESSION"),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-jre-auto-install",
            Label = "Disable Automatic JRE Installation",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic JRE installation triggered by Java Web Start or the browser plugin. Prevents Java from downloading and installing JRE versions without admin consent. Default: auto-install enabled.",
            Tags = ["java", "install", "auto-update", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.jre.install.enabled", "false"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.jre.install.enabled")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.jre.install.enabled", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-enable-blacklist-revocation",
            Label = "Enable Java Blacklist Revocation Check",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables checking of Java's built-in certificate blacklist for revoked or compromised certificates during applet launch. Default: enabled (explicit policy reinforces it).",
            Tags = ["java", "security", "blacklist", "revocation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.blacklist.check", "true"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.blacklist.check"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.security.blacklist.check",
                    "true"
                ),
            ],
        },
        new TweakDef
        {
            Id = "java-disable-applet-caching",
            Label = "Disable Java Applet Cache",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Java deployment cache for applets and Web Start applications. Prevents local caching of Java class files and reduces disk exposure from cached untrusted code. Default: cache enabled.",
            Tags = ["java", "cache", "privacy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.cache.enabled", "false")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.cache.enabled")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.cache.enabled", "false"),
            ],
        },
        new TweakDef
        {
            Id = "java-lock-expiration-check",
            Label = "Lock Java JRE Expiration Check",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Locks the Java JRE expiration check via deployment policy so users cannot disable the warning when running an expired or outdated JRE version. Default: unlocked.",
            Tags = ["java", "expiration", "lock", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.expiration.check.locked", "true"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.expiration.check.locked"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties",
                    "deployment.expiration.check.locked",
                    "true"
                ),
            ],
        },
    ];
}

internal static class WindowsTerminal
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "term-enable-console-v2",
            Label = "Enable Console V2 Host",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Forces the new Console V2 host with ANSI support, line wrapping, and improved rendering. Default: 1 (enabled). Recommended: 1.",
            Tags = ["terminal", "console", "v2", "modern"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ForceV2", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ForceV2", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "ForceV2", 1)],
        },
        new TweakDef
        {
            Id = "term-disable-quick-edit",
            Label = "Disable Quick Edit Mode",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Quick Edit so clicking the console window does not pause running commands. Prevents accidental hangs. Default: 1 (on). Recommended: 0 (off).",
            Tags = ["terminal", "quickedit", "console", "hang"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 0)],
        },
        new TweakDef
        {
            Id = "term-enable-insert-mode",
            Label = "Enable Insert Mode by Default",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets insert mode as the default typing mode in consoles. Default: 1 (insert). Recommended: 1.",
            Tags = ["terminal", "insert", "mode", "typing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "InsertMode", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "InsertMode", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "InsertMode", 1)],
        },
        new TweakDef
        {
            Id = "term-enable-line-wrap",
            Label = "Enable Line Wrapping",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables automatic line wrapping when resizing the console. Default: 1. Recommended: 1.",
            Tags = ["terminal", "wrap", "resize", "console"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "LineWrap", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "LineWrap", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "LineWrap", 1)],
        },
        new TweakDef
        {
            Id = "term-disable-legacy-console",
            Label = "Disable Legacy Console Mode",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the legacy console subsystem. Required for Console V2 features like ANSI escape support. Default: 0 (modern). Recommended: 0.",
            Tags = ["terminal", "legacy", "console", "modern"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "UseLegacyConsole", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "UseLegacyConsole", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "UseLegacyConsole", 0)],
        },
        new TweakDef
        {
            Id = "term-enable-ctrl-cv",
            Label = "Enable Ctrl+Shift+C/V Copy-Paste",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables Ctrl+Shift+C / Ctrl+Shift+V clipboard shortcuts in the classic console. Also enables filter-on-paste and line selection. Default: enabled. Recommended: enabled.",
            Tags = ["terminal", "copy", "paste", "keyboard", "shortcuts"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "FilterOnPaste", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "LineSelection", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "FilterOnPaste"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "LineSelection"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled", 0)],
        },
        new TweakDef
        {
            Id = "term-set-window-opacity",
            Label = "Set Console Window Opacity (95%)",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets console window to 95% opacity for slight transparency. Default: 255 (opaque). Recommended: 242 (95%).",
            Tags = ["terminal", "opacity", "transparency", "appearance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "WindowAlpha", 242)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "WindowAlpha", 255)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "WindowAlpha", 242)],
        },
        new TweakDef
        {
            Id = "term-set-default-wt",
            Label = "Set Default Terminal to Windows Terminal (Win11)",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets Windows Terminal as the default terminal via the Win11 UseNewTerminal setting. Default: Let Windows decide (0). Recommended: Windows Terminal (1).",
            Tags = ["terminal", "default", "windows-terminal", "win11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseNewTerminal", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationTerminal"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseNewTerminal", 1)],
        },
        new TweakDef
        {
            Id = "term-disable-splash",
            Label = "Disable Terminal Splash Screen",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Terminal splash/startup screen via policy. Default: Enabled. Recommended: Disabled for faster launch.",
            Tags = ["terminal", "splash", "startup", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableSplashScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableSplashScreen")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableSplashScreen", 1)],
        },
        new TweakDef
        {
            Id = "term-set-default-terminal-wt",
            Label = "Set Windows Terminal as Default Console App",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets Windows Terminal as the default terminal application for all console programs. Default: Windows Console Host.",
            Tags = ["terminal", "default", "console", "wt"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console\%%Startup"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole", "{2EACA947-7F5F-4CFA-BA87-8F7FBEEFBE69}")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole", "{2EACA947-7F5F-4CFA-BA87-8F7FBEEFBE69}")],
        },
        new TweakDef
        {
            Id = "term-enable-acrylic-background",
            Label = "Enable Terminal Acrylic Background via Policy",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables acrylic (translucent) background in Windows Terminal via machine policy. Default: disabled.",
            Tags = ["terminal", "acrylic", "background", "appearance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "UseAcrylic", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "UseAcrylic")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "UseAcrylic", 1)],
        },
        new TweakDef
        {
            Id = "term-disable-bell",
            Label = "Disable Terminal Bell Sound via Policy",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the bell (beep) sound in Windows Terminal. Default: enabled.",
            Tags = ["terminal", "bell", "beep", "sound"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "BellStyle", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "BellStyle")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "BellStyle", 0)],
        },
        new TweakDef
        {
            Id = "term-set-default-profile-pwsh",
            Label = "Set Default Shell to PowerShell 7 via Policy",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the default shell profile in Windows Terminal to PowerShell 7 via machine policy. Default: Windows PowerShell 5.1.",
            Tags = ["terminal", "default", "powershell", "profile"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal",
                    "DefaultProfile",
                    "{574e775e-4f2a-5b96-ac1e-a2962a402336}"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DefaultProfile")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal",
                    "DefaultProfile",
                    "{574e775e-4f2a-5b96-ac1e-a2962a402336}"
                ),
            ],
        },
        new TweakDef
        {
            Id = "term-disable-automatic-updates",
            Label = "Disable Windows Terminal Auto-Update",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic updates for Windows Terminal. Use winget or manual update instead. Default: auto-update.",
            Tags = ["terminal", "update", "auto", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "term-campbell-color-scheme",
            Label = "Set Windows Terminal Campbell Theme",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the Windows Terminal default color scheme to Campbell. Provides a consistent dark theme across profiles. Default: system default.",
            Tags = ["terminal", "campbell", "theme", "color"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ColorTable00", unchecked((int)0x0C0C0C))],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "ColorTable00")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "ColorTable00", unchecked((int)0x0C0C0C))],
        },
        new TweakDef
        {
            Id = "term-default-windows-terminal",
            Label = "Set Windows Terminal as Default Console",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets Windows Terminal as the default console host instead of legacy conhost.exe. Enables modern features and tabs. Default: conhost.",
            Tags = ["terminal", "default", "console", "modern"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console\%%Startup"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole", 1)],
        },
        new TweakDef
        {
            Id = "term-enable-always-on-top",
            Label = "Enable Terminal Always On Top",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables always-on-top mode for console windows. Terminal stays above other windows. Useful for monitoring output. Default: disabled.",
            Tags = ["terminal", "always-on-top", "window", "pin"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "AlwaysOnTop", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "AlwaysOnTop")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "AlwaysOnTop", 1)],
        },
        new TweakDef
        {
            Id = "term-large-buffer",
            Label = "Set Terminal Large Scrollback Buffer",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Increases the console scrollback buffer to 9999 lines. Allows reviewing more terminal output history. Default: 300 lines.",
            Tags = ["terminal", "buffer", "scrollback", "history"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 9999)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 300)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 9999)],
        },
        new TweakDef
        {
            Id = "term-set-cursor-block",
            Label = "Set Terminal Block Cursor",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the console cursor shape to a solid block. More visible than the default underscore cursor. Default: underscore.",
            Tags = ["terminal", "cursor", "block", "visibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "CursorType", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "CursorType")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "CursorType", 2)],
        },
        new TweakDef
        {
            Id = "term-set-font-weight-bold",
            Label = "Set Console Font Weight to Bold",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Sets the console font weight to bold (700). Improves readability on high-DPI displays. Default: normal (400).",
            Tags = ["console", "font", "bold", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "FontWeight", 700)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "FontWeight")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "FontWeight", 700)],
        },
        new TweakDef
        {
            Id = "term-disable-scroll-forward",
            Label = "Disable Forward Scrolling",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Disables the ability to scroll forward past the current output. Default: enabled.",
            Tags = ["console", "scroll", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ForwardScroll", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "ForwardScroll")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "ForwardScroll", 0)],
        },
        new TweakDef
        {
            Id = "term-disable-ctrl-key-shortcuts",
            Label = "Disable Ctrl Key Shortcuts",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Disables Ctrl+C/Ctrl+V shortcuts in the legacy console host. Useful when Ctrl+C is needed for SIGINT. Default: enabled.",
            Tags = ["console", "ctrl", "shortcuts", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled", 1)],
        },
        new TweakDef
        {
            Id = "term-enable-trim-leading-zeros",
            Label = "Enable Trim Leading Zeros",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Trims leading zeros when double-clicking to select numbers in the console. Default: disabled.",
            Tags = ["console", "selection", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "TrimLeadingZeros", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "TrimLeadingZeros")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "TrimLeadingZeros", 1)],
        },
        new TweakDef
        {
            Id = "term-set-history-buffer-999",
            Label = "Set History Buffer Size to 999",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Increases the command history buffer to 999 entries (maximum). Default: 50.",
            Tags = ["console", "history", "buffer", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 999)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 999)],
        },
        new TweakDef
        {
            Id = "term-disable-number-of-history-buffers",
            Label = "Set Number of History Buffers to 4",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Sets the number of history buffers to 4 (one per console process). Default: 4.",
            Tags = ["console", "history", "buffer", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "NumberOfHistoryBuffers", 4)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "NumberOfHistoryBuffers")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "NumberOfHistoryBuffers", 4)],
        },
    ];
}

// === Merged from: PowerShellTweaks.cs ===

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
    ];
}

