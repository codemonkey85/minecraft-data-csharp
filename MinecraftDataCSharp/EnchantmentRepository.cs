namespace MinecraftDataCSharp;

public class EnchantmentRepository(IFileApi fileApi)
{
    private IFileApi FileApi { get; set; } = fileApi;

    private List<Enchantment> enchantments = [];

    public async Task<List<Enchantment>> GetAllEnchantments()
    {
        if (enchantments.Count != 0)
        {
            return enchantments;
        }

        var fileText = await FileApi.ReadAllText(Constants.EnchantmentsFilePath);

        return enchantments = JsonSerializer.Deserialize<List<Enchantment>>(fileText) ?? [];
    }

    public async Task<Enchantment?> GetEnchantmentById(int id)
    {
        var enchantments = await GetAllEnchantments();

        return enchantments?.FirstOrDefault(enchantment => enchantment.id == id);
    }

    public async Task<Enchantment?> GetEnchantmentByName(string name)
    {
        var enchantments = await GetAllEnchantments();

        return enchantments?.FirstOrDefault(enchantment =>
                      enchantment.name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<List<Enchantment>> SearchEnchantmentsByName(string name)
    {
        var enchantments = await GetAllEnchantments();

        return enchantments?
            .Where(enchantment =>
                              enchantment.name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList() ?? [];
    }
}

public class Enchantment
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string displayName { get; set; } = string.Empty;
    public int maxLevel { get; set; }
    public Mincost minCost { get; set; } = default!;
    public Maxcost maxCost { get; set; } = default!;
    public bool treasureOnly { get; set; }
    public bool curse { get; set; }
    public string[] exclude { get; set; } = [];
    public string category { get; set; } = string.Empty;
    public int weight { get; set; }
    public bool tradeable { get; set; }
    public bool discoverable { get; set; }
}

public class Mincost
{
    public int a { get; set; }
    public int b { get; set; }
}

public class Maxcost
{
    public int a { get; set; }
    public int b { get; set; }
}
