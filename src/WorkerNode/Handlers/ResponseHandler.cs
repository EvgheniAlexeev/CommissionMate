using Domain.Models;

namespace WorkerNode.Handlers
{
    public interface IResponseHandler
    {
        Task<Result<TResponse>> HandleResponse<TResponse>(ApiResponse<TResponse> response);
    }

    public class ResponseHandler : IResponseHandler
    {
        public async Task<Result<TResponse>> HandleResponse<TResponse>(ApiResponse<TResponse> response)
        {
            if (response.Result.Values == null || response.Result.Values.Length == 0)
                return new();

            return await Task.FromResult(response.Result);
        }
    }
}
