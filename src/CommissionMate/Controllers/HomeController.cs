using CommissionMate.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Net.Http;

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
            GraphServiceClient graphServiceClient,
            ITokenAcquisition tokenAcquisition, 
            HttpClient httpClient)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;
            _tokenAcquisition = tokenAcquisition;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { "https://amdaris.com/rewards.system/user_impersonation" });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync("http://localhost:7071/api/Function1");

            var content = await response.Content.ReadAsStringAsync();
            ViewData["GraphApiResult"] = content;

            return View();
        }

        //[AuthorizeForScopes(ScopeKeySection = "MicrosoftGraph:Scopes")]
        //public async Task<IActionResult> Index()
        //{
        //    var user = await _graphServiceClient.Me.Request().GetAsync();

        //    ViewData["GraphApiResult"] = user.DisplayName;
        //    return View();
        //}

        public IActionResult Privacy()
        {
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
