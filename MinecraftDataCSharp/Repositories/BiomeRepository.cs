namespace MinecraftDataCSharp.Repositories;

public class BiomeRepository(IFileApi fileApi, MinecraftDataManager dataManager)
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { TypeInfoResolver = BiomeJsonContext.Default };

    private IFileApi FileApi { get; } = fileApi;

    private MinecraftDataManager DataManager { get; } = dataManager;

    private List<Biome> Biomes { get; set; } = [];

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    public async Task<List<Biome>> GetAllBiomes()
    {
        var filePath = DataManager.GetFilePath(Constants.BiomesFilePath)
                       ?? throw new FileNotFoundException("Items file path not found for the selected version.");

        var fileText = await FileApi.ReadAllText(filePath);

        return Biomes = JsonSerializer.Deserialize<List<Biome>>(fileText, JsonSerializerOptions) ?? [];
    }

    public async Task<Biome?> GetBiomeById(int id)
    {
        await GetAllBiomes();
        return Biomes.FirstOrDefault(biome => biome.Id == id);
    }

    public async Task<Biome?> GetBiomeByName(string name)
    {
        await GetAllBiomes();
        return Biomes.FirstOrDefault(biome =>
            biome.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<List<Biome>> SearchBiomesByName(string name)
    {
        await GetAllBiomes();
        return Biomes.Where(biome =>
                biome.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}

[JsonSerializable(typeof(List<Biome>))]
// ReSharper disable once ClassNeverInstantiated.Global
internal partial class BiomeJsonContext : JsonSerializerContext;

public class Biome
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("temperature")]
    public float Temperature { get; set; }

    [JsonPropertyName("has_precipitation")]
    public bool HasPrecipitation { get; set; }

    [JsonPropertyName("dimension")]
    public string Dimension { get; set; } = string.Empty;

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("color")]
    public int Color { get; set; }
}
