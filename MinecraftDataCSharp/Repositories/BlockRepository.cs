namespace MinecraftDataCSharp.Repositories;

public class BlockRepository(IFileApi fileApi)
{
    private IFileApi FileApi { get; } = fileApi;

    private List<Block> Blocks { get; set; } = [];

    // ReSharper disable once MemberCanBePrivate.Global
    public async Task GetAllBlocks()
    {
        if (Blocks.Count != 0)
        {
            return;
        }

        var fileText = await FileApi.ReadAllText(Constants.BlocksFilePath);

        Blocks = JsonSerializer.Deserialize<List<Block>>(fileText) ?? [];
    }

    public async Task<Block?> GetBlockById(int id)
    {
        await GetAllBlocks();
        return Blocks.FirstOrDefault(block => block.Id == id);
    }

    public async Task<Block?> GetBlockByName(string name)
    {
        await GetAllBlocks();
        return Blocks.FirstOrDefault(block =>
            block.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<List<Block>> SearchBlocksByName(string name)
    {
        await GetAllBlocks();
        return Blocks.Where(block =>
                block.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}

public class Block
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public float Hardness { get; set; }
    public float Resistance { get; set; }
    public int StackSize { get; set; }
    public bool Diggable { get; set; }
    public string Material { get; set; } = string.Empty;
    public bool Transparent { get; set; }
    public int EmitLight { get; set; }
    public int FilterLight { get; set; }
    public int DefaultState { get; set; }
    public int MinStateId { get; set; }
    public int MaxStateId { get; set; }
    public State[] States { get; set; } = [];
    public int?[] Drops { get; set; } = [];
    public string BoundingBox { get; set; } = string.Empty;
    public HarvestTools HarvestTools { get; set; } = default!;
}

public class HarvestTools
{
    public bool _779 { get; set; }
    public bool _784 { get; set; }
    public bool _789 { get; set; }
    public bool _794 { get; set; }
    public bool _799 { get; set; }
    public bool _804 { get; set; }
    public bool _777 { get; set; }
    public bool _782 { get; set; }
    public bool _787 { get; set; }
    public bool _792 { get; set; }
    public bool _797 { get; set; }
    public bool _802 { get; set; }
    public bool _942 { get; set; }
    public bool _778 { get; set; }
    public bool _783 { get; set; }
    public bool _788 { get; set; }
    public bool _793 { get; set; }
    public bool _798 { get; set; }
    public bool _803 { get; set; }
}

public class State
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int NumValues { get; set; }
    public string[] Values { get; set; } = [];
}
