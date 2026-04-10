namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyBITS
{
    private const string BitsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BITS";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "bits-limit-max-bandwidth",
            Label = "Limit BITS Maximum Bandwidth (500 KB/s)",
            Category = "Network — BITS Transfer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxBandwidth=500 (KB/s) in BITS policy. Caps the maximum bandwidth that "
                + "Background Intelligent Transfer Service can consume, preventing large Windows Update "
                + "or SCCM downloads from saturating the network link during business hours.",
            Tags = ["bits", "bandwidth", "network", "policy", "throttle"],
            RegistryKeys = [BitsKey],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Caps BITS to 500 KB/s; large updates may take longer to download.",
            ApplyOps = [RegOp.SetDword(BitsKey, "MaxBandwidth", 500)],
            RemoveOps = [RegOp.DeleteValue(BitsKey, "MaxBandwidth")],
            DetectOps = [RegOp.CheckDword(BitsKey, "MaxBandwidth", 500)],
        },
        new TweakDef
        {
            Id = "bits-disable-peercaching",
            Label = "Disable BITS Peer Caching",
            Category = "Network — BITS Transfer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnablePeercaching=0 in BITS policy. Disables peer-to-peer file sharing for BITS "
                + "transfers, ensuring all downloads come from the configured server or CDN. Reduces "
                + "LAN traffic from peer-to-peer BITS transfers and closes a lateral file-sharing vector.",
            Tags = ["bits", "peer-cache", "network", "policy", "security"],
            RegistryKeys = [BitsKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "All BITS downloads come from the server; peer caching disabled.",
            ApplyOps = [RegOp.SetDword(BitsKey, "EnablePeercaching", 0)],
            RemoveOps = [RegOp.DeleteValue(BitsKey, "EnablePeercaching")],
            DetectOps = [RegOp.CheckDword(BitsKey, "EnablePeercaching", 0)],
        },
        new TweakDef
        {
            Id = "bits-max-jobs-per-user",
            Label = "Limit BITS Jobs Per User to 50",
            Category = "Network — BITS Transfer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxJobsPerUser=50 in BITS policy. Caps the number of concurrent BITS transfer "
                + "jobs a single user can create, preventing resource exhaustion from runaway or malicious "
                + "BITS job creation. Default is 300.",
            Tags = ["bits", "jobs", "limit", "policy", "security"],
            RegistryKeys = [BitsKey],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Standard users limited to 50 BITS jobs; automated tools may need adjustment.",
            ApplyOps = [RegOp.SetDword(BitsKey, "MaxJobsPerUser", 50)],
            RemoveOps = [RegOp.DeleteValue(BitsKey, "MaxJobsPerUser")],
            DetectOps = [RegOp.CheckDword(BitsKey, "MaxJobsPerUser", 50)],
        },
        new TweakDef
        {
            Id = "bits-max-files-per-job",
            Label = "Limit BITS Files Per Job to 100",
            Category = "Network — BITS Transfer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxFilesPerJob=100 in BITS policy. Caps the number of files in a single BITS "
                + "transfer job, preventing oversized jobs from monopolising network resources and disk I/O.",
            Tags = ["bits", "files", "limit", "policy"],
            RegistryKeys = [BitsKey],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Each BITS job limited to 100 files; large deployments may need splitting.",
            ApplyOps = [RegOp.SetDword(BitsKey, "MaxFilesPerJob", 100)],
            RemoveOps = [RegOp.DeleteValue(BitsKey, "MaxFilesPerJob")],
            DetectOps = [RegOp.CheckDword(BitsKey, "MaxFilesPerJob", 100)],
        },
        new TweakDef
        {
            Id = "bits-max-ranges-per-file",
            Label = "Limit BITS Byte Ranges Per File to 50",
            Category = "Network — BITS Transfer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxRangesPerFile=50 in BITS policy. Caps the number of byte ranges a BITS job "
                + "can request for a single file. Prevents excessive partial-download requests that "
                + "increase server load and network round-trips.",
            Tags = ["bits", "ranges", "limit", "policy", "performance"],
            RegistryKeys = [BitsKey],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Limits byte-range requests per file; normal downloads unaffected.",
            ApplyOps = [RegOp.SetDword(BitsKey, "MaxRangesPerFile", 50)],
            RemoveOps = [RegOp.DeleteValue(BitsKey, "MaxRangesPerFile")],
            DetectOps = [RegOp.CheckDword(BitsKey, "MaxRangesPerFile", 50)],
        },
        new TweakDef
        {
            Id = "bits-disable-notifications",
            Label = "Disable BITS Transfer Notifications",
            Category = "Network — BITS Transfer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableBITSNotification=1 in BITS policy. Prevents BITS from raising toast "
                + "notifications or balloon tips when jobs complete or fail, reducing notification "
                + "noise on managed desktops.",
            Tags = ["bits", "notifications", "policy", "quiet"],
            RegistryKeys = [BitsKey],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "No BITS completion/failure notifications; use Event Viewer for monitoring.",
            ApplyOps = [RegOp.SetDword(BitsKey, "DisableBITSNotification", 1)],
            RemoveOps = [RegOp.DeleteValue(BitsKey, "DisableBITSNotification")],
            DetectOps = [RegOp.CheckDword(BitsKey, "DisableBITSNotification", 1)],
        },
        new TweakDef
        {
            Id = "bits-disable-background-http",
            Label = "Disable BITS Background HTTP Downloads",
            Category = "Network — BITS Transfer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableBackgroundHTTP=1 in BITS policy. Prevents BITS from using background "
                + "HTTP transfers, requiring all transfers to be foreground (user-initiated). Useful "
                + "on metered connections where background downloads are undesirable.",
            Tags = ["bits", "background", "http", "policy", "metered"],
            RegistryKeys = [BitsKey],
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "All BITS transfers require foreground; Windows Update may not auto-download.",
            ApplyOps = [RegOp.SetDword(BitsKey, "DisableBackgroundHTTP", 1)],
            RemoveOps = [RegOp.DeleteValue(BitsKey, "DisableBackgroundHTTP")],
            DetectOps = [RegOp.CheckDword(BitsKey, "DisableBackgroundHTTP", 1)],
        },
        new TweakDef
        {
            Id = "bits-max-download-time-hours",
            Label = "Limit BITS Max Download Time to 24 Hours",
            Category = "Network — BITS Transfer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxDownloadTime=86400 (seconds = 24 hours) in BITS policy. Sets the maximum "
                + "time a BITS job can run before being cancelled. Prevents stalled jobs from occupying "
                + "BITS queue slots indefinitely.",
            Tags = ["bits", "timeout", "limit", "policy"],
            RegistryKeys = [BitsKey],
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Stalled BITS jobs cancelled after 24 hours; very large downloads may need retry.",
            ApplyOps = [RegOp.SetDword(BitsKey, "MaxDownloadTime", 86400)],
            RemoveOps = [RegOp.DeleteValue(BitsKey, "MaxDownloadTime")],
            DetectOps = [RegOp.CheckDword(BitsKey, "MaxDownloadTime", 86400)],
        },
        new TweakDef
        {
            Id = "bits-disable-client-certificate",
            Label = "Disable BITS Client Certificate Auth",
            Category = "Network — BITS Transfer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableClientCertificate=1 in BITS policy. Prevents BITS from using client-side "
                + "X.509 certificates for transfer authentication. Simplifies BITS transport security "
                + "by relying on server-side TLS only.",
            Tags = ["bits", "certificate", "auth", "policy", "tls"],
            RegistryKeys = [BitsKey],
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "BITS cannot use client certs; mutual TLS auth for BITS transfers disabled.",
            ApplyOps = [RegOp.SetDword(BitsKey, "DisableClientCertificate", 1)],
            RemoveOps = [RegOp.DeleteValue(BitsKey, "DisableClientCertificate")],
            DetectOps = [RegOp.CheckDword(BitsKey, "DisableClientCertificate", 1)],
        },
        new TweakDef
        {
            Id = "bits-max-jobs-per-machine",
            Label = "Limit BITS Total Jobs Per Machine to 200",
            Category = "Network — BITS Transfer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxJobsPerMachine=200 in BITS policy. Caps the total number of BITS jobs "
                + "across all users on the machine. Prevents runaway BITS job creation from "
                + "exhausting system resources. Default is 800.",
            Tags = ["bits", "jobs", "machine", "limit", "policy"],
            RegistryKeys = [BitsKey],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Machine-wide BITS job limit 200; heavy automation environments may need more.",
            ApplyOps = [RegOp.SetDword(BitsKey, "MaxJobsPerMachine", 200)],
            RemoveOps = [RegOp.DeleteValue(BitsKey, "MaxJobsPerMachine")],
            DetectOps = [RegOp.CheckDword(BitsKey, "MaxJobsPerMachine", 200)],
        },
    ];
}
