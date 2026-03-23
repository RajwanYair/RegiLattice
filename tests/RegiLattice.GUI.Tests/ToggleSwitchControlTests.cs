using System.Drawing;
using Xunit;

namespace RegiLattice.GUI.Tests;

/// <summary>Tests for ToggleSwitchControl: state, events, animation, accessibility.</summary>
public sealed class ToggleSwitchControlTests
{
    // ── Checked state ──────────────────────────────────────────────────────
    [Fact]
    public void Checked_DefaultsToFalse()
    {
        using var ctrl = new Controls.ToggleSwitchControl();
        Assert.False(ctrl.Checked);
    }

    [Fact]
    public void Checked_SetTrue_IsTrue()
    {
        using var ctrl = new Controls.ToggleSwitchControl();
        ctrl.Checked = true;
        Assert.True(ctrl.Checked);
    }

    [Fact]
    public void Checked_SetFalse_AfterTrue_IsFalse()
    {
        using var ctrl = new Controls.ToggleSwitchControl();
        ctrl.Checked = true;
        ctrl.Checked = false;
        Assert.False(ctrl.Checked);
    }

    [Fact]
    public void Checked_SetSameValue_NoEvent()
    {
        using var ctrl = new Controls.ToggleSwitchControl();
        int calls = 0;
        ctrl.CheckedChanged += (_, _) => calls++;

        ctrl.Checked = false; // same as default
        Assert.Equal(0, calls);
    }

    // ── CheckedChanged event ───────────────────────────────────────────────
    [Fact]
    public void CheckedChanged_FiresOnToggle()
    {
        using var ctrl = new Controls.ToggleSwitchControl();
        bool fired = false;
        ctrl.CheckedChanged += (_, _) => fired = true;

        ctrl.Checked = true;
        Assert.True(fired);
    }

    [Fact]
    public void CheckedChanged_FiresOnToggleOff()
    {
        using var ctrl = new Controls.ToggleSwitchControl();
        ctrl.Checked = true;

        bool fired = false;
        ctrl.CheckedChanged += (_, _) => fired = true;
        ctrl.Checked = false;

        Assert.True(fired);
    }

    // ── SetCheckedSilent ───────────────────────────────────────────────────
    [Fact]
    public void SetCheckedSilent_DoesNotRaiseEvent()
    {
        using var ctrl = new Controls.ToggleSwitchControl();
        int calls = 0;
        ctrl.CheckedChanged += (_, _) => calls++;

        ctrl.SetCheckedSilent(true);
        Assert.True(ctrl.Checked);
        Assert.Equal(0, calls);
    }

    [Fact]
    public void SetCheckedSilent_NoChange_NoEvent()
    {
        using var ctrl = new Controls.ToggleSwitchControl();
        int calls = 0;
        ctrl.CheckedChanged += (_, _) => calls++;

        ctrl.SetCheckedSilent(false); // already false
        Assert.Equal(0, calls);
    }

    // ── ApplyTheme ─────────────────────────────────────────────────────────
    [Fact]
    public void ApplyTheme_DoesNotThrow()
    {
        using var ctrl = new Controls.ToggleSwitchControl();
        var ex = Record.Exception(() =>
            ctrl.ApplyTheme(Color.Blue, Color.DimGray, Color.White));
        Assert.Null(ex);
    }

    // ── Accessibility ──────────────────────────────────────────────────────
    [Fact]
    public void AccessibleRole_IsCheckButton()
    {
        using var ctrl = new Controls.ToggleSwitchControl();
        Assert.Equal(AccessibleRole.CheckButton, ctrl.AccessibleRole);
    }

    [Fact]
    public void AccessibleName_IsSet()
    {
        using var ctrl = new Controls.ToggleSwitchControl();
        Assert.False(string.IsNullOrEmpty(ctrl.AccessibleName));
    }

    // ── Dispose safety ─────────────────────────────────────────────────────
    [Fact]
    public void Dispose_DoesNotThrow()
    {
        var ex = Record.Exception(() =>
        {
            using var ctrl = new Controls.ToggleSwitchControl();
            ctrl.Checked = true;
        });
        Assert.Null(ex);
    }

    // ── Size ───────────────────────────────────────────────────────────────
    [Fact]
    public void DefaultSize_IsPositive()
    {
        using var ctrl = new Controls.ToggleSwitchControl();
        Assert.True(ctrl.Size.Width > 0 && ctrl.Size.Height > 0);
    }

    [Fact]
    public void GetPreferredSize_IsPositive()
    {
        using var ctrl = new Controls.ToggleSwitchControl();
        Size s = ctrl.GetPreferredSize(Size.Empty);
        Assert.True(s.Width > 0 && s.Height > 0);
    }
}
