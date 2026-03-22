#nullable enable

using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace RegiLattice.GUI;

/// <summary>
/// Displays brief Windows toast notifications after batch operations.
/// <para>
/// Toast notifications work best when the app is installed from a Start-menu
/// shortcut with a registered AppUserModelID.  When WinRT toasts are unavailable,
/// the service automatically falls back to the system-tray balloon-tip API provided
/// by a supplied <see cref="System.Windows.Forms.NotifyIcon"/>.
/// </para>
/// </summary>
public sealed class ToastNotificationService
{
    // Windows App User Model ID — must match the app's Start-menu shortcut AUMID.
    // Unpackaged WinForms apps fall back to NotifyIcon if this AUMID is not registered.
    private const string AppId = "RegiLattice.RegiLattice";

    private readonly System.Windows.Forms.NotifyIcon? _trayIcon;
    private bool _toastAvailable;

    /// <summary>
    /// Creates the service, optionally accepting a <paramref name="trayIcon"/> to use
    /// as the balloon-tip fallback when WinRT toasts cannot be delivered.
    /// </summary>
    public ToastNotificationService(System.Windows.Forms.NotifyIcon? trayIcon = null)
    {
        _trayIcon = trayIcon;

        // Probe WinRT toast availability once at startup.
        try
        {
            ToastNotificationManager.CreateToastNotifier(AppId);
            _toastAvailable = true;
        }
        catch (Exception)
        {
            _toastAvailable = false;
        }
    }

    // -------------------------------------------------------------------------
    //  Public API
    // -------------------------------------------------------------------------

    /// <summary>Shows a "batch apply complete" notification.</summary>
    public void ShowApplyComplete(int succeeded, int total)
    {
        string title = succeeded == total ? "\u2705 Tweaks Applied" : $"\u26A0 Applied {succeeded}/{total}";
        string message =
            succeeded == total
                ? $"Successfully applied {total} tweak{(total == 1 ? "" : "s")}. A restart may be needed."
                : $"{succeeded} of {total} tweak{(total == 1 ? "" : "s")} applied. Check the log for details.";

        Show(title, message);
    }

    /// <summary>Shows a "batch remove complete" notification.</summary>
    public void ShowRemoveComplete(int succeeded, int total)
    {
        string title = $"\u21A9 Tweaks Reverted";
        string message = $"{succeeded} of {total} tweak{(total == 1 ? "" : "s")} reverted.";
        Show(title, message);
    }

    /// <summary>Shows a generic informational notification.</summary>
    public void ShowInfo(string title, string message) => Show(title, message);

    /// <summary>Shows a compliance drift notification when tweaks have reverted unexpectedly.</summary>
    public void ShowComplianceDrift(int violations)
    {
        string title = "\u26A0 Compliance Drift Detected";
        string message =
            violations == 1
                ? "1 previously-applied tweak has reverted. Open RegiLattice to review."
                : $"{violations} previously-applied tweaks have reverted. Open RegiLattice to review.";
        Show(title, message);
    }

    /// <summary>Shows an update-available notification.</summary>
    public void ShowUpdateAvailable(string version)
    {
        string title = "\u2B06 RegiLattice Update Available";
        string message = $"Version {version} is available. Open RegiLattice to download.";
        Show(title, message);
    }

    // -------------------------------------------------------------------------
    //  Internal helpers
    // -------------------------------------------------------------------------

    private void Show(string title, string message)
    {
        if (_toastAvailable && TryShowToast(title, message))
            return;

        // Fallback: system-tray balloon tip (always available in WinForms apps)
        ShowBalloon(title, message);
    }

    private bool TryShowToast(string title, string message)
    {
        try
        {
            string xml = BuildToastXml(title, message);
            XmlDocument doc = new();
            doc.LoadXml(xml);

            ToastNotification toast = new(doc) { ExpirationTime = DateTimeOffset.Now.AddSeconds(8) };
            ToastNotificationManager.CreateToastNotifier(AppId).Show(toast);
            return true;
        }
        catch (Exception)
        {
            _toastAvailable = false; // stop trying WinRT after first failure
            return false;
        }
    }

    private void ShowBalloon(string title, string message)
    {
        if (_trayIcon is null)
            return;

        try
        {
            // Ensure the icon is visible so the balloon can be anchored to it
            bool wasVisible = _trayIcon.Visible;
            if (!wasVisible)
                _trayIcon.Visible = true;

            _trayIcon.ShowBalloonTip(timeout: 4000, tipTitle: title, tipText: message, tipIcon: System.Windows.Forms.ToolTipIcon.Info);

            if (!wasVisible)
                _trayIcon.Visible = false;
        }
        catch (Exception)
        {
            // Balloon tip also unavailable (e.g. notification area suppressed) — silently skip.
        }
    }

    /// <summary>Builds a minimal WinRT toast XML payload.</summary>
    private static string BuildToastXml(string title, string message)
    {
        string safeTitle = System.Net.WebUtility.HtmlEncode(title);
        string safeMessage = System.Net.WebUtility.HtmlEncode(message);

        return $"""
            <toast>
              <visual>
                <binding template="ToastGeneric">
                  <text>{safeTitle}</text>
                  <text>{safeMessage}</text>
                </binding>
              </visual>
            </toast>
            """;
    }
}
