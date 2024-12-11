using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

using System.Net;

using WorkerNode.Services;

namespace WorkerNode
{
    public class HealthCheck(ILogger<AuthenticateFunc> logger, IApiClient apiClient) : BaseFunc
    {
        private readonly ILogger _logger = logger;
        private readonly IApiClient _apiClient = apiClient;

        [Function(nameof(HealthCheck))]
        [OpenApiOperation(operationId: nameof(HealthCheck), tags: ["Azure Function HealthCheck"])]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/text", bodyType: typeof(string), Description = "The OK response")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            var forecasts = await apiClient.GetWeatherForecast();
            return CreateOkTextResponse(req, $"CommissionMate function is working, version: {version}");
        }
    }
}
