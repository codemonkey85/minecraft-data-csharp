namespace MinecraftDataCSharp;

public static class Constants
{
    public const string RootPath = @"data";
    public const string Edition = "pc";
    public const string Version = "1.20";

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
}
