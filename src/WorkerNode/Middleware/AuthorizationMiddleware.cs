using FluentEmail.Core;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Configuration;

using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

using WorkerNode.Authorization;
using WorkerNode.Middleware;

using static IsolatedFunctionAuth.Middleware.AuthenticationMiddleware;

namespace IsolatedFunctionAuth.Middleware
{
    /// <summary>
    /// https://github.com/juunas11/IsolatedFunctionsAuthentication.git
    /// </summary>
    public class AuthorizationMiddleware : IFunctionsWorkerMiddleware
    {
        private const string ScopeClaimType = "http://schemas.microsoft.com/identity/claims/scope";
        private readonly string _secretSalt;

        public AuthorizationMiddleware(IConfiguration configuration)
        {
            _secretSalt = configuration["AuthenticationSecretSalt"]!;
        }

        public async Task Invoke(
            FunctionContext context,
            FunctionExecutionDelegate next)
        {
            var principalFeature = context.Features.Get<JwtPrincipalFeature>();
            if (principalFeature != null && !AuthorizePrincipal(context, principalFeature.Principal, principalFeature.EncodedRoles, _secretSalt))
            {
                context.SetHttpResponseStatusCode(HttpStatusCode.Forbidden);
                return;
            }

            await next(context);
        }

        private static bool AuthorizePrincipal(
            FunctionContext context,
            ClaimsPrincipal principal, 
            string encodedRoles,
            string _secretSalt)
        {
            var isValidHeader = VerifySignedHeader(encodedRoles, secretKey: _secretSalt, out string decodedString);
            var roles = JsonSerializer.Deserialize<XRole>(decodedString);

            if (isValidHeader && roles != null)
            {
                var email = principal.FindFirst(ClaimTypes.Name)!.Value;
                context.Features.Set(new UserContextFeature(email, roles.Roles));
                // Check user roles
                return AuthorizeUserPermissions(context, roles);
            }

            return isValidHeader;
        }

        private static bool AuthorizeUserPermissions(FunctionContext context, XRole payload)
        {
            var targetMethod = context.GetTargetFunctionMethod();
            var acceptedAppRoles = GetAcceptedAppRoles(targetMethod);

            return acceptedAppRoles.Count() == 0 || payload.Roles.Any(r => acceptedAppRoles.Contains(r));
        }

        private static List<string> GetAcceptedAppRoles(MethodInfo targetMethod)
        {
            var attributes = GetCustomAttributesOnClassAndMethod<AuthorizeAttribute>(targetMethod);
            // Same as above for scopes and user roles,
            // only allow app roles that are common in
            // class and method level attributes.
            return attributes
                .Skip(1)
                .Select(a => a.AppRoles)
                .Aggregate(attributes.FirstOrDefault()?.UserRoles ?? Enumerable.Empty<string>(), (result, acceptedRoles) =>
                {
                    return result.Intersect(acceptedRoles);
                })
                .ToList();
        }

        private static List<T> GetCustomAttributesOnClassAndMethod<T>(MethodInfo targetMethod)
            where T : Attribute
        {
            var methodAttributes = targetMethod.GetCustomAttributes<T>();
            var classAttributes = targetMethod.DeclaringType.GetCustomAttributes<T>();
            return methodAttributes.Concat(classAttributes).ToList();
        }

        private static bool VerifySignedHeader(string signedHeader, string secretKey, out string decodedString)
        {
            decodedString = string.Empty;
            var parts = signedHeader.Split('.');
            if (parts.Length != 2)
                return false;

            string payload = parts[0];

            byte[] decodedBytes = Convert.FromBase64String(payload);

            decodedString = Encoding.UTF8.GetString(decodedBytes);
            string signature = parts[1];
            string expectedSignature = AuthenticationMiddleware.GenerateHmacSignature(decodedString, secretKey);
            return signature == expectedSignature;
        }
    }
}
