namespace MinecraftDataCSharp.Repositories;

public class BlockRepository(IFileApi fileApi, MinecraftDataManager dataManager)
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { TypeInfoResolver = BlockJsonContext.Default };

    private IFileApi FileApi { get; } = fileApi;

    private MinecraftDataManager DataManager { get; } = dataManager;

    private List<Block> Blocks { get; set; } = [];

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    public async Task<List<Block>> GetAllBlocks()
    {
        var filePath = DataManager.GetFilePath(Constants.BlocksFilePath)
                       ?? throw new FileNotFoundException("Items file path not found for the selected version.");

        var fileText = await FileApi.ReadAllText(filePath);

        return Blocks = JsonSerializer.Deserialize<List<Block>>(fileText, JsonSerializerOptions) ?? [];
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
        return
        [
            .. Blocks.Where(block =>
                block.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
        ];
    }
}

[JsonSerializable(typeof(List<Block>))]
// ReSharper disable once ClassNeverInstantiated.Global
internal partial class BlockJsonContext : JsonSerializerContext;

public class Block
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("hardness")]
    public float Hardness { get; set; }

    [JsonPropertyName("resistance")]
    public float Resistance { get; set; }

    [JsonPropertyName("stackSize")]
    public int StackSize { get; set; }

    [JsonPropertyName("diggable")]
    public bool Diggable { get; set; }

    [JsonPropertyName("material")]
    public string Material { get; set; } = string.Empty;

    [JsonPropertyName("transparent")]
    public bool Transparent { get; set; }

    [JsonPropertyName("emitLight")]
    public int EmitLight { get; set; }

    [JsonPropertyName("filterLight")]
    public int FilterLight { get; set; }

    [JsonPropertyName("defaultState")]
    public int DefaultState { get; set; }

    [JsonPropertyName("minStateId")]
    public int MinStateId { get; set; }

    [JsonPropertyName("maxStateId")]
    public int MaxStateId { get; set; }

    [JsonPropertyName("states")]
    public State[] States { get; set; } = [];

    [JsonPropertyName("drops")]
    public int?[] Drops { get; set; } = [];

    [JsonPropertyName("boundingBox")]
    public string BoundingBox { get; set; } = string.Empty;

    [JsonPropertyName("harvestTools")]
    public HarvestTools HarvestTools { get; set; } = null!;
}

public class HarvestTools
{
    [JsonPropertyName("_779")]
    public bool _779 { get; set; }

    [JsonPropertyName("_784")]
    public bool _784 { get; set; }

    [JsonPropertyName("_789")]
    public bool _789 { get; set; }

    [JsonPropertyName("_794")]
    public bool _794 { get; set; }

    [JsonPropertyName("_799")]
    public bool _799 { get; set; }

    [JsonPropertyName("_804")]
    public bool _804 { get; set; }

    [JsonPropertyName("_777")]
    public bool _777 { get; set; }

    [JsonPropertyName("_782")]
    public bool _782 { get; set; }

    [JsonPropertyName("_787")]
    public bool _787 { get; set; }

    [JsonPropertyName("_792")]
    public bool _792 { get; set; }

    [JsonPropertyName("_797")]
    public bool _797 { get; set; }

    [JsonPropertyName("_802")]
    public bool _802 { get; set; }

    [JsonPropertyName("_942")]
    public bool _942 { get; set; }

    [JsonPropertyName("_778")]
    public bool _778 { get; set; }

    [JsonPropertyName("_783")]
    public bool _783 { get; set; }

    [JsonPropertyName("_788")]
    public bool _788 { get; set; }

    [JsonPropertyName("_793")]
    public bool _793 { get; set; }

    [JsonPropertyName("_798")]
    public bool _798 { get; set; }

    [JsonPropertyName("_803")]
    public bool _803 { get; set; }
}

public class State
{
    [JsonPropertyName("id")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("num_values")]
    public int NumValues { get; set; }

    [JsonPropertyName("values")]
    public string[] Values { get; set; } = [];
}
