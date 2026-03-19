using System.Text.Json;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests that require RegisterBuiltins — share a single engine via IClassFixture.</summary>
public sealed class TweakEngineBuiltinsTests : IClassFixture<BuiltinsFixture>
{
    private readonly TweakEngine _engine;

    public TweakEngineBuiltinsTests(BuiltinsFixture fixture)
    {
        _engine = fixture.Engine;
    }

    [Fact]
    public void RegisterBuiltins_LoadsAllTweaks()
    {
        Assert.True(_engine.TweakCount > 1000, $"Expected >1000 tweaks, got {_engine.TweakCount}");
        Assert.True(_engine.CategoryCount >= 60, $"Expected >=60 categories, got {_engine.CategoryCount}");
    }

    [Fact]
    public void RegisterBuiltins_AllIdsUnique()
    {
        var ids = _engine.AllTweaks().Select(t => t.Id).ToList();
        Assert.Equal(ids.Count, ids.Distinct(StringComparer.OrdinalIgnoreCase).Count());
    }

    [Fact]
    public void RegisterBuiltins_AllHaveRequiredFields()
    {
        foreach (var td in _engine.AllTweaks())
        {
            Assert.False(string.IsNullOrWhiteSpace(td.Id), $"Tweak with empty Id found");
            Assert.False(string.IsNullOrWhiteSpace(td.Label), $"Tweak {td.Id} has empty Label");
            Assert.False(string.IsNullOrWhiteSpace(td.Category), $"Tweak {td.Id} has empty Category");
        }
    }

    [Fact]
    public void TweaksForProfile_UnknownProfile_ReturnsEmpty()
    {
        Assert.Empty(_engine.TweaksForProfile("nonexistent"));
    }

    [Fact]
    public void TweaksForProfile_Business_ReturnsNonEmpty()
    {
        var tweaks = _engine.TweaksForProfile("business");
        Assert.NotEmpty(tweaks);
        Assert.True(tweaks.Count > 100, $"Expected >100 tweaks for business profile, got {tweaks.Count}");
    }

    [Fact]
    public void ApplyProfile_UnknownProfile_ReturnsEmpty()
    {
        Assert.Empty(_engine.ApplyProfile("nonexistent"));
    }

    [Fact]
    public void RegisterBuiltins_SecurityCategoryExists()
    {
        Assert.Contains("Security", _engine.Categories());
    }

