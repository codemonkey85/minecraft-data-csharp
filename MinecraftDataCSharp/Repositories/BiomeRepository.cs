namespace MinecraftDataCSharp.Repositories;

public class BiomeRepository(IFileApi fileApi)
{
    private IFileApi FileApi { get; } = fileApi;

    private List<Biome> Biomes { get; set; } = [];

    // ReSharper disable once MemberCanBePrivate.Global
    public async Task GetAllBiomes()
    {
        if (Biomes.Count != 0)
        {
            return;
        }

        var fileText = await FileApi.ReadAllText(Constants.BiomesFilePath);

        Biomes = JsonSerializer.Deserialize<List<Biome>>(fileText) ?? [];
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

public class Biome
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public float Temperature { get; set; }
    public bool HasPrecipitation { get; set; }
    public string Dimension { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public int Color { get; set; }
}
