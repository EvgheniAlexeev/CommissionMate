using Domain;
using Domain.Models;
using Domain.Models.Responses;

using Microsoft.Identity.Abstractions;

using WorkerNode.Handlers;

namespace WorkerNode.Services
{
    public interface IApiClient
    {
        Task<IEnumerable<CommissionRate>?> GetCommissions(string email);
        Task<IEnumerable<WeatherForecast>?> GetWeatherForecast();

    }

    public class ApiClient(IDownstreamApi api,
            IApiClientHandler apiHandler,
            IResponseHandler responseHandler,
            IConnectionErrorNotificationService connectionErrorNotificationService) : IApiClient
    {
        private readonly IDownstreamApi _api = api;
        private readonly IApiClientHandler _apiHandler = apiHandler;
        private readonly IResponseHandler _responseHandler = responseHandler;
        private readonly IConnectionErrorNotificationService _connectionErrorNotificationService = connectionErrorNotificationService;

        public async Task<IEnumerable<CommissionRate>?> GetCommissions(string email)
        {
            var response = await _apiHandler.ExecuteClientMethod(
                _api,
                async (x) => await x.GetForAppAsync<EmailRequest, ApiResponse<CommissionRate>>(
                    AppConstants.PowerAutomateApiGetCommissions,
                    input: new EmailRequest() { EmailAddress = email }
                ));

            return (await _responseHandler.HandleResponse(response)).Values;
        }

        public async Task<IEnumerable<WeatherForecast>?> GetWeatherForecast()
        {
            var response = await _apiHandler.ExecuteClientMethod(
                _api,
                async (x) => await x.GetForAppAsync<ApiResponse<WeatherForecast>>(
                    AppConstants.PowerAutomateApiGetWeatherForecast
                ));

            return (await _responseHandler.HandleResponse(response)).Values;
        }
    }
}
