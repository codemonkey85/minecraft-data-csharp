using System.Text.RegularExpressions;

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

public partial class Effect
{
    public int id { get; set; }
    public string name { get; set; }
    public string bedrock_name => GetBedrockName();
    public string displayName { get; set; }
    public string type { get; set; }

    private string GetBedrockName()
    {
        // convert from camelCase to snake_case
        var snakeCase = CamelToSnake().Replace(name, "_$1").ToLower().Remove(0, 1);
        return snakeCase;
    }

    [GeneratedRegex("([A-Z])")]
    private static partial Regex CamelToSnake();
}
