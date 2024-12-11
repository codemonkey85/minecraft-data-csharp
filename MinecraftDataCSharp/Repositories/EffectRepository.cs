using System.Text.RegularExpressions;

namespace MinecraftDataCSharp.Repositories;

public class EffectRepository(IFileApi fileApi)
{
    private IFileApi FileApi { get; } = fileApi;

    private List<Effect> Effects { get; set; } = [];

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    public async Task<List<Effect>> GetAllEffects()
    {
        if (Effects.Count != 0)
        {
            return [];
        }

        var fileText = await FileApi.ReadAllText(Constants.EffectsFilePath);

        return Effects = JsonSerializer.Deserialize<List<Effect>>(fileText) ?? [];
    }

    public async Task<Effect?> GetEffectById(int id)
    {
        await GetAllEffects();
        return Effects.FirstOrDefault(effect => effect.Id == id);
    }

    public async Task<Effect?> GetEffectByName(string name)
    {
        await GetAllEffects();
        return Effects.FirstOrDefault(effect =>
            effect.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<List<Effect>> SearchEffectsByName(string name)
    {
        await GetAllEffects();
        return Effects.Where(effect =>
                effect.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}

public partial class Effect
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BedrockName => GetBedrockName();
    public string DisplayName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    private string GetBedrockName() =>
        // convert from camelCase to snake_case
        CamelToSnake().Replace(Name, "_$1").ToLower().Remove(0, 1);

    [GeneratedRegex("([A-Z])")]
    private static partial Regex CamelToSnake();
}
