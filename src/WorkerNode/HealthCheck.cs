using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace WorkerNode
{
    public class HealthCheck : BaseFunc
    {
        private readonly ILogger _logger;

        public HealthCheck(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HealthCheck>();
        }

        [Function(nameof(HealthCheck))]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            return CreateOkTextResponse(req, $"CommissionMate function is working, version: {version}");
        }
    }
}
