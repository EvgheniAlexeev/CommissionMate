using System.Security.Claims;

namespace WorkerNode.Middleware
{
    /// <summary>
    /// Holds the authenticated user principal
    /// for the request along with the
    /// access token they used.
    /// </summary>
    public class JwtPrincipalFeature
    {
        public JwtPrincipalFeature(ClaimsPrincipal principal, string accessToken, string encodedRoles)
        {
            Principal = principal;
            AccessToken = accessToken;
            EncodedRoles = encodedRoles;
        }

        public ClaimsPrincipal Principal { get; }

        /// <summary>
        /// The access token that was used for this
        /// request. Can be used to acquire further
        /// access tokens with the on-behalf-of flow.
        /// </summary>
        public string AccessToken { get; }

        public string EncodedRoles { get; }
    }
}
