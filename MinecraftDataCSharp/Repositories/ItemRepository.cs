namespace MinecraftDataCSharp.Repositories;

public class ItemRepository(IFileApi fileApi, MinecraftDataManager dataManager)
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { TypeInfoResolver = ItemJsonContext.Default };

    private IFileApi FileApi { get; } = fileApi;

    private MinecraftDataManager DataManager { get; } = dataManager;

    private List<Item> Items { get; set; } = [];

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    public async Task<List<Item>> GetAllItems()
    {
        var filePath = DataManager.GetFilePath(Constants.ItemsFilePath)
                       ?? throw new FileNotFoundException("Items file path not found for the selected version.");

        var fileText = await FileApi.ReadAllText(filePath);

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

[JsonSerializable(typeof(List<Item>))]
// ReSharper disable once ClassNeverInstantiated.Global
internal partial class ItemJsonContext : JsonSerializerContext;

public class Item
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("stackSize")]
    public int StackSize { get; set; }

    [JsonPropertyName("enchantCategories")]
    public string[] EnchantCategories { get; set; } = [];

    [JsonPropertyName("maxDurability")]
    public int MaxDurability { get; set; }

    [JsonPropertyName("repairWith")]
    public string[] RepairWith { get; set; } = [];
}
