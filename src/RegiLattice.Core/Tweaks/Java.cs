namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Java
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "java-security-high",
            Label = "Java: Set Security Level to Very High",
            Category = "Java",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Raises the Java security level to VERY_HIGH, blocking unsigned applets.",
            Tags = ["java", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH")],
        },
        new TweakDef
        {
            Id = "java-disable-java-tip-of-day",
            Label = "Disable Java Tip of the Day",
            Category = "Java",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the 'Tip of the Day' pop-up dialog in Java Control Panel.",
            Tags = ["java", "ui", "annoyance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.tip.day", "false"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.tip.day"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.javaws.tip.day", "false")],
        },
        new TweakDef
        {
            Id = "java-disable-update-check",
            Label = "Disable Java Auto-Update Check",
            Category = "Java",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Java's automatic update check at startup. Reduces background network traffic. Default: Enabled. Recommended: Disabled for managed environments.",
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
            Category = "Java",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables hardware graphics acceleration for Java/JavaFX applications. Improves rendering performance. Default: Software. Recommended: Hardware.",
            Tags = ["java", "graphics", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment", "JavaFXHardwareAcceleration", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment", "JavaFXHardwareAcceleration"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment", "JavaFXHardwareAcceleration", 1)],
        },
        new TweakDef
        {
            Id = "java-disable-sponsor-offers",
            Label = "Disable Java Sponsor Offers",
            Category = "Java",
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
            Category = "Java",
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
            Category = "Java",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets Java deployment security level to VERY_HIGH via policy. Default: HIGH. Recommended: VERY_HIGH.",
            Tags = ["java", "security", "deployment", "veryhigh"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level", "HIGH"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\JavaSoft\DeploymentProperties", "deployment.security.level", "VERY_HIGH")],
        },
        new TweakDef
        {
            Id = "java-disable-usage-tracking",
            Label = "Disable Java Usage Tracking",
            Category = "Java",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Java usage tracker analytics. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["java", "usage", "tracking", "analytics", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.usagetracker.enabled", "false"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.usagetracker.enabled"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "deployment.usagetracker.enabled", "false")],
        },
        new TweakDef
        {
            Id = "java-set-high-security",
            Label = "Set Java Security Level to Very High",
            Category = "Java",
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
            Category = "Java",
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
            Category = "Java",
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
            Category = "Java",
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
            Category = "Java",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Java Usage Tracker that reports Java runtime usage data to Oracle. Default: enabled.",
            Tags = ["java", "usage", "tracker", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "com.oracle.usagetracker.track.last.usage", "false")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "com.oracle.usagetracker.track.last.usage")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\DeploymentProperties", "com.oracle.usagetracker.track.last.usage", "false")],
        },
    ];
}
