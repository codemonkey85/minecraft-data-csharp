namespace MinecraftDataCSharp.Repositories;

public class BlockRepository(IFileApi fileApi)
{
    private IFileApi FileApi { get; } = fileApi;

    private List<Block> Blocks { get; set; } = [];

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    public async Task<List<Block>> GetAllBlocks()
    {
        if (Blocks.Count != 0)
        {
            return Blocks;
        }

        var fileText = await FileApi.ReadAllText(Constants.BlocksFilePath);

        return Blocks = JsonSerializer.Deserialize<List<Block>>(fileText) ?? [];
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

[JsonSerializable(typeof(Block))]
public class Block
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("displayName")] public string DisplayName { get; set; } = string.Empty;
    [JsonPropertyName("hardness")] public float Hardness { get; set; }
    [JsonPropertyName("resistance")] public float Resistance { get; set; }
    [JsonPropertyName("stackSize")] public int StackSize { get; set; }
    [JsonPropertyName("diggable")] public bool Diggable { get; set; }
    [JsonPropertyName("material")] public string Material { get; set; } = string.Empty;
    [JsonPropertyName("transparent")] public bool Transparent { get; set; }
    [JsonPropertyName("emitLight")] public int EmitLight { get; set; }
    [JsonPropertyName("filterLight")] public int FilterLight { get; set; }
    [JsonPropertyName("defaultState")] public int DefaultState { get; set; }
    [JsonPropertyName("minStateId")] public int MinStateId { get; set; }
    [JsonPropertyName("maxStateId")] public int MaxStateId { get; set; }
    [JsonPropertyName("states")] public State[] States { get; set; } = [];
    [JsonPropertyName("drops")] public int?[] Drops { get; set; } = [];
    [JsonPropertyName("boundingBox")] public string BoundingBox { get; set; } = string.Empty;
    [JsonPropertyName("harvestTools")] public HarvestTools HarvestTools { get; set; } = default!;
}

[JsonSerializable(typeof(HarvestTools))]
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

[JsonSerializable(typeof(State))]
public class State
{
    [JsonPropertyName("id")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;
    [JsonPropertyName("num_values")] public int NumValues { get; set; }
    [JsonPropertyName("values")] public string[] Values { get; set; } = [];
}
