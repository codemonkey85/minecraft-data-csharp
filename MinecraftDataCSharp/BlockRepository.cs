namespace MinecraftDataCSharp;

public class BlockRepository(IFileApi fileApi)
{
    private IFileApi FileApi { get; set; } = fileApi;

    private List<Block> blocks = [];

    public async Task<List<Block>> GetAllBlocks()
    {
        if (blocks.Count != 0)
        {
            return blocks;
        }

        var fileText = await FileApi.ReadAllText(Constants.BlocksFilePath);

        return blocks = JsonSerializer.Deserialize<List<Block>>(fileText) ?? [];
    }

    public async Task<Block?> GetBlockById(int id)
    {
        var blocks = await GetAllBlocks();

        return blocks?.FirstOrDefault(block => block.id == id);
    }

    public async Task<Block?> GetBlockByName(string name)
    {
        var blocks = await GetAllBlocks();

        return blocks?.FirstOrDefault(block =>
               block.name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<List<Block>> SearchBlocksByName(string name)
    {
        var blocks = await GetAllBlocks();

        return blocks?
            .Where(block =>
                   block.name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList() ?? [];
    }
}

public class Block
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string displayName { get; set; } = string.Empty;
    public float hardness { get; set; }
    public float resistance { get; set; }
    public int stackSize { get; set; }
    public bool diggable { get; set; }
    public string material { get; set; } = string.Empty;
    public bool transparent { get; set; }
    public int emitLight { get; set; }
    public int filterLight { get; set; }
    public int defaultState { get; set; }
    public int minStateId { get; set; }
    public int maxStateId { get; set; }
    public State[] states { get; set; } = [];
    public int?[] drops { get; set; } = [];
    public string boundingBox { get; set; } = string.Empty;
    public HarvestTools harvestTools { get; set; } = default!;
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
    public string name { get; set; } = string.Empty;
    public string type { get; set; } = string.Empty;
    public int num_values { get; set; }
    public string[] values { get; set; } = [];
}
