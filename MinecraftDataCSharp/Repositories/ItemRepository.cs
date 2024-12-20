namespace MinecraftDataCSharp.Repositories;

public class ItemRepository(IFileApi fileApi)
{
    private IFileApi FileApi { get; } = fileApi;

    private List<Item> Items { get; set; } = [];

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        TypeInfoResolver = ItemJsonContext.Default
    };

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    public async Task<List<Item>> GetAllItems()
    {
        if (Items.Count != 0)
        {
            return Items;
        }

        var fileText = await FileApi.ReadAllText(Constants.ItemsFilePath);

        return Items = JsonSerializer.Deserialize<List<Item>>(fileText, JsonSerializerOptions) ?? [];
    }

    public async Task<Item?> GetItemById(int id)
    {
        await GetAllItems();
        return Items.FirstOrDefault(item => item.Id == id);
    }

    public async Task<Item?> GetItemByName(string name)
    {
        await GetAllItems();
        return Items.FirstOrDefault(item =>
            item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<List<Item>> SearchItemsByName(string name)
    {
        await GetAllItems();
        return Items.Where(item =>
                item.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}

[JsonSerializable(typeof(Item))]
// ReSharper disable once ClassNeverInstantiated.Global
internal partial class ItemJsonContext : JsonSerializerContext;

public class Item
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("displayName")] public string DisplayName { get; set; } = string.Empty;
    [JsonPropertyName("stackSize")] public int StackSize { get; set; }

    [JsonPropertyName("enchantCategories")]
    public string[] EnchantCategories { get; set; } = [];

    [JsonPropertyName("maxDurability")] public int MaxDurability { get; set; }
    [JsonPropertyName("repairWith")] public string[] RepairWith { get; set; } = [];
}