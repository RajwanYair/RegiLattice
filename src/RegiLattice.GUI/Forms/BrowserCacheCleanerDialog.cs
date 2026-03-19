// RegiLattice.GUI — Forms/BrowserCacheCleanerDialog.cs
// Cleans cached data (cache, cookies, session) from installed browsers.
#nullable enable

using System.Diagnostics;

namespace RegiLattice.GUI.Forms;

/// <summary>
/// Browser Cache Cleaner.
/// Discovers installed browser profile directories (Chrome, Edge, Firefox,
/// Brave, Vivaldi, Opera) and lets the user delete cache/cookies/session data.
/// </summary>
internal sealed class BrowserCacheCleanerDialog : BaseDialog
{
    private sealed record BrowserProfile(string BrowserName, string ProfilePath, string CachePath, long CacheBytes, bool Exists);

    private readonly ListView _list = new()
    {
        View = View.Details,
        FullRowSelect = true,
        CheckBoxes = true,
        GridLines = true,
        Dock = DockStyle.Fill,
    };
    private readonly Button _btnScan = new()
    {
        Text = "⟳ Scan",
        Width = 90,
        Height = 28,
    };
    private readonly Button _btnClean = new()
    {
        Text = "🗑 Clean Selected",
        Width = 130,
        Height = 28,
    };
    private readonly Button _btnClose = new()
    {
        Text = "Close",
        Width = 75,
        Height = 28,
        DialogResult = DialogResult.Cancel,
    };
    private readonly Label _lblStatus = new()
    {
        Dock = DockStyle.Top,
        Height = 26,
        TextAlign = ContentAlignment.MiddleLeft,
        Padding = new Padding(6, 0, 0, 0),
    };
    private readonly Label _lblNote = new()
    {
        Text = "⚠ Close all browsers before cleaning. Running browsers may lock cache files.",
        Dock = DockStyle.Top,
        Height = 26,
        TextAlign = ContentAlignment.MiddleLeft,
        ForeColor = Color.FromArgb(180, 100, 0),
        Padding = new Padding(6, 0, 0, 0),
    };

    // History + Cookie clean options
    private readonly CheckBox _chkHistory = new()
    {
        Text = "Clear History",
        AutoSize = true,
        Checked = false,
    };
    private readonly CheckBox _chkCookies = new()
    {
        Text = "Clear Cookies",
        AutoSize = true,
        Checked = false,
    };

    private List<BrowserProfile> _profiles = [];

    // Known browser cache path patterns relative to %LOCALAPPDATA% or %APPDATA%
    private static readonly (string Name, string RelRoot, string CacheSuffix, bool UseLocal)[] s_browsers =
    [
        ("Google Chrome", @"Google\Chrome\User Data\Default", @"Cache\Cache_Data", true),
        ("Microsoft Edge", @"Microsoft\Edge\User Data\Default", @"Cache\Cache_Data", true),
        ("Brave", @"BraveSoftware\Brave-Browser\User Data\Default", @"Cache\Cache_Data", true),
        ("Vivaldi", @"Vivaldi\User Data\Default", @"Cache\Cache_Data", true),
        ("Opera", @"Opera Software\Opera Stable", @"Cache\Cache_Data", true),
        ("Opera GX", @"Opera Software\Opera GX Stable", @"Cache\Cache_Data", true),
        ("Firefox", @"Mozilla\Firefox\Profiles", "", false), // special
        ("Waterfox", @"Waterfox\Profiles", "", false), // special
    ];

