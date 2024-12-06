using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using System.Net;

using WorkerNode.Authorization;

namespace WorkerNode
{
    public class AuthenticateFunc : BaseFunc
    {
        private readonly ILogger<AuthenticateFunc> _logger;

        public AuthenticateFunc(ILogger<AuthenticateFunc> logger)
        {
            _logger = logger;
        }

        [Function(nameof(Authorize))]
        public HttpResponseData Authorize(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, 
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
