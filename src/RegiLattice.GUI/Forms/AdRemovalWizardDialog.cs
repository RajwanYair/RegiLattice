// RegiLattice.GUI — Forms/AdRemovalWizardDialog.cs
// Sprint 32: Guided Windows ad, tip, and suggestion removal wizard (OFGB-inspired).

#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Microsoft.Win32;
using RegiLattice.Core;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Step-by-step guided wizard for removing Windows desktop ads, tips,
/// suggestions, news feeds, and other Microsoft promotional content.
/// Each step shows a description and a checkbox to opt in/out.
/// Inspired by OFGB (Oh Frick Go Back) and similar tools.
/// </summary>
[SupportedOSPlatform("windows")]
internal sealed class AdRemovalWizardDialog : BaseDialog
{
    // ── Model ─────────────────────────────────────────────────────────────────
    private sealed record AdItem(
        string Title,
        string Description,
        string RegPath,
        string ValueName,
        int DisabledValue,
        int EnabledValue,
        bool DefaultChecked = true
    );

    // ── Controls ──────────────────────────────────────────────────────────────
    private readonly ListView _list = new()
    {
        Dock = DockStyle.Fill,
        FullRowSelect = true,
        MultiSelect = false,
        View = View.Details,
        GridLines = true,
        CheckBoxes = true,
    };
    private readonly RichTextBox _descBox = new()
    {
        Dock = DockStyle.Bottom,
        Height = 90,
        ReadOnly = true,
        BorderStyle = BorderStyle.FixedSingle,
        Font = new Font(SystemFonts.DefaultFont.FontFamily, 8.5f),
    };
    private readonly FlowLayoutPanel _btnPanel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 42,
        Padding = new Padding(6, 6, 6, 4),
        FlowDirection = FlowDirection.LeftToRight,
        WrapContents = false,
    };
    private readonly Button _btnSelectAll = new() { Text = "Select All", Width = 90 };
    private readonly Button _btnClearAll = new() { Text = "Clear All", Width = 90 };
    private readonly Button _btnApply = new() { Text = "Remove Selected Ads", Width = 145 };
    private readonly Button _btnRestore = new() { Text = "Restore Defaults", Width = 125 };
    private readonly Button _btnClose = new() { Text = "Close", Width = 80 };
    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Bottom,
        Height = 22,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };

    private readonly List<AdItem> _items = [];

    // ── Construction ──────────────────────────────────────────────────────────
    internal AdRemovalWizardDialog()
        : base("Windows Ad & Tip Removal Wizard", new Size(780, 560), resizable: true)
    {
        BuildItems();
        BuildLayout();
        LoadCurrentState();

        _btnSelectAll.Click += (_, _) => { foreach (ListViewItem i in _list.Items) i.Checked = true; };
        _btnClearAll.Click += (_, _) => { foreach (ListViewItem i in _list.Items) i.Checked = false; };
        _btnApply.Click += OnApply;
        _btnRestore.Click += OnRestore;
        _btnClose.Click += (_, _) => Close();
        _list.SelectedIndexChanged += OnSelectionChanged;

        if (!Elevation.IsAdmin())
            Controls.Add(CreateAdminBanner("⚠  Some settings require Administrator rights."));
    }

    // ── Ad Item Definitions ───────────────────────────────────────────────────
    private void BuildItems()
    {
        const string Personalization = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager";
        const string ExplorerPolicies = @"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer";
        const string SearchPolicies = @"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Windows Search";
        const string StartMenu = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
        const string Feeds = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds";

        _items.AddRange([
            new AdItem(
                "Start Menu Recommended Content",
                "Removes 'Recommended' app suggestions and recently opened files from the Start menu. "
                + "Windows 11 Start Menu shows personalized app suggestions powered by cloud data. "
                + "Setting this to 0 prevents these suggestions from appearing.",
                Personalization, "SubscribedContent-338389Enabled", 0, 1
            ),
            new AdItem(
                "Start Menu App Suggestions",
                "Disables Microsoft Store app suggestions that appear in the Start menu. "
                + "These are ads for paid apps that Microsoft rotates periodically.",
                Personalization, "SystemPaneSuggestionsEnabled", 0, 1
            ),
            new AdItem(
                "Lock Screen Tips and Fun Facts",
                "Removes 'Windows spotlight' tips, fun facts, and ads that appear on the lock screen. "
                + "Windows uses this placement to promote Microsoft services and Bing searches.",
                Personalization, "RotatingLockScreenOverlayEnabled", 0, 1
            ),
            new AdItem(
                "Lock Screen Trivia and Ads",
                "Disables the trivia/tips overlay on lock screen images (Windows Spotlight).",
                Personalization, "SubscribedContent-338387Enabled", 0, 1
            ),
            new AdItem(
                "Notification Ads (Action Center)",
                "Prevents Windows from showing promotional notifications in Action Center, "
                + "such as Microsoft 365 upsells and OneDrive prompts.",
                Personalization, "SubscribedContent-338393Enabled", 0, 1
            ),
            new AdItem(
                "Settings App Suggestions",
                "Removes suggested app and feature tips from the Windows Settings app. "
                + "These appear as banner cards promoted by Microsoft.",
                Personalization, "SubscribedContent-353694Enabled", 0, 1
            ),
            new AdItem(
                "Taskbar News and Interests Widget",
                "Hides the News and Interests / Widgets button on the taskbar. "
                + "This feature shows MSN news, weather, and ads fetched from the internet.",
                @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Feeds", "ShellFeedsTaskbarViewMode", 2, 0
            ),
            new AdItem(
                "Search Box Bing Suggestions",
                "Disables web search results and Bing suggestions in the Windows search box. "
                + "Keeps local search results only and stops sending keystrokes to Microsoft servers.",
                @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled", 0, 1
            ),
            new AdItem(
                "Search Box Trending Searches",
                "Removes 'trending' MSN/Bing search suggestions from the search popup. "
                + "These include sponsored topics and news headlines.",
                SearchPolicies, "EnableDynamicContentInWSB", 0, 1
            ),
            new AdItem(
                "Explorer Ads in Sync Provider",
                "Disables ads shown in the File Explorer navigation pane by cloud sync providers "
                + "(typically OneDrive / Microsoft 365 promotional banners).",
                ExplorerPolicies, "ShowSyncProviderNotifications", 0, 1
            ),
            new AdItem(
                "Cortana Welcome Tips",
                "Disables the Cortana welcome experience and first-run tips that appear after login.",
                Personalization, "SubscribedContent-310093Enabled", 0, 1
            ),
            new AdItem(
                "Windows Welcome Experience (Feature Highlights)",
                "Disables the 'What's new in Windows' welcome screen that appears after updates, "
                + "which often highlights Microsoft services and OneDrive upsells.",
                Personalization, "SubscribedContent-338388Enabled", 0, 1
            ),
            new AdItem(
                "Timeline Suggestions",
                "Disables Windows Timeline activity suggestions powered by Microsoft Graph, "
                + "which can include AI-powered suggestions based on your activity.",
                @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SuggestedContent-338393Enabled", 0, 1
            ),
            new AdItem(
                "My People Promotional Content",
                "Disables promotional badges and suggestions in the My People feature on the taskbar.",
                Personalization, "SubscribedContent-314563Enabled", 0, 1
            ),
        ]);
    }

    // ── Layout ────────────────────────────────────────────────────────────────
    private void BuildLayout()
    {
        _list.Columns.AddRange([
            new ColumnHeader { Text = "Ad / Tip Type", Width = 290 },
            new ColumnHeader { Text = "Current Status", Width = 110 },
            new ColumnHeader { Text = "Registry Key", Width = 240 },
        ]);

        _btnPanel.Controls.AddRange([_btnSelectAll, _btnClearAll, _btnApply, _btnRestore, _btnClose]);
        Controls.AddRange([_list, _descBox, _statusLabel, _btnPanel]);
    }

    // ── Load Current State ────────────────────────────────────────────────────
    private void LoadCurrentState()
    {
        _list.BeginUpdate();
        _list.Items.Clear();

        foreach (var item in _items)
        {
            bool isDisabled = ReadIsDisabled(item);
            var lvi = new ListViewItem(item.Title) { Tag = item, Checked = item.DefaultChecked };
            lvi.SubItems.Add(isDisabled ? "Disabled ✓" : "Active");
            lvi.SubItems.Add($"...\\{item.ValueName}");
            if (isDisabled)
                lvi.ForeColor = Color.ForestGreen;
            _list.Items.Add(lvi);
        }

        _list.EndUpdate();
        _statusLabel.Text = $"{_items.Count} ad/tip types detected. Check items to disable.";
    }

    private static bool ReadIsDisabled(AdItem item)
    {
        try
        {
            var hiveKey = item.RegPath.StartsWith("HKEY_CURRENT_USER", StringComparison.OrdinalIgnoreCase)
                ? (Registry.CurrentUser, item.RegPath.Substring("HKEY_CURRENT_USER\\".Length))
                : (Registry.LocalMachine, item.RegPath.Substring("HKEY_LOCAL_MACHINE\\".Length));

            using var key = hiveKey.Item1.OpenSubKey(hiveKey.Item2);
            if (key is null) return false;
            var val = key.GetValue(item.ValueName);
            return val is int i && i == item.DisabledValue;
        }
        catch { return false; }
    }

    // ── Apply ─────────────────────────────────────────────────────────────────
    private void OnApply(object? sender, EventArgs e)
    {
        int applied = 0;
        int failed = 0;

        foreach (ListViewItem lvi in _list.Items)
        {
            if (!lvi.Checked || lvi.Tag is not AdItem item)
                continue;

            try
            {
                bool isHkcu = item.RegPath.StartsWith("HKEY_CURRENT_USER", StringComparison.OrdinalIgnoreCase);
                string subKey = isHkcu
                    ? item.RegPath.Substring("HKEY_CURRENT_USER\\".Length)
                    : item.RegPath.Substring("HKEY_LOCAL_MACHINE\\".Length);

                using var key = isHkcu
                    ? Registry.CurrentUser.CreateSubKey(subKey, writable: true)
                    : Registry.LocalMachine.CreateSubKey(subKey, writable: true);

                key?.SetValue(item.ValueName, item.DisabledValue, RegistryValueKind.DWord);
                lvi.SubItems[1].Text = "Disabled ✓";
                lvi.ForeColor = Color.ForestGreen;
                applied++;
            }
            catch
            {
                lvi.SubItems[1].Text = "Error";
                lvi.ForeColor = Color.Red;
                failed++;
            }
        }

        _statusLabel.Text = $"✓ {applied} ad type(s) disabled. {(failed > 0 ? $"{failed} failed." : "")} Sign out or restart Explorer to see changes.";
    }

    private void OnRestore(object? sender, EventArgs e)
    {
        int restored = 0;
        foreach (ListViewItem lvi in _list.Items)
        {
            if (lvi.Tag is not AdItem item)
                continue;
            try
            {
                bool isHkcu = item.RegPath.StartsWith("HKEY_CURRENT_USER", StringComparison.OrdinalIgnoreCase);
                string subKey = isHkcu
                    ? item.RegPath.Substring("HKEY_CURRENT_USER\\".Length)
                    : item.RegPath.Substring("HKEY_LOCAL_MACHINE\\".Length);

                using var key = isHkcu
                    ? Registry.CurrentUser.CreateSubKey(subKey, writable: true)
                    : Registry.LocalMachine.CreateSubKey(subKey, writable: true);

                key?.SetValue(item.ValueName, item.EnabledValue, RegistryValueKind.DWord);
                lvi.SubItems[1].Text = "Active";
                lvi.ForeColor = SystemColors.WindowText;
                restored++;
            }
            catch { /* ignore */ }
        }
        _statusLabel.Text = $"{restored} setting(s) restored to Windows defaults.";
    }

    private void OnSelectionChanged(object? sender, EventArgs e)
    {
        if (_list.SelectedItems.Count > 0 && _list.SelectedItems[0].Tag is AdItem item)
            _descBox.Text = item.Description;
    }
}