    public BrowserCacheCleanerDialog()
        : base("Browser Cache Cleaner", new Size(800, 580), resizable: true)
    {
        MinimumSize = new Size(640, 440);
        EnableStandaloneMode();

        _list.Columns.AddRange([
            new ColumnHeader { Text = "Browser", Width = 150 },
            new ColumnHeader { Text = "Profile", Width = 240 },
            new ColumnHeader { Text = "Cache Size", Width = 100 },
            new ColumnHeader { Text = "Cache Path", Width = 280 },
        ]);
        ListViewColumnSorter.AttachTo(_list);

        var btnPanel = new Panel { Dock = DockStyle.Bottom, Height = 38 };
        _btnScan.Location = new Point(8, 5);
        _btnClean.Location = new Point(104, 5);
        _chkHistory.Location = new Point(244, 9);
        _chkCookies.Location = new Point(350, 9);
        _btnClose.Location = new Point(btnPanel.Width - _btnClose.Width - 8, 5);
        btnPanel.Resize += (_, _) => _btnClose.Location = new Point(btnPanel.Width - _btnClose.Width - 8, 5);
        _btnScan.Click += async (_, _) => await ScanAsync();
        _btnClean.Click += async (_, _) => await CleanAsync();
        _btnClose.Click += (_, _) => Close();
        btnPanel.Controls.AddRange(new Control[] { _btnScan, _btnClean, _chkHistory, _chkCookies, _btnClose });

        Controls.Add(_list);
        Controls.Add(btnPanel);
        Controls.Add(_lblStatus);
        Controls.Add(_lblNote);

        _ = ScanAsync();
    }

    private async Task ScanAsync()
    {
        _btnScan.Enabled = false;
        _btnClean.Enabled = false;
        _lblStatus.Text = "Scanning for browser profiles…";
        _list.Items.Clear();

        _profiles = await Task.Run(DiscoverProfiles);

        _list.BeginUpdate();
        foreach (BrowserProfile p in _profiles)
        {
            var lvi = new ListViewItem(p.BrowserName);
            lvi.SubItems.Add(Path.GetFileName(p.ProfilePath));
            lvi.SubItems.Add(p.Exists ? FormatSize(p.CacheBytes) : "Not Found");
            lvi.SubItems.Add(p.CachePath);
            if (!p.Exists)
                lvi.ForeColor = Color.FromArgb(128, 128, 128);
            lvi.Checked = p.Exists && p.CacheBytes > 0;
            _list.Items.Add(lvi);
        }
        _list.EndUpdate();

        long totalBytes = _profiles.Where(p => p.Exists).Sum(p => p.CacheBytes);
        _lblStatus.Text = $"{_profiles.Count(p => p.Exists)} browser profile(s) found — {FormatSize(totalBytes)} total cache.";
        _btnScan.Enabled = true;
        _btnClean.Enabled = _profiles.Any(p => p.Exists && p.CacheBytes > 0);
    }

    private static List<BrowserProfile> DiscoverProfiles()
    {
        string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var result = new List<BrowserProfile>();

        foreach (var (name, relRoot, cacheSuffix, useLocal) in s_browsers)
        {
            string baseDir = useLocal ? local : roaming;
            string profileRoot = Path.Combine(baseDir, relRoot);

            if (name is "Firefox" or "Waterfox")
            {
                // Firefox uses named profile directories inside Profiles/
                if (!Directory.Exists(profileRoot))
                    continue;
                foreach (string profileDir in Directory.GetDirectories(profileRoot))
                {
                    string cachePath = Path.Combine(profileDir, "cache2");
                    long bytes = DirectorySize(cachePath);
                    result.Add(new BrowserProfile(name, profileDir, cachePath, bytes, Directory.Exists(cachePath)));
                }
            }
            else
            {
                string cachePath = Path.Combine(profileRoot, cacheSuffix);
                long bytes = DirectorySize(cachePath);
                result.Add(new BrowserProfile(name, profileRoot, cachePath, bytes, Directory.Exists(profileRoot)));
            }
        }
        return result;
    }

