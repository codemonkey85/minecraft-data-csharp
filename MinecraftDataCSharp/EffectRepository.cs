namespace MinecraftDataCSharp;

public class EffectRepository(IFileApi fileApi)
{
    private List<Effect> effects = [];

    public List<Effect> GetAllEffects()
    {
        if (effects.Count != 0)
        {
            return effects;
        }

        var fileText = File.ReadAllText(Constants.EffectsFilePath);

        return effects = JsonSerializer.Deserialize<List<Effect>>(fileText) ?? [];
    }

    public Effect? GetEffectById(int id)
    {
        var effects = GetAllEffects();

        return effects?.FirstOrDefault(effect => effect.id == id);
    }

    public Effect? GetEffectByName(string name)
    {
        var effects = GetAllEffects();

        return effects?.FirstOrDefault(effect =>
               effect.name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public List<Effect> SearchEffectsByName(string name)
    {
        var effects = GetAllEffects();

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
