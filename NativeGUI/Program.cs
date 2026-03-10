using RegiLattice.Native.Forms;

namespace RegiLattice.Native;

/// <summary>Application entry point.</summary>
internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
