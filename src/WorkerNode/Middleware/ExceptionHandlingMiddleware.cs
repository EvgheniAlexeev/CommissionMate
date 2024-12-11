using Domain.Exceptions;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

using System.Net;

using WorkerNode.Services;

namespace WorkerNode.Middleware
{
    public class ExceptionHandlingMiddleware(
        ILogger<ExceptionHandlingMiddleware> logger, 
        IConnectionErrorNotificationService notificationService) : IFunctionsWorkerMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;
        private readonly IConnectionErrorNotificationService _notificationService = notificationService;

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                if (ex is KnownException) 
                {
                    return;
                }

                _logger.LogError(ex, "Error processing invocation");
                await _notificationService.SendConnectionErrorNotification(new(ex, context.FunctionDefinition.Name));

                var httpReqData = await context.GetHttpRequestDataAsync();

                if (httpReqData != null)
                {
                    // Create an instance of HttpResponseData with 500 status code.
                    var newHttpResponse = httpReqData.CreateResponse(HttpStatusCode.InternalServerError);
                    // You need to explicitly pass the status code in WriteAsJsonAsync method.
                    // https://github.com/Azure/azure-functions-dotnet-worker/issues/776
                    await newHttpResponse.WriteAsJsonAsync(new 
                    { 
                        ApiStatus = $"Azure Function Invocation failed! ({(int)newHttpResponse.StatusCode}) {newHttpResponse.StatusCode}" 
                    });

                    var invocationResult = context.GetInvocationResult();

                    var httpOutputBindingFromMultipleOutputBindings = GetHttpOutputBindingFromMultipleOutputBinding(context);
                    if (httpOutputBindingFromMultipleOutputBindings is not null)
                    {
                        httpOutputBindingFromMultipleOutputBindings.Value = newHttpResponse;
                    }
                    else
                    {
                        invocationResult.Value = newHttpResponse;
                    }
                }
            }
        }

        private static OutputBindingData<HttpResponseData>? GetHttpOutputBindingFromMultipleOutputBinding(FunctionContext context)
        {
            // The output binding entry name will be "$return" only when the function return type is HttpResponseData
            var httpOutputBinding = context.GetOutputBindings<HttpResponseData>()
                .FirstOrDefault(b => b.BindingType == "http" && b.Name != "$return");

            return httpOutputBinding;
        }
    }
}
