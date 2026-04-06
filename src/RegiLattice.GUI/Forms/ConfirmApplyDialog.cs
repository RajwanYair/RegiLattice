// RegiLattice.GUI — Forms/ConfirmApplyDialog.cs
// Phase 2.3: Risk-confirmation dialog shown before applying a tweak with low safety
// rating or destructive registry operations.

#nullable enable

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using RegiLattice.Core.Models;
using RegiLattice.Core.Services;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// A themed modal dialog that warns the user about a risky tweak before it is applied.
/// Call <see cref="ShouldConfirm"/> to check whether the dialog is needed, then
/// <see cref="ShowConfirm"/> to display it and get the user's decision.
/// </summary>
internal sealed class ConfirmApplyDialog : Form
{
    // ── Public API ──────────────────────────────────────────────────────────

    /// <inheritdoc cref="ConfirmApplyThreshold.ShouldConfirm"/>
    public static bool ShouldConfirm(TweakDef td) => ConfirmApplyThreshold.ShouldConfirm(td);

    /// <summary>
    /// Displays the risk-confirmation dialog and returns <c>true</c> when the user
    /// elects to proceed with the apply operation.
    /// </summary>
    public static bool ShowConfirm(IWin32Window owner, TweakDef tweak)
    {
        using var dlg = new ConfirmApplyDialog(tweak);
        dlg.ShowDialog(owner);
        return dlg._userApproved;
    }

    // ── Private state ───────────────────────────────────────────────────────

    private readonly TweakDef _tweak;
    private bool _userApproved;

    private ConfirmApplyDialog(TweakDef tweak)
    {
        _tweak = tweak;
        BuildUi();
    }

    // ── UI construction ─────────────────────────────────────────────────────

    private void BuildUi()
    {
        SuspendLayout();

        Text = "Confirm Apply — Risk Warning";
        Size = new Size(560, 430);
        MinimumSize = Size;
        MaximizeBox = false;
        MinimizeBox = false;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = AppTheme.Bg;
        ForeColor = AppTheme.Fg;
        Font = AppTheme.Regular;

        // ── Warning title ────────────────────────────────────────────────
        var lblTitle = new Label
        {
            Text = $"⚠  Apply: {_tweak.Label}",
            Font = AppTheme.Bold,
            ForeColor = Color.FromArgb(0xF3, 0x9C, 0x12),
            AutoSize = false,
            Bounds = new Rectangle(16, 14, 528, 26),
        };

        // ── Category / safety row ─────────────────────────────────────────
        var lblInfo = new Label
        {
            Text = $"Category: {_tweak.Category}   •   Safety: {_tweak.SafetyRating}/5   •   Impact: {_tweak.ImpactScore}/5",
            Font = AppTheme.Small,
            ForeColor = AppTheme.FgDim,
            AutoSize = false,
            Bounds = new Rectangle(16, 44, 528, 16),
        };

        // ── Risk-flag badges ──────────────────────────────────────────────
        var riskPanel = new FlowLayoutPanel
        {
            Bounds = new Rectangle(16, 64, 528, 26),
            AutoSize = false,
            BackColor = AppTheme.Bg,
            WrapContents = false,
        };
        AddRiskBadges(riskPanel);

        // ── Description / impact note ─────────────────────────────────────
        string desc = string.IsNullOrWhiteSpace(_tweak.Description) ? _tweak.ImpactNote : _tweak.Description;
        var lblDesc = new Label
        {
            Text = string.IsNullOrWhiteSpace(desc) ? "(No description provided.)" : desc,
            Font = AppTheme.Small,
            ForeColor = AppTheme.Fg,
            AutoSize = false,
            Bounds = new Rectangle(16, 96, 528, 52),
        };

        // ── Registry ops header ────────────────────────────────────────────
        var lblOpsHeader = new Label
        {
            Text = "Registry operations that will be performed:",
            Font = AppTheme.Small,
            ForeColor = AppTheme.FgDim,
            AutoSize = false,
            Bounds = new Rectangle(16, 152, 528, 16),
        };

        // ── Registry ops list ─────────────────────────────────────────────
        var opsBox = new RichTextBox
        {
            ReadOnly = true,
            BackColor = AppTheme.Surface,
            ForeColor = AppTheme.Fg,
            Font = AppTheme.Mono,
            ScrollBars = RichTextBoxScrollBars.Vertical,
            BorderStyle = BorderStyle.None,
            Bounds = new Rectangle(16, 172, 528, 116),
        };
        FillOpsBox(opsBox);

        // ── Buttons ───────────────────────────────────────────────────────
        var btnApply = new Button
        {
            Text = "Apply Anyway",
            Bounds = new Rectangle(16, 306, 130, 32),
            BackColor = Color.FromArgb(0xB0, 0x2A, 0x18),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = AppTheme.Regular,
        };
        btnApply.FlatAppearance.BorderSize = 0;
        btnApply.Click += (_, _) =>
        {
            _userApproved = true;
            Close();
        };

        var btnCancel = new Button
        {
            Text = "Cancel",
            Bounds = new Rectangle(158, 306, 100, 32),
            BackColor = AppTheme.Surface2,
            ForeColor = AppTheme.Fg,
            FlatStyle = FlatStyle.Flat,
            Font = AppTheme.Regular,
        };
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.Click += (_, _) =>
        {
            _userApproved = false;
            Close();
        };

        // ── Hint note ──────────────────────────────────────────────────────
        var lblNote = new Label
        {
            Text = "Tip: Use dry-run mode (--dry-run CLI / Force-check in GUI) to preview without making changes.",
            Font = AppTheme.Small,
            ForeColor = AppTheme.FgDim,
            AutoSize = false,
            Bounds = new Rectangle(16, 352, 528, 16),
        };

        Controls.AddRange([lblTitle, lblInfo, riskPanel, lblDesc, lblOpsHeader, opsBox, btnApply, btnCancel, lblNote]);
        AcceptButton = btnApply;
        CancelButton = btnCancel;
        ResumeLayout(true);
    }

