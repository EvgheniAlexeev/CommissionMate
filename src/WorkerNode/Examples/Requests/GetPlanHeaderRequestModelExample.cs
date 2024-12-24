using Domain.Models.Requests;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples.Requests
{
    public class GetPlanHeaderRequestModelExample 
        : OpenApiExample<GetPlanHeaderRequestModel>
    {
        public override IOpenApiExample<GetPlanHeaderRequestModel> Build(NamingStrategy? namingStrategy = null)
        {
            var ex = new GetPlanHeaderRequestModel();
            ex.FullName = $"PLANA_{DateTime.UtcNow.Year}";

            if (!Examples.TryGetValue("GetPlanHeaderModelExample", out _))
            {
                Examples.Add(OpenApiExampleResolver.Resolve(
                    "GetPlanHeaderModelExample",
                    ex,
                    namingStrategy
                ));
            }

            return this;
        }
    }
}
