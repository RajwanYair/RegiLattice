// RegiLattice.Core — Tweaks/TerminalServicesAdvPolicy.cs
// Terminal Services (RDP) Session Security Advanced Policy — Sprint 627.
// Category: "Terminal Services Adv Policy" | Slug: tssvcadv
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TerminalServicesAdvPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "tssvcadv-require-network-level-authentication",
            Label = "TS Adv: Require Network Level Authentication for RDP Connections",
            Category = "Terminal Services Adv Policy",
            Description = "Sets UserAuthentication=1 in Terminal Services policy. Requires Network Level Authentication (NLA) for all Remote Desktop connections to this computer. NLA forces the connecting client to authenticate using their domain credentials before the full Windows logon screen is presented — if NLA fails, no graphical session or memory buffering occurs. " +
                "Without NLA, Remote Desktop presents a full Windows logon screen to any network client that completes the TLS handshake. This graphical pre-authentication screen consumes significant server-side memory and CPU for rendering, is vulnerable to credential brute-force attacks against the visible login prompt, and is exploitable via session pre-authentication logic flaws (BlueKeep/DejaBlue were pre-NLA vulnerabilities). With NLA, unauthenticated clients never reach the graphical login stage — the server rejects unauthenticated connections at the NTLMSSP or Kerberos layer before any session rendering occurs.",
            Tags = ["ts-adv", "rdp", "nla", "network-level-authentication", "bluekeep", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "NLA required for all RDP; unauthenticated clients rejected before graphical session. Requires NLA-capable clients (Windows Vista+, all modern RDP clients).",
            ApplyOps = [RegOp.SetDword(Key, "UserAuthentication", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "UserAuthentication")],
            DetectOps = [RegOp.CheckDword(Key, "UserAuthentication", 1)],
        },
        new TweakDef
        {
            Id = "tssvcadv-require-tls-security-layer",
            Label = "TS Adv: Enforce TLS Security Layer for RDP Session Encryption",
            Category = "Terminal Services Adv Policy",
            Description = "Sets SecurityLayer=2 in Terminal Services policy. Forces RDP to use TLS 1.2+ for the transport security layer of all Remote Desktop connections, replacing the legacy RDP Security Layer (SecurityLayer=0) and Negotiate mode (SecurityLayer=1). TLS authentication requires the server to present a valid certificate, providing mutual authentication and preventing connection to a man-in-the-middle rogue RDP server. " +
                "The legacy RDP Security Layer uses RC4-128 encryption proprietary to the RDP protocol. It provides no server identity verification — a client connecting to a rogue RDP endpoint (via DNS poisoning or BGP hijack) has no mechanism to verify they are connected to the legitimate server. With SecurityLayer=2 (SSL/TLS), the server presents an X.509 certificate that the client validates against a trusted CA. An RDP certificate pinned to the domain CA ensures that a man-in-the-middle attack requires forging a domain-trusted certificate — a significantly harder attack than spoofing the legacy RDP protocol layer.",
            Tags = ["ts-adv", "rdp", "tls", "security-layer", "certificate", "mitm"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "TLS required for all RDP transport. Legacy RDP Security Layer disabled. Requires valid server certificate — auto-generated self-signed cert is accepted but domain cert preferred.",
            ApplyOps = [RegOp.SetDword(Key, "SecurityLayer", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "SecurityLayer")],
            DetectOps = [RegOp.CheckDword(Key, "SecurityLayer", 2)],
        },
        new TweakDef
        {
            Id = "tssvcadv-enforce-high-encryption",
            Label = "TS Adv: Enforce High (128-bit) RDP Session Encryption Minimum",
            Category = "Terminal Services Adv Policy",
            Description = "Sets MinEncryptionLevel=3 in Terminal Services policy (3 = High — 128-bit minimum). Requires all Remote Desktop sessions to use 128-bit or higher AES encryption. Setting 1 (Low) allows 56-bit DES; setting 2 (Client compatible) negotiates down to whatever the client supports. Setting 3 (High) rejects any client that cannot negotiate at least 128-bit AES encryption. " +
                "Low or medium encryption settings allow legacy RDP clients to negotiate DES-56 or RC4-40 encryption — both of which are trivially breakable with modern hardware within hours. Any network capture of a DES-56 encrypted RDP session can be decrypted offline. All modern RDP clients (Windows Vista+, FreeRDP 2.0+, macOS Microsoft Remote Desktop 10+, iOS/Android Microsoft Remote Desktop 10+) support 128-bit AES. Enforcing MinEncryptionLevel=3 rejects only RDP 4.0-era legacy clients created before 2001.",
            Tags = ["ts-adv", "rdp", "encryption", "128-bit", "aes", "minimum-encryption"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "128-bit minimum encryption enforced. DES-56 and RC4-40 clients rejected. All clients from 2002 onward are unaffected.",
            ApplyOps = [RegOp.SetDword(Key, "MinEncryptionLevel", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "MinEncryptionLevel")],
            DetectOps = [RegOp.CheckDword(Key, "MinEncryptionLevel", 3)],
        },
        new TweakDef
        {
            Id = "tssvcadv-limit-max-idle-time",
            Label = "TS Adv: Enforce 30-Minute Maximum RDP Session Idle Timeout",
            Category = "Terminal Services Adv Policy",
            Description = "Sets MaxIdleTime=1800000 in Terminal Services policy (milliseconds — 30 minutes). Disconnects idle RDP sessions after 30 minutes of inactivity. An idle session that remains connected indefinitely is a persistent attack surface: an attacker who gains network access can hijack an idle session without re-authenticating (using RDP session shadowing or session token replay if credentials are weak). Disconnecting idle sessions forces re-authentication and clears the session's memory state. " +
                "Session idle time limits are a defence-in-depth control against insider threat and unauthorised physical access scenarios. A developer who leaves an RDP session connected overnight to a production server creates an unmonitored privileged session on that server. If the developer's endpoint is compromised (via malware or physical access), the attacker can traverse the existing RDP session to reach the server without any new authentication event in the Security log. MaxIdleTime forces session termination, requiring fresh authentication and generating a new logon event.",
            Tags = ["ts-adv", "rdp", "idle-timeout", "session-management", "disconnect"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Sessions disconnecting after 30 minutes idle. Users should save work before stepping away from long RDP sessions.",
            ApplyOps = [RegOp.SetDword(Key, "MaxIdleTime", 1800000)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxIdleTime")],
            DetectOps = [RegOp.CheckDword(Key, "MaxIdleTime", 1800000)],
        },
        new TweakDef
        {
            Id = "tssvcadv-limit-max-connection-time",
            Label = "TS Adv: Enforce 8-Hour Maximum RDP Active Session Duration",
            Category = "Terminal Services Adv Policy",
            Description = "Sets MaxConnectionTime=28800000 in Terminal Services policy (milliseconds — 8 hours). Terminates active RDP sessions after 8 hours of continuous connection, requiring re-authentication. Without a maximum connection time, a session established on Monday morning could remain continuously active through the weekend. Long-lived sessions accumulate open handles, privileged process tokens, and memory that should be periodically refreshed. " +
                "Maximum connection time is distinct from idle timeout — an active session that the user is continuously using can still run indefinitely without this limit. On shared terminal servers (RDSH — Remote Desktop Session Host), long-lived sessions consume CALs (RDS Client Access Licences) and server resources. From a security perspective, a 72-hour active session may have been left running after the initiating user departed (e.g., automated script session that outlived human oversight), creating an orphaned privileged session that is no longer monitored.",
            Tags = ["ts-adv", "rdp", "max-session-time", "session-management", "rdsh"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Active sessions disconnected after 8 hours. Users running long overnight jobs via RDP should use scheduled tasks instead.",
            ApplyOps = [RegOp.SetDword(Key, "MaxConnectionTime", 28800000)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxConnectionTime")],
            DetectOps = [RegOp.CheckDword(Key, "MaxConnectionTime", 28800000)],
        },
        new TweakDef
        {
            Id = "tssvcadv-limit-reconnection-time",
            Label = "TS Adv: Enforce 15-Minute Disconnected Session Reconnection Limit",
            Category = "Terminal Services Adv Policy",
            Description = "Sets MaxDisconnectionTime=900000 in Terminal Services policy (milliseconds — 15 minutes). Terminates disconnected (not active) RDP sessions after 15 minutes. A disconnected session maintains the user's running processes, network connections, and open files on the RDS host, consuming server-side resources. More importantly, a disconnected session can be reconnected from any client with the corresponding user credentials — if a user's credentials are stolen, an attacker can reconnect to an existing privileged session in a disconnected state. " +
                "The distinction between disconnected and logged-off is critical: when a user closes the RDP window without logging off, the session becomes disconnected — processes continue running, network connections remain active, and the session is available for reconnection. From an attacker's perspective, a disconnected session can be reconnected via standard RDP credentials. Terminating disconnected sessions after 15 minutes ensures that sessions cannot be indefinitely parked in disconnected state waiting for credential theft.",
            Tags = ["ts-adv", "rdp", "disconnection-timeout", "session-management"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Disconnected sessions terminated after 15 minutes. Users must save work before disconnecting rather than leaving sessions in background.",
            ApplyOps = [RegOp.SetDword(Key, "MaxDisconnectionTime", 900000)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxDisconnectionTime")],
            DetectOps = [RegOp.CheckDword(Key, "MaxDisconnectionTime", 900000)],
        },
        new TweakDef
        {
            Id = "tssvcadv-disable-session-shadowing",
            Label = "TS Adv: Disable RDP Session Shadowing to Prevent Covert Observation",
            Category = "Terminal Services Adv Policy",
            Description = "Sets Shadow=4 in Terminal Services policy (4 = No remote control — shadowing disabled). Disables the RDP session shadowing feature that allows administrators to view or interact with another user's Remote Desktop session. While useful for helpdesk support, session shadowing creates a backdoor for privileged observation without the target user's knowledge on systems configured with Shadow=2 (Full control without user permission). " +
                "Session shadowing (Remote Control in older RDP documentation) with Shadow=2 allows domain administrators to take full control of any active user session without generating a visible prompt to the session user. From a data privacy perspective, this means an administrator can silently observe and control everything a user types, views, or sends — including personal passwords entered in non-SSO login forms, confidential documents open in the session, or personal communications. Setting Shadow=4 eliminates shadowing capability even for administrators; helpdesk support must use alternative methods (Teams screenshare, Intune remote assist).",
            Tags = ["ts-adv", "rdp", "session-shadowing", "remote-control", "privacy", "disable"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "RDP session shadowing disabled. Admins cannot view or control other users' sessions via RDP shadow. Helpdesk support requires alternative tools.",
            ApplyOps = [RegOp.SetDword(Key, "Shadow", 4)],
            RemoveOps = [RegOp.DeleteValue(Key, "Shadow")],
            DetectOps = [RegOp.CheckDword(Key, "Shadow", 4)],
        },
        new TweakDef
        {
            Id = "tssvcadv-enable-rpc-traffic-encryption",
            Label = "TS Adv: Enable RPC Traffic Encryption for Terminal Services Channel",
            Category = "Terminal Services Adv Policy",
            Description = "Sets fEncryptRPCTraffic=1 in Terminal Services policy. Enables encryption of all RPC (Remote Procedure Call) traffic on the Terminal Services management channel — the control plane channel used for session brokering, licensing, and management operations separate from the RDP data channel. " +
                "The Terminal Services RPC management channel is used for session reconnection brokering, RD Gateway authentication, RD Connection Broker negotiation, and licensing validation between RDS components. Without RPC traffic encryption on this channel, an attacker with network access to internal RDS infrastructure can intercept session broker negotiations, connection authorisation data, and licensing state exchanges. These are lower-frequency channels than the RDP data stream but contain session routing and authentication decisions that could be manipulated to redirect sessions or suppress licensing enforcement.",
            Tags = ["ts-adv", "rdp", "rpc-encryption", "session-broker", "control-plane"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "RDS control channel RPC traffic encrypted. No user-visible impact; applies to RDS infrastructure communication.",
            ApplyOps = [RegOp.SetDword(Key, "fEncryptRPCTraffic", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "fEncryptRPCTraffic")],
            DetectOps = [RegOp.CheckDword(Key, "fEncryptRPCTraffic", 1)],
        },
        new TweakDef
        {
            Id = "tssvcadv-enable-keepalive-connection",
            Label = "TS Adv: Enable RDP Keep-Alive to Detect and Clean Up Stale Sessions",
            Category = "Terminal Services Adv Policy",
            Description = "Sets KeepAliveEnable=1 and KeepAliveInterval=1 in Terminal Services policy. Enables the RDP keep-alive mechanism to send periodic keep-alive probes (every 1 minute) to detect stale or dead RDP sessions. When a client disappears abruptly without a graceful disconnect (network failure, power outage, crash), the server-side session remains in a connected/active state consuming resources until the TCP timeout expires — which can be hours on some network stacks. " +
                "Stale sessions occupying Connected status are undetectable via casual inspection (they appear active) but are abandoned by their client. On RDSH (Remote Desktop Session Host) deployments that license per-concurrent-session, stale sessions consume RDS CALs. More critically, a stale session with an open command prompt or elevated shell is an exploitable privileged session — if an attacker can send RST packets to the genuine client's TCP stream and then replay the session token, they may be able to inherit the session state.",
            Tags = ["ts-adv", "rdp", "keep-alive", "stale-session", "session-cleanup"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Keep-alive probes every 1 minute; stale/dead sessions cleaned up promptly instead of lingering for hours consuming resources.",
            ApplyOps = [RegOp.SetDword(Key, "KeepAliveEnable", 1), RegOp.SetDword(Key, "KeepAliveInterval", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "KeepAliveEnable"), RegOp.DeleteValue(Key, "KeepAliveInterval")],
            DetectOps = [RegOp.CheckDword(Key, "KeepAliveEnable", 1)],
        },
        new TweakDef
        {
            Id = "tssvcadv-disable-clipboard-redirection",
            Label = "TS Adv: Disable RDP Clipboard Redirection to Prevent Data Exfiltration",
            Category = "Terminal Services Adv Policy",
            Description = "Sets DisableClipboardRedirection=1 in Terminal Services policy. Disables the clipboard channel in RDP sessions, preventing clients from copying text or files from their local clipboard into the RDP session (or vice versa). Clipboard redirection is a primary data exfiltration vector: an attacker with a compromised client endpoint can paste data from a server-side RDP session directly to a local application, bypassing DLP controls that monitor local clipboard activity. " +
                "In high-security environments (PCI DSS, HIPAA, financial trading floors), users operating on sensitive servers via RDP must not be able to copy sensitive data from the server to their client workstation. Clipboard redirection creates a bidirectional unmonitored channel between the server (where sensitive data resides) and the client workstation (which may be less hardened and have internet access). Eliminating clipboard redirection forces users to use proper documented data transfer mechanisms (shared drives with audit logging, email with DLP scanning) rather than direct clipboard copy.",
            Tags = ["ts-adv", "rdp", "clipboard", "data-exfiltration", "dlp", "disable-redirection"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Clipboard copy-paste between RDP client and server blocked. Users cannot paste passwords or copy data across the RDP session boundary.",
            ApplyOps = [RegOp.SetDword(Key, "DisableClipboardRedirection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardRedirection")],
            DetectOps = [RegOp.CheckDword(Key, "DisableClipboardRedirection", 1)],
        },
    ];
}
