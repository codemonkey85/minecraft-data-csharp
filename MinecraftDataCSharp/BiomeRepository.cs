namespace MinecraftDataCSharp;

public class BiomeRepository(IFileApi fileApi)
{
    private IFileApi FileApi { get; set; } = fileApi;

    private List<Biome> biomes = [];

    public async Task<List<Biome>> GetAllBiomes()
    {
        if (biomes.Count != 0)
        {
            return biomes;
        }

        var fileText = await FileApi.ReadAllText(Constants.BiomesFilePath);

        return biomes = JsonSerializer.Deserialize<List<Biome>>(fileText) ?? [];
    }

    public async Task<Biome?> GetBiomeById(int id)
    {
        var biomes = await GetAllBiomes();

        return biomes?.FirstOrDefault(biome => biome.id == id);
    }

    public async Task<Biome?> GetBiomeByName(string name)
    {
        var biomes = await GetAllBiomes();

        return biomes?.FirstOrDefault(biome =>
               biome.name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<List<Biome>> SearchBiomesByName(string name)
    {
        var biomes = await GetAllBiomes();

        return biomes?
            .Where(biome =>
                   biome.name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList() ?? [];
    }
}

public class Biome
{
    public int id { get; set; }
    public string name { get; set; }
    public string category { get; set; }
    public float temperature { get; set; }
    public bool has_precipitation { get; set; }
    public string dimension { get; set; }
    public string displayName { get; set; }
    public int color { get; set; }
}
