namespace MinecraftDataCSharp.Repositories;

public class EntityRepository(IFileApi fileApi, MinecraftDataManager dataManager)
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { TypeInfoResolver = EntityJsonContext.Default };

    private IFileApi FileApi { get; } = fileApi;

    private MinecraftDataManager DataManager { get; } = dataManager;

    private List<Entity> Entities { get; set; } = [];

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    public async Task<List<Entity>> GetAllEntities()
    {
        var filePath = DataManager.GetFilePath(Constants.EntitiesFilePath)
                       ?? throw new FileNotFoundException("Items file path not found for the selected version.");

        var fileText = await FileApi.ReadAllText(filePath);

        return Entities = JsonSerializer.Deserialize<List<Entity>>(fileText, JsonSerializerOptions) ?? [];
    }

    public async Task<Entity?> GetEntityById(int id)
    {
        await GetAllEntities();
        return Entities.FirstOrDefault(entity => entity.Id == id);
    }

    public async Task<Entity?> GetEntityByName(string name)
    {
        await GetAllEntities();
        return Entities.FirstOrDefault(entity =>
            entity.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<List<Entity>> SearchEntitiesByName(string name)
    {
        await GetAllEntities();
        return Entities.Where(entity =>
                entity.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}

[JsonSerializable(typeof(List<Entity>))]
// ReSharper disable once ClassNeverInstantiated.Global
internal partial class EntityJsonContext : JsonSerializerContext;

public class Entity
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("internalId")]
    public int InternalId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("width")]
    public float Width { get; set; }

    [JsonPropertyName("height")]
    public float Height { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("metadataKeys")]
    public string[] MetadataKeys { get; set; } = [];
}
