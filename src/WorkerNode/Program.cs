
using DataLayer.Repositories;

using Domain;
using Domain.Configurations;

using IsolatedFunctionAuth.Middleware;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;

using Smtp.Infrastructure;
using Smtp.Infrastructure.Services;

using WorkerNode.Azure.Models;
using WorkerNode.Azure.Web;
using WorkerNode.Extensions;
using WorkerNode.Handlers;
using WorkerNode.Middleware;
using WorkerNode.Providers;
using WorkerNode.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        worker.UseMiddleware<ExceptionHandlingMiddleware>();
        worker.UseMiddleware<AuthenticationMiddleware>();
        worker.UseMiddleware<AuthorizationMiddleware>();
    })
    .ConfigureServices(services =>
    {
        var httpClient = services.AddHttpClient()
            .ConfigureHttpClientDefaults(configure => configure.AddPolicyHandler(RetryPolicy.GetRetryPolicy()));

        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
#if DEBUG
            .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
#endif
            .AddEnvironmentVariables()
            .Build();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Commission Calc API", Version = "v1" });
            c.IgnoreObsoleteProperties();
        });

        services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
            new OpenApiConfigurationOptions
            {
                Info = new OpenApiInfo
                {
                    Title = "Commission Calc API",
                    Version = "1.0.0",
                    Description = "API for Insight Commission Calc",
                    Contact = new OpenApiContact
                    {
                        Name = "Amdaris",
                        Email = "evgheni.alexeev@amdaris.com",
                        Url = new Uri("https://amdaris.com"),
                    }
                },
                OpenApiVersion = OpenApiVersionType.V3
            });

        services.AddSingleton(configuration);
        services.Configure<AzureAd>(configuration.GetSection(nameof(AzureAd)));

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddScoped<IUserRepository, UserRepositoryStub>();
        services.AddScoped<ICommissionProvider, CommissionProvider>();

        services.AddMicrosoftIdentityWebAppAuthentication(configuration, nameof(AzureAd))
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddDownstreamApi(AppConstants.PowerAutomateApiGetCommissions, configuration.GetSection(AppConstants.PowerAutomateApiGetCommissionsSection))
                .AddDownstreamApi(AppConstants.PowerAutomateApiGetWeatherForecast, configuration.GetSection(AppConstants.PowerAutomateApiGetWeatherForecastSection))
                .AddInMemoryTokenCaches();

#if DEBUG
        services.AddTransient<ITokenAcquisition, DefaultAzureTokenAcquisition>();
#endif
        services.Configure<SmtpConfiguration>(configuration.GetSection(nameof(SmtpConfiguration)));
        services.Configure<MessageTypes>(configuration.GetSection(nameof(MessageTypes)));
        services.AddTransient<IMessageTypes>(sp =>
                    sp.GetRequiredService<IOptionsMonitor<MessageTypes>>().CurrentValue);

        services.Configure<GraphAcquireTokenOptions>(configuration.GetSection(AppConstants.MicrosoftGraphAcquireTokenSection));
        //services.AddScoped<IGraphServiceClient, GraphServiceClient>();
        services.AddScoped<IApiClient, ApiClient>();
        //services.AddScoped<IGraphApimClient, GraphApimClient>();
        services.AddScoped<IApiClientHandler, ApiClientHandler>();
        services.AddScoped<IResponseHandler, ResponseHandler>();
        services.AddScoped<ISmtpClient, SmtpClient>();
        services.AddScoped<IEmailBodyBuilder, EmailBodyBuilder>();
        services.AddScoped<IEmailMetaBuilder, EmailMetaBuilder>();
        services.AddScoped<IEmailSenderService, EmailSenderService>();
        services.AddScoped<IConnectionErrorNotificationService, ConnectionErrorNotificationService>();

        services.AddFluentEmail("admin@insight.com").AddRazorRenderer();
    })
    .Build();

await host.RunAsync();