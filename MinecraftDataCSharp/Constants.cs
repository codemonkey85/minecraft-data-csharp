namespace MinecraftDataCSharp;

public static class Constants
{
    private const string RootPath = "data";
    private const string Edition = "pc";
    private const string Version = "1.20";

    public const string BlocksFilePath =
        $@"{RootPath}\data\{Edition}\{Version}\blocks.json";

    public const string EffectsFilePath =
        $@"{RootPath}\data\{Edition}\{Version}\effects.json";

    public const string ItemsFilePath =
        $@"{RootPath}\data\{Edition}\{Version}\items.json";

    public const string BiomesFilePath =
        $@"{RootPath}\data\{Edition}\{Version}\biomes.json";

    public const string EntitiesFilePath =
        $@"{RootPath}\data\{Edition}\{Version}\entities.json";

    public const string EnchantmentsFilePath =
        $@"{RootPath}\data\{Edition}\{Version}\enchantments.json";
}
