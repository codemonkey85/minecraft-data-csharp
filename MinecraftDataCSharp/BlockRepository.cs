using System.Text.Json;

namespace MinecraftDataCSharp;

public static class BlockRepository
{
    public static List<Block> GetBlocks()
    {
        const string blocksFilePath =
            @"C:\git\minecraft-data-csharp\minecraft_data\data\pc\1.20\blocks.json";

        var fileText = File.ReadAllText(blocksFilePath);

        var blocks = JsonSerializer.Deserialize<List<Block>>(fileText);

        if (blocks is null)
        {
            Console.WriteLine("Failed to deserialize blocks file");
            return [];
        }

        return blocks;
    }
}

public class Block
{
    public int id { get; set; }
    public string name { get; set; }
    public string displayName { get; set; }
    public float hardness { get; set; }
    public float resistance { get; set; }
    public int stackSize { get; set; }
    public bool diggable { get; set; }
    public string material { get; set; }
    public bool transparent { get; set; }
    public int emitLight { get; set; }
    public int filterLight { get; set; }
    public int defaultState { get; set; }
    public int minStateId { get; set; }
    public int maxStateId { get; set; }
    public State[] states { get; set; }
    public int?[] drops { get; set; }
    public string boundingBox { get; set; }
    public Harvesttools harvestTools { get; set; }
}

public class Harvesttools
{
    public bool _779 { get; set; }
    public bool _784 { get; set; }
    public bool _789 { get; set; }
    public bool _794 { get; set; }
    public bool _799 { get; set; }
    public bool _804 { get; set; }
    public bool _777 { get; set; }
    public bool _782 { get; set; }
    public bool _787 { get; set; }
    public bool _792 { get; set; }
    public bool _797 { get; set; }
    public bool _802 { get; set; }
    public bool _942 { get; set; }
    public bool _778 { get; set; }
    public bool _783 { get; set; }
    public bool _788 { get; set; }
    public bool _793 { get; set; }
    public bool _798 { get; set; }
    public bool _803 { get; set; }
}

public class State
{
    public string name { get; set; }
    public string type { get; set; }
    public int num_values { get; set; }
    public string[] values { get; set; }
}
