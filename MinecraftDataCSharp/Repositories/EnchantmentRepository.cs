namespace MinecraftDataCSharp.Repositories;

public class EnchantmentRepository(IFileApi fileApi)
{
    private IFileApi FileApi { get; } = fileApi;

    private List<Enchantment> Enchantments { get; set; } = [];

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    public async Task<List<Enchantment>> GetAllEnchantments()
    {
        if (Enchantments.Count != 0)
        {
            return Enchantments;
        }

        var fileText = await FileApi.ReadAllText(Constants.EnchantmentsFilePath);

        return Enchantments = JsonSerializer.Deserialize<List<Enchantment>>(fileText) ?? [];
    }

    public async Task<Enchantment?> GetEnchantmentById(int id)
    {
        await GetAllEnchantments();
        return Enchantments.FirstOrDefault(enchantment => enchantment.Id == id);
    }

    public async Task<Enchantment?> GetEnchantmentByName(string name)
    {
        await GetAllEnchantments();
        return Enchantments.FirstOrDefault(enchantment =>
            enchantment.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<List<Enchantment>> SearchEnchantmentsByName(string name)
    {
        await GetAllEnchantments();
        return Enchantments.Where(enchantment =>
                enchantment.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}

[JsonSerializable(typeof(Enchantment))]
public class Enchantment
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("displayName")] public string DisplayName { get; set; } = string.Empty;
    [JsonPropertyName("maxLevel")] public int MaxLevel { get; set; }
    [JsonPropertyName("minCost")] public Mincost MinCost { get; set; } = default!;
    [JsonPropertyName("maxCost")] public Maxcost MaxCost { get; set; } = default!;
    [JsonPropertyName("treasureOnly")] public bool TreasureOnly { get; set; }
    [JsonPropertyName("curse")] public bool Curse { get; set; }
    [JsonPropertyName("exclude")] public string[] Exclude { get; set; } = [];
    [JsonPropertyName("category")] public string Category { get; set; } = string.Empty;
    [JsonPropertyName("weight")] public int Weight { get; set; }
    [JsonPropertyName("tradeable")] public bool Tradeable { get; set; }
    [JsonPropertyName("discoverable")] public bool Discoverable { get; set; }
}

[JsonSerializable(typeof(Mincost))]
public class Mincost
{
    [JsonPropertyName("a")] public int A { get; set; }
    [JsonPropertyName("b")] public int B { get; set; }
}

[JsonSerializable(typeof(Maxcost))]
public class Maxcost
{
    [JsonPropertyName("a")] public int A { get; set; }
    [JsonPropertyName("b")] public int B { get; set; }
}