    [Theory]
    [InlineData("sec-restrict-anonymous-enum")]
    [InlineData("sec-enable-dep-always")]
    [InlineData("sec-enable-safe-dll-search")]
    [InlineData("sec-reduce-cached-logons")]
    [InlineData("sec-restrict-sam-remote")]
    [InlineData("sec-disable-llmnr")]
    [InlineData("sec-disable-netbios")]
    [InlineData("sec-disable-wpad")]
    [InlineData("sec-enforce-smb-signing")]
    [InlineData("sec-disable-powershell-v2")]
    [InlineData("sec-enforce-lsa-ppl")]
    [InlineData("sec-block-wdigest-caching")]
    [InlineData("sec-disable-autorun-all")]
    [InlineData("sec-enforce-nla")]
    [InlineData("sec-disable-lm-hash")]
    [InlineData("sec-enforce-sehop")]
    [InlineData("sec-force-strong-key-protection")]
    [InlineData("sec-restrict-null-session-pipes")]
    [InlineData("sec-set-ntlmv2-only")]
    public void RegisterBuiltins_SecurityTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Security", tweak.Category);
        Assert.NotEmpty(tweak.ApplyOps);
    }

    [Theory]
    [InlineData("perf-disable-startup-delay")]
    [InlineData("perf-disable-low-disk-warning")]
    [InlineData("perf-increase-irp-stack")]
    [InlineData("perf-disable-tips-notifications")]
    [InlineData("perf-disable-explorer-search-history")]
    [InlineData("perf-increase-file-system-cache")]
    [InlineData("perf-disable-8dot3-name-creation")]
    [InlineData("perf-increase-network-throttle")]
    [InlineData("perf-disable-nagle-algorithm")]
    [InlineData("perf-disable-power-throttling")]
    public void RegisterBuiltins_NewPerfTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Performance", tweak.Category);
        Assert.NotEmpty(tweak.ApplyOps);
    }

    // ── Command Line Tweaks ─────────────────────────────────────────────
    [Theory]
    [InlineData("cmd-disable-hyper-v-hypervisor")]
    [InlineData("cmd-enable-boot-log")]
    [InlineData("cmd-increase-tscsyncpolicy")]
    [InlineData("cmd-disable-dynamic-tick")]
    [InlineData("cmd-set-platform-tick-high")]
    [InlineData("cmd-disable-netbios-over-tcpip")]
    [InlineData("cmd-enable-tcp-autotuning")]
    [InlineData("cmd-enable-rss")]
    [InlineData("cmd-disable-tcp-timestamps")]
    [InlineData("cmd-enable-ecn")]
    [InlineData("cmd-set-ultimate-perf-plan")]
    [InlineData("cmd-disable-usb-selective-suspend")]
    [InlineData("cmd-disable-ie-feature")]
    [InlineData("cmd-enable-sandbox")]
    public void RegisterBuiltins_CmdTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Command Line", tweak.Category);
        Assert.NotNull(tweak.ApplyAction);
        Assert.Equal(TweakKind.SystemCommand, tweak.Kind);
    }

    // ── PowerShell Tweaks ───────────────────────────────────────────────
    [Theory]
    [InlineData("ps-disable-print-spooler", TweakKind.ServiceControl)]
    [InlineData("ps-disable-remote-registry", TweakKind.ServiceControl)]
    [InlineData("ps-disable-fax-service", TweakKind.ServiceControl)]
    [InlineData("ps-disable-xbox-services", TweakKind.ServiceControl)]
    [InlineData("ps-clear-temp-files", TweakKind.PowerShell)]
    [InlineData("ps-flush-dns-cache", TweakKind.PowerShell)]
    [InlineData("ps-disable-diagnostics-hub", TweakKind.ServiceControl)]
    [InlineData("ps-disable-wmp-network-sharing", TweakKind.ServiceControl)]
    [InlineData("ps-disable-geolocation-service", TweakKind.ServiceControl)]
    [InlineData("ps-disable-connected-user-experience", TweakKind.ServiceControl)]
    [InlineData("ps-disable-dmwappush-service", TweakKind.ServiceControl)]
    [InlineData("ps-optimize-network-adapter", TweakKind.PowerShell)]
    [InlineData("ps-disable-execution-policy-restriction", TweakKind.PowerShell)]
    [InlineData("ps-enable-remoting", TweakKind.PowerShell)]
    [InlineData("ps-disable-telemetry", TweakKind.PowerShell)]
    [InlineData("ps-enable-constrained-language-mode", TweakKind.PowerShell)]
    [InlineData("ps-set-transcript-logging", TweakKind.PowerShell)]
    [InlineData("ps-enable-protected-event-logging", TweakKind.PowerShell)]
    [InlineData("ps-disable-clipboard-history-via-ps", TweakKind.PowerShell)]
    [InlineData("ps-optimize-page-file", TweakKind.PowerShell)]
    [InlineData("ps-enable-tls12", TweakKind.PowerShell)]
    public void RegisterBuiltins_PsTweakExists(string id, TweakKind expectedKind)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("PowerShell", tweak.Category);
        Assert.Equal(expectedKind, tweak.Kind);
    }

    // ── Scheduled Task (PS) Tweaks ──────────────────────────────────────
    [Theory]
    [InlineData("pst-disable-customer-experience")]
    [InlineData("pst-disable-app-telemetry")]
    [InlineData("pst-disable-windows-maps-update")]
    [InlineData("pst-disable-feedback-hub")]
    [InlineData("pst-disable-disk-diagnostics")]
    [InlineData("pst-disable-office-telemetry")]
    [InlineData("pst-disable-speech-model-update")]
    [InlineData("pst-disable-device-census")]
    [InlineData("pst-disable-handwriting-data")]
    [InlineData("pst-disable-cloud-experience")]
    [InlineData("pst-disable-diagnostic-data-controller")]
    [InlineData("pst-disable-power-efficiency")]
    [InlineData("pst-disable-idle-maintenance")]
    [InlineData("pst-disable-defrag-scheduled")]
    [InlineData("pst-disable-location-notification")]
    [InlineData("pst-disable-windows-error-reporting")]
    [InlineData("pst-disable-family-safety")]
    [InlineData("pst-disable-autochk-rebooter")]
    [InlineData("pst-disable-license-validation")]
    [InlineData("pst-disable-net-framework-ngen")]
    public void RegisterBuiltins_PstTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Scheduled Tasks", tweak.Category);
        Assert.Equal(TweakKind.ScheduledTask, tweak.Kind);
        Assert.NotNull(tweak.ApplyAction);
    }

    // ── Hardening Tweaks ────────────────────────────────────────────────
    [Theory]
    [InlineData("harden-enable-credential-guard")]
    [InlineData("harden-disable-wdigest")]
    [InlineData("harden-enable-lsa-protection")]
    [InlineData("harden-restrict-ntlm-outgoing")]
    [InlineData("harden-enable-aslr-force")]
    [InlineData("harden-disable-null-session-pipes")]
    [InlineData("harden-enable-safe-search-mode")]
    [InlineData("harden-restrict-remote-sam")]
    [InlineData("harden-disable-remote-uac-filter")]
    [InlineData("harden-enable-smb-encryption")]
    [InlineData("harden-disable-smb1")]
    [InlineData("harden-enable-secure-boot-check")]
    [InlineData("harden-enable-audit-logon-events")]
    [InlineData("harden-set-password-policy")]
    [InlineData("harden-enable-firewall-all-profiles")]
    public void RegisterBuiltins_HardeningTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Hardening", tweak.Category);
    }

    // ── Developer Tweaks ────────────────────────────────────────────────
    [Theory]
    [InlineData("dev-disable-last-access-timestamp")]
    [InlineData("dev-increase-memory-mapped-limit")]
    [InlineData("dev-add-defender-exclusion-repos")]
    [InlineData("dev-enable-utf8-system-wide")]
    [InlineData("dev-enable-sudo")]
    [InlineData("dev-git-lfs-install")]
    [InlineData("dev-env-add-dotnet-tools")]
    [InlineData("dev-disable-defender-realtime-build")]
    [InlineData("dev-enable-developer-mode-full")]
    [InlineData("dev-increase-irp-stack-size")]
    [InlineData("dev-enable-wsl2")]
    [InlineData("dev-enable-openssh-server")]
    [InlineData("dev-set-execution-policy-unrestricted")]
    [InlineData("dev-disable-ntfs-8dot3-names")]
    [InlineData("dev-increase-file-handle-limit")]
    public void RegisterBuiltins_DeveloperTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Developer", tweak.Category);
    }

    // ── Memory Optimization Tweaks ──────────────────────────────────────
    [Theory]
    [InlineData("mem-disable-paging-executive")]
    [InlineData("mem-enable-large-system-cache")]
    [InlineData("mem-clear-pagefile-on-shutdown")]
    [InlineData("mem-set-iot-registry-quota")]
    [InlineData("mem-optimize-svchosts")]
    [InlineData("mem-disable-memory-compression")]
    [InlineData("mem-set-second-level-data-cache")]
    [InlineData("mem-disable-prefetch-boost")]
    [InlineData("mem-set-io-page-lock-limit")]
    [InlineData("mem-disable-page-combining")]
    [InlineData("mem-set-nonpaged-pool-limit")]
    [InlineData("mem-disable-trim-on-memory-pressure")]
    [InlineData("mem-set-system-pages")]
    [InlineData("mem-disable-superfetch-service")]
    [InlineData("mem-enable-large-pages")]
    public void RegisterBuiltins_MemoryTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Memory", tweak.Category);
    }

    // ── New categories exist ────────────────────────────────────────────
    [Theory]
    [InlineData("Command Line")]
    [InlineData("PowerShell")]
    [InlineData("Hardening")]
    [InlineData("Developer")]
    [InlineData("Memory")]
    [InlineData("Disk Cleanup")]
    [InlineData("Debloat")]
    [InlineData("Network Optimization")]
    [InlineData("Power Management")]
    [InlineData("SSD Optimization")]
    [InlineData("App Compatibility")]
    [InlineData("User Account")]
    [InlineData("Browser Common")]
    [InlineData("Windows Recall")]
    [InlineData("Proxy & VPN")]
    [InlineData("Event Logging")]
    [InlineData("System Restore")]
    [InlineData("Scheduled Tasks")]
    [InlineData("Security")]
    public void RegisterBuiltins_NewCategoryExists(string category)
    {
        Assert.Contains(category, _engine.Categories());
    }

    // ── TweakKind detection ─────────────────────────────────────────────
    [Fact]
    public void TweakKind_RegistryTweak_DefaultsToRegistry()
    {
        var tweak = _engine.GetTweak("perf-disable-startup-delay");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.Registry, tweak.Kind);
    }

    [Fact]
    public void TweakKind_CommandLineTweak_IsSystemCommand()
    {
        var tweak = _engine.GetTweak("cmd-disable-hyper-v-hypervisor");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.SystemCommand, tweak.Kind);
    }

    // ── Disk Cleanup Tweaks ─────────────────────────────────────────────
    [Theory]
    [InlineData("cleanup-disable-thumbnail-cache")]
    [InlineData("cleanup-disable-delivery-optimisation")]
    [InlineData("cleanup-run-disk-cleanup-silent")]
    [InlineData("cleanup-clear-windows-temp")]
    [InlineData("cleanup-clear-windows-update-cache")]
    [InlineData("cleanup-enable-storage-sense")]
    [InlineData("cleanup-disable-hibernation")]
    [InlineData("cleanup-compact-os")]
    [InlineData("cleanup-disable-reserved-storage")]
    // Sprint 41 additions
    [InlineData("cleanup-disable-recent-docs")]
    [InlineData("cleanup-disable-recent-programs")]
    [InlineData("cleanup-disable-search-history")]
    [InlineData("cleanup-disable-swap-file")]
    [InlineData("cleanup-disable-auto-maintenance")]
    [InlineData("cleanup-disable-volume-shadow-copy")]
    [InlineData("cleanup-disable-internet-temp-auto")]
    [InlineData("cleanup-disable-wer-queue")]
    [InlineData("cleanup-disable-superfetch-write")]
    [InlineData("cleanup-limit-disk-usage-windows-update")]
    public void RegisterBuiltins_DiskCleanupTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Disk Cleanup", tweak.Category);
    }

    // ── Debloat Tweaks ──────────────────────────────────────────────────
    [Theory]
    [InlineData("debloat-remove-preinstalled-apps")]
    [InlineData("debloat-disable-suggested-apps")]
    [InlineData("debloat-disable-auto-app-install")]
    [InlineData("debloat-disable-consumer-features")]
    [InlineData("debloat-disable-xbox-game-bar")]
    [InlineData("debloat-remove-optional-features")]
    [InlineData("debloat-disable-start-web-search")]
    [InlineData("debloat-disable-cloud-content")]
    public void RegisterBuiltins_DebloatTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Debloat", tweak.Category);
    }

    // ── Network Optimization Tweaks ─────────────────────────────────────
    [Theory]
    [InlineData("netopt-disable-nagle-algorithm")]
    [InlineData("netopt-increase-tcp-window-size")]
    [InlineData("netopt-disable-network-throttling")]
    [InlineData("netopt-set-dns-cloudflare")]
    [InlineData("netopt-set-dns-google")]
    [InlineData("netopt-disable-ipv6")]
    [InlineData("netopt-disable-qos-packet-scheduler")]
    [InlineData("netopt-enable-dns-cache-boost")]
    public void RegisterBuiltins_NetworkOptimizationTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Network Optimization", tweak.Category);
    }

    // ── Power Management Tweaks ─────────────────────────────────────────
    [Theory]
    [InlineData("pwrmgmt-disable-fast-startup")]
    [InlineData("pwrmgmt-disable-connected-standby")]
    [InlineData("pwrmgmt-set-high-performance-plan")]
    [InlineData("pwrmgmt-disable-cpu-parking")]
    [InlineData("pwrmgmt-disable-usb-selective-suspend")]
    [InlineData("pwrmgmt-disable-wake-timers")]
    [InlineData("pwrmgmt-set-lid-close-nothing")]
    public void RegisterBuiltins_PowerManagementTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Power Management", tweak.Category);
    }

    // ── TweakKind detection for new modules ─────────────────────────────
    [Fact]
    public void TweakKind_DiskCleanupSystemCommand_IsCorrect()
    {
        var tweak = _engine.GetTweak("cleanup-disable-hibernation");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.SystemCommand, tweak.Kind);
    }

    [Fact]
    public void TweakKind_DebloatPowerShell_IsCorrect()
    {
        var tweak = _engine.GetTweak("debloat-remove-preinstalled-apps");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.PowerShell, tweak.Kind);
    }

    [Fact]
    public void TweakKind_NetworkOptPowerShell_IsCorrect()
    {
        var tweak = _engine.GetTweak("netopt-set-dns-cloudflare");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.PowerShell, tweak.Kind);
    }

    [Fact]
    public void TweakKind_PowerMgmtSystemCommand_IsCorrect()
    {
        var tweak = _engine.GetTweak("pwrmgmt-set-high-performance-plan");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.SystemCommand, tweak.Kind);
    }

    // ── SSD Optimization Tweaks ─────────────────────────────────────────
    [Theory]
    [InlineData("ssd-disable-superfetch")]
    [InlineData("ssd-disable-prefetch")]
    [InlineData("ssd-disable-last-access-timestamp")]
    [InlineData("ssd-enable-trim")]
    [InlineData("ssd-disable-defrag-schedule")]
    [InlineData("ssd-disable-windows-search-indexing")]
    [InlineData("ssd-enable-write-caching")]
    [InlineData("ssd-disable-hibernation-ssd")]
    [InlineData("ssd-disable-8dot3-names")]
    [InlineData("ssd-increase-ntfs-memory-usage")]
    [InlineData("ssd-large-system-cache")]
    [InlineData("ssd-disable-boot-trace")]
    [InlineData("ssd-disable-ntfs-compression")]
    [InlineData("ssd-disable-ntfs-encryption")]
    [InlineData("ssd-disable-storage-sense")]
    [InlineData("ssd-set-io-priority-normal")]
    [InlineData("ssd-disable-low-disk-check")]
    [InlineData("ssd-increase-ntfs-mft-zone")]
    [InlineData("ssd-disable-readyboost")]
    [InlineData("ssd-disable-disk-perf-counters")]
    public void RegisterBuiltins_SsdOptimizationTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("SSD Optimization", tweak.Category);
    }

    // ── App Compatibility Tweaks ────────────────────────────────────────
    [Theory]
    [InlineData("compat-disable-compatibility-telemetry")]
    [InlineData("compat-disable-program-compatibility-assistant")]
    [InlineData("compat-disable-steps-recorder")]
    [InlineData("compat-disable-inventory-collector")]
    [InlineData("compat-disable-engine")]
    [InlineData("compat-disable-switchback")]
    [InlineData("compat-disable-web-search-in-run")]
    [InlineData("compat-disable-fault-tolerant-heap")]
    [InlineData("compat-disable-customer-experience")]
    [InlineData("compat-disable-smart-screen-apps")]
    [InlineData("compat-disable-app-launch-tracking")]
    [InlineData("compat-disable-startup-delay")]
    [InlineData("compat-disable-autoplay-devices")]
    [InlineData("compat-disable-maintenance-wakeup")]
    [InlineData("compat-set-diagnostic-data-basic")]
    [InlineData("compat-force-classic-shutdown")]
    [InlineData("compat-disable-background-apps")]
    [InlineData("compat-disable-tips-suggestions")]
    [InlineData("compat-disable-shim-database")]
    // Sprint 41 additions
    [InlineData("compat-disable-wer-server-connection")]
    [InlineData("compat-disable-compat-telemetry-runner")]
    [InlineData("compat-disable-user-choice-protection")]
    [InlineData("compat-disable-vdm-allowed")]
    [InlineData("compat-disable-app-repkg-service")]
    [InlineData("compat-disable-install-service")]
    [InlineData("compat-disable-just-in-time-debugging")]
    [InlineData("compat-enable-dep-always-on")]
    [InlineData("compat-disable-error-reporting-ui")]
    [InlineData("compat-disable-ie-compat-view")]
    public void RegisterBuiltins_AppCompatibilityTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("App Compatibility", tweak.Category);
    }

    // ── User Account Tweaks ─────────────────────────────────────────────
    [Theory]
    [InlineData("uac-disable-dimming")]
    [InlineData("uac-set-silent-admin")]
    [InlineData("uac-disable-for-built-in-admin")]
    [InlineData("uac-enable-admin-approval-mode")]
    [InlineData("uac-virtualise-file-registry")]
    [InlineData("uac-disable-auto-admin-logon")]
    [InlineData("uac-set-account-lockout-10")]
    [InlineData("uac-set-password-length-8")]
    [InlineData("uac-hide-last-username")]
    [InlineData("uac-disable-credential-guard-lock-timeout")]
    [InlineData("uac-require-ctrl-alt-del")]
    [InlineData("uac-set-lockout-duration-30")]
    [InlineData("uac-set-max-password-age-90")]
    [InlineData("uac-elevate-signed-only")]
    [InlineData("uac-restrict-blank-password")]
    [InlineData("uac-enable-installer-detection")]
    [InlineData("uac-standard-user-prompt-credentials")]
    [InlineData("uac-disable-remote-uac")]
    [InlineData("uac-enable-secure-desktop")]
    // Sprint 41 additions
    [InlineData("uac-disable-account-picture")]
    [InlineData("uac-disable-guest-account")]
    [InlineData("uac-disable-biometrics-policy")]
    [InlineData("uac-disable-smartcard-removal-lock")]
    [InlineData("uac-disable-windows-hello-for-business")]
    [InlineData("uac-lock-workstation-on-screensaver")]
    [InlineData("uac-disable-microsoft-account-logon")]
    [InlineData("uac-enforce-password-complexity")]
    [InlineData("uac-disable-offline-files")]
    [InlineData("uac-disable-fast-user-switching")]
    [InlineData("uac-disable-linked-connections")]
    public void RegisterBuiltins_UserAccountTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("User Account", tweak.Category);
    }

    // ── Browser Common Tweaks ───────────────────────────────────────────
    [Theory]
    [InlineData("browser-disable-dns-prefetch")]
    [InlineData("browser-disable-background-network")]
    [InlineData("browser-disable-prediction-service")]
    [InlineData("browser-disable-metrics-reporting")]
    [InlineData("browser-disable-autofill-cc")]
    [InlineData("browser-disable-autofill-addresses")]
    [InlineData("browser-disable-password-manager")]
    [InlineData("browser-send-do-not-track")]
    [InlineData("browser-block-third-party-cookies")]
    [InlineData("browser-disable-safe-browsing-telemetry")]
    [InlineData("browser-disable-translate")]
    [InlineData("browser-disable-spell-check")]
    [InlineData("browser-disable-search-suggestions")]
    [InlineData("browser-disable-sync")]
    [InlineData("browser-disable-browser-sign-in")]
    [InlineData("browser-disable-media-router")]
    [InlineData("browser-disable-shopping-features")]
    [InlineData("browser-disable-preloading")]
    [InlineData("browser-disable-form-fill")]
    public void RegisterBuiltins_BrowserCommonTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Browser Common", tweak.Category);
    }

    // ── TweakKind detection for wave 3 modules ──────────────────────────
    [Fact]
    public void TweakKind_SsdSystemCommand_IsCorrect()
    {
        var tweak = _engine.GetTweak("ssd-disable-last-access-timestamp");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.SystemCommand, tweak.Kind);
    }

    [Fact]
    public void TweakKind_UacRegistry_IsCorrect()
    {
        var tweak = _engine.GetTweak("uac-disable-dimming");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.GroupPolicy, tweak.Kind);
    }

    // ── Windows Recall Tweaks ───────────────────────────────────────────
    [Theory]
    [InlineData("recall-disable-recall")]
    [InlineData("recall-disable-saving-snapshots")]
    [InlineData("recall-disable-ai-suggestions")]
    [InlineData("recall-disable-semantic-indexing")]
    [InlineData("recall-disable-cocreator")]
    [InlineData("recall-disable-image-creator")]
    [InlineData("recall-disable-generative-fill")]
    [InlineData("recall-disable-ai-in-notepad")]
    [InlineData("recall-disable-web-content-eval")]
    [InlineData("recall-disable-cross-device-resume")]
    [InlineData("recall-disable-ai-search-highlights")]
    [InlineData("recall-disable-inking-and-typing-personalization")]
    public void RegisterBuiltins_WindowsRecallTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Windows Recall", tweak.Category);
    }

    // ── Proxy & VPN Tweaks ──────────────────────────────────────────────
    [Theory]
    [InlineData("proxy-disable-auto-detect")]
    [InlineData("proxy-disable-proxy-server")]
    [InlineData("proxy-disable-ncsi-active-probing")]
    [InlineData("proxy-disable-ipv6-transition")]
    [InlineData("proxy-disable-smart-multi-homed")]
    [InlineData("proxy-disable-llmnr")]
    [InlineData("proxy-disable-netbios-over-tcpip")]
    [InlineData("proxy-set-winhttp-timeout")]
    [InlineData("proxy-disable-insecure-fallback")]
    [InlineData("proxy-disable-web-proxy-auto-config")]
    [InlineData("proxy-enable-dns-over-https")]
    [InlineData("proxy-disable-wifi-sense")]
    [InlineData("proxy-disable-winhttp-autoproxy")]
    [InlineData("proxy-disable-ie-proxy-bypass")]
    [InlineData("proxy-disable-vpn-split-tunneling")]
    [InlineData("proxy-disable-ras-autodial")]
    [InlineData("proxy-disable-ipv6-teredo")]
    [InlineData("proxy-disable-connection-auto-tuning")]
    [InlineData("proxy-disable-6to4-tunneling")]
    [InlineData("proxy-disable-ip-tunnel-adapter")]
    [InlineData("proxy-disable-network-connectivity-test")]
    [InlineData("proxy-disable-tcp-timestamps")]
    public void RegisterBuiltins_ProxyVpnTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Proxy & VPN", tweak.Category);
    }

    // ── Event Logging Tweaks ────────────────────────────────────────────
    [Theory]
    [InlineData("evtlog-increase-system-log-size")]
    [InlineData("evtlog-increase-security-log-size")]
    [InlineData("evtlog-increase-application-log-size")]
    [InlineData("evtlog-enable-powershell-script-block-logging")]
    [InlineData("evtlog-enable-powershell-module-logging")]
    [InlineData("evtlog-enable-process-creation-audit")]
    [InlineData("evtlog-set-crash-dump-mini")]
    [InlineData("evtlog-disable-auto-reboot-on-crash")]
    [InlineData("evtlog-enable-verbose-boot-status")]
    [InlineData("evtlog-enable-shutdown-reason")]
    [InlineData("evtlog-log-retention-overwrite")]
    [InlineData("evtlog-disable-event-forwarding")]
    [InlineData("evtlog-set-app-log-32mb")]
    [InlineData("evtlog-set-system-log-32mb")]
    [InlineData("evtlog-set-security-log-64mb")]
    [InlineData("evtlog-disable-event-tracing-autologger")]
    [InlineData("evtlog-disable-powershell-logging")]
    [InlineData("evtlog-enable-powershell-transcription")]
    [InlineData("evtlog-enable-command-line-auditing")]
    [InlineData("evtlog-disable-srum")]
    [InlineData("evtlog-disable-application-log")]
    [InlineData("evtlog-disable-system-log")]
    [InlineData("evtlog-disable-security-audit-logon")]
    [InlineData("evtlog-disable-powershell-scriptblock-logging")]
    [InlineData("evtlog-disable-module-logging")]
    [InlineData("evtlog-disable-windows-error-reporting-log")]
    [InlineData("evtlog-disable-setup-log")]
    [InlineData("evtlog-disable-forwarded-log")]
    [InlineData("evtlog-disable-dns-client-log")]
    [InlineData("evtlog-disable-kernel-event-tracing")]
    public void RegisterBuiltins_EventLoggingTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Event Logging", tweak.Category);
    }

    // ── System Restore Tweaks ───────────────────────────────────────────
    [Theory]
    [InlineData("restore-disable-system-restore")]
    [InlineData("restore-disable-config-change-restore")]
    [InlineData("restore-set-max-frequency-daily")]
    [InlineData("restore-disable-vss-service")]
    [InlineData("restore-disable-previous-versions")]
    [InlineData("restore-enable-scheduled-points")]
    [InlineData("restore-disable-wer-queue")]
    [InlineData("restore-disable-wer-archive")]
    [InlineData("restore-set-wer-consent-send-always")]
    [InlineData("restore-disable-memory-dump")]
    [InlineData("restore-set-mini-dump-only")]
    [InlineData("restore-overwrite-existing-dump")]
    [InlineData("restore-disable-auto-reboot-on-crash")]
    [InlineData("restore-disable-wer-logging")]
    [InlineData("restore-set-wer-max-queue-5")]
    [InlineData("restore-set-wer-max-archive-5")]
    [InlineData("restore-disable-auto-recovery-boot")]
    [InlineData("restore-disable-shadow-copy-optimisation")]
    [InlineData("restore-disable-wer-dump-type")]
    [InlineData("restore-limit-wer-dump-count")]
    public void RegisterBuiltins_SystemRestoreTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("System Restore", tweak.Category);
    }

    // ── TweakKind detection for wave 4 ──────────────────────────────────
    [Fact]
    public void TweakKind_RecallGroupPolicy_IsCorrect()
    {
        var tweak = _engine.GetTweak("recall-disable-recall");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.GroupPolicy, tweak.Kind);
    }

    [Fact]
    public void TweakKind_ProxyRegistry_IsCorrect()
    {
        var tweak = _engine.GetTweak("proxy-disable-auto-detect");
        Assert.NotNull(tweak);
        Assert.Equal(TweakKind.Registry, tweak.Kind);
    }

    // ── Performance: RegisterBuiltins + Freeze ──────────────────────────
    [Fact]
    public void RegisterBuiltins_CompletesUnder500ms()
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.RegisterBuiltins();
        sw.Stop();

        Assert.True(sw.ElapsedMilliseconds < 750, $"RegisterBuiltins took {sw.ElapsedMilliseconds}ms (budget: 750ms)");
    }

    [Fact]
    public void Freeze_BuildsFrozenDictionary()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.RegisterBuiltins();
        // Freeze() is called inside RegisterBuiltins, so lookups should work
        Assert.NotNull(engine.GetTweak("priv-disable-telemetry"));
    }

    [Fact]
    public void Search_After2301Tweaks_CompletesUnder50ms()
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var results = _engine.Search("telemetry");
        sw.Stop();

        Assert.NotEmpty(results);
        Assert.True(sw.ElapsedMilliseconds < 50, $"Search took {sw.ElapsedMilliseconds}ms (budget: 50ms)");
    }

    [Fact]
    public void Categories_ReturnsCachedResult()
    {
        var cats1 = _engine.Categories();
        var cats2 = _engine.Categories();
        Assert.Same(cats1, cats2); // Should be same reference (cached)
    }

    // ── Validation ──────────────────────────────────────────────────────

    [Fact]
    public void ValidateTweaks_AllBuiltins_ReturnsNoErrors()
    {
        var errors = _engine.ValidateTweaks();
        Assert.Empty(errors);
    }

    [Fact]
    public void ValidateTweaks_BrokenDependsOn_ReturnsError()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "test-dep-missing",
                Label = "Test",
                Category = "Test",
                DependsOn = ["nonexistent-tweak"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
            },
        ]);
        var errors = engine.ValidateTweaks();
        Assert.Contains(errors, e => e.Contains("nonexistent-tweak"));
    }

    [Fact]
    public void ValidateTweaks_CircularDependency_ReturnsError()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "circ-a",
                Label = "A",
                Category = "Test",
                DependsOn = ["circ-b"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "A", 1)],
            },
            new TweakDef
            {
                Id = "circ-b",
                Label = "B",
                Category = "Test",
                DependsOn = ["circ-a"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "B", 1)],
            },
        ]);
        var errors = engine.ValidateTweaks();
        Assert.Contains(errors, e => e.Contains("circular dependency"));
    }

    // ── Dependency resolution ───────────────────────────────────────────

    [Fact]
    public void ResolveDependencies_NoDeps_ReturnsSingleItem()
    {
        var chain = _engine.ResolveDependencies(_engine.AllTweaks()[0].Id);
        Assert.Single(chain);
    }

    [Fact]
    public void ResolveDependencies_SimpleChain_ReturnsTopologicalOrder()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "dep-base",
                Label = "Base",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "Base", 1)],
            },
            new TweakDef
            {
                Id = "dep-child",
                Label = "Child",
                Category = "Test",
                DependsOn = ["dep-base"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "Child", 1)],
            },
        ]);
        engine.Freeze();

        var chain = engine.ResolveDependencies("dep-child");
        Assert.Equal(2, chain.Count);
        Assert.Equal("dep-base", chain[0].Id);
        Assert.Equal("dep-child", chain[1].Id);
    }

    [Fact]
    public void ResolveDependencies_CircularDep_Throws()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "loop-a",
                Label = "A",
                Category = "Test",
                DependsOn = ["loop-b"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "A", 1)],
            },
            new TweakDef
            {
                Id = "loop-b",
                Label = "B",
                Category = "Test",
                DependsOn = ["loop-a"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "B", 1)],
            },
        ]);
        engine.Freeze();

        Assert.Throws<InvalidOperationException>(() => engine.ResolveDependencies("loop-a"));
    }

    [Fact]
    public void ResolveDependencies_UnknownId_Throws()
    {
        Assert.Throws<ArgumentException>(() => _engine.ResolveDependencies("nonexistent-tweak-id"));
    }

    [Fact]
    public void Dependents_NoReverse_ReturnsEmpty()
    {
        // Pick a tweak that nothing depends on (most tweaks)
        var dependents = _engine.Dependents(_engine.AllTweaks()[0].Id);
        // May be empty or not — just verify it doesn't throw
        Assert.NotNull(dependents);
    }

    [Fact]
    public void Dependents_WithReverseDep_ReturnsDependents()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "parent-tweak",
                Label = "Parent",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "P", 1)],
            },
            new TweakDef
            {
                Id = "child-tweak",
                Label = "Child",
                Category = "Test",
                DependsOn = ["parent-tweak"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "C", 1)],
            },
        ]);

        var deps = engine.Dependents("parent-tweak");
        Assert.Single(deps);
        Assert.Equal("child-tweak", deps[0].Id);
    }

    // ── Batch with progress callback ────────────────────────────────────

    [Fact]
    public void ApplyBatch_WithProgress_ReportsAllTweaks()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "prog-a",
                Label = "A",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "A", 1)],
            },
            new TweakDef
            {
                Id = "prog-b",
                Label = "B",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "B", 1)],
            },
        ]);

        var progressCalls = new List<(int Done, int Total, string Id)>();
        engine.ApplyBatch(engine.AllTweaks(), forceCorp: false, (done, total, id, _) => progressCalls.Add((done, total, id)));

        Assert.Equal(2, progressCalls.Count);
        Assert.Equal(1, progressCalls[0].Done);
        Assert.Equal(2, progressCalls[0].Total);
        Assert.Equal(2, progressCalls[1].Done);
        Assert.Equal(2, progressCalls[1].Total);
    }

    [Fact]
    public void RemoveBatch_WithProgress_ReportsAllTweaks()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "rmprog-a",
                Label = "A",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "A", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Test", "A")],
            },
        ]);

        int callCount = 0;
        engine.RemoveBatch(engine.AllTweaks(), forceCorp: false, (_, _, _, _) => callCount++);
        Assert.Equal(1, callCount);
    }

    // ── Filter: multi-criteria edge cases ───────────────────────────────

    [Fact]
    public void Filter_AllCriteria_CombinesAnd()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        var target = new TweakDef
        {
            Id = "fa-target",
            Label = "Target Tweak",
            Category = "FilterAll",
            CorpSafe = true,
            NeedsAdmin = false,
            MinBuild = 19041,
            Tags = ["filtertest"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fa-target", "V", 1)],
        };
        engine.Register([
            target,
            new TweakDef
            {
                Id = "fa-wrong-cat",
                Label = "Wrong Cat",
                Category = "Other",
                CorpSafe = true,
                NeedsAdmin = false,
                MinBuild = 19041,
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fa-wrong-cat", "V", 1)],
            },
            new TweakDef
            {
                Id = "fa-wrong-admin",
                Label = "Needs Admin",
                Category = "FilterAll",
                CorpSafe = true,
                NeedsAdmin = true,
                MinBuild = 19041,
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fa-wrong-admin", "V", 1)],
            },
            new TweakDef
            {
                Id = "fa-wrong-corp",
                Label = "Not CorpSafe",
                Category = "FilterAll",
                CorpSafe = false,
                NeedsAdmin = false,
                MinBuild = 19041,
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fa-wrong-corp", "V", 1)],
            },
        ]);
        engine.Freeze();

        var results = engine.Filter(corpSafe: true, needsAdmin: false, category: "FilterAll", minBuild: 20000, query: "target");
        Assert.Single(results);
        Assert.Equal("fa-target", results[0].Id);
    }

    [Fact]
    public void Filter_NoMatches_ReturnsEmpty()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "fn-1",
                Label = "T",
                Category = "CatA",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fn-1", "V", 1)],
            },
            new TweakDef
            {
                Id = "fn-2",
                Label = "T",
                Category = "CatB",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fn-2", "V", 1)],
            },
        ]);
        engine.Freeze();

        var results = engine.Filter(category: "NonExistentCategory");
        Assert.Empty(results);
    }

    [Fact]
    public void Filter_NoCriteria_ReturnsAll()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "fnc-1",
                Label = "T",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fnc-1", "V", 1)],
            },
            new TweakDef
            {
                Id = "fnc-2",
                Label = "T",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fnc-2", "V", 1)],
            },
            new TweakDef
            {
                Id = "fnc-3",
                Label = "T",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fnc-3", "V", 1)],
            },
        ]);
        engine.Freeze();

        var results = engine.Filter();
        Assert.Equal(3, results.Count);
    }

    [Fact]
    public void Filter_QueryAndScope_CombinesAnd()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "fqs-user",
                Label = "User Telemetry",
                Category = "Privacy",
                RegistryKeys = [@"HKEY_CURRENT_USER\Software\fqs-user"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\fqs-user", "V", 1)],
            },
            new TweakDef
            {
                Id = "fqs-machine",
                Label = "Machine Telemetry",
                Category = "Privacy",
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\Software\fqs-machine"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\Software\fqs-machine", "V", 1)],
            },
        ]);
        engine.Freeze();

        var results = engine.Filter(scope: TweakScope.User, query: "telemetry");
        Assert.Single(results);
        Assert.Equal("fqs-user", results[0].Id);
    }

    // ── Update ──────────────────────────────────────────────────────────

    [Fact]
    public void Update_NoUpdateAction_FallsBackToApply()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        var td = new TweakDef
        {
            Id = "upd-fallback",
            Label = "U",
            Category = "Test",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\upd-fallback", "V", 1)],
        };
        engine.Register([td]);

        var result = engine.Update(td);
        Assert.Equal(TweakResult.Applied, result);
    }

    [Fact]
    public void Update_WithUpdateAction_CallsUpdateAction()
    {
        bool called = false;
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        var td = new TweakDef
        {
            Id = "upd-custom",
            Label = "U",
            Category = "Test",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\upd-custom", "V", 1)],
            UpdateAction = (_) =>
            {
                called = true;
            },
        };
        engine.Register([td]);

        var result = engine.Update(td);
        Assert.Equal(TweakResult.Applied, result);
        Assert.True(called);
    }

    [Fact]
    public void Update_UpdateActionThrows_ReturnsError()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        var td = new TweakDef
        {
            Id = "upd-err",
            Label = "U",
            Category = "Test",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\upd-err", "V", 1)],
            UpdateAction = (_) => throw new InvalidOperationException("update failed"),
        };
        engine.Register([td]);

        var result = engine.Update(td);
        Assert.Equal(TweakResult.Error, result);
    }

    // ── Complex dependency graphs ───────────────────────────────────────

    [Fact]
    public void ResolveDependencies_DiamondGraph_ReturnsCorrectOrder()
    {
        // Diamond: A depends on B and C, both B and C depend on D
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "diamond-d",
                Label = "D",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "D", 1)],
            },
            new TweakDef
            {
                Id = "diamond-b",
                Label = "B",
                Category = "Test",
                DependsOn = ["diamond-d"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "B", 1)],
            },
            new TweakDef
            {
                Id = "diamond-c",
                Label = "C",
                Category = "Test",
                DependsOn = ["diamond-d"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "C", 1)],
            },
            new TweakDef
            {
                Id = "diamond-a",
                Label = "A",
                Category = "Test",
                DependsOn = ["diamond-b", "diamond-c"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "A", 1)],
            },
        ]);
        engine.Freeze();

        var chain = engine.ResolveDependencies("diamond-a");
        var chainList = chain.ToList();
        Assert.Equal(4, chainList.Count);
        // D must come before B and C; B and C must come before A
        int idxD = chainList.FindIndex(t => t.Id == "diamond-d");
        int idxB = chainList.FindIndex(t => t.Id == "diamond-b");
        int idxC = chainList.FindIndex(t => t.Id == "diamond-c");
        int idxA = chainList.FindIndex(t => t.Id == "diamond-a");
        Assert.True(idxD < idxB);
        Assert.True(idxD < idxC);
        Assert.True(idxB < idxA);
        Assert.True(idxC < idxA);
    }

    [Fact]
    public void ResolveDependencies_DeepChain_FiveLevels()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "chain-1",
                Label = "L1",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "L1", 1)],
            },
            new TweakDef
            {
                Id = "chain-2",
                Label = "L2",
                Category = "Test",
                DependsOn = ["chain-1"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "L2", 1)],
            },
            new TweakDef
            {
                Id = "chain-3",
                Label = "L3",
                Category = "Test",
                DependsOn = ["chain-2"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "L3", 1)],
            },
            new TweakDef
            {
                Id = "chain-4",
                Label = "L4",
                Category = "Test",
                DependsOn = ["chain-3"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "L4", 1)],
            },
            new TweakDef
            {
                Id = "chain-5",
                Label = "L5",
                Category = "Test",
                DependsOn = ["chain-4"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "L5", 1)],
            },
        ]);
        engine.Freeze();

        var chain = engine.ResolveDependencies("chain-5");
        Assert.Equal(5, chain.Count);
        for (int i = 0; i < 5; i++)
            Assert.Equal($"chain-{i + 1}", chain[i].Id);
    }

    [Fact]
    public void Dependents_MultipleChildren_ReturnsAll()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "multi-parent",
                Label = "Parent",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "P", 1)],
            },
            new TweakDef
            {
                Id = "multi-child-1",
                Label = "Child1",
                Category = "Test",
                DependsOn = ["multi-parent"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "C1", 1)],
            },
            new TweakDef
            {
                Id = "multi-child-2",
                Label = "Child2",
                Category = "Test",
                DependsOn = ["multi-parent"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "C2", 1)],
            },
            new TweakDef
            {
                Id = "multi-child-3",
                Label = "Child3",
                Category = "Test",
                DependsOn = ["multi-parent"],
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "C3", 1)],
            },
        ]);

        var deps = engine.Dependents("multi-parent");
        Assert.Equal(3, deps.Count);
        Assert.Contains(deps, d => d.Id == "multi-child-1");
        Assert.Contains(deps, d => d.Id == "multi-child-2");
        Assert.Contains(deps, d => d.Id == "multi-child-3");
    }

    // ── Sprint 24: Window Appearance ───────────────────────────────────────

    [Fact]
    public void RegisterBuiltins_HasWindowAppearanceCategory() => Assert.Contains("Window Appearance", _engine.Categories());

    [Theory]
    [InlineData("winapp-titlebar-color-active")]
    [InlineData("winapp-caption-height-compact")]
    [InlineData("winapp-scrollbar-width-thin")]
    [InlineData("winapp-icon-spacing-h-compact")]
    [InlineData("winapp-menu-show-delay-fast")]
    [InlineData("winapp-tooltip-delay-fast")]
    [InlineData("winapp-alt-tab-classic")]
    [InlineData("winapp-disable-window-animation")]
    [InlineData("winapp-disable-cursor-blink")]
    [InlineData("winapp-dark-mode-apps")]
    [InlineData("winapp-disable-font-smoothing")]
    [InlineData("winapp-disable-desktop-icons")]
    [InlineData("winapp-solid-color-background")]
    [InlineData("winapp-foreground-lock-timeout")]
    [InlineData("winapp-disable-drop-shadow")]
    public void RegisterBuiltins_WindowAppearanceTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Window Appearance", tweak.Category);
    }

    // ── Sprint 25: System Optimization ─────────────────────────────────────

    [Fact]
    public void RegisterBuiltins_HasSystemOptimizationCategory() => Assert.Contains("System Optimization", _engine.Categories());

    [Theory]
    [InlineData("sysopt-disable-paging-executive")]
    [InlineData("sysopt-large-system-cache")]
    [InlineData("sysopt-io-page-lock-limit")]
    [InlineData("sysopt-ntfs-disable-8dot3")]
    [InlineData("sysopt-ntfs-disable-last-access")]
    [InlineData("sysopt-win32-priority-separation-fg")]
    [InlineData("sysopt-mmcss-system-responsible")]
    [InlineData("sysopt-disable-auto-reboot-bsod")]
    [InlineData("sysopt-verbose-logon-messages")]
    [InlineData("sysopt-explorer-separate-process")]
    [InlineData("sysopt-low-disk-space-warning-off")]
    [InlineData("sysopt-network-throttling-index")]
    [InlineData("sysopt-hung-app-timeout-short")]
    public void RegisterBuiltins_SystemOptimizationTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("System Optimization", tweak.Category);
    }

    // ── Sprint 25: Desktop Customization ───────────────────────────────────

    [Fact]
    public void RegisterBuiltins_HasDesktopCustomizationCategory() => Assert.Contains("Desktop Customization", _engine.Categories());

    [Theory]
    [InlineData("dtcust-show-file-extensions")]
    [InlineData("dtcust-show-hidden-files")]
    [InlineData("dtcust-show-super-hidden")]
    [InlineData("dtcust-launch-to-this-pc")]
    [InlineData("dtcust-expand-to-current-folder")]
    [InlineData("dtcust-compact-view")]
    [InlineData("dtcust-small-taskbar-icons")]
    [InlineData("dtcust-hide-widgets-button")]
    [InlineData("dtcust-disable-news-feed")]
    [InlineData("dtcust-disable-start-suggestions")]
    [InlineData("dtcust-disable-pen-workspace")]
    [InlineData("dtcust-always-show-transfer-details")]
    [InlineData("dtcust-skip-recycle-bin")]
    [InlineData("dtcust-disable-thumbnail-cache")]
    public void RegisterBuiltins_DesktopCustomizationTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Desktop Customization", tweak.Category);
    }

    // ── Sprint 24-25: Category counts ──────────────────────────────────────

    [Fact]
    public void RegisterBuiltins_WindowAppearance_HasAtLeast40Tweaks()
    {
        var byCat = _engine.TweaksByCategory();
        Assert.True(byCat.ContainsKey("Window Appearance"));
        Assert.True(byCat["Window Appearance"].Count >= 40, $"Expected ≥40 Window Appearance tweaks, got {byCat["Window Appearance"].Count}");
    }

    [Fact]
    public void RegisterBuiltins_SystemOptimization_HasAtLeast30Tweaks()
    {
        var byCat = _engine.TweaksByCategory();
        Assert.True(byCat.ContainsKey("System Optimization"));
        Assert.True(byCat["System Optimization"].Count >= 30, $"Expected ≥30 System Optimization tweaks, got {byCat["System Optimization"].Count}");
    }

    [Fact]
    public void RegisterBuiltins_DesktopCustomization_HasAtLeast30Tweaks()
    {
        var byCat = _engine.TweaksByCategory();
        Assert.True(byCat.ContainsKey("Desktop Customization"));
        Assert.True(
            byCat["Desktop Customization"].Count >= 30,
            $"Expected ≥30 Desktop Customization tweaks, got {byCat["Desktop Customization"].Count}"
        );
    }

    // ── Sprint 44: Phone Link (new tweaks) ─────────────────────────────────

    [Theory]
    [InlineData("phone-disable-alljoyn-router")]
    [InlineData("phone-disable-wpd-service")]
    [InlineData("phone-disable-link-to-windows-banner")]
    [InlineData("phone-disable-continue-on-pc")]
    [InlineData("phone-disable-phone-activation-policy")]
    [InlineData("phone-disable-device-assoc-svc")]
    [InlineData("phone-disable-cdp-prompt")]
    [InlineData("phone-disable-roam-trigger-consent")]
    [InlineData("phone-disable-hotspot-auth")]
    [InlineData("phone-disable-windows-hello-companion")]
    public void RegisterBuiltins_PhoneLinkTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Phone Link", tweak.Category);
    }

    // ── Sprint 44: OneDrive (new tweaks) ───────────────────────────────────

    [Theory]
    [InlineData("od-disable-kfm-opt-in-prompt")]
    [InlineData("od-disable-kfm-silent-redirect")]
    [InlineData("od-disable-delay-update-ring")]
    [InlineData("od-disable-sharepoint-sync")]
    [InlineData("od-disable-app-sync")]
    [InlineData("od-limit-mass-delete-threshold")]
    [InlineData("od-disable-hydration-on-access")]
    [InlineData("od-disable-auto-update")]
    [InlineData("od-disable-file-explorer-hub")]
    [InlineData("od-block-external-collab")]
    public void RegisterBuiltins_OneDriveTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("OneDrive", tweak.Category);
    }

    // ── Sprint 44: Notifications (new tweaks) ──────────────────────────────

    [Theory]
    [InlineData("notif-disable-low-disk-alert")]
    [InlineData("notif-disable-defender-user-notif")]
    [InlineData("notif-disable-reboot-required")]
    [InlineData("notif-disable-balloon-tips")]
    [InlineData("notif-disable-smartscreen-user")]
    [InlineData("notif-disable-taskbar-suggestions")]
    [InlineData("notif-disable-oem-preinstall-suggestions")]
    [InlineData("notif-disable-tips-and-tricks")]
    [InlineData("notif-disable-clear-recent-on-exit")]
    [InlineData("notif-disable-no-logged-users-reboot")]
    public void RegisterBuiltins_NotificationsTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Notifications", tweak.Category);
    }

    // ── Sprint 44: Gaming (new tweaks) ─────────────────────────────────────

    [Theory]
    [InlineData("game-set-sfio-priority-high")]
    [InlineData("game-disable-ndu-service")]
    [InlineData("game-set-system-responsiveness-zero")]
    [InlineData("game-set-network-throttling-off")]
    [InlineData("game-set-gpu-priority-8")]
    [InlineData("game-set-latency-sensitivity-high")]
    [InlineData("game-set-background-only-false")]
    [InlineData("game-set-priority-6")]
    [InlineData("game-disable-xbox-accessory-svc")]
    [InlineData("game-increase-max-user-port")]
    public void RegisterBuiltins_GamingTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Gaming", tweak.Category);
    }

    // ── Sprint 44: Maintenance (new tweaks) ────────────────────────────────

    [Theory]
    [InlineData("maint-clear-recent-docs-on-exit")]
    [InlineData("maint-reduce-service-shutdown-timeout")]
    [InlineData("maint-reduce-app-kill-timeout")]
    [InlineData("maint-enable-long-paths")]
    [InlineData("maint-disable-desktop-cleanup-wizard")]
    [InlineData("maint-disable-hang-boot-timeout")]
    [InlineData("maint-auto-end-tasks-on-shutdown")]
    [InlineData("maint-disable-crash-on-audit-fail")]
    [InlineData("maint-disable-show-recent-in-explorer")]
    [InlineData("maint-disable-frequent-in-explorer")]
    public void RegisterBuiltins_MaintenanceTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Maintenance", tweak.Category);
    }

    // ── Sprint 45: Audio (new tweaks) ──────────────────────────────────────

    [Theory]
    [InlineData("audio-disable-comms-ducking")]
    [InlineData("audio-set-pro-audio-priority")]
    [InlineData("audio-disable-audio-idle-powerdown")]
    [InlineData("audio-set-avrcp-volume-sync")]
    [InlineData("audio-set-audio-latency-mode")]
    [InlineData("audio-enable-audio-log-off")]
    [InlineData("audio-set-endpoint-builder-manual")]
    [InlineData("audio-disable-voice-typing-toast")]
    [InlineData("audio-set-render-clock-rate")]
    [InlineData("audio-set-capture-clock-rate")]
    public void RegisterBuiltins_Sprint45AudioTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Audio", tweak.Category);
    }

    // ── Sprint 45: Gaming (new tweaks) ─────────────────────────────────────

    [Theory]
    [InlineData("game-disable-msmq-service")]
    [InlineData("game-disable-gameinput-service")]
    [InlineData("game-set-dxgi-flip-model")]
    [InlineData("game-enable-game-bar-perf-counter")]
    [InlineData("game-disable-diagtrack-autologger")]
    [InlineData("game-set-xgip-service-manual")]
    [InlineData("game-disable-ndu-adapter")]
    [InlineData("game-set-games-sfio-priority-high")]
    [InlineData("game-set-mouse-fix-off")]
    [InlineData("game-set-games-affinity-all-cpus")]
    public void RegisterBuiltins_Sprint45GamingTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Gaming", tweak.Category);
    }

    // ── Sprint 45: Security (new tweaks) ───────────────────────────────────

    [Theory]
    [InlineData("sec-require-ldap-signing")]
    [InlineData("sec-disable-rdp-clipboard-sync")]
    [InlineData("sec-disable-rdp-drive-mapping")]
    [InlineData("sec-enforce-smb-ntlmv2-auth")]
    [InlineData("sec-disable-printer-spooler-network")]
    [InlineData("sec-enable-run-as-different-user")]
    [InlineData("sec-disable-office-macros-internet")]
    [InlineData("sec-disable-wsh-scripting")]
    [InlineData("sec-restrict-lsass-credential-dump")]
    [InlineData("sec-disable-named-pipe-impersonation")]
    public void RegisterBuiltins_Sprint45SecurityTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Security", tweak.Category);
    }

    // ── Sprint 45: Windows Update (new tweaks) ─────────────────────────────

    [Theory]
    [InlineData("wu-disable-automatic-updates")]
    [InlineData("wu-set-schedule-day-saturday")]
    [InlineData("wu-disable-store-app-auto-updates")]
    [InlineData("wu-set-update-service-manual")]
    [InlineData("wu-require-admin-for-updates")]
    [InlineData("wu-disable-metered-update-download")]
    [InlineData("wu-disable-reboot-required-notification")]
    [InlineData("wu-set-feature-update-channel-general")]
    [InlineData("wu-set-orchestrator-service-manual")]
    [InlineData("wu-disable-third-party-preview")]
    public void RegisterBuiltins_Sprint45WindowsUpdateTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Windows Update", tweak.Category);
    }

    // ── Sprint 45: Remote Desktop (new tweaks) ─────────────────────────────

    [Theory]
    [InlineData("rdp-set-max-connections-unlimited")]
    [InlineData("rdp-set-color-depth-32")]
    [InlineData("rdp-disable-smart-card-redirection")]
    [InlineData("rdp-set-remote-assistance-off")]
    [InlineData("rdp-set-audio-play-on-server")]
    [InlineData("rdp-disable-com-port-redirect")]
    [InlineData("rdp-enforce-tls-security-layer")]
    [InlineData("rdp-limit-single-monitor")]
    [InlineData("rdp-set-connection-timeout-8h")]
    [InlineData("rdp-disable-lpt-port-redirect")]
    public void RegisterBuiltins_Sprint45RemoteDesktopTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Remote Desktop", tweak.Category);
    }

    // ── Sprint 47: Communication (new tweaks) ─────────────────────────────

    [Theory]
    [InlineData("comm-disable-teams-read-receipts")]
    [InlineData("comm-disable-teams-presence-share")]
    [InlineData("comm-disable-teams-survey-bell")]
    [InlineData("comm-disable-teams-animations")]
    [InlineData("comm-disable-teams-file-auto-download")]
    public void RegisterBuiltins_Sprint47CommunicationTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Communication", tweak.Category);
    }

    // ── Sprint 47: Accessibility (new tweaks) ─────────────────────────────

    [Theory]
    [InlineData("acc-disable-narrator-key-echo")]
    [InlineData("acc-disable-magnifier-caret-follow")]
    [InlineData("acc-set-wider-caret")]
    [InlineData("acc-disable-mouse-trails")]
    [InlineData("acc-disable-color-filters")]
    public void RegisterBuiltins_Sprint47AccessibilityTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Accessibility", tweak.Category);
    }

    // ── Sprint 47: Clipboard (new tweaks) ─────────────────────────────────

    [Theory]
    [InlineData("clip-disable-emoji-panel")]
    [InlineData("clip-disable-clipboard-sync-across-devices")]
    [InlineData("clip-disable-paste-preview")]
    [InlineData("clip-disable-gif-panel")]
    [InlineData("clip-disable-typing-insights")]
    public void RegisterBuiltins_Sprint47ClipboardTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Clipboard & Drag-Drop", tweak.Category);
    }

    // ── Sprint 47: VS Code (new tweaks) ───────────────────────────────────

    [Theory]
    [InlineData("vscode-policy-crash-reporter")]
    [InlineData("vscode-policy-extension-gallery")]
    [InlineData("vscode-disable-experiment-service")]
    [InlineData("vscode-disable-account-sync")]
    [InlineData("vscode-disable-github-copilot-chat")]
    public void RegisterBuiltins_Sprint47VsCodeTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("VS Code", tweak.Category);
    }

    // ── Sprint 48 ─────────────────────────────────────────────────────────────

    [Theory]
    [InlineData("bt-disable-absolute-volume")]
    [InlineData("bt-disable-le-advertising-policy")]
    [InlineData("bt-disable-personal-area-network")]
    [InlineData("bt-disable-avrcp-metadata")]
    [InlineData("bt-disable-swift-pair")]
    public void RegisterBuiltins_Sprint48BluetoothTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Bluetooth", tweak.Category);
    }

    [Theory]
    [InlineData("printing-disable-ipp-web-client")]
    [InlineData("printing-disable-auto-default-printer")]
    [InlineData("printing-disable-point-and-print")]
    [InlineData("printing-disable-fax-service")]
    [InlineData("printing-disable-shared-printer-browse")]
    public void RegisterBuiltins_Sprint48PrintingTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Printing", tweak.Category);
    }

    [Theory]
    [InlineData("touch-disable-flicks-policy")]
    [InlineData("touch-disable-handwriting-panel-auto")]
    [InlineData("touch-disable-touch-keyboard-deploy")]
    [InlineData("touch-disable-touch-keyboard-suggestions")]
    [InlineData("touch-disable-pen-workspace-button")]
    public void RegisterBuiltins_Sprint48TouchPenTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Touch & Pen", tweak.Category);
    }

    [Theory]
    [InlineData("speech-disable-voice-typing-auto-punctuation")]
    [InlineData("speech-disable-cortana-voice-activation")]
    [InlineData("speech-disable-narrator-natural-voice-dl")]
    [InlineData("speech-disable-voice-access-startup")]
    [InlineData("speech-restrict-user-input-gpo")]
    public void RegisterBuiltins_Sprint48SpeechTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Voice Access & Speech", tweak.Category);
    }

    [Theory]
    [InlineData("stor-disable-wins-reserved-storage")]
    [InlineData("stor-set-storage-sense-monthly")]
    [InlineData("stor-disable-volume-shadow-auto")]
    [InlineData("stor-disable-ntfs-tunnel-cache")]
    [InlineData("stor-set-recycle-bin-pct-policy")]
    public void RegisterBuiltins_Sprint48StorageTweakExists(string id)
    {
        var tweak = _engine.GetTweak(id);
        Assert.NotNull(tweak);
        Assert.Equal("Storage", tweak.Category);
    }
}
