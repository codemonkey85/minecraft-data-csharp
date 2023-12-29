namespace MinecraftDataCSharp;

public static class ItemRepository
{
    private static List<Item> items = [];

    public static List<Item> GetAllItems()
    {
        if (items.Count != 0)
        {
            return items;
        }

        var fileText = File.ReadAllText(Constants.ItemsFilePath);

        return items = JsonSerializer.Deserialize<List<Item>>(fileText) ?? [];
    }

    public static Item? GetItemById(int id)
    {
        var items = GetAllItems();

        return items?.FirstOrDefault(item => item.id == id);
    }

    public static Item? GetItemByName(string name)
    {
        var items = GetAllItems();

        return items?.FirstOrDefault(item =>
               item.name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public static List<Item> SearchItemsByName(string name)
    {
        var items = GetAllItems();

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
