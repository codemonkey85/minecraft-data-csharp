namespace MinecraftDataCSharp.Repositories;

public class DataPathResolver
{
    private readonly Dictionary<string, Dictionary<string, Dictionary<string, string>>> _dataPaths;

    public DataPathResolver(string jsonFilePath)
    {
        var json = File.ReadAllText(jsonFilePath);
        _dataPaths = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(json)
                     ?? throw new Exception("Failed to parse dataPaths.json");
    }

    public string? GetFilePath(string edition, string version, string category)
    {
        if (_dataPaths.TryGetValue(edition, out var versions) &&
            versions.TryGetValue(version, out var categories) &&
            categories.TryGetValue(category, out var path))
        {
            return $"{Constants.DataPath}/{path}/{category}.json";
        }

        return null;
    }
}
