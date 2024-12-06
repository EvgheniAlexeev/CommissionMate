using Microsoft.Azure.Functions.Worker.Http;

using System.Net;

namespace WorkerNode
{
    public class BaseFunc
    {
        public HttpResponseData CreateOkTextResponse(
            HttpRequestData request,
            string text)
        {
            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString(text);
            return response;
        }
    }
}
