foreach (var block in BlockRepository.SearchBlocksByName("diamond")
    .Take(10))
{
    Console.WriteLine($"{block.id} - {block.name}");
}
