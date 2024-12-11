namespace MinecraftDataCSharp.Repositories;

public class EnchantmentRepository(IFileApi fileApi)
{
    private IFileApi FileApi { get; } = fileApi;

    private List<Enchantment> Enchantments { get; set; } = [];

    // ReSharper disable once MemberCanBePrivate.Global
    public async Task GetAllEnchantments()
    {
        if (Enchantments.Count != 0)
        {
            return;
        }

        var fileText = await FileApi.ReadAllText(Constants.EnchantmentsFilePath);

        Enchantments = JsonSerializer.Deserialize<List<Enchantment>>(fileText) ?? [];
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

public class Enchantment
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public int MaxLevel { get; set; }
    public Mincost MinCost { get; set; } = default!;
    public Maxcost MaxCost { get; set; } = default!;
    public bool TreasureOnly { get; set; }
    public bool Curse { get; set; }
    public string[] Exclude { get; set; } = [];
    public string Category { get; set; } = string.Empty;
    public int Weight { get; set; }
    public bool Tradeable { get; set; }
    public bool Discoverable { get; set; }
}

public class Mincost
{
    public int A { get; set; }
    public int B { get; set; }
}

public class Maxcost
{
    public int A { get; set; }
    public int B { get; set; }
}
