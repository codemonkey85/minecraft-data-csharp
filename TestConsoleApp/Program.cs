foreach (var item in ItemRepository.SearchItemsByName("diamond")
    .Take(10))
{
    Console.WriteLine($"{item.id} - {item.name}");
}
