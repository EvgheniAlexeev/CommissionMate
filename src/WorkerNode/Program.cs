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
        //worker.UseMiddleware<JwtValidationMiddleware>();
    })
    .ConfigureServices(services =>
    {
        services.AddScoped<IUserRepository, UserRepository>();
        //services.AddApplicationInsightsTelemetryWorkerService();
        //services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

await host.RunAsync();