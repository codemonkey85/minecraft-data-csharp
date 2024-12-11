namespace MinecraftDataCSharp.Repositories;

public class EntityRepository(IFileApi fileApi)
{
    private IFileApi FileApi { get; } = fileApi;

    private List<Entity> Entities { get; set; } = [];

    // ReSharper disable once MemberCanBePrivate.Global
    public async Task GetAllEntities()
    {
        if (Entities.Count != 0)
        {
            return;
        }

        var fileText = await FileApi.ReadAllText(Constants.EntitiesFilePath);

        Entities = JsonSerializer.Deserialize<List<Entity>>(fileText) ?? [];
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

public class Entity
{
    public int Id { get; set; }
    public int InternalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public float Width { get; set; }
    public float Height { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string[] MetadataKeys { get; set; } = [];
}
