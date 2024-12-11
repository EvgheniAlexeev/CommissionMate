using Microsoft.Extensions.Logging;

using Polly;
using Polly.Extensions.Http;

namespace WorkerNode.Extensions
{
    public static class RetryPolicy
    {   public static readonly int RetryCount = 3;
        public static readonly int Pow2 = 2;

        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger(nameof(RetryPolicy));

            return HttpPolicyExtensions
                .HandleTransientHttpError() // Handles 5XX, 408, and network errors
                .WaitAndRetryAsync(RetryCount, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(Pow2, retryAttempt)), (outcome, timespan, retryCount, context) =>
                    {
                        logger.LogInformation($"Retry {retryCount} after {timespan.Seconds} seconds due to {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
                    });
        }
    }
}
