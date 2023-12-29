using MinecraftDataCSharp;

Console.WriteLine("Hello, World!");

var blocks = BlockRepository.GetBlocks();

if (blocks is null)
{
    Console.WriteLine("Failed to deserialize blocks file");
    return;
}

foreach (var block in blocks
    .Where(b => b.name.Contains("void", StringComparison.OrdinalIgnoreCase))
    .Take(10))
{
    Console.WriteLine($"{block.id} - {block.name}");
}
