using CommissionMate.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Graph;
using Microsoft.Identity.Web;

using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace CommissionMate.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly GraphServiceClient _graphServiceClient;
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger,
            ITokenAcquisition tokenAcquisition,
            HttpClient httpClient, GraphServiceClient graphServiceClient)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;
            _tokenAcquisition = tokenAcquisition;
            _httpClient = httpClient;
        }

        [AuthorizeForScopes(ScopeKeySection = "MicrosoftGraph:Scopes")]
        public async Task<IActionResult> Index()
        {
            var accessToken = await _tokenAcquisition
                .GetAccessTokenForUserAsync(["https://amdaris.com/rewards.system/user_impersonation"]);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync("http://localhost:7071/api/Authorize");

            var content = await response.Content.ReadAsStringAsync();
            //var result = content == string.Empty ? null :JsonSerializer.Deserialize<OkObjectResult>(content);
            if (response.Headers.TryGetValues("X-User-Roles", out IEnumerable<string> values))
            {
                _httpClient.DefaultRequestHeaders.Add("X-User-Roles", values.First());
                Response.Cookies.Append("X-User-Roles", values.First(), new CookieOptions
                {
                    HttpOnly = true, // Protect cookie from JavaScript intervention
                    Secure = true,   // HTTPS only
                    SameSite = SameSiteMode.Strict, // Protect from CSRF atacs
                    Expires = DateTime.UtcNow.AddDays(7) // Expiration time
                });
            }

            //var response2 = await _httpClient.GetAsync("http://localhost:7071/api/RunAuthorized");

            ViewData["AzFunctionApiResult"] = string.IsNullOrEmpty(content) ? $" {(int)response.StatusCode} {response.ReasonPhrase}" : content;


            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var accessToken = await _tokenAcquisition
                .GetAccessTokenForUserAsync(["https://amdaris.com/rewards.system/user_impersonation"]);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Add("X-User-Roles", Request.Cookies["X-User-Roles"]);


            var response = await _httpClient.GetAsync("http://localhost:7071/api/GetCurrentPlan");

            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
