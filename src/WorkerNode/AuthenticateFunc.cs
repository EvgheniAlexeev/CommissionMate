using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

using System.Net;

using WorkerNode.Authorization;
using WorkerNode.Middleware;
using WorkerNode.Services;

namespace WorkerNode
{
    public class AuthenticateFunc(
        ILogger<AuthenticateFunc> logger, 
        IApiClient apiClient) : BaseFunc
    {
        private readonly ILogger<AuthenticateFunc> _logger = logger;
        private readonly IApiClient _apiClient = apiClient;

        [Function(nameof(Authorize))]
        [OpenApiOperation(operationId: nameof(Authorize), tags: ["Users Authorization"])]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK, 
            contentType: "application/json", 
            bodyType: typeof(string), 
            Description = "OK response in case of successful Authorization with 'x-user-roles' header")]
        public async Task<HttpResponseData> Authorize(
            [HttpTrigger(AuthorizationLevel.User, "get", Route = null)] HttpRequestData req, 
            FunctionContext executionContext)
        {
            return CreateJsonResponse(HttpStatusCode.OK, req, "You were successfully athorized!");
        }

        [Authorize(UserRoles = [UserRoles.Sales])]
        [Function(nameof(RunAuthorized))]
        public HttpResponseData RunAuthorized(
            [HttpTrigger(AuthorizationLevel.User, "get", "post", Route = null)] HttpRequestData req, 
            FunctionContext executionContext)
        {
            var userContext = executionContext.Features.Get<UserContextFeature>();
            return CreateOkTextResponse(req, "Can be called with by admin user only!");
        }
    }
}
