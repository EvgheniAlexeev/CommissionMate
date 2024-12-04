//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace WorkerNode
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            //_logger = logger;
        }

        [Function(nameof(Function1))]
        public static HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, FunctionContext executionContext)
        {
            var _logger = executionContext.GetLogger<Function1>();
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            //var authHeader = req.Headers["Authorization"].FirstOrDefault();
            //if (authHeader != null && authHeader.StartsWith("Bearer "))
            //{
            //    var token = authHeader.Substring("Bearer ".Length).Trim();

            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var jwtToken = tokenHandler.ReadJwtToken(token);

            //    var claims = jwtToken.Claims.Select(c => new { c.Type, c.Value }).ToList();
            //    // Log or use the claims as needed
            //    _logger.LogInformation("User claims: {Claims}", claims);
            //}


            var response = req.CreateResponse(HttpStatusCode.OK);

            // Set a context item the middleware can retrieve
            executionContext.Items.Add("functionitem", "Hello from function!");

            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("Welcome to .NET 6!!");

            return response;
        }
    }
}
