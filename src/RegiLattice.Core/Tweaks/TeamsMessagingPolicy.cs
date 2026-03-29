// RegiLattice.Core — Tweaks/TeamsMessagingPolicy.cs
// Microsoft Teams Messaging and Chat Policy — Sprint 593.
// Configures Teams chat retention, external messaging controls,
// message editing/deletion limits, and priority notifications.
// Category: "Teams Messaging Policy" | Slug: tmsmsg
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Teams

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TeamsMessagingPolicy
{
    private const string TeamsPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "tmsmsg-disable-external-chat",
                Label = "Teams Messaging: Block Chat Messages with External (Federated) Teams Users",
                Category = "Teams Messaging Policy",
                Description =
                    "Sets AllowExternalChat=0 in the Teams policy key. Prevents Teams users from initiating or receiving chat messages with external Teams users from other organisations (federation). Teams federation allows users in different Microsoft 365 tenants to message each other directly — this capability creates a potential data exfiltration channel where sensitive information can be transmitted to non-corporate Teams users via chat. In high-security environments, all external collaboration should go through approved collaboration channels with proper DLP controls rather than open federation.",
                Tags = ["teams", "external-chat", "federation", "dlp", "external-messaging"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "External (federated) Teams messaging blocked. Users cannot send or receive direct chat messages from external Teams users. Existing external chat conversations are still visible in history but new messages are blocked. Business processes that rely on Teams federation with partners, contractors, or suppliers should be migrated to authorised Guest Access (in-tenant channel) before deploying.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "AllowExternalChat", 0)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "AllowExternalChat")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "AllowExternalChat", 0)],
            },
            new TweakDef
            {
                Id = "tmsmsg-enable-message-immutability",
                Label = "Teams Messaging: Enable Message Immutability (Prevent User Delete/Edit of Sent Messages)",
                Category = "Teams Messaging Policy",
                Description =
                    "Sets AllowUserDeleteChat=0 and AllowUserEditMessage=0 in the Teams policy key. Prevents users from deleting or editing messages after they have been sent in Teams chat and channels. Message immutability is required in regulated industries — in financial services, legal, and healthcare, chat communications must be retained unaltered as a complete record. Allowing message deletion or editing enables users to delete incriminating or non-compliant messages after the fact. This policy ensures that the full chat history is preserved for eDiscovery and compliance review.",
                Tags = ["teams", "message-immutability", "delete-message", "compliance", "ediscovery"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Users cannot delete or edit sent messages. Messages containing errors or sensitive information cannot be corrected or withdrawn. IT/compliance officers can use Teams content moderation tools to remove flagged content via admin centre. Inform users of this change before deploying — it has a significant impact on messaging behaviour.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "AllowUserDeleteChat", 0), RegOp.SetDword(TeamsPolicyKey, "AllowUserEditMessage", 0)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "AllowUserDeleteChat"), RegOp.DeleteValue(TeamsPolicyKey, "AllowUserEditMessage")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "AllowUserDeleteChat", 0), RegOp.CheckDword(TeamsPolicyKey, "AllowUserEditMessage", 0)],
            },
            new TweakDef
            {
                Id = "tmsmsg-disable-giphy-in-chat",
                Label = "Teams Messaging: Disable Giphy GIF Integration in Teams Chat",
                Category = "Teams Messaging Policy",
                Description =
                    "Sets AllowGiphy=0 in the Teams policy key. Disables the Giphy GIF search and insertion feature in Teams chat. The Giphy integration sends search queries to the Giphy CDN (external service) to retrieve GIF content. This creates an implicit data disclosure: search terms typed in the Teams chat GIF search box are transmitted to Giphy's servers. Additionally, GIF content retrieved from Giphy is subject to Giphy's content policies — in professional environments, inappropriate GIFs inserted in public channels can create a hostile work environment compliance risk.",
                Tags = ["teams", "giphy", "gif", "external-service", "content-moderation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote =
                    "Giphy GIF integration removed from the Teams chat toolbar. Users cannot search for or insert Giphy GIFs. Standard emoji and built-in Teams stickers are not affected. Custom sticker packs managed via Teams admin centre can still be enabled.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "AllowGiphy", 0)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "AllowGiphy")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "AllowGiphy", 0)],
            },
            new TweakDef
            {
                Id = "tmsmsg-disable-memes-in-chat",
                Label = "Teams Messaging: Disable Meme/Praise Card Creation in Teams Chat",
                Category = "Teams Messaging Policy",
                Description =
                    "Sets AllowMemes=0 combined with AllowPraise=0 in the Teams policy key. Disables the built-in meme editor (Meme Generator) and Praise badge cards in Teams chat. The meme generator allows users to create and send image-overlaid text memes in chat — content that may range from benign to potentially offensive or harassment-enabling. Praise cards are appreciation cards with badge icons. In risk-averse enterprise environments where all chat content is subject to legal hold and compliance review, meme content in corporate chat creates legal and policy exposure.",
                Tags = ["teams", "memes", "praise", "content-moderation", "enterprise-policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote =
                    "Meme editor and Praise cards disabled in Teams. No functional impact on work communications. If organisations have a culture initiative using Praise cards, this disables the feature. Consider the cultural impact before deploying in human-focused organisations that actively use Praise for employee recognition.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "AllowMemes", 0), RegOp.SetDword(TeamsPolicyKey, "AllowPraise", 0)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "AllowMemes"), RegOp.DeleteValue(TeamsPolicyKey, "AllowPraise")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "AllowMemes", 0), RegOp.CheckDword(TeamsPolicyKey, "AllowPraise", 0)],
            },
            new TweakDef
            {
                Id = "tmsmsg-enable-priority-notifications",
                Label = "Teams Messaging: Enable Priority (Urgent) Notifications with Repeated Alerts",
                Category = "Teams Messaging Policy",
                Description =
                    "Sets AllowPriorityMessages=1 in the Teams policy key. Enables the Priority Notifications feature in Teams — senders can mark a message as 'Urgent' which causes the notification to repeat every 2 minutes for 20 minutes until the recipient opens the message (or the notification expires). Priority notifications provide a mechanism for genuinely time-critical communications (on-call incidents, security alerts, physical emergency notifications) that need guaranteed attention within minutes. Without priority notifications, all messages are treated equally regardless of urgency.",
                Tags = ["teams", "priority-notifications", "urgent", "on-call", "incident-response"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Priority (Urgent) messaging enabled. Urgent messages create repeated notifications every 2 minutes for up to 20 minutes. Misuse (sending non-urgent messages as Urgent) can be disruptive. Consider applying a Teams messaging policy in the admin centre that limits who can send Urgent messages (e.g., only on-call and security teams).",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "AllowPriorityMessages", 1)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "AllowPriorityMessages")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "AllowPriorityMessages", 1)],
            },
            new TweakDef
            {
                Id = "tmsmsg-enable-read-receipts",
                Label = "Teams Messaging: Enable Read Receipts in Teams Chat (Sent/Read Indicators)",
                Category = "Teams Messaging Policy",
                Description =
                    "Sets AllowReadReceipts=1 in the Teams policy key. Enables message read receipts in Teams one-on-one chat and small group chats — senders can see which recipients have read their messages (tick mark under message). Read receipts improve communication efficiency by allowing senders to determine whether a recipient has seen a message without needing to ask 'did you see my message?'. This is particularly valuable for hybrid teams where asynchronous communication is common and message delivery confirmation improves workflow coordination.",
                Tags = ["teams", "read-receipts", "message-delivery", "async-communication", "confirmation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Read receipts enabled for Teams chat. Recipients who read a message will have their read status visible to the sender. In some work cultures, read receipts create pressure to respond immediately. Teams policy can be set to allow users to control whether they show read receipts individually via their Teams settings.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "AllowReadReceipts", 1)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "AllowReadReceipts")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "AllowReadReceipts", 1)],
            },
            new TweakDef
            {
                Id = "tmsmsg-disable-meeting-chat-during-meeting",
                Label = "Teams Messaging: Allow Meeting Chat Only During Meeting (Disable Chat After)",
                Category = "Teams Messaging Policy",
                Description =
                    "Sets AllowMeetingChat=1 (1 = enabled during meeting only) in the Teams policy key. Configures meeting chat to be available only during the meeting session. After the meeting ends, the chat thread closes and becomes read-only. Meeting chat threads that remain open post-meeting become informal communication channels where sensitive discussions from the meeting continue outside the meeting context, potentially without proper retention policies. Closing chat after the meeting ensures the meeting context is preserved in the recording/transcript rather than scattered across a chat thread.",
                Tags = ["teams", "meeting-chat", "post-meeting", "retention", "governance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote =
                    "Meeting chat available only during active meeting. After the meeting ends, the chat becomes read-only. Meeting participants can still access the full chat history. Post-meeting follow-up conversations should be directed to the team channel or a new chat thread.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "AllowMeetingChat", 1)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "AllowMeetingChat")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "AllowMeetingChat", 1)],
            },
            new TweakDef
            {
                Id = "tmsmsg-disable-teams-anonymous-join",
                Label = "Teams Messaging: Disable Anonymous User Join to Teams Meetings",
                Category = "Teams Messaging Policy",
                Description =
                    "Sets AllowAnonymousUsersToJoinMeeting=0 in the Teams policy key. Prevents anonymous (unauthenticated) users from joining Teams meetings hosted by this organisation. By default, Teams allows anyone with a meeting link to join without signing in — appearing as 'Joseph (Guest)' or similar. Anonymous join poses security risks: meeting links can be forwarded, posted publicly, or guessed, allowing unintended parties to eavesdrop on meetings. Requiring authentication ensures only intentionally invited users can participate.",
                Tags = ["teams", "anonymous-join", "meeting-security", "unauthenticated", "meeting-link"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Anonymous join disabled. External participants (vendors, clients, interview candidates) must sign in with a Microsoft account or Teams account to join meetings. Consider enabling the Teams lobby for external participants so authenticated external users wait for host approval before entering. Web-based Teams join still supported with Microsoft account sign-in.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "AllowAnonymousUsersToJoinMeeting", 0)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "AllowAnonymousUsersToJoinMeeting")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "AllowAnonymousUsersToJoinMeeting", 0)],
            },
            new TweakDef
            {
                Id = "tmsmsg-enable-supervised-chat",
                Label = "Teams Messaging: Enable Supervised Chat (Educator/Supervisor Oversight Mode)",
                Category = "Teams Messaging Policy",
                Description =
                    "Sets SupervisedChatEnabled=1 in the Teams policy key. Enables Supervised Chat mode for Teams — a Teams for Education feature that allows supervisors (teachers, managers) to monitor chat conversations between specific user groups. In supervised chat mode, restricted users (e.g., students, apprentices) can only initiate chats with supervisors — they cannot start direct chats with peers without a supervisor present. For non-education enterprises, supervised chat provides a management layer for compliance monitoring in sensitive departments (trading floors, customer service, healthcare).",
                Tags = ["teams", "supervised-chat", "education", "monitoring", "compliance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Supervised chat mode enabled. Restricted users must initiate chats through supervisors. This significantly restricts peer-to-peer messaging for the restricted user group. Primarily designed for Teams for Education (K-12). For enterprise, consider using Microsoft Purview Communication Compliance for passive monitoring rather than active supervision as it is less disruptive.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "SupervisedChatEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "SupervisedChatEnabled")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "SupervisedChatEnabled", 1)],
            },
            new TweakDef
            {
                Id = "tmsmsg-disable-teams-consumer-accounts",
                Label = "Teams Messaging: Block Teams (Free) Personal Consumer Account Chat Federation",
                Category = "Teams Messaging Policy",
                Description =
                    "Sets DisableConsumerFederation=1 in the Teams policy key. Blocks Teams Work accounts from messaging Teams Personal (Teams Free/consumer) account users. Microsoft introduced consumer-to-work Teams messaging in 2022 — corporate employees can chat with personal 'Teams Free' account holders. This creates a data governance gap: regulatory and compliance controls on Teams work accounts do not extend to the consumer federation path. Blocking consumer federation ensures that all Teams communications into and out of the organisation go through the governed work account channel.",
                Tags = ["teams", "consumer-federation", "teams-free", "data-governance", "dlp"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Teams work accounts cannot message Teams Free (personal) account users. Business processes that involve communicating with consumers or partners who use Teams Free must use an alternative channel (Teams Guest Access, email, or external sharing). Consumer federation was disabled by default in enterprise tenants prior to 2023.",
                ApplyOps = [RegOp.SetDword(TeamsPolicyKey, "DisableConsumerFederation", 1)],
                RemoveOps = [RegOp.DeleteValue(TeamsPolicyKey, "DisableConsumerFederation")],
                DetectOps = [RegOp.CheckDword(TeamsPolicyKey, "DisableConsumerFederation", 1)],
            },
        ];
}
