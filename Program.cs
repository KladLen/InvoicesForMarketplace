using InvoicesForMarketplace.APIClient.APIEmag;
using InvoicesForMarketplace.APIClient.APIFakturownia;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IAPIEmag, APIEmag>();
        services.AddScoped<IAPIFakturownia, APIFakturownia>();
    })
    .Build();

host.Run();
