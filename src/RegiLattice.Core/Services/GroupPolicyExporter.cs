// RegiLattice.Core — Services/GroupPolicyExporter.cs
// Exports Group Policy type tweaks as .admx / .adml template files (Phase 9 #88).

using System.Text;
using System.Xml;
using RegiLattice.Core.Models;

namespace RegiLattice.Core;

/// <summary>
/// Generates an .admx Group Policy administrative template and a matching .adml string table
/// from the <c>GroupPolicy</c>-kind tweaks registered in a <see cref="TweakEngine"/>.
/// </summary>
public static class GroupPolicyExporter
{
    private const string Namespace = "RegiLattice";
    private const string DisplayName = "RegiLattice Tweaks";
    private const string SchemaVersion = "1.192";

    /// <summary>
    /// Exports all <see cref="TweakKind.GroupPolicy"/> tweaks to <paramref name="admxPath"/>.
    /// Creates a companion <em>.adml</em> file in the same directory as <paramref name="admxPath"/>.
    /// </summary>
    public static void Export(TweakEngine engine, string admxPath)
    {
        ArgumentNullException.ThrowIfNull(engine);
        ArgumentException.ThrowIfNullOrWhiteSpace(admxPath);

        var gpoTweaks = engine
            .AllTweaks()
            .Where(t => t.Kind == TweakKind.GroupPolicy && t.ApplyOps.Count > 0)
            .OrderBy(t => t.Category)
            .ThenBy(t => t.Label)
            .ToList();

        var admxXml = BuildAdmx(gpoTweaks);
        var admlXml = BuildAdml(gpoTweaks);
        var admlDir = Path.Combine(Path.GetDirectoryName(admxPath) ?? ".", "en-US");

        Directory.CreateDirectory(admlDir);

        File.WriteAllText(admxPath, admxXml, Encoding.UTF8);
        File.WriteAllText(Path.Combine(admlDir, Path.ChangeExtension(Path.GetFileName(admxPath), ".adml")), admlXml, Encoding.UTF8);
    }

    // ── Internal builders ──────────────────────────────────────────────────────

    private static string BuildAdmx(IReadOnlyList<TweakDef> tweaks)
    {
        var sb = new StringBuilder();
        using var w = XmlWriter.Create(
            sb,
            new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false,
            }
        );

        w.WriteStartDocument();
        w.WriteStartElement("policyDefinitions", "http://schemas.microsoft.com/GroupPolicy/2006/07/PolicyDefinitions");
        w.WriteAttributeString("revision", "1.0");
        w.WriteAttributeString("schemaVersion", SchemaVersion);

        // <policyNamespaces>
        w.WriteStartElement("policyNamespaces");
        w.WriteStartElement("target");
        w.WriteAttributeString("prefix", Namespace);
        w.WriteAttributeString("namespace", $"Microsoft.Policies.{Namespace}");
        w.WriteEndElement();
        w.WriteEndElement();

        // <resources>
        w.WriteStartElement("resources");
        w.WriteAttributeString("minRequiredRevision", "1.0");
        w.WriteEndElement();

        // <categories>
        var categories = tweaks.Select(t => t.Category).Distinct().OrderBy(c => c).ToList();
        w.WriteStartElement("categories");
        w.WriteStartElement("category");
        w.WriteAttributeString("name", $"{Namespace}::{Namespace}");
        w.WriteStartElement("parentCategory");
        w.WriteAttributeString("ref", "windows:WindowsComponents");
        w.WriteEndElement();
        w.WriteEndElement();
        foreach (var cat in categories)
        {
            w.WriteStartElement("category");
            w.WriteAttributeString("name", $"{Namespace}::{Sanitize(cat)}");
            w.WriteStartElement("parentCategory");
            w.WriteAttributeString("ref", $"{Namespace}::{Namespace}");
            w.WriteEndElement();
            w.WriteEndElement();
        }
        w.WriteEndElement(); // categories

        // <policies>
        w.WriteStartElement("policies");
        foreach (var tw in tweaks)
        {
            var firstOp = tw.ApplyOps[0];
            w.WriteStartElement("policy");
            w.WriteAttributeString("name", $"{Namespace}::{Sanitize(tw.Id)}");
            w.WriteAttributeString("class", "Machine");
            w.WriteAttributeString("displayName", $"$(string.{Sanitize(tw.Id)}_Name)");
            w.WriteAttributeString("explainText", $"$(string.{Sanitize(tw.Id)}_Help)");
            w.WriteAttributeString("key", firstOp.Path);

            w.WriteStartElement("parentCategory");
            w.WriteAttributeString("ref", $"{Namespace}::{Sanitize(tw.Category)}");
            w.WriteEndElement();

            w.WriteStartElement("supportedOn");
            w.WriteAttributeString("ref", "windows:SUPPORTED_WIN10");
            w.WriteEndElement();

            w.WriteEndElement(); // policy
        }
        w.WriteEndElement(); // policies

        w.WriteEndElement(); // policyDefinitions
        w.WriteEndDocument();
        return sb.ToString();
    }

    private static string BuildAdml(IReadOnlyList<TweakDef> tweaks)
    {
        var sb = new StringBuilder();
        using var w = XmlWriter.Create(
            sb,
            new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false,
            }
        );

        w.WriteStartDocument();
        w.WriteStartElement("policyDefinitionResources", "http://schemas.microsoft.com/GroupPolicy/2006/07/PolicyDefinitions");
        w.WriteAttributeString("revision", "1.0");
        w.WriteAttributeString("schemaVersion", SchemaVersion);

        w.WriteStartElement("displayName");
        w.WriteString(DisplayName);
        w.WriteEndElement();

        w.WriteStartElement("description");
        w.WriteString($"Group Policy definitions generated by RegiLattice ({tweaks.Count} policies).");
        w.WriteEndElement();

        w.WriteStartElement("resources");
        w.WriteStartElement("stringTable");

        // Category strings
        w.WriteStartElement("string");
        w.WriteAttributeString("id", $"{Namespace}_{Namespace}");
        w.WriteString(DisplayName);
        w.WriteEndElement();

        foreach (var cat in tweaks.Select(t => t.Category).Distinct().OrderBy(c => c))
        {
            w.WriteStartElement("string");
            w.WriteAttributeString("id", $"{Namespace}_{Sanitize(cat)}_Category");
            w.WriteString(cat);
            w.WriteEndElement();
        }

        // Policy strings
        foreach (var tw in tweaks)
        {
            var safe = Sanitize(tw.Id);
            w.WriteStartElement("string");
            w.WriteAttributeString("id", $"{safe}_Name");
            w.WriteString(tw.Label);
            w.WriteEndElement();

            w.WriteStartElement("string");
            w.WriteAttributeString("id", $"{safe}_Help");
            w.WriteString(string.IsNullOrWhiteSpace(tw.Description) ? tw.Label : tw.Description);
            w.WriteEndElement();
        }

        w.WriteEndElement(); // stringTable
        w.WriteEndElement(); // resources
        w.WriteEndElement(); // policyDefinitionResources
        w.WriteEndDocument();
        return sb.ToString();
    }

    private static string Sanitize(string s) => System.Text.RegularExpressions.Regex.Replace(s, @"[^A-Za-z0-9_]", "_");
}
