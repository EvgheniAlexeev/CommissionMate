//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

using static System.Formats.Asn1.AsnWriter;
using WorkerNode.Authorization;
using System.Runtime.InteropServices;

namespace WorkerNode
{
    //[Authorize(
    //    Scopes = new[] { Scopes.FunctionsAccess },
    //    UserRoles = new[] { UserRoles.User, UserRoles.Admin },
    //    AppRoles = new[] { AppRoles.AccessAllFunctions })]
    public class AuthenticateFunc
    {
        private readonly ILogger<AuthenticateFunc> _logger;

        public AuthenticateFunc(ILogger<AuthenticateFunc> logger)
        {
            _logger = logger;
        }

        [Authorize(UserRoles = [UserRoles.User])]
        [Function(nameof(Authorize))]
        public HttpResponseData Authorize([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, FunctionContext executionContext)
        {
            return CreateOkTextResponse(req, "You were successfully athorized!");

        }

        [Authorize(UserRoles = [UserRoles.Admin])]
        [Function(nameof(RunAuthorized))]
        public HttpResponseData RunAuthorized([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, FunctionContext executionContext)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            return CreateOkTextResponse(req, "Can be called with scopes and admin user only");

        }

        private static HttpResponseData CreateOkTextResponse(
            HttpRequestData request,
            string text)
        {
            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString(text);
            return response;
        }
    }
}
