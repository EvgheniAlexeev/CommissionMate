using IsolatedFunctionAuth.Middleware;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkerNode.Middleware;
using WorkerNode.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        worker.UseMiddleware<AuthenticationMiddleware>();
        worker.UseMiddleware<AuthorizationMiddleware>();
    })
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddScoped<IUserRepository, UserRepository>();
    })
    .Build();

await host.RunAsync();