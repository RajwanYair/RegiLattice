// RegiLattice.Core — Tweaks/IisHardeningPolicy.cs
// IIS Web Server Security Hardening — Sprint 432.
// Controls HTTP.sys and W3SVC registry parameters to harden IIS web servers against
// common attack vectors: buffer overflow, URL traversal, connection flooding, header
// injection, and encoding-based WAF bypass techniques.
// Category: "IIS Hardening Policy" | Slug: iisharden
// Registry paths:
//   HKLM\SYSTEM\CurrentControlSet\Services\HTTP\Parameters  — HTTP.sys driver settings
//   HKLM\SYSTEM\CurrentControlSet\Services\W3SVC\Parameters — IIS W3SVC service

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class IisHardeningPolicy
{
    private const string HttpKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\HTTP\Parameters";
    private const string W3SvcKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W3SVC\Parameters";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "iisharden-limit-max-request-bytes",
                Label = "Limit IIS Max Request Buffer (16 KB)",
                Category = "IIS Hardening Policy",
                Description =
                    "Sets MaxRequestBytes=16384 in HTTP.sys. Caps the maximum size of the HTTP request entity body buffer accepted by the kernel-mode HTTP stack. Oversized request buffers are a common vector for DoS and buffer-overflow attacks against IIS. 16 KB is the documented default; reducing it further hardens high-security endpoints while having no effect on typical REST APIs that receive JSON payloads under a few kilobytes.",
                Tags = ["iis", "http-sys", "request-limit", "dos", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Limits entity body buffer; increase for sites that accept large file uploads or form POST bodies.",
                ApplyOps = [RegOp.SetDword(HttpKey, "MaxRequestBytes", 16384)],
                RemoveOps = [RegOp.DeleteValue(HttpKey, "MaxRequestBytes")],
                DetectOps = [RegOp.CheckDword(HttpKey, "MaxRequestBytes", 16384)],
            },
            new TweakDef
            {
                Id = "iisharden-limit-max-field-length",
                Label = "Limit IIS Max Header Field Length (16 KB)",
                Category = "IIS Hardening Policy",
                Description =
                    "Sets MaxFieldLength=16384 in HTTP.sys. Restricts the maximum size of any single HTTP request header field. Excessively long header values (e.g., Cookie or Authorization) are exploited in header injection, HTTP request smuggling, and slow-header denial-of-service attacks. Capping individual fields at 16 KB protects the kernel HTTP stack without affecting any legitimate browser or API client.",
                Tags = ["iis", "http-sys", "header-limit", "injection", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks requests with oversized individual headers; standard browsers and API clients are unaffected.",
                ApplyOps = [RegOp.SetDword(HttpKey, "MaxFieldLength", 16384)],
                RemoveOps = [RegOp.DeleteValue(HttpKey, "MaxFieldLength")],
                DetectOps = [RegOp.CheckDword(HttpKey, "MaxFieldLength", 16384)],
            },
            new TweakDef
            {
                Id = "iisharden-disallow-restricted-chars",
                Label = "Block Restricted Characters in IIS URLs",
                Category = "IIS Hardening Policy",
                Description =
                    "Sets AllowRestrictedChars=0 in HTTP.sys. Instructs the kernel HTTP driver to reject URLs containing characters from the restricted set (control characters and other disallowed byte sequences). Prevents directory traversal and URL injection attacks that rely on encoding restricted characters (e.g., %00, %2F) to confuse URL parsers. This is the secure default on all modern Windows versions.",
                Tags = ["iis", "http-sys", "url-security", "traversal", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Rejects URLs containing restricted characters; legitimate encoded paths are unaffected.",
                ApplyOps = [RegOp.SetDword(HttpKey, "AllowRestrictedChars", 0)],
                RemoveOps = [RegOp.DeleteValue(HttpKey, "AllowRestrictedChars")],
                DetectOps = [RegOp.CheckDword(HttpKey, "AllowRestrictedChars", 0)],
            },
            new TweakDef
            {
                Id = "iisharden-limit-url-segment-length",
                Label = "Limit IIS URL Segment Length (260 chars)",
                Category = "IIS Hardening Policy",
                Description =
                    "Sets UrlSegmentMaxLength=260 in HTTP.sys. Caps the character length of individual URL path segments (the portions between slash delimiters). Excessively long URL segments are used in buffer-overflow probes and WAF evasion techniques. 260 characters aligns with the Windows MAX_PATH constant and accommodates all standard web application URL structures without restriction.",
                Tags = ["iis", "http-sys", "url-length", "buffer", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Rejects URL segments longer than 260 chars; uncommon in legitimate production traffic.",
                ApplyOps = [RegOp.SetDword(HttpKey, "UrlSegmentMaxLength", 260)],
                RemoveOps = [RegOp.DeleteValue(HttpKey, "UrlSegmentMaxLength")],
                DetectOps = [RegOp.CheckDword(HttpKey, "UrlSegmentMaxLength", 260)],
            },
            new TweakDef
            {
                Id = "iisharden-disable-non-utf-encodings",
                Label = "Force UTF-8 URL Encoding on IIS",
                Category = "IIS Hardening Policy",
                Description =
                    "Sets EnableNonUTFEncodings=0 in HTTP.sys. Prevents the kernel HTTP stack from accepting URLs encoded in non-UTF-8 character sets such as MBCS or DBCS. Non-UTF-8 encoded paths are a well-known vector for double-decode attacks and WAF bypass techniques that exploit charset confusion. Enforcing UTF-8 simplifies URL parsing and eliminates an entire class of encoding-based attacks.",
                Tags = ["iis", "http-sys", "encoding", "utf-8", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Rejects non-UTF-8 encoded URLs; applications using DBCS/CJK path segments may need adjustment.",
                ApplyOps = [RegOp.SetDword(HttpKey, "EnableNonUTFEncodings", 0)],
                RemoveOps = [RegOp.DeleteValue(HttpKey, "EnableNonUTFEncodings")],
                DetectOps = [RegOp.CheckDword(HttpKey, "EnableNonUTFEncodings", 0)],
            },
            new TweakDef
            {
                Id = "iisharden-set-connection-timeout",
                Label = "Set IIS Connection Timeout (120 s)",
                Category = "IIS Hardening Policy",
                Description =
                    "Sets ConnectionTimeout=120 in W3SVC Parameters. Limits how long IIS waits for a request to complete or a response to be fully sent before forcibly closing the connection. A 120-second timeout prevents slowloris-style and slow-POST denial-of-service attacks without affecting legitimate long-running API calls. The Windows default is 120 seconds but this may be overridden by IIS configuration; setting it explicitly ensures the hardened value is always in effect.",
                Tags = ["iis", "w3svc", "timeout", "slowloris", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disconnects idle/slow connections after 120 s; increase for APIs with intentionally long-running operations.",
                ApplyOps = [RegOp.SetDword(W3SvcKey, "ConnectionTimeout", 120)],
                RemoveOps = [RegOp.DeleteValue(W3SvcKey, "ConnectionTimeout")],
                DetectOps = [RegOp.CheckDword(W3SvcKey, "ConnectionTimeout", 120)],
            },
            new TweakDef
            {
                Id = "iisharden-limit-listen-backlog",
                Label = "Limit IIS TCP Listen Backlog (1 000)",
                Category = "IIS Hardening Policy",
                Description =
                    "Sets ListenBackLog=1000 in W3SVC Parameters. Controls the TCP incoming connection queue depth for the IIS listener socket. Bounding the backlog prevents memory exhaustion from SYN-flood attacks by limiting the number of half-open connections the kernel will queue before dropping new SYN packets. 1 000 entries is more than sufficient for most enterprise IIS workloads.",
                Tags = ["iis", "w3svc", "tcp", "syn-flood", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Caps the connection queue; under extreme flood conditions new SYN packets may be dropped sooner.",
                ApplyOps = [RegOp.SetDword(W3SvcKey, "ListenBackLog", 1000)],
                RemoveOps = [RegOp.DeleteValue(W3SvcKey, "ListenBackLog")],
                DetectOps = [RegOp.CheckDword(W3SvcKey, "ListenBackLog", 1000)],
            },
            new TweakDef
            {
                Id = "iisharden-disable-socket-pooling",
                Label = "Disable IIS Socket Pooling",
                Category = "IIS Hardening Policy",
                Description =
                    "Sets DisableSocketPool=1 in W3SVC Parameters. Disables IIS socket pooling, which pre-allocates a pool of listening sockets shared across all web sites bound to the same IP address. Socket pooling can allow one site's TLS configuration to influence another site on the same IP. Disabling it gives each site an isolated socket lifecycle and prevents cross-site socket interference in multi-tenant IIS deployments.",
                Tags = ["iis", "w3svc", "socket-pool", "isolation", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables shared socket pool; may slightly increase connection setup overhead on multi-site servers.",
                ApplyOps = [RegOp.SetDword(W3SvcKey, "DisableSocketPool", 1)],
                RemoveOps = [RegOp.DeleteValue(W3SvcKey, "DisableSocketPool")],
                DetectOps = [RegOp.CheckDword(W3SvcKey, "DisableSocketPool", 1)],
            },
            new TweakDef
            {
                Id = "iisharden-limit-max-connections",
                Label = "Limit IIS Max Simultaneous Connections (10 000)",
                Category = "IIS Hardening Policy",
                Description =
                    "Sets MaxConnections=10000 in W3SVC Parameters. Caps the total number of simultaneous TCP connections IIS will accept across all sites. Without an explicit cap IIS uses the OS default which is effectively unlimited, leaving the server vulnerable to connection-flood attacks. 10 000 connections accommodates legitimate enterprise HTTP/1.1 and HTTP/2 traffic while preventing unbounded memory and thread consumption.",
                Tags = ["iis", "w3svc", "connection-limit", "dos", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Caps simultaneous connections at 10 000; high-traffic public-facing sites may require a larger value.",
                ApplyOps = [RegOp.SetDword(W3SvcKey, "MaxConnections", 10000)],
                RemoveOps = [RegOp.DeleteValue(W3SvcKey, "MaxConnections")],
                DetectOps = [RegOp.CheckDword(W3SvcKey, "MaxConnections", 10000)],
            },
            new TweakDef
            {
                Id = "iisharden-enable-log-error-requests",
                Label = "Enable IIS Error Request Logging",
                Category = "IIS Hardening Policy",
                Description =
                    "Sets LogErrorRequests=1 in HTTP.sys Parameters. Instructs the kernel HTTP driver to log all requests that result in an error response (4xx/5xx). Error request logs are essential for detecting attack reconnaissance (404 directory sweeps), injection probes, and protocol violation attempts. Disabled by default in some configurations; enabling it ensures complete HTTP-level audit coverage independent of IIS application-layer logging.",
                Tags = ["iis", "http-sys", "logging", "audit", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Increases log volume on actively attacked servers; no functional impact on normal HTTP traffic.",
                ApplyOps = [RegOp.SetDword(HttpKey, "LogErrorRequests", 1)],
                RemoveOps = [RegOp.DeleteValue(HttpKey, "LogErrorRequests")],
                DetectOps = [RegOp.CheckDword(HttpKey, "LogErrorRequests", 1)],
            },
        ];
}
