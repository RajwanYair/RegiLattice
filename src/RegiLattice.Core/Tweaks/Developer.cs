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

// ── merged from PackageManagement.cs ──
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
        new TweakDef
        {
            Id = "pkg-disable-shared-experiences",
            Label = "Disable Cross-Device App Experiences",
            Category = "Developer",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables shared experiences (app hand-off between devices). Default: enabled.",
            Tags = ["package", "shared-experiences", "cross-device", "cdp"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "RomeSdkChannelUserAuthzPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "RomeSdkChannelUserAuthzPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP", "RomeSdkChannelUserAuthzPolicy", 0)],
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
            Id = "java-disable-update-scheduler",
            Label = "Disable Java Update Scheduler Notifications",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Java update scheduler download/install notifications. Default: Enabled. Recommended: Disabled.",
            Tags = ["java", "update", "scheduler", "notifications"],
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
            Id = "java-security-veryhigh",
            Label = "Set Java Security Level to Very High",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets Java deployment security level to VERY_HIGH via policy. Default: HIGH. Recommended: VERY_HIGH.",
            Tags = ["java", "security", "deployment", "veryhigh"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH"),
            ],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level", "HIGH")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH"),
            ],
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
            Id = "java-disable-auto-update",
            Label = "Disable Java Auto-Update",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Java automatic update checks. Prevents background update service from consuming resources. Default: enabled.",
            Tags = ["java", "update", "auto-update", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 0)],
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
            Id = "java-disable-java-sponsor",
            Label = "Disable Java Sponsor Offers",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables sponsor offers (toolbars, search engines) during Java updates. Prevents bundled software installation. Default: enabled.",
            Tags = ["java", "sponsor", "offers", "bloatware"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 0)],
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
            Id = "java-disable-java-update",
            Label = "Disable Java Update Service",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Java Update Scheduler (jusched.exe) at the Run key level. Prevents background update checks. Default: enabled.",
            Tags = ["java", "update", "scheduler", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\JavaSoft\Java Update\Policy", "EnableJavaUpdate", 0)],
        },
        new TweakDef
        {
            Id = "java-disable-java-web-plugin",
            Label = "Disable Java Web Browser Plugin",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Java browser plugin for all browsers. Reduces attack surface from browser-based Java exploits. Default: enabled.",
            Tags = ["java", "browser", "plugin", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "false")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "true")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.webjava.enabled", "false")],
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
            Id = "java-disable-auto-update-notify",
            Label = "Disable Java Auto-Update Notification",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Prevents Java from checking for updates and showing update notifications. Default: enabled.",
            Tags = ["java", "update", "notification"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Update\Policy", "EnableAutoUpdateCheck", 0)],
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
            Id = "java-set-security-very-high",
            Label = "Set Java Security Level to Very High",
            Category = "Developer",
            NeedsAdmin = true,
            Description = "Sets the Java security slider to Very High, requiring all applets to be signed and valid. Default: High.",
            Tags = ["java", "security", "applet", "hardening"],
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
            Id = "java-disable-web-java",
            Label = "Disable Java in Web Browsers",
            Category = "Developer",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Java execution in web browsers via the deployment policy. Prevents Java applets from running in any browser with the Java plugin. Default: web Java enabled.",
            Tags = ["java", "browser", "web", "security"],
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
