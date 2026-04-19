#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Win32;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Plugins;
using RegiLattice.Core.Registry;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;
public sealed class SnapshotNullJsonBranchTests
{
    [Fact]
    public void Load_NullJsonContent_ReturnsEmptyDictionary()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "null");
            var result = SnapshotManager.Load(path);
            Assert.Empty(result);
        }
        finally
        {
            File.Delete(path);
        }
    }
}
