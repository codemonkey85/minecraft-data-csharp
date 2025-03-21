using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace MinecraftDataParserGenerator
{
    [Generator]
    public class VersionListGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var versionFiles = context.AdditionalTextsProvider
                .Where(file => Path.GetFileName(file.Path).Equals("versions.json", StringComparison.OrdinalIgnoreCase));

            var editionNames = versionFiles
                .Select((file, _) => GetEditionFromPath(file.Path)) // Explicitly provide both arguments
                .Where(edition => !string.IsNullOrEmpty(edition))
                .Collect() // Ensure unique values
                .Select((editions, _) => editions.Distinct().ToImmutableArray());

            var versionData = versionFiles.Select((file, ct) =>
            {
                var text = file.GetText(ct);
                if (text == null)
                {
                    return null;
                }

                var json = text.ToString();
                var versions = ParseVersionsJson(json);

                // Get folder name correctly
                var folder = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(file.Path)));
                if (string.IsNullOrEmpty(folder))
                {
                    folder = "Unknown"; // Fallback for safety
                }

                return new { Folder = folder, Versions = versions };
            }).Where(x => x != null);

            context.RegisterSourceOutput(versionData, (spc, data) =>
            {
                var className = ConvertToPascalCase(data.Folder) + "Versions";
                var classSource = GenerateVersionClass(className, data.Versions);

                spc.AddSource($"{data.Folder}_Versions.g.cs", SourceText.From(classSource, Encoding.UTF8));
            });

            // Ensure Editions class is only added once
            context.RegisterSourceOutput(editionNames, (spc, editions) =>
            {
                var distinctEditions = editions.Distinct();
                var editionsClass = GenerateEditionsClass(distinctEditions);
                spc.AddSource("Editions.g.cs", SourceText.From(editionsClass, Encoding.UTF8));
            });
        }

        private static string ConvertToPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        private List<string> ParseVersionsJson(string json)
        {
            var versions = new List<string>();

            // Remove brackets
            json = json.Trim().TrimStart('[').TrimEnd(']');

            // Split by commas and remove quotes
            foreach (var item in json.Split(','))
            {
                var version = item.Trim().Trim('"');
                if (!string.IsNullOrEmpty(version))
                {
                    versions.Add(version);
                }
            }

            return versions;
        }

        private string GenerateVersionClass(string className, List<string> versions)
        {
            var sb = new StringBuilder();

            sb.AppendLine("namespace MinecraftDataCSharp");
            sb.AppendLine("{");
            sb.AppendLine("    public static class " + className);
            sb.AppendLine("    {");

            var usedNames = new HashSet<string>();

            // Determine latest version (last entry in the list)
            var latestVersion = versions.Last();
            sb.AppendLine($"        public const string Latest = \"{latestVersion}\";");

            foreach (var version in versions)
            {
                var name = ConvertVersionToConstName(version);

                if (!usedNames.Add(name))
                {
                    continue;
                }

                sb.AppendLine($"        public const string {name} = \"{version}\";");
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static string ConvertVersionToConstName(string version)
        {
            // Convert version string to a valid C# identifier
            var name = "V" + version
                .Replace(".", "_") // Replace dots with underscores
                .Replace("-", "_") // Replace dashes with underscores
                .ToUpper(); // Ensure uniform casing

            return name;
        }

        private string GenerateEditionsClass(IEnumerable<string> editions)
        {
            var sb = new StringBuilder();

            sb.AppendLine("namespace MinecraftDataCSharp");
            sb.AppendLine("{");
            sb.AppendLine("    public static class Editions");
            sb.AppendLine("    {");

            foreach (var edition in editions)
            {
                var editionName = ConvertToPascalCase(edition);
                sb.AppendLine($"        public const string {editionName} = \"{edition}\";");
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static string GetEditionFromPath(string filePath)
        {
            var parts = filePath.Split(Path.DirectorySeparatorChar);

            return (
                from part in parts
                where part.Equals("pc", StringComparison.OrdinalIgnoreCase)
                      || part.Equals("bedrock", StringComparison.OrdinalIgnoreCase)
                select part.ToLower()
            ).FirstOrDefault();
        }
    }
}