    private void AddRiskBadges(FlowLayoutPanel panel)
    {
        var flags = _tweak.EffectiveRiskFlags;

        if (_tweak.SafetyRating <= 2)
            AddBadge(panel, "🔴 HIGH RISK", Color.FromArgb(0xB0, 0x2A, 0x18));
        else if (_tweak.SafetyRating == 3)
            AddBadge(panel, "🟠 MODERATE RISK", Color.FromArgb(0xC2, 0x6A, 0x00));

        if (flags.HasFlag(TweakRisk.DeletesKey))
            AddBadge(panel, "🗑 Deletes Registry Key", Color.FromArgb(0x8B, 0x1A, 0x1A));
        if (flags.HasFlag(TweakRisk.RequiresReboot))
            AddBadge(panel, "🔄 Reboot Required", Color.FromArgb(0x1A, 0x4E, 0x8E));
        if (flags.HasFlag(TweakRisk.AffectsSecurity))
            AddBadge(panel, "🔒 Affects Security", Color.FromArgb(0x0D, 0x5E, 0x2D));
        if (flags.HasFlag(TweakRisk.PotentialDataLoss))
            AddBadge(panel, "💾 Data Loss Risk", Color.FromArgb(0x5C, 0x10, 0x60));
        if (flags.HasFlag(TweakRisk.AffectsNetwork))
            AddBadge(panel, "🌐 Affects Network", Color.FromArgb(0x1A, 0x4A, 0x6A));
        if (flags.HasFlag(TweakRisk.AffectsService))
            AddBadge(panel, "⚙ Affects Service", Color.FromArgb(0x4A, 0x3A, 0x00));
    }

    private void AddBadge(FlowLayoutPanel panel, string text, Color bg)
    {
        var lbl = new Label
        {
            Text = text,
            AutoSize = true,
            BackColor = bg,
            ForeColor = Color.White,
            Font = AppTheme.Small,
            Margin = new Padding(0, 0, 4, 0),
            Padding = new Padding(4, 2, 4, 2),
        };
        panel.Controls.Add(lbl);
    }

    private void FillOpsBox(RichTextBox box)
    {
        if (_tweak.ApplyOps.Count == 0)
        {
            box.AppendText("(Custom action — no declarative registry operations)");
            return;
        }

        int shown = Math.Min(_tweak.ApplyOps.Count, 10);
        foreach (var op in _tweak.ApplyOps.Take(shown))
        {
            string opText = op.Kind switch
            {
                RegOpKind.DeleteTree => $"DELETE KEY  {op.Path}",
                RegOpKind.DeleteValue => $"DELETE VAL  {op.Path}\\{op.Name}",
                RegOpKind.SetValue => BuildSetLine(op),
                _ => $"{op.Kind}  {op.Path}\\{op.Name}",
            };
            box.AppendText(opText + "\n");
        }

        if (_tweak.ApplyOps.Count > shown)
            box.AppendText($"  … +{_tweak.ApplyOps.Count - shown} more operations");
    }

    private static string BuildSetLine(RegOp op)
    {
        string typeTag = op.ValueKind switch
        {
            RegistryValueKind.DWord => "DWORD",
            RegistryValueKind.QWord => "QWORD",
            RegistryValueKind.ExpandString => "EXPAND_SZ",
            RegistryValueKind.MultiString => "MULTI_SZ",
            RegistryValueKind.Binary => "BINARY",
            _ => "SZ",
        };
        string val = op.Value?.ToString() ?? "";
        if (val.Length > 40)
            val = val[..40] + "…";
        return $"SET  {op.Path}\\{op.Name}  [{typeTag}]  = {val}";
    }
}
