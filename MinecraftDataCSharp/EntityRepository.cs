namespace MinecraftDataCSharp;

public class EntityRepository(IFileApi fileApi)
{
    private IFileApi FileApi { get; set; } = fileApi;

    private List<Entity> entities = [];

    public async Task<List<Entity>> GetAllEntities()
    {
        if (entities.Count != 0)
        {
            return entities;
        }

        var fileText = await FileApi.ReadAllText(Constants.EntitiesFilePath);

        return entities = JsonSerializer.Deserialize<List<Entity>>(fileText) ?? [];
    }

    public async Task<Entity?> GetEntityById(int id)
    {
        var entities = await GetAllEntities();

        return entities?.FirstOrDefault(entity => entity.id == id);
    }

    public async Task<Entity?> GetEntityByName(string name)
    {
        var entities = await GetAllEntities();

        return entities?.FirstOrDefault(entity =>
               entity.name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<List<Entity>> SearchEntitiesByName(string name)
    {
        var entities = await GetAllEntities();

        return entities?
            .Where(entity =>
                   entity.name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList() ?? [];
    }
}

public class Entity
{
    public int id { get; set; }
    public int internalId { get; set; }
    public string name { get; set; }
    public string displayName { get; set; }
    public float width { get; set; }
    public float height { get; set; }
    public string type { get; set; }
    public string category { get; set; }
    public string[] metadataKeys { get; set; }
}
