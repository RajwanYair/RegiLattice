using RegiLattice.Native.Forms;
using Xunit;

namespace NativeGUITests;

/// <summary>
/// Smoke tests that key form constructors don't throw.
/// Forms are constructed without calling Application.Run(), which is valid
/// for WinForms unit testing (just don't Show() them in a headless environment).
/// </summary>
public sealed class FormCreationTests
{
    [Fact]
    public void AboutDialog_ConstructsWithoutThrowing()
    {
        using var f = new AboutDialog(1490, 69, "python.exe", false);
        Assert.NotNull(f);
        Assert.Equal("About RegiLattice", f.Text);
    }

    [Fact]
    public void ScoopManagerDialog_ConstructsWithoutThrowing()
    {
        using var f = new ScoopManagerDialog();
        Assert.NotNull(f);
        Assert.Equal("Scoop Package Manager", f.Text);
    }

    [Fact]
    public void PSModuleManagerDialog_ConstructsWithoutThrowing()
    {
        using var f = new PSModuleManagerDialog();
        Assert.NotNull(f);
        Assert.Equal("PowerShell Modules Manager", f.Text);
    }

    [Fact]
    public void PipManagerDialog_ConstructsWithoutThrowing()
    {
        using var f = new PipManagerDialog("python.exe");
        Assert.NotNull(f);
        Assert.Equal("pip Package Manager", f.Text);
    }

    [Fact]
    public void MainForm_ConstructsWithoutThrowing()
    {
        using var f = new MainForm();
        Assert.NotNull(f);
        Assert.StartsWith("RegiLattice", f.Text);
    }
}
