namespace MinecraftDataCSharp.Repositories;

public class ItemRepository(IFileApi fileApi)
{
    private IFileApi FileApi { get; } = fileApi;

    private List<Item> Items { get; set; } = [];

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    public async Task<List<Item>> GetAllItems()
    {
        if (Items.Count != 0)
        {
            return Items;
        }

        var fileText = await FileApi.ReadAllText(Constants.ItemsFilePath);

        return Items = JsonSerializer.Deserialize<List<Item>>(fileText) ?? [];
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

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public int StackSize { get; set; }
    public string[] EnchantCategories { get; set; } = [];
    public int MaxDurability { get; set; }
    public string[] RepairWith { get; set; } = [];
}