    private async Task CleanAsync()
    {
        var toClean = new List<BrowserProfile>();
        for (int i = 0; i < _list.Items.Count; i++)
        {
            if (_list.Items[i].Checked && i < _profiles.Count && _profiles[i].Exists)
                toClean.Add(_profiles[i]);
        }

        if (toClean.Count == 0)
        {
            _lblStatus.Text = "No profiles selected for cleaning.";
            return;
        }

        bool cleanHistory = _chkHistory.Checked;
        bool cleanCookies = _chkCookies.Checked;

        _btnScan.Enabled = false;
        _btnClean.Enabled = false;
        _lblStatus.Text = $"Cleaning {toClean.Count} profile(s)…";

        long freedBytes = await Task.Run(() =>
        {
            long freed = 0;
            foreach (var p in toClean)
            {
                try
                {
                    // Clean cache
                    if (Directory.Exists(p.CachePath))
                    {
                        freed += DirectorySize(p.CachePath);
                        DeleteDirectoryContents(p.CachePath);
                    }

                    // Clean history (Chromium: History file; Firefox: places.sqlite)
                    if (cleanHistory)
                    {
                        string historyFile = Path.Combine(p.ProfilePath, "History");
                        string placesFile = Path.Combine(p.ProfilePath, "places.sqlite");
                        foreach (string f in new[] { historyFile, placesFile })
                        {
                            try
                            {
                                if (File.Exists(f))
                                {
                                    freed += new FileInfo(f).Length;
                                    File.Delete(f);
                                }
                            }
                            catch { }
                        }
                        // Chromium favicons and visited links
                        foreach (string extra in new[] { "Visited Links", "Favicons" })
                        {
                            string ef = Path.Combine(p.ProfilePath, extra);
                            try
                            {
                                if (File.Exists(ef))
                                {
                                    freed += new FileInfo(ef).Length;
                                    File.Delete(ef);
                                }
                            }
                            catch { }
                        }
                    }

                    // Clean cookies (Chromium: Network/Cookies or Cookies; Firefox: cookies.sqlite)
                    if (cleanCookies)
                    {
                        string networkCookies = Path.Combine(p.ProfilePath, "Network", "Cookies");
                        string chromiumCookies = Path.Combine(p.ProfilePath, "Cookies");
                        string firefoxCookies = Path.Combine(p.ProfilePath, "cookies.sqlite");
                        foreach (string f in new[] { networkCookies, chromiumCookies, firefoxCookies })
                        {
                            try
                            {
                                if (File.Exists(f))
                                {
                                    freed += new FileInfo(f).Length;
                                    File.Delete(f);
                                }
                            }
                            catch { }
                        }
                    }
                }
                catch
                {
                    // Skip locked files (browser may still be running)
                }
            }
            return freed;
        });

        var what = new List<string> { "cache" };
        if (cleanHistory)
            what.Add("history");
        if (cleanCookies)
            what.Add("cookies");
        _lblStatus.Text = $"✓ Cleaned {string.Join(", ", what)} for {toClean.Count} profile(s) — freed ~{FormatSize(freedBytes)}.";
        await ScanAsync();
    }

    private static long DirectorySize(string path)
    {
        if (!Directory.Exists(path))
            return 0;
        try
        {
            return new DirectoryInfo(path)
                .EnumerateFiles("*", SearchOption.AllDirectories)
                .Sum(f =>
                {
                    try
                    {
                        return f.Length;
                    }
                    catch
                    {
                        return 0L;
                    }
                });
        }
        catch
        {
            return 0;
        }
    }

    private static void DeleteDirectoryContents(string path)
    {
        foreach (string file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
        {
            try
            {
                File.Delete(file);
            }
            catch { }
        }
        foreach (string dir in Directory.GetDirectories(path))
        {
            try
            {
                Directory.Delete(dir, recursive: true);
            }
            catch { }
        }
    }

    private static string FormatSize(long bytes)
    {
        if (bytes < 1024)
            return $"{bytes} B";
        if (bytes < 1024 * 1024)
            return $"{bytes / 1024.0:F1} KB";
        if (bytes < 1024L * 1024 * 1024)
            return $"{bytes / (1024.0 * 1024):F1} MB";
        return $"{bytes / (1024.0 * 1024 * 1024):F2} GB";
    }
}
