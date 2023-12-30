namespace MinecraftDataCSharp;

public class EffectRepository(IFileApi fileApi)
{
    private IFileApi FileApi { get; set; } = fileApi;

    private List<Effect> effects = [];

    public async Task<List<Effect>> GetAllEffects()
    {
        if (effects.Count != 0)
        {
            return effects;
        }

        var fileText = await FileApi.ReadAllText(Constants.EffectsFilePath);

        return effects = JsonSerializer.Deserialize<List<Effect>>(fileText) ?? [];
    }

    public async Task<Effect?> GetEffectById(int id)
    {
        var effects = await GetAllEffects();

        return effects?.FirstOrDefault(effect => effect.id == id);
    }

    public async Task<Effect?> GetEffectByName(string name)
    {
        var effects = await GetAllEffects();

        return effects?.FirstOrDefault(effect =>
               effect.name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<List<Effect>> SearchEffectsByName(string name)
    {
        var effects = await GetAllEffects();

        return effects?
            .Where(effect =>
                   effect.name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList() ?? [];
    }
}

public class Effect
{
    public int id { get; set; }
    public string name { get; set; }
    public string displayName { get; set; }
    public string type { get; set; }
}
