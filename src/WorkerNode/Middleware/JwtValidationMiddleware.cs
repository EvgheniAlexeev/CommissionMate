namespace WorkerNode.Middleware
{
    using IsolatedFunctionAuth.Middleware;

    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Azure.Functions.Worker.Middleware;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using static System.Net.WebRequestMethods;

    internal sealed class JwtValidationMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly ILogger<JwtValidationMiddleware> _logger;

        public JwtValidationMiddleware(ILogger<JwtValidationMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {

            var requestData = await context.GetHttpRequestDataAsync();

            string? authHeader = null;
            if (requestData!.Headers.TryGetValues("Authorization", out var values))
            {
                authHeader = values.First();
                if (authHeader != null && authHeader.StartsWith("Bearer "))
                {
                    var token = authHeader.Substring("Bearer ".Length).Trim();

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = false,
                        ValidIssuer = "https://sts.windows.net/544f8ac3-ce4c-47d1-9b72-284ac54b8d1c/",
                        ValidAudience = "https://amdaris.com/rewards.system",
                        SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                        {
                            var jwt = new JwtSecurityToken(token);
                            return jwt;
                        },
#if !DEBUG
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("client_secret"))
#endif
                    };

                    try
                    {
                        var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                        //context.Items["User"] = principal;
                        context.Features.Set(new JwtPrincipalFeature(principal, token, "test"));
                    }
                    catch (SecurityTokenException ex)
                    {
                        _logger.LogError("Token validation failed: {Exception}", ex);
                        context.SetHttpResponseStatusCode(HttpStatusCode.Unauthorized);
                    }
                }
            }
            else
            {
                context.SetHttpResponseStatusCode(HttpStatusCode.Unauthorized);
            }

            await next(context);

            context.GetHttpResponseData()?.Headers.Add("Authorization", [authHeader]);
            context.GetHttpResponseData()?.Headers.Add("X-User-Roles", [authHeader]);
        }
    }
}
