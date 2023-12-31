var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostingContext, services) => services
    .AddScoped<IFileApi, FileApi>()
    .AddScoped<BlockRepository>()
    .AddScoped<EffectRepository>()
    .AddScoped<ItemRepository>()
    .AddScoped<BiomeRepository>()
    .AddScoped<EntityRepository>());

using var host = builder.Build();

var itemRepository = host.Services.GetRequiredService<ItemRepository>();

var items = await itemRepository.SearchItemsByName("diamond");

foreach (var item in items
    .Take(10))
{
    Console.WriteLine($"{item.id} - {item.name}");
}
