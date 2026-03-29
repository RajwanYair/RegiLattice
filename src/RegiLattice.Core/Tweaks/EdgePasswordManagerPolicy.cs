// RegiLattice.Core — Tweaks/EdgePasswordManagerPolicy.cs
// Microsoft Edge built-in password manager and credential security Group Policy controls (Sprint 613).
// Category: "Edge Password Manager Policy" | Slug: edgepwm
// Key: HKLM\SOFTWARE\Policies\Microsoft\Edge

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EdgePasswordManagerPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "edgepwm-disable-built-in-password-manager",
            Label = "Edge Password Manager: Disable Edge's Built-In Password Save Prompts",
            Category = "Edge Password Manager Policy",
            Description = "Sets PasswordManagerEnabled=0 in Edge policy. Disables the Edge built-in password manager's offer to save new credentials, preventing Edge from storing work account passwords in the browser's local credential store. " +
                "The Edge password manager stores credentials in a file encrypted with the Windows DPAPI (Data Protection API) encryption key, which is bound to the user's Windows login credentials. If an unprivileged process on the same machine gains access to the browser's LocalState file (e.g., via a malicious script running as the same user), it can request DPAPI decryption of the stored passwords without any additional authentication, recovering plaintext credentials for all saved sites.",
            Tags = ["edge", "password-manager", "credential-storage", "dpapi"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Edge will not offer to save passwords; users must use an approved enterprise password manager instead.",
            ApplyOps = [RegOp.SetDword(Key, "PasswordManagerEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "PasswordManagerEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "PasswordManagerEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edgepwm-disable-password-reveal-button",
            Label = "Edge Password Manager: Disable Show-Password Reveal Button in Input Fields",
            Category = "Edge Password Manager Policy",
            Description = "Sets PasswordRevealEnabled=0 in Edge policy. Removes the 'eye' icon reveal button that appears in password input fields in Edge, preventing users from visually revealing the entered password text in a password field. " +
                "The password reveal button, while intended for usability, is a security risk in shared workspace environments: a screen-sharing session (Teams, Zoom, remote support) that shows the browser window while a user is entering a password could inadvertently reveal the masked password text if the user or a collaborator clicks the reveal button. Disabling the reveal button removes this inadvertent exposure channel.",
            Tags = ["edge", "password", "reveal", "screen-sharing", "shoulder-surfing"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Password reveal button removed from Edge password fields; entered passwords cannot be un-masked by button click.",
            ApplyOps = [RegOp.SetDword(Key, "PasswordRevealEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "PasswordRevealEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "PasswordRevealEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edgepwm-disable-primary-password-bypass",
            Label = "Edge Password Manager: Require Primary Password (Master Password) Protection",
            Category = "Edge Password Manager Policy",
            Description = "Sets PrimaryPasswordSetting=2 in Edge policy (value 2 = Required). Requires users to set and enter a primary password (master password) to decrypt and view any credential saved in the Edge password manager, adding an additional authentication factor before stored passwords are revealed. " +
                "Without a primary password, any process running as the current user — including malware, malicious scripts, and other browser extensions — can access the Edge password manager's stored credentials via the Edge DevTools protocol or the profile's Cookies/Login Data files without additional authentication. A primary password means the DPAPI-encrypted store has a second layer of protection beyond just the Windows session key.",
            Tags = ["edge", "password-manager", "primary-password", "master-password", "mfa"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Primary password required to view saved Edge credentials; extra authentication layer beyond Windows session.",
            ApplyOps = [RegOp.SetDword(Key, "PrimaryPasswordSetting", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "PrimaryPasswordSetting")],
            DetectOps = [RegOp.CheckDword(Key, "PrimaryPasswordSetting", 2)],
        },
        new TweakDef
        {
            Id = "edgepwm-disable-password-autocomplete-on-login-forms",
            Label = "Edge Password Manager: Disable AutoComplete on Bank and Sensitive Login Forms",
            Category = "Edge Password Manager Policy",
            Description = "Sets AutofillEnabledOnSecureForms=0 in Edge policy. Disables Edge's autofill feature specifically on forms that have autocomplete='off' or that are classified as high-security by Edge's form classifier (banking portals, credential re-authentication forms). " +
                "Bank and financial institution login forms explicitly set autocomplete='off' as a security directive. Edge's autocomplete override bypasses this signal and fills stored credentials anyway. On kiosk-style machines where sessions may not be fully cleared between users, prefilled credential forms can expose credentials from previous sessions.",
            Tags = ["edge", "autofill", "autocomplete", "banking", "sensitive-forms"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Edge autofill disabled on autocomplete=off and high-security forms; users must type credentials manually.",
            ApplyOps = [RegOp.SetDword(Key, "AutofillEnabledOnSecureForms", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AutofillEnabledOnSecureForms")],
            DetectOps = [RegOp.CheckDword(Key, "AutofillEnabledOnSecureForms", 0)],
        },
        new TweakDef
        {
            Id = "edgepwm-enable-password-strength-monitor",
            Label = "Edge Password Manager: Enable Weak Password Detection and Warning",
            Category = "Edge Password Manager Policy",
            Description = "Sets PasswordMonitorAllowed=1 in Edge policy. Enables Edge's password strength monitor to warn users when a saved password is detected to be weak (short, common, dictionary word) or has been found in public credential breach databases via the Microsoft breach database API. " +
                "Employees who reuse simple passwords across work and personal accounts are a primary initial access vector for credential-stuffing attacks. Edge's breach monitor checks saved passwords against a k-anonymity hash database of compromised credentials and surfaces warnings without transmitting the full password hash to Microsoft. Enabling this monitor provides passive security hygiene enforcement without requiring additional tooling.",
            Tags = ["edge", "password", "breach", "weak-password", "hibp"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Edge warns when saved passwords are weak or found in breach databases; passive credential hygiene enforcement.",
            ApplyOps = [RegOp.SetDword(Key, "PasswordMonitorAllowed", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PasswordMonitorAllowed")],
            DetectOps = [RegOp.CheckDword(Key, "PasswordMonitorAllowed", 1)],
        },
        new TweakDef
        {
            Id = "edgepwm-disable-web-credential-import",
            Label = "Edge Password Manager: Block Importing Passwords from Other Browsers or Files",
            Category = "Edge Password Manager Policy",
            Description = "Sets ImportSavedPasswordsAllowed=0 in Edge policy. Disables the Edge feature that allows users to import saved passwords from other browsers (Chrome, Firefox, IE) or from CSV password export files into the Edge password manager. " +
                "Password imports are a common initial vector for credential disclosure: a social engineering attack can cause a user to import a maliciously-modified password CSV that establishes fake entries for internal site URLs, enabling future credential phishing. Additionally, mass-importing passwords from a less-secure browser or a cleartext CSV file into Edge aggregates credentials into a single easily-targetable store.",
            Tags = ["edge", "password", "import", "social-engineering", "credential"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Password imports blocked in Edge; existing credentials must be added individually, not bulk-imported.",
            ApplyOps = [RegOp.SetDword(Key, "ImportSavedPasswordsAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "ImportSavedPasswordsAllowed")],
            DetectOps = [RegOp.CheckDword(Key, "ImportSavedPasswordsAllowed", 0)],
        },
        new TweakDef
        {
            Id = "edgepwm-disable-password-sharing-between-devices",
            Label = "Edge Password Manager: Block Password Sync to Other Devices via Edge",
            Category = "Edge Password Manager Policy",
            Description = "Sets PasswordExportAllowed=0 in Edge policy. Disables Edge's password export function that allows users to download all their saved Edge passwords to a cleartext CSV file for transfer to another device or password manager. " +
                "The Edge 'Export passwords' feature creates a comma-separated file with site URL, username, and cleartext password for every saved credential. This file, once exported to the Downloads folder, is not protected — it can be exfiltrated via email, USB, or cloud storage by any process with filesystem access. A single click exports the entire Edge credential store to a cleartext file with no additional authentication required.",
            Tags = ["edge", "password", "export", "cleartext", "exfiltration"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Password CSV export disabled in Edge; entire credential store cannot be dumped to a cleartext file.",
            ApplyOps = [RegOp.SetDword(Key, "PasswordExportAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "PasswordExportAllowed")],
            DetectOps = [RegOp.CheckDword(Key, "PasswordExportAllowed", 0)],
        },
        new TweakDef
        {
            Id = "edgepwm-block-third-party-password-manager-override",
            Label = "Edge Password Manager: Prevent Third-Party Extensions Overriding Password Fields",
            Category = "Edge Password Manager Policy",
            Description = "Sets AllowPasswordGenerationEnabled=0 in Edge policy. Disables Edge's own password generation feature and prevents third-party password manager browser extensions from having elevated API access to password input field values in Edge. " +
                "Malicious browser extensions that present themselves as password managers request the 'all_urls' and 'tabs' permissions, which allows them to read the contents of every form field (including password fields) on every page. Limiting password field API access reduces the exposure that a compromised or malicious password manager extension has to credentials being typed into pages.",
            Tags = ["edge", "extension", "password-field", "api-access", "malicious-extension"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Edge password generation disabled; extension-level password field access controlled via policy.",
            ApplyOps = [RegOp.SetDword(Key, "AllowPasswordGenerationEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowPasswordGenerationEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "AllowPasswordGenerationEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edgepwm-disable-filled-credentials-auto-sign-in",
            Label = "Edge Password Manager: Disable Auto Sign-In with Saved Credentials",
            Category = "Edge Password Manager Policy",
            Description = "Sets AutoSignInEnabled=0 in Edge policy. Prevents Edge from automatically submitting the login form without user interaction when it detects a single saved credential for a visited site, requiring the user to actively click 'Sign in' even when credentials are pre-filled. " +
                "Automatic sign-in means that visiting a work sign-in page immediately authenticates the user and establishes an authenticated session — without the user actively choosing to authenticate. If the user's Windows session has been taken over (e.g., via a remote desktop hijack or accessibility API automation), auto sign-in enables an attacker to silently authenticate to all internal web apps without the user's knowledge just by navigating to login pages.",
            Tags = ["edge", "auto-sign-in", "credential", "session-hijack"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Edge auto-sign-in disabled; user must actively submit sign-in form even when credentials are pre-filled.",
            ApplyOps = [RegOp.SetDword(Key, "AutoSignInEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AutoSignInEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "AutoSignInEnabled", 0)],
        },
        new TweakDef
        {
            Id = "edgepwm-block-password-change-via-browser",
            Label = "Edge Password Manager: Block In-Browser Password Change Flow",
            Category = "Edge Password Manager Policy",
            Description = "Sets PasswordChangeThroughBrowserEnabled=0 in Edge policy. Disables Edge's 'Change password' recommendation flow that offers to navigate users directly to a site's password change page when a breached or weak credential is detected, preventing the browser from accessing password management URLs on behalf of the user. " +
                "While the 'Change password' flow is a usability feature, it involves Edge automatically navigating to account settings URLs and interacting with credential change forms using the user's currently authenticated session. In enterprise environments where password changes must go through an identity governance workflow (PAM, helpdesk ticket), browser-automated password changes bypass these controls.",
            Tags = ["edge", "password-change", "identity-governance", "pam"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Edge in-browser password change flow disabled; password changes must go through the approved identity governance process.",
            ApplyOps = [RegOp.SetDword(Key, "PasswordChangeThroughBrowserEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "PasswordChangeThroughBrowserEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "PasswordChangeThroughBrowserEnabled", 0)],
        },
    ];
}
