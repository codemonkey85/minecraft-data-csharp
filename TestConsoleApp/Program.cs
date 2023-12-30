var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostingContext, services) => services
    .AddSingleton<IFileApi, FileApi>()
    .AddSingleton<BlockRepository>()
    .AddSingleton<EffectRepository>()
    .AddSingleton<ItemRepository>());

using var host = builder.Build();

var itemRepository = host.Services.GetRequiredService<ItemRepository>();

foreach (var item in itemRepository.SearchItemsByName("diamond")
    .Take(10))
{
    Console.WriteLine($"{item.id} - {item.name}");
}

//host.Run();
