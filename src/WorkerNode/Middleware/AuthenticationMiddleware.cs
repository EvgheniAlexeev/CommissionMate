using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

using WorkerNode.Middleware;
using WorkerNode.Services;

namespace IsolatedFunctionAuth.Middleware
{
    /// <summary>
    /// https://github.com/juunas11/IsolatedFunctionsAuthentication.git
    /// </summary>
    public class AuthenticationMiddleware : IFunctionsWorkerMiddleware
    {
        public static readonly string XUserRolesHeader = "X-User-Roles";
        private static readonly ConcurrentBag<string> AllowedAnonymous = new() { "healthcheck", "swagger/ui", "/swagger.json" };
        private readonly JwtSecurityTokenHandler _tokenValidator;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ConfigurationManager<OpenIdConnectConfiguration> _configurationManager;
        private readonly IUserRepository _repository;
        private readonly string _secretSalt;

        public AuthenticationMiddleware(IConfiguration configuration, IUserRepository repository)
        {
            var authority = configuration["AuthenticationAuthority"];
            var audience = configuration["AuthenticationClientId"];
            _secretSalt = configuration["AuthenticationSecretSalt"]!;

            _tokenValidator = new JwtSecurityTokenHandler();
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = audience
            };
            _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"{authority}/.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever());


            _repository = repository;
        }

        public static string GenerateHmacSignature(string payload, string secretKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                return Convert.ToBase64String(hash);
            }
        }

        
        public async Task Invoke(
            FunctionContext context,
            FunctionExecutionDelegate next)
        {
            var request = await context.GetHttpRequestDataAsync();
            var url = request.Url.AbsolutePath.ToLower();
            if (AllowedAnonymous.Any(a => url.Contains(a)))
            {
                await next(context);
                return;
            }

            if (!TryGetTokenFromHeaders(context, out var token))
            {
                // Unable to get token from headers
                context.SetHttpResponseStatusCode(HttpStatusCode.Unauthorized);
                return;
            }

            if (!_tokenValidator.CanReadToken(token))
            {
                // Token is malformed
                context.SetHttpResponseStatusCode(HttpStatusCode.Unauthorized);
                return;
            }

            // Get OpenID Connect metadata
            var validationParameters = _tokenValidationParameters.Clone();
            var openIdConfig = await _configurationManager.GetConfigurationAsync(default);
            validationParameters.ValidIssuer = openIdConfig.Issuer;
            validationParameters.IssuerSigningKeys = openIdConfig.SigningKeys;

            try
            {
                // Validate token
                var principal = _tokenValidator.ValidateToken(token, validationParameters, out _);
                var email = principal.FindFirst(ClaimTypes.Name)!.Value;

                // Set principal + token in Features collection
                // They can be accessed from here later in the call chain
                if (context.BindingContext.BindingData.TryGetValue("Headers", out object? value))
                {
                    if (value != null)
                    {
                        using JsonDocument document = JsonDocument.Parse(value.ToString());
                        if (document.RootElement.TryGetProperty(XUserRolesHeader, out JsonElement xUserRolesElement))
                        {
                            string headerRoles = xUserRolesElement.GetString();
                            context.Features.Set(new JwtPrincipalFeature(principal, token, headerRoles));
                            
                            await next(context);
                            return;
                        }
                    }
                }

                IEnumerable<string>? roles = null;
                string encodedRoles = string.Empty;
                if (url.Contains("api/authorize"))
                {
                    roles = GetUserRoles(email);
                    encodedRoles = GenerateXRolesHeader(roles, email, _secretSalt);
                    context.Features.Set(new JwtPrincipalFeature(principal, token, encodedRoles));
                    context.Features.Set(new UserContextFeature(email, roles));
                }

                await next(context);
                context.GetHttpResponseData()?.Headers.Add(XUserRolesHeader, [encodedRoles]);
            }
            catch (SecurityTokenException)
            {
                // Token is not valid (expired etc.)
                context.SetHttpResponseStatusCode(HttpStatusCode.Unauthorized);
                return;
            }
        }

        private static bool TryGetTokenFromHeaders(FunctionContext context, out string token)
        {
            token = null;
            // HTTP headers are in the binding context as a JSON object
            // The first checks ensure that we have the JSON string
            if (!context.BindingContext.BindingData.TryGetValue("Headers", out var headersObj))
            {
                return false;
            }

            if (headersObj is not string headersStr)
            {
                return false;
            }

            // Deserialize headers from JSON
            var headers = JsonSerializer.Deserialize<Dictionary<string, string>>(headersStr);
            var normalizedKeyHeaders = headers.ToDictionary(h => h.Key.ToLowerInvariant(), h => h.Value);
            if (!normalizedKeyHeaders.TryGetValue("authorization", out var authHeaderValue))
            {
                // No Authorization header present
                return false;
            }

            if (!authHeaderValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                // Scheme is not Bearer
                return false;
            }

            token = authHeaderValue.Substring("Bearer ".Length).Trim();
            return true;
        }

        private IEnumerable<string> GetUserRoles(string email)
        {
            return  _repository.GetUserRoles(email);
        }

        private string GenerateXRolesHeader(IEnumerable<string> userRoles, string email, string secretKey)
        {
            string payload = JsonSerializer.Serialize(new XRole { Roles = userRoles, Email = email, IssuedAt = DateTime.UtcNow });
            string signature = GenerateHmacSignature(payload, secretKey);
            var header =  Convert.ToBase64String(Encoding.UTF8.GetBytes(payload)) + "." + signature;
            return header;
        }

        public class XRole
        {
            public IEnumerable<string> Roles { get; set; }
            public string Email { get; set; }
            public DateTime IssuedAt { get; set; }
        }
    }
}
