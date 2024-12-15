using Domain.Models.JsonConvertor;

using Microsoft.Azure.Functions.Worker.Http;

using System.Collections.Concurrent;
using System.Net;
using System.Text.Json;

namespace WorkerNode
{
    public class BaseFunc
    {
        private static readonly ConcurrentDictionary<Type, JsonSerializerOptions> _cache = new();

        private static JsonSerializerOptions CreateOptions<T>() where T : class
        {
            return new JsonSerializerOptions
            {
                Converters = { new CamelPascalCaseConverter<T>() , new IsoDateOnlyConverter() }
            };
        }

        private static readonly JsonSerializerOptions GetIsoDateOnlyOptionc = new()
        {
            Converters = { new IsoDateOnlyConverter() }
        };


        protected static JsonSerializerOptions GetOptions<T>() where T : class
        {
            return _cache.GetOrAdd(typeof(T), _ => CreateOptions<T>());
        }

        protected T? GetRequestBody<T>(HttpRequestData request) where T : class
        {
            var body = request.ReadAsStringAsync().Result;
            var options = GetOptions<T>();


            if (string.IsNullOrEmpty(body))
            {
                return null;
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
            response.WriteString(JsonSerializer.Serialize(obj, GetIsoDateOnlyOptionc));
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
