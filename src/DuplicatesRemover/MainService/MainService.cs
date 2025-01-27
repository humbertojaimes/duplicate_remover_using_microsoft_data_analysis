using DuplicatesRemover.Data;

namespace DuplicatesRemover.MainService;

public class MainService(SalesService salesService) : IMainService
{
    public async Task Run()
    {
        Console.WriteLine("MyService is running...");
        await salesService.ProcessData();
    }
}