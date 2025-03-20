namespace MinecraftDataCSharp.Repositories;

public class DataPathResolver(IServiceProvider serviceProvider)
{
    private IServiceProvider ServiceProvider { get; set; } = serviceProvider;

    private Dictionary<string, Dictionary<string, Dictionary<string, string>>> _dataPaths = [];

    public async Task Initialize()
    {
        using var scope = ServiceProvider.CreateScope();
        var fileApi = scope.ServiceProvider.GetRequiredService<IFileApi>();

        var json = await fileApi.ReadAllText(Constants.DataPathsJson);
        _dataPaths = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(json)
                     ?? throw new Exception("Failed to parse dataPaths.json");
    }

    public string? GetFilePath(string edition, string version, string category) => _dataPaths.TryGetValue(edition, out var versions) &&
            versions.TryGetValue(version, out var categories) &&
            categories.TryGetValue(category, out var path)
            ? $"{Constants.DataPath}/{path}/{category}.json"
            : null;
}
