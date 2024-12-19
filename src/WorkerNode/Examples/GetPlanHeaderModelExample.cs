using Domain.Models.Requests;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples
{
    public class GetPlanHeaderModelExample : OpenApiExample<GetPlanHeaderModel>
    {
        public override IOpenApiExample<GetPlanHeaderModel> Build(NamingStrategy namingStrategy = null)
        {
            var ex = new GetPlanHeaderModel();
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
