// RegiLattice.Core — Tweaks/TeamsCallingPolicy.cs
// Microsoft Teams telephony and calling policy controls (Sprint 594).
// Category: "Teams Calling Policy" | Slug: tmscall
// Key: HKLM\SOFTWARE\Policies\Microsoft\MicrosoftTeams
// All tweaks are CorpSafe = true (Group Policy path).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TeamsCallingPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftTeams";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "tmscall-enable-voicemail-routing",
            Label = "Teams Calling: Enable Voicemail Routing for Missed Calls",
            Category = "Teams Calling Policy",
            Description = "Sets AllowVoicemail=1 in MicrosoftTeams policy. Enables voicemail as a call routing target when a call is unanswered. " +
                "Without voicemail routing, unanswered calls drop silently. This is required for Teams Phone users to have a compliant missed-call record and supports call centre audit trails.",
            Tags = ["teams", "calling", "voicemail", "routing", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enables voicemail for Teams users; unanswered calls are redirected to cloud voicemail instead of dropping.",
            ApplyOps = [RegOp.SetDword(Key, "AllowVoicemail", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowVoicemail")],
            DetectOps = [RegOp.CheckDword(Key, "AllowVoicemail", 1)],
        },
        new TweakDef
        {
            Id = "tmscall-enable-shared-line-delegation",
            Label = "Teams Calling: Enable Boss-Delegate Shared Line Appearance",
            Category = "Teams Calling Policy",
            Description = "Sets AllowDelegation=1 in MicrosoftTeams policy. Enables shared line appearance (SLA) so a delegate (admin assistant) can answer calls on behalf of an executive. " +
                "Without this, only direct Teams-to-Teams calls are supported and PSTN delegation is not available to non-admin accounts.",
            Tags = ["teams", "calling", "delegation", "sla", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Enables shared line appearance delegation for Teams Phone; executive-admin call handling workflows.",
            ApplyOps = [RegOp.SetDword(Key, "AllowDelegation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowDelegation")],
            DetectOps = [RegOp.CheckDword(Key, "AllowDelegation", 1)],
        },
        new TweakDef
        {
            Id = "tmscall-enable-call-park-feature",
            Label = "Teams Calling: Enable Call Park and Retrieve",
            Category = "Teams Calling Policy",
            Description = "Sets AllowCallPark=1 in MicrosoftTeams policy. Enables the call park feature so users can place an active call on hold and retrieve it from any Teams endpoint. " +
                "Widely used in healthcare and hospitality environments where calls must be handed off between staff without transferring or dropping.",
            Tags = ["teams", "calling", "call-park", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enables call park/retrieve; supports multi-device call handoff scenarios in enterprise environments.",
            ApplyOps = [RegOp.SetDword(Key, "AllowCallPark", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCallPark")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCallPark", 1)],
        },
        new TweakDef
        {
            Id = "tmscall-disable-external-call-forwarding",
            Label = "Teams Calling: Block Call Forwarding to External Numbers",
            Category = "Teams Calling Policy",
            Description = "Sets AllowCallForwardingToExternalNumbers=0 in MicrosoftTeams policy. Prevents users from forwarding Teams calls to external PSTN numbers. " +
                "This reduces the risk of toll fraud and data exfiltration through forwarded calls, which is a common security concern in regulated financial and legal organisations.",
            Tags = ["teams", "calling", "forwarding", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks PSTN call forwarding; prevents toll-fraud and out-of-band data leakage via forwarded calls.",
            ApplyOps = [RegOp.SetDword(Key, "AllowCallForwardingToExternalNumbers", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCallForwardingToExternalNumbers")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCallForwardingToExternalNumbers", 0)],
        },
        new TweakDef
        {
            Id = "tmscall-enable-busy-on-busy",
            Label = "Teams Calling: Enable Busy-on-Busy for Active Calls",
            Category = "Teams Calling Policy",
            Description = "Sets BusyOnBusyEnabled=1 in MicrosoftTeams policy. When a user is already in a Teams call, additional incoming PSTN calls will hear a busy signal instead of ringing through or diverting to voicemail. " +
                "This gives callers a clear signal and prevents voicemail from filling up during back-to-back meetings.",
            Tags = ["teams", "calling", "busy", "pstn", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Callers hear busy signal when user is already on a call; reduces voicemail clutter.",
            ApplyOps = [RegOp.SetDword(Key, "BusyOnBusyEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BusyOnBusyEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "BusyOnBusyEnabled", 1)],
        },
        new TweakDef
        {
            Id = "tmscall-disable-simultaneous-ring-external",
            Label = "Teams Calling: Block Simultaneous Ring to External Numbers",
            Category = "Teams Calling Policy",
            Description = "Sets AllowSimultaneousRingToExternalNumbers=0 in MicrosoftTeams policy. Prevents Teams calls from simultaneously ringing external PSTN phone numbers. " +
                "Similar to blocking external forwarding, this eliminates a toll-fraud vector and prevents users from bypassing corporate monitoring by routing calls to personal mobiles.",
            Tags = ["teams", "calling", "simultaneous-ring", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks simultaneous ring on external numbers; reduces toll fraud risk and enforces call recording compliance.",
            ApplyOps = [RegOp.SetDword(Key, "AllowSimultaneousRingToExternalNumbers", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowSimultaneousRingToExternalNumbers")],
            DetectOps = [RegOp.CheckDword(Key, "AllowSimultaneousRingToExternalNumbers", 0)],
        },
        new TweakDef
        {
            Id = "tmscall-enable-call-transcription",
            Label = "Teams Calling: Enable Automatic Call Transcription",
            Category = "Teams Calling Policy",
            Description = "Sets AllowTranscriptionForCalling=1 in MicrosoftTeams policy. Enables automatic real-time transcription for Teams PSTN and VoIP calls. " +
                "Transcripts are stored in Teams call history and can be reviewed for accessibility, compliance, and knowledge capture without requiring a call recorder.",
            Tags = ["teams", "calling", "transcription", "accessibility", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Enables real-time call transcription; supports accessibility, compliance review, and action-item capture.",
            ApplyOps = [RegOp.SetDword(Key, "AllowTranscriptionForCalling", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowTranscriptionForCalling")],
            DetectOps = [RegOp.CheckDword(Key, "AllowTranscriptionForCalling", 1)],
        },
        new TweakDef
        {
            Id = "tmscall-disable-external-call-transfer",
            Label = "Teams Calling: Block Blind Transfer to External PSTN Numbers",
            Category = "Teams Calling Policy",
            Description = "Sets AllowTransferToExternalNumbers=0 in MicrosoftTeams policy. Prevents users from blind-transferring active Teams calls to external PSTN telephone numbers. " +
                "Complements the external forwarding block. Call transfers bypass recording infrastructure, making this a key control for MiFID II and financial sector compliance.",
            Tags = ["teams", "calling", "transfer", "compliance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks PSTN blind transfers; enforces call recording compliance under MiFID II and similar regulations.",
            ApplyOps = [RegOp.SetDword(Key, "AllowTransferToExternalNumbers", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowTransferToExternalNumbers")],
            DetectOps = [RegOp.CheckDword(Key, "AllowTransferToExternalNumbers", 0)],
        },
        new TweakDef
        {
            Id = "tmscall-enable-caller-id-override-policy",
            Label = "Teams Calling: Enable Caller ID Override Policy",
            Category = "Teams Calling Policy",
            Description = "Sets CallerIdPolicyEnabled=1 in MicrosoftTeams policy. Allows IT to override the outbound caller ID for Teams PSTN calls. " +
                "This is needed when multiple departments share a single external number and calls should display a generic department DID rather than the individual user's direct number. Also required to block presentation of personal mobile numbers to external parties.",
            Tags = ["teams", "calling", "caller-id", "pstn", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enables IT control over outbound caller ID; privacy protection and department-level DID management.",
            ApplyOps = [RegOp.SetDword(Key, "CallerIdPolicyEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "CallerIdPolicyEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "CallerIdPolicyEnabled", 1)],
        },
        new TweakDef
        {
            Id = "tmscall-enable-music-on-hold",
            Label = "Teams Calling: Enable Music on Hold for PSTN Calls",
            Category = "Teams Calling Policy",
            Description = "Sets AllowMusicOnHold=1 in MicrosoftTeams policy. Plays hold music to external PSTN callers when a Teams user places them on hold. " +
                "Without this, external callers hear silence during hold, leading to call abandonment. Music on hold is a standard business telephony expectation for enterprise PSTN deployments.",
            Tags = ["teams", "calling", "hold", "pstn", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Plays hold music to external callers on hold; reduces call abandonment; improves perceived responsiveness.",
            ApplyOps = [RegOp.SetDword(Key, "AllowMusicOnHold", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowMusicOnHold")],
            DetectOps = [RegOp.CheckDword(Key, "AllowMusicOnHold", 1)],
        },
    ];
}
