using RegiLattice.GUI.Forms;

namespace RegiLattice.GUI;

internal static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        Form? managerForm = ResolveManagerArg(args);
        if (managerForm is not null)
        {
            AppTheme.Apply(managerForm);
            Application.Run(managerForm);
            return;
        }

        Application.Run(new MainForm());
    }

    /// <summary>
    /// Parses --manager &lt;name&gt; and returns the matching dialog, or null for normal launch.
    /// </summary>
    private static Form? ResolveManagerArg(string[] args)
    {
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (!args[i].Equals("--manager", StringComparison.OrdinalIgnoreCase))
                continue;

            return args[i + 1].ToLowerInvariant() switch
            {
                "scoop" => new ScoopManagerDialog(),
                "psmodule" => new PSModuleManagerDialog(),
                "pip" => new PipManagerDialog(),
                "winget" => new WinGetManagerDialog(),
                "chocolatey" => new ChocolateyManagerDialog(),
                "toolversions" => new ToolVersionsDialog(),
                _ => null,
            };
        }

        return null;
    }
}
