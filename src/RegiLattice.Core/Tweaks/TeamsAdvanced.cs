// RegiLattice.Core — Tweaks/TeamsAdvanced.cs
// Microsoft Teams advanced admin policy settings (Sprint 78).
// Slug "teams" — GPO-level policy registry keys under
// HKLM\SOFTWARE\Policies\Microsoft\MicrosoftTeams.
// All tweaks are CorpSafe = true (these are system policy keys).
// Distinct from Communication.cs which covers per-user app autostart/GPU settings.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TeamsAdvanced
{
    private const string Policy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MicrosoftTeams";
    private const string PolicyUsers = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "teams-disable-meeting-recording",
            Label = "Disable Teams Meeting Recording",
            Category = "Microsoft Teams",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["teams", "meeting", "recording", "privacy", "policy"],
            Description =
                "Prevents users from recording Microsoft Teams meetings via GPO policy. "
                + "Recordings will be disabled for all meetings, including calls and channel meetings.",
            ApplyOps = [RegOp.SetDword(Policy, "AllowMeetingRecording", 0)],
            RemoveOps = [RegOp.DeleteValue(Policy, "AllowMeetingRecording")],
            DetectOps = [RegOp.CheckDword(Policy, "AllowMeetingRecording", 0)],
        },
        new TweakDef
        {
            Id = "teams-disable-anonymous-join",
            Label = "Disable Anonymous Meeting Join",
            Category = "Microsoft Teams",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["teams", "meeting", "anonymous", "security", "policy"],
            Description =
                "Prevents anonymous (unauthenticated) users from joining Teams meetings. "
                + "All participants must sign in with an authenticated account.",
            ApplyOps = [RegOp.SetDword(Policy, "AllowAnonymousUsersToJoinMeeting", 0)],
            RemoveOps = [RegOp.DeleteValue(Policy, "AllowAnonymousUsersToJoinMeeting")],
            DetectOps = [RegOp.CheckDword(Policy, "AllowAnonymousUsersToJoinMeeting", 0)],
        },
        new TweakDef
        {
            Id = "teams-disable-anon-start-meeting",
            Label = "Prevent Anonymous Users from Starting Meetings",
            Category = "Microsoft Teams",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["teams", "meeting", "anonymous", "security", "policy"],
            Description =
                "Prevents anonymous (unauthenticated) users from starting or initiating "
                + "Teams meetings without an authenticated organizer present.",
            ApplyOps = [RegOp.SetDword(Policy, "AllowAnonymousUsersToStartMeeting", 0)],
            RemoveOps = [RegOp.DeleteValue(Policy, "AllowAnonymousUsersToStartMeeting")],
            DetectOps = [RegOp.CheckDword(Policy, "AllowAnonymousUsersToStartMeeting", 0)],
        },
        new TweakDef
        {
            Id = "teams-disable-giphy",
            Label = "Disable Giphy in Teams Chat",
            Category = "Microsoft Teams",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["teams", "chat", "giphy", "content", "policy"],
            Description =
                "Disables the Giphy animated GIF integration in Teams chat via policy. "
                + "Reduces bandwidth usage and enforces professional communication standards.",
            ApplyOps = [RegOp.SetDword(Policy, "AllowGiphy", 0)],
            RemoveOps = [RegOp.DeleteValue(Policy, "AllowGiphy")],
            DetectOps = [RegOp.CheckDword(Policy, "AllowGiphy", 0)],
        },
        new TweakDef
        {
            Id = "teams-disable-stickers",
            Label = "Disable Stickers in Teams",
            Category = "Microsoft Teams",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["teams", "chat", "stickers", "content", "policy"],
            Description =
                "Disables the sticker tab and sticker sharing in Teams chat via policy. "
                + "Enforces professional communication in business environments.",
            ApplyOps = [RegOp.SetDword(Policy, "AllowStickers", 0)],
            RemoveOps = [RegOp.DeleteValue(Policy, "AllowStickers")],
            DetectOps = [RegOp.CheckDword(Policy, "AllowStickers", 0)],
        },
        new TweakDef
        {
            Id = "teams-disable-memes",
            Label = "Disable Meme Images in Teams",
            Category = "Microsoft Teams",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["teams", "chat", "memes", "content", "policy"],
            Description =
                "Disables the meme/image captioning tab in Teams chat via policy. "
                + "Prevents custom meme creation and sharing in workplace communications.",
            ApplyOps = [RegOp.SetDword(Policy, "AllowMemes", 0)],
            RemoveOps = [RegOp.DeleteValue(Policy, "AllowMemes")],
            DetectOps = [RegOp.CheckDword(Policy, "AllowMemes", 0)],
        },
        new TweakDef
        {
            Id = "teams-disable-discover-private-channels",
            Label = "Hide Private Channels from Search",
            Category = "Microsoft Teams",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["teams", "channels", "privacy", "security", "policy"],
            Description =
                "Prevents users from discovering private Team channels via the Teams "
                + "search interface. Only explicit channel members can see private channels.",
            ApplyOps = [RegOp.SetDword(Policy, "AllowDiscoverPrivateChannels", 0)],
            RemoveOps = [RegOp.DeleteValue(Policy, "AllowDiscoverPrivateChannels")],
            DetectOps = [RegOp.CheckDword(Policy, "AllowDiscoverPrivateChannels", 0)],
        },
        new TweakDef
        {
            Id = "teams-disable-org-wide-team-creation",
            Label = "Restrict Org-Wide Team Creation",
            Category = "Microsoft Teams",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["teams", "team", "creation", "governance", "policy"],
            Description =
                "Restricts creation of org-wide Teams (visible to all users in the tenant). "
                + "Only Global Administrators can create org-wide teams.",
            ApplyOps = [RegOp.SetDword(Policy, "AllowOrgWideTeamCreation", 0)],
            RemoveOps = [RegOp.DeleteValue(Policy, "AllowOrgWideTeamCreation")],
            DetectOps = [RegOp.CheckDword(Policy, "AllowOrgWideTeamCreation", 0)],
        },
        new TweakDef
        {
            Id = "teams-set-giphy-rating-strict",
            Label = "Set Teams Giphy Content to Strict (G-Rated)",
            Category = "Microsoft Teams",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["teams", "giphy", "content", "safe", "policy"],
            Description =
                "Sets the Giphy content rating to Strict (G-rated only) in Teams chat. "
                + "Value 1 = Strict. When Giphy is enabled, only family-friendly GIFs are shown.",
            ApplyOps = [RegOp.SetDword(Policy, "AllowGiphyRating", 1)],
            RemoveOps = [RegOp.DeleteValue(Policy, "AllowGiphyRating")],
            DetectOps = [RegOp.CheckDword(Policy, "AllowGiphyRating", 1)],
        },
        new TweakDef
        {
            Id = "teams-disable-private-calling",
            Label = "Disable Teams Private Calling",
            Category = "Microsoft Teams",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["teams", "calling", "private", "policy"],
            Description =
                "Disables peer-to-peer private calling in Microsoft Teams via policy. "
                + "Users can still participate in Team or channel calls, but cannot initiate "
                + "direct 1:1 PSTN or VoIP calls.",
            ApplyOps = [RegOp.SetDword(Policy, "AllowPrivateCalling", 0)],
            RemoveOps = [RegOp.DeleteValue(Policy, "AllowPrivateCalling")],
            DetectOps = [RegOp.CheckDword(Policy, "AllowPrivateCalling", 0)],
        },
    ];
}
