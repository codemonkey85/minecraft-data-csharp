namespace MinecraftDataCSharp;

public class ItemRepository(IFileApi fileApi)
{
    private IFileApi FileApi { get; set; } = fileApi;

    private List<Item> items = [];

    public async Task<List<Item>> GetAllItems()
    {
        if (items.Count != 0)
        {
            return items;
        }

        var fileText = await FileApi.ReadAllText(Constants.ItemsFilePath);

        return items = JsonSerializer.Deserialize<List<Item>>(fileText) ?? [];
    }

    public async Task<Item?> GetItemById(int id)
    {
        var items = await GetAllItems();

        return items?.FirstOrDefault(item => item.id == id);
    }

    public async Task<Item?> GetItemByName(string name)
    {
        var items = await GetAllItems();

        return items?.FirstOrDefault(item =>
               item.name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<List<Item>> SearchItemsByName(string name)
    {
        var items = await GetAllItems();

        return items?
            .Where(item =>
                   item.name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList() ?? [];
    }
}

public class Item
{
    public int id { get; set; }
    public string name { get; set; }
    public string displayName { get; set; }
    public int stackSize { get; set; }
    public string[] enchantCategories { get; set; }
    public int maxDurability { get; set; }
    public string[] repairWith { get; set; }
}
