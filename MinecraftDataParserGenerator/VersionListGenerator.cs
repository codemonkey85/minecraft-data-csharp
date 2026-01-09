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

        private static string ConvertToPascalCase(string input) => string.IsNullOrEmpty(input)
            ? string.Empty
            : char.ToUpper(input[0]) + input.Substring(1).ToLower();

        private static List<string> ParseVersionsJson(string json)
        {
            // Remove brackets
            json = json.Trim().TrimStart('[').TrimEnd(']');

            // Split by commas and remove quotes

            return json
                .Split(',')
                .Select(item => item.Trim().Trim('"'))
                .Where(version => !string.IsNullOrEmpty(version))
                .ToList();
        }

        private static string GenerateVersionClass(string className, List<string> versions)
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

            sb.AppendLine();
            sb.AppendLine("        public static IEnumerable<string> GetAll()");
            sb.AppendLine("        {");

            foreach (var version in versions)
            {
                var name = ConvertVersionToConstName(version);

                if (name == "Latest")
                {
                    continue;
                }

                if (!usedNames.Contains(name))
                {
                    continue;
                }

                sb.AppendLine($"            yield return {name};");
            }

            sb.AppendLine("        }");

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

        private static string GenerateEditionsClass(IEnumerable<string> editions)
        {
            var sb = new StringBuilder();
            var editionsList = editions.ToList();

            sb.AppendLine("namespace MinecraftDataCSharp");
            sb.AppendLine("{");
            sb.AppendLine("    public static class Editions");
            sb.AppendLine("    {");

            foreach (var edition in editionsList)
            {
                var editionName = ConvertToPascalCase(edition);
                sb.AppendLine($"        public const string {editionName} = \"{edition}\";");
            }

            sb.AppendLine();
            sb.AppendLine("        public static IEnumerable<string> GetAll()");
            sb.AppendLine("        {");

            foreach (var edition in editionsList)
            {
                var editionName = ConvertToPascalCase(edition);
                sb.AppendLine($"            yield return {editionName};");
            }

            sb.AppendLine("        }");

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static string GetEditionFromPath(string filePath)
        {
            // Extract the folder name dynamically
            var parts = filePath.Split(Path.DirectorySeparatorChar);

            // Assuming the folder structure is like: ".../pc/common/versions.json"
            // We take the folder that contains "common"
            for (var i = 0; i < parts.Length - 1; i++)
            {
                if (parts[i].Equals("common", StringComparison.OrdinalIgnoreCase))
                {
                    return parts[i - 1].ToLower(); // The parent folder should be the edition
                }
            }

            return null; // If no valid edition is found
        }
    }
}
