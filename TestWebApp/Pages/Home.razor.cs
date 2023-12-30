namespace TestWebApp.Pages;

public partial class Home
{
    private async void ButtonClicked()
    {
        var items = await ItemRepository.SearchItemsByName("diamond");

        foreach (var item in items
            .Take(10))
        {
            Console.WriteLine($"{item.id} - {item.name}");
        }
    }
}
