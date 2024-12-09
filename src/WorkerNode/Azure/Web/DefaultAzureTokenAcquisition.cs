using Azure.Core;
using Azure.Identity;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;

using System.Security.Claims;

using WorkerNode.Azure.Models;

namespace WorkerNode.Azure.Web
{
    public class DefaultAzureTokenAcquisition : ITokenAcquisition
    {
        private readonly DefaultAzureCredential _defaultAzureCredential;

        public DefaultAzureTokenAcquisition(IOptions<GraphAcquireTokenOptions> options)
        {
            _defaultAzureCredential = new DefaultAzureCredential(
                        new DefaultAzureCredentialOptions
                        {
                            TenantId = options.Value.Tenant,
                            ManagedIdentityClientId = options.Value.ManagedIdentity?.UserAssignedClientId,
                        });
        }

        public async Task<string> GetAccessTokenAsync(
            string[] scopes,
            string tenant = null,
            string user = null,
            TokenAcquisitionOptions options = null)
        {
            var tokenRequestContext = new TokenRequestContext(scopes);
            var token = await _defaultAzureCredential.GetTokenAsync(tokenRequestContext);
            return token.Token;
        }

        public async Task<string> GetAccessTokenForAppAsync(
            string scope,
            string? authenticationScheme,
            string? tenant = null,
            TokenAcquisitionOptions? tokenAcquisitionOptions = null)
        {
            var tokenRequestContext = new TokenRequestContext(scope.Split(" "));
            var token = await _defaultAzureCredential.GetTokenAsync(tokenRequestContext);
            return token.Token;
        }

        public async Task<string> GetAccessTokenForUserAsync(
            IEnumerable<string> scopes,
            string? authenticationScheme,
            string? tenantId = null,
            string? userFlow = null,
            ClaimsPrincipal? user = null,
            TokenAcquisitionOptions? tokenAcquisitionOptions = null)
        {
            var tokenRequestContext = new TokenRequestContext(scopes.ToArray());

            if (tokenAcquisitionOptions != null)
            {
                tokenRequestContext = new TokenRequestContext(scopes.ToArray(), claims: tokenAcquisitionOptions.Claims);
            }

            var token = await _defaultAzureCredential.GetTokenAsync(tokenRequestContext);
            return token.Token;
        }

        public async Task<AuthenticationResult> GetAuthenticationResultForAppAsync(
            string scope,
            string? authenticationScheme,
            string? tenant = null,
            TokenAcquisitionOptions? tokenAcquisitionOptions = null)
        {
            var tokenRequestContext = new TokenRequestContext(scope.Split(" "));
            var token = await _defaultAzureCredential.GetTokenAsync(tokenRequestContext);

            var authenticationResult = new AuthenticationResult(
                token.Token,
                false,
                Guid.NewGuid().ToString(),
                token.ExpiresOn,
                token.ExpiresOn,
                Guid.NewGuid().ToString(),
                null,
                null,
                [.. scope.Split(" ")],
                Guid.NewGuid()
            );

            return authenticationResult;
        }

        public async Task<AuthenticationResult> GetAuthenticationResultForUserAsync(
            IEnumerable<string> scopes,
            string? authenticationScheme,
            string? tenantId = null,
            string? userFlow = null,
            ClaimsPrincipal? user = null,
            TokenAcquisitionOptions? tokenAcquisitionOptions = null)
        {
            var tokenRequestContext = new TokenRequestContext(scopes.ToArray());
            var token = await _defaultAzureCredential.GetTokenAsync(tokenRequestContext);

            var authenticationResult = new AuthenticationResult(
                token.Token,
                false,
                Guid.NewGuid().ToString(),
                DateTimeOffset.UtcNow.AddSeconds(token.ExpiresOn.Subtract(DateTimeOffset.UtcNow).TotalSeconds),
                DateTimeOffset.UtcNow.AddSeconds(token.ExpiresOn.Subtract(DateTimeOffset.UtcNow).TotalSeconds),
                Guid.NewGuid().ToString(),
                null,
                null,
                scopes,
                Guid.NewGuid()
            );

            return authenticationResult;
        }

        public string GetEffectiveAuthenticationScheme(string? authenticationScheme)
        {
            throw new NotImplementedException();
        }

        public void ReplyForbiddenWithWwwAuthenticateHeader(
            IEnumerable<string> scopes,
            MsalUiRequiredException msalServiceException,
            string? authenticationScheme,
            HttpResponse? httpResponse = null)
        {
            throw new NotImplementedException();
        }

        public Task ReplyForbiddenWithWwwAuthenticateHeaderAsync(
            IEnumerable<string> scopes,
            MsalUiRequiredException msalServiceException,
            HttpResponse? httpResponse = null)
        {
            throw new NotImplementedException();
        }
    }
}
