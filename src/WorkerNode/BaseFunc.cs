using Domain.Models.JsonConvertor;

using Microsoft.Azure.Functions.Worker.Http;

using System.Net;
using System.Text.Json;

namespace WorkerNode
{
    public class BaseFunc
    {
        public static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        protected T? GetRequestBody<T>(HttpRequestData request) where T : class
        {
            var body = request.ReadAsStringAsync().Result;

            var options = new JsonSerializerOptions
            {
                Converters = { new CamelPascalCaseConverter<T>() }
            };

            
            if (string.IsNullOrEmpty(body))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(body, options);
        }

        protected HttpResponseData CreateOkTextResponse(
            HttpRequestData request,
            string text)
        {
            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString(text);
            return response;
        }

        protected HttpResponseData CreateJsonResponse(HttpStatusCode statusCode,
            HttpRequestData request,
            object obj)
        {
            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(JsonSerializer.Serialize(obj));
            return response;
        }

        protected HttpResponseData CreateUnauthorizedResponse(HttpRequestData request, string text)
        {
            var response = request.CreateResponse(HttpStatusCode.Unauthorized);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString(text);

            return response;
        }

        protected HttpResponseData CreateBadRequestResponse(HttpRequestData request)
        {
            return request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
