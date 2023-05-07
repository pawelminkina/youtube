using Application;
using Application.Services.ToDoItems;
using BenchmarkDotNet.Attributes;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Benchmark;

[MemoryDiagnoser()]
public class ToDoBenchmark
{
    private IToDoItemsService _toDoItemsService;
    [GlobalSetup]
    public void GlobalSetup()
    {
        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var collection = new ServiceCollection();
        collection.AddInfrastructure(configuration);
        collection.AddApplication();
        collection.AddSingleton(configuration);
        collection.AddLogging(s => s.AddSerilog(logger));
        var serviceProvider = collection.BuildServiceProvider();
        _toDoItemsService = serviceProvider.GetService<IToDoItemsService>()!;
    }

    [Benchmark()]
    public async Task PublishAllItems()
    {
        await _toDoItemsService.PublishAllToDoItems(CancellationToken.None);
    }
}