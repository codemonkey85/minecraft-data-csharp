﻿using System.Text.RegularExpressions;

namespace MinecraftDataCSharp.Repositories;

public class EffectRepository(IFileApi fileApi, MinecraftDataManager dataManager)
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { TypeInfoResolver = EffectJsonContext.Default };

    private IFileApi FileApi { get; } = fileApi;

    private MinecraftDataManager DataManager { get; } = dataManager;

    private List<Effect> Effects { get; set; } = [];

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedMethodReturnValue.Global
    public async Task<List<Effect>> GetAllEffects()
    {
        var filePath = DataManager.GetFilePath(Constants.EffectsFilePath)
                       ?? throw new FileNotFoundException("Items file path not found for the selected version.");

        var fileText = await FileApi.ReadAllText(filePath);

        return Effects = JsonSerializer.Deserialize<List<Effect>>(fileText, JsonSerializerOptions) ?? [];
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
        return
        [
            .. Effects.Where(effect =>
                effect.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
        ];
    }
}

[JsonSerializable(typeof(List<Effect>))]
// ReSharper disable once ClassNeverInstantiated.Global
internal partial class EffectJsonContext : JsonSerializerContext;

public partial class Effect
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("bedrock_name")]
    public string BedrockName => GetBedrockName();

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    private string GetBedrockName() =>
        // convert from camelCase to snake_case
        CamelToSnake().Replace(Name, "_$1").ToLower().Remove(0, 1);

    [GeneratedRegex("([A-Z])")]
    private static partial Regex CamelToSnake();
}
