namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── merged from Java.cs ──
internal static class Java
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "java-security-high",
            Label = "Java: Set Security Level to Very High",
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Id = "java-set-proxy-direct",
            Label = "Set Java Proxy to Direct (No Proxy)",
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 1",
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
            Category = "Developer — Windows Terminal Advanced 2",
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
            Category = "Developer — Windows Terminal Advanced 2",
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
            Category = "Developer — Windows Terminal Advanced 2",
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
            Category = "Developer — Windows Terminal Advanced 2",
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
            Category = "Developer — Windows Terminal Advanced 2",
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
            Category = "Developer — Windows Terminal Advanced 2",
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
            Category = "Developer — Windows Terminal Advanced 2",
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
            Category = "Developer — Windows Terminal Advanced 2",
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
            Category = "Developer — Windows Terminal Advanced 2",
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
            Category = "Developer — Windows Terminal Advanced 2",
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
            Category = "Developer — Windows Terminal Advanced 2",
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
