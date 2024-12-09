using Microsoft.Extensions.Logging;

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using WorkerNode.Services;

namespace WorkerNode.Handlers
{
    public interface IApiClientHandler
    {
        Task<TModel> ExecuteClientMethod<TClient, TModel>(
            TClient client,
            Func<TClient, Task<TModel>> clientMethod);

        Task ExecuteClientMethod<TClient>(
            TClient client,
            Func<TClient, Task> clientMethod);
    }

    public class ApiClientHandler(ILogger<ApiClientHandler> logger,
        IConnectionErrorNotificationService connectionErrorNotificationService) : IApiClientHandler
    {
        private readonly ILogger<ApiClientHandler> _logger = logger;
        private readonly IConnectionErrorNotificationService _notificationService = connectionErrorNotificationService;

        public async Task<TModel> ExecuteClientMethod<TClient, TModel>(
            TClient client,
            Func<TClient, Task<TModel>> clientMethod)
        {
            TModel response;
            try
            {
                response = await clientMethod(client);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception Message: {Message}", ex.Message);


                await _notificationService
                    .SendConnectionErrorNotification(new(ex, GetMethodName(clientMethod)));

                throw;
            }

            return response;
        }

        public async Task ExecuteClientMethod<TClient>(TClient client, Func<TClient, Task> clientMethod)
        {
            try
            {
                await clientMethod(client);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception Message: {Message}", ex.Message);
                await _notificationService
                    .SendConnectionErrorNotification(new(ex, GetMethodName(clientMethod)));

                throw;
            }
        }

        private string GetMethodName<TClient>(Func<TClient, Task> clientMethod)
        {
            var methodInfo = clientMethod.Method;
            string methodName;

            if (methodInfo.DeclaringType != null && methodInfo.DeclaringType.IsDefined(typeof(CompilerGeneratedAttribute), inherit: true))
            {
                string pattern = @"<([^>]+)>";
                Match match = Regex.Match(methodInfo.Name, pattern);
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }

                methodName = "Anonymous or Lambda Method";
            }
            else
            {
                methodName = methodInfo.Name;
            }

            return methodName;
        }
    }
}
