namespace MinecraftDataCSharp.Repositories;

public class BiomeRepository(IFileApi fileApi)
{
    private IFileApi FileApi { get; } = fileApi;

    private List<Biome> Biomes { get; set; } = [];

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        TypeInfoResolver = BiomeJsonContext.Default
    };

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    public async Task<List<Biome>> GetAllBiomes()
    {
        if (Biomes.Count != 0)
        {
            return Biomes;
        }

        var fileText = await FileApi.ReadAllText(Constants.BiomesFilePath);

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
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("category")] public string Category { get; set; } = string.Empty;
    [JsonPropertyName("temperature")] public float Temperature { get; set; }

    [JsonPropertyName("has_precipitation")]
    public bool HasPrecipitation { get; set; }

    [JsonPropertyName("dimension")] public string Dimension { get; set; } = string.Empty;
    [JsonPropertyName("displayName")] public string DisplayName { get; set; } = string.Empty;
    [JsonPropertyName("color")] public int Color { get; set; }
}