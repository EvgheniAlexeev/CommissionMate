using Domain.Configurations;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

using System.Net;

using WorkerNode.Authorization;
using WorkerNode.Services;

namespace WorkerNode
{
    public class AuthenticateFunc(ILogger<AuthenticateFunc> logger, IApiClient apiClient) : BaseFunc
    {
        private readonly ILogger<AuthenticateFunc> _logger = logger;
        private readonly IApiClient _apiClient = apiClient;

        [Function(nameof(Authorize))]
        [OpenApiOperation(operationId: nameof(Authorize), tags: ["Authorize for users"])]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(AzureAd), Description = "The OK response")]
        public async Task<HttpResponseData> Authorize(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req, 
            FunctionContext executionContext)
        {
            return CreateOkTextResponse(req, "You were successfully athorized!");
        }

        [Authorize(UserRoles = [UserRoles.Admin])]
        [Function(nameof(RunAuthorized))]
        public HttpResponseData RunAuthorized(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, 
            FunctionContext executionContext)
        {
            return CreateOkTextResponse(req, "Can be called with by admin user only!");
        }
    }
}
