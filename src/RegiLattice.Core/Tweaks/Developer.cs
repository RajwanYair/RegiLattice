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
            Id = "dev-disable-jit-debugger",
            Label = "Disable JIT Debugger Auto-Attach on Crash",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Auto=0 in AeDebug. Prevents Windows from automatically launching a just-in-time debugger attachment prompt when a process crashes, allowing crash-loop testing to proceed unattended.",
            Tags = ["developer", "debugger", "jit", "crash"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug", "Auto", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug", "Auto", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug", "Auto", "0")],
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
            Id = "dev-enable-utf8-codepage",
            Label = "Enable UTF-8 System Codepage",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the system default codepage to UTF-8 (65001). Improves compatibility with international text. Default: system locale.",
            Tags = ["dev", "utf8", "codepage", "unicode"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage", "ACP", "65001"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage", "OEMCP", "65001"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage", "ACP", "1252"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage", "OEMCP", "437"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage", "ACP", "65001")],
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
            Id = "dev-enable-console-ansi",
            Label = "Enable ANSI VT100 Colour in Console (Dev)",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables Virtual Terminal Level processing in the Windows console so ANSI escape codes (colours, cursor movement) work in cmd.exe and PowerShell. Required for colour-aware CLI tools. Default: disabled in cmd.exe.",
            Tags = ["dev", "console", "ansi", "colour", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel", 1)],
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
            Id = "vscode-disable-telemetry",
            Label = "Disable VS Code Telemetry via Policy",
            Category = "Developer",
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
            Id = "vscode-disable-update-notification",
            Label = "Disable VS Code Update Notifications",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents VS Code from showing update notifications via policy.",
            Tags = ["vscode", "update", "notification", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.showReleaseNotes", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.showReleaseNotes")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.showReleaseNotes", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-vscode-experiments",
            Label = "Disable VS Code A/B Experiments",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Microsoft A/B experiment features in VS Code via policy.",
            Tags = ["vscode", "experiments", "ab-testing", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.enableExperiments", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.enableExperiments")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.enableExperiments", 0)],
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
            Id = "vscode-disable-vscode-recommendations",
            Label = "Disable VS Code Extension Suggestions",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables extension install recommendations and suggestions in VS Code via policy.",
            Tags = ["vscode", "recommendations", "suggestions", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.showRecommendationsOnlyOnDemand", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.showRecommendationsOnlyOnDemand")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.showRecommendationsOnlyOnDemand", 1)],
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
            Id = "vscode-disable-vscode-update",
            Label = "Disable VS Code Auto-Update",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic updates in VS Code via Group Policy. Useful for managed dev environments.",
            Tags = ["vscode", "update", "auto-update", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.mode", "none")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.mode")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.mode", "none")],
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
            Id = "vscode-disable-auto-update",
            Label = "Disable VS Code Auto-Updates",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents VS Code from checking for and downloading updates automatically via policy.",
            Tags = ["vscode", "updates"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.mode", "none")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.mode")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.mode", "none")],
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
            Id = "vscode-disable-experiment-service",
            Label = "Disable VS Code Experiment Service",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Opts out of VS Code A/B experiments that change editor behaviour without user consent.",
            Tags = ["vscode", "privacy", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.enableExperiments", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.enableExperiments")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.enableExperiments", 0)],
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
    ];
}
