using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WorkerNode.Authentication;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        worker.UseMiddleware<JwtValidationMiddleware>();
    })
    .ConfigureServices(services =>
    {
        //services.AddApplicationInsightsTelemetryWorkerService();
        //services.ConfigureFunctionsApplicationInsights();
    })
    //.ConfigureFunctionsWebApplication()
    .Build();

await host.RunAsync();