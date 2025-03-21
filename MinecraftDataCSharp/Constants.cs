namespace MinecraftDataCSharp;

public static class Constants
{
    public const string DefaultEdition = Editions.Pc;

    public const string DefaultPcVersion = PcVersions.Latest;

    public const string DefaultBedrockVersion = BedrockVersions.Latest;

    public const string DataPath = "data/data/";

    public const string DataPathsJson = $"{DataPath}dataPaths.json";

    public const string BlocksFilePath = "blocks";

    public const string EffectsFilePath = "effects";

    public const string ItemsFilePath = "items";

    public const string BiomesFilePath = "biomes";

    public const string EntitiesFilePath = "entities";

    public const string EnchantmentsFilePath = "enchantments";
}

public static class Editions
{
    public const string Pc = "pc";

    public const string Bedrock = "bedrock";
}
